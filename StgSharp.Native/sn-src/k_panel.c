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
                top128 = src_top->xmm[kc_col];
                bot128 = src_bot->xmm[kc_col];

                /* Combine into an AVX column: low 128 = top, high 128 = bottom */
                col256 = _mm256_set_m128(bot128, top128);

                PANEL_COL_VEC((*panel), pc, 0) = col256;
        }
}

<<<<<<< HEAD
<<<<<<< HEAD
DECLARE_BUILD_PANEL(sse, float)
{
        /* Single kernel -> 4 columns, each is one __m128 column vector. */
        const MAT_KERNEL(float) *src = matrix + (col_index * row_length + row_index);
=======
DECLARE_BUILD_PANEL(sse, float)
{
        /* Single kernel -> 4 columns, each is one __m128 column vector. */
        const MAT_KERNEL(float) *src = matrix + (row_index * col_length + col_index);
>>>>>>> stgsharp-dev/giga

        for (int pc = 0; pc < 4; ++pc) {
                PANEL_COL_VEC((*panel), pc, 0) = src->xmm[pc];
        }
}

DECLARE_BUILD_PANEL(512, float)
{
        const MAT_KERNEL(float) *const zero = (const MAT_KERNEL(float) *)&SN_ZERO_KERNEL;
<<<<<<< HEAD
        const MAT_KERNEL(float) *k[4][4];
=======
        const MAT_KERNEL(float) * k[4][4];
>>>>>>> stgsharp-dev/giga

        for (int kc = 0; kc < 4; ++kc) {
                for (int kr = 0; kr < 4; ++kr) {
                        const int col = col_index + kc;
                        const int row = row_index + kr;
                        k[kc][kr] = (col < col_length && row < row_length) ?
<<<<<<< HEAD
                                            (matrix + (col * row_length + row)) :
=======
                                            (matrix + (row * col_length + col)) :
>>>>>>> stgsharp-dev/giga
                                            zero;
                }
        }

        /* Each panel column corresponds to a kernel column (0..3) within a block col (0..3). */
        for (int pc = 0; pc < 16; ++pc) {
<<<<<<< HEAD
                const int blk_col = pc >> 2;     /* which kernel column block (0..3) */
                const int kc_col = pc & 3;       /* column inside a 4x4 kernel */
=======
                const int blk_col = pc >> 2; /* which kernel column block (0..3) */
                const int kc_col = pc & 3; /* column inside a 4x4 kernel */
>>>>>>> stgsharp-dev/giga

                const __m128 r0 = k[blk_col][0]->xmm[kc_col];
                const __m128 r1 = k[blk_col][1]->xmm[kc_col];
                const __m128 r2 = k[blk_col][2]->xmm[kc_col];
                const __m128 r3 = k[blk_col][3]->xmm[kc_col];

                const __m256 top = _mm256_set_m128(r1, r0);
                const __m256 bot = _mm256_set_m128(r3, r2);

                __m512 col = _mm512_castps256_ps512(top);
                col = _mm512_insertf32x8(col, bot, 1);

                PANEL_COL_VEC((*panel), pc, 0) = col;
        }
}

<<<<<<< HEAD
=======
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
=======
>>>>>>> stgsharp-dev/giga
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
                k_rt->xmm[kc_col] = top128;
                k_rb->xmm[kc_col] = bot128;
        }
        for (; i < Q; i++) {
                const int kc_col = i - 4; /* column index inside 4x4 kernel */

                /* Load the panel column vector */
                const __m256 col256 = PANEL_COL_VEC((*panel), i, 0);
                /* Split into top/bottom halves */
                top128 = _mm256_castps256_ps128(col256); /* low 128 */
                bot128 = _mm256_extractf128_ps(col256, 1); /* high 128 */

                /* Store back into kernels directly (out-of-bounds writes go to dummy) */
                k_lt->xmm[kc_col] = top128;
                k_lb->xmm[kc_col] = bot128;
        }
}
<<<<<<< HEAD
<<<<<<< HEAD

DECLARE_STORE_PANEL(sse, float)
{
        MAT_KERNEL(float) *dst = matrix + (col_index * row_length + row_index);
=======

DECLARE_STORE_PANEL(sse, float)
{
        MAT_KERNEL(float) *dst = matrix + (row_index * col_length + col_index);
>>>>>>> stgsharp-dev/giga

        for (int pc = 0; pc < 4; ++pc) {
                const __m128 col = PANEL_COL_VEC((*panel), pc, 0);
                dst->xmm[pc] = col;
        }
}

DECLARE_STORE_PANEL(512, float)
{
        MAT_KERNEL(float) zero;
<<<<<<< HEAD
        MAT_KERNEL(float) *k[4][4];
=======
        MAT_KERNEL(float) * k[4][4];
>>>>>>> stgsharp-dev/giga

        for (int kc = 0; kc < 4; ++kc) {
                for (int kr = 0; kr < 4; ++kr) {
                        const int col = col_index + kc;
                        const int row = row_index + kr;
                        k[kc][kr] = (col < col_length && row < row_length) ?
<<<<<<< HEAD
                                            (matrix + (col * row_length + row)) :
=======
                                            (matrix + (row * col_length + col)) :
>>>>>>> stgsharp-dev/giga
                                            &zero;
                }
        }

        for (int pc = 0; pc < 16; ++pc) {
<<<<<<< HEAD
                const int blk_col = pc >> 2;     /* which kernel column block (0..3) */
                const int kc_col = pc & 3;       /* column inside a 4x4 kernel */
=======
                const int blk_col = pc >> 2; /* which kernel column block (0..3) */
                const int kc_col = pc & 3; /* column inside a 4x4 kernel */
>>>>>>> stgsharp-dev/giga

                const __m512 col = PANEL_COL_VEC((*panel), pc, 0);

                const __m256 top = _mm512_castps512_ps256(col);
                const __m256 bot = _mm512_extractf32x8_ps(col, 1);

                const __m128 r0 = _mm256_castps256_ps128(top);
                const __m128 r1 = _mm256_extractf128_ps(top, 1);
                const __m128 r2 = _mm256_castps256_ps128(bot);
                const __m128 r3 = _mm256_extractf128_ps(bot, 1);

                k[blk_col][0]->xmm[kc_col] = r0;
                k[blk_col][1]->xmm[kc_col] = r1;
                k[blk_col][2]->xmm[kc_col] = r2;
                k[blk_col][3]->xmm[kc_col] = r3;
        }
}
<<<<<<< HEAD
=======
>>>>>>> 7bcb460a3994dda40f24cae0044b5a36f4f16515
=======
>>>>>>> stgsharp-dev/giga
