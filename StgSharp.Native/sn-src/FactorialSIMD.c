#include "sn_internal.h"
#include "sn_intrinsic.h"

INTERNAL __m128i facIncrement = { 4, 4, 4, 4 };
INTERNAL __m128i facBase = { 1, 2, 3, 4 };

INTERNAL uint64_t SN_DECL factorial_simd_sse(int n)
{
        __m128i baseResult = facBase;
        __m128i facBaseLocal = facBase;
        const int cycle = (n - 3) >> 2;
        const uint32_t rest = (cycle << 2) + 1;
        for (size_t i = 0; i < cycle; i++) {
                facBaseLocal = _mm_add_epi32(facBaseLocal, facIncrement);
                baseResult = _mm_mullo_epi32(baseResult, facBaseLocal);
        }
        uint64_t result = (uint64_t)baseResult.m128i_i32[0] * (uint64_t)baseResult.m128i_i32[1] *
                          (uint64_t)baseResult.m128i_i32[2] * (uint64_t)baseResult.m128i_i32[3];
        for (uint64_t i = rest; i < n; i++) {
                result *= i ;
        }
        return result;
}