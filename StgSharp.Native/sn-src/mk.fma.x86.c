#include "sn_target.h"
/* clang-format off */
#if SN_IS_ARCH(SN_ARCH_X86_64)

// #include "sn_intrinsic.h"
#include "sn_template.fma.x86.h"
#include "sn_intrinsic.std.h"
#include "sn_intrinsic.context.x86.h"

#define _FMA_SELECT(i) _MM_SHUFFLE(i, i, i, i)

#define GET_INDEX(col_len, x, y) (col_len * x + y)

// float

SN_MK_PROC_DECL_STD(float, sse, , fma)
{
        __mk_param_std(TILE, LEFT_RIGHT_ANS, float);

        SN_MAT_KERNEL_LOCAL *restrict ans_cur =
                ans + GET_INDEX(common_y, x, y); // GetKernelAddressUnsafe

        register __m128 c0 = _mm_load_ps((float *)&ans_cur->f32_x[0]);
        register __m128 c1 = _mm_load_ps((float *)&ans_cur->f32_x[1]);
        register __m128 c2 = _mm_load_ps((float *)&ans_cur->f32_x[2]);
        register __m128 c3 = _mm_load_ps((float *)&ans_cur->f32_x[3]);
        register __m128 bcol;

        for (size_t z = 0; z < common_z; z++) {
                SN_MAT_KERNEL_LOCAL const *right_cur = right + GET_INDEX(common_z, x, z);
                SN_MAT_KERNEL_LOCAL const *left_cur = left + GET_INDEX(common_y, z, y);

                const __m128 a0 = _mm_load_ps((float *)&left_cur->f32_x[0]);
                const __m128 a1 = _mm_load_ps((float *)&left_cur->f32_x[1]);
                const __m128 a2 = _mm_load_ps((float *)&left_cur->f32_x[2]);
                const __m128 a3 = _mm_load_ps((float *)&left_cur->f32_x[3]);

                /* Column 0 */
                bcol = _mm_load_ps((float *)&right_cur->f32_x[0]);
                c0 = _mm_add_ps(c0, _mm_mul_ps(a0, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(0))));
                c0 = _mm_add_ps(c0, _mm_mul_ps(a1, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(1))));
                c0 = _mm_add_ps(c0, _mm_mul_ps(a2, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(2))));
                c0 = _mm_add_ps(c0, _mm_mul_ps(a3, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(3))));

                /* Column 1 */
                bcol = _mm_load_ps((float *)&right_cur->f32_x[1]);
                c1 = _mm_add_ps(c1, _mm_mul_ps(a0, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(0))));
                c1 = _mm_add_ps(c1, _mm_mul_ps(a1, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(1))));
                c1 = _mm_add_ps(c1, _mm_mul_ps(a2, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(2))));
                c1 = _mm_add_ps(c1, _mm_mul_ps(a3, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(3))));

                /* Column 2 */
                bcol = _mm_load_ps((float *)&right_cur->f32_x[2]);
                c2 = _mm_add_ps(c2, _mm_mul_ps(a0, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(0))));
                c2 = _mm_add_ps(c2, _mm_mul_ps(a1, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(1))));
                c2 = _mm_add_ps(c2, _mm_mul_ps(a2, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(2))));
                c2 = _mm_add_ps(c2, _mm_mul_ps(a3, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(3))));

                /* Column 3 */
                bcol = _mm_load_ps((float *)&right_cur->f32_x[3]);
                c3 = _mm_add_ps(c3, _mm_mul_ps(a0, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(0))));
                c3 = _mm_add_ps(c3, _mm_mul_ps(a1, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(1))));
                c3 = _mm_add_ps(c3, _mm_mul_ps(a2, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(2))));
                c3 = _mm_add_ps(c3, _mm_mul_ps(a3, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(3))));
        }

        _mm_store_ps((float *)&ans_cur->f32_x[0], c0);
        _mm_store_ps((float *)&ans_cur->f32_x[1], c1);
        _mm_store_ps((float *)&ans_cur->f32_x[2], c2);
        _mm_store_ps((float *)&ans_cur->f32_x[3], c3);
}

SN_MK_PROC_DECL_STD(float, avx, , fma)
{
        __mk_param_std(TILE, LEFT_RIGHT_ANS, float);

        SN_MAT_KERNEL_LOCAL *restrict ans_cur =
                ans + GET_INDEX(common_y, x, y); // GetKernelAddressUnsafe

        register __m256 c01 = _mm256_load_ps((float *)&ans_cur->f32_y[0]);
        register __m256 c23 = _mm256_load_ps((float *)&ans_cur->f32_y[1]);

        for (size_t z = 0; z < common_z; z++) {
                SN_MAT_KERNEL_LOCAL const *right_cur = right + GET_INDEX(common_z, x, z);
                SN_MAT_KERNEL_LOCAL const *left_cur = left + GET_INDEX(common_y, z, y);

                register __m256 b01 = _mm256_load_ps((float *)&right_cur->f32_y[0]);
                register __m256 b23 = _mm256_load_ps((float *)&right_cur->f32_y[1]);

                register __m256 a0 = _mm256_broadcast_ps(&left_cur->f32_x[0]);
                c01 = _mm256_add_ps(_mm256_mul_ps(a0, b01), c01);
                c23 = _mm256_add_ps(_mm256_mul_ps(a0, b23), c23);
                register __m256 a1 = _mm256_broadcast_ps(&left_cur->f32_x[1]);
                c01 = _mm256_add_ps(_mm256_mul_ps(a1, b01), c01);
                c23 = _mm256_add_ps(_mm256_mul_ps(a1, b23), c23);
                register __m256 a2 = _mm256_broadcast_ps(&left_cur->f32_x[2]);
                c01 = _mm256_add_ps(_mm256_mul_ps(a2, b01), c01);
                c23 = _mm256_add_ps(_mm256_mul_ps(a2, b23), c23);
                register __m256 a3 = _mm256_broadcast_ps(&left_cur->f32_x[3]);
                c01 = _mm256_add_ps(_mm256_mul_ps(a3, b01), c01);
                c23 = _mm256_add_ps(_mm256_mul_ps(a3, b23), c23);
        }
        _mm256_store_ps((float *)&ans_cur->f32_y[0], c01);
        _mm256_store_ps((float *)&ans_cur->f32_y[1], c23);
}

SN_MK_PROC_DECL_STD(float, avx, _fma, fma)
{
        __mk_param_std(TILE, LEFT_RIGHT_ANS, float);

        SN_MAT_KERNEL_LOCAL *restrict ans_cur =
                ans + GET_INDEX(common_y, x, y); // GetKernelAddressUnsafe

        register __m256 c01 = _mm256_load_ps((float *)&ans_cur->f32_y[0]);
        register __m256 c23 = _mm256_load_ps((float *)&ans_cur->f32_y[1]);

        for (size_t z = 0; z < common_z; z++) {
                SN_MAT_KERNEL_LOCAL const *right_cur = right + GET_INDEX(common_z, x, z);
                SN_MAT_KERNEL_LOCAL const *left_cur = left + GET_INDEX(common_y, z, y);
                register __m256 b01 = _mm256_load_ps((float *)&right_cur->f32_y[0]);
                register __m256 b23 = _mm256_load_ps((float *)&right_cur->f32_y[1]);

                register __m256 a0 = _mm256_broadcast_ps(&left_cur->f32_x[0]);
                c01 = _mm256_fmadd_ps(a0, b01, c01);
                c23 = _mm256_fmadd_ps(a0, b23, c23);
                register __m256 a1 = _mm256_broadcast_ps(&left_cur->f32_x[1]);
                c01 = _mm256_fmadd_ps(a1, b01, c01);
                c23 = _mm256_fmadd_ps(a1, b23, c23);
                register __m256 a2 = _mm256_broadcast_ps(&left_cur->f32_x[2]);
                c01 = _mm256_fmadd_ps(a2, b01, c01);
                c23 = _mm256_fmadd_ps(a2, b23, c23);
                register __m256 a3 = _mm256_broadcast_ps(&left_cur->f32_x[3]);
                c01 = _mm256_fmadd_ps(a3, b01, c01);
                c23 = _mm256_fmadd_ps(a3, b23, c23);
        }
        _mm256_store_ps((float *)&ans_cur->f32_y[0], c01);
        _mm256_store_ps((float *)&ans_cur->f32_y[1], c23);
}


SN_MK_PROC_DECL_STD(float, 512, , fma)
{
        __mk_param_std(TILE, LEFT_RIGHT_ANS, float);
        SN_MAT_KERNEL_LOCAL *restrict ans_cur = ans + GET_INDEX(common_y, x, y);
        register __m512 c_0 = _mm512_load_ps((float *)ans_cur);
        register __m512 c_1 = c_0;
        register __m512 c_2 = c_0;
        register __m512 c_3 = c_0;

        for (size_t z = 0; z < common_z; z+=4) {
                SN_MAT_KERNEL_LOCAL const *right_ = right + z;
                SN_MAT_KERNEL_LOCAL const *left_0 = left + z + 0;
                SN_MAT_KERNEL_LOCAL const *left_1 = left + z + 1;
                SN_MAT_KERNEL_LOCAL const *left_2 = left + z + 2;
                SN_MAT_KERNEL_LOCAL const *left_3 = left + z + 3;

                // full right load：[kb_col0 | kb_col1 | kb_col2 | kb_col3]
                register __m512 b_0 = _mm512_load_ps((float *)(right+0));
                register __m512 b_1 = _mm512_load_ps((float *)(right+1));
                register __m512 b_2 = _mm512_load_ps((float *)(right+2));
                register __m512 b_3 = _mm512_load_ps((float *)(right+3));

                // k=0: broadcast ka_col0 到4个lane，permute_ps 取 kb(0,j)
                register __m512 ak;
                register __m512 ak_1;
                register __m512 ak_2;
                register __m512 ak_3;
                
                SN_MK_FMA_F32_512_PHASE(0);
                SN_MK_FMA_F32_512_PHASE(1);
                SN_MK_FMA_F32_512_PHASE(2);
                SN_MK_FMA_F32_512_PHASE(3);
        }
        c_0 = _mm512_add_ps(c_0, c_1);
        c_2= _mm512_add_ps(c_2, c_3);
        c_0 = _mm512_add_ps(c_0, c_2);
        _mm512_store_ps((float *)ans_cur, c_0);
}


SN_MK_PROC_DECL_STD(double, sse, , fma)
{
        __mk_param_std(TILE, LEFT_RIGHT_ANS, double);

        SN_MAT_KERNEL_LOCAL *restrict ans_cur =
                ans + GET_INDEX(common_y, x, y); // GetKernelAddressUnsafe

        register __m128d c0_lo = ans_cur->f64_x[0];
        register __m128d c0_hi = ans_cur->f64_x[1];
        register __m128d c1_lo = ans_cur->f64_x[2];
        register __m128d c1_hi = ans_cur->f64_x[3];
        register __m128d c2_lo = ans_cur->f64_x[4];
        register __m128d c2_hi = ans_cur->f64_x[5];
        register __m128d c3_lo = ans_cur->f64_x[6];
        register __m128d c3_hi = ans_cur->f64_x[7];

        for (size_t z = 0; z < common_z; z++) {
                SN_MAT_KERNEL_LOCAL const *right_cur = right + GET_INDEX(common_z, x, z);
                SN_MAT_KERNEL_LOCAL const *left_cur = left + GET_INDEX(common_y, z, y);
                const double *right_values = &right_cur->m[0][0];
                const double *left_values = &left_cur->m[0][0];

                SN_MK_FMA_F64_SSE_PHASE(0);

                SN_MK_FMA_F64_SSE_PHASE(1);

                SN_MK_FMA_F64_SSE_PHASE(2);

                SN_MK_FMA_F64_SSE_PHASE(3);
        }

        ans_cur->f64_x[0] = c0_lo;
        ans_cur->f64_x[1] = c0_hi;
        ans_cur->f64_x[2] = c1_lo;
        ans_cur->f64_x[3] = c1_hi;
        ans_cur->f64_x[4] = c2_lo;
        ans_cur->f64_x[5] = c2_hi;
        ans_cur->f64_x[6] = c3_lo;
        ans_cur->f64_x[7] = c3_hi;
}

SN_MK_PROC_DECL_STD(double, avx, , fma)
{
        __mk_param_std(TILE, LEFT_RIGHT_ANS, double);

        SN_MAT_KERNEL_LOCAL *restrict ans_cur = ans + GET_INDEX(common_y, x, y); // tKernelAddressUnsafe

        register __m256d c0 = _mm256_load_pd((double *)&ans_cur->f64_y[0]);
        register __m256d c1 = _mm256_load_pd((double *)&ans_cur->f64_y[1]);
        register __m256d c2 = _mm256_load_pd((double *)&ans_cur->f64_y[2]);
        register __m256d c3 = _mm256_load_pd((double *)&ans_cur->f64_y[3]);
        register __m256d bcol;

        for (size_t z = 0; z < common_z; z++) {
                SN_MAT_KERNEL_LOCAL const *right_cur = right + GET_INDEX(common_z, x, z);
                SN_MAT_KERNEL_LOCAL const *left_cur = left + GET_INDEX(common_y, z, y);

                const __m256d a0 = _mm256_load_pd((double *)&left_cur->f64_y[0]);
                const __m256d a1 = _mm256_load_pd((double *)&left_cur->f64_y[1]);
                const __m256d a2 = _mm256_load_pd((double *)&left_cur->f64_y[2]);
                const __m256d a3 = _mm256_load_pd((double *)&left_cur->f64_y[3]);

                /* Column 0 */
                bcol = _mm256_load_pd((double *)&right_cur->f64_y[0]);
                c0 = _mm256_add_pd(c0,
                                   _mm256_mul_pd(a0, _mm256_permute4x64_pd(bcol, _FMA_SELECT(0))));
                c0 = _mm256_add_pd(c0,
                                   _mm256_mul_pd(a1, _mm256_permute4x64_pd(bcol, _FMA_SELECT(1))));
                c0 = _mm256_add_pd(c0,
                                   _mm256_mul_pd(a2, _mm256_permute4x64_pd(bcol, _FMA_SELECT(2))));
                c0 = _mm256_add_pd(c0,
                                   _mm256_mul_pd(a3, _mm256_permute4x64_pd(bcol, _FMA_SELECT(3))));

                /* Column 1 */
                bcol = _mm256_load_pd((double *)&right_cur->f64_y[1]);
                c1 = _mm256_add_pd(c1,
                                   _mm256_mul_pd(a0, _mm256_permute4x64_pd(bcol, _FMA_SELECT(0))));
                c1 = _mm256_add_pd(c1,
                                   _mm256_mul_pd(a1, _mm256_permute4x64_pd(bcol, _FMA_SELECT(1))));
                c1 = _mm256_add_pd(c1,
                                   _mm256_mul_pd(a2, _mm256_permute4x64_pd(bcol, _FMA_SELECT(2))));
                c1 = _mm256_add_pd(c1,
                                   _mm256_mul_pd(a3, _mm256_permute4x64_pd(bcol, _FMA_SELECT(3))));

                /* Column 2 */
                bcol = _mm256_load_pd((double *)&right_cur->f64_y[2]);
                c2 = _mm256_add_pd(c2,
                                   _mm256_mul_pd(a0, _mm256_permute4x64_pd(bcol, _FMA_SELECT(0))));
                c2 = _mm256_add_pd(c2,
                                   _mm256_mul_pd(a1, _mm256_permute4x64_pd(bcol, _FMA_SELECT(1))));
                c2 = _mm256_add_pd(c2,
                                   _mm256_mul_pd(a2, _mm256_permute4x64_pd(bcol, _FMA_SELECT(2))));
                c2 = _mm256_add_pd(c2,
                                   _mm256_mul_pd(a3, _mm256_permute4x64_pd(bcol, _FMA_SELECT(3))));

                /* Column 3 */
                bcol = _mm256_load_pd((double *)&right_cur->f64_y[3]);
                c3 = _mm256_add_pd(c3,
                                   _mm256_mul_pd(a0, _mm256_permute4x64_pd(bcol, _FMA_SELECT(0))));
                c3 = _mm256_add_pd(c3,
                                   _mm256_mul_pd(a1, _mm256_permute4x64_pd(bcol, _FMA_SELECT(1))));
                c3 = _mm256_add_pd(c3,
                                   _mm256_mul_pd(a2, _mm256_permute4x64_pd(bcol, _FMA_SELECT(2))));
                c3 = _mm256_add_pd(c3,
                                   _mm256_mul_pd(a3, _mm256_permute4x64_pd(bcol, _FMA_SELECT(3))));
        }

        _mm256_store_pd((double *)&ans_cur->f64_y[0], c0);
        _mm256_store_pd((double *)&ans_cur->f64_y[1], c1);
        _mm256_store_pd((double *)&ans_cur->f64_y[2], c2);
        _mm256_store_pd((double *)&ans_cur->f64_y[3], c3);
}

SN_MK_PROC_DECL_STD(double, avx, _fma, fma)
{
        __mk_param_std(TILE, LEFT_RIGHT_ANS, double);

        SN_MAT_KERNEL_LOCAL *restrict ans_cur = ans + GET_INDEX(common_y, x, y); // GetKernelAddressUnsafe

        register __m256d c0 = _mm256_load_pd((double *)&ans_cur->f64_y[0]);
        register __m256d c1 = _mm256_load_pd((double *)&ans_cur->f64_y[1]);
        register __m256d c2 = _mm256_load_pd((double *)&ans_cur->f64_y[2]);
        register __m256d c3 = _mm256_load_pd((double *)&ans_cur->f64_y[3]);
        register __m256d bcol;

        for (size_t z = 0; z < common_z; z++) {
                SN_MAT_KERNEL_LOCAL const *right_cur = right + GET_INDEX(common_z, x, z);
                SN_MAT_KERNEL_LOCAL const *left_cur = left + GET_INDEX(common_y, z, y);

                const __m256d a0 = _mm256_load_pd((double *)&left_cur->f64_y[0]);
                const __m256d a1 = _mm256_load_pd((double *)&left_cur->f64_y[1]);
                const __m256d a2 = _mm256_load_pd((double *)&left_cur->f64_y[2]);
                const __m256d a3 = _mm256_load_pd((double *)&left_cur->f64_y[3]);

                /* Column 0 */
                bcol = _mm256_load_pd((double *)&right_cur->f64_y[0]);
                c0 = _mm256_fmadd_pd(a0, _mm256_permute4x64_pd(bcol, _FMA_SELECT(0)), c0);
                c0 = _mm256_fmadd_pd(a1, _mm256_permute4x64_pd(bcol, _FMA_SELECT(1)), c0);
                c0 = _mm256_fmadd_pd(a2, _mm256_permute4x64_pd(bcol, _FMA_SELECT(2)), c0);
                c0 = _mm256_fmadd_pd(a3, _mm256_permute4x64_pd(bcol, _FMA_SELECT(3)), c0);

                /* Column 1 */
                bcol = _mm256_load_pd((double *)&right_cur->f64_y[1]);
                c1 = _mm256_fmadd_pd(a0, _mm256_permute4x64_pd(bcol, _FMA_SELECT(0)), c1);
                c1 = _mm256_fmadd_pd(a1, _mm256_permute4x64_pd(bcol, _FMA_SELECT(1)), c1);
                c1 = _mm256_fmadd_pd(a2, _mm256_permute4x64_pd(bcol, _FMA_SELECT(2)), c1);
                c1 = _mm256_fmadd_pd(a3, _mm256_permute4x64_pd(bcol, _FMA_SELECT(3)), c1);

                /* Column 2 */
                bcol = _mm256_load_pd((double *)&right_cur->f64_y[2]);
                c2 = _mm256_fmadd_pd(a0, _mm256_permute4x64_pd(bcol, _FMA_SELECT(0)), c2);
                c2 = _mm256_fmadd_pd(a1, _mm256_permute4x64_pd(bcol, _FMA_SELECT(1)), c2);
                c2 = _mm256_fmadd_pd(a2, _mm256_permute4x64_pd(bcol, _FMA_SELECT(2)), c2);
                c2 = _mm256_fmadd_pd(a3, _mm256_permute4x64_pd(bcol, _FMA_SELECT(3)), c2);

                /* Column 3 */
                bcol = _mm256_load_pd((double *)&right_cur->f64_y[3]);
                c3 = _mm256_fmadd_pd(a0, _mm256_permute4x64_pd(bcol, _FMA_SELECT(0)), c3);
                c3 = _mm256_fmadd_pd(a1, _mm256_permute4x64_pd(bcol, _FMA_SELECT(1)), c3);
                c3 = _mm256_fmadd_pd(a2, _mm256_permute4x64_pd(bcol, _FMA_SELECT(2)), c3);
                c3 = _mm256_fmadd_pd(a3, _mm256_permute4x64_pd(bcol, _FMA_SELECT(3)), c3);
        }

        _mm256_store_pd((double *)&ans_cur->f64_y[0], c0);
        _mm256_store_pd((double *)&ans_cur->f64_y[1], c1);
        _mm256_store_pd((double *)&ans_cur->f64_y[2], c2);
        _mm256_store_pd((double *)&ans_cur->f64_y[3], c3);
}

SN_MK_PROC_DECL_STD(double, 512, , fma)
{
        __mk_param_std(TILE, LEFT_RIGHT_ANS, double);

        SN_MAT_KERNEL_LOCAL *restrict ans_cur = ans + GET_INDEX(common_y, x, y);

        register __m512d c01 = _mm512_load_pd((double *)&ans_cur->f64_z[0]);
        register __m512d c23 = _mm512_load_pd((double *)&ans_cur->f64_z[1]);

        for (size_t z = 0; z < common_z; z++) {
                SN_MAT_KERNEL_LOCAL const *right_cur = right + GET_INDEX(common_z, x, z);
                SN_MAT_KERNEL_LOCAL const *left_cur = left + GET_INDEX(common_y, z, y);

                register __m512d a01 = _mm512_load_pd((double *)&left_cur->f64_z[0]);
                register __m512d a23 = _mm512_load_pd((double *)&left_cur->f64_z[1]);

                register __m512d b0 = _mm512_broadcast_f64x4(right_cur->f64_y[0]);
                c01 = _mm512_fmadd_pd(a01, b0, c01);
                c23 = _mm512_fmadd_pd(a23, b0, c23);

                register __m512d b1 = _mm512_broadcast_f64x4(right_cur->f64_y[1]);
                c01 = _mm512_fmadd_pd(a01, b1, c01);
                c23 = _mm512_fmadd_pd(a23, b1, c23);

                register __m512d b2 = _mm512_broadcast_f64x4(right_cur->f64_y[2]);
                c01 = _mm512_fmadd_pd(a01, b2, c01);
                c23 = _mm512_fmadd_pd(a23, b2, c23);

                register __m512d b3 = _mm512_broadcast_f64x4(right_cur->f64_y[3]);
                c01 = _mm512_fmadd_pd(a01, b3, c01);
                c23 = _mm512_fmadd_pd(a23, b3, c23);
        }
        _mm512_store_pd((double *)&ans_cur->f64_z[0], c01);
        _mm512_store_pd((double *)&ans_cur->f64_z[1], c23);
}
#endif
