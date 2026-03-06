#include "sn_target.h"

#if SN_IS_ARCH(SN_ARCH_X86_64)

#include "sn_matkernel.h"

DECLARE_BUILD_PANEL(avx, float)
{
        const int Q = 8; /* fixed for AVX float panel */

        /* Neighbor kernels; substitute global zero kernel where out-of-bounds */
        const int has_right = (col_index + 1) < col_length;
        const int has_bottom = (row_index + 1) < row_length;

        const MAT_KERNEL(float) *const k_lt = matrix + (row_index * col_length + col_index);
        const MAT_KERNEL(float) *const k_rt =
                has_right ? (matrix + (row_index * col_length + (col_index + 1))) :
                            (const MAT_KERNEL(float) *)&SN_ZERO_KERNEL;
        const MAT_KERNEL(float) *const k_lb =
                has_bottom ? (matrix + ((row_index + 1) * col_length + col_index)) :
                             (const MAT_KERNEL(float) *)&SN_ZERO_KERNEL;
        const MAT_KERNEL(float) *const k_rb =
                (has_right && has_bottom) ?
                        (matrix + ((row_index + 1) * col_length + (col_index + 1))) :
                        (const MAT_KERNEL(float) *)&SN_ZERO_KERNEL;

        register __m128 top128, bot128;
        register __m256 col256;

        for (int pc = 0; pc < Q; ++pc) {
                const int is_right_half = (pc >= 4);
                const int kc_col = pc & 3; /* column index inside 4x4 kernel */

                const MAT_KERNEL(float) *src_top = is_right_half ? k_rt : k_lt;
                const MAT_KERNEL(float) *src_bot = is_right_half ? k_rb : k_lb;

                /* Vector loads: each kernel column is a compact __m128 */
                top128 = src_top->f32_x[kc_col];
                bot128 = src_bot->f32_x[kc_col];

                /* Combine into an AVX column: low 128 = top, high 128 = bottom */
                col256 = _mm256_set_m128(bot128, top128);

                PANEL_COL_VEC((*panel), pc, 0) = col256;
        }
}

DECLARE_BUILD_PANEL(512, float)
{
        const MAT_KERNEL(float) *const zero = (const MAT_KERNEL(float) *)&SN_ZERO_KERNEL;
        const MAT_KERNEL(float) * k[4][4];

        for (int kc = 0; kc < 4; ++kc) {
                for (int kr = 0; kr < 4; ++kr) {
                        const int col = col_index + kc;
                        const int row = row_index + kr;
                        k[kc][kr] = (col < col_length && row < row_length) ?
                                            (matrix + (row * col_length + col)) :
                                            zero;
                }
        }

        /* Each panel column corresponds to a kernel column (0..3) within a block col (0..3). */
        for (int pc = 0; pc < 16; ++pc) {
                const int blk_col = pc >> 2; /* which kernel column block (0..3) */
                const int kc_col = pc & 3; /* column inside a 4x4 kernel */

                const __m128 r0 = k[blk_col][0]->f32_x[kc_col];
                const __m128 r1 = k[blk_col][1]->f32_x[kc_col];
                const __m128 r2 = k[blk_col][2]->f32_x[kc_col];
                const __m128 r3 = k[blk_col][3]->f32_x[kc_col];

                const __m256 top = _mm256_set_m128(r1, r0);
                const __m256 bot = _mm256_set_m128(r3, r2);

                __m512 col = _mm512_castps256_ps512(top);
                col = _mm512_insertf32x8(col, bot, 1);

                PANEL_COL_VEC((*panel), pc, 0) = col;
        }
}

DECLARE_BUILD_PANEL(512, double)
{
        const int Q = 8; /* max(4, lanes) for double with ZMM -> 8 columns */

        const int has_right = (col_index + 1) < col_length;
        const int has_bottom = (row_index + 1) < row_length;

        const MAT_KERNEL(double) *const k_lt = matrix + (row_index * col_length + col_index);
        const MAT_KERNEL(double) *const k_rt =
                has_right ? (matrix + (row_index * col_length + (col_index + 1))) :
                            (const MAT_KERNEL(double) *)&SN_ZERO_KERNEL;
        const MAT_KERNEL(double) *const k_lb =
                has_bottom ? (matrix + ((row_index + 1) * col_length + col_index)) :
                             (const MAT_KERNEL(double) *)&SN_ZERO_KERNEL;
        const MAT_KERNEL(double) *const k_rb =
                (has_right && has_bottom) ?
                        (matrix + ((row_index + 1) * col_length + (col_index + 1))) :
                        (const MAT_KERNEL(double) *)&SN_ZERO_KERNEL;

        for (int pc = 0; pc < Q; ++pc) {
                const int is_right_half = (pc >= 4);
                const int kc_col = pc & 3; /* column index inside 4x4 kernel */

                const MAT_KERNEL(double) *src_top = is_right_half ? k_rt : k_lt;
                const MAT_KERNEL(double) *src_bot = is_right_half ? k_rb : k_lb;

                const __m256d top256 = src_top->f64_y[kc_col];
                const __m256d bot256 = src_bot->f64_y[kc_col];

                __m512d col = _mm512_castpd256_pd512(top256);
                col = _mm512_insertf64x4(col, bot256, 1);

                PANEL_COL_VEC((*panel), pc, 0) = _mm512_castpd_ps(col);
        }
}

DECLARE_STORE_PANEL(avx, float)
{
        const int Q = 8; /* fixed for AVX float panel */

        /* Local dummy kernel to swallow out-of-bounds writes; no init needed */
        MAT_KERNEL(float) zero;

        const int has_right = (col_index + 1) < col_length;
        const int has_bottom = (row_index + 1) < row_length;

        MAT_KERNEL(float) *const k_lt = (col_index < col_length && row_index < row_length) ?
                                                (matrix + (row_index * col_length + col_index)) :
                                                &zero;
        MAT_KERNEL(float) *const k_rt =
                has_right && (row_index < row_length) ?
                        (matrix + (row_index * col_length + (col_index + 1))) :
                        &zero;
        MAT_KERNEL(float) *const k_lb =
                has_bottom && (col_index < col_length) ?
                        (matrix + ((row_index + 1) * col_length + col_index)) :
                        &zero;
        MAT_KERNEL(float) *const k_rb =
                (has_right && has_bottom) ?
                        (matrix + ((row_index + 1) * col_length + (col_index + 1))) :
                        &zero;

        register __m128 top128, bot128;
        int i = 0;
        for (; i < Q / 2; i++) {
                const int kc_col = i; /* column index inside 4x4 kernel */

                /* Load the panel column vector */
                const __m256 col256 = PANEL_COL_VEC((*panel), i, 0);
                /* Split into top/bottom halves */
                top128 = _mm256_castps256_ps128(col256); /* low 128 */
                bot128 = _mm256_extractf128_ps(col256, 1); /* high 128 */

                /* Store back into kernels directly (out-of-bounds writes go to dummy) */
                k_rt->f32_x[kc_col] = top128;
                k_rb->f32_x[kc_col] = bot128;
        }
        for (; i < Q; i++) {
                const int kc_col = i - 4; /* column index inside 4x4 kernel */

                /* Load the panel column vector */
                const __m256 col256 = PANEL_COL_VEC((*panel), i, 0);
                /* Split into top/bottom halves */
                top128 = _mm256_castps256_ps128(col256); /* low 128 */
                bot128 = _mm256_extractf128_ps(col256, 1); /* high 128 */

                /* Store back into kernels directly (out-of-bounds writes go to dummy) */
                k_lt->f32_x[kc_col] = top128;
                k_lb->f32_x[kc_col] = bot128;
        }
}

DECLARE_STORE_PANEL(512, float)
{
        MAT_KERNEL(float) zero;
        MAT_KERNEL(float) * k[4][4];

        for (int kc = 0; kc < 4; ++kc) {
                for (int kr = 0; kr < 4; ++kr) {
                        const int col = col_index + kc;
                        const int row = row_index + kr;
                        k[kc][kr] = (col < col_length && row < row_length) ?
                                            (matrix + (row * col_length + col)) :
                                            &zero;
                }
        }

        for (int pc = 0; pc < 16; ++pc) {
                const int blk_col = pc >> 2; /* which kernel column block (0..3) */
                const int kc_col = pc & 3; /* column inside a 4x4 kernel */

                const __m512 col = PANEL_COL_VEC((*panel), pc, 0);

                const __m256 top = _mm512_castps512_ps256(col);
                const __m256 bot = _mm512_extractf32x8_ps(col, 1);

                const __m128 r0 = _mm256_castps256_ps128(top);
                const __m128 r1 = _mm256_extractf128_ps(top, 1);
                const __m128 r2 = _mm256_castps256_ps128(bot);
                const __m128 r3 = _mm256_extractf128_ps(bot, 1);

                k[blk_col][0]->f32_x[kc_col] = r0;
                k[blk_col][1]->f32_x[kc_col] = r1;
                k[blk_col][2]->f32_x[kc_col] = r2;
                k[blk_col][3]->f32_x[kc_col] = r3;
        }
}

DECLARE_STORE_PANEL(sse, double)
{
        MAT_KERNEL(double) *dst = matrix + (row_index * col_length + col_index);

        for (int pc = 0; pc < 4; ++pc) {
                const int base = pc * 2;
                dst->f64_x[base] = _mm_castps_pd(PANEL_COL_VEC((*panel), pc, 0));
                dst->f64_x[base + 1] = _mm_castps_pd(PANEL_COL_VEC((*panel), pc, 1));
        }
}

DECLARE_STORE_PANEL(avx, double)
{
        const int Q = 8; /* fixed for AVX double panel */

        MAT_KERNEL(double) zero;

        const int has_right = (col_index + 1) < col_length;
        const int has_bottom = (row_index + 1) < row_length;

        MAT_KERNEL(double) *const k_lt = (col_index < col_length && row_index < row_length) ?
                                                 (matrix + (row_index * col_length + col_index)) :
                                                 &zero;
        MAT_KERNEL(double) *const k_rt =
                has_right && (row_index < row_length) ?
                        (matrix + (row_index * col_length + (col_index + 1))) :
                        &zero;
        MAT_KERNEL(double) *const k_lb =
                has_bottom && (col_index < col_length) ?
                        (matrix + ((row_index + 1) * col_length + col_index)) :
                        &zero;
        MAT_KERNEL(double) *const k_rb =
                (has_right && has_bottom) ?
                        (matrix + ((row_index + 1) * col_length + (col_index + 1))) :
                        &zero;

        for (int i = 0; i < Q; ++i) {
                const int kc_col = (i < 4) ? i : (i - 4);
                const __m256 top_chunk = PANEL_COL_VEC((*panel), i, 0);
                const __m256 bot_chunk = PANEL_COL_VEC((*panel), i, 1);

                MAT_KERNEL(double) *const top_dst = (i < 4) ? k_rt : k_lt;
                MAT_KERNEL(double) *const bot_dst = (i < 4) ? k_rb : k_lb;

                top_dst->f64_y[kc_col] = _mm256_castps_pd(top_chunk);
                bot_dst->f64_y[kc_col] = _mm256_castps_pd(bot_chunk);
        }
}

DECLARE_STORE_PANEL(512, double)
{
        const int Q = 8; /* max(4, lanes) -> 8 columns, one ZMM per column */

        MAT_KERNEL(double) zero;

        const int has_right = (col_index + 1) < col_length;
        const int has_bottom = (row_index + 1) < row_length;

        MAT_KERNEL(double) *const k_lt = (col_index < col_length && row_index < row_length) ?
                                                 (matrix + (row_index * col_length + col_index)) :
                                                 &zero;
        MAT_KERNEL(double) *const k_rt =
                has_right && (row_index < row_length) ?
                        (matrix + (row_index * col_length + (col_index + 1))) :
                        &zero;
        MAT_KERNEL(double) *const k_lb =
                has_bottom && (col_index < col_length) ?
                        (matrix + ((row_index + 1) * col_length + col_index)) :
                        &zero;
        MAT_KERNEL(double) *const k_rb =
                (has_right && has_bottom) ?
                        (matrix + ((row_index + 1) * col_length + (col_index + 1))) :
                        &zero;

        for (int i = 0; i < Q; ++i) {
                const int kc_col = (i < 4) ? i : (i - 4);
                const __m512 col = PANEL_COL_VEC((*panel), i, 0);
                const __m512d col_pd = _mm512_castps_pd(col);

                const __m256d top256 = _mm512_castpd512_pd256(col_pd);
                const __m256d bot256 = _mm512_extractf64x4_pd(col_pd, 1);

                MAT_KERNEL(double) *const top_dst = (i < 4) ? k_rt : k_lt;
                MAT_KERNEL(double) *const bot_dst = (i < 4) ? k_rb : k_lb;

                top_dst->f64_y[kc_col] = top256;
                bot_dst->f64_y[kc_col] = bot256;
        }
}

#endif
