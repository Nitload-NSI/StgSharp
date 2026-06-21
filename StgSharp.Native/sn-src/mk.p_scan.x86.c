
#include "sn_target.h"

#if SN_IS_ARCH(SN_ARCH_X86_64)

#include "sn_internal.h"
#include "sn_intrinsic.context.x86.h"
#include "sn_intrinsic.std.h"
#include "sn_intrinsic.matkernel.h"
#include <emmintrin.h>
#include <immintrin.h>
#include <pmmintrin.h>

SN_MK_PROC_DECL_STD(float, sse, , k_quality)
{
        __mk_param_std(RIGHT_ANS_SCALAR, float);
}

INTERNAL SN_STD_TARGET_ATTR(sse, ) void kp_check_f32(mat_kernel *kernel)
{
        typedef MAT_KERNEL(float) SN_MAT;
        SN_MAT const *k = (SN_MAT const *)kernel;
        float cross[4];
        cross[0] = k->m[0][0];
        cross[1] = k->m[1][1];
        cross[2] = k->m[2][2];
        cross[3] = k->m[3][3];

        float evaluation[4];

        register __m128 cross_vec = _mm_loadu_ps((float *)cross);
        register __m128 abs_mask = _mm_castsi128_ps(_mm_set1_epi32(0x7fffffff));
        cross_vec = _mm_and_ps(cross_vec, abs_mask);
        register __m128 average_vec = _mm_hadd_ps(cross_vec, cross_vec);
        average_vec = _mm_hadd_ps(average_vec, average_vec);
        register __m128 quarter_vec = _mm_set1_ps(0.25f);
        average_vec = _mm_mul_ps(average_vec, quarter_vec);

        register __m128 stddev_vec = _mm_sub_ps(cross_vec, average_vec);
        stddev_vec = _mm_mul_ps(stddev_vec, stddev_vec);
        stddev_vec = _mm_hadd_ps(stddev_vec, stddev_vec);
        stddev_vec = _mm_hadd_ps(stddev_vec, stddev_vec);
        stddev_vec = _mm_mul_ps(stddev_vec, quarter_vec);
        stddev_vec = _mm_sqrt_ps(stddev_vec);

        evaluation[0] = _mm_cvtss_f32(average_vec);
        evaluation[1] = _mm_cvtss_f32(stddev_vec);
}

#endif
