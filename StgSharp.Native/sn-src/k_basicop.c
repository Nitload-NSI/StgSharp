#include "sn_intrinsic.h"

<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> stgsharp-dev/giga
DECLARE_PNL_PROC_ANS(float, sse, clear)
{
        register __m128 zero = _mm_setzero_ps();
        PANEL_COL_VEC((*ans), 0, 0) = zero;
        PANEL_COL_VEC((*ans), 1, 0) = zero;
        PANEL_COL_VEC((*ans), 2, 0) = zero;
        PANEL_COL_VEC((*ans), 3, 0) = zero;
}

DECLARE_PNL_PROC_ANS(float, avx, clear)
{
        register __m256 zero = _mm256_setzero_ps();
<<<<<<< HEAD
        PANEL_COL_VEC((*ans), 0, 0) = zero;
        PANEL_COL_VEC((*ans), 1, 0) = zero;
        PANEL_COL_VEC((*ans), 2, 0) = zero;
        PANEL_COL_VEC((*ans), 3, 0) = zero;
=======
        for (size_t i = 0; i < 8; i++) {
                PANEL_COL_VEC((*ans), i, 0) = zero;
        }
>>>>>>> stgsharp-dev/giga
}

DECLARE_PNL_PROC_ANS(float, 512, clear)
{
        register __m512 zero = _mm512_setzero_ps();
<<<<<<< HEAD
        PANEL_COL_VEC((*ans), 0, 0) = zero;
        PANEL_COL_VEC((*ans), 1, 0) = zero;
        PANEL_COL_VEC((*ans), 2, 0) = zero;
        PANEL_COL_VEC((*ans), 3, 0) = zero;
}

=======
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
=======
        for (size_t i = 0; i < 16; i++) {
                PANEL_COL_VEC((*ans), i, 0) = zero;
        }
}

>>>>>>> stgsharp-dev/giga
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
