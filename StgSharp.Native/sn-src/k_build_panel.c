#include "sn_matkernel.h"

DECLARE_BUILD_PANEL(AVX, float)
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
