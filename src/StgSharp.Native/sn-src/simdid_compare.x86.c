#include "sn_target.h"

#if SN_IS_ARCH(SN_ARCH_X86_64)

#include "StgSharpNative.h"
#include "sn_intrinsic.context.x86.h"

/* -----------------------------------------------------------------------
 * Field extraction helpers.
 *
 * SIMDID layout (64 bits, little-endian byte order in make_byte[]):
 *   [3..0]   Root ISA           make_byte[0] low nibble
 *   [7..4]   Manufacture        make_byte[0] high nibble
 *   [15..8]  MainLevel          make_byte[1]
 *   [31..16] AVX512 Feature     make_byte[2..3]  (16 bits)
 *   [39..32] AVX Feature        make_byte[4]
 *   [47..40] AVX10 Feature      make_byte[5]
 *   [55..48] uArchHi            make_byte[6]
 *   [63..56] Reserved           make_byte[7]
 * ----------------------------------------------------------------------- */

#define FIELD_MAIN(id) ((unsigned char)(id).make_byte[1])
#define FIELD_AVX512(id) \
        ((uint16_t)((unsigned char)(id).make_byte[2] | ((unsigned char)(id).make_byte[3] << 8)))
#define FIELD_AVX(id) ((unsigned char)(id).make_byte[4])
#define FIELD_AVX10(id) ((unsigned char)(id).make_byte[5])
#define FIELD_UARCH(id) ((unsigned char)(id).make_byte[6])

/* Portable popcount for a 16-bit value. */
static int popcount16(uint16_t v)
{
        int c = 0;
        while (v) {
                v &= v - 1;
                c++;
        }
        return c;
}

/* Map AVX512_IMPL 2-bit field to a strength ordinal for tie-breaking.
 * Index = impl value (0=UNKNOWN, 1=DUAL, 2=NATIVE, 3=reserved).
 * Strength: DUAL(0) < UNKNOWN(1) < NATIVE(2). */
static const int impl_strength_table[4] = { 1, 0, 2, 1 };

static int impl_strength(uint16_t avx512)
{
        unsigned impl = (avx512 >> SIMDID_AVX512_IMPL_SHIFT) & 0x3u;
        return impl_strength_table[impl];
}

/* -----------------------------------------------------------------------
 * sn_get_unite_simd
 *
 * Produce the conservative intersection of two SIMDIDs — the strongest
 * capability set that is safe to use on BOTH cores:
 *   - MainLevel       : min  (only assume what both cores support)
 *   - AVX512 features : bitwise AND  (IMPL field: pick the weaker one)
 *   - AVX features    : bitwise AND
 *   - AVX10 version   : min; discrete features: bitwise AND
 *   - Root / Manu     : keep left (must be same arch in practice)
 *   - uArchHi         : bitwise OR (HYBRID_P/E union; SIMD_DUAL set if either core is dual)
 * ----------------------------------------------------------------------- */
SN_API SIMDID SN_DECL sn_get_unite_simd(SIMDID left, SIMDID right)
{
        SIMDID out;
        out.mask = 0;

        /* Root + Manufacture (byte 0): prefer left (same arch assumed) */
        unsigned char lb0 = (unsigned char)left.make_byte[0];
        unsigned char rb0 = (unsigned char)right.make_byte[0];
        out.make_byte[0] = (char)(lb0 ? lb0 : rb0);

        /* MainLevel (byte 1): min */
        unsigned char lm = FIELD_MAIN(left);
        unsigned char rm = FIELD_MAIN(right);
        out.make_byte[1] = (char)(lm < rm ? lm : rm);

        /* AVX512 features (bytes 2-3): AND all bits, then fix IMPL to weaker */
        uint16_t l512 = FIELD_AVX512(left);
        uint16_t r512 = FIELD_AVX512(right);
        uint16_t merged = l512 & r512;

        /* Clear the IMPL field, then set the weaker one */
        int ls = impl_strength(l512);
        int rs = impl_strength(r512);
        int s = (ls <= rs) ? l512 : r512; /* pick the one with weaker strength ordinal */
        unsigned weak_impl = (s >> SIMDID_AVX512_IMPL_SHIFT) & 0x3u;
        merged &= ~(uint16_t)SIMDID_AVX512_IMPL_MASK;
        merged |= (uint16_t)(weak_impl << SIMDID_AVX512_IMPL_SHIFT);
        out.make_byte[2] = (char)(merged & 0xFF);
        out.make_byte[3] = (char)((merged >> 8) & 0xFF);

        /* AVX (byte 4): AND */
        out.make_byte[4] = left.make_byte[4] & right.make_byte[4];

        /* AVX10 (byte 5): minimum version and common discrete features. */
        unsigned char lavx10 = FIELD_AVX10(left);
        unsigned char ravx10 = FIELD_AVX10(right);
        unsigned char lver = lavx10 & SIMDID_AVX10_VERSION_MASK;
        unsigned char rver = ravx10 & SIMDID_AVX10_VERSION_MASK;
        unsigned char common = (lavx10 & ravx10) & SIMDID_AVX10_VNNI_INT;
        out.make_byte[5] = (char)((lver < rver ? lver : rver) | common);

        /* uArchHi (byte 6):
         *   topology bits [1..0]: AND — conservative intersection (weaker topology wins)
         *   SIMD_DUAL  bit  [2]:  OR  — dual if either core is dual */
        unsigned char lu = FIELD_UARCH(left);
        unsigned char ru = FIELD_UARCH(right);
        unsigned char topo = (lu & 0x3u) & (ru & 0x3u);
        unsigned char dual = (lu | ru) & (unsigned char)SIMDID_UARCH_SIMD_DUAL;
        out.make_byte[6] = (char)(topo | dual);

        /* Reserved tail byte. */
        out.make_byte[7] = 0;

        return out;
}

/* -----------------------------------------------------------------------
 * sn_compare_simd
 *
 * Compare two SIMDIDs by ISA strength. Returns:
 *   +1  if left is strictly stronger than right
 *   -1  if left is strictly weaker  than right
 *    0  if equal
 *
 * Comparison priority (most significant first):
 *
 *   1. MainLevel ([15..8])
 *      Higher value = stronger. This is the primary discriminator:
 *        NONE < SSE < AVX2 < AVX512 < AVX10
 *
 *   2. AVX10 version and discrete features ([47..40])
 *      Only meaningful when both MainLevel == AVX10.
 *      Higher versions are stronger; VNNI_INT is a tie-breaker.
 *
 *   3. AVX512 BASE bit (bit 4 of [31..16])
 *      Only meaningful when both MainLevel >= AVX512.
 *      BASE=1 (F+CD+VL+DQ+BW all present) > BASE=0.
 *
 *   4. AVX512 extension popcount
 *      VNNI is currently the only tracked AVX512 extension.
 *      The IMPL and BASE bits are masked out before counting so they don't
 *      inflate the extension count.
 *
 *   5. AVX512 IMPL strength (tie-break, performance hint)
 *      DUAL(weakest) < UNKNOWN < NATIVE(strongest).
 *      This is not ISA capability but execution-width risk.
 *
 *   6. uArchHi topology bits [1..0]
 *      Numeric value: unified(11=3) > P-core(10=2) > E-core(01=1) > unknown(00=0).
 *      Higher value = stronger / higher-capability core form.
 *
 *   7. uArchHi SIMD_DUAL (final tie-break)
 *      Non-dual (SIMD_DUAL=0) > dual (SIMD_DUAL=1).
 * ----------------------------------------------------------------------- */
SN_API int SN_DECL sn_compare_simd(SIMDID left, SIMDID right)
{
        /* 1. MainLevel */
        unsigned lm = FIELD_MAIN(left);
        unsigned rm = FIELD_MAIN(right);
        if (lm != rm)
                return lm > rm ? 1 : -1;

        /* 2. AVX10 version and discrete features (only at AVX10). */
        if (lm >= SIMDID_MAIN_LVL_AVX10) {
                unsigned la = FIELD_AVX10(left);
                unsigned ra = FIELD_AVX10(right);
                unsigned lv = la & SIMDID_AVX10_VERSION_MASK;
                unsigned rv = ra & SIMDID_AVX10_VERSION_MASK;
                if (lv != rv)
                        return lv > rv ? 1 : -1;

                int lvnni = (la & SIMDID_AVX10_VNNI_INT) != 0;
                int rvnni = (ra & SIMDID_AVX10_VNNI_INT) != 0;
                if (lvnni != rvnni)
                        return lvnni ? 1 : -1;
        }

        /* 3. AVX512 BASE (only when MainLevel >= AVX512) */
        if (lm >= SIMDID_MAIN_LVL_AVX512) {
                uint16_t l512 = FIELD_AVX512(left);
                uint16_t r512 = FIELD_AVX512(right);

                int lb = (l512 & SIMDID_AVX512_BASE) != 0;
                int rb = (r512 & SIMDID_AVX512_BASE) != 0;
                if (lb != rb)
                        return lb > rb ? 1 : -1;

                // 4. Extension popcount (mask out BASE + IMPL + reserved low nibble)
                int drop = ~(SIMDID_AVX512_BASE | SIMDID_AVX512_IMPL_MASK | 0x000Fu);
                uint16_t ext_mask = drop & (l512 | r512); /* only consider bits set in either */
                int lp = popcount16(l512 & ext_mask);
                int rp = popcount16(r512 & ext_mask);
                if (lp != rp)
                        return lp > rp ? 1 : -1;

                /* 5. IMPL strength */
                int ls = impl_strength(l512);
                int rs = impl_strength(r512);
                if (ls != rs)
                        return ls > rs ? 1 : -1;
        }

        /* 6. uArchHi topology bits [1..0]: unified(3) > P(2) > E(1) > unknown(0) */
        unsigned lu = FIELD_UARCH(left);
        unsigned ru = FIELD_UARCH(right);
        unsigned lt = lu & 0x3u;
        unsigned rt = ru & 0x3u;
        if (lt != rt)
                return lt > rt ? 1 : -1;

        /* 7. uArchHi SIMD_DUAL: non-dual (SIMD_DUAL=0) > dual (SIMD_DUAL=1) */
        int ld = (lu & SIMDID_UARCH_SIMD_DUAL) != 0;
        int rd = (ru & SIMDID_UARCH_SIMD_DUAL) != 0;
        if (ld != rd)
                return ld ? -1 : 1; /* non-dual is stronger */

        return 0;
}

#endif
