#include "sn_target.h"

#if SN_IS_ARCH(SN_ARCH_X86_64)

#include "sn_intrinsic.h"
#include "sn_intrinsic.std.h"
#include "sn_intrinsic.context.x86.h"

#define _FMA_SELECT(i) _MM_SHUFFLE(i, i, i, i)

#define GET_INDEX(col_len, x, y) (col_len * x + y)

// float

SN_MK_PROC_DECL_STD(float, sse, , fma)
{
        __mk_param_std(TILE, LEFT_RIGHT_ANS, float);

        MAT_KERNEL(float)
        const *ans_cur = ans + GET_INDEX(common_y, x, y); // GetKernelAddressUnsafe

        register __m128 c0 = _mm_load_ps((float *)&ans_cur->f32_x[0]);
        register __m128 c1 = _mm_load_ps((float *)&ans_cur->f32_x[1]);
        register __m128 c2 = _mm_load_ps((float *)&ans_cur->f32_x[2]);
        register __m128 c3 = _mm_load_ps((float *)&ans_cur->f32_x[3]);
        register __m128 bcol;

        for (size_t z = 0; z < common_z; z++) {
                SN_MAT_KERNEL_LOCAL const *right_cur = right + GET_INDEX(common_z, x, z),
                                          const *left_cur = left + GET_INDEX(common_y, z, y);

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
        static const __m256i b01_shift_source = { .m256i_i32 = { 0, 1, 2, 3, 5, 6, 7, 4 } };
        static const __m256i b23_shift_source = { .m256i_i32 = { 2, 3, 0, 1, 7, 4, 5, 6 } };

        __mk_param_std(TILE, LEFT_RIGHT_ANS, float);

        MAT_KERNEL(float)
        const *ans_cur = ans + GET_INDEX(common_y, x, y); // GetKernelAddressUnsafe

        register __m256 c01 = _mm256_load_ps((float *)&ans->f32_y[0]);
        register __m256 c23 = _mm256_load_ps((float *)&ans->f32_y[1]);

        for (size_t z = 0; z < common_z; z++) {
                SN_MAT_KERNEL_LOCAL const *right_cur = right + GET_INDEX(common_z, x, z),
                                          const *left_cur = left + GET_INDEX(common_y, z, y);

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

        MAT_KERNEL(float)
        const *ans_cur = ans + GET_INDEX(common_y, x, y); // GetKernelAddressUnsafe

        register __m256 c01 = _mm256_load_ps((float *)&ans_cur->f32_y[0]);
        register __m256 c23 = _mm256_load_ps((float *)&ans_cur->f32_y[1]);

        for (size_t z = 0; z < common_z; z++) {
                SN_MAT_KERNEL_LOCAL const *right_cur = right + GET_INDEX(common_z, x, z),
                                          const *left_cur = left + GET_INDEX(common_y, z, y);
                register __m256 b01 = _mm256_load_ps((float *)&right_cur->f32_y[0]);
                register __m256 b23 = _mm256_load_ps((float *)&right_cur->f32_y[1]);

                register __m256 a0 = _mm256_broadcast_ps((float *)&left_cur->f32_x[0]);
                c01 = _mm256_fmadd_ps(a0, b01, c01);
                c23 = _mm256_fmadd_ps(a0, b23, c23);
                register __m256 a1 = _mm256_broadcast_ps((float *)&left_cur->f32_x[1]);
                c01 = _mm256_fmadd_ps(a1, b01, c01);
                c23 = _mm256_fmadd_ps(a1, b23, c23);
                register __m256 a2 = _mm256_broadcast_ps((float *)&left_cur->f32_x[2]);
                c01 = _mm256_fmadd_ps(a2, b01, c01);
                c23 = _mm256_fmadd_ps(a2, b23, c23);
                register __m256 a3 = _mm256_broadcast_ps((float *)&left_cur->f32_x[3]);
                c01 = _mm256_fmadd_ps(a3, b01, c01);
                c23 = _mm256_fmadd_ps(a3, b23, c23);
        }
        _mm256_store_ps((float *)&ans_cur->f32_y[0], c01);
        _mm256_store_ps((float *)&ans_cur->f32_y[1], c23);
}

SN_MK_PROC_DECL_STD(float, 512, , fma)
{
        __mk_param_std(TILE, LEFT_RIGHT_ANS, float);
        SN_MAT_KERNEL_LOCAL const *ans_cur = ans + GET_INDEX(common_y, x, y);
        register __m512 c = _mm512_load_ps((float *)ans_cur);

        for (size_t z = 0; z < common_z; z++) {
                SN_MAT_KERNEL_LOCAL const *right_cur = right + GET_INDEX(common_z, x, z);
                SN_MAT_KERNEL_LOCAL const *left_cur = left + GET_INDEX(common_y, z, y);

                // full right load：[kb_col0 | kb_col1 | kb_col2 | kb_col3]
                register __m512 b = _mm512_load_ps((float *)right_cur);

                // k=0: broadcast ka_col0 到4个lane，permute_ps 取 kb(0,j)
                register __m512 ak;
                ak = _mm512_broadcast_f32x4(left_cur->f32_x[0]);
                c = _mm512_fmadd_ps(ak, _mm512_permute_ps(b, _FMA_SELECT(0)), c);

                // k=1
                ak = _mm512_broadcast_f32x4(left_cur->f32_x[1]);
                c = _mm512_fmadd_ps(ak, _mm512_permute_ps(b, _FMA_SELECT(1)), c);

                // k=2
                ak = _mm512_broadcast_f32x4(left_cur->f32_x[2]);
                c = _mm512_fmadd_ps(ak, _mm512_permute_ps(b, _FMA_SELECT(2)), c);

                // k=3
                ak = _mm512_broadcast_f32x4(left_cur->f32_x[3]);
                c = _mm512_fmadd_ps(ak, _mm512_permute_ps(b, _FMA_SELECT(3)), c);
        }
        _mm512_store_ps((float *)ans_cur, c);
}

// double
#define DOUBLE_KERNEL_FMA_CYCLE(i)                                            \
        do {                                                                  \
                /* k = 0 */                                                   \
                register __m128d b_lo = _mm_load_pd(right_cur + (2 * i + 0)); \
                register __m128d b_hi = _mm_load_pd(right_cur + (2 * i + 1)); \
                register __m128d a_lo = _mm_load_pd(left_cur + (0 * 2 + 0));  \
                register __m128d a_hi = _mm_load_pd(left_cur + (0 * 2 + 1));  \
                register __m128d b = _mm_shuffle_pd(b_lo, b_lo, 0x0);         \
                c##i##_lo = _mm_add_pd(c##i##_lo, _mm_mul_pd(a_lo, b));       \
                c##i##_hi = _mm_add_pd(c##i##_hi, _mm_mul_pd(a_hi, b));       \
                                                                              \
                /* group = 1 */                                               \
                a_lo = _mm_load_pd(left_cur + (1 * 2 + 0));                   \
                a_hi = _mm_load_pd(left_cur + (1 * 2 + 1));                   \
                b = _mm_shuffle_pd(b_lo, b_lo, 0x3);                          \
                c##i##_lo = _mm_add_pd(c##i##_lo, _mm_mul_pd(a_lo, b));       \
                c##i##_hi = _mm_add_pd(c##i##_hi, _mm_mul_pd(a_hi, b));       \
                                                                              \
                /* groupp = 2 */                                              \
                a_lo = _mm_load_pd(left_cur + (2 * 2 + 0));                   \
                a_hi = _mm_load_pd(left_cur + (2 * 2 + 1));                   \
                b = _mm_shuffle_pd(b_hi, b_hi, 0x0);                          \
                c##i##_lo = _mm_add_pd(c##i##_lo, _mm_mul_pd(a_lo, b));       \
                c##i##_hi = _mm_add_pd(c##i##_hi, _mm_mul_pd(a_hi, b));       \
                                                                              \
                /* froup = 3 */                                               \
                a_lo = _mm_load_pd(left_cur + (3 * 2 + 0));                   \
                a_hi = _mm_load_pd(left_cur + (3 * 2 + 1));                   \
                b = _mm_shuffle_pd(b_hi, b_hi, 0x3);                          \
                c##i##_lo = _mm_add_pd(c##i##_lo, _mm_mul_pd(a_lo, b));       \
                c##i##_hi = _mm_add_pd(c##i##_hi, _mm_mul_pd(a_hi, b));       \
        } while (0)

SN_MK_PROC_DECL_STD(double, sse, , fma)
{
        __mk_param_std(TILE, LEFT_RIGHT_ANS, double);

        MAT_KERNEL(double)
        const *ans_cur = ans + GET_INDEX(common_y, x, y); // GetKernelAddressUnsafe

        register __m128d c0_lo = _mm_load_pd(ans_cur->f64_x + (0 * 2 + 0));
        register __m128d c0_hi = _mm_load_pd(ans_cur->f64_x + (0 * 2 + 1));
        register __m128d c1_lo = _mm_load_pd(ans_cur->f64_x + (1 * 2 + 0));
        register __m128d c1_hi = _mm_load_pd(ans_cur->f64_x + (1 * 2 + 1));
        register __m128d c2_lo = _mm_load_pd(ans_cur->f64_x + (2 * 2 + 0));
        register __m128d c2_hi = _mm_load_pd(ans_cur->f64_x + (2 * 2 + 1));
        register __m128d c3_lo = _mm_load_pd(ans_cur->f64_x + (3 * 2 + 0));
        register __m128d c3_hi = _mm_load_pd(ans_cur->f64_x + (3 * 2 + 1));

        for (size_t z = 0; z < common_z; z++) {
                SN_MAT_KERNEL_LOCAL const *right_cur = right + GET_INDEX(common_z, x, z),
                                          const *left_cur = left + GET_INDEX(common_y, z, y);

                DOUBLE_KERNEL_FMA_CYCLE(0);

                DOUBLE_KERNEL_FMA_CYCLE(1);

                DOUBLE_KERNEL_FMA_CYCLE(2);

                DOUBLE_KERNEL_FMA_CYCLE(3);
        }

        _mm_store_pd(ans_cur->f64_x + (0 * 2 + 0), c0_lo);
        _mm_store_pd(ans_cur->f64_x + (0 * 2 + 1), c0_hi);
        _mm_store_pd(ans_cur->f64_x + (1 * 2 + 0), c1_lo);
        _mm_store_pd(ans_cur->f64_x + (1 * 2 + 1), c1_hi);
        _mm_store_pd(ans_cur->f64_x + (2 * 2 + 0), c2_lo);
        _mm_store_pd(ans_cur->f64_x + (2 * 2 + 1), c2_hi);
        _mm_store_pd(ans_cur->f64_x + (3 * 2 + 0), c3_lo);
        _mm_store_pd(ans_cur->f64_x + (3 * 2 + 1), c3_hi);
}

SN_MK_PROC_DECL_STD(double, avx, , fma)
{
        __mk_param_std(TILE, LEFT_RIGHT_ANS, double);

        MAT_KERNEL(double)
        const *ans_cur = ans + GET_INDEX(common_y, x, y); // GetKernelAddressUnsafe

        register __m256d c0 = _mm256_load_pd((double *)&ans_cur->f64_y[0]);
        register __m256d c1 = _mm256_load_pd((double *)&ans_cur->f64_y[1]);
        register __m256d c2 = _mm256_load_pd((double *)&ans_cur->f64_y[2]);
        register __m256d c3 = _mm256_load_pd((double *)&ans_cur->f64_y[3]);
        register __m256d bcol;

        for (size_t z = 0; z < common_z; z++) {
                SN_MAT_KERNEL_LOCAL const *right_cur = right + GET_INDEX(common_z, x, z),
                                          const *left_cur = left + GET_INDEX(common_y, z, y);

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

        MAT_KERNEL(double)
        const *ans_cur = ans + GET_INDEX(common_y, x, y); // GetKernelAddressUnsafe

        register __m256d c0 = _mm256_load_pd((double *)&ans_cur->f64_y[0]);
        register __m256d c1 = _mm256_load_pd((double *)&ans_cur->f64_y[1]);
        register __m256d c2 = _mm256_load_pd((double *)&ans_cur->f64_y[2]);
        register __m256d c3 = _mm256_load_pd((double *)&ans_cur->f64_y[3]);
        register __m256d bcol;

        for (size_t z = 0; z < common_z; z++) {
                SN_MAT_KERNEL_LOCAL const *right_cur = right + GET_INDEX(common_z, x, z),
                                          const *left_cur = left + GET_INDEX(common_y, z, y);

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

        MAT_KERNEL(double)
        const *ans_cur = ans + GET_INDEX(common_y, x, y);

        register __m512d c01 = _mm512_load_pd((double *)&ans->f64_z[0]);
        register __m512d c23 = _mm512_load_pd((double *)&ans->f64_z[1]);

        for (size_t z = 0; z < common_z; z++) {
                SN_MAT_KERNEL_LOCAL const *right_cur = right + GET_INDEX(common_z, x, z),
                                          const *left_cur = left + GET_INDEX(common_y, z, y);

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
