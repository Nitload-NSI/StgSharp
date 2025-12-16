#include "sn_intrinsic.h"
#include "StgSharpNative.h"

INTERNAL int SN_DECL city_hash_simplify_sse(byte const *str, int const length)
{
        uint32_t hash = length, size = length << 1;
        byte *end = str + size, *ptr = str;

        if (size < 4) {
                for (; ptr < end; ptr += 2) {
                        hash = _mm_crc32_u16(hash, *ptr);
                }
        } else if (size < 16) {
                byte *last = end - 4;
                for (; ptr < end; ptr += 4) {
                        hash = _mm_crc32_u32(hash, *(uint32_t *)ptr);
                }
                for (; ptr < end; ptr++) {
                        hash = _mm_crc32_u16(hash, *ptr);
                }
        } else {
                byte *last_128 = end - 16;
                byte *last_32 = end - 4;
                uint64_t hash64 = size;
                for (; ptr < last_128; ptr += 16) {
                        __m128i vec = _mm_loadu_si32((int *)ptr);
                        hash64 = _mm_crc32_u64(hash64, _mm_extract_epi64(vec, 0));
                        hash64 = _mm_crc32_u64(hash64, _mm_extract_epi64(vec, 1));
                }
                hash = _mm_crc32_u8(hash, hash64);
                hash = _mm_crc32_u8(hash, hash64 >> 32);
                for (; ptr < last_32; ptr += 4) {
                        hash = _mm_crc32_u32(hash, *(uint32_t *)ptr);
                }
                for (; ptr < end; ptr += 2) {
                        hash = _mm_crc32_u8(hash, *ptr);
                }
        }
        hash ^= hash >> 16;
        hash *= 0x85ebca6b;
        hash ^= hash >> 13;
        hash *= 0xc2b2ae35;
        hash ^= hash >> 16;
        return hash;
}
