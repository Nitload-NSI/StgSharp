#include "sn_target.h"

#if SN_IS_ARCH(SN_ARCH_X86_64)


#define SN_MK_FMA_F32_512_PHASE(i)\
        do {    \
                ak = _mm512_broadcast_f32x4(left_##i ->f32_x[0]);\
                c_0 = _mm512_fmadd_ps(ak, _mm512_permute_ps(b_##i, _FMA_SELECT(0)), c_##i);\
                ak = _mm512_broadcast_f32x4(left_##i ->f32_x[0]);\
                c_1 = _mm512_fmadd_ps(ak, _mm512_permute_ps(b_##i, _FMA_SELECT(0)), c_##i);\
                ak = _mm512_broadcast_f32x4(left_##i ->f32_x[0]);\
                c_2 = _mm512_fmadd_ps(ak, _mm512_permute_ps(b_##i, _FMA_SELECT(0)), c_##i);\
                ak = _mm512_broadcast_f32x4(left_##i ->f32_x[0]);\
                c_3 = _mm512_fmadd_ps(ak, _mm512_permute_ps(b_##i, _FMA_SELECT(0)), c_##i);\
        }while(0)


// double
#define SN_MK_FMA_F64_SSE_PHASE(i)                                            \
        do {                                                                  \
                /* k = 0 */                                                   \
                register __m128d b_lo = _mm_load_pd(right_values + (2 * i + 0)); \
                register __m128d b_hi = _mm_load_pd(right_values + (2 * i + 1)); \
                register __m128d a_lo = _mm_load_pd(left_values + (0 * 2 + 0));  \
                register __m128d a_hi = _mm_load_pd(left_values + (0 * 2 + 1));  \
                register __m128d b = _mm_shuffle_pd(b_lo, b_lo, 0x0);         \
                c##i##_lo = _mm_add_pd(c##i##_lo, _mm_mul_pd(a_lo, b));       \
                c##i##_hi = _mm_add_pd(c##i##_hi, _mm_mul_pd(a_hi, b));       \
                                                                              \
                /* group = 1 */                                               \
                a_lo = _mm_load_pd(left_values + (1 * 2 + 0));                 \
                a_hi = _mm_load_pd(left_values + (1 * 2 + 1));                 \
                b = _mm_shuffle_pd(b_lo, b_lo, 0x3);                          \
                c##i##_lo = _mm_add_pd(c##i##_lo, _mm_mul_pd(a_lo, b));       \
                c##i##_hi = _mm_add_pd(c##i##_hi, _mm_mul_pd(a_hi, b));       \
                                                                              \
                /* group = 2 */                                               \
                a_lo = _mm_load_pd(left_values + (2 * 2 + 0));                 \
                a_hi = _mm_load_pd(left_values + (2 * 2 + 1));                 \
                b = _mm_shuffle_pd(b_hi, b_hi, 0x0);                          \
                c##i##_lo = _mm_add_pd(c##i##_lo, _mm_mul_pd(a_lo, b));       \
                c##i##_hi = _mm_add_pd(c##i##_hi, _mm_mul_pd(a_hi, b));       \
                                                                              \
                /* group = 3 */                                               \
                a_lo = _mm_load_pd(left_values + (3 * 2 + 0));                 \
                a_hi = _mm_load_pd(left_values + (3 * 2 + 1));                 \
                b = _mm_shuffle_pd(b_hi, b_hi, 0x3);                          \
                c##i##_lo = _mm_add_pd(c##i##_lo, _mm_mul_pd(a_lo, b));       \
                c##i##_hi = _mm_add_pd(c##i##_hi, _mm_mul_pd(a_hi, b));       \
        } while (0)

#endif