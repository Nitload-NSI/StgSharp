#include "ssc_intrinsic.h"
#include "StgSharpC.h"

static __m128i hash_mask = { .m128i_i32 = { 31 * 31 * 31, 31 * 31, 31, 1 } };

INTERNAL int quick_string_hash(byte const *str, int const length)
{
        __m128i source;
        int source_length = length * 2;
        if (source_length > 15) {
                source = _mm_lddqu_si128((__m128i *)str);
        } else {
                byte buffer[16];
                memcpy(buffer, str, source_length);
                memset(buffer + source_length - 1, 0, 16 - source_length);
                source = _mm_lddqu_si128((__m128i *)buffer);
        }

        __m128i ret = _mm_mullo_epi32(source, hash_mask);
        ret = _mm_hadd_epi32(ret, ret);
        ret = _mm_hadd_epi32(ret, ret);

        return _mm_cvtsi128_si32(ret);
}
