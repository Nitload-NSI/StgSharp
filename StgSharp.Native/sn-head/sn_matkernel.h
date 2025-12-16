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

#define MAT_KERNEL(T)                        \
        union {                              \
                __m128 xmm[XMMCOUNT(T) / 4]; \
                __m256 ymm[YMMCOUNT(T) / 4]; \
                __m512 zmm[ZMMCOUNT(T) / 4]; \
                T m[4][4];                   \
        }

#define VEC(T)                               \
        union {                              \
                __m128 xmm[XMMCOUNT(T) / 4]; \
                __m256 ymm[YMMCOUNT(T) / 4]; \
                __m512 zmm[ZMMCOUNT(T) / 4]; \
                T v[4];                      \
        }

/* Provide a named alias for the zero kernel type to ensure consistent linkage in C++. */
typedef MAT_KERNEL(uint64_t) sn_zero_kernel_u64;

/* Helpers (all compile-time): */
#define SN_LANES(T_VEC, T) (sizeof(T_VEC) / sizeof(T))
#define SN_MAX(a, b) (((a)) > ((b)) ? (a) : (b))
#define SN_CEIL_DIV(a, b) (((a) + (b) - 1) / (b))

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
        - AVX2 : MINQ=8
        - AVX-512: MINQ=8 (16 for float/i32 naturally, so this is a no-op there)
*/

/* SSE panel layout: column-major buffer holding 4 logical columns with MINQ=4.
        Columns: each column packs up to four scalars so ALIGN equals sizeof(__m128).
        Alignment: every column boundary lands on a 16-byte multiple for XMM loads. */
#define MAT_PANEL_sse(T) SN_ALIGNAS(16) MAT_PANEL_SQ_MIN_NOALIGN(T, __m128, 4)
/* AVX2 panel layout: column-major buffer holding 8 logical columns with MINQ=8.
        Columns: each column stores eight elements via two 256-bit chunks when T is 32-bit.
        Alignment: column starts are 32-byte aligned so AVX2 loads/stores remain contiguous. */
#define MAT_PANEL_avx(T) SN_ALIGNAS(32) MAT_PANEL_SQ_MIN_NOALIGN(T, __m256, 8)
/* AVX2 panel layout: column-major buffer holding 8 logical columns with MINQ=8.
        Columns: each column stores eight elements via two 256-bit chunks when T is 32-bit.
        Alignment: column starts are 32-byte aligned so AVX2 loads/stores remain contiguous. */
#define MAT_PANEL_avx_fma(T) SN_ALIGNAS(32) MAT_PANEL_SQ_MIN_NOALIGN(T, __m256, 8)
/* AVX-512 panel layout: column-major buffer holding 8 or more columns depending on T.
        Columns: each column expands to ceil(Q/lanes) ZMM chunks; default MINQ=8 keeps doublse-wide tiles.
        Alignment: column starts sit on 64-byte boundaries to match AVX-512 streaming access. */
#define MAT_PANEL_512(T) SN_ALIGNAS(64) MAT_PANEL_SQ_MIN_NOALIGN(T, __m512, 8)

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
<<<<<<< HEAD
<<<<<<< HEAD
DECLARE_BUILD_PANEL(sse, float);
DECLARE_BUILD_PANEL(avx, float);
DECLARE_BUILD_PANEL(512, float);
DECLARE_STORE_PANEL(sse, float);
DECLARE_STORE_PANEL(avx, float);
DECLARE_STORE_PANEL(512, float);
=======
=======
DECLARE_BUILD_PANEL(sse, float);
>>>>>>> stgsharp-dev/giga
DECLARE_BUILD_PANEL(avx, float);
DECLARE_BUILD_PANEL(512, float);
DECLARE_STORE_PANEL(sse, float);
DECLARE_STORE_PANEL(avx, float);
<<<<<<< HEAD
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
=======
DECLARE_STORE_PANEL(512, float);
>>>>>>> stgsharp-dev/giga

#endif /* SN_MATKERNEL */
