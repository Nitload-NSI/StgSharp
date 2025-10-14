#include "sn_intrinsic.h"
#include "sn_internal.h"

INTERNAL void SN_DECL f32_normalize(VEC(float) *restrict source, VEC(float) *restrict target)
{
        register __m128 s = _mm_load_ps(&source->xmm);
        register __m128 a_length_sq = _mm_mul_ps(s, s);
        a_length_sq = _mm_add_ps(a_length_sq,
                                 _mm_shuffle_ps(a_length_sq, a_length_sq, _MM_SHUFFLE(2, 3, 0, 1)));
        a_length_sq = _mm_add_ss(a_length_sq,
                                 _mm_shuffle_ps(a_length_sq, a_length_sq, _MM_SHUFFLE(0, 0, 3, 2)));

        __m128 a_inv_length = _mm_rsqrt_ps(a_length_sq);
        s = _mm_mul_ps(s, a_inv_length);

        __m128 dot_product = _mm_mul_ps(target->xmm[0], s);
        dot_product = _mm_add_ps(dot_product,
                                 _mm_shuffle_ps(dot_product, dot_product, _MM_SHUFFLE(2, 3, 0, 1)));
        dot_product = _mm_add_ss(dot_product,
                                 _mm_shuffle_ps(dot_product, dot_product, _MM_SHUFFLE(0, 0, 3, 2)));

        __m128 b_orthogonal = _mm_sub_ps(target->xmm[0], _mm_mul_ps(s, dot_product));

        __m128 b_length_sq = _mm_mul_ps(b_orthogonal, b_orthogonal);
        b_length_sq = _mm_add_ps(b_length_sq,
                                 _mm_shuffle_ps(b_length_sq, b_length_sq, _MM_SHUFFLE(2, 3, 0, 1)));
        b_length_sq = _mm_add_ss(b_length_sq,
                                 _mm_shuffle_ps(b_length_sq, b_length_sq, _MM_SHUFFLE(0, 0, 3, 2)));

        __m128 b_inv_length = _mm_rsqrt_ps(b_length_sq);
        target->xmm[0] = _mm_mul_ps(b_orthogonal, b_inv_length);
}
