#include "sn_target.h"

#if SN_IS_ARCH(SN_ARCH_X86_64)

#include "sn_intrinsic.h"
#include "sn_intrincontext.x86.h"

#define _FMA_SELECT(i) _MM_SHUFFLE(i, i, i, i)

// float

DECLARE_KER_PROC_LEFT_RIGHT_ANS(float, sse, , fma)
{
        const __m128 a0 = _mm_load_ps((float *)&left->f32_x[0]);
        const __m128 a1 = _mm_load_ps((float *)&left->f32_x[1]);
        const __m128 a2 = _mm_load_ps((float *)&left->f32_x[2]);
        const __m128 a3 = _mm_load_ps((float *)&left->f32_x[3]);

        __m128 c0 = _mm_load_ps((float *)&ans->f32_x[0]);
        __m128 c1 = _mm_load_ps((float *)&ans->f32_x[1]);
        __m128 c2 = _mm_load_ps((float *)&ans->f32_x[2]);
        __m128 c3 = _mm_load_ps((float *)&ans->f32_x[3]);

        __m128 bcol;

        /* Column 0 */
        bcol = _mm_load_ps((float *)&right->f32_x[0]);
        c0 = _mm_add_ps(c0, _mm_mul_ps(a0, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(0))));
        c0 = _mm_add_ps(c0, _mm_mul_ps(a1, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(1))));
        c0 = _mm_add_ps(c0, _mm_mul_ps(a2, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(2))));
        c0 = _mm_add_ps(c0, _mm_mul_ps(a3, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(3))));

        /* Column 1 */
        bcol = _mm_load_ps((float *)&right->f32_x[1]);
        c1 = _mm_add_ps(c1, _mm_mul_ps(a0, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(0))));
        c1 = _mm_add_ps(c1, _mm_mul_ps(a1, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(1))));
        c1 = _mm_add_ps(c1, _mm_mul_ps(a2, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(2))));
        c1 = _mm_add_ps(c1, _mm_mul_ps(a3, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(3))));

        /* Column 2 */
        bcol = _mm_load_ps((float *)&right->f32_x[2]);
        c2 = _mm_add_ps(c2, _mm_mul_ps(a0, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(0))));
        c2 = _mm_add_ps(c2, _mm_mul_ps(a1, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(1))));
        c2 = _mm_add_ps(c2, _mm_mul_ps(a2, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(2))));
        c2 = _mm_add_ps(c2, _mm_mul_ps(a3, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(3))));

        /* Column 3 */
        bcol = _mm_load_ps((float *)&right->f32_x[3]);
        c3 = _mm_add_ps(c3, _mm_mul_ps(a0, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(0))));
        c3 = _mm_add_ps(c3, _mm_mul_ps(a1, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(1))));
        c3 = _mm_add_ps(c3, _mm_mul_ps(a2, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(2))));
        c3 = _mm_add_ps(c3, _mm_mul_ps(a3, _mm_shuffle_ps(bcol, bcol, _FMA_SELECT(3))));

        _mm_store_ps((float *)&ans->f32_x[0], c0);
        _mm_store_ps((float *)&ans->f32_x[1], c1);
        _mm_store_ps((float *)&ans->f32_x[2], c2);
        _mm_store_ps((float *)&ans->f32_x[3], c3);
}

DECLARE_KER_PROC_LEFT_RIGHT_ANS(float, avx, , fma)
{
        static const __m256i b01_shift = { .m256i_i32 = { 0, 1, 2, 3, 5, 6, 7, 4 } };
        static const __m256i b23_shift = { .m256i_i32 = { 2, 3, 0, 1, 7, 4, 5, 6 } };

        register __m256 a01 = _mm256_load_ps((float *)&left->f32_y[0]);
        register __m256 a23 = _mm256_load_ps((float *)&left->f32_y[1]);

        register __m256 b01 = _mm256_load_ps((float *)&right->f32_y[0]);
        register __m256 b23 = _mm256_load_ps((float *)&right->f32_y[1]);
        b01 = _mm256_permutevar_ps(b01, b01_shift);
        b23 = _mm256_permutevar_ps(b23, b23_shift);

        register __m256 c01 = _mm256_load_ps((float *)&ans->f32_y[0]);
        register __m256 c23 = _mm256_load_ps((float *)&ans->f32_y[1]);

        //group 0
        register __m256 b01_perm = _mm256_shuffle_ps(b01, b01, _FMA_SELECT(0));
        register __m256 b23_perm = _mm256_shuffle_ps(b23, b23, _FMA_SELECT(0));
        c01 = _mm256_add_ps(_mm256_mul_ps(a01, b01_perm), c01);
        c23 = _mm256_add_ps(_mm256_mul_ps(a23, b23_perm), c23);

        //group 2
        b01_perm = _mm256_shuffle_ps(b01, b01, _FMA_SELECT(2));
        b23_perm = _mm256_shuffle_ps(b23, b23, _FMA_SELECT(2));
        c01 = _mm256_add_ps(_mm256_mul_ps(a23, b01_perm), c01);
        c23 = _mm256_add_ps(_mm256_mul_ps(a01, b23_perm), c23);

        //group 1
        register __m256 a12 = _mm256_permute2f128_ps(a01, a23, 0x21);
        register __m256 a30 = _mm256_permute2f128_ps(a01, a23, 0x30);
        b01_perm = _mm256_shuffle_ps(b01, b01, _FMA_SELECT(1));
        b23_perm = _mm256_shuffle_ps(b23, b23, _FMA_SELECT(1));
        c01 = _mm256_add_ps(_mm256_mul_ps(a12, b01_perm), c01);
        c23 = _mm256_add_ps(_mm256_mul_ps(a30, b23_perm), c23);

        //group 3
        b01_perm = _mm256_shuffle_ps(b01, b01, _FMA_SELECT(3));
        b23_perm = _mm256_shuffle_ps(b23, b23, _FMA_SELECT(3));
        c01 = _mm256_add_ps(_mm256_mul_ps(a30, b01_perm), c01);
        c23 = _mm256_add_ps(_mm256_mul_ps(a12, b23_perm), c23);

        ans->f32_y[0] = c01;
        ans->f32_y[1] = c23;
}

DECLARE_KER_PROC_LEFT_RIGHT_ANS(float, avx, _fma, fma)
{
        static const __m256i b01_shift = { .m256i_i32 = { 0, 1, 2, 3, 5, 6, 7, 4 } };
        static const __m256i b23_shift = { .m256i_i32 = { 2, 3, 0, 1, 7, 4, 5, 6 } };

        register __m256 a01 = _mm256_load_ps((float *)&left->f32_y[0]);
        register __m256 a23 = _mm256_load_ps((float *)&left->f32_y[1]);

        register __m256 b01 = _mm256_load_ps((float *)&right->f32_y[0]);
        register __m256 b23 = _mm256_load_ps((float *)&right->f32_y[1]);
        b01 = _mm256_permutevar_ps(b01, b01_shift);
        b23 = _mm256_permutevar_ps(b23, b23_shift);

        register __m256 c01 = _mm256_load_ps((float *)&ans->f32_y[0]);
        register __m256 c23 = _mm256_load_ps((float *)&ans->f32_y[1]);

        //group 0
        register __m256 b01_perm = _mm256_shuffle_ps(b01, b01, _FMA_SELECT(0));
        register __m256 b23_perm = _mm256_shuffle_ps(b23, b23, _FMA_SELECT(0));
        c01 = _mm256_fmadd_ps(a01, b01_perm, c01);
        c23 = _mm256_fmadd_ps(a23, b23_perm, c23);

        //group 2
        b01_perm = _mm256_shuffle_ps(b01, b01, _FMA_SELECT(2));
        b23_perm = _mm256_shuffle_ps(b23, b23, _FMA_SELECT(2));
        c01 = _mm256_fmadd_ps(a23, b01_perm, c01);
        c23 = _mm256_fmadd_ps(a01, b23_perm, c23);

        //group 1
        register __m256 a12 = _mm256_permute2f128_ps(a01, a23, 0x21);
        register __m256 a30 = _mm256_permute2f128_ps(a01, a23, 0x30);
        b01_perm = _mm256_shuffle_ps(b01, b01, _FMA_SELECT(1));
        b23_perm = _mm256_shuffle_ps(b23, b23, _FMA_SELECT(1));
        c01 = _mm256_fmadd_ps(a12, b01_perm, c01);
        c23 = _mm256_fmadd_ps(a30, b23_perm, c23);

        //group 3
        b01_perm = _mm256_shuffle_ps(b01, b01, _FMA_SELECT(3));
        b23_perm = _mm256_shuffle_ps(b23, b23, _FMA_SELECT(3));
        c01 = _mm256_fmadd_ps(a30, b01_perm, c01);
        c23 = _mm256_fmadd_ps(a12, b23_perm, c23);

        ans->f32_y[0] = c01;
        ans->f32_y[1] = c23;
}

DECLARE_KER_PROC_LEFT_RIGHT_ANS(float, 512, , fma)
{
        static const __m512i b_shift = { .m512i_i32 = { 0, 1, 2, 3, 5, 6, 7, 4, 10, 11, 8, 9, 15,
                                                        12, 13, 14 } };

        register __m512 a = _mm512_load_ps((float *)left);
        register __m512 b = _mm512_load_ps((float *)right);
        register __m512 c = _mm512_load_ps((float *)ans);

        b = _mm512_permutexvar_ps(b_shift, b);
        register __m512 a_perm = _mm512_shuffle_f32x4(a, a, _MM_SHUFFLE(3, 2, 1, 0));
        register __m512 b_perm = _mm512_permute_ps(b, _FMA_SELECT(0));
        c = _mm512_fmadd_ps(a_perm, b_perm, c);

        a_perm = _mm512_shuffle_f32x4(a, a, _MM_SHUFFLE(2, 1, 0, 3));
        b_perm = _mm512_permute_ps(b, _FMA_SELECT(1));
        c = _mm512_fmadd_ps(a_perm, b_perm, c);

        a_perm = _mm512_shuffle_f32x4(a, a, _MM_SHUFFLE(1, 0, 3, 2));
        b_perm = _mm512_permute_ps(b, _FMA_SELECT(2));
        c = _mm512_fmadd_ps(a_perm, b_perm, c);

        a_perm = _mm512_shuffle_f32x4(a, a, _MM_SHUFFLE(0, 3, 2, 1));
        b_perm = _mm512_permute_ps(b, _FMA_SELECT(3));
        c = _mm512_fmadd_ps(a_perm, b_perm, c);

        _mm512_store_ps((float *)ans, c);
}

// double

DECLARE_KER_PROC_LEFT_RIGHT_ANS(double, sse, , fma)
{
        for (int col = 0; col < 4; ++col) {
                /* current answer column (two chunks: rows 0-1 and 2-3) */
                register __m128d c_lo = _mm_load_pd(ans->f64_x + (col * 2 + 0));
                register __m128d c_hi = _mm_load_pd(ans->f64_x + (col * 2 + 1));

                /* right matrix column split into two chunks */
                register __m128d b_lo = _mm_load_pd(right + (2 * col + 0));
                register __m128d b_hi = _mm_load_pd(right + (2 * col + 1));

                /* k = 0 */
                register __m128d a_lo = _mm_load_pd(left + (0 * 2 + 0));
                register __m128d a_hi = _mm_load_pd(left + (0 * 2 + 1));
                register __m128d b = _mm_shuffle_pd(b_lo, b_lo, 0x0);
                c_lo = _mm_add_pd(c_lo, _mm_mul_pd(a_lo, b));
                c_hi = _mm_add_pd(c_hi, _mm_mul_pd(a_hi, b));

                /* k = 1 */
                a_lo = _mm_load_pd(left + (1 * 2 + 0));
                a_hi = _mm_load_pd(left + (1 * 2 + 1));
                b = _mm_shuffle_pd(b_lo, b_lo, 0x3);
                c_lo = _mm_add_pd(c_lo, _mm_mul_pd(a_lo, b));
                c_hi = _mm_add_pd(c_hi, _mm_mul_pd(a_hi, b));

                /* k = 2 */
                a_lo = _mm_load_pd(left + (1 * 2 + 0));
                a_hi = _mm_load_pd(left + (1 * 2 + 1));
                b = _mm_shuffle_pd(b_hi, b_hi, 0x0);
                c_lo = _mm_add_pd(c_lo, _mm_mul_pd(a_lo, b));
                c_hi = _mm_add_pd(c_hi, _mm_mul_pd(a_hi, b));

                /* k = 3 */
                a_lo = _mm_load_pd(left + (1 * 2 + 0));
                a_hi = _mm_load_pd(left + (1 * 2 + 1));
                b = _mm_shuffle_pd(b_hi, b_hi, 0x3);
                c_lo = _mm_add_pd(c_lo, _mm_mul_pd(a_lo, b));
                c_hi = _mm_add_pd(c_hi, _mm_mul_pd(a_hi, b));

                ans->f64_x[col * 2 + 0] = c_lo;
                ans->f64_x[col * 2 + 1] = c_hi;
        }
}

DECLARE_KER_PROC_LEFT_RIGHT_ANS(double, avx, , fma)
{
        const __m256d a0 = _mm256_load_pd((double *)&left->f64_y[0]);
        const __m256d a1 = _mm256_load_pd((double *)&left->f64_y[1]);
        const __m256d a2 = _mm256_load_pd((double *)&left->f64_y[2]);
        const __m256d a3 = _mm256_load_pd((double *)&left->f64_y[3]);

        __m256d b0 = _mm256_load_pd((double *)&right->f64_y[0]);
        __m256d b1 = _mm256_load_pd((double *)&right->f64_y[1]);
        __m256d b2 = _mm256_load_pd((double *)&right->f64_y[2]);
        __m256d b3 = _mm256_load_pd((double *)&right->f64_y[3]);

        register __m256d ccol;

        /* Column 0 */
        ccol = _mm256_load_pd((double *)&ans->f32_y[0]);
        ccol = _mm256_add_pd(ccol, _mm256_mul_pd(a0, _mm256_permute4x64_pd(b0, _FMA_SELECT(0))));
        ccol = _mm256_add_pd(ccol, _mm256_mul_pd(a1, _mm256_permute4x64_pd(b0, _FMA_SELECT(1))));
        ccol = _mm256_add_pd(ccol, _mm256_mul_pd(a2, _mm256_permute4x64_pd(b0, _FMA_SELECT(2))));
        ccol = _mm256_add_pd(ccol, _mm256_mul_pd(a3, _mm256_permute4x64_pd(b0, _FMA_SELECT(3))));
        ans->f64_y[0] = ccol;
        /* Column 1 */
        ccol = _mm256_load_pd((double *)&ans->f32_y[1]);
        ccol = _mm256_add_pd(ccol, _mm256_mul_pd(a0, _mm256_permute4x64_pd(b1, _FMA_SELECT(0))));
        ccol = _mm256_add_pd(ccol, _mm256_mul_pd(a1, _mm256_permute4x64_pd(b1, _FMA_SELECT(1))));
        ccol = _mm256_add_pd(ccol, _mm256_mul_pd(a2, _mm256_permute4x64_pd(b1, _FMA_SELECT(2))));
        ccol = _mm256_add_pd(ccol, _mm256_mul_pd(a3, _mm256_permute4x64_pd(b1, _FMA_SELECT(3))));
        ans->f64_y[1] = ccol;
        /* Column 2 */
        ccol = _mm256_load_pd((double *)&ans->f32_y[2]);
        ccol = _mm256_add_pd(ccol, _mm256_mul_pd(a0, _mm256_permute4x64_pd(b2, _FMA_SELECT(0))));
        ccol = _mm256_add_pd(ccol, _mm256_mul_pd(a1, _mm256_permute4x64_pd(b2, _FMA_SELECT(1))));
        ccol = _mm256_add_pd(ccol, _mm256_mul_pd(a2, _mm256_permute4x64_pd(b2, _FMA_SELECT(2))));
        ccol = _mm256_add_pd(ccol, _mm256_mul_pd(a3, _mm256_permute4x64_pd(b2, _FMA_SELECT(3))));
        ans->f64_y[2] = ccol;
        /* Column 3 */
        ccol = _mm256_load_pd((double *)&ans->f32_y[3]);
        ccol = _mm256_add_pd(ccol, _mm256_mul_pd(a0, _mm256_permute4x64_pd(b3, _FMA_SELECT(0))));
        ccol = _mm256_add_pd(ccol, _mm256_mul_pd(a1, _mm256_permute4x64_pd(b3, _FMA_SELECT(1))));
        ccol = _mm256_add_pd(ccol, _mm256_mul_pd(a2, _mm256_permute4x64_pd(b3, _FMA_SELECT(2))));
        ccol = _mm256_add_pd(ccol, _mm256_mul_pd(a3, _mm256_permute4x64_pd(b3, _FMA_SELECT(3))));
        ans->f64_y[3] = ccol;
}

DECLARE_KER_PROC_LEFT_RIGHT_ANS(double, avx, _fma, fma)
{
        const __m256d a0 = _mm256_load_pd((double *)&left->f64_y[0]);
        const __m256d a1 = _mm256_load_pd((double *)&left->f64_y[1]);
        const __m256d a2 = _mm256_load_pd((double *)&left->f64_y[2]);
        const __m256d a3 = _mm256_load_pd((double *)&left->f64_y[3]);

        __m256d b0 = _mm256_load_pd((double *)&right->f64_y[0]);
        __m256d b1 = _mm256_load_pd((double *)&right->f64_y[1]);
        __m256d b2 = _mm256_load_pd((double *)&right->f64_y[2]);
        __m256d b3 = _mm256_load_pd((double *)&right->f64_y[3]);

        register __m256d ccol;

        /* Column 0 */
        ccol = _mm256_load_pd((double *)&ans->f32_y[0]);
        ccol = _mm256_fmadd_pd(a0, _mm256_permute4x64_pd(b0, _FMA_SELECT(0)), ccol);
        ccol = _mm256_fmadd_pd(a1, _mm256_permute4x64_pd(b0, _FMA_SELECT(1)), ccol);
        ccol = _mm256_fmadd_pd(a2, _mm256_permute4x64_pd(b0, _FMA_SELECT(2)), ccol);
        ccol = _mm256_fmadd_pd(a3, _mm256_permute4x64_pd(b0, _FMA_SELECT(3)), ccol);
        ans->f64_y[0] = ccol;
        /* Column 1 */
        ccol = _mm256_load_pd((double *)&ans->f32_y[1]);
        ccol = _mm256_fmadd_pd(a0, _mm256_permute4x64_pd(b1, _FMA_SELECT(0)), ccol);
        ccol = _mm256_fmadd_pd(a1, _mm256_permute4x64_pd(b1, _FMA_SELECT(1)), ccol);
        ccol = _mm256_fmadd_pd(a2, _mm256_permute4x64_pd(b1, _FMA_SELECT(2)), ccol);
        ccol = _mm256_fmadd_pd(a3, _mm256_permute4x64_pd(b1, _FMA_SELECT(3)), ccol);
        ans->f64_y[1] = ccol;
        /* Column 2 */
        ccol = _mm256_load_pd((double *)&ans->f32_y[2]);
        ccol = _mm256_fmadd_pd(a0, _mm256_permute4x64_pd(b2, _FMA_SELECT(0)), ccol);
        ccol = _mm256_fmadd_pd(a1, _mm256_permute4x64_pd(b2, _FMA_SELECT(1)), ccol);
        ccol = _mm256_fmadd_pd(a2, _mm256_permute4x64_pd(b2, _FMA_SELECT(2)), ccol);
        ccol = _mm256_fmadd_pd(a3, _mm256_permute4x64_pd(b2, _FMA_SELECT(3)), ccol);
        ans->f64_y[2] = ccol;
        /* Column 3 */
        ccol = _mm256_load_pd((double *)&ans->f32_y[3]);
        ccol = _mm256_fmadd_pd(a0, _mm256_permute4x64_pd(b3, _FMA_SELECT(0)), ccol);
        ccol = _mm256_fmadd_pd(a1, _mm256_permute4x64_pd(b3, _FMA_SELECT(1)), ccol);
        ccol = _mm256_fmadd_pd(a2, _mm256_permute4x64_pd(b3, _FMA_SELECT(2)), ccol);
        ccol = _mm256_fmadd_pd(a3, _mm256_permute4x64_pd(b3, _FMA_SELECT(3)), ccol);
        ans->f64_y[3] = ccol;
}

DECLARE_KER_PROC_LEFT_RIGHT_ANS(double, 512, , fma)
{
        static const __m512i b01_shift = { .m512i_i64 = { 0, 1, 2, 3, 5, 6, 7, 4 } };
        static const __m512i b23_shift = { .m512i_i64 = { 2, 3, 0, 1, 7, 4, 5, 6 } };

        register __m512d a01 = _mm512_load_pd((double *)&left->f64_z[0]);
        register __m512d a23 = _mm512_load_pd((double *)&left->f64_z[1]);

        register __m512d b01 = _mm512_load_pd((double *)&right->f64_z[0]);
        register __m512d b23 = _mm512_load_pd((double *)&right->f64_z[1]);
        b01 = _mm512_permutexvar_pd(b01_shift, b01);
        b23 = _mm512_permutexvar_pd(b23_shift, b23);

        register __m512d c01 = _mm512_load_pd((float *)&ans->f64_z[0]);
        register __m512d c23 = _mm512_load_pd((float *)&ans->f64_z[1]);

        //group 0
        register __m512d b01_perm = _mm512_shuffle_pd(b01, b01, _FMA_SELECT(0));
        register __m512d b23_perm = _mm512_shuffle_pd(b23, b23, _FMA_SELECT(0));
        c01 = _mm512_fmadd_pd(a01, b01_perm, c01);
        c23 = _mm512_fmadd_pd(a23, b23_perm, c23);

        //group 2
        b01_perm = _mm512_shuffle_pd(b01, b01, _FMA_SELECT(2));
        b23_perm = _mm512_shuffle_pd(b23, b23, _FMA_SELECT(2));
        c01 = _mm512_fmadd_pd(a23, b01_perm, c01);
        c23 = _mm512_fmadd_pd(a01, b23_perm, c23);

        //group 1
        register __m512d a12 = _mm512_shuffle_f64x2(a01, a23, _MM_SHUFFLE(1, 0, 3, 2));
        register __m512d a30 = _mm512_shuffle_f64x2(a23, a01, _MM_SHUFFLE(1, 0, 3, 2));
        b01_perm = _mm512_shuffle_pd(b01, b01, _FMA_SELECT(1));
        b23_perm = _mm512_shuffle_pd(b23, b23, _FMA_SELECT(1));
        c01 = _mm512_fmadd_pd(a12, b01_perm, c01);
        c23 = _mm512_fmadd_pd(a30, b23_perm, c23);

        //group 3
        b01_perm = _mm512_shuffle_pd(b01, b01, _FMA_SELECT(3));
        b23_perm = _mm512_shuffle_pd(b23, b23, _FMA_SELECT(3));
        c01 = _mm512_fmadd_pd(a30, b01_perm, c01);
        c23 = _mm512_fmadd_pd(a12, b23_perm, c23);

        ans->f64_z[0] = c01;
        ans->f64_z[1] = c23;
}

#endif
