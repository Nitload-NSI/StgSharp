#ifndef NFN_INTRINSIC_CONTEXT_X86_H
#define NFN_INTRINSIC_CONTEXT_X86_H

#include "nfn_target.h"

#if NFN_IS_ARCH(NFN_ARCH_X86_64)

#include "nfn_intrinsic.h"
#include "nfn_intrinsic.std.h"
#include "nfn_intrinsic.context.base.h"

#ifndef NFN_X86_TARGET_ATTR_sse
#define NFN_STD_TARGET_ATTR_IMPL(T_arch, T_feature) NFN_X86_TARGET_ATTR_##T_arch##T_feature
#define NFN_X86_TARGET_ATTR_sse NFN_CLANG_TARGET_ATTR("sse4.2")
#define NFN_X86_TARGET_ATTR_avx NFN_CLANG_TARGET_ATTR("avx")
#define NFN_X86_TARGET_ATTR_avx_fma NFN_CLANG_TARGET_ATTR("avx,avx2,fma")
#define NFN_X86_TARGET_ATTR_512 NFN_CLANG_TARGET_ATTR("avx512f,avx512vl")
#endif

// -----------------------------------------------------------------------------
// SIMDID layout helpers (see SIMDID.x86.md)
// bits [3..0]  : Root ISA
// bits [7..4]  : Manufacture (ISA-specific)
// bits [15..8] : MainLevel (monotonic capability)
// bits [31..16]: AVX512 Feature
// bits [39..32]: AVX Feature
// bits [47..40]: AVX10 Feature
// bits [55..48]: uArchHi / Policy (optional)
// bits [63..56]: Reserved for future tail extension
// -----------------------------------------------------------------------------
#define SIMDID_SHIFT_AVX512 16
#define SIMDID_SHIFT_AVX 32
#define SIMDID_SHIFT_AVX10 40
#define SIMDID_SHIFT_UARCH 48

#define SIMDID_MASK_AVX512 (0xFFFFULL << SIMDID_SHIFT_AVX512)
#define SIMDID_MASK_AVX (0xFFULL << SIMDID_SHIFT_AVX)
#define SIMDID_MASK_AVX10 (0xFFULL << SIMDID_SHIFT_AVX10)
#define SIMDID_MASK_UARCH (0xFFULL << SIMDID_SHIFT_UARCH)

// Manufacture (x86-64 mapping; other ISAs may reinterpret)
#define SIMDID_MANU_UNKNOWN 0x0
#define SIMDID_MANU_INTEL 0x1
#define SIMDID_MANU_AMD 0x2
#define SIMDID_MANU_VIA 0x3
#define SIMDID_MANU_HYGON 0x4 /* 海光: Zen1-derived, separate from AMD */

// MainLevel (ordinal, shared bits 8..15; not bit-flags)
#define SIMDID_MAIN_LVL_NONE 0x00u
#define SIMDID_MAIN_LVL_SSE 0x01u /* SSE1..SSE4.2 */
#define SIMDID_MAIN_LVL_AVX2 0x02u
#define SIMDID_MAIN_LVL_AVX512 0x03u
#define SIMDID_MAIN_LVL_AVX10 0x04u

#ifndef SIMDID_AVX512_BASE
#define SIMDID_AVX512_VNNI (1U << 11)
#define SIMDID_AVX512_BASE (1U << 4)
#define SIMDID_AVX512_IMPL_SHIFT 6
#define SIMDID_AVX512_IMPL_MASK (0x3U << SIMDID_AVX512_IMPL_SHIFT)
#define SIMDID_AVX512_IMPL_UNKNOWN (0x0U << SIMDID_AVX512_IMPL_SHIFT)
#define SIMDID_AVX512_IMPL_DUAL (0x1U << SIMDID_AVX512_IMPL_SHIFT)
#define SIMDID_AVX512_IMPL_NATIVE (0x2U << SIMDID_AVX512_IMPL_SHIFT)
#endif

#ifndef SIMDID_AVX_BASE
#define SIMDID_AVX (1U << 0) /* relative bit 0 within [39..32] */
#define SIMDID_AVX2 (1U << 1)
#define SIMDID_AVX_FMA (1U << 2)
#define SIMDID_AVX_F16C (1U << 3)
#define SIMDID_AVX_DUAL \
        (1U << 4) /* glued/split-lane YMM (two 128-bit units; Zen1, Hygon, VIA/Zhaoxin) */
#endif

#ifndef SIMDID_AVX10_VERSION_MASK
#define SIMDID_AVX10_VERSION_MASK 0x7FU /* CPUID.24H.0:EBX[7:0], saturated to 7 bits */
#define SIMDID_AVX10_VNNI_INT (1U << 7) /* CPUID.24H.1:ECX[2] */
#endif

/* uArchHi ([55..48]) bitmask — core topology and SIMD-width execution policy */
#ifndef SIMDID_UARCH_HYBRID_P
/* Topology 2-bit field (bits [1..0] of uArchHi byte):
 *   00 = unknown (undetected hybrid type or pre-CPUID.1A hardware)
 *   01 = E-core  (efficiency/small core in a hybrid CPU)
 *   10 = P-core  (performance/big core in a hybrid CPU)
 *   11 = unified (confirmed non-hybrid; numerically strongest)
 * Higher numeric value == stronger / higher-capability topology. */
#define SIMDID_UARCH_HYBRID_E (1U << 0) /* E-core (efficiency/small) in a hybrid topology */
#define SIMDID_UARCH_HYBRID_P (1U << 1) /* P-core (performance/big) in a hybrid topology */
#define SIMDID_UARCH_SIMD_DUAL (1U << 2) /* SIMD is glued/dual (summary: AVX_DUAL or AVX512_IMPL=DUAL) */
#endif

// Pack helpers (define locally to avoid missing macros when included standalone)
#ifndef SIMDID_PACK_X86
#define SIMDID_PACK_X86(root, manu, main) \
        (SIMDID_PACK_ROOT(root) | SIMDID_PACK_MANU(manu) | SIMDID_PACK_MAIN(main))
#define SIMDID_PACK_AVX512(avx512_bits) \
        (((uint64_t)(avx512_bits) & 0xFFFFULL) << SIMDID_SHIFT_AVX512)
#define SIMDID_PACK_AVX(avx_bits) (((uint64_t)(avx_bits) & 0xFFULL) << SIMDID_SHIFT_AVX)
#define SIMDID_PACK_AVX10(avx10_bits) (((uint64_t)(avx10_bits) & 0xFFULL) << SIMDID_SHIFT_AVX10)
#define SIMDID_PACK_UARCH(uarch_hi) (((uint64_t)(uarch_hi) & 0xFFULL) << SIMDID_SHIFT_UARCH)
#endif

/* Pre-shifted helpers for convenience when constructing SIMDID */
#define SIMDID_MAIN_ENC_NONE SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_NONE)
#define SIMDID_MAIN_ENC_SSE SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_SSE)
#define SIMDID_MAIN_ENC_AVX2 SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_AVX2)
#define SIMDID_MAIN_ENC_AVX512 SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_AVX512)
#define SIMDID_MAIN_ENC_AVX10 SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_AVX10)

#pragma region matix function

INTERNAL void f32_normalize(VEC_SEGMENT(float) * source, VEC_SEGMENT(float) * target);

NFN_MK_PROC_DECL_STD(float, sse, , k_quality);

NFN_MK_PROC_DECL_STD(float, sse, , add);
NFN_MK_PROC_DECL_STD(float, avx, , add);
NFN_MK_PROC_DECL_STD(float, 512, , add);

NFN_MK_PROC_DECL_STD(double, sse, , add);
NFN_MK_PROC_DECL_STD(double, avx, , add);
NFN_MK_PROC_DECL_STD(double, 512, , add);

NFN_MK_PROC_DECL_STD(float, sse, , sub);
NFN_MK_PROC_DECL_STD(float, avx, , sub);
NFN_MK_PROC_DECL_STD(float, 512, , sub);

NFN_MK_PROC_DECL_STD(double, sse, , sub);
NFN_MK_PROC_DECL_STD(double, avx, , sub);
NFN_MK_PROC_DECL_STD(double, 512, , sub);

NFN_MK_PROC_DECL_STD(float, sse, , transpose);
NFN_MK_PROC_DECL_STD(float, avx, , transpose);
NFN_MK_PROC_DECL_STD(float, 512, , transpose);

NFN_MK_PROC_DECL_STD(float, sse, , scalar_mul);
NFN_MK_PROC_DECL_STD(float, avx, , scalar_mul);
NFN_MK_PROC_DECL_STD(float, 512, , scalar_mul);

NFN_MK_PROC_DECL_STD(double, sse, , scalar_mul);
NFN_MK_PROC_DECL_STD(double, avx, , scalar_mul);
NFN_MK_PROC_DECL_STD(double, 512, , scalar_mul);

NFN_MK_PROC_DECL_STD(float, sse, , fill);
NFN_MK_PROC_DECL_STD(float, avx, , fill);
NFN_MK_PROC_DECL_STD(float, 512, , fill);

NFN_MK_PROC_DECL_STD(double, sse, , fill);
NFN_MK_PROC_DECL_STD(double, avx, , fill);
NFN_MK_PROC_DECL_STD(double, 512, , fill);

NFN_MK_PROC_DECL_STD(float, sse, , quality);

/*
* pack left matrix data to L4 buffer
* 
* CONVENSIONS:
* 
* buffer_1 is pointer to original colum, 
* offset_1 is the offset to the original column, 
* count_1 is the count can be packed
* 
* C# shceduler is responsible for calculating the offset and count based on the actual buffer size and matrix dimension, 
* and make sure that count_1 will not exceeds lenght of the column
* 
* scalar pack carries column length and offset data:
* [column length, empty, empty, empty]
*/
NFN_MK_PROC_DECL_STD(float, sse, , fma_pack_left);
NFN_MK_PROC_DECL_STD(float, avx, , fma_pack_left);
NFN_MK_PROC_DECL_STD(float, 512, , fma_pack_left);

/*
* pack right matrix data to L4 buffer
* 
* CONVENSIONS:
* 
* buffer_1 is pointer to original row, 
* offset_1 is the offset to the original row, 
* count_1 is the count can be packed
* 
* C# shceduler is responsible for calculating the offset and count based on the actual buffer size and matrix dimension, 
* and make sure that count_1 will not exceeds lenght of the row
* 
* scalar pack carries column length and offset data:
* [column length, empty, empty, empty]
*/
NFN_MK_PROC_DECL_STD(float, sse, , fma_pack_right);
NFN_MK_PROC_DECL_STD(float, avx, , fma_pack_right);
NFN_MK_PROC_DECL_STD(float, 512, , fma_pack_right);

NFN_MK_PROC_DECL_STD(float, sse, , fma);
NFN_MK_PROC_DECL_STD(float, avx, , fma);
NFN_MK_PROC_DECL_STD(float, avx, _fma, fma);
NFN_MK_PROC_DECL_STD(float, 512, , fma);

NFN_MK_PROC_DECL_STD(float, sse, , fma_unpack);
NFN_MK_PROC_DECL_STD(float, avx, , fma_unpack);
NFN_MK_PROC_DECL_STD(float, 512, , fma_unpack);

NFN_MK_PROC_DECL_STD(double, sse, , fma_pack_left);
NFN_MK_PROC_DECL_STD(double, avx, , fma_pack_left);
NFN_MK_PROC_DECL_STD(double, 512, , fma_pack_left);

NFN_MK_PROC_DECL_STD(double, sse, , fma);
NFN_MK_PROC_DECL_STD(double, avx, , fma);
NFN_MK_PROC_DECL_STD(double, avx, _fma, fma);
NFN_MK_PROC_DECL_STD(double, 512, , fma);

NFN_MK_PROC_DECL_STD(double, sse, , fma_unpack);
NFN_MK_PROC_DECL_STD(double, avx, , fma_unpack);
NFN_MK_PROC_DECL_STD(double, 512, , fma_unpack);

NFN_SK_PROC_DECL_STD(float, avx, , add);
NFN_SK_PROC_DECL_STD(float, 512, , add);

NFN_SK_PROC_DECL_STD(double, sse, , add);
NFN_SK_PROC_DECL_STD(double, avx, , add);
NFN_SK_PROC_DECL_STD(double, 512, , add);

NFN_SK_PROC_DECL_STD(float, sse, , sub);
NFN_SK_PROC_DECL_STD(float, avx, , sub);
NFN_SK_PROC_DECL_STD(float, 512, , sub);

NFN_SK_PROC_DECL_STD(double, sse, , sub);
NFN_SK_PROC_DECL_STD(double, avx, , sub);
NFN_SK_PROC_DECL_STD(double, 512, , sub);

NFN_SK_PROC_DECL_STD(float, sse, , transpose);
NFN_SK_PROC_DECL_STD(float, avx, , transpose);
NFN_SK_PROC_DECL_STD(float, 512, , transpose);

NFN_SK_PROC_DECL_STD(float, sse, , scalar_mul);
NFN_SK_PROC_DECL_STD(float, avx, , scalar_mul);
NFN_SK_PROC_DECL_STD(float, 512, , scalar_mul);

NFN_SK_PROC_DECL_STD(double, sse, , scalar_mul);
NFN_SK_PROC_DECL_STD(double, avx, , scalar_mul);
NFN_SK_PROC_DECL_STD(double, 512, , scalar_mul);

NFN_SK_PROC_DECL_STD(float, sse, , fill);
NFN_SK_PROC_DECL_STD(float, avx, , fill);
NFN_SK_PROC_DECL_STD(float, 512, , fill);

NFN_SK_PROC_DECL_STD(double, sse, , fill);
NFN_SK_PROC_DECL_STD(double, avx, , fill);
NFN_SK_PROC_DECL_STD(double, 512, , fill);

NFN_SK_PROC_DECL_STD(float, sse, , quality);

NFN_SK_PROC_DECL_STD(float, sse, , fma);
NFN_SK_PROC_DECL_STD(float, avx, , fma);
NFN_SK_PROC_DECL_STD(float, avx, _fma, fma);
NFN_SK_PROC_DECL_STD(float, 512, , fma);

NFN_SK_PROC_DECL_STD(double, sse, , fma);
NFN_SK_PROC_DECL_STD(double, avx, , fma);
NFN_SK_PROC_DECL_STD(double, avx, _fma, fma);
NFN_SK_PROC_DECL_STD(double, 512, , fma);

#pragma endregion

#pragma region scaler_simd

INTERNAL uint64_t NFN_DECL factorial_simd_sse(int n);

#pragma endregion

#pragma region dot_product

INTERNAL void FORCEINLINE NFN_DECL dot_41_sse(MAT_KERNEL(float) const *transpose,
                                             __m128 const *vector, __m128 *ans);
INTERNAL void NFN_DECL dot_41_avx(MAT_KERNEL(float) const *transpose, __m128 const *vector,
                                 __m128 *ans);
INTERNAL void NFN_DECL dot_42_sse(MAT_KERNEL(float) const *transpose, __m128 const *vector,
                                 __m128 *ans);
INTERNAL void NFN_DECL dot_42_avx(MAT_KERNEL(float) const *transpose, __m128 const *vector,
                                 __m128 *ans);

#pragma endregion

#pragma region string

INTERNAL int NFN_DECL city_hash_simplify_sse(byte const *str, int length);
INTERNAL int NFN_DECL index_pair_sse(short const *stram, uint32_t target, int length);

#pragma endregion

#else

typedef enum intrinsic_level { SIMD0 = 0 } intrinsic_level;

#endif
#endif
