#include "sn_target.h"

#if SN_IS_ARCH(SN_ARCH_X86_64)

#include "sn_internal.h"
#include "sn_intrinsic.h"

static __m128i facIncrement;
static __m128i facBase;

INTERNAL uint64_t SN_DECL factorial_simd_sse(int n)
{
        facIncrement = _mm_set1_epi32(4);
        facBase = _mm_setr_epi32(1, 2, 3, 4);
        __m128i baseResult = facBase;
        __m128i facBaseLocal = facBase;
        const int cycle = (n - 3) >> 2;
        const uint32_t rest = (cycle << 2) + 1;
        for (size_t i = 0; i < cycle; i++) {
                facBaseLocal = _mm_add_epi32(facBaseLocal, facIncrement);
                baseResult = _mm_mullo_epi32(baseResult, facBaseLocal);
        }
        SN_ALIGNAS(16) int32_t lanes[4];
        _mm_store_si128((__m128i *)lanes, baseResult);
        uint64_t result = (uint64_t)lanes[0] * (uint64_t)lanes[1] * (uint64_t)lanes[2] *
                          (uint64_t)lanes[3];
        for (uint64_t i = rest; i < n; i++) {
                result *= i;
        }
        return result;
}

#endif
