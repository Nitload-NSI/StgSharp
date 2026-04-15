#include "sn_target.h"

#if SN_IS_ARCH(SN_ARCH_X86_64)

#include "sn_intrinsic.h"
#include "sn_intrinsic.context.x86.h"
#include "sn_intrinsic.std.h"

// Matrix transpose implementations
SN_MK_PROC_DECL_STD(float, sse, , transpose)
{
        __mk_param_std(KERNEL, RIGHT_ANS, float);

        register __m128 col0 = right->f32_x[0];
        register __m128 col1 = right->f32_x[1];
        register __m128 col2 = right->f32_x[2];
        register __m128 col3 = right->f32_x[3];
        register __m128 tmp0 = _mm_unpacklo_ps(col0, col1);
        register __m128 tmp1 = _mm_unpackhi_ps(col0, col1);
        register __m128 tmp2 = _mm_unpacklo_ps(col2, col3);
        register __m128 tmp3 = _mm_unpackhi_ps(col2, col3);

        ans->f32_x[0] = _mm_movelh_ps(tmp0, tmp2);
        ans->f32_x[1] = _mm_movehl_ps(tmp2, tmp0);
        ans->f32_x[2] = _mm_movelh_ps(tmp1, tmp3);
        ans->f32_x[3] = _mm_movehl_ps(tmp3, tmp1);
}

SN_MK_PROC_DECL_STD(float, avx, , transpose)
{
        __mk_param_std(KERNEL, RIGHT_ANS, float);
        register __m256 col_low = right->f32_y[0];
        register __m256 col_high = right->f32_y[1];

        register __m256 unpack_low = _mm256_unpacklo_ps(col_low, col_high);
        register __m256 unpack_high = _mm256_unpackhi_ps(col_low, col_high);

        col_low = _mm256_permute2f128_ps(unpack_low, unpack_high, 0x20);
        col_high = _mm256_permute2f128_ps(unpack_low, unpack_high, 0x31);

        ans->f32_y[0] = _mm256_shuffle_ps(col_high, col_low, _MM_SHUFFLE(2, 0, 2, 0));
        ans->f32_y[1] = _mm256_shuffle_ps(col_high, col_low, _MM_SHUFFLE(3, 1, 3, 1));
}

SN_MK_PROC_DECL_STD(float, 512, , transpose)
{
        __mk_param_std(KERNEL, RIGHT_ANS, float);
        register __m512 matrix = right->f32_z[0];

        const __m512i transpose_indices =
                _mm512_setr_epi32(0, 4, 8, 12, 1, 5, 9, 13, 2, 6, 10, 14, 3, 7, 11, 15);

        ans->f32_z[0] = _mm512_permutexvar_ps(transpose_indices, matrix);
}

#endif
