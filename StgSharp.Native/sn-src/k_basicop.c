#include "sn_intrinsic.h"

INTERNAL void SN_DECL f32_add_sse(MAT_KERNEL(float) const *left, MAT_KERNEL(float) const *right,
                                  MAT_KERNEL(float) *restrict ans)
{
        register __m128 c0 = _mm_add_ps(left->xmm[0], right->xmm[0]);
        register __m128 c1 = _mm_add_ps(left->xmm[1], right->xmm[1]);
        register __m128 c2 = _mm_add_ps(left->xmm[2], right->xmm[2]);
        register __m128 c3 = _mm_add_ps(left->xmm[3], right->xmm[3]);

        ans->xmm[0] = c0;
        ans->xmm[1] = c1;
        ans->xmm[2] = c2;
        ans->xmm[3] = c3;
}

INTERNAL void SN_DECL f32_add_avx(MAT_KERNEL(float) const *left, MAT_KERNEL(float) const *right,
                                  MAT_KERNEL(float) *restrict ans)
{
        register __m256 c0 = _mm256_add_ps(left->ymm[0], right->ymm[0]);
        register __m256 c1 = _mm256_add_ps(left->ymm[1], right->ymm[1]);
        ans->ymm[0] = c0;
        ans->ymm[1] = c1;
}

INTERNAL void SN_DECL f32_add_512(MAT_KERNEL(float) const *left, MAT_KERNEL(float) const *right,
                                  MAT_KERNEL(float) *restrict ans)
{
        register __m512 c0 = _mm512_add_ps(left->zmm[0], right->zmm[0]);
        ans->zmm[0] = c0;
}

INTERNAL void SN_DECL f32_sub_sse(MAT_KERNEL(float) const *left, MAT_KERNEL(float) const *right,
                                  MAT_KERNEL(float) *restrict ans)
{
        register __m128 c0 = _mm_sub_ps(left->xmm[0], right->xmm[0]);
        register __m128 c1 = _mm_sub_ps(left->xmm[1], right->xmm[1]);
        register __m128 c2 = _mm_sub_ps(left->xmm[2], right->xmm[2]);
        register __m128 c3 = _mm_sub_ps(left->xmm[3], right->xmm[3]);

        ans->xmm[0] = c0;
        ans->xmm[1] = c1;
        ans->xmm[2] = c2;
        ans->xmm[3] = c3;
}

INTERNAL void SN_DECL f32_sub_avx(MAT_KERNEL(float) const *left, MAT_KERNEL(float) const *right,
                                  MAT_KERNEL(float) *restrict ans)
{
        register __m256 c0 = _mm256_sub_ps(left->ymm[0], right->ymm[0]);
        register __m256 c1 = _mm256_sub_ps(left->ymm[1], right->ymm[1]);
        ans->ymm[0] = c0;
        ans->ymm[1] = c1;
}

INTERNAL void SN_DECL f32_sub_512(MAT_KERNEL(float) const *left, MAT_KERNEL(float) const *right,
                                  MAT_KERNEL(float) *restrict ans)
{
        register __m512 c0 = _mm512_sub_ps(left->zmm[0], right->zmm[0]);
        ans->zmm[0] = c0;
}

// Matrix transpose implementations
INTERNAL void SN_DECL f32_transpose_sse(MAT_KERNEL(float) const *source,
                                        MAT_KERNEL(float) *restrict target)
{
        register __m128 col0 = source->xmm[0];
        register __m128 col1 = source->xmm[1];
        register __m128 col2 = source->xmm[2];
        register __m128 col3 = source->xmm[3];
        register __m128 tmp0 = _mm_unpacklo_ps(col0, col1);
        register __m128 tmp1 = _mm_unpackhi_ps(col0, col1);
        register __m128 tmp2 = _mm_unpacklo_ps(col2, col3);
        register __m128 tmp3 = _mm_unpackhi_ps(col2, col3);

        target->xmm[0] = _mm_movelh_ps(tmp0, tmp2);
        target->xmm[1] = _mm_movehl_ps(tmp2, tmp0);
        target->xmm[2] = _mm_movelh_ps(tmp1, tmp3);
        target->xmm[3] = _mm_movehl_ps(tmp3, tmp1);
}

INTERNAL void SN_DECL f32_transpose_avx(MAT_KERNEL(float) const *source,
                                        MAT_KERNEL(float) *restrict target)
{
        register __m256 col_low = source->ymm[0];
        register __m256 col_high = source->ymm[1];

        register __m256 unpack_low = _mm256_unpacklo_ps(col_low, col_high);
        register __m256 unpack_high = _mm256_unpackhi_ps(col_low, col_high);

        col_low = _mm256_permute2f128_ps(unpack_low, unpack_high, 0x20);
        col_high = _mm256_permute2f128_ps(unpack_low, unpack_high, 0x31);

        target->ymm[0] = _mm256_shuffle_ps(col_high, col_low, _MM_SHUFFLE(2, 0, 2, 0));
        target->ymm[1] = _mm256_shuffle_ps(col_high, col_low, _MM_SHUFFLE(3, 1, 3, 1));
}

INTERNAL void SN_DECL f32_transpose_512(MAT_KERNEL(float) const *source,
                                        MAT_KERNEL(float) *restrict target)
{
        register __m512 matrix = source->zmm[0];

        const __m512i transpose_indices =
                _mm512_setr_epi32(0, 4, 8, 12, 1, 5, 9, 13, 2, 6, 10, 14, 3, 7, 11, 15);

        target->zmm[0] = _mm512_permutexvar_ps(transpose_indices, matrix);
}

INTERNAL void SN_DECL f32_scalar_mul_sse(MAT_KERNEL(float) const *matrix, float const scalar,
                                         MAT_KERNEL(float) *restrict ans)
{
        register __m128 temp = _mm_load_ss(&scalar);
        register __m128 scalar_vec = _mm_shuffle_ps(temp, temp, _MM_SHUFFLE(0, 0, 0, 0));

        register __m128 c0 = _mm_mul_ps(matrix->xmm[0], scalar_vec);
        register __m128 c1 = _mm_mul_ps(matrix->xmm[1], scalar_vec);
        register __m128 c2 = _mm_mul_ps(matrix->xmm[2], scalar_vec);
        register __m128 c3 = _mm_mul_ps(matrix->xmm[3], scalar_vec);

        ans->xmm[0] = c0;
        ans->xmm[1] = c1;
        ans->xmm[2] = c2;
        ans->xmm[3] = c3;
}

INTERNAL void SN_DECL f32_scalar_mul_avx(MAT_KERNEL(float) const *matrix, float const scalar,
                                         MAT_KERNEL(float) *restrict ans)
{
        register __m256 scalar_vec = _mm256_broadcast_ss(&scalar);

        register __m256 c0 = _mm256_mul_ps(matrix->ymm[0], scalar_vec);
        register __m256 c1 = _mm256_mul_ps(matrix->ymm[1], scalar_vec);

        ans->ymm[0] = c0;
        ans->ymm[1] = c1;
}

INTERNAL void SN_DECL f32_scalar_mul_512(MAT_KERNEL(float) const *matrix, float const scalar,
                                         MAT_KERNEL(float) *restrict ans)
{
        __m512 scalar_vec = _mm512_broadcastss_ps(_mm_load_ss(&scalar));

        register __m512 c0 = _mm512_mul_ps(matrix->zmm[0], scalar_vec);

        ans->zmm[0] = c0;
}
