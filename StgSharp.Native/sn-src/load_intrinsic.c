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

        context->mat[F32].buffer_add = BUF_PROC(float, sse, add);
        context->mat[F32].buffer_fill = BUF_PROC(float, sse, fill);
        context->mat[F32].buffer_scalar_mul = BUF_PROC(float, sse, scalar_mul);
        context->mat[F32].buffer_sub = BUF_PROC(float, sse, sub);
        context->mat[F32].buffer_transpose = KER_PROC(float, sse, transpose);
        context->mat[F32].build_panel = NULL;
        context->mat[F32].clear_panel = PNL_PROC(float, sse, clear);
        context->mat[F32].panel_fma = PNL_PROC(float, sse, fma);
        context->mat[F32].pivot = pivot_float;
        context->mat[F32].store_panel = NULL;

        context->mat[F64].buffer_add = BUF_PROC(double, sse, add);
        context->mat[F64].buffer_fill = BUF_PROC(double, sse, fill);
        context->mat[F64].buffer_scalar_mul = BUF_PROC(double, sse, scalar_mul);
        context->mat[F64].buffer_sub = BUF_PROC(double, sse, sub);
        context->mat[F64].buffer_transpose = NULL;
        context->mat[F64].build_panel = NULL;
        context->mat[F64].clear_panel = PNL_PROC(double, sse, clear);
        context->mat[F64].panel_fma = NULL;
        context->mat[F64].pivot = NULL;
        context->mat[F64].store_panel = NULL;

        context->vec_f32_normalize_3 = f32_normalize;
}

void load_intrinsic_avx(sn_intrinsic *context)
{
        context->city_hash_simplify = city_hash_simplify_sse;
        context->factorial_simd = factorial_simd_sse;
        context->index_pair = index_pair_sse;

        context->mat[F32].buffer_add = BUF_PROC(float, avx, add);
        context->mat[F32].buffer_fill = BUF_PROC(float, avx, fill);
        context->mat[F32].buffer_scalar_mul = BUF_PROC(float, avx, scalar_mul);
        context->mat[F32].buffer_sub = BUF_PROC(float, avx, sub);
        context->mat[F32].buffer_transpose = KER_PROC(float, avx, transpose);
        context->mat[F32].build_panel = BUILD_PANEL(avx, float);
        context->mat[F32].clear_panel = PNL_PROC(float, avx, clear);
        context->mat[F32].panel_fma = PNL_PROC(float, avx, fma);
        context->mat[F32].pivot = pivot_float;
        context->mat[F32].store_panel = STORE_PANEL(avx, float);

        context->mat[F64].buffer_add = BUF_PROC(double, avx, add);
        context->mat[F64].buffer_fill = BUF_PROC(double, avx, fill);
        context->mat[F64].buffer_scalar_mul = BUF_PROC(double, avx, scalar_mul);
        context->mat[F64].buffer_sub = BUF_PROC(double, avx, sub);
        context->mat[F64].buffer_transpose = NULL;
        context->mat[F64].build_panel = NULL;
        context->mat[F64].clear_panel = PNL_PROC(double, avx, clear);
        context->mat[F64].panel_fma = NULL;
        context->mat[F64].pivot = NULL;
        context->mat[F64].store_panel = NULL;

        context->vec_f32_normalize_3 = f32_normalize;
}

void load_intrinsic_512(sn_intrinsic *context)
{
        context->city_hash_simplify = city_hash_simplify_sse;
        context->factorial_simd = factorial_simd_sse;
        context->index_pair = index_pair_sse;

        context->mat[F32].buffer_add = BUF_PROC(float, 512, add);
        context->mat[F32].buffer_fill = BUF_PROC(float, 512, fill);
        context->mat[F32].buffer_scalar_mul = BUF_PROC(float, 512, scalar_mul);
        context->mat[F32].buffer_sub = BUF_PROC(float, 512, sub);
        context->mat[F32].buffer_transpose = KER_PROC(float, 512, transpose);
        context->mat[F32].build_panel = BUILD_PANEL(512, float);
        context->mat[F32].clear_panel = PNL_PROC(float, 512, clear);
        context->mat[F32].panel_fma = PNL_PROC(float, 512, fma);
        context->mat[F32].pivot = pivot_float;
        context->mat[F32].store_panel = STORE_PANEL(512, float);

        context->mat[F64].buffer_add = BUF_PROC(double, 512, add);
        context->mat[F64].buffer_fill = BUF_PROC(double, 512, fill);
        context->mat[F64].buffer_scalar_mul = BUF_PROC(double, 512, scalar_mul);
        context->mat[F64].buffer_sub = BUF_PROC(double, 512, sub);
        context->mat[F64].buffer_transpose = NULL;
        context->mat[F64].build_panel = BUILD_PANEL(512, double);
        context->mat[F64].clear_panel = PNL_PROC(double, 512, clear);
        context->mat[F64].panel_fma = NULL;
        context->mat[F64].pivot = NULL;
        context->mat[F64].store_panel = STORE_PANEL(512, double);

        context->vec_f32_normalize_3 = f32_normalize;
}
