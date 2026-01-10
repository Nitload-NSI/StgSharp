#include "sn_target.h"
#include "sn_intrinsic.h"

static SIMDID global_simdid = { 0 };

#if SN_IS_ARCH(SN_ARCH_X86_64)

#include "sn_intrincontext.x86.h"

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

static int is_intel_dual_avx512(int family, int model)
{
        return (family == 6) && (model == 0x55); /* SKX/CLX/CPX */
}

static int is_intel_native_avx512(int family, int model)
{
        if (family != 6)
                return 0;
        switch (model) {
        case 0x6A: /* ICX */
        case 0x6C: /* ICX stepping */
        case 0x8F: /* SPR/EMR */
        case 0xCF: /* GNR */
                return 1;
        default:
                return 0;
        }
}

SN_API SIMDID SN_DECL sn_get_simd_level()
{
        if (global_simdid.mask) {
                return global_simdid;
        }

        int eax = 0, ebx = 0, ecx = 0, edx = 0;

        /* Vendor */
        get_cpuid(0, 0, &eax, &ebx, &ecx, &edx);
        char vendor[13];
        ((int *)vendor)[0] = ebx;
        ((int *)vendor)[1] = edx;
        ((int *)vendor)[2] = ecx;
        vendor[12] = '\0';
        const int is_intel = (strcmp(vendor, "GenuineIntel") == 0);
        const int is_amd = (strcmp(vendor, "AuthenticAMD") == 0) ||
                           (strcmp(vendor, "HygonGenuine") == 0);

        /* CPUID.1 */
        get_cpuid(1, 0, &eax, &ebx, &ecx, &edx);
        int family = ((eax >> 8) & 0xF) + ((eax >> 20) & 0xFF0);
        int model = ((eax >> 4) & 0xF) | ((eax >> 12) & 0xF0);

        int has_sse42 = (ecx & (1 << 20)) != 0;
        int has_avx = (ecx & (1 << 28)) != 0;
        int has_osxsave = (ecx & (1 << 27)) != 0;
        int has_fma = (ecx & (1 << 12)) != 0;
        int has_f16c = (ecx & (1 << 29)) != 0;

        uint64_t root = SIMDID_PACK_ROOT(SIMDID_ROOT_X86_64);
        uint64_t manu = SIMDID_PACK_MANU(is_intel ? SIMDID_MANU_INTEL :
                                         is_amd   ? SIMDID_MANU_AMD :
                                                    SIMDID_MANU_UNKNOWN);
        uint64_t main_lvl = SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_NONE);
        uint64_t avx512_bits = 0;
        uint64_t amx_bits = 0;
        uint64_t avx_bits = 0;

        /* Baseline main level */
        if (has_sse42)
                main_lvl = SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_SSE);

        if (has_avx && has_osxsave && xgetbv_has_xmm_ymm()) {
                avx_bits |= SIMDID_AVX;

                /* CPUID.7.0 */
                get_cpuid(7, 0, &eax, &ebx, &ecx, &edx);
                const int has_bmi1 = (ebx & (1 << 3)) != 0;
                const int has_bmi2 = (ebx & (1 << 8)) != 0;
                const int has_avx2 = (ebx & (1 << 5)) != 0;
                const int has_avx512f = (ebx & (1 << 16)) != 0;
                const int has_avx512dq = (ebx & (1 << 17)) != 0;
                const int has_avx512bw = (ebx & (1 << 30)) != 0;
                const int has_avx512vl = (ebx & (1u << 31)) != 0;
                const int has_avx512_vnni = (ebx & (1 << 11)) != 0;
                const int has_vbmi = (ecx & (1 << 1)) != 0;
                const int has_vbmi2 = (ecx & (1 << 6)) != 0;

                /* leaf 7, subleaf 1 for BF16/FP16 */
                get_cpuid(7, 1, &eax, &ebx, &ecx, &edx);
                const int has_bf16 = (eax & (1 << 5)) != 0; /* AVX512_BF16 */
                const int has_fp16 = (eax & (1 << 23)) != 0; /* AVX512_FP16 */

                /* AMX in leaf 7 subleaf 0 EDX bits 24/25/26 */
                const int has_amx_tile = (edx & (1 << 24)) != 0;
                const int has_amx_int8 = (edx & (1 << 25)) != 0;
                const int has_amx_bf16 = (edx & (1 << 26)) != 0;

                if (has_avx2) {
                        avx_bits |= SIMDID_AVX2;
                        if (has_fma)
                                avx_bits |= SIMDID_AVX_FMA;
                        if (has_f16c)
                                avx_bits |= SIMDID_AVX_F16C;

                        main_lvl = SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_AVX2);
                }

                if (has_avx512f && xgetbv_has_avx512_state()) {
                        /* Ensure base subset present */
                        if (has_avx512bw && has_avx512vl && has_avx512dq) {
                                avx512_bits |= SIMDID_AVX512_BASE;
                        }
                        if (has_avx512_vnni)
                                avx512_bits |= SIMDID_AVX512_VNNI;
                        if (has_bf16)
                                avx512_bits |= SIMDID_AVX512_BF16;
                        if (has_fp16)
                                avx512_bits |= SIMDID_AVX512_FP16;
                        if (has_vbmi)
                                avx512_bits |= SIMDID_AVX512_VBMI;
                        if (has_vbmi2)
                                avx512_bits |= SIMDID_AVX512_VBMI2;

                        /* impl classification */
                        const int glue = is_amd || is_intel_dual_avx512(family, model);
                        const int native512 = is_intel_native_avx512(family, model);
                        if (glue) {
                                avx512_bits |= SIMDID_AVX512_IMPL_DUAL;
                        } else if (native512) {
                                avx512_bits |= SIMDID_AVX512_IMPL_NATIVE;
                        } else {
                                avx512_bits |= SIMDID_AVX512_IMPL_UNKNOWN;
                        }

                        /* main level for AVX512 */
                        main_lvl = SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_AVX512);

                        /* AMX promotion if present */
                        if (has_amx_tile) {
                                amx_bits |= SIMDID_AMX_TILE;
                                if (has_amx_int8)
                                        amx_bits |= SIMDID_AMX_INT8;
                                if (has_amx_bf16)
                                        amx_bits |= SIMDID_AMX_BF16;
                                main_lvl = SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_AMX);
                        }
                }
        }

        global_simdid.mask = root | manu | main_lvl | SIMDID_PACK_AVX512(avx512_bits) |
                             SIMDID_PACK_AMX(amx_bits) | SIMDID_PACK_AVX(avx_bits);
        return global_simdid;
}

#else

SIMDID sn_get_simd_level()
{
        SIMDID simd_id;
        simd_id.mask = 0;
        return simd_id; /* non-x86: no SIMD level probing */
}

#endif
