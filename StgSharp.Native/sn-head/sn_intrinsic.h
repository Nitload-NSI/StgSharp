#ifdef _MSC_VER
#pragma once
#endif

#ifndef SSC_INTRIN
#define SSC_INTRIN

#include "StgSharpNative.h"
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
#define BIT_CVRT(pack, index, T) *(T *)(pack->data + index)
#define zero_vec _mm_setzero_ps()
#define zero_vec_256 _mm256_setzero_ps()

typedef union uint64_u {
        uint64_t u64;
        uint32_t u32[2];
} uint64_u;

typedef union mat_kernel {
        __m128 xmm[4];
        __m256 ymm[2];
        __m512 zmm[1];
        float m[4][4];
} mat_kernel;

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

typedef union scalar_pack {
        uint64_t data[8];
} scalar_pack;

typedef void(SN_DECL *VECTORNORMALIZEPROC)(__m128 *source, __m128 *target);
typedef void(SN_DECL *DOTPROC)(void *transpose, __m128 *vector, __m128 *ans);
typedef void(SN_DECL *MATRIXTRANSPOSEPROC)(void *source, void *target);
typedef void(SN_DECL *MATRIXDETPROC)(void *mat, void *transpose);
typedef void(SN_DECL *MATARITHMETIC)(void *left, void *right, void *ans);
typedef void(SN_DECL *SCALERMATARITHMETIC)(void *left, void *right, void *ans);
typedef void(SN_DECL *MATFMAPROC)(mat_kernel *left, mat_kernel *right, mat_kernel *ans);
typedef void(SN_DECL *SCALARMATARITHMETIC)(mat_kernel *matrix, float scalar, mat_kernel *ans);
typedef void(SN_DECL *MATTRANSPOSEPROC)(mat_kernel *source, mat_kernel *target);
typedef uint64_t(SN_DECL *FACTORIALROC)(int n);

#pragma region matix function

INTERNAL void normalize(__m128 *source, __m128 *target);

INTERNAL void SN_DECL kernel_add_sse(mat_kernel const *left, mat_kernel const *right,
                                     mat_kernel *ans);
INTERNAL void SN_DECL kernel_add_avx(mat_kernel const *left, mat_kernel const *right,
                                     mat_kernel *ans);
INTERNAL void SN_DECL kernel_add_512(mat_kernel const *left, mat_kernel const *right,
                                     mat_kernel *ans);
INTERNAL void SN_DECL kernel_sub_sse(mat_kernel const *left, mat_kernel const *right,
                                     mat_kernel *ans);
INTERNAL void SN_DECL kernel_sub_avx(mat_kernel const *left, mat_kernel const *right,
                                     mat_kernel *ans);
INTERNAL void SN_DECL kernel_sub_512(mat_kernel const *left, mat_kernel const *right,
                                     mat_kernel *ans);
INTERNAL void SN_DECL kernel_transpose_sse(mat_kernel const *source, mat_kernel *target);
INTERNAL void SN_DECL kernel_transpose_avx(mat_kernel const *source, mat_kernel *target);
INTERNAL void SN_DECL kernel_transpose_512(mat_kernel const *source, mat_kernel *target);
INTERNAL void SN_DECL kernel_scalar_mul_sse(mat_kernel const *matrix, float const scalar,
                                            mat_kernel *ans);
INTERNAL void SN_DECL kernel_scalar_mul_avx(mat_kernel const *matrix, float const scalar,
                                            mat_kernel *ans);
INTERNAL void SN_DECL kernel_scalar_mul_512(mat_kernel const *matrix, float const scalar,
                                            mat_kernel *ans);

#pragma endregion

#pragma region fma

INTERNAL void SN_DECL kernel_fma_sse(mat_kernel const *restrict left,
                                     mat_kernel const *restrict right, mat_kernel *restrict ans);

#pragma endregion

#pragma region scaler_simd
INTERNAL uint64_t SN_DECL factorial_simd_sse(int n);
#pragma endregion

#pragma region string

INTERNAL int SN_DECL city_hash_simplify_sse(byte const *str, int length);
INTERNAL int SN_DECL index_pair_sse(short const *str, uint32_t target, int length);

#pragma endregion

typedef struct sn_intrinsic {
        int (*city_hash_simplify)(byte *str, int length);
        FACTORIALROC factorial_simd;
        int (*index_pair)(short const *str, uint32_t target, int length);
        MATARITHMETIC kernel_add;
        void (*kernel_fma)(void *left, void *right, void *ans);
        SCALARMATARITHMETIC kernel_scalar_mul;
        MATARITHMETIC kernel_sub;
        MATTRANSPOSEPROC kernel_transpose;
        VECTORNORMALIZEPROC normalize_3;
} sn_intrinsic;

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
