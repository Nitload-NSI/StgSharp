#include "sn_target.h"

#if SN_IS_ARCH(SN_ARCH_X86_64)

#include "sn_intrinsic.h"
#include <math.h>

static const float quality_threshold = 1e-6f;

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

#define DOT_F32_SSE(a, b, ans)                                               \
        do {                                                                 \
                __m128 _p = _mm_mul_ps((a), (b)); /* p0 p1 p2 p3 */          \
                __m128 _t = _mm_shuffle_ps(_p, _p, _MM_SHUFFLE(2, 3, 0, 1)); \
                _p = _mm_add_ps(_p, _t);                                     \
                _t = _mm_movehl_ps(_t, _p);                                  \
                ans = _mm_cvtss_f32(_mm_add_ss(_p, _t));                        \
        } while (0)

#define FIND_MAX_F32_SSE(a, b)                                      \
        do {                                                        \
                __m128 _t = _mm_movehl_ps(a, a);                    \
                a = _mm_max_ps(a, _t);                              \
                _t = _mm_shuffle_ps(a, a, _MM_SHUFFLE(2, 3, 0, 1)); \
                b = _mm_max_ps(a, _t);                              \
        } while (0)

/*
* layout of scalarpack:
* 
*/
DECLARE_KER_PROC_RIGHT_ANS(float, sse, , quality)
{
        static const __m128 tiny_source = { .m128_f32 = { 1e-6f, 1e-6f, 1e-6f, 1e-6f } };

        register __m128 tiny = _mm_load_ps(tiny_source.m128_f32);

        register __m128 col0 = _mm_load_ps((float *)&right->f32_x[0]);
        register __m128 col1 = _mm_load_ps((float *)&right->f32_x[1]);
        register __m128 col2 = _mm_load_ps((float *)&right->f32_x[2]);
        register __m128 col3 = _mm_load_ps((float *)&right->f32_x[3]);

        __m128 self_dot_src = { 0 };
        // try in reigister mix later
        DOT_F32_SSE(col0, col0, self_dot_src.m128_f32[0]);
        DOT_F32_SSE(col1, col1, self_dot_src.m128_f32[1]);
        DOT_F32_SSE(col2, col2, self_dot_src.m128_f32[2]);
        DOT_F32_SSE(col3, col3, self_dot_src.m128_f32[3]);

        register __m128 self_dot = _mm_load_ps(self_dot_src.m128_f32);

        register __m128 max;
        FIND_MAX_F32_SSE(self_dot, max);
        //float norm = _mm_cvtss_f32(max);

        // min on cross
        register __m128 col_shuff_1 = _mm_shuffle_ps(col1, col1, _MM_SHUFFLE(0, 3, 2, 1));
        register __m128 col_shuff_2 = _mm_shuffle_ps(col2, col2, _MM_SHUFFLE(1, 0, 3, 2));
        register __m128 col_shuff_3 = _mm_shuffle_ps(col3, col3, _MM_SHUFFLE(2, 1, 0, 3));
        register __m128 min = _mm_min_ps(col0, col_shuff_1);
        register __m128 min0 = _mm_min_ps(col_shuff_2, col_shuff_3);
        min = _mm_min_ps(min, min0);

        float quality = _mm_cvtss_f32(_mm_div_ps(min, _mm_add_ps(max, tiny)));
        ans->f32_x[0].m128_f32[0] = quality;
        if (quality < quality_threshold) {
                return;
        }

        __m128 cross_dot_src[2];
        DOT_F32_SSE(col0, col1, cross_dot_src[0].m128_f32[0]);
        DOT_F32_SSE(col0, col2, cross_dot_src[0].m128_f32[1]);
        DOT_F32_SSE(col0, col3, cross_dot_src[0].m128_f32[2]);
        DOT_F32_SSE(col1, col2, cross_dot_src[0].m128_f32[3]);
        DOT_F32_SSE(col1, col3, cross_dot_src[1].m128_f32[0]);
        DOT_F32_SSE(col2, col3, cross_dot_src[1].m128_f32[1]);

        register __m128 cross_dot_0 = _mm_load_ps(cross_dot_src[0].m128_f32);
        register __m128 cross_dot_1 = _mm_load_ps(cross_dot_src[1].m128_f32);

        register __m128 base_0 = _mm_shuffle_ps(self_dot, self_dot, _MM_SHUFFLE(1, 0, 0, 0));
        register __m128 base_1 = _mm_shuffle_ps(self_dot, self_dot, _MM_SHUFFLE(3, 3, 2, 1));

        register __m128 relation_0 =
                _mm_div_ps(cross_dot_0, _mm_add_ps(_mm_mul_ps(base_0, base_1), tiny));

        base_0 = _mm_shuffle_ps(self_dot, self_dot, _MM_SHUFFLE(2, 3, 2, 1));
        base_1 = _mm_shuffle_ps(self_dot, self_dot, _MM_SHUFFLE(3, 3, 3, 3));

        register __m128 relation_1 =
                _mm_div_ps(cross_dot_0, _mm_add_ps(_mm_mul_ps(base_0, base_1), tiny));

        relation_0 = _mm_max_ps(relation_0, relation_1);
        FIND_MAX_F32_SSE(relation_0, relation_0);
        ans->f32_x[0].m128_f32[1] = _mm_cvtss_f32(relation_0);
}

#endif
