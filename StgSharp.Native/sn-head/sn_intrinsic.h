#ifdef _MSC_VER
#pragma once
#endif

#ifndef SSC_INTRIN
#define SSC_INTRIN

#define SSC_INTRIN_HEAD

#include "StgSharpNative.h"
#include "sn_matkernel.h"
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

INTERNAL void f32_normalize(VEC(float) * source, VEC(float) * target);

INTERNAL void SN_DECL f32_add_sse(MAT_KERNEL(float) const *left, MAT_KERNEL(float) const *right,
                                  MAT_KERNEL(float) *restrict ans);
INTERNAL void SN_DECL f32_add_avx(MAT_KERNEL(float) const *left, MAT_KERNEL(float) const *right,
                                  MAT_KERNEL(float) *restrict ans);
INTERNAL void SN_DECL f32_add_512(MAT_KERNEL(float) const *left, MAT_KERNEL(float) const *right,
                                  MAT_KERNEL(float) *restrict ans);
INTERNAL void SN_DECL f32_sub_sse(MAT_KERNEL(float) const *left, MAT_KERNEL(float) const *right,
                                  MAT_KERNEL(float) *restrict ans);
INTERNAL void SN_DECL f32_sub_avx(MAT_KERNEL(float) const *left, MAT_KERNEL(float) const *right,
                                  MAT_KERNEL(float) *restrict ans);
INTERNAL void SN_DECL f32_sub_512(MAT_KERNEL(float) const *left, MAT_KERNEL(float) const *right,
                                  MAT_KERNEL(float) *restrict ans);
INTERNAL void SN_DECL f32_transpose_sse(MAT_KERNEL(float) const *source,
                                        MAT_KERNEL(float) *restrict target);
INTERNAL void SN_DECL f32_transpose_avx(MAT_KERNEL(float) const *source,
                                        MAT_KERNEL(float) *restrict target);
INTERNAL void SN_DECL f32_transpose_512(MAT_KERNEL(float) const *source,
                                        MAT_KERNEL(float) *restrict target);
INTERNAL void SN_DECL f32_scalar_mul_sse(MAT_KERNEL(float) const *matrix, float const scalar,
                                         MAT_KERNEL(float) *restrict ans);
INTERNAL void SN_DECL f32_scalar_mul_avx(MAT_KERNEL(float) const *matrix, float const scalar,
                                         MAT_KERNEL(float) *restrict ans);
INTERNAL void SN_DECL f32_scalar_mul_512(MAT_KERNEL(float) const *matrix, float const scalar,
                                         MAT_KERNEL(float) *restrict ans);
#pragma endregion

#pragma region fma

INTERNAL void SN_DECL f32_fma_sse(MAT_KERNEL(float) const *restrict left,
                                  MAT_KERNEL(float) const *restrict right,
                                  MAT_KERNEL(float) *restrict ans);

#pragma endregion

#pragma region scaler_simd
INTERNAL uint64_t SN_DECL factorial_simd_sse(int n);
#pragma endregion

#pragma region dot_product

INTERNAL void FORCEINLINE SN_DECL dot_41_sse(MAT_KERNEL(float) const *transpose,
                                             __m128 const *vector, __m128 *ans);
INTERNAL void SN_DECL dot_41_avx(MAT_KERNEL(float) const *transpose, __m128 const *vector,
                                 __m128 *ans);
INTERNAL void SN_DECL dot_42_sse(MAT_KERNEL(float) const *transpose, __m128 const *vector,
                                 __m128 *ans);
INTERNAL void SN_DECL dot_42_avx(MAT_KERNEL(float) const *transpose, __m128 const *vector,
                                 __m128 *ans);

#pragma endregion

#pragma region string

INTERNAL int SN_DECL city_hash_simplify_sse(byte const *str, int length);
INTERNAL int SN_DECL index_pair_sse(short const *str, uint32_t target, int length);

#pragma endregion

typedef struct sn_intrinsic {
        int (*city_hash_simplify)(byte *str, int length);
        FACTORIALROC factorial_simd;
        int (*index_pair)(short const *str, uint32_t target, int length);
        MATARITHMETIC f32_add;
        void (*f32_fma)(void *left, void *right, void *ans);
        SCALARMATARITHMETIC f32_scalar_mul;
        MATARITHMETIC f32_sub;
        MATTRANSPOSEPROC f32_transpose;
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
