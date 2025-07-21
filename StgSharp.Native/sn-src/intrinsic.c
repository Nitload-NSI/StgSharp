#include "StgSharpC.h"
#include "ssc_intrinsic.h"


#if defined(_MSC_VER) && !defined(__clang__)
#include <intrin.h>
#include <isa_availability.h>
#else
#include <cpuid.h>
#endif //  _MSC_VER

PRIVATE void load_intrinsic_sse(ssc_intrinsic *context);
PRIVATE void load_intrinsic_avx(ssc_intrinsic *context);
PRIVATE void load_intrinsic_512(ssc_intrinsic *context);

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

SSCAPI int load_intrinsic_function(void *intrinsic_context)
{
        ssc_null_assert(intrinsic_context);

        ssc_intrinsic *context = (ssc_intrinsic *)intrinsic_context;

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

void load_intrinsic_sse(ssc_intrinsic *context)
{
        context->add_m2 = addmatrix2_sse;
        context->add_m3 = addmatrix3_sse;
        context->add_m4 = addmatrix4_sse;
        context->transpose_23 = transpose23;
        context->transpose_24 = transpose24;
        context->transpose_32 = transpose32;
        context->transpose_33 = transpose33;
        context->transpose_34 = transpose34;
        context->transpose_42 = transpose42;
        context->transpose_43 = transpose43;
        context->transpose_44 = transpose44;
        context->normalize_3 = normalize;
        context->det_matrix33 = det_mat3;
        context->det_matrix44 = det_mat4;
        context->dot_31 = dot_31_sse;
        context->dot_32 = dot_32_sse;
        context->dot_41 = dot_41_sse;
        context->dot_42 = dot_42_sse;
        context->dot_43 = dot_43_sse;
        context->city_hash_simplify = city_hash_simplify_sse;
        context->index_pair = index_pair_sse;
        context->sub_m2 = submatrix2_sse;
        context->sub_m3 = submatrix3_sse;
        context->sub_m4 = submatrix4_sse;
        context->factorial_simd = factorial_simd_sse;
}

void load_intrinsic_avx(ssc_intrinsic *context)
{
        context->add_m2 = addmatrix2_avx;
        context->add_m3 = addmatrix3_avx;
        context->add_m4 = addmatrix4_avx;
        context->transpose_23 = transpose23;
        context->transpose_24 = transpose24;
        context->transpose_32 = transpose32;
        context->transpose_33 = transpose33;
        context->transpose_34 = transpose34;
        context->transpose_42 = transpose42;
        context->transpose_43 = transpose43;
        context->transpose_44 = transpose44;
        context->normalize_3 = normalize;
        context->det_matrix33 = det_mat3;
        context->det_matrix44 = det_mat4;
        context->dot_31 = dot_31_avx;
        context->dot_32 = dot_32_avx;
        context->dot_41 = dot_41_avx;
        context->dot_42 = dot_42_avx;
        context->dot_43 = dot_43_avx;
        context->city_hash_simplify = city_hash_simplify_sse;
        context->index_pair = index_pair_sse;
        context->factorial_simd = factorial_simd_sse;
}

void load_intrinsic_512(ssc_intrinsic *context)
{
        context->add_m2 = addmatrix2_avx;
        context->add_m3 = addmatrix3_512;
        context->add_m4 = addmatrix4_512;
        context->transpose_23 = transpose23;
        context->transpose_24 = transpose24;
        context->transpose_32 = transpose32;
        context->transpose_33 = transpose33;
        context->transpose_34 = transpose34;
        context->transpose_42 = transpose42;
        context->transpose_43 = transpose43;
        context->transpose_44 = transpose44;
        context->normalize_3 = normalize;
        context->det_matrix33 = det_mat3;
        context->det_matrix44 = det_mat4;
        context->dot_31 = dot_31_avx;
        context->dot_32 = dot_32_avx;
        context->dot_41 = dot_41_avx;
        context->dot_42 = dot_42_avx;
        context->dot_43 = dot_43_avx;
        context->city_hash_simplify = city_hash_simplify_sse;
        context->index_pair = index_pair_sse;
        context->factorial_simd = factorial_simd_sse;
}
