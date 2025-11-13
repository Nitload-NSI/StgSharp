#include "sn_intrinsic.h"

DECLARE_BUF_PROC_ANS_SCALAR(float, sse, fill)
{
        register __m128 vec = _mm_set_ps1(((float *)(scalar->data))[0]);
        for (size_t i = 0; i < count; i++) {
                ans[i].xmm[0] = vec;
                ans[i].xmm[1] = vec;
                ans[i].xmm[2] = vec;
                ans[i].xmm[3] = vec;
        }
}

DECLARE_BUF_PROC_ANS_SCALAR(float, avx, fill)
{
        register __m256 vec = _mm256_broadcast_ss(((float *)(scalar->data)));
        for (size_t i = 0; i < count; i++) {
                ans[i].ymm[0] = vec;
                ans[i].ymm[1] = vec;
        }
}

DECLARE_BUF_PROC_ANS_SCALAR(float, 512, fill)
{
        register __m512 vec = _mm512_broadcastss_ps(((__m128 *)(scalar->data))[0]);
        for (size_t i = 0; i < count; i++) {
                ans[i].zmm[0] = vec;
        }
}

DECLARE_KER_PROC_LEFT_RIGHT_ANS(float, sse, add)
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

DECLARE_KER_PROC_LEFT_RIGHT_ANS(float, avx, add)
{
        register __m256 c0 = _mm256_add_ps(left->ymm[0], right->ymm[0]);
        register __m256 c1 = _mm256_add_ps(left->ymm[1], right->ymm[1]);
        ans->ymm[0] = c0;
        ans->ymm[1] = c1;
}

DECLARE_KER_PROC_LEFT_RIGHT_ANS(float, 512, add)
{
        register __m512 c0 = _mm512_add_ps(left->zmm[0], right->zmm[0]);
        ans->zmm[0] = c0;
}

DECLARE_KER_PROC_LEFT_RIGHT_ANS(float, sse, sub)
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

DECLARE_KER_PROC_LEFT_RIGHT_ANS(float, avx, sub)
{
        register __m256 c0 = _mm256_sub_ps(left->ymm[0], right->ymm[0]);
        register __m256 c1 = _mm256_sub_ps(left->ymm[1], right->ymm[1]);
        ans->ymm[0] = c0;
        ans->ymm[1] = c1;
}

DECLARE_KER_PROC_LEFT_RIGHT_ANS(float, 512, sub)
{
        register __m512 c0 = _mm512_sub_ps(left->zmm[0], right->zmm[0]);
        ans->zmm[0] = c0;
}

// Matrix transpose implementations
DECLARE_KER_PROC_RIGHT_ANS(float, sse, transpose)
{
        register __m128 col0 = right->xmm[0];
        register __m128 col1 = right->xmm[1];
        register __m128 col2 = right->xmm[2];
        register __m128 col3 = right->xmm[3];
        register __m128 tmp0 = _mm_unpacklo_ps(col0, col1);
        register __m128 tmp1 = _mm_unpackhi_ps(col0, col1);
        register __m128 tmp2 = _mm_unpacklo_ps(col2, col3);
        register __m128 tmp3 = _mm_unpackhi_ps(col2, col3);

        ans->xmm[0] = _mm_movelh_ps(tmp0, tmp2);
        ans->xmm[1] = _mm_movehl_ps(tmp2, tmp0);
        ans->xmm[2] = _mm_movelh_ps(tmp1, tmp3);
        ans->xmm[3] = _mm_movehl_ps(tmp3, tmp1);
}

DECLARE_KER_PROC_RIGHT_ANS(float, avx, transpose)
{
        register __m256 col_low = right->ymm[0];
        register __m256 col_high = right->ymm[1];

        register __m256 unpack_low = _mm256_unpacklo_ps(col_low, col_high);
        register __m256 unpack_high = _mm256_unpackhi_ps(col_low, col_high);

        col_low = _mm256_permute2f128_ps(unpack_low, unpack_high, 0x20);
        col_high = _mm256_permute2f128_ps(unpack_low, unpack_high, 0x31);

        ans->ymm[0] = _mm256_shuffle_ps(col_high, col_low, _MM_SHUFFLE(2, 0, 2, 0));
        ans->ymm[1] = _mm256_shuffle_ps(col_high, col_low, _MM_SHUFFLE(3, 1, 3, 1));
}

DECLARE_KER_PROC_RIGHT_ANS(float, 512, transpose)
{
        register __m512 matrix = right->zmm[0];

        const __m512i transpose_indices =
                _mm512_setr_epi32(0, 4, 8, 12, 1, 5, 9, 13, 2, 6, 10, 14, 3, 7, 11, 15);

        ans->zmm[0] = _mm512_permutexvar_ps(transpose_indices, matrix);
}

DECLARE_KER_PROC_RIGHT_ANS_SCALAR(float, sse, scalar_mul)
{
        register __m128 scalar_vec = ((__m128 *)(scalar->data))[0];

        register __m128 c0 = _mm_mul_ps(right->xmm[0], scalar_vec);
        register __m128 c1 = _mm_mul_ps(right->xmm[1], scalar_vec);
        register __m128 c2 = _mm_mul_ps(right->xmm[2], scalar_vec);
        register __m128 c3 = _mm_mul_ps(right->xmm[3], scalar_vec);

        ans->xmm[0] = c0;
        ans->xmm[1] = c1;
        ans->xmm[2] = c2;
        ans->xmm[3] = c3;
}

DECLARE_KER_PROC_RIGHT_ANS_SCALAR(float, avx, scalar_mul)
{
        register __m256 scalar_vec = ((__m256 *)(scalar->data))[0];

        register __m256 c0 = _mm256_mul_ps(right->ymm[0], scalar_vec);
        register __m256 c1 = _mm256_mul_ps(right->ymm[1], scalar_vec);

        ans->ymm[0] = c0;
        ans->ymm[1] = c1;
}

DECLARE_KER_PROC_RIGHT_ANS_SCALAR(float, 512, scalar_mul)
{
        register __m512 scalar_vec = ((__m512 *)(scalar->data))[0];

        register __m512 c0 = _mm512_mul_ps(right->zmm[0], scalar_vec);

        ans->zmm[0] = c0;
}
