#include "StgSharpC.h"
#include "ssc_internal.h"

SSCAPI void SSCDECL normalize(__m128 *source, __m128 *target)
{
        __m128 a_length_sq = _mm_mul_ps(*source, *source);
        a_length_sq = _mm_add_ps(a_length_sq,
                                 _mm_shuffle_ps(a_length_sq, a_length_sq, _MM_SHUFFLE(2, 3, 0, 1)));
        a_length_sq = _mm_add_ss(a_length_sq,
                                 _mm_shuffle_ps(a_length_sq, a_length_sq, _MM_SHUFFLE(0, 0, 3, 2)));

        __m128 a_inv_length = _mm_rsqrt_ps(a_length_sq);
        *source = _mm_mul_ps(*source, a_inv_length);

        __m128 dot_product = _mm_mul_ps(*target, *source);
        dot_product = _mm_add_ps(dot_product,
                                 _mm_shuffle_ps(dot_product, dot_product, _MM_SHUFFLE(2, 3, 0, 1)));
        dot_product = _mm_add_ss(dot_product,
                                 _mm_shuffle_ps(dot_product, dot_product, _MM_SHUFFLE(0, 0, 3, 2)));

        __m128 b_orthogonal = _mm_sub_ps(*target, _mm_mul_ps(*source, dot_product));

        __m128 b_length_sq = _mm_mul_ps(b_orthogonal, b_orthogonal);
        b_length_sq = _mm_add_ps(b_length_sq,
                                 _mm_shuffle_ps(b_length_sq, b_length_sq, _MM_SHUFFLE(2, 3, 0, 1)));
        b_length_sq = _mm_add_ss(b_length_sq,
                                 _mm_shuffle_ps(b_length_sq, b_length_sq, _MM_SHUFFLE(0, 0, 3, 2)));

        __m128 b_inv_length = _mm_rsqrt_ps(b_length_sq);
        *target = _mm_mul_ps(b_orthogonal, b_inv_length);
}
