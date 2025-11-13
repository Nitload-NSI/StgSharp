#include "sn_intrinsic.h"

// ans = left * right + ans
// right is not transposed
DECLARE_KER_PROC_LEFT_RIGHT_ANS(float, sse, fma)
{
        register __m128 source0 = _mm_load_ps(&left->xmm[0]);
        register __m128 source1 = _mm_load_ps(&left->xmm[1]);
        register __m128 source2 = _mm_load_ps(&left->xmm[2]);
        register __m128 source3 = _mm_load_ps(&left->xmm[3]);

        register __m128 ans0 = _mm_load_ps(&ans->xmm[0]);
        register __m128 ans1 = _mm_load_ps(&ans->xmm[1]);
        register __m128 ans2 = _mm_load_ps(&ans->xmm[2]);
        register __m128 ans3 = _mm_load_ps(&ans->xmm[3]);

        register __m128 broadcast;
        for (size_t i = 0; i < 4; i++) {
                broadcast = _mm_load_ps(&right->xmm[i]);

                ans0 = _mm_add_ps(ans0,
                                  _mm_mul_ps(source0, _mm_shuffle_ps(broadcast, broadcast, 0x00)));
                ans1 = _mm_add_ps(ans1,
                                  _mm_mul_ps(source1, _mm_shuffle_ps(broadcast, broadcast, 0x55)));
                ans2 = _mm_add_ps(ans2,
                                  _mm_mul_ps(source2, _mm_shuffle_ps(broadcast, broadcast, 0xAA)));
                ans3 = _mm_add_ps(ans3,
                                  _mm_mul_ps(source3, _mm_shuffle_ps(broadcast, broadcast, 0xFF)));
        }

        _mm_store_ps(&ans->xmm[0], ans0);
        _mm_store_ps(&ans->xmm[1], ans1);
        _mm_store_ps(&ans->xmm[2], ans2);
        _mm_store_ps(&ans->xmm[3], ans3);
}
