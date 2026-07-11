#include "sn_target.h"
#include <immintrin.h>

#if SN_IS_ARCH(SN_ARCH_X86_64)

// #include "sn_intrinsic.h"
#include "sn_template.fma.x86.h"
#include "sn_intrinsic.std.h"
#include "sn_intrinsic.context.x86.h"

#include <stdio.h>

#define _FMA_SELECT(i) _MM_SHUFFLE(i, i, i, i)


SN_MK_PROC_DECL_STD(float, sse, , fma)
{
        __mk_param_std(LEFT_RIGHT_ANS, float);

        SN_MAT_KERNEL_LOCAL *restrict ans_cur = ans + offset_2;

        register __m128 c0 = _mm_load_ps((float *)&ans_cur->x[0]);
        register __m128 c1 = _mm_load_ps((float *)&ans_cur->x[1]);
        register __m128 c2 = _mm_load_ps((float *)&ans_cur->x[2]);
        register __m128 c3 = _mm_load_ps((float *)&ans_cur->x[3]);
        register __m128 bcol;

        for (size_t z = 0; z < count_0; z++) {
                SN_MAT_KERNEL_LOCAL const *right_base = right;
                SN_MAT_KERNEL_LOCAL const *left_base = left;

                const __m128 a0 = _mm_load_ps((float *)&left_base->x[0]);
                const __m128 a1 = _mm_load_ps((float *)&left_base->x[1]);
                const __m128 a2 = _mm_load_ps((float *)&left_base->x[2]);
                const __m128 a3 = _mm_load_ps((float *)&left_base->x[3]);

                /* Column 0 */
                bcol = _mm_load_ps((float *)&right_base->x[0]);
                c0 = _mm_add_ps(c0, _mm_mul_ps(a0, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(0))));
                c0 = _mm_add_ps(c0, _mm_mul_ps(a1, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(1))));
                c0 = _mm_add_ps(c0, _mm_mul_ps(a2, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(2))));
                c0 = _mm_add_ps(c0, _mm_mul_ps(a3, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(3))));

                /* Column 1 */
                bcol = _mm_load_ps((float *)&right_base->x[1]);
                c1 = _mm_add_ps(c1, _mm_mul_ps(a0, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(0))));
                c1 = _mm_add_ps(c1, _mm_mul_ps(a1, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(1))));
                c1 = _mm_add_ps(c1, _mm_mul_ps(a2, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(2))));
                c1 = _mm_add_ps(c1, _mm_mul_ps(a3, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(3))));

                /* Column 2 */
                bcol = _mm_load_ps((float *)&right_base->x[2]);
                c2 = _mm_add_ps(c2, _mm_mul_ps(a0, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(0))));
                c2 = _mm_add_ps(c2, _mm_mul_ps(a1, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(1))));
                c2 = _mm_add_ps(c2, _mm_mul_ps(a2, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(2))));
                c2 = _mm_add_ps(c2, _mm_mul_ps(a3, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(3))));

                /* Column 3 */
                bcol = _mm_load_ps((float *)&right_base->x[3]);
                c3 = _mm_add_ps(c3, _mm_mul_ps(a0, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(0))));
                c3 = _mm_add_ps(c3, _mm_mul_ps(a1, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(1))));
                c3 = _mm_add_ps(c3, _mm_mul_ps(a2, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(2))));
                c3 = _mm_add_ps(c3, _mm_mul_ps(a3, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(3))));
        }

        _mm_store_ps((float *)&ans_cur->x[0], c0);
        _mm_store_ps((float *)&ans_cur->x[1], c1);
        _mm_store_ps((float *)&ans_cur->x[2], c2);
        _mm_store_ps((float *)&ans_cur->x[3], c3);
}

SN_MK_PROC_DECL_STD(float, avx, , fma)
{
        __mk_param_std(LEFT_RIGHT_ANS, float);
        SN_MAT_KERNEL_LOCAL *restrict ans_cur = ans + offset_2;
        register __m256 c0 = _mm256_load_ps((float *)&ans_cur->y[0]);
        register __m256 c1 = _mm256_load_ps((float *)&ans_cur->y[1]);
        register __m256 c2 = _mm256_load_ps((float *)&ans_cur->y[2]);
        register __m256 c3 = _mm256_load_ps((float *)&ans_cur->y[3]);

        for (size_t z = 0; z < count_0; z++) {
                SN_MAT_KERNEL_LOCAL const *b_base = right;
                SN_MAT_KERNEL_LOCAL const *a_base = left;

                register __m256 b0 = _mm256_load_ps((float *)&b_base->y[0]);
                register __m256 b1 = _mm256_load_ps((float *)&b_base->y[1]);
                register __m256 b2 = _mm256_load_ps((float *)&b_base->y[2]);
                register __m256 b3 = _mm256_load_ps((float *)&b_base->y[3]);

                register __m256 a0 = _mm256_load_ps((float *)&a_base->y[0]);
                register __m256 a1 = _mm256_load_ps((float *)&a_base->y[1]);
                register __m256 a2 = _mm256_load_ps((float *)&a_base->y[2]);
                register __m256 a3 = _mm256_load_ps((float *)&a_base->y[3]);

                SN_MK_FMA_F32_AVX2_CYCLE(0);
                SN_MK_FMA_F32_AVX2_CYCLE(1);
                SN_MK_FMA_F32_AVX2_CYCLE(2);
                SN_MK_FMA_F32_AVX2_CYCLE(3);
        }
        _mm256_store_ps((float *)&(ans_cur->y[0]), c0);
        _mm256_store_ps((float *)&(ans_cur->y[1]), c1);
        _mm256_store_ps((float *)&(ans_cur->y[2]), c2);
        _mm256_store_ps((float *)&(ans_cur->y[3]), c3);
}

SN_MK_PROC_DECL_STD(float, avx, _fma, fma)
{
        __mk_param_std(LEFT_RIGHT_ANS, float);
        SN_MAT_KERNEL_LOCAL *restrict ans_cur = ans + offset_2;
        register __m256 c0 = _mm256_load_ps((float *)&ans_cur->y[0]);
        register __m256 c1 = _mm256_load_ps((float *)&ans_cur->y[1]);
        register __m256 c2 = _mm256_load_ps((float *)&ans_cur->y[2]);
        register __m256 c3 = _mm256_load_ps((float *)&ans_cur->y[3]);

        for (size_t z = 0; z < count_0; z += 2) {
                const SN_MAT_KERNEL_LOCAL *a_base = left + z;
                const SN_MAT_KERNEL_LOCAL *b_base = right + z;

                register __m256 b0 = _mm256_load_ps((float *)&b_base->y[0]);
                register __m256 b1 = _mm256_load_ps((float *)&b_base->y[1]);
                register __m256 b2 = _mm256_load_ps((float *)&b_base->y[2]);
                register __m256 b3 = _mm256_load_ps((float *)&b_base->y[3]);

                register __m256 a0 = _mm256_load_ps((float *)&a_base->y[0]);
                register __m256 a1 = _mm256_load_ps((float *)&a_base->y[1]);
                register __m256 a2 = _mm256_load_ps((float *)&a_base->y[2]);
                register __m256 a3 = _mm256_load_ps((float *)&a_base->y[3]);
                
                SN_MK_FMA_F32_AVX2_CYCLE(0);
                SN_MK_FMA_F32_AVX2_CYCLE(1);
                SN_MK_FMA_F32_AVX2_CYCLE(2);
                SN_MK_FMA_F32_AVX2_CYCLE(3);
        }
        _mm256_store_ps((float *)&(ans_cur->y[0]), c0);
        _mm256_store_ps((float *)&(ans_cur->y[1]), c1);
        _mm256_store_ps((float *)&(ans_cur->y[2]), c2);
        _mm256_store_ps((float *)&(ans_cur->y[3]), c3);
}

SN_MK_PROC_DECL_STD(float, 512, , fma)
{
        __mk_param_std(LEFT_RIGHT_ANS, float);
        SN_MAT_KERNEL_LOCAL *restrict ans_cur = ans + offset_2;
        register __m512 c0 = _mm512_load_ps((float *)&ans_cur->z[0]);
        register __m512 c1 = _mm512_load_ps((float *)&ans_cur->z[0]);
        register __m512 c2 = _mm512_load_ps((float *)&ans_cur->z[0]);
        register __m512 c3 = _mm512_load_ps((float *)&ans_cur->z[0]);

        for (size_t z = 0; z < count_0; z += 4) {
                const SN_MAT_KERNEL_LOCAL *a_base = left + z;
                const SN_MAT_KERNEL_LOCAL *b_base = right + z;

                register __m512 b0 = _mm512_load_ps((float *)&b_base->z[0]);
                register __m512 b1 = _mm512_load_ps((float *)&b_base->z[1]);
                register __m512 b2 = _mm512_load_ps((float *)&b_base->z[2]);
                register __m512 b3 = _mm512_load_ps((float *)&b_base->z[3]);

                register __m512 a0 = _mm512_load_ps((float *)&a_base->z[0]);
                register __m512 a1 = _mm512_load_ps((float *)&a_base->z[1]);
                register __m512 a2 = _mm512_load_ps((float *)&a_base->z[2]);
                register __m512 a3 = _mm512_load_ps((float *)&a_base->z[3]);

                SN_MK_FMA_F32_512_CYCLE(0);
                SN_MK_FMA_F32_512_CYCLE(1);
                SN_MK_FMA_F32_512_CYCLE(2);
                SN_MK_FMA_F32_512_CYCLE(3);
        }
        _mm512_store_ps((float *)&ans_cur->z[0], c0);
        _mm512_store_ps((float *)&ans_cur->z[1], c1);
        _mm512_store_ps((float *)&ans_cur->z[2], c2);
        _mm512_store_ps((float *)&ans_cur->z[3], c3);
}

SN_MK_PROC_DECL_STD(double, sse, , fma)
{
        __mk_param_std(LEFT_RIGHT_ANS, double);

        SN_MAT_KERNEL_LOCAL *restrict ans_cur = ans + offset_2;

        register __m128d c0_lo = ans_cur->x[0];
        register __m128d c0_hi = ans_cur->x[1];
        register __m128d c1_lo = ans_cur->x[2];
        register __m128d c1_hi = ans_cur->x[3];
        register __m128d c2_lo = ans_cur->x[4];
        register __m128d c2_hi = ans_cur->x[5];
        register __m128d c3_lo = ans_cur->x[6];
        register __m128d c3_hi = ans_cur->x[7];

        for (size_t z = 0; z < count_0; z++) {
                SN_MAT_KERNEL_LOCAL const *right_base = right;
                SN_MAT_KERNEL_LOCAL const *left_base = left;
                const double *right_values = &right_base->m[0][0];
                const double *left_values = &left_base->m[0][0];

                SN_MK_FMA_F64_SSE_PHASE(0);

                SN_MK_FMA_F64_SSE_PHASE(1);

                SN_MK_FMA_F64_SSE_PHASE(2);

                SN_MK_FMA_F64_SSE_PHASE(3);
        }

        ans_cur->x[0] = c0_lo;
        ans_cur->x[1] = c0_hi;
        ans_cur->x[2] = c1_lo;
        ans_cur->x[3] = c1_hi;
        ans_cur->x[4] = c2_lo;
        ans_cur->x[5] = c2_hi;
        ans_cur->x[6] = c3_lo;
        ans_cur->x[7] = c3_hi;
}

SN_MK_PROC_DECL_STD(double, avx, , fma)
{
        __mk_param_std(LEFT_RIGHT_ANS, double);

        SN_MAT_KERNEL_LOCAL *restrict ans_cur = ans + offset_2;

        register __m256d c0 = _mm256_load_pd((double *)&ans_cur->y[0]);
        register __m256d c1 = _mm256_load_pd((double *)&ans_cur->y[1]);
        register __m256d c2 = _mm256_load_pd((double *)&ans_cur->y[2]);
        register __m256d c3 = _mm256_load_pd((double *)&ans_cur->y[3]);
        register __m256d bcol;

        for (size_t z = 0; z < count_0; z++) {
                SN_MAT_KERNEL_LOCAL const *right_base = right;
                SN_MAT_KERNEL_LOCAL const *left_base = left;

                const __m256d a0 = _mm256_load_pd((double *)&left_base->y[0]);
                const __m256d a1 = _mm256_load_pd((double *)&left_base->y[1]);
                const __m256d a2 = _mm256_load_pd((double *)&left_base->y[2]);
                const __m256d a3 = _mm256_load_pd((double *)&left_base->y[3]);

                /* Column 0 */
                bcol = _mm256_load_pd((double *)&right_base->y[0]);
                c0 = _mm256_add_pd(c0,
                                   _mm256_mul_pd(a0, _mm256_permute4x64_pd(bcol, _FMA_SELECT(0))));
                c0 = _mm256_add_pd(c0,
                                   _mm256_mul_pd(a1, _mm256_permute4x64_pd(bcol, _FMA_SELECT(1))));
                c0 = _mm256_add_pd(c0,
                                   _mm256_mul_pd(a2, _mm256_permute4x64_pd(bcol, _FMA_SELECT(2))));
                c0 = _mm256_add_pd(c0,
                                   _mm256_mul_pd(a3, _mm256_permute4x64_pd(bcol, _FMA_SELECT(3))));

                /* Column 1 */
                bcol = _mm256_load_pd((double *)&right_base->y[1]);
                c1 = _mm256_add_pd(c1,
                                   _mm256_mul_pd(a0, _mm256_permute4x64_pd(bcol, _FMA_SELECT(0))));
                c1 = _mm256_add_pd(c1,
                                   _mm256_mul_pd(a1, _mm256_permute4x64_pd(bcol, _FMA_SELECT(1))));
                c1 = _mm256_add_pd(c1,
                                   _mm256_mul_pd(a2, _mm256_permute4x64_pd(bcol, _FMA_SELECT(2))));
                c1 = _mm256_add_pd(c1,
                                   _mm256_mul_pd(a3, _mm256_permute4x64_pd(bcol, _FMA_SELECT(3))));

                /* Column 2 */
                bcol = _mm256_load_pd((double *)&right_base->y[2]);
                c2 = _mm256_add_pd(c2,
                                   _mm256_mul_pd(a0, _mm256_permute4x64_pd(bcol, _FMA_SELECT(0))));
                c2 = _mm256_add_pd(c2,
                                   _mm256_mul_pd(a1, _mm256_permute4x64_pd(bcol, _FMA_SELECT(1))));
                c2 = _mm256_add_pd(c2,
                                   _mm256_mul_pd(a2, _mm256_permute4x64_pd(bcol, _FMA_SELECT(2))));
                c2 = _mm256_add_pd(c2,
                                   _mm256_mul_pd(a3, _mm256_permute4x64_pd(bcol, _FMA_SELECT(3))));

                /* Column 3 */
                bcol = _mm256_load_pd((double *)&right_base->y[3]);
                c3 = _mm256_add_pd(c3,
                                   _mm256_mul_pd(a0, _mm256_permute4x64_pd(bcol, _FMA_SELECT(0))));
                c3 = _mm256_add_pd(c3,
                                   _mm256_mul_pd(a1, _mm256_permute4x64_pd(bcol, _FMA_SELECT(1))));
                c3 = _mm256_add_pd(c3,
                                   _mm256_mul_pd(a2, _mm256_permute4x64_pd(bcol, _FMA_SELECT(2))));
                c3 = _mm256_add_pd(c3,
                                   _mm256_mul_pd(a3, _mm256_permute4x64_pd(bcol, _FMA_SELECT(3))));
        }

        _mm256_store_pd((double *)&ans_cur->y[0], c0);
        _mm256_store_pd((double *)&ans_cur->y[1], c1);
        _mm256_store_pd((double *)&ans_cur->y[2], c2);
        _mm256_store_pd((double *)&ans_cur->y[3], c3);
}

SN_MK_PROC_DECL_STD(double, avx, _fma, fma)
{
        __mk_param_std(LEFT_RIGHT_ANS, double);

        SN_MAT_KERNEL_LOCAL *restrict ans_cur = ans + offset_2;

        register __m256d c0 = _mm256_load_pd((double *)&ans_cur->y[0]);
        register __m256d c1 = _mm256_load_pd((double *)&ans_cur->y[1]);
        register __m256d c2 = _mm256_load_pd((double *)&ans_cur->y[2]);
        register __m256d c3 = _mm256_load_pd((double *)&ans_cur->y[3]);
        register __m256d bcol;

        for (size_t z = 0; z < count_0; z++) {
                SN_MAT_KERNEL_LOCAL const *right_base = right;
                SN_MAT_KERNEL_LOCAL const *left_base = left;

                const __m256d a0 = _mm256_load_pd((double *)&left_base->y[0]);
                const __m256d a1 = _mm256_load_pd((double *)&left_base->y[1]);
                const __m256d a2 = _mm256_load_pd((double *)&left_base->y[2]);
                const __m256d a3 = _mm256_load_pd((double *)&left_base->y[3]);

                /* Column 0 */
                bcol = _mm256_load_pd((double *)&right_base->y[0]);
                c0 = _mm256_fmadd_pd(a0, _mm256_permute4x64_pd(bcol, _FMA_SELECT(0)), c0);
                c0 = _mm256_fmadd_pd(a1, _mm256_permute4x64_pd(bcol, _FMA_SELECT(1)), c0);
                c0 = _mm256_fmadd_pd(a2, _mm256_permute4x64_pd(bcol, _FMA_SELECT(2)), c0);
                c0 = _mm256_fmadd_pd(a3, _mm256_permute4x64_pd(bcol, _FMA_SELECT(3)), c0);

                /* Column 1 */
                bcol = _mm256_load_pd((double *)&right_base->y[1]);
                c1 = _mm256_fmadd_pd(a0, _mm256_permute4x64_pd(bcol, _FMA_SELECT(0)), c1);
                c1 = _mm256_fmadd_pd(a1, _mm256_permute4x64_pd(bcol, _FMA_SELECT(1)), c1);
                c1 = _mm256_fmadd_pd(a2, _mm256_permute4x64_pd(bcol, _FMA_SELECT(2)), c1);
                c1 = _mm256_fmadd_pd(a3, _mm256_permute4x64_pd(bcol, _FMA_SELECT(3)), c1);

                /* Column 2 */
                bcol = _mm256_load_pd((double *)&right_base->y[2]);
                c2 = _mm256_fmadd_pd(a0, _mm256_permute4x64_pd(bcol, _FMA_SELECT(0)), c2);
                c2 = _mm256_fmadd_pd(a1, _mm256_permute4x64_pd(bcol, _FMA_SELECT(1)), c2);
                c2 = _mm256_fmadd_pd(a2, _mm256_permute4x64_pd(bcol, _FMA_SELECT(2)), c2);
                c2 = _mm256_fmadd_pd(a3, _mm256_permute4x64_pd(bcol, _FMA_SELECT(3)), c2);

                /* Column 3 */
                bcol = _mm256_load_pd((double *)&right_base->y[3]);
                c3 = _mm256_fmadd_pd(a0, _mm256_permute4x64_pd(bcol, _FMA_SELECT(0)), c3);
                c3 = _mm256_fmadd_pd(a1, _mm256_permute4x64_pd(bcol, _FMA_SELECT(1)), c3);
                c3 = _mm256_fmadd_pd(a2, _mm256_permute4x64_pd(bcol, _FMA_SELECT(2)), c3);
                c3 = _mm256_fmadd_pd(a3, _mm256_permute4x64_pd(bcol, _FMA_SELECT(3)), c3);
        }

        _mm256_store_pd((double *)&ans_cur->y[0], c0);
        _mm256_store_pd((double *)&ans_cur->y[1], c1);
        _mm256_store_pd((double *)&ans_cur->y[2], c2);
        _mm256_store_pd((double *)&ans_cur->y[3], c3);
}

SN_MK_PROC_DECL_STD(double, 512, , fma)
{
        __mk_param_std(LEFT_RIGHT_ANS, double);
        SN_MAT_KERNEL_LOCAL *restrict ans_cur = ans + offset_2;

        register __m512d c0 = _mm512_load_pd((double *)&ans_cur->z[0]);
        register __m512d c1 = _mm512_load_pd((double *)&ans_cur->z[1]);
        register __m512d c2 = _mm512_load_pd((double *)&ans_cur->z[2]);
        register __m512d c3 = _mm512_load_pd((double *)&ans_cur->z[3]);

        for (size_t z = 0; z < count_0; z += 2) {
                SN_MAT_KERNEL_LOCAL const *right_base = right + z;
                SN_MAT_KERNEL_LOCAL const *left_base = left + z;

                register __m512d a0 = _mm512_load_pd((double *)&left_base->z[0]);
                register __m512d a1 = _mm512_load_pd((double *)&left_base->z[1]);
                register __m512d a2 = _mm512_load_pd((double *)&left_base->z[2]);
                register __m512d a3 = _mm512_load_pd((double *)&left_base->z[3]);

                register __m512d b0 = _mm512_load_pd((double *)&right_base->z[0]);
                register __m512d b1 = _mm512_load_pd((double *)&right_base->z[1]);
                register __m512d b2 = _mm512_load_pd((double *)&right_base->z[2]);
                register __m512d b3 = _mm512_load_pd((double *)&right_base->z[3]);

                SN_MK_FMA_F64_AVX2_CYCLE(0);
                SN_MK_FMA_F64_AVX2_CYCLE(1);
                SN_MK_FMA_F64_AVX2_CYCLE(2);
                SN_MK_FMA_F64_AVX2_CYCLE(3);
        }
        _mm512_store_pd((double *)&ans_cur->z[0], c0);
        _mm512_store_pd((double *)&ans_cur->z[1], c1);
        _mm512_store_pd((double *)&ans_cur->z[2], c2);
        _mm512_store_pd((double *)&ans_cur->z[3], c3);
}
#endif
