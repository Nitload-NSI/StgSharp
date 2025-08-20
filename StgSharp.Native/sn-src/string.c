#include "sn_intrinsic.h"

int index_pair_sse(short const *str, uint32_t target, int length)
{
        int index = 0, result = 0, size = length, end = size - 8;
        __m128i s = { 
                                   target,
                                   target,
                                   target,
                                   target,
                           };
        __m128i vec, com;

        while (index < end) {
                vec = _mm_loadu_epi32(str + index);
                com = _mm_cmpeq_epi16(vec, s);
                result = _mm_movemask_epi8(com);
                if (result) {
                        for (size_t i = 0; i < 7; i++) {
                                if (((result & 0xF) == 0xF)) {
                                        return index + i;
                                }
                                if (result == 0) {
                                        break;
                                }
                                result >>= 2;
                        }
                }
                index += 1;
                vec = _mm_loadu_epi32(str + index);
                com = _mm_cmpeq_epi16(vec, s);
                result = _mm_movemask_epi8(com);
                if (result) {
                        for (size_t i = 0; i < 7; i++) {
                                if (((result & 0xF) == 0xF)) {
                                        return index + i;
                                }
                                if (result == 0) {
                                        break;
                                }
                                result >>= 2;
                        }
                }
                index += 7;
        }
        for (; (index < size - 1) && (!result); index += 2) {
                result = *(int *)(str + index) == target;
        }
        if (result) {
                return index;
        }
        return -1;
}
