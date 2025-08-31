#ifdef _MSC_VER
#pragma once
#endif

#ifndef SSC_INTRIN
#define SSC_INTRIN

#include "StgSharpC.h"
#include <immintrin.h>
#include <smmintrin.h>

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
#define M256_PERMUTE(s3, s2, s1, s0) (LASTTWO(s3) << 6) | (LASTTWO(s2) << 4) | (LASTTWO(s1) << 2) | (LASTTWO(s0)

#define zero_vec _mm_setzero_ps()
#define zero_vec_256 _mm256_setzero_ps()

typedef union uint64_u {
        uint64_t u64;
        uint32_t u32[2];
} uint64_u;

typedef union mat_kernel{
        __m128 xmm[4];
        __m256 ymm[2];
        __m512 zmm[1];
        float m[4][4];
}mat_kernel;

typedef union column_2 {
        __m256 stream;
        __m128 column[2];
        float m[4][2];
} __2_columnset;

typedef union column_3 {
        __m256 part;
        __m128 column[3];
        float m[4][3];
} __3_columnset;

typedef union column_4 {
        __m512 stream;
        __m256 half[2];
        __m128 column[4];
        float m[4][4];
} __4_columnset;

typedef void(SSCDECL *VECTORNORMALIZEPROC)(__m128 *source, __m128 *target);
typedef void(SSCDECL *DOTPROC)(void *transpose, __m128 *vector, __m128 *ans);
typedef void(SSCDECL *MATRIXTRANSPOSEPROC)(void *source, void *target);
typedef void(SSCDECL *MATRIXDETPROC)(void *mat, void *transpose);
typedef void(SSCDECL *MATARITHMETIC)(void *left, void *right, void *ans);
typedef void(SSCDECL *SCALERMATARITHMETIC)(void *left, void *right, void *ans);
typedef uint64_t(SSCDECL *FACTORIALROC)(int n);


#pragma region matix function

INTERNAL void normalize(__m128 *source, __m128 *target);

INTERNAL void SSCDECL addmatrix2_sse(__2_columnset *left, __2_columnset *right, __2_columnset *ans);
INTERNAL void SSCDECL addmatrix2_avx(__2_columnset *left, __2_columnset *right, __2_columnset *ans);
INTERNAL void SSCDECL submatrix2_sse(__2_columnset *left, __2_columnset *right, __2_columnset *ans);
INTERNAL void SSCDECL addmatrix2_avx(__2_columnset *left, __2_columnset *right, __2_columnset *ans);
INTERNAL void SSCDECL transpose23(__2_columnset const *source, __3_columnset *target);
INTERNAL void SSCDECL transpose24(__2_columnset const *source, __4_columnset *target);

INTERNAL void SSCDECL addmatrix3_sse(__3_columnset *left, __3_columnset *right, __3_columnset *ans);
INTERNAL void SSCDECL addmatrix3_avx(__3_columnset *left, __3_columnset *right, __3_columnset *ans);
INTERNAL void SSCDECL addmatrix3_512(__3_columnset *left, __3_columnset *right, __3_columnset *ans);
INTERNAL void SSCDECL submatrix3_sse(__3_columnset *left, __3_columnset *right, __3_columnset *ans);
INTERNAL void SSCDECL submatrix3_avx(__3_columnset *left, __3_columnset *right, __3_columnset *ans);
INTERNAL void SSCDECL submatrix3_512(__3_columnset *left, __3_columnset *right, __3_columnset *ans);
INTERNAL void SSCDECL transpose32(__3_columnset const *source, __2_columnset *target);
INTERNAL void SSCDECL transpose33(__3_columnset const *source, __3_columnset *target);
INTERNAL void SSCDECL transpose34(__3_columnset const *source, __4_columnset *target);
INTERNAL float SSCDECL det_mat3(__3_columnset *matrix, __3_columnset *tranpose);

INTERNAL void SSCDECL addmatrix4_sse(__4_columnset *left, __4_columnset *right, __4_columnset *ans);
INTERNAL void SSCDECL addmatrix4_avx(__4_columnset *left, __4_columnset *right, __4_columnset *ans);
INTERNAL void SSCDECL addmatrix4_512(__4_columnset *left, __4_columnset *right, __4_columnset *ans);
INTERNAL void SSCDECL submatrix4_sse(__4_columnset *left, __4_columnset *right, __4_columnset *ans);
INTERNAL void SSCDECL submatrix4_avx(__4_columnset *left, __4_columnset *right, __4_columnset *ans);
INTERNAL void SSCDECL submatrix4_512(__4_columnset *left, __4_columnset *right, __4_columnset *ans);
INTERNAL void SSCDECL transpose42(__4_columnset const *source, __2_columnset *target);
INTERNAL void SSCDECL transpose43(__4_columnset const *source, __3_columnset *target);
INTERNAL void SSCDECL transpose44(__4_columnset const *source, __4_columnset *target);
INTERNAL float SSCDECL det_mat4(__4_columnset *matrix, __4_columnset *transpose);

#pragma endregion

#pragma region dot

INTERNAL void FORCEINLINE SSCDECL dot_41_sse(__4_columnset *transpose, __m128 *vector, __m128 *ans);
INTERNAL void FORCEINLINE SSCDECL dot_31_sse(__3_columnset *transpose, __m128 *vector, __m128 *ans);
INTERNAL void FORCEINLINE SSCDECL dot_41_avx(__4_columnset *transpose, __m128 *vector, __m128 *ans);
INTERNAL void FORCEINLINE SSCDECL dot_31_avx(__3_columnset *transpose, __m128 *vector, __m128 *ans);

INTERNAL void SSCDECL dot_32_sse(__3_columnset *transpose, __m128 *vector, __m128 *ans);
INTERNAL void SSCDECL dot_32_avx(__3_columnset *transpose, __m128 *vector, __m128 *ans);
INTERNAL void SSCDECL dot_42_sse(__4_columnset *transpose, __m128 *vector, __m128 *ans);
INTERNAL void SSCDECL dot_42_avx(__4_columnset *transpose, __m128 *vector, __m128 *ans);

INTERNAL void SSCDECL dot_43_sse(__4_columnset *transpose, __m128 *vector, __m128 *ans);
INTERNAL void SSCDECL dot_43_avx(__4_columnset *transpose, __m128 *vector, __m128 *ans);
//INTERNAL void SSCDECL dot_43_512(__4_columnset *transpose, __m128 *vector, __m128 *ans);

#pragma endregion

#pragma region scaler_simd
INTERNAL uint64_t SSCDECL factorial_simd_sse(int n);
#pragma endregion

#pragma region string

INTERNAL int SSCDECL city_hash_simplify_sse(byte const *str, int length);
INTERNAL int SSCDECL index_pair_sse(short const *str, uint32_t target, int length);

#pragma endregion

typedef struct ssc_intrinsic {
        MATARITHMETIC add_m2;
        MATARITHMETIC add_m3;
        MATARITHMETIC add_m4;
        int (*city_hash_simplify)(byte *str, int length);
        MATRIXDETPROC det_matrix33;
        MATRIXDETPROC det_matrix44;
        DOTPROC dot_31;
        DOTPROC dot_32;
        DOTPROC dot_41;
        DOTPROC dot_42;
        DOTPROC dot_43;
        FACTORIALROC factorial_simd;
        int (*index_pair)(short const *str, uint32_t target, int length);
        VECTORNORMALIZEPROC normalize_3;
        MATARITHMETIC sub_m2;
        MATARITHMETIC sub_m3;
        MATARITHMETIC sub_m4;
        MATRIXTRANSPOSEPROC transpose_23;
        MATRIXTRANSPOSEPROC transpose_24;
        MATRIXTRANSPOSEPROC transpose_32;
        MATRIXTRANSPOSEPROC transpose_33;
        MATRIXTRANSPOSEPROC transpose_34;
        MATRIXTRANSPOSEPROC transpose_42;
        MATRIXTRANSPOSEPROC transpose_43;
        MATRIXTRANSPOSEPROC transpose_44;
} ssc_intrinsic;

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

#endif // !SSC_INTRIN
