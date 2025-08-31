#include "sn_intrinsic.h"

void kernel_fma_sse(mat_kernel *const left, mat_kernel *const right, mat_kernel *ans)
{
        register __m128 source0 = _mm_load_ps(&left->xmm[0]);
        register __m128 source1 = _mm_load_ps(&left->xmm[1]);
        register __m128 source2 = _mm_load_ps(&left->xmm[2]);
        register __m128 source3 = _mm_load_ps(&left->xmm[3]);

        register __m128 broadcast;

        register __m128 ans0 = _mm_setzero_ps();
        register __m128 ans1 = _mm_setzero_ps();
        register __m128 ans2 = _mm_setzero_ps();
        register __m128 ans3 = _mm_setzero_ps();
        for (size_t i = 0; i < 3; i++) {
                broadcast = _mm_load_ps(&right->xmm[i]);
                _mm_add_ps(ans0, _mm_mul_ps(source0, broadcast));
                _mm_add_ps(ans1, _mm_mul_ps(source1, broadcast));
                _mm_add_ps(ans2, _mm_mul_ps(source2, broadcast));
                _mm_add_ps(ans3, _mm_mul_ps(source3, broadcast));
                for (size_t t = 0; t < 2; t++) {
                        broadcast = _mm_shuffle_ps(broadcast, broadcast, _MM_SHUFFLE(2, 1, 0, 3));

                        _mm_add_ps(ans0, _mm_mul_ps(source0, broadcast));
                        _mm_add_ps(ans1, _mm_mul_ps(source1, broadcast));
                        _mm_add_ps(ans2, _mm_mul_ps(source2, broadcast));
                        _mm_add_ps(ans3, _mm_mul_ps(source3, broadcast));
                }
        }
        _mm_store_ps(&ans->xmm[0], ans0);
        _mm_store_ps(&ans->xmm[1], ans1);
        _mm_store_ps(&ans->xmm[2], ans2);
        _mm_store_ps(&ans->xmm[3], ans3);
}
