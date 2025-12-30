#include "sn_intrinsic.h"

#define _FMA_SELECT(i) _MM_SHUFFLE(i, i, i, i)

// ans = left * right + ans
// right is not transposed
DECLARE_PNL_PROC_LEFT_RIGHT_ANS(float, sse, fma)
{
        const __m128 a0 = PANEL_COL_VEC((*left), 0, 0);
        const __m128 a1 = PANEL_COL_VEC((*left), 1, 0);
        const __m128 a2 = PANEL_COL_VEC((*left), 2, 0);
        const __m128 a3 = PANEL_COL_VEC((*left), 3, 0);

        __m128 c0 = PANEL_COL_VEC((*ans), 0, 0);
        __m128 c1 = PANEL_COL_VEC((*ans), 1, 0);
        __m128 c2 = PANEL_COL_VEC((*ans), 2, 0);
        __m128 c3 = PANEL_COL_VEC((*ans), 3, 0);

        __m128 bcol;

        /* Column 0 */
        bcol = PANEL_COL_VEC((*right), 0, 0);
        c0 = _mm_add_ps(c0, _mm_mul_ps(a0, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(0))));
        c0 = _mm_add_ps(c0, _mm_mul_ps(a1, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(1))));
        c0 = _mm_add_ps(c0, _mm_mul_ps(a2, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(2))));
        c0 = _mm_add_ps(c0, _mm_mul_ps(a3, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(3))));

        /* Column 1 */
        bcol = PANEL_COL_VEC((*right), 1, 0);
        c1 = _mm_add_ps(c1, _mm_mul_ps(a0, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(0))));
        c1 = _mm_add_ps(c1, _mm_mul_ps(a1, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(1))));
        c1 = _mm_add_ps(c1, _mm_mul_ps(a2, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(2))));
        c1 = _mm_add_ps(c1, _mm_mul_ps(a3, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(3))));

        /* Column 2 */
        bcol = PANEL_COL_VEC((*right), 2, 0);
        c2 = _mm_add_ps(c2, _mm_mul_ps(a0, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(0))));
        c2 = _mm_add_ps(c2, _mm_mul_ps(a1, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(1))));
        c2 = _mm_add_ps(c2, _mm_mul_ps(a2, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(2))));
        c2 = _mm_add_ps(c2, _mm_mul_ps(a3, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(3))));

        /* Column 3 */
        bcol = PANEL_COL_VEC((*right), 3, 0);
        c3 = _mm_add_ps(c3, _mm_mul_ps(a0, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(0))));
        c3 = _mm_add_ps(c3, _mm_mul_ps(a1, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(1))));
        c3 = _mm_add_ps(c3, _mm_mul_ps(a2, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(2))));
        c3 = _mm_add_ps(c3, _mm_mul_ps(a3, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(3))));

        PANEL_COL_VEC((*ans), 0, 0) = c0;
        PANEL_COL_VEC((*ans), 1, 0) = c1;
        PANEL_COL_VEC((*ans), 2, 0) = c2;
        PANEL_COL_VEC((*ans), 3, 0) = c3;
}

// ans = left * right + ans
// right is not transposed
DECLARE_PNL_PROC_LEFT_RIGHT_ANS(float, avx_fma, fma)
{
        static const __m256i idx0 = { 1, 1, 1, 1, 1, 1, 1, 1 };

        for (int col = 0; col < 8; ++col) {
                register __m256 bcol = _mm256_load_ps((float *)&right->vec[col][0]);
                register __m256 ccol = _mm256_load_ps((float *)&ans->vec[col][0]);
                register __m256i idx = _mm256_setzero_si256();
                register __m256i increament = _mm256_load_si256(&idx0);

                register __m256 a0 = _mm256_load_ps((float *)&left->vec[0][0]);
                register __m256 a1 = _mm256_load_ps((float *)&left->vec[1][0]);
                register __m256 a2 = _mm256_load_ps((float *)&left->vec[2][0]);
                register __m256 a3 = _mm256_load_ps((float *)&left->vec[3][0]);

                ccol = _mm256_fmadd_ps(a0, _mm256_permutevar8x32_ps(bcol, idx), ccol);
                idx = _mm256_add_epi32(idx, increament);
                ccol = _mm256_fmadd_ps(a1, _mm256_permutevar8x32_ps(bcol, idx), ccol);
                idx = _mm256_add_epi32(idx, increament);
                ccol = _mm256_fmadd_ps(a2, _mm256_permutevar8x32_ps(bcol, idx), ccol);
                idx = _mm256_add_epi32(idx, increament);
                ccol = _mm256_fmadd_ps(a3, _mm256_permutevar8x32_ps(bcol, idx), ccol);
                idx = _mm256_add_epi32(idx, increament);

                a0 = _mm256_load_ps((float *)&left->vec[4][0]);
                a1 = _mm256_load_ps((float *)&left->vec[5][0]);
                a2 = _mm256_load_ps((float *)&left->vec[6][0]);
                a3 = _mm256_load_ps((float *)&left->vec[7][0]);

                ccol = _mm256_fmadd_ps(a0, _mm256_permutevar8x32_ps(bcol, idx), ccol);
                idx = _mm256_add_epi32(idx, increament);
                ccol = _mm256_fmadd_ps(a1, _mm256_permutevar8x32_ps(bcol, idx), ccol);
                idx = _mm256_add_epi32(idx, increament);
                ccol = _mm256_fmadd_ps(a2, _mm256_permutevar8x32_ps(bcol, idx), ccol);
                idx = _mm256_add_epi32(idx, increament);
                ccol = _mm256_fmadd_ps(a3, _mm256_permutevar8x32_ps(bcol, idx), ccol);

                _mm256_store_ps((float *)&ans->vec[col][0], ccol);
        }
}

// ans = left * right + ans
// right is not transposed
DECLARE_PNL_PROC_LEFT_RIGHT_ANS(float, avx, fma)
{
        static const __m256i idx0 = { 1, 1, 1, 1, 1, 1, 1, 1 };

        for (int col = 0; col < 8; ++col) {
                register __m256 bcol = _mm256_load_ps((float *)&right->vec[col][0]);
                register __m256 ccol = _mm256_load_ps((float *)&ans->vec[col][0]);
                register __m256i idx = _mm256_setzero_si256();
                register __m256i increament = _mm256_load_si256(&idx0);

                register __m256 a0 = _mm256_load_ps((float *)&left->vec[0][0]);
                register __m256 a1 = _mm256_load_ps((float *)&left->vec[1][0]);
                register __m256 a2 = _mm256_load_ps((float *)&left->vec[2][0]);
                register __m256 a3 = _mm256_load_ps((float *)&left->vec[3][0]);

                ccol = _mm256_add_ps(ccol, _mm256_mul_ps(a0, _mm256_permutevar8x32_ps(bcol, idx)));
                idx = _mm256_add_epi32(idx, increament);
                ccol = _mm256_add_ps(ccol, _mm256_mul_ps(a1, _mm256_permutevar8x32_ps(bcol, idx)));
                idx = _mm256_add_epi32(idx, increament);
                ccol = _mm256_add_ps(ccol, _mm256_mul_ps(a2, _mm256_permutevar8x32_ps(bcol, idx)));
                idx = _mm256_add_epi32(idx, increament);
                ccol = _mm256_add_ps(ccol, _mm256_mul_ps(a3, _mm256_permutevar8x32_ps(bcol, idx)));
                idx = _mm256_add_epi32(idx, increament);

                a0 = _mm256_load_ps((float *)&left->vec[4][0]);
                a1 = _mm256_load_ps((float *)&left->vec[5][0]);
                a2 = _mm256_load_ps((float *)&left->vec[6][0]);
                a3 = _mm256_load_ps((float *)&left->vec[7][0]);

                ccol = _mm256_add_ps(ccol, _mm256_mul_ps(a0, _mm256_permutevar8x32_ps(bcol, idx)));
                idx = _mm256_add_epi32(idx, increament);
                ccol = _mm256_add_ps(ccol, _mm256_mul_ps(a1, _mm256_permutevar8x32_ps(bcol, idx)));
                idx = _mm256_add_epi32(idx, increament);
                ccol = _mm256_add_ps(ccol, _mm256_mul_ps(a2, _mm256_permutevar8x32_ps(bcol, idx)));
                idx = _mm256_add_epi32(idx, increament);
                ccol = _mm256_add_ps(ccol, _mm256_mul_ps(a3, _mm256_permutevar8x32_ps(bcol, idx)));

                _mm256_store_ps((float *)&ans->vec[col][0], ccol);
        }
}

DECLARE_PNL_PROC_LEFT_RIGHT_ANS(float, 512, fma)
{
        const __m512i ones = _mm512_set1_epi32(1);

        for (int col = 0; col < 8; ++col) {
                register __m512 bcol = _mm512_load_ps((float *)&right->vec[col][0]);
                register __m512 ccol = _mm512_load_ps((float *)&ans->vec[col][0]);
                register __m512i idx = _mm512_setzero_si512();
                register __m512i increament = _mm512_load_si512(&ones);

                register __m512 a0 = _mm512_load_ps((float *)&left->vec[0][0]);
                register __m512 a1 = _mm512_load_ps((float *)&left->vec[1][0]);
                register __m512 a2 = _mm512_load_ps((float *)&left->vec[2][0]);
                register __m512 a3 = _mm512_load_ps((float *)&left->vec[3][0]);
                register __m512 a4 = _mm512_load_ps((float *)&left->vec[4][0]);
                register __m512 a5 = _mm512_load_ps((float *)&left->vec[5][0]);
                register __m512 a6 = _mm512_load_ps((float *)&left->vec[6][0]);
                register __m512 a7 = _mm512_load_ps((float *)&left->vec[7][0]);

                ccol = _mm512_fmadd_ps(a0, _mm512_permutexvar_ps(idx, bcol), ccol);
                idx = _mm512_add_epi32(idx, increament);
                ccol = _mm512_fmadd_ps(a1, _mm512_permutexvar_ps(idx, bcol), ccol);
                idx = _mm512_add_epi32(idx, increament);
                ccol = _mm512_fmadd_ps(a2, _mm512_permutexvar_ps(idx, bcol), ccol);
                idx = _mm512_add_epi32(idx, increament);
                ccol = _mm512_fmadd_ps(a3, _mm512_permutexvar_ps(idx, bcol), ccol);
                idx = _mm512_add_epi32(idx, increament);
                ccol = _mm512_fmadd_ps(a4, _mm512_permutexvar_ps(idx, bcol), ccol);
                idx = _mm512_add_epi32(idx, increament);
                ccol = _mm512_fmadd_ps(a5, _mm512_permutexvar_ps(idx, bcol), ccol);
                idx = _mm512_add_epi32(idx, increament);
                ccol = _mm512_fmadd_ps(a6, _mm512_permutexvar_ps(idx, bcol), ccol);
                idx = _mm512_add_epi32(idx, increament);
                ccol = _mm512_fmadd_ps(a7, _mm512_permutexvar_ps(idx, bcol), ccol);
                idx = _mm512_add_epi32(idx, increament);

                a0 = _mm512_load_ps((float *)&left->vec[8][0]);
                a1 = _mm512_load_ps((float *)&left->vec[9][0]);
                a2 = _mm512_load_ps((float *)&left->vec[10][0]);
                a3 = _mm512_load_ps((float *)&left->vec[11][0]);
                a4 = _mm512_load_ps((float *)&left->vec[12][0]);
                a5 = _mm512_load_ps((float *)&left->vec[13][0]);
                a6 = _mm512_load_ps((float *)&left->vec[14][0]);
                a7 = _mm512_load_ps((float *)&left->vec[15][0]);

                ccol = _mm512_fmadd_ps(a0, _mm512_permutexvar_ps(idx, bcol), ccol);
                idx = _mm512_add_epi32(idx, increament);
                ccol = _mm512_fmadd_ps(a1, _mm512_permutexvar_ps(idx, bcol), ccol);
                idx = _mm512_add_epi32(idx, increament);
                ccol = _mm512_fmadd_ps(a2, _mm512_permutexvar_ps(idx, bcol), ccol);
                idx = _mm512_add_epi32(idx, increament);
                ccol = _mm512_fmadd_ps(a3, _mm512_permutexvar_ps(idx, bcol), ccol);
                idx = _mm512_add_epi32(idx, increament);
                ccol = _mm512_fmadd_ps(a4, _mm512_permutexvar_ps(idx, bcol), ccol);
                idx = _mm512_add_epi32(idx, increament);
                ccol = _mm512_fmadd_ps(a5, _mm512_permutexvar_ps(idx, bcol), ccol);
                idx = _mm512_add_epi32(idx, increament);
                ccol = _mm512_fmadd_ps(a6, _mm512_permutexvar_ps(idx, bcol), ccol);
                idx = _mm512_add_epi32(idx, increament);
                ccol = _mm512_fmadd_ps(a7, _mm512_permutexvar_ps(idx, bcol), ccol);
                idx = _mm512_add_epi32(idx, increament);

                _mm512_store_ps((float *)&ans->vec[col][0], ccol);
        }
}
