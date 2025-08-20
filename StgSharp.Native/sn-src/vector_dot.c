#include "sn_intrinsic.h"
#include "sn_internal.h"

static const int avxdot_indices[] = { 0, 4, 1, 5, 2, 3, 6, 7 };

INTERNAL void FORCEINLINE SN_DECL dot_41_sse(__4_columnset *transpose, __m128 *vector, __m128 *ans)
{
        *ans = _mm_dp_ps(transpose->column[0], *vector, 0B11110001);
        *ans = _mm_insert_ps(*ans, _mm_dp_ps(transpose->column[1], *vector, 0B11110001),
                             SSE_INSERT_SIMPLE(1, 1));
        *ans = _mm_insert_ps(*ans, _mm_dp_ps(transpose->column[2], *vector, 0B11110001),
                             SSE_INSERT_SIMPLE(1, 3));
        *ans = _mm_insert_ps(*ans, _mm_dp_ps(transpose->column[3], *vector, 0B11110001),
                             SSE_INSERT_SIMPLE(1, 4));
}

INTERNAL void SN_DECL dot_41_avx(__4_columnset *transpose, __m128 *vector, __m128 *ans)
{
        __m256 _double = _mm256_set_m128(vector[0], vector[0]);
        __m256 tmp = _mm256_dp_ps(transpose->half[0], _double, 0B11110001);
        tmp = _mm256_blend_ps(tmp, _mm256_dp_ps(transpose->half[0], _double, 0B11110010),
                              0B00100010);
        *ans = _mm256_castps256_ps128(
                _mm256_permutevar8x32_ps(tmp, _mm256_loadu_epi32(avxdot_indices)));
}

INTERNAL void FORCEINLINE SN_DECL dot_31_sse(__3_columnset *transpose, __m128 *vector, __m128 *ans)
{
        *ans = _mm_dp_ps(transpose->column[0], *vector, 0B11110001);
        *ans = _mm_insert_ps(*ans, _mm_dp_ps(transpose->column[1], *vector, 0B11110001),
                             SSE_INSERT_SIMPLE(1, 2));
        *ans = _mm_insert_ps(*ans, _mm_dp_ps(transpose->column[2], *vector, 0B11110001),
                             SSE_INSERT_SIMPLE(1, 3));
}

INTERNAL void FORCEINLINE SN_DECL dot_31_avx(__3_columnset *transpose, __m128 *vector, __m128 *ans)
{
        __m256 _double = _mm256_set_m128(vector[0], vector[0]);
        __m256 tmp = _mm256_dp_ps(transpose->part, _double, 0B11110001);
        *ans = _mm256_castps256_ps128(
                _mm256_permutevar8x32_ps(tmp, _mm256_loadu_epi32(avxdot_indices)));
        *ans = _mm_insert_ps(*ans, _mm_dp_ps(transpose->column[2], *vector, 0B11110001),
                             SSE_INSERT_SIMPLE(1, 3));
}

INTERNAL void SN_DECL dot_42_sse(__4_columnset *transpose, __m128 *vector, __m128 *ans)
{
        ans[0] = _mm_dp_ps(transpose->column[0], vector[0], 0B11110001);
        ans[0] = _mm_insert_ps(*ans, _mm_dp_ps(transpose->column[1], vector[0], 0B11110001),
                               SSE_INSERT_SIMPLE(1, 2));
        ans[0] = _mm_insert_ps(*ans, _mm_dp_ps(transpose->column[2], vector[0], 0B11110001),
                               SSE_INSERT_SIMPLE(1, 3));
        ans[0] = _mm_insert_ps(*ans, _mm_dp_ps(transpose->column[3], vector[0], 0B11110001),
                               SSE_INSERT_SIMPLE(1, 4));

        ans[1] = _mm_dp_ps(transpose->column[0], vector[1], 0B11110001);
        ans[1] = _mm_insert_ps(*ans, _mm_dp_ps(transpose->column[1], vector[1], 0B11110001),
                               SSE_INSERT_SIMPLE(1, 2));
        ans[1] = _mm_insert_ps(*ans, _mm_dp_ps(transpose->column[2], vector[1], 0B11110001),
                               SSE_INSERT_SIMPLE(1, 3));
        ans[1] = _mm_insert_ps(*ans, _mm_dp_ps(transpose->column[3], vector[1], 0B11110001),
                               SSE_INSERT_SIMPLE(1, 4));
}

INTERNAL void SN_DECL dot_32_sse(__3_columnset *transpose, __m128 *vector, __m128 *ans)
{
        ans[0] = _mm_dp_ps(transpose->column[0], vector[0], 0B11110001);
        ans[0] = _mm_insert_ps(*ans, _mm_dp_ps(transpose->column[1], vector[0], 0B11110001),
                               SSE_INSERT_SIMPLE(1, 2));
        ans[0] = _mm_insert_ps(*ans, _mm_dp_ps(transpose->column[2], vector[0], 0B11110001),
                               SSE_INSERT_SIMPLE(1, 3));

        ans[1] = _mm_dp_ps(transpose->column[0], vector[1], 0B11110001);
        ans[1] = _mm_insert_ps(*ans, _mm_dp_ps(transpose->column[1], vector[1], 0B11110001),
                               SSE_INSERT_SIMPLE(1, 2));
        ans[1] = _mm_insert_ps(*ans, _mm_dp_ps(transpose->column[2], vector[1], 0B11110001),
                               SSE_INSERT_SIMPLE(1, 3));
}

INTERNAL void SN_DECL dot_42_avx(__4_columnset *transpose, __m128 *vector, __m128 *ans)
{
        __2_columnset *ans_ptr = (__2_columnset *)ans, *right_ptr = (__2_columnset *)vector;
        __m256 temp = ALIGN256(transpose->half[0]);

        ans_ptr->stream = _mm256_dp_ps(temp, right_ptr->stream, 0B11110001);
        temp = _mm256_permute2f128_ps(temp, temp, 0B00000001);
        ans_ptr->stream = _mm256_blend_ps(
                ans_ptr->stream, _mm256_dp_ps(temp, right_ptr->stream, 0B11110010), 0B00100010);

        temp = ALIGN256(transpose->half[1]);
        ans_ptr->stream = _mm256_blend_ps(
                ans_ptr->stream, _mm256_dp_ps(temp, right_ptr->stream, 0B11110100), 0B01000100);
        temp = _mm256_permute2f128_ps(temp, temp, 0B00000001);
        ans_ptr->stream = _mm256_blend_ps(
                ans_ptr->stream, _mm256_dp_ps(temp, right_ptr->stream, 0B11111000), 0B10001000);

        ans[1] = _mm_shuffle_ps(ALIGN(ans[1]), ALIGN(ans[1]), _MM_SHUFFLE(2, 3, 0, 1));
}

INTERNAL void SN_DECL dot_32_avx(__3_columnset *transpose, __m128 *vector, __m128 *ans)
{
        __2_columnset *ans_ptr = (__2_columnset *)ans, *right_ptr = (__2_columnset *)vector;
        __m256 temp = transpose->part;

        ans_ptr->stream = _mm256_dp_ps(temp, right_ptr->stream, 0B11110001);
        temp = _mm256_permute2f128_ps(temp, temp, 0B00000001);
        ans_ptr->stream = _mm256_blend_ps(
                ans_ptr->stream, _mm256_dp_ps(temp, right_ptr->stream, 0B11110010), 0B00100010);

        temp = _mm256_set_m128(transpose->column[2], transpose->column[2]);
        ans_ptr->stream = _mm256_blend_ps(
                ans_ptr->stream, _mm256_dp_ps(temp, right_ptr->stream, 0B11110100), 0B01000100);

        ans[1] = _mm_shuffle_ps(ALIGN(ans[1]), ALIGN(ans[1]), _MM_SHUFFLE(2, 2, 0, 1));
}

INTERNAL void SN_DECL dot_43_sse(__4_columnset *transpose, __m128 *vector, __m128 *ans)
{
        dot_41_sse(transpose, vector, ans);
        dot_41_sse(transpose, vector + 1, ans + 1);
        dot_41_sse(transpose, vector + 2, ans + 2);
}

INTERNAL void SN_DECL dot_43_avx(__4_columnset *transpose, __m128 *vector, __m128 *ans)
{
        __3_columnset *ans_ptr = (__3_columnset *)ans, *right_ptr = (__3_columnset *)vector;
        __m256 temp = ALIGN256(transpose->half[0]);
        __m256 third = _mm256_set_m128(right_ptr->column[2], right_ptr->column[2]);

        ans_ptr->part = _mm256_dp_ps(temp, right_ptr->part, 0B11110001);
        __m256 ans_third = _mm256_dp_ps(temp, third, 0B11110001);
        temp = _mm256_permute2f128_ps(temp, temp, 0B00000001);
        ans_ptr->part = _mm256_blend_ps(
                ans_ptr->part, _mm256_dp_ps(temp, right_ptr->part, 0B11110010), 0B00100010);

        temp = ALIGN256(transpose->half[1]);
        ans_ptr->part = _mm256_blend_ps(
                ans_ptr->part, _mm256_dp_ps(temp, right_ptr->part, 0B11110100), 0B01000100);
        ans_third = _mm256_blend_ps(ans_third, _mm256_dp_ps(temp, third, 0B11110010), 0B00100010);
        temp = _mm256_permute2f128_ps(temp, temp, 0B00000001);
        ans_ptr->part = _mm256_blend_ps(
                ans_ptr->part, _mm256_dp_ps(temp, right_ptr->part, 0B11111000), 0B10001000);

        ans[1] = _mm_shuffle_ps(ALIGN(ans[1]), ALIGN(ans[1]), _MM_SHUFFLE(2, 3, 0, 1));
        ans[2] = _mm256_castps256_ps128(
                _mm256_permutevar8x32_ps(ans_third, _mm256_loadu_epi32(avxdot_indices)));
}
