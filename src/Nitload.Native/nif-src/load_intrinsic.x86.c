#include "nif_target.h"

#if NIF_IS_ARCH(NIF_ARCH_X86_64)

#include "NitloadFrameWork.h"
#include "nif_intrinsic.h"
#include "nif_intrinsic.std.h"
#include "nif_intrinsic.context.x86.h"

PRIVATE void load_intrinsic_sse(nif_intrinsic *context, SIMDID id);
PRIVATE void load_intrinsic_avx(nif_intrinsic *context, SIMDID id);
PRIVATE void load_intrinsic_512(nif_intrinsic *context, SIMDID id);

NIF_API void NIF_DECL nif_load_intrinsic_function(void *intrinsic_context, uint64_t id)
{
        nif_null_assert(intrinsic_context);

        SIMDID simdid;
        if (id) {
                simdid.mask = id;
        } else {
                simdid = nif_get_simd_level_global();
        }

        nif_intrinsic *context = (nif_intrinsic *)intrinsic_context;

        switch (simdid.make_byte[1]) {
        case SIMDID_MAIN_LVL_SSE:
                load_intrinsic_sse(context, simdid);
                break;
        case SIMDID_MAIN_LVL_AVX2:
                load_intrinsic_avx(context, simdid);
                break;
        case SIMDID_MAIN_LVL_AVX512:
        case SIMDID_MAIN_LVL_AVX10:
                load_intrinsic_512(context, simdid);
                break;
        }
}

void load_intrinsic_sse(nif_intrinsic *context, SIMDID id)
{
        context->city_hash_simplify = city_hash_simplify_sse;
        context->factorial_simd = factorial_simd_sse;

        context->mat_mk[F32].proc_array[Add] = GET_MK_PROC_STD(float, sse, , add);
        context->mat_mk[F32].proc_array[Fill] = GET_MK_PROC_STD(float, sse, , fill);
        context->mat_mk[F32].proc_array[ScalarMul] = GET_MK_PROC_STD(float, sse, , scalar_mul);
        context->mat_mk[F32].proc_array[Sub] = GET_MK_PROC_STD(float, sse, , sub);
        context->mat_mk[F32].proc_array[Transpose] = GET_MK_PROC_STD(float, sse, , transpose);
        context->mat_mk[F32].proc_array[Fma] = GET_MK_PROC_STD(float, sse, , fma);
        context->mat_mk[F32].proc_array[Pivot] = NULL;

        context->mat_mk[F64].proc_array[Add] = GET_MK_PROC_STD(double, sse, , add);
        context->mat_mk[F64].proc_array[Fill] = GET_MK_PROC_STD(double, sse, , fill);
        context->mat_mk[F64].proc_array[ScalarMul] = GET_MK_PROC_STD(double, sse, , scalar_mul);
        context->mat_mk[F64].proc_array[Sub] = GET_MK_PROC_STD(double, sse, , sub);
        context->mat_mk[F64].proc_array[Transpose] = NULL;
        context->mat_mk[F64].proc_array[Fma] = GET_MK_PROC_STD(double, sse, , fma);
        context->mat_mk[F64].proc_array[Pivot] = NULL;

        context->vec_f32_normalize_3 = f32_normalize;
}

void load_intrinsic_avx(nif_intrinsic *context, SIMDID id)
{
        char avx_seg = id.make_byte[4];
        char is_fma = (avx_seg & SIMDID_AVX_FMA) != 0;

        context->city_hash_simplify = city_hash_simplify_sse;
        context->factorial_simd = factorial_simd_sse;

        context->mat_mk[F32].proc_array[Add] = GET_MK_PROC_STD(float, avx, , add);
        context->mat_mk[F32].proc_array[Fill] = GET_MK_PROC_STD(float, avx, , fill);
        context->mat_mk[F32].proc_array[ScalarMul] = GET_MK_PROC_STD(float, avx, , scalar_mul);
        context->mat_mk[F32].proc_array[Sub] = GET_MK_PROC_STD(float, avx, , sub);
        context->mat_mk[F32].proc_array[Transpose] = GET_MK_PROC_STD(float, avx, , transpose);
        context->mat_mk[F32].proc_array[Fma] = is_fma ? GET_MK_PROC_STD(float, avx, _fma, fma) :
                                            GET_MK_PROC_STD(float, avx, , fma);
        context->mat_mk[F32].proc_array[Pivot] = NULL;

        context->mat_mk[F64].proc_array[Add] = GET_MK_PROC_STD(double, avx, , add);
        context->mat_mk[F64].proc_array[Fill] = GET_MK_PROC_STD(double, avx, , fill);
        context->mat_mk[F64].proc_array[ScalarMul] = GET_MK_PROC_STD(double, avx, , scalar_mul);
        context->mat_mk[F64].proc_array[Sub] = GET_MK_PROC_STD(double, avx, , sub);
        context->mat_mk[F64].proc_array[Transpose] = NULL;
        context->mat_mk[F64].proc_array[Fma] = is_fma ? GET_MK_PROC_STD(double, avx, _fma, fma) :
                                            GET_MK_PROC_STD(double, avx, , fma);
        context->mat_mk[F64].proc_array[Pivot] = NULL;

        context->vec_f32_normalize_3 = f32_normalize;
}

void load_intrinsic_512(nif_intrinsic *context, SIMDID id)
{
        context->city_hash_simplify = city_hash_simplify_sse;
        context->factorial_simd = factorial_simd_sse;

        context->mat_mk[F32].proc_array[Add] = GET_MK_PROC_STD(float, 512, , add);
        context->mat_mk[F32].proc_array[Fill] = GET_MK_PROC_STD(float, 512, , fill);
        context->mat_mk[F32].proc_array[ScalarMul] = GET_MK_PROC_STD(float, 512, , scalar_mul);
        context->mat_mk[F32].proc_array[Sub] = GET_MK_PROC_STD(float, 512, , sub);
        context->mat_mk[F32].proc_array[Transpose] = GET_MK_PROC_STD(float, 512, , transpose);
        context->mat_mk[F32].proc_array[Fma] = GET_MK_PROC_STD(float, 512, , fma);
        context->mat_mk[F32].proc_array[Pivot] = NULL;

        context->mat_mk[F64].proc_array[Add] = GET_MK_PROC_STD(double, 512, , add);
        context->mat_mk[F64].proc_array[Fill] = GET_MK_PROC_STD(double, 512, , fill);
        context->mat_mk[F64].proc_array[ScalarMul] = GET_MK_PROC_STD(double, 512, , scalar_mul);
        context->mat_mk[F64].proc_array[Sub] = GET_MK_PROC_STD(double, 512, , sub);
        context->mat_mk[F64].proc_array[Transpose] = NULL;
        context->mat_mk[F64].proc_array[Fma] = GET_MK_PROC_STD(double, 512, , fma);
        context->mat_mk[F64].proc_array[Pivot] = NULL;

        context->vec_f32_normalize_3 = f32_normalize;
}

#endif
