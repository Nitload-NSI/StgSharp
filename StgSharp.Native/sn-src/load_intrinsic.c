#include "StgSharpNative.h"
#include "sn_intrinsic.h"

#if defined(_MSC_VER) && !defined(__clang__)
#include <intrin.h>
#include <isa_availability.h>
#else
#include <cpuid.h>
#endif //  _MSC_VER

PRIVATE void load_intrinsic_sse(sn_intrinsic *context);
PRIVATE void load_intrinsic_avx(sn_intrinsic *context);
PRIVATE void load_intrinsic_512(sn_intrinsic *context);

PRIVATE int check_isa(most_advanced_instruction instruction_family)
{
        if (instruction_family != NEON) {
#if defined(_MSC_VER) && !defined(__clang__)
                return __check_isa_support(instruction_family, 0);
#else
                switch (instruction_family) {
                case SSE:
                        return __builtin_cpu_supports("sse4.2");
                case AVX256:
                        return __builtin_cpu_supports("avx2");
                case AVX512:
                        return __builtin_cpu_supports("avx512f");
                default:
                        return 0;
                }
#endif //  _MSC_VER
        }

#if defined(__ARM_NEON) || defined(__ARM_NEON__)
        return 1;
#else
        return 0;
#endif
}

SN_API int load_intrinsic_function(void *intrinsic_context)
{
        sn_null_assert(intrinsic_context);

        sn_intrinsic *context = (sn_intrinsic *)intrinsic_context;

        if (check_isa(AVX512)) {
                load_intrinsic_512(context);
                return 3;
        } else if (check_isa(AVX256)) {
                load_intrinsic_avx(context);
                return 2;
        } else if (check_isa(SSE)) {
                load_intrinsic_sse(context);
                return 1;
        } else if (check_isa(NEON)) {
                return -1;
        }

        return 0;
}

void load_intrinsic_sse(sn_intrinsic *context)
{
        context->city_hash_simplify = city_hash_simplify_sse;
        context->factorial_simd = factorial_simd_sse;
        context->index_pair = index_pair_sse;
        context->f32_add = (MATARITHMETIC)f32_add_sse;
        context->f32_fma = (void (*)(void *, void *, void *))f32_fma_sse;
        context->f32_scalar_mul = f32_scalar_mul_sse;
        context->f32_sub = (MATARITHMETIC)f32_sub_sse;
        context->f32_transpose = (MATTRANSPOSEPROC)f32_transpose_sse;
        context->f32_normalize_3 = f32_normalize;
}

void load_intrinsic_avx(sn_intrinsic *context)
{
        context->city_hash_simplify = city_hash_simplify_sse;
        context->factorial_simd = factorial_simd_sse;
        context->index_pair = index_pair_sse;
        context->f32_add = (MATARITHMETIC)f32_add_avx;
        context->f32_fma = (void (*)(void *, void *, void *))f32_fma_sse;
        context->f32_scalar_mul = f32_scalar_mul_avx;
        context->f32_sub = (MATARITHMETIC)f32_sub_avx;
        context->f32_transpose = (MATTRANSPOSEPROC)f32_transpose_avx;
        context->f32_normalize_3 = f32_normalize;
}

void load_intrinsic_512(sn_intrinsic *context)
{
        context->city_hash_simplify = city_hash_simplify_sse;
        context->factorial_simd = factorial_simd_sse;
        context->index_pair = index_pair_sse;
        context->f32_add = (MATARITHMETIC)f32_add_512;
        context->f32_fma = (void (*)(void *, void *, void *))f32_fma_sse;
        context->f32_scalar_mul = f32_scalar_mul_512;
        context->f32_sub = (MATARITHMETIC)f32_sub_512;
        context->f32_transpose = (MATTRANSPOSEPROC)f32_transpose_512;
        context->f32_normalize_3 = f32_normalize;
}
