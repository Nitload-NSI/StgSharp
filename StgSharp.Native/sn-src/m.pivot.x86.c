#include "sn_target.h"

#if SN_IS_ARCH(SN_ARCH_X86_64)

#include "sn_intrinsic.h"

// Hybrid implementation: SIMD for Abs, Scalar for Max
// Assumes Column-Major layout for both MAT_KERNEL content and Kernel sequence
INTERNAL void SN_DECL pivot_float(MAT_KERNEL(float) const *source, size_t col_begin,
                                  size_t col_length, size_t *restrict ans)
{
        float max_v[4] = { -1.0f, -1.0f, -1.0f, -1.0f };

        // Initialize ans with the first row index of the search range
        size_t start_row = col_begin * 4;
        ans[0] = start_row;
        ans[1] = start_row;
        ans[2] = start_row;
        ans[3] = start_row;

        const MAT_KERNEL(float) *k_ptr = source + col_begin;

        // Mask for absolute value: 0x7FFFFFFF (clears the sign bit)
        __m128i abs_mask = _mm_set1_epi32(0x7FFFFFFF);

        for (size_t i = 0; i < col_length; ++i) {
                const float *mat_ptr = (const float *)(k_ptr + i);
                size_t base_row = (col_begin + i) * 4;

                // Iterate over 4 columns of the kernel
                for (int c = 0; c < 4; ++c) {
                        // Load column c: 4 floats (Column-Major: contiguous in memory)
                        __m128 col_vec = _mm_load_ps(mat_ptr + c * 4);

                        // Convert to integer vector, AND with mask, convert back
                        __m128i col_i = _mm_castps_si128(col_vec);
                        col_i = _mm_and_si128(col_i, abs_mask);
                        col_vec = _mm_castsi128_ps(col_i);

                        VEC(float) vals;
                        _mm_store_ps(vals.v, col_vec);

                        // Local reduction to find the max within this column chunk (4 elements)
                        // This breaks the dependency chain and reduces branches/cmovs on the global max_v
                        float v0 = vals.v[0];
                        float v1 = vals.v[1];
                        float v2 = vals.v[2];
                        float v3 = vals.v[3];

                        // Compare pairs (0 vs 1) and (2 vs 3) - these can execute in parallel
                        int i01 = (v1 > v0) ? 1 : 0;
                        float m01 = (v1 > v0) ? v1 : v0;

                        int i23 = (v3 > v2) ? 3 : 2;
                        float m23 = (v3 > v2) ? v3 : v2;

                        // Compare winners
                        int i_local = (m23 > m01) ? i23 : i01;
                        float m_local = (m23 > m01) ? m23 : m01;

                        // Single check against global maximum
                        if (m_local > max_v[c]) {
                                max_v[c] = m_local;
                                ans[c] = base_row + i_local;
                        }
                }
        }
}

#endif
