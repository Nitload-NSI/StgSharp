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
