#include "sn_target.h"

#if SN_IS_ARCH(SN_ARCH_X86_64)

#include <immintrin.h>
#include "sn_intrinsic.h"
#include "sn_intrinsic.std.h"
#include "sn_intrinsic.context.x86.h"

SN_MK_PROC_DECL_STD(float, sse, , fill)
{
        __mk_param_std(ANS_SCALAR, float);
        register __m128 vec = _mm_set_ps1(scalar.f32[0]);
        for (size_t i = 0; i < count_2; i++) {
                ans[i].x[0] = vec;
                ans[i].x[1] = vec;
                ans[i].x[2] = vec;
                ans[i].x[3] = vec;
        }
}

SN_MK_PROC_DECL_STD(float, avx, , fill)
{
        __mk_param_std(ANS_SCALAR, float);
        register __m256 vec = _mm256_broadcast_ss(&scalar.f32[0]);
        for (size_t i = 0; i < count_2; i++) {
                ans[i].y[0] = vec;
                ans[i].y[1] = vec;
        }
}

SN_MK_PROC_DECL_STD(float, 512, , fill)
{
        __mk_param_std(ANS_SCALAR, float);
        register __m512 vec = _mm512_broadcastss_ps(scalar.m128);
        for (size_t i = 0; i < count_2; i++) {
                ans[i].z[0] = vec;
        }
}

SN_MK_PROC_DECL_STD(double, sse, , fill)
{
        __mk_param_std(ANS_SCALAR, double);
        register __m128d vec_d = _mm_set1_pd(scalar.f64[0]);
        register __m128 vec = _mm_castpd_ps(vec_d);
        for (size_t i = 0; i < count_2; i++) {
                ans[i].x[0] = vec;
                ans[i].x[1] = vec;
                ans[i].x[2] = vec;
                ans[i].x[3] = vec;
                ans[i].x[4] = vec;
                ans[i].x[5] = vec;
                ans[i].x[6] = vec;
                ans[i].x[7] = vec;
        }
}

SN_MK_PROC_DECL_STD(double, avx, , fill)
{
        __mk_param_std(ANS_SCALAR, double);
        register __m256d vec = _mm256_broadcast_sd(&scalar.f64[0]);
        for (size_t i = 0; i < count_2; i++) {
                ans[i].y[0] = _mm256_castpd_ps(vec);
                ans[i].y[1] = _mm256_castpd_ps( vec);
                ans[i].y[2] = _mm256_castpd_ps( vec);
                ans[i].y[3] = _mm256_castpd_ps( vec);
        }
}

SN_MK_PROC_DECL_STD(double, 512, , fill)
{
        __mk_param_std(ANS_SCALAR, double);
        register __m512d vec = _mm512_set1_pd(scalar.f64[0]);
        for (size_t i = 0; i < count_2; i++) {
                ans[i].z[0] = _mm512_castpd_ps(vec);
                ans[i].z[1] = _mm512_castpd_ps(vec);
        }
}

SN_MK_PROC_DECL_STD(float, sse, , add)
{
        __mk_param_std(LEFT_RIGHT_ANS, float);

        for (size_t i = 0; i < count_0; i++) {
                register __m128 c0 = _mm_add_ps(left[i].x[0], right[i].x[0]);
                register __m128 c1 = _mm_add_ps(left[i].x[1], right[i].x[1]);
                register __m128 c2 = _mm_add_ps(left[i].x[2], right[i].x[2]);
                register __m128 c3 = _mm_add_ps(left[i].x[3], right[i].x[3]);

                ans[i].x[0] = c0;
                ans[i].x[1] = c1;
                ans[i].x[2] = c2;
                ans[i].x[3] = c3;
        }
}

SN_MK_PROC_DECL_STD(float, avx, , add)
{
        __mk_param_std(LEFT_RIGHT_ANS, float);

        for (size_t i = 0; i < count_0; i++) {
                size_t s = sizeof(MAT_KERNEL(float));
                register __m256 c0 = _mm256_add_ps(left[i].y[0], right[i].y[0]);
                register __m256 c1 = _mm256_add_ps(left[i].y[1], right[i].y[1]);
                ans[i].y[0] = c0;
                ans[i].y[1] = c1;
        }
}

SN_MK_PROC_DECL_STD(float, 512, , add)
{
        __mk_param_std(LEFT_RIGHT_ANS, float);

        for (size_t i = 0; i < count_0; i++) {
                register __m512 c0 = _mm512_add_ps(left[i].z[0], right[i].z[0]);
                ans[i].z[0] = c0;
        }
}
SN_MK_PROC_DECL_STD(double, sse, , add)
{
        __mk_param_std( LEFT_RIGHT_ANS, double);

        for (size_t i = 0; i < count_0; i++) {
                register __m128d c0 = _mm_add_pd(_mm_castps_pd(left[i].x[0]),
                                                 _mm_castps_pd(right[i].x[0]));
                register __m128d c1 = _mm_add_pd(_mm_castps_pd(left[i].x[1]),
                                                 _mm_castps_pd(right[i].x[1]));
                register __m128d c2 = _mm_add_pd(_mm_castps_pd(left[i].x[2]),
                                                 _mm_castps_pd(right[i].x[2]));
                register __m128d c3 = _mm_add_pd(_mm_castps_pd(left[i].x[3]),
                                                 _mm_castps_pd(right[i].x[3]));
                register __m128d c4 = _mm_add_pd(_mm_castps_pd(left[i].x[4]),
                                                 _mm_castps_pd(right[i].x[4]));
                register __m128d c5 = _mm_add_pd(_mm_castps_pd(left[i].x[5]),
                                                 _mm_castps_pd(right[i].x[5]));
                register __m128d c6 = _mm_add_pd(_mm_castps_pd(left[i].x[6]),
                                                 _mm_castps_pd(right[i].x[6]));
                register __m128d c7 = _mm_add_pd(_mm_castps_pd(left[i].x[7]),
                                                 _mm_castps_pd(right[i].x[7]));

                ans[i].x[0] = _mm_castpd_ps(c0);
                ans[i].x[1] = _mm_castpd_ps(c1);
                ans[i].x[2] = _mm_castpd_ps(c2);
                ans[i].x[3] = _mm_castpd_ps(c3);
                ans[i].x[4] = _mm_castpd_ps(c4);
                ans[i].x[5] = _mm_castpd_ps(c5);
                ans[i].x[6] = _mm_castpd_ps(c6);
                ans[i].x[7] = _mm_castpd_ps(c7);
        }
}

SN_MK_PROC_DECL_STD(double, avx, , add)
{
        __mk_param_std( LEFT_RIGHT_ANS, double);

        for (size_t i = 0; i < count_0; i++) {
                size_t s = sizeof(MAT_KERNEL(float));
                register __m256d c0 = _mm256_add_pd(left[i].y[0], right[i].y[0]);
                register __m256d c1 = _mm256_add_pd(left[i].y[1], right[i].y[1]);
                register __m256d c2 = _mm256_add_pd(left[i].y[0], right[i].y[0]);
                register __m256d c3 = _mm256_add_pd(left[i].y[1], right[i].y[1]);
                ans[i].y[0] = c0;
                ans[i].y[1] = c1;
                ans[i].y[2] = c2;
                ans[i].y[3] = c3;
        }
}

SN_MK_PROC_DECL_STD(double, 512, , add)
{
        __mk_param_std( LEFT_RIGHT_ANS, double);

        for (size_t i = 0; i < count_0; i++) {
                register __m512d c0 = _mm512_add_pd(_mm512_castps_pd( left[i].z[0]), _mm512_castps_pd( right[i].z[0]));
                register __m512d c1 = _mm512_add_pd(_mm512_castps_pd( left[i].z[1]), _mm512_castps_pd( right[i].z[1]));
                ans[i].z[0] = c0;
                ans[i].z[1] = c1;
        }
}

SN_MK_PROC_DECL_STD(float, sse, , sub)
{
        __mk_param_std( LEFT_RIGHT_ANS, float);

        for (size_t i = 0; i < count_0; i++) {
                register __m128 c0 = _mm_sub_ps(left[i].x[0], right[i].x[0]);
                register __m128 c1 = _mm_sub_ps(left[i].x[1], right[i].x[1]);
                register __m128 c2 = _mm_sub_ps(left[i].x[2], right[i].x[2]);
                register __m128 c3 = _mm_sub_ps(left[i].x[3], right[i].x[3]);

                ans[i].x[0] = c0;
                ans[i].x[1] = c1;
                ans[i].x[2] = c2;
                ans[i].x[3] = c3;
        }
}

SN_MK_PROC_DECL_STD(float, avx, , sub)
{
        __mk_param_std( LEFT_RIGHT_ANS, float);

        for (size_t i = 0; i < count_0; i++) {
                register __m256 c0 = _mm256_sub_ps(left[i].y[0], right[i].y[0]);
                register __m256 c1 = _mm256_sub_ps(left[i].y[1], right[i].y[1]);
                ans[i].y[0] = c0;
                ans[i].y[1] = c1;
        }
}

SN_MK_PROC_DECL_STD(float, 512, , sub)
{
        __mk_param_std( LEFT_RIGHT_ANS, float);

        for (size_t i = 0; i < count_0; i++) {
                register __m512 c0 = _mm512_sub_ps(left[i].z[0], right[i].z[0]);
                ans[i].z[0] = c0;
        }
}

SN_MK_PROC_DECL_STD(double, sse, , sub)
{
        __mk_param_std( LEFT_RIGHT_ANS, double);

        for (size_t i = 0; i < count_0; i++) {
                register __m128d c0 = _mm_sub_pd(left[i].x[0], right[i].x[0]);
                register __m128d c1 = _mm_sub_pd(left[i].x[1], right[i].x[1]);
                register __m128d c2 = _mm_sub_pd(left[i].x[2], right[i].x[2]);
                register __m128d c3 = _mm_sub_pd(left[i].x[3], right[i].x[3]);
                register __m128d c4 = _mm_sub_pd(left[i].x[4], right[i].x[4]);
                register __m128d c5 = _mm_sub_pd(left[i].x[5], right[i].x[5]);
                register __m128d c6 = _mm_sub_pd(left[i].x[6], right[i].x[6]);
                register __m128d c7 = _mm_sub_pd(left[i].x[7], right[i].x[7]);

                ans[i].x[0] = c0;
                ans[i].x[1] = c1;
                ans[i].x[2] = c2;
                ans[i].x[3] = c3;
                ans[i].x[4] = c4;
                ans[i].x[5] = c5;
                ans[i].x[6] = c6;
                ans[i].x[7] = c7;
        }
}

SN_MK_PROC_DECL_STD(double, avx, , sub)
{
        __mk_param_std( LEFT_RIGHT_ANS, double);

        for (size_t i = 0; i < count_0; i++) {
                register __m256 c0 = _mm256_sub_ps(left[i].y[0], right[i].y[0]);
                register __m256 c1 = _mm256_sub_ps(left[i].y[1], right[i].y[1]);
                ans[i].y[0] = c0;
                ans[i].y[1] = c1;
        }
}

SN_MK_PROC_DECL_STD(double, 512, , sub)
{
        __mk_param_std( LEFT_RIGHT_ANS, double);

        for (size_t i = 0; i < count_0; i++) {
                register __m512 c0 = _mm512_sub_ps(left[i].z[0], right[i].z[0]);
                ans[i].z[0] = c0;
        }
}

SN_MK_PROC_DECL_STD(float, sse, , scalar_mul)
{
        __mk_param_std( RIGHT_ANS_SCALAR, float);

        register __m128 vec = _mm_set_ps1(scalar.f32[0]);
        for (size_t i = 0; i < count_1; i++) {
                register __m128 c0 = _mm_mul_ps(right[i].x[0], vec);
                register __m128 c1 = _mm_mul_ps(right[i].x[1], vec);
                register __m128 c2 = _mm_mul_ps(right[i].x[2], vec);
                register __m128 c3 = _mm_mul_ps(right[i].x[3], vec);

                ans[i].x[0] = c0;
                ans[i].x[1] = c1;
                ans[i].x[2] = c2;
                ans[i].x[3] = c3;
        }
}

SN_MK_PROC_DECL_STD(float, avx, , scalar_mul)
{
        __mk_param_std( RIGHT_ANS_SCALAR, float);

        register __m256 vec = _mm256_broadcast_ss(&scalar.f32[0]);
        for (size_t i = 0; i < count_1; i++) {
                register __m256 c0 = _mm256_mul_ps(right[i].y[0], vec);
                register __m256 c1 = _mm256_mul_ps(right[i].y[1], vec);

                ans[i].y[0] = c0;
                ans[i].y[1] = c1;
        }
}

SN_MK_PROC_DECL_STD(float, 512, , scalar_mul)
{
        __mk_param_std( RIGHT_ANS_SCALAR, float);

        register __m512 vec = _mm512_broadcastss_ps(scalar.m128);
        for (size_t i = 0; i < count_1; i++) {
                register __m512 c0 = _mm512_mul_ps(right[i].z[0], vec);

                ans[i].z[0] = c0;
        }
}

SN_MK_PROC_DECL_STD(double, sse, , scalar_mul)
{
        __mk_param_std( RIGHT_ANS_SCALAR, double);

        register __m128d vec = _mm_set1_pd(scalar.f64[0]);
        for (size_t i = 0; i < count_1; i++) {
                register __m128d c0 = _mm_mul_pd(right[i].x[0], vec);
                register __m128d c1 = _mm_mul_pd(right[i].x[1], vec);
                register __m128d c2 = _mm_mul_pd(right[i].x[2], vec);
                register __m128d c3 = _mm_mul_pd(right[i].x[3], vec);
                register __m128d c4 = _mm_mul_pd(right[i].x[4], vec);
                register __m128d c5 = _mm_mul_pd(right[i].x[5], vec);
                register __m128d c6 = _mm_mul_pd(right[i].x[6], vec);
                register __m128d c7 = _mm_mul_pd(right[i].x[7], vec);

                ans[i].x[0] = c0;
                ans[i].x[1] = c1;
                ans[i].x[2] = c2;
                ans[i].x[3] = c3;
                ans[i].x[4] = c4;
                ans[i].x[5] = c5;
                ans[i].x[6] = c6;
                ans[i].x[7] = c7;
        }
}

SN_MK_PROC_DECL_STD(double, avx, , scalar_mul)
{
        __mk_param_std( RIGHT_ANS_SCALAR, double);

        register __m256d vec = _mm256_broadcast_sd(&scalar.f64[0]);
        for (size_t i = 0; i < count_1; i++) {
                register __m256d c0 = _mm256_mul_pd(right[i].y[0], vec);
                register __m256d c1 = _mm256_mul_pd(right[i].y[1], vec);
                register __m256d c2 = _mm256_mul_pd(right[i].y[2], vec);
                register __m256d c3 = _mm256_mul_pd(right[i].y[3], vec);

                ans[i].y[0] = c0;
                ans[i].y[1] = c1;
                ans[i].y[2] = c2;
                ans[i].y[3] = c3;
        }
}

SN_MK_PROC_DECL_STD(double, 512, , scalar_mul)
{
        __mk_param_std( RIGHT_ANS_SCALAR, double);

        register __m512d vec = _mm512_set1_pd(scalar.f64[0]);
        for (size_t i = 0; i < count_1; i++) {
                register __m512d c0 = _mm512_mul_pd(right[i].z[0], vec);
                register __m512d c1 = _mm512_mul_pd(right[i].z[1], vec);

                ans[i].z[0] = c0;
                ans[i].z[1] = c1;
        }
}

#endif
