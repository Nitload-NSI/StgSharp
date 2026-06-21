#ifndef SSC_INTRIN
#define SSC_INTRIN

#define SSC_INTRIN_HEAD

#include "StgSharpNative.h"
#include "sn_target.h"
#include <xmmintrin.h>
#include <immintrin.h>
#include <smmintrin.h>
#include "sn_intrinsic.matkernel.h"
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

#if SN_IS_ARCH(x86_64)

#define vec_128 __m128
#define vec_256 __m256
#define vec_512 __m512

#define zero_vec _mm_setzero_ps()
#define zero_vec_256 _mm256_setzero_ps()

#define ALIGN(simdVec) _mm_loadu_ps(&simdVec)
#define ALIGN256(simdVec) _mm256_loadu_ps(&simdVec)
#define ALIGN512(simdVec) _mm512_loadu_ps(&simdVec)

#else

#define zero_vec
#define zero_vec_256

#define ALIGN(simdVec)
#define ALIGN256(simdVec)
#define ALIGN512(simdVec)

#endif

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

typedef union M128{
        #if SN_IS_ARCH(x86_64)
        __m128 m128;
        #endif
        float f32[4];
        double f64[2];
        int32_t i32[4];
        int64_t i64[2];
}M128;

typedef union M256{
        #if SN_IS_ARCH(x86_64)
        __m256 m256;
        #endif
        float f32[8];
        double f64[4];
        int32_t i32[8];
        int64_t i64[4];
}M256;

typedef union M512{
        #if SN_IS_ARCH(x86_64)
        __m512 m512;
        #endif
        uint64_t u64[8];
        uint32_t u32[16];
        float f32[16];
        double f64[8];
        int32_t i32[16];
        int64_t i64[8];
}M512;

typedef struct sn_L4_cache_head {
        uint64_t reverse_mask_0;
        uint32_t position_mask;
        uint32_t size;
        uint64_t reverse_mask_1;
        uint64_t reverse_mask_2;
} sn_L4_cache_head;

typedef struct sn_L4_handle {
        void *ptr;
        sn_L4_cache_head* head;
} sn_L4_handle;

typedef struct matrix_task {
        sn_L4_handle buffer_0;
        sn_L4_handle buffer_1;
        sn_L4_handle buffer_2;
        sn_L4_handle buffer_3;
        union {
                struct {
                        int32_t reverse_0;
                        int32_t reverse_1;
                        int32_t offset_0;
                        int32_t count_0;
                        int32_t offset_1;
                        int32_t count_1;
                        int32_t offset_2;
                        int32_t count_2;
                        int32_t offset_3;
                        int32_t count_3;
                        int32_t reverse_2;
                        int32_t reverse_3;
                        M128 scalar_pack;
                };
                M512 profile;
        };

} matrix_task;

#define MAT_TASK(T) matrix_task

#if defined(__cplusplus)
static_assert(sizeof(matrix_task) == 128, "matrix_parallel_task_package size mismatch");
#elif defined(__STDC_VERSION__)
static_assert(sizeof(matrix_task) == 128, "matrix_parallel_task_package size mismatch");
#endif

typedef void(SN_DECL *UNI_MK_PROC)(matrix_task *task);
typedef void(SN_DECL *UNI_SK_PROC)(mat_kernel const *left, mat_kernel const *right,
                                   mat_kernel *restrict ans, uint64_t scalar);

typedef void(SN_DECL *VECTORNORMALIZEPROC)(VEC_SEGMENT(float) *source, VEC_SEGMENT(float) *target);
typedef void(SN_DECL *DOTPROC)(void *transpose, __m128 *vector, __m128 *ans);
typedef uint64_t(SN_DECL *FACTORIALROC)(int n);

#pragma region string

typedef int(SN_DECL *HASH)(byte const *str, int length);
typedef int(SN_DECL *INDEX_PAIR)(short const *stram, uint32_t target, int length);

#pragma endregion

typedef struct mat_mk_intrinsic {
        UNI_MK_PROC proc_array[10];
} mat_mk_intrinsic;

typedef struct mat_sk_intrinsic {
        UNI_SK_PROC proc_array[10];
} mat_sk_intrinsic;

// Mirrors C# IntrinsicContext field order so managed/unmanaged layouts stay in sync.
typedef struct sn_intrinsic {
        HASH city_hash_simplify;
        FACTORIALROC factorial_simd;
        VECTORNORMALIZEPROC vec_f32_normalize_3;
        mat_mk_intrinsic mat_mk[0x0a];
        mat_sk_intrinsic mat_sk[0x0a];
} sn_intrinsic;


typedef enum mat_proc_index {
        Add = 0,
        Fill = 1,
        ScalarMul = 2,
        Sub = 3,
        Transpose = 4,
        ClearPanel = 5,
        Fma = 6,
        PackFmaLeft = 7,
        PackFmaRight = 8,
        Pivot = 9,
} mat_proc_index;
typedef enum elment_mask { F32 = 0, F64 = 1, I32 = 2, I64 = 3 } element_mask;

#endif
