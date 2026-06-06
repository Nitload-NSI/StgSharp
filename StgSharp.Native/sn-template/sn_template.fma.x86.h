#include "sn_target.h"

#if SN_IS_ARCH(SN_ARCH_X86_64)

#define SN_MK_FMA_F32_512_CYCLE(i)                                                       \
        do {                                                                             \
                c0 = _mm512_fmadd_ps(c0, a0, _mm512_permute_ps(b0, _FMA_SELECT(i))); \
                c1 = _mm512_fmadd_ps(c1, a1, _mm512_permute_ps(b1, _FMA_SELECT(i))); \
                c2 = _mm512_fmadd_ps(c2, a2, _mm512_permute_ps(b2, _FMA_SELECT(i))); \
                c3 = _mm512_fmadd_ps(c3, a3, _mm512_permute_ps(b3, _FMA_SELECT(i))); \
        } while (0)

#define SN_MK_FMA_F32_AVX2_CYCLE(i)                                                       \
        do {                                                                             \
                c0 = _mm256_fmadd_ps(c0, a0, _mm256_permute_ps(b0, _FMA_SELECT(i))); \
                c1 = _mm256_fmadd_ps(c1, a1, _mm256_permute_ps(b1, _FMA_SELECT(i))); \
                c2 = _mm256_fmadd_ps(c2, a2, _mm256_permute_ps(b2, _FMA_SELECT(i))); \
                c3 = _mm256_fmadd_ps(c3, a3, _mm256_permute_ps(b3, _FMA_SELECT(i))); \
        } while (0)

#define SN_FMA_512_PACK_COLUMN(col, target)                                   \
        do {                                                              \
                register __m256 low = _mm512_castps512_ps256(col);        \
                register __m256 high = _mm512_extractf32x8_ps(col, 1);    \
                register __m256 half = _mm256_add_ps(high, low);          \
                register __m128 high128 = _mm256_extractf32x4_ps(half, 1); \
                register __m128 low128 = _mm256_castps256_ps128(half);    \
                register __m128 half128 = _mm_add_ps(high128, low128);    \
                _mm_store_ps(target, half128);                            \
        } while (0)

#define SN_FMA_AVX2_PACK_COLUMN(col, target)                                   \
        do {                                                              \
                register __m128 high128 = _mm256_extractf32x4_ps(col, 1); \
                register __m128 low128 = _mm256_castps256_ps128(col);    \
                register __m128 half128 = _mm_add_ps(high128, low128);    \
                _mm_store_ps(target, half128);                            \
        } while (0)

// double
#define SN_MK_FMA_F64_SSE_PHASE(i)                                               \
        do {                                                                     \
                /* k = 0 */                                                      \
                register __m128d b_lo = _mm_load_pd(right_values + (2 * i + 0)); \
                register __m128d b_hi = _mm_load_pd(right_values + (2 * i + 1)); \
                register __m128d a_lo = _mm_load_pd(left_values + (0 * 2 + 0));  \
                register __m128d a_hi = _mm_load_pd(left_values + (0 * 2 + 1));  \
                register __m128d b = _mm_shuffle_pd(b_lo, b_lo, 0x0);            \
                c##i##_lo = _mm_add_pd(c##i##_lo, _mm_mul_pd(a_lo, b));          \
                c##i##_hi = _mm_add_pd(c##i##_hi, _mm_mul_pd(a_hi, b));          \
                                                                                 \
                /* group = 1 */                                                  \
                a_lo = _mm_load_pd(left_values + (1 * 2 + 0));                   \
                a_hi = _mm_load_pd(left_values + (1 * 2 + 1));                   \
                b = _mm_shuffle_pd(b_lo, b_lo, 0x3);                             \
                c##i##_lo = _mm_add_pd(c##i##_lo, _mm_mul_pd(a_lo, b));          \
                c##i##_hi = _mm_add_pd(c##i##_hi, _mm_mul_pd(a_hi, b));          \
                                                                                 \
                /* group = 2 */                                                  \
                a_lo = _mm_load_pd(left_values + (2 * 2 + 0));                   \
                a_hi = _mm_load_pd(left_values + (2 * 2 + 1));                   \
                b = _mm_shuffle_pd(b_hi, b_hi, 0x0);                             \
                c##i##_lo = _mm_add_pd(c##i##_lo, _mm_mul_pd(a_lo, b));          \
                c##i##_hi = _mm_add_pd(c##i##_hi, _mm_mul_pd(a_hi, b));          \
                                                                                 \
                /* group = 3 */                                                  \
                a_lo = _mm_load_pd(left_values + (3 * 2 + 0));                   \
                a_hi = _mm_load_pd(left_values + (3 * 2 + 1));                   \
                b = _mm_shuffle_pd(b_hi, b_hi, 0x3);                             \
                c##i##_lo = _mm_add_pd(c##i##_lo, _mm_mul_pd(a_lo, b));          \
                c##i##_hi = _mm_add_pd(c##i##_hi, _mm_mul_pd(a_hi, b));          \
        } while (0)

#endif
