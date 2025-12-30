#ifndef SN_MATKERNEL
#define SN_MATKERNEL

#include <immintrin.h>
#include <stdint.h>

#include "sn_internal.h"

/* Kernel storage unions for a fixed 4x4 block. */
#define ELEMENTSIZE(T) sizeof(T)
#define XMMCOUNT(T) (sizeof(T))
#define YMMCOUNT(T) (sizeof(T) / 2)
#define ZMMCOUNT(T) (sizeof(T) / 4)

#define MAT_KERNEL(T)                       \
        union /* MAT_KERNEL_##T## /**/ {    \
                __m128 f32_x[XMMCOUNT(T)];  \
                __m256 f32_y[YMMCOUNT(T)];  \
                __m512 f32_z[ZMMCOUNT(T)];  \
                                            \
                __m128d f64_x[XMMCOUNT(T)]; \
                __m256d f64_y[YMMCOUNT(T)]; \
                __m512d f64_z[ZMMCOUNT(T)]; \
                T m[4][4];                  \
        }

#define VEC(T)                                  \
        union {                                 \
                __m128 f32_x[XMMCOUNT(T) / 4];  \
                __m256 f32_y[YMMCOUNT(T) / 4];  \
                __m512 f32_z[ZMMCOUNT(T) / 4];  \
                                                \
                __m128d f64_x[XMMCOUNT(T) / 4]; \
                __m256d f64_y[YMMCOUNT(T) / 4]; \
                __m512d f64_z[ZMMCOUNT(T) / 4]; \
                T v[4];                         \
        }

/* Provide a named alias for the zero kernel type to ensure consistent linkage in C++. */
typedef MAT_KERNEL(uint64_t) sn_zero_kernel_u64;

/* Helpers (all compile-time): */
#define SN_LANES(T_VEC, T) (sizeof(T_VEC) / sizeof(T))
#define SN_MAX(a, b) (((a)) > ((b)) ? (a) : (b))
#define SN_CEIL_DIV(a, b) (((a) + (b) - 1) / (b))
#define SN_BITS(x) ((int)(sizeof(x) * 8))
#define PANEL_MINQ(T_VEC, T) SN_MAX(4, SN_MAX(SN_BITS(T_VEC) / 64, SN_BITS(T_VEC) / SN_BITS(T)))

/* Panel geometry helpers (all compile-time): */
#define PANEL_Q(T, T_VEC, MINQ) SN_MAX(SN_LANES(T_VEC, T), (MINQ))
#define PANEL_CHUNKS(T, T_VEC, MINQ) SN_CEIL_DIV(PANEL_Q(T, T_VEC, MINQ), SN_LANES(T_VEC, T))

/*
        Column-major square panel with minimum side Q:
        - Let R = lanes per vector = sizeof(T_VEC)/sizeof(T).
        - Side length Q = max(R, MINQ) to enforce the requested minimum extent.
        - Each column is stored contiguously via CH = ceil(Q/R) vector chunks.
        - Memory layout:
                        vec[col][chunk] is a T_VEC holding up to R scalars of column 'col'
                        spanning rows chunk*R .. chunk*R + (R - 1).
                        flat[] mirrors the same column-major order (column stride = Q).
        - SIMD view remains integer-typed; cast inside micro-kernels to the
                appropriate floating-point/integer vector type on demand.
*/
#define MAT_PANEL_SQ_MIN_NOALIGN(T, T_VEC, MINQ)                                  \
        union {                                                                   \
                T_VEC vec[PANEL_Q(T, T_VEC, MINQ)][PANEL_CHUNKS(T, T_VEC, MINQ)]; \
                T flat[PANEL_Q(T, T_VEC, MINQ) * PANEL_Q(T, T_VEC, MINQ)];        \
        }

/* ISA presets with minimum square side:
        - SSE  : MINQ=4   (avoid 2x2 for double)
        - AVX2 : MINQ computed as max(4, vector_bits/64, vector_bits/element_bits)
        - AVX-512: same rule (float/i32 naturally expand to 16)
*/

/* SSE panel layout: column-major buffer holding 4 logical columns with MINQ=4.
        Columns: each column packs up to four scalars so ALIGN equals sizeof(__m128).
        Alignment: every column boundary lands on a 16-byte multiple for XMM loads. */
#define MAT_PANEL_sse(T) SN_ALIGNAS(16) MAT_PANEL_SQ_MIN_NOALIGN(T, __m128, PANEL_MINQ(__m128, T))
/* AVX2 panel layout: column-major buffer holding 8 logical columns with MINQ per PANEL_MINQ.
        Columns: each column stores up to lanes elements via contiguous chunks.
        Alignment: column starts are 32-byte aligned so AVX2 loads/stores remain contiguous. */
#define MAT_PANEL_avx(T) SN_ALIGNAS(32) MAT_PANEL_SQ_MIN_NOALIGN(T, __m256, PANEL_MINQ(__m256, T))
/* AVX2/FMA panel layout shares the same geometry as AVX2. */
#define MAT_PANEL_avx_fma(T) \
        SN_ALIGNAS(32) MAT_PANEL_SQ_MIN_NOALIGN(T, __m256, PANEL_MINQ(__m256, T))
/* AVX-512 panel layout: column-major buffer holding 8 or more columns depending on T.
        Columns: each column expands to ceil(Q/lanes) ZMM chunks.
        Alignment: column starts sit on 64-byte boundaries to match AVX-512 streaming access. */
#define MAT_PANEL_512(T) SN_ALIGNAS(64) MAT_PANEL_SQ_MIN_NOALIGN(T, __m512, PANEL_MINQ(__m512, T))

/* Global zero kernel declaration (defined in dllmain.c), aligned to 64 bytes */
#ifdef __cplusplus
extern "C" {
#endif
extern __declspec(align(64)) sn_zero_kernel_u64 SN_ZERO_KERNEL;
#ifdef __cplusplus
}
#endif

/* Access helpers (all compile-time friendly):
   Given Q = PANEL_Q(...), CH = PANEL_CHUNKS(...), R = lanes per vector:
   - Column c occupies vec[c][0..CH-1].
   - Row r inside column c is located at vec[c][r / R] with lane (r % R).
*/
#define PANEL_COL_VEC(panel_obj, c, chunk) ((panel_obj).vec[(int)(c)][(int)(chunk)])
/* Scalar flat index helper for column-major addressing */
#define PANEL_ELEM(panel_obj, T, T_VEC, MINQ, r, c) \
        ((panel_obj).flat[(int)(c) * PANEL_Q(T, T_VEC, MINQ) + (int)(r)])

/* API macros */
#define BUILD_PANEL(arch, element) build_panel_##arch##_##element
#define STORE_PANEL(arch, element) store_panel_##arch##_##element

#define DECLARE_BUILD_PANEL(arch, element)                                               \
        INTERNAL void SN_DECL BUILD_PANEL(arch, element)(                                \
                MAT_PANEL_##arch(element) *restrict panel,                               \
                MAT_KERNEL(element) const *const matrix, int col_length, int row_length, \
                int col_index, int row_index)
#define DECLARE_STORE_PANEL(arch, element)                                                     \
        INTERNAL void SN_DECL STORE_PANEL(arch, element)(MAT_PANEL_##arch(element)             \
                                                                 const *const panel,           \
                                                         MAT_KERNEL(element) *restrict matrix, \
                                                         int col_length, int row_length,       \
                                                         int col_index, int row_index)

/* Declarations for AVX float variant */
DECLARE_BUILD_PANEL(avx, float);
DECLARE_BUILD_PANEL(512, float);

DECLARE_STORE_PANEL(avx, float);
DECLARE_STORE_PANEL(512, float);
DECLARE_STORE_PANEL(512, float);

DECLARE_BUILD_PANEL(512, double);

DECLARE_STORE_PANEL(512, double);

#endif /* SN_MATKERNEL */
