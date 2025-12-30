#include "sn_intrinsic.h"
#include "sn_internal.h"

static const int avxdot_indices[] = { 0, 4, 1, 5, 2, 3, 6, 7 };

INTERNAL void FORCEINLINE SN_DECL dot_41_sse(MAT_KERNEL(float) const *transpose,
                                             __m128 const *vector, __m128 *ans)
{
        *ans = _mm_dp_ps(transpose->f32_x[0], *vector, 0B11110001);
        *ans = _mm_insert_ps(*ans, _mm_dp_ps(transpose->f32_x[1], *vector, 0B11110001),
                             SSE_INSERT_SIMPLE(1, 1));
        *ans = _mm_insert_ps(*ans, _mm_dp_ps(transpose->f32_x[2], *vector, 0B11110001),
                             SSE_INSERT_SIMPLE(1, 3));
        *ans = _mm_insert_ps(*ans, _mm_dp_ps(transpose->f32_x[3], *vector, 0B11110001),
                             SSE_INSERT_SIMPLE(1, 4));
}

INTERNAL void SN_DECL dot_41_avx(MAT_KERNEL(float) const *transpose, __m128 const *vector,
                                 __m128 *ans)
{
        __m256 _double = _mm256_set_m128(vector[0], vector[0]);
        __m256 tmp = _mm256_dp_ps(transpose->f32_y[0], _double, 0B11110001);
        tmp = _mm256_blend_ps(tmp, _mm256_dp_ps(transpose->f32_y[0], _double, 0B11110010),
                              0B00100010);
        *ans = _mm256_castps256_ps128(
                _mm256_permutevar8x32_ps(tmp, _mm256_loadu_epi32(avxdot_indices)));
}

INTERNAL void SN_DECL dot_42_sse(MAT_KERNEL(float) const *transpose, __m128 const *vector,
                                 __m128 *ans)
{
        ans[0] = _mm_dp_ps(transpose->f32_x[0], vector[0], 0B11110001);
        ans[0] = _mm_insert_ps(*ans, _mm_dp_ps(transpose->f32_x[1], vector[0], 0B11110001),
                               SSE_INSERT_SIMPLE(1, 2));
        ans[0] = _mm_insert_ps(*ans, _mm_dp_ps(transpose->f32_x[2], vector[0], 0B11110001),
                               SSE_INSERT_SIMPLE(1, 3));
        ans[0] = _mm_insert_ps(*ans, _mm_dp_ps(transpose->f32_x[3], vector[0], 0B11110001),
                               SSE_INSERT_SIMPLE(1, 4));

        ans[1] = _mm_dp_ps(transpose->f32_x[0], vector[1], 0B11110001);
        ans[1] = _mm_insert_ps(*ans, _mm_dp_ps(transpose->f32_x[1], vector[1], 0B11110001),
                               SSE_INSERT_SIMPLE(1, 2));
        ans[1] = _mm_insert_ps(*ans, _mm_dp_ps(transpose->f32_x[2], vector[1], 0B11110001),
                               SSE_INSERT_SIMPLE(1, 3));
        ans[1] = _mm_insert_ps(*ans, _mm_dp_ps(transpose->f32_x[3], vector[1], 0B11110001),
                               SSE_INSERT_SIMPLE(1, 4));
}

INTERNAL void SN_DECL dot_42_avx(MAT_KERNEL(float) const *transpose, __m128 const *vector,
                                 __m128 *ans)
{
        __m256 temp = transpose->f32_y[0];
        __m256 vector_stream = _mm256_set_m128(vector[1], vector[0]);

        __m256 result = _mm256_dp_ps(temp, vector_stream, 0B11110001);
        temp = _mm256_permute2f128_ps(temp, temp, 0B00000001);
        result = _mm256_blend_ps(result, _mm256_dp_ps(temp, vector_stream, 0B11110010), 0B00100010);

        temp = transpose->f32_y[1];
        result = _mm256_blend_ps(result, _mm256_dp_ps(temp, vector_stream, 0B11110100), 0B01000100);
        temp = _mm256_permute2f128_ps(temp, temp, 0B00000001);
        result = _mm256_blend_ps(result, _mm256_dp_ps(temp, vector_stream, 0B11111000), 0B10001000);

        ans[0] = _mm256_castps256_ps128(result);
        ans[1] = _mm256_extractf128_ps(result, 1);
        ans[1] = _mm_shuffle_ps(ans[1], ans[1], _MM_SHUFFLE(2, 3, 0, 1));
}
