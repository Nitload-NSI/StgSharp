#include "sn_target.h"

#if SN_IS_ARCH(SN_ARCH_X86_64)

#include "sn_intrinsic.h"
#include <math.h>

DECLARE_KER_PROC_LEFT_RIGHT_ANS_SCALAR(float, sse, , try_plu)
{
        /* Aliases */
        MAT_KERNEL(float) *upper = left; /* U */
        MAT_KERNEL(float) *lower = right; /* L (unit diag) */
        MAT_KERNEL(float) *source = ans; /* A */
        unsigned char *pivot = (unsigned char *)&scalar->data[0]; /* length = 4 */
        int *success = (int *)&scalar->data[1];

        /* Init permutation and outputs */
        pivot[0] = 0;
        pivot[1] = 1;
        pivot[2] = 2;
        pivot[3] = 3;
        *success = 1;

        /* Working copy of A in column-major a[col][row], load via SIMD view */
        MAT_KERNEL(float) a;
        const __m128 z = _mm_setzero_ps();
        for (int c = 0; c < 4; ++c) {
                __m128 col = _mm_load_ps((float *)&source->f32_x[c]);
                _mm_storeu_ps((float *)&a.f32_x[c], col);
                _mm_store_ps((float *)&upper->f32_x[c], z);
                _mm_store_ps((float *)&lower->f32_x[c], z);
        }

        const float eps = 1e-12f;

        for (int k = 0; k < 4; ++k) {
                /* Pivot search on column k, rows k..3 */
                int p = k;
                float maxabs = fabsf(a.m[k][p]);
                for (int r = k + 1; r < 4; ++r) {
                        float v = fabsf(a.m[k][r]);
                        if (v > maxabs) {
                                maxabs = v;
                                p = r;
                        }
                }

                if (maxabs < eps) {
                        *success = 0;
                        /* leave outputs zeroed */
                        return;
                }

                /* Swap rows if needed */
                if (p != k) {
                        for (int c = 0; c < 4; ++c) {
                                float tmp = a.m[c][k];
                                a.m[c][k] = a.m[c][p];
                                a.m[c][p] = tmp;
                        }
                        unsigned char tmpi = pivot[k];
                        pivot[k] = pivot[p];
                        pivot[p] = tmpi;
                }

                float pivot_val = a.m[k][k];

                /* Eliminate below pivot: store multipliers in L column k */
                for (int r = k + 1; r < 4; ++r) {
                        float factor = a.m[k][r] / pivot_val;
                        lower->m[k][r] = factor;
                        for (int c = k; c < 4; ++c) {
                                a.m[c][r] -= factor * a.m[c][k];
                        }
                }

                /* Set U row k (columns k..3) and L diagonal */
                lower->m[k][k] = 1.0f;
                for (int c = k; c < 4; ++c) {
                        upper->m[c][k] = a.m[c][k];
                }
        }

        /* Ensure remaining diagonal ones for L */
        for (int d = 0; d < 4; ++d) {
                if (lower->m[d][d] == 0.0f)
                        lower->m[d][d] = 1.0f;
        }
}

DECLARE_KER_PROC_RIGHT_ANS(float, sse, , quality)
{
        static const __m128 abs_mask_source = { .m128_i32 = { 0x80000000, 0x80000000, 0x80000000,
                                                              0x80000000 } };
        register __m128 abs_mask = _mm_load_ps(abs_mask_source.m128_i32);

        register __m128 col0 = _mm_andnot_ps(abs_mask, _mm_load_ps((float *)&right->f32_x[0]));
        register __m128 col1 = _mm_andnot_ps(abs_mask, _mm_load_ps((float *)&right->f32_x[1]));
        register __m128 col2 = _mm_andnot_ps(abs_mask, _mm_load_ps((float *)&right->f32_x[2]));
        register __m128 col3 = _mm_andnot_ps(abs_mask, _mm_load_ps((float *)&right->f32_x[3]));

        // norm
        register __m128 row_sum = _mm_add_ps(col0, col1);
        row_sum = _mm_add_ps(row_sum, col2);
        row_sum = _mm_add_ps(row_sum, col3);

        // max(0,2) and max(1,3)
        register __m128 max = _mm_max_ps(row_sum, _mm_movehl_ps(row_sum, row_sum));
        max = _mm_max_ps(max, _mm_shuffle_ps(max, max, _MM_SHUFFLE(2, 3, 0, 1)));
        float norm = _mm_cvtss_f32(max);

        ans->f32_x[0].m128_f32[0] = norm;

        // min on cross
        col1 = _mm_shuffle_ps(col1, col1, _MM_SHUFFLE(0, 3, 2, 1));
        col2 = _mm_shuffle_ps(col2, col2, _MM_SHUFFLE(1, 0, 3, 2));
        col3 = _mm_shuffle_ps(col3, col3, _MM_SHUFFLE(2, 1, 0, 3));
        max = _mm_min_ps(col0, col2);
        register __m128 max0 = _mm_min_ps(max, col1);
        max = _mm_min_ps(max0, max);

        float min_val = _mm_cvtss_f32(max);
        ans->f32_x[0].m128_f32[1] = min_val;

        float quality = min_val / (norm + 1e-20f);
        ans->f32_x[0].m128_f32[2] = quality;
}

#endif
