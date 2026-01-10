#include "sn_target.h"

#if SN_IS_ARCH(SN_ARCH_X86_64)

#include "StgSharpNative.h"
#include "sn_intrinsic.h"
#include "sn_intrincontext.x86.h"

#if defined(_MSC_VER) && !defined(__clang__)
#include <intrin.h>
#include <isa_availability.h>
#else
#include <cpuid.h>
#endif //  _MSC_VER

PRIVATE void load_intrinsic_sse(sn_intrinsic *context, SIMDID id);
PRIVATE void load_intrinsic_avx(sn_intrinsic *context, SIMDID id);
PRIVATE void load_intrinsic_512(sn_intrinsic *context, SIMDID id);

SN_API SIMDID load_intrinsic_function(void *intrinsic_context)
{
        sn_null_assert(intrinsic_context);

        SIMDID simdid = sn_get_simd_level();

        sn_intrinsic *context = (sn_intrinsic *)intrinsic_context;

        switch (simdid.make_byte[1]) {
        case SIMDID_MAIN_LVL_SSE:
                load_intrinsic_sse(context, simdid);
                break;
        case SIMDID_MAIN_LVL_AVX2:
                load_intrinsic_avx(context, simdid);
                break;
        case SIMDID_MAIN_LVL_AVX512:
                load_intrinsic_512(context, simdid);
                break;
        }
        return simdid;
}

void load_intrinsic_sse(sn_intrinsic *context, SIMDID id)
{
        context->city_hash_simplify = city_hash_simplify_sse;
        context->factorial_simd = factorial_simd_sse;
        context->index_pair = index_pair_sse;

        context->mat[F32].buffer_add = BUF_PROC(float, sse, , add);
        context->mat[F32].buffer_fill = BUF_PROC(float, sse, , fill);
        context->mat[F32].buffer_scalar_mul = BUF_PROC(float, sse, , scalar_mul);
        context->mat[F32].buffer_sub = BUF_PROC(float, sse, , sub);
        context->mat[F32].buffer_transpose = KER_PROC(float, sse, , transpose);
        context->mat[F32].build_panel = NULL;
        context->mat[F32].clear_panel = PNL_PROC(float, sse, , clear);
        context->mat[F32].ker_fma = KER_PROC(float, sse, , fma);
        context->mat[F32].ker_tile_fma = KERTILE_PROC(float, sse, , fma);
        context->mat[F32].pivot = pivot_float;
        context->mat[F32].store_panel = NULL;

        context->mat[F64].buffer_add = BUF_PROC(double, sse, , add);
        context->mat[F64].buffer_fill = BUF_PROC(double, sse, , fill);
        context->mat[F64].buffer_scalar_mul = BUF_PROC(double, sse, , scalar_mul);
        context->mat[F64].buffer_sub = BUF_PROC(double, sse, , sub);
        context->mat[F64].buffer_transpose = NULL;
        context->mat[F64].build_panel = NULL;
        context->mat[F64].clear_panel = PNL_PROC(double, sse, , clear);
        context->mat[F64].ker_fma = KER_PROC(double, sse, , fma);
        context->mat[F64].ker_tile_fma = KERTILE_PROC(double, sse, , fma);
        context->mat[F64].pivot = NULL;
        context->mat[F64].store_panel = NULL;

        context->vec_f32_normalize_3 = f32_normalize;
}

void load_intrinsic_avx(sn_intrinsic *context, SIMDID id)
{
        char avx_seg = id.make_byte[5];
        char is_fma = (avx_seg & 1 << 3) != 0;
        char is_f16c = (avx_seg & 1 << 4) != 0;

        context->city_hash_simplify = city_hash_simplify_sse;
        context->factorial_simd = factorial_simd_sse;
        context->index_pair = index_pair_sse;

        context->mat[F32].buffer_add = BUF_PROC(float, avx, , add);
        context->mat[F32].buffer_fill = BUF_PROC(float, avx, , fill);
        context->mat[F32].buffer_scalar_mul = BUF_PROC(float, avx, , scalar_mul);
        context->mat[F32].buffer_sub = BUF_PROC(float, avx, , sub);
        context->mat[F32].buffer_transpose = KER_PROC(float, avx, , transpose);
        context->mat[F32].build_panel = BUILD_PANEL(avx, float);
        context->mat[F32].clear_panel = PNL_PROC(float, avx, , clear);
        context->mat[F32].ker_fma = is_fma ? KER_PROC(float, avx, , fma) :
                                             KER_PROC(float, avx, _fma, fma);
        context->mat[F32].ker_tile_fma = is_fma ? KERTILE_PROC(float, avx, , fma) :
                                                  KERTILE_PROC(float, avx, _fma, fma);
        context->mat[F32].pivot = pivot_float;
        context->mat[F32].store_panel = STORE_PANEL(avx, float);

        context->mat[F64].buffer_add = BUF_PROC(double, avx, , add);
        context->mat[F64].buffer_fill = BUF_PROC(double, avx, , fill);
        context->mat[F64].buffer_scalar_mul = BUF_PROC(double, avx, , scalar_mul);
        context->mat[F64].buffer_sub = BUF_PROC(double, avx, , sub);
        context->mat[F64].buffer_transpose = NULL;
        context->mat[F64].build_panel = NULL;
        context->mat[F64].clear_panel = PNL_PROC(double, avx, , clear);
        context->mat[F64].ker_fma = is_fma ? KER_PROC(double, avx, , fma) :
                                             KER_PROC(double, avx, _fma, fma);
        context->mat[F64].ker_tile_fma = is_fma ? KERTILE_PROC(double, avx, , fma) :
                                                  KERTILE_PROC(double, avx, _fma, fma);
        context->mat[F64].pivot = NULL;
        context->mat[F64].store_panel = NULL;

        context->vec_f32_normalize_3 = f32_normalize;
}

void load_intrinsic_512(sn_intrinsic *context, SIMDID id)
{
        context->city_hash_simplify = city_hash_simplify_sse;
        context->factorial_simd = factorial_simd_sse;
        context->index_pair = index_pair_sse;

        context->mat[F32].buffer_add = BUF_PROC(float, 512, , add);
        context->mat[F32].buffer_fill = BUF_PROC(float, 512, , fill);
        context->mat[F32].buffer_scalar_mul = BUF_PROC(float, 512, , scalar_mul);
        context->mat[F32].buffer_sub = BUF_PROC(float, 512, , sub);
        context->mat[F32].buffer_transpose = KER_PROC(float, 512, , transpose);
        context->mat[F32].build_panel = BUILD_PANEL(512, float);
        context->mat[F32].clear_panel = PNL_PROC(float, 512, , clear);
        context->mat[F32].ker_fma = KER_PROC(float, 512, , fma);
        context->mat[F32].ker_tile_fma = KERTILE_PROC(float, 512, , fma);
        context->mat[F32].pivot = pivot_float;
        context->mat[F32].store_panel = STORE_PANEL(512, float);

        context->mat[F64].buffer_add = BUF_PROC(double, 512, , add);
        context->mat[F64].buffer_fill = BUF_PROC(double, 512, , fill);
        context->mat[F64].buffer_scalar_mul = BUF_PROC(double, 512, , scalar_mul);
        context->mat[F64].buffer_sub = BUF_PROC(double, 512, , sub);
        context->mat[F64].buffer_transpose = NULL;
        context->mat[F64].build_panel = BUILD_PANEL(512, double);
        context->mat[F64].clear_panel = PNL_PROC(double, 512, , clear);
        context->mat[F64].ker_fma = KER_PROC(double, 512, , fma);
        context->mat[F64].ker_tile_fma = KERTILE_PROC(double, 512, , fma);
        context->mat[F64].pivot = NULL;
        context->mat[F64].store_panel = STORE_PANEL(512, double);

        context->vec_f32_normalize_3 = f32_normalize;
}

#endif
