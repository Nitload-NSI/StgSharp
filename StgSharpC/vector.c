#include "StgSharpC.h"
#include "ssgc_internal.h"

SSCAPI void SSCDECL normalize(__m128* source, __m128* target)
{
    // 计算source的长度的平方
    __m128 a_length_sq = _mm_mul_ps(*source, *source); // 向量点乘自身
    a_length_sq = _mm_add_ps(a_length_sq, _mm_shuffle_ps(a_length_sq, a_length_sq, _MM_SHUFFLE(2, 3, 0, 1)));
    a_length_sq = _mm_add_ss(a_length_sq, _mm_shuffle_ps(a_length_sq, a_length_sq, _MM_SHUFFLE(0, 0, 3, 2)));

    // 归一化向量source
    __m128 a_inv_length = _mm_rsqrt_ps(a_length_sq); // 计算source长度的倒数平方根
    *source = _mm_mul_ps(*source, a_inv_length); // 用source乘以其长度的倒数平方根得到单位向量

    // 计算target与source的点积
    __m128 dot_product = _mm_mul_ps(*target, *source);
    dot_product = _mm_add_ps(dot_product, _mm_shuffle_ps(dot_product, dot_product, _MM_SHUFFLE(2, 3, 0, 1)));
    dot_product = _mm_add_ss(dot_product, _mm_shuffle_ps(dot_product, dot_product, _MM_SHUFFLE(0, 0, 3, 2)));

    // 从target中减去source的方向分量，使其垂直于source
    __m128 b_orthogonal = _mm_sub_ps(*target, _mm_mul_ps(*source, dot_product));

    // 归一化向量b_orthogonal
    __m128 b_length_sq = _mm_mul_ps(b_orthogonal, b_orthogonal);
    b_length_sq = _mm_add_ps(b_length_sq, _mm_shuffle_ps(b_length_sq, b_length_sq, _MM_SHUFFLE(2, 3, 0, 1)));
    b_length_sq = _mm_add_ss(b_length_sq, _mm_shuffle_ps(b_length_sq, b_length_sq, _MM_SHUFFLE(0, 0, 3, 2)));

    __m128 b_inv_length = _mm_rsqrt_ps(b_length_sq);
    *target = _mm_mul_ps(b_orthogonal, b_inv_length);

}
