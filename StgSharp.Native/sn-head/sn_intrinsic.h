#ifndef SSC_INTRIN
#define SSC_INTRIN

#define SSC_INTRIN_HEAD

#include "StgSharpNative.h"
#include "sn_matkernel.h"
#include <immintrin.h>
#include <smmintrin.h>
#include <stdint.h>

#ifdef _MSC_VER
#include <intrin.h>
#include <isa_availability.h>
#endif

#define LASTTWO(x) (x & 3)
#define LAST(x) (x & 3)
#define get_align_begin(x) ((x + 15) & (~15))

#define SSE_INSERT(source, target, reset3, reset2, reset1, reset0)                      \
        (LASTTWO(source - 1) << 6) | (LASTTWO(target - 1) << 4) | (LAST(reset3) << 3) | \
                (LAST(reset2) << 2) | (LAST(reset1) << 1) | LAST(reset0)
#define SSE_INSERT_SIMPLE(source, target) SSE_INSERT(source, target, 0, 0, 0, 0)
#define M256_PERMUTE(s3, s2, s1, s0) \
        ((LASTTWO(s3) << 6) | (LASTTWO(s2) << 4) | (LASTTWO(s1) << 2) | (LASTTWO(s0)))
#define BIT_CVRT(pack, index, T) *(T *)(pack->data + index)
#define zero_vec _mm_setzero_ps()
#define zero_vec_256 _mm256_setzero_ps()

typedef union uint64_u {
        uint64_t u64;
        uint32_t u32[2];
} uint64_u;

typedef struct mat_kernel mat_kernel;

typedef struct mat_panel mat_panel;

typedef union scalar_pack {
        uint64_t data[8];
} scalar_pack;

// define operation style:

#define PANEL_OP 1
#define BUFFER_OP 2
#define KERNEL_OP 3

#define RIGHT_ANS_PARAM 1
#define LEFT_RIGHT_ANS_PARAM 2
#define RIGHT_ANS_SCALAR_PARAM 3
#define LEFT_RIGHT_ANS_SCALAR_PARAM 4
#define ANS_SCALAR_PARAM 5

typedef struct matrix_op_mode {
        int16_t operation_style; /* PANEL_OP, BUFFER_OP, KERNEL_OP */
        int16_t parameter_style; /* RIGHT_ANS_PARAM, LEFT_RIGHT_ANS_PARAM, e.g. */
} matrix_op_mode;

/* Mirrors StgSharp.Mathematics.Numeric.MatrixParallelTaskPackage<T> (128 bytes). */
#define MAT_TASK(T)                                                                \
        struct {                                                                   \
                MAT_KERNEL(T) * left; /* Offset 0: left matrix kernel base */      \
                MAT_KERNEL(T) * right; /* Offset 8: right matrix kernel base */    \
                MAT_KERNEL(T) * result; /* Offset 16: result matrix kernel base */ \
                int32_t element_size; /* Offset 24: sizeof(T) */                   \
                int32_t reserved_ptr0; /* Offset 28: reserved/extension slot */    \
                int32_t left_prim_offset; /* Offset 32 */                          \
                int32_t left_prim_stride; /* Offset 36 */                          \
                int32_t left_sec_offset; /* Offset 40 */                           \
                int32_t left_sec_stride; /* Offset 44 */                           \
                int32_t right_prim_offset; /* Offset 48 */                         \
                int32_t right_prim_stride; /* Offset 52 */                         \
                int32_t right_sec_offset; /* Offset 56 */                          \
                int32_t right_sec_stride; /* Offset 60 */                          \
                int32_t result_prim_offset; /* Offset 64 */                        \
                int32_t result_prim_stride; /* Offset 68 */                        \
                int32_t result_sec_offset; /* Offset 72 */                         \
                int32_t result_sec_stride; /* Offset 76 */                         \
                scalar_pack *scalar; /* Offset 80 */                               \
                void *compute_handle; /* Offset 88 */                              \
                int32_t prim_count; /* Offset 96 */                                \
                matrix_op_mode compute_mode; /* Offset 100 */                      \
                int32_t sec_count; /* Offset 104 */                                \
                int32_t reserved_int0; /* Offset 108 */                            \
                int32_t prim_tile_offset; /* Offset 112 */                         \
                int32_t sec_tile_offset; /* Offset 116 */                          \
                int32_t prim_cols_in_tile; /* Offset 120 */                        \
                int32_t sec_cols_in_tile; /* Offset 124 */                         \
        }
typedef struct matrix_task matrix_task;

#if defined(__cplusplus)
static_assert(sizeof(matrix_task) == 128, "matrix_parallel_task_package size mismatch");
#elif defined(__STDC_VERSION__)
static_assert(sizeof(MAT_TASK(float)) == 128, "matrix_parallel_task_package size mismatch");
#endif

#pragma region matrix buffer operation function type

typedef void(SN_DECL *BUF_PROC_AS)(mat_kernel *ans, scalar_pack *scalar, int count);
typedef void(SN_DECL *BUF_PROC_RAS)(mat_kernel *right, mat_kernel *ans, scalar_pack *scalar,
                                    int count);
typedef void(SN_DECL *BUF_PROC_LRAS)(mat_kernel *left, mat_kernel *right, mat_kernel *ans,
                                     scalar_pack *scalar, int count);
typedef void(SN_DECL *BUF_PROC_RA)(mat_kernel *right, mat_kernel *ans, int count);
typedef void(SN_DECL *BUF_PROC_LRA)(mat_kernel *left, mat_kernel *right, mat_kernel *ans,
                                    int count);

#define BUF_PROC(T_type, T_arch, T_feature, T_op_name) \
        T_type##_buf_##T_op_name##_##T_arch##T_feature

#define DECLARE_BUF_PROC_ANS_SCALAR(T_type, T_arch, T_feature, T_op_name)     \
        INTERNAL void SN_DECL BUF_PROC(T_type, T_arch, T_feature, T_op_name)( \
                MAT_KERNEL(T_type) *restrict ans, scalar_pack const *scalar, size_t count)
#define DECLARE_BUF_PROC_RIGHT_ANS_SCALAR(T_type, T_arch, T_feature, T_op_name)    \
        INTERNAL void SN_DECL BUF_PROC(T_type, T_arch, T_feature, T_op_name)(      \
                MAT_KERNEL(T_type) const *right, MAT_KERNEL(T_type) *restrict ans, \
                scalar_pack const *scalar, size_t count)
#define DECLARE_BUF_PROC_LEFT_RIGHT_ANS_SCALAR(T_type, T_arch, T_feature, T_op_name) \
        INTERNAL void SN_DECL BUF_PROC(T_type, T_arch, T_feature, T_op_name)(        \
                MAT_KERNEL(T_type) const *left, MAT_KERNEL(T_type) const *right,     \
                MAT_KERNEL(T_type) *restrict ans, scalar_pack const *scalar, size_t count)
#define DECLARE_BUF_PROC_RIGHT_ANS(T_type, T_arch, T_feature, T_op_name)      \
        INTERNAL void SN_DECL BUF_PROC(T_type, T_arch, T_feature, T_op_name)( \
                MAT_KERNEL(T_type) const *right, MAT_KERNEL(T_type) *restrict ans, size_t count)
#define DECLARE_BUF_PROC_LEFT_RIGHT_ANS(T_type, T_arch, T_feature, T_op_name)    \
        INTERNAL void SN_DECL BUF_PROC(T_type, T_arch, T_feature, T_op_name)(    \
                MAT_KERNEL(T_type) const *left, MAT_KERNEL(T_type) const *right, \
                MAT_KERNEL(T_type) *restrict ans, size_t count)

#pragma endregion

#pragma region matrix panel function type

typedef void(SN_DECL *PANEL_PROC)(mat_panel *panel, mat_kernel *kernel, int col_length,
                                  int row_length, int col_index, int row_index);
typedef void(SN_DECL *PNL_PROC_LRA)(mat_panel *left, mat_panel *right, mat_panel *ans);
typedef void(SN_DECL *PNL_PROC_RA)(mat_panel *right, mat_panel *ans);
typedef void(SN_DECL *PNL_PROC_LRAS)(mat_panel *left, mat_panel *right, mat_panel *ans,
                                     scalar_pack *scalar);
typedef void(SN_DECL *PNL_PROC_RAS)(mat_panel *right, mat_panel *ans, scalar_pack *scalar);

#define PNL_PROC(T_type, T_arch, T_feature, T_op_name) \
        T_type##_pnl_##T_op_name##_##T_arch##T_feature
#define DECLARE_PNL_PROC_ANS(T_type, T_arch, T_feature, T_op_name) \
        INTERNAL void SN_DECL PNL_PROC(T_type, T_arch, T_feature,  \
                                       T_op_name)(MAT_PANEL_##T_arch##(T_type) *restrict ans)
#define DECLARE_PNL_PROC_LEFT_RIGHT_ANS(T_type, T_arch, T_feature, T_op_name)                \
        INTERNAL void SN_DECL PNL_PROC(T_type, T_arch, T_feature,                            \
                                       T_op_name)(MAT_PANEL_##T_arch##(T_type) const *left,  \
                                                  MAT_PANEL_##T_arch##(T_type) const *right, \
                                                  MAT_PANEL_##T_arch##(T_type) *restrict ans)
#define DECLARE_PNL_PROC_LEFT_RIGHT_ANS_SCALAR(T_type, T_arch, T_feature, T_op_name) \
        INTERNAL void SN_DECL PNL_PROC(T_type, T_arch, T_op_name)(                   \
                MAT_PANEL_##T_arch##(T_type) const *left,                            \
                MAT_PANEL_##T_arch##(T_type) const *right,                           \
                MAT_PANEL_##T_arch##(T_type) *restrict ans, scalar_pack const *scalar)

#pragma endregion

#pragma region matrix kernel function type

typedef void(SN_DECL *KER_PROC_AS)(mat_kernel *ans, scalar_pack *scalar);
typedef void(SN_DECL *KER_PROC_RA)(mat_kernel *left, mat_kernel *ans);
typedef void(SN_DECL *KER_PROC_LRA)(mat_kernel *left, mat_kernel *right, mat_kernel *ans);
typedef void(SN_DECL *KER_PROC_LAS)(mat_kernel *matrix, mat_kernel *ans, scalar_pack *scalar);

#define KER_PROC(T_type, T_arch, T_feature, T_op_name) \
        T_type##_kernel_##T_op_name##_##T_arch##T_feature

#define DECLARE_KER_PROC_LEFT_RIGHT_ANS_SCALAR(T_type, T_arch, T_feature, T_op_name) \
        INTERNAL void SN_DECL KER_PROC(T_type, T_arch, T_feature, T_op_name)(        \
                MAT_KERNEL(T_type) const *left, MAT_KERNEL(T_type) const *right,     \
                MAT_KERNEL(T_type) *restrict ans, scalar_pack const *scalar)
#define DECLARE_KER_PROC_RIGHT_ANS_SCALAR(T_type, T_arch, T_feature, T_op_name)    \
        INTERNAL void SN_DECL KER_PROC(T_type, T_arch, T_feature, T_op_name)(      \
                MAT_KERNEL(T_type) const *right, MAT_KERNEL(T_type) *restrict ans, \
                scalar_pack const *scalar)
#define DECLARE_KER_PROC_LEFT_RIGHT_ANS(T_type, T_arch, T_feature, T_op_name)    \
        INTERNAL void SN_DECL KER_PROC(T_type, T_arch, T_feature, T_op_name)(    \
                MAT_KERNEL(T_type) const *left, MAT_KERNEL(T_type) const *right, \
                MAT_KERNEL(T_type) *restrict ans)
#define DECLARE_KER_PROC_RIGHT_ANS(T_type, T_arch, T_feature, T_op_name)      \
        INTERNAL void SN_DECL KER_PROC(T_type, T_arch, T_feature, T_op_name)( \
                MAT_KERNEL(T_type) const *right, MAT_KERNEL(T_type) *restrict ans)

#pragma endregion

#pragma region matrix kernel tile function type

typedef void(SN_DECL *KERTILE_PROC_AS)(mat_kernel *ans, scalar_pack *scalar, __m128 *enumeration);
typedef void(SN_DECL *KERTILE_PROC_RA)(mat_kernel *left, mat_kernel *ans, __m128 *enumeration);
typedef void(SN_DECL *KERTILE_PROC_LRA)(mat_kernel *left, mat_kernel *right, mat_kernel *ans,
                                        __m128 *enumeration);
typedef void(SN_DECL *KERTILE_PROC_LAS)(mat_kernel *matrix, mat_kernel *ans, scalar_pack *scalar,
                                        __m128 *enumeration);

#define KERTILE_PROC(T_type, T_arch, T_feature, T_op_name) \
        T_type##_kernel_tile_##T_op_name##_##T_arch##T_feature

#define DECLARE_KERTILE_PROC_LEFT_RIGHT_ANS_SCALAR(T_type, T_arch, T_feature, T_op_name) \
        INTERNAL void SN_DECL KERTILE_PROC(T_type, T_arch, T_feature, T_op_name)(        \
                MAT_KERNEL(T_type) const *left, MAT_KERNEL(T_type) const *right,         \
                MAT_KERNEL(T_type) *restrict ans, scalar_pack const *scalar,             \
                __m128 const *enumeration)
#define DECLARE_KERTILE_PROC_RIGHT_ANS_SCALAR(T_type, T_arch, T_feature, T_op_name) \
        INTERNAL void SN_DECL KERTILE_PROC(T_type, T_arch, T_feature, T_op_name)(   \
                MAT_KERNEL(T_type) const *right, MAT_KERNEL(T_type) *restrict ans,  \
                scalar_pack const *scalar, __m128 const *enumeration)
#define DECLARE_KERTILE_PROC_LEFT_RIGHT_ANS(T_type, T_arch, T_feature, T_op_name) \
        INTERNAL void SN_DECL KERTILE_PROC(T_type, T_arch, T_feature, T_op_name)( \
                MAT_KERNEL(T_type) const *left, MAT_KERNEL(T_type) const *right,  \
                MAT_KERNEL(T_type) *restrict ans, __m128 const *enumeration)
#define DECLARE_KERTILE_PROC_RIGHT_ANS(T_type, T_arch, T_feature, T_op_name)       \
        INTERNAL void SN_DECL KERTILE_PROC(T_type, T_arch, T_feature, T_op_name)(  \
                MAT_KERNEL(T_type) const *right, MAT_KERNEL(T_type) *restrict ans, \
                __m128 const *enumeration)

#pragma endregion

#pragma region special matrix kernel operation

typedef void(SN_DECL *PIVOTPROC)(mat_kernel const *source, size_t col_begin, size_t col_length,
                                 size_t *restrict ans);
INTERNAL void SN_DECL pivot_float(MAT_KERNEL(float) const *source, size_t col_begin,
                                  size_t col_length, size_t *restrict ans);

#pragma endregion

typedef void(SN_DECL *VECTORNORMALIZEPROC)(__m128 *source, __m128 *target);
typedef void(SN_DECL *DOTPROC)(void *transpose, __m128 *vector, __m128 *ans);
typedef uint64_t(SN_DECL *FACTORIALROC)(int n);

#pragma region string

typedef int(SN_DECL *HASH)(byte const *str, int length);
typedef int(SN_DECL *INDEX_PAIR)(short const *stram, uint32_t target, int length);

#pragma endregion

typedef struct mat_intrinsic {
        BUF_PROC_LRA buffer_add;
        BUF_PROC_AS buffer_fill;
        BUF_PROC_RAS buffer_scalar_mul;
        BUF_PROC_LRA buffer_sub;
        KER_PROC_RA buffer_transpose;
        PANEL_PROC build_panel;
        PANEL_PROC clear_panel;
        PNL_PROC_LRA ker_fma;
        KERTILE_PROC_LRA ker_tile_fma;
        PIVOTPROC pivot;
        PANEL_PROC store_panel;

} mat_intrinsic;

// Mirrors C# IntrinsicContext field order so managed/unmanaged layouts stay in sync.
typedef struct sn_intrinsic {
        HASH city_hash_simplify;
        FACTORIALROC factorial_simd;
        INDEX_PAIR index_pair;
        VECTORNORMALIZEPROC vec_f32_normalize_3;
        mat_intrinsic mat[4];
} sn_intrinsic;

typedef enum elment_type { F32 = 0, F64 = 1, I32 = 2, I64 = 3 } element_type;

typedef enum most_advanced_instruction {
#ifdef _MSC_VER
        SSE = __IA_SUPPORT_SSE42,
        AVX256 = __IA_SUPPORT_VECTOR256,
        AVX512 = __IA_SUPPORT_VECTOR512,
        NEON
#else
        SSE,
        AVX256,
        AVX512,
        NEON
#endif
} most_advanced_instruction;

#include "sn_target.h"

#endif
