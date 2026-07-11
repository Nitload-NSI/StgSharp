#include "sn_target.h"

/* clang-format off */

#if SN_IS_ARCH(SN_ARCH_X86_64)
#include "sn_intrinsic.h"
#include "StgSharpNative.h"
static SIMDID global_simdid = { 0 };

#include "sn_intrinsic.context.x86.h"

#include <immintrin.h>
#include <string.h>
#if defined(_MSC_VER)
#include <intrin.h>
#else
#include <cpuid.h>
#endif

static void get_cpuid(int leaf, int subleaf, int *eax, int *ebx, int *ecx, int *edx)
{
#if defined(_MSC_VER)
        int regs[4];
        __cpuidex(regs, leaf, subleaf);
        *eax = regs[0];
        *ebx = regs[1];
        *ecx = regs[2];
        *edx = regs[3];
#else
        unsigned int a, b, c, d;
        __cpuid_count((unsigned int)leaf, (unsigned int)subleaf, a, b, c, d);
        *eax = (int)a;
        *ebx = (int)b;
        *ecx = (int)c;
        *edx = (int)d;
#endif
}

static int xgetbv_has_xmm_ymm(void)
{
#if defined(_MSC_VER)
        unsigned __int64 xcr0 = _xgetbv(0);
#else
        unsigned long long xcr0 = __builtin_ia32_xgetbv(0);
#endif
        return ((xcr0 & 0x6) == 0x6);
}

static int xgetbv_has_avx512_state(void)
{
        const unsigned long long mask = (1ull << 0) | (1ull << 1) | (1ull << 2) | (1ull << 5) |
                                        (1ull << 6);
#if defined(_MSC_VER)
        unsigned __int64 xcr0 = _xgetbv(0);
#else
        unsigned long long xcr0 = __builtin_ia32_xgetbv(0);
#endif
        return ((xcr0 & mask) == mask);
}

static void decode_family_model(int cpuid1_eax, int *family, int *model)
{
        int family_value = 0;
        int model_value = 0;
        const int base_fam = (cpuid1_eax >> 8) & 0xF;
        const int ext_fam = (cpuid1_eax >> 20) & 0xFF;
        const int base_model = (cpuid1_eax >> 4) & 0xF;
        const int ext_model = (cpuid1_eax >> 16) & 0xF;

        if (base_fam == 0xF) {
                family_value = base_fam + ext_fam;
        } else {
                family_value = base_fam;
        }

        if ((base_fam == 0x6) || (base_fam == 0xF)) {
                model_value = (ext_model << 4) | base_model;
        } else {
                model_value = base_model;
        }

        *family = family_value;
        *model = model_value;
}

/* AVX-512 execution-width classification for AMD, by CPU family.
 * Zen4 (0x19): single 256-bit ALU, 512-bit op = two 256-bit micro-ops (dual-pump) → DUAL.
 * Zen5+ (>=0x1A): native 512-bit execution units → NATIVE.
 * Other/future AMD families → UNKNOWN (conservative). */
static uint64_t amd_avx512_impl(int family)
{
        if (family == 0x19) {
                return SIMDID_AVX512_IMPL_DUAL;
        } else if (family >= 0x1A) {
                return SIMDID_AVX512_IMPL_NATIVE;
        } else {
                return SIMDID_AVX512_IMPL_UNKNOWN;
        }
}

/* Returns 1 if this AMD/Hygon CPU uses split-lane YMM (two fused 128-bit execution units).
 * - AMD Zen1 (family 0x17, model < 0x30) and Zen+ (0x18-0x2F) use a split/glued path.
 * - Hygon Dhyana is architecturally Zen1-based; always split.
 * - AMD Zen2+ (family 0x17 model >= 0x30, family 0x19+) have native 256-bit execution. */
static int is_amd_glue_ymm(const char *vendor, int cpuid1_eax)
{
        int family = 0;
        int model = 0;

        if (strcmp(vendor, "HygonGenuine") == 0) {
                return 1;
        }
        if (strcmp(vendor, "AuthenticAMD") != 0) {
                return 0;
        }

        decode_family_model(cpuid1_eax, &family, &model);
        return (family == 0x17) && (model < 0x30);
}

SN_API SIMDID SN_DECL sn_get_simd_level_local()
{
        SIMDID simdid = { 0 };

        int eax = 0;
        int ebx = 0;
        int ecx = 0;
        int edx = 0;
        int family = 0;
        int model = 0;

        get_cpuid(0, 0, &eax, &ebx, &ecx, &edx);
        const int max_basic_leaf = eax;

        char vendor[13];
        memcpy(vendor + 0, &ebx, 4);
        memcpy(vendor + 4, &edx, 4);
        memcpy(vendor + 8, &ecx, 4);
        vendor[12] = '\0';

        const int is_intel = (strcmp(vendor, "GenuineIntel") == 0);
        const int is_amd = (strcmp(vendor, "AuthenticAMD") == 0);
        const int is_hygon = (strcmp(vendor, "HygonGenuine") == 0);
        const int is_centaur = (strcmp(vendor, "CentaurHauls") == 0);
        const int is_zhaoxin = (strcmp(vendor, "  Shanghai  ") == 0);
        const int is_via = is_centaur || is_zhaoxin;

        get_cpuid(1, 0, &eax, &ebx, &ecx, &edx);
        const int cpuid1_eax = eax;
        decode_family_model(cpuid1_eax, &family, &model);
        (void)model;

        const int has_sse42 = (ecx & (1 << 20)) != 0;
        const int has_avx = (ecx & (1 << 28)) != 0;
        const int has_osxsave = (ecx & (1 << 27)) != 0;
        const int has_fma = (ecx & (1 << 12)) != 0;
        const int has_f16c = (ecx & (1 << 29)) != 0;

        const uint64_t root = SIMDID_PACK_ROOT(SIMDID_ROOT_X86_64);
        uint64_t manu_code = SIMDID_MANU_UNKNOWN;
        if (is_intel) {
                manu_code = SIMDID_MANU_INTEL;
        } else if (is_amd) {
                manu_code = SIMDID_MANU_AMD;
        } else if (is_hygon) {
                manu_code = SIMDID_MANU_HYGON;
        } else if (is_via) {
                manu_code = SIMDID_MANU_VIA;
        }
        const uint64_t manu = SIMDID_PACK_MANU(manu_code);

        uint64_t main_lvl = SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_NONE);
        uint64_t avx_bits = 0;
        uint64_t avx512_bits = 0;
        uint64_t amx_bits = 0;
        uint64_t avx10_bits = 0;
        uint64_t uarch_bits = 0;
        int has_hybrid = 0;

        if (has_sse42) {
                main_lvl = SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_SSE);
        }

        if (has_avx && has_osxsave && xgetbv_has_xmm_ymm()) {
                avx_bits |= SIMDID_AVX;

                if (is_amd_glue_ymm(vendor, cpuid1_eax) || is_via) {
                        avx_bits |= SIMDID_AVX_DUAL;
                }

                if (max_basic_leaf >= 7) {
                        get_cpuid(7, 0, &eax, &ebx, &ecx, &edx);

                        const int has_avx2 = (ebx & (1 << 5)) != 0;
                        const int has_avx512f = (ebx & (1 << 16)) != 0;
                        const int has_avx512dq = (ebx & (1 << 17)) != 0;
                        const int has_avx512bw = (ebx & (1 << 30)) != 0;
                        const int has_avx512vl = (ebx & (1u << 31)) != 0;
                        const int has_avx512_vnni = (ecx & (1 << 11)) != 0;
                        const int has_vbmi = (ecx & (1 << 1)) != 0;
                        const int has_vbmi2 = (ecx & (1 << 6)) != 0;

                        const int has_amx_tile = (edx & (1 << 24)) != 0;
                        const int has_amx_int8 = (edx & (1 << 25)) != 0;
                        const int has_amx_bf16 = (edx & (1 << 22)) != 0;
                        has_hybrid = (edx & (1 << 15)) != 0;

                        int has_bf16 = 0;
                        int has_fp16 = 0;
                        int has_avx10 = 0;
                        if (eax >= 1) {
                                get_cpuid(7, 1, &eax, &ebx, &ecx, &edx);
                                has_bf16 = (eax & (1 << 5)) != 0;
                                has_fp16 = (eax & (1 << 23)) != 0;
                                has_avx10 = (edx & (1 << 19)) != 0;
                        }

                        if (has_avx2) {
                                avx_bits |= SIMDID_AVX2;
                                if (has_fma) {
                                        avx_bits |= SIMDID_AVX_FMA;
                                }
                                if (has_f16c) {
                                        avx_bits |= SIMDID_AVX_F16C;
                                }
                                main_lvl = SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_AVX2);
                        }

                        if (has_avx512f && xgetbv_has_avx512_state()) {
                                if (has_avx512bw && has_avx512vl && has_avx512dq) {
                                        avx512_bits |= SIMDID_AVX512_BASE;
                                }
                                if (has_avx512_vnni) {
                                        avx512_bits |= SIMDID_AVX512_VNNI;
                                }
                                if (has_bf16) {
                                        avx512_bits |= SIMDID_AVX512_BF16;
                                }
                                if (has_fp16) {
                                        avx512_bits |= SIMDID_AVX512_FP16;
                                }
                                if (has_vbmi) {
                                        avx512_bits |= SIMDID_AVX512_VBMI;
                                }
                                if (has_vbmi2) {
                                        avx512_bits |= SIMDID_AVX512_VBMI2;
                                }

                                if (is_intel) {
                                        avx512_bits |= SIMDID_AVX512_IMPL_NATIVE;
                                } else if (is_amd) {
                                        avx512_bits |= amd_avx512_impl(family);
                                } else {
                                        avx512_bits |= SIMDID_AVX512_IMPL_UNKNOWN;
                                }

                                main_lvl = SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_AVX512);

                                if (has_amx_tile) {
                                        amx_bits |= SIMDID_AMX_TILE;
                                        if (has_amx_int8) {
                                                amx_bits |= SIMDID_AMX_INT8;
                                        }
                                        if (has_amx_bf16) {
                                                amx_bits |= SIMDID_AMX_BF16;
                                        }
                                        main_lvl = SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_AMX);
                                }

                                if (has_avx10 && (max_basic_leaf >= 0x24)) {
                                        int a10_eax = 0;
                                        int a10_ebx = 0;
                                        int a10_ecx = 0;
                                        int a10_edx = 0;
                                        int avx10_ver = 0;
                                        int has_256w = 0;
                                        int has_512w = 0;

                                        get_cpuid(0x24, 0, &a10_eax, &a10_ebx, &a10_ecx, &a10_edx);
                                        avx10_ver = a10_ebx & 0xFF;
                                        if (avx10_ver >= 1) {
                                                avx10_bits |= SIMDID_AVX10 | SIMDID_AVX10_V1;
                                                if (avx10_ver >= 2) {
                                                        avx10_bits |= SIMDID_AVX10_V2;
                                                }
                                                has_256w = (a10_ebx & (1 << 17)) != 0;
                                                has_512w = (a10_ebx & (1 << 18)) != 0;
                                                if (has_256w) {
                                                        avx10_bits |= SIMDID_AVX10_256W;
                                                }
                                                if (has_512w) {
                                                        avx10_bits |= SIMDID_AVX10_512W;
                                                }
                                                main_lvl = SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_AVX10);
                                        }
                                }
                        }
                }
        }

        if (has_hybrid && (max_basic_leaf >= 0x1A)) {
                int h_eax = 0;
                int h_ebx = 0;
                int h_ecx = 0;
                int h_edx = 0;

                get_cpuid(0x1A, 0, &h_eax, &h_ebx, &h_ecx, &h_edx);
                {
                        const unsigned core_type = (unsigned)(h_eax >> 24) & 0xFFU;
                        if (core_type == 0x40U) {
                                uarch_bits |= SIMDID_UARCH_HYBRID_P;
                        } else if (core_type == 0x20U) {
                                uarch_bits |= SIMDID_UARCH_HYBRID_E;
                        }
                }
        } else {
                uarch_bits |= SIMDID_UARCH_HYBRID_P | SIMDID_UARCH_HYBRID_E;
        }

        {
                const int has_avx_dual = (avx_bits & SIMDID_AVX_DUAL) != 0;
                const uint64_t avx512_impl = avx512_bits & SIMDID_AVX512_IMPL_MASK;
                const int has_avx512_dual = avx512_impl == SIMDID_AVX512_IMPL_DUAL;
                if (has_avx_dual || has_avx512_dual) {
                        uarch_bits |= SIMDID_UARCH_SIMD_DUAL;
                }
        }

        {
                uint64_t final_mask = root;
                final_mask |= manu;
                final_mask |= main_lvl;
                final_mask |= SIMDID_PACK_AVX512(avx512_bits);
                final_mask |= SIMDID_PACK_AMX(amx_bits);
                final_mask |= SIMDID_PACK_AVX(avx_bits);
                final_mask |= SIMDID_PACK_AVX10(avx10_bits);
                final_mask |= SIMDID_PACK_UARCH(uarch_bits);
                simdid.mask = final_mask;
        }

        return simdid;
}

SN_API SIMDID SN_DECL sn_get_simd_level_global()
{
        if (global_simdid.mask) {
                return global_simdid;
        }

        global_simdid = sn_get_simd_level_local();
        return global_simdid;
}



#endif
