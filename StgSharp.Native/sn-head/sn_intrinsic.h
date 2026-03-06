#ifndef SSC_INTRIN
#define SSC_INTRIN

#define SSC_INTRIN_HEAD

#include "StgSharpNative.h"
#include "sn_intrinsic.matkernel.h"
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

#define SPAN(T)                \
        struct SPAN_T {        \
                T *pointer;    \
                size_t length; \
        };

typedef union uint64_u {
        uint64_t u64;
        uint32_t u32[2];
} uint64_u;

typedef struct mat_kernel mat_kernel;

typedef struct mat_panel mat_panel;

typedef union scalar_pack {
        uint64_t data[8];
} scalar_pack;

/* Mirrors StgSharp.Mathematics.Numeric.MatrixParallelTaskPackage<T> (128 bytes). */
#define MAT_TASK(T)                                                                             \
        struct {                                                                                \
                MAT_KERNEL(T) * mat_0; /* Offset 0: 1st matrix kernel base */                   \
                MAT_KERNEL(T) * mat_1; /* Offset 8: 2nd matrix kernel base */                   \
                MAT_KERNEL(T) * mat_2; /* Offset 16: 3rd matrix kernel base */                  \
                void *scalar; /* Offset 24: scalar pack */                                      \
                int32_t x; /* Offset 32: row index of the result tile */                        \
                int32_t y; /* Offset 36: column index of the result tile */                     \
                int32_t z; /* Offset 40: column index of the result tile */                     \
                int32_t common_x; /* Offset 44: row index of the result tile */                 \
                int32_t common_y; /* Offset 48: column index of the result tile */              \
                int32_t common_z; /* Offset 52: column index of the result tile */              \
                uint32_t mask; /* Offset 56: mask for task schedualing, not useful in C side */ \
                uint32_t reverse; /* Offset 60: reserved for future use */                      \
                scalar_pack padding; /* Offset 64: padding to make the struct size 128 bytes */ \
        }
typedef struct matrix_task matrix_task;

#if defined(__cplusplus)
static_assert(sizeof(matrix_task) == 128, "matrix_parallel_task_package size mismatch");
#elif defined(__STDC_VERSION__)
static_assert(sizeof(MAT_TASK(float)) == 128, "matrix_parallel_task_package size mismatch");
#endif

typedef void(SN_DECL *UNI_MK_PROC)(matrix_task *task);
typedef void(SN_DECL *UNI_SK_PROC)(mat_kernel const *left, mat_kernel const *right,
                                   mat_kernel *restrict ans, uint64_t scalar);

typedef void(SN_DECL *VECTORNORMALIZEPROC)(__m128 *source, __m128 *target);
typedef void(SN_DECL *DOTPROC)(void *transpose, __m128 *vector, __m128 *ans);
typedef uint64_t(SN_DECL *FACTORIALROC)(int n);

#pragma region string

typedef int(SN_DECL *HASH)(byte const *str, int length);
typedef int(SN_DECL *INDEX_PAIR)(short const *stram, uint32_t target, int length);

#pragma endregion

typedef struct mat_mk_intrinsic {
        UNI_MK_PROC add;
        UNI_MK_PROC fill;
        UNI_MK_PROC scalar_mul;
        UNI_MK_PROC sub;
        UNI_MK_PROC transpose;
        UNI_MK_PROC fma;
        UNI_MK_PROC pivot;
} mat_mk_intrinsic;

typedef struct mat_sk_intrinsic {
        UNI_SK_PROC add;
        UNI_SK_PROC fill;
        UNI_SK_PROC scalar_mul;
        UNI_SK_PROC sub;
        UNI_SK_PROC transpose;
        UNI_SK_PROC fma;
        UNI_SK_PROC pivot;
} mat_sk_intrinsic;

// Mirrors C# IntrinsicContext field order so managed/unmanaged layouts stay in sync.
typedef struct sn_intrinsic {
        HASH city_hash_simplify;
        FACTORIALROC factorial_simd;
        VECTORNORMALIZEPROC vec_f32_normalize_3;
        mat_mk_intrinsic mat_mk[0x0a];
        mat_sk_intrinsic mat_sk[0x0a];
} sn_intrinsic;

typedef enum elment_mask { F32 = 0, F64 = 1, I32 = 2, I64 = 3 } element_mask;

#include "sn_target.h"

#endif
