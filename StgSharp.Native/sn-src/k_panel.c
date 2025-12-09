#include "sn_matkernel.h"

DECLARE_BUILD_PANEL(avx, float)
{
        const int Q = 8; /* fixed for AVX float panel */

        /* Neighbor kernels; substitute global zero kernel where out-of-bounds */
        const int has_right = (col_index + 1) < col_length;
        const int has_bottom = (row_index + 1) < row_length;

        const MAT_KERNEL(float) *const k_lt = matrix + (col_index * row_length + row_index);
        const MAT_KERNEL(float) *const k_rt =
                has_right ? (matrix + ((col_index + 1) * row_length + row_index)) :
                            (const MAT_KERNEL(float) *)&SN_ZERO_KERNEL;
        const MAT_KERNEL(float) *const k_lb =
                has_bottom ? (matrix + (col_index * row_length + (row_index + 1))) :
                             (const MAT_KERNEL(float) *)&SN_ZERO_KERNEL;
        const MAT_KERNEL(float) *const k_rb =
                (has_right && has_bottom) ?
                        (matrix + ((col_index + 1) * row_length + (row_index + 1))) :
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

DECLARE_STORE_PANEL(avx, float)
{
        const int Q = 8; /* fixed for AVX float panel */

        /* Local dummy kernel to swallow out-of-bounds writes; no init needed */
        MAT_KERNEL(float) zero;

        const int has_right = (col_index + 1) < col_length;
        const int has_bottom = (row_index + 1) < row_length;

        MAT_KERNEL(float) *const k_lt = (col_index < col_length && row_index < row_length) ?
                                                (matrix + (col_index * row_length + row_index)) :
                                                &zero;
        MAT_KERNEL(float) *const k_rt =
                has_right && (row_index < row_length) ?
                        (matrix + ((col_index + 1) * row_length + row_index)) :
                        &zero;
        MAT_KERNEL(float) *const k_lb =
                has_bottom && (col_index < col_length) ?
                        (matrix + (col_index * row_length + (row_index + 1))) :
                        &zero;
        MAT_KERNEL(float) *const k_rb =
                (has_right && has_bottom) ?
                        (matrix + ((col_index + 1) * row_length + (row_index + 1))) :
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
