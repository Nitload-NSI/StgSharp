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

DECLARE_BUF_PROC_LEFT_RIGHT_ANS(float, sse, add)
{
        for (size_t i = 0; i < count; i++) {
                register __m128 c0 = _mm_add_ps(left[i].xmm[0], right[i].xmm[0]);
                register __m128 c1 = _mm_add_ps(left[i].xmm[1], right[i].xmm[1]);
                register __m128 c2 = _mm_add_ps(left[i].xmm[2], right[i].xmm[2]);
                register __m128 c3 = _mm_add_ps(left[i].xmm[3], right[i].xmm[3]);

                ans[i].xmm[0] = c0;
                ans[i].xmm[1] = c1;
                ans[i].xmm[2] = c2;
                ans[i].xmm[3] = c3;
        }
}

DECLARE_BUF_PROC_LEFT_RIGHT_ANS(float, avx, add)
{
        for (size_t i = 0; i < count; i++) {
                size_t s = sizeof(MAT_KERNEL(float));
                register __m256 c0 = _mm256_add_ps(left[i].ymm[0], right[i].ymm[0]);
                register __m256 c1 = _mm256_add_ps(left[i].ymm[1], right[i].ymm[1]);
                ans[i].ymm[0] = c0;
                ans[i].ymm[1] = c1;
        }
}

DECLARE_BUF_PROC_LEFT_RIGHT_ANS(float, 512, add)
{
        for (size_t i = 0; i < count; i++) {
                register __m512 c0 = _mm512_add_ps(left[i].zmm[0], right[i].zmm[0]);
                ans[i].zmm[0] = c0;
        }
}

DECLARE_BUF_PROC_LEFT_RIGHT_ANS(float, sse, sub)
{
        for (size_t i = 0; i < count; i++) {
                register __m128 c0 = _mm_sub_ps(left[i].xmm[0], right[i].xmm[0]);
                register __m128 c1 = _mm_sub_ps(left[i].xmm[1], right[i].xmm[1]);
                register __m128 c2 = _mm_sub_ps(left[i].xmm[2], right[i].xmm[2]);
                register __m128 c3 = _mm_sub_ps(left[i].xmm[3], right[i].xmm[3]);

                ans[i].xmm[0] = c0;
                ans[i].xmm[1] = c1;
                ans[i].xmm[2] = c2;
                ans[i].xmm[3] = c3;
        }
}

DECLARE_BUF_PROC_LEFT_RIGHT_ANS(float, avx, sub)
{
        for (size_t i = 0; i < count; i++) {
                register __m256 c0 = _mm256_sub_ps(left[i].ymm[0], right[i].ymm[0]);
                register __m256 c1 = _mm256_sub_ps(left[i].ymm[1], right[i].ymm[1]);
                ans[i].ymm[0] = c0;
                ans[i].ymm[1] = c1;
        }
}

DECLARE_BUF_PROC_LEFT_RIGHT_ANS(float, 512, sub)
{
        for (size_t i = 0; i < count; i++) {
                register __m512 c0 = _mm512_sub_ps(left[i].zmm[0], right[i].zmm[0]);
                ans[i].zmm[0] = c0;
        }
}

DECLARE_BUF_PROC_RIGHT_ANS_SCALAR(float, sse, scalar_mul)
{
        register __m128 vec = _mm_set_ps1(((float *)(scalar->data))[0]);
        for (size_t i = 0; i < count; i++) {
                register __m128 c0 = _mm_mul_ps(right[i].xmm[0], vec);
                register __m128 c1 = _mm_mul_ps(right[i].xmm[1], vec);
                register __m128 c2 = _mm_mul_ps(right[i].xmm[2], vec);
                register __m128 c3 = _mm_mul_ps(right[i].xmm[3], vec);

                ans[i].xmm[0] = c0;
                ans[i].xmm[1] = c1;
                ans[i].xmm[2] = c2;
                ans[i].xmm[3] = c3;
        }
}

DECLARE_BUF_PROC_RIGHT_ANS_SCALAR(float, avx, scalar_mul)
{
        register __m256 vec = _mm256_broadcast_ss(((float *)(scalar->data)));
        for (size_t i = 0; i < count; i++) {
                register __m256 c0 = _mm256_mul_ps(right[i].ymm[0], vec);
                register __m256 c1 = _mm256_mul_ps(right[i].ymm[1], vec);

                ans[i].ymm[0] = c0;
                ans[i].ymm[1] = c1;
        }
}

DECLARE_BUF_PROC_RIGHT_ANS_SCALAR(float, 512, scalar_mul)
{
        register __m512 vec = _mm512_broadcastss_ps(((__m128 *)(scalar->data))[0]);
        for (size_t i = 0; i < count; i++) {
                register __m512 c0 = _mm512_mul_ps(right[i].zmm[0], vec);

                ans[i].zmm[0] = c0;
        }
}
