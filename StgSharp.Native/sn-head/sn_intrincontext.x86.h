#include "sn_target.h"

#if SN_IS_ARCH(SN_ARCH_X86_64)

#include "sn_intrinsic.h"

// -----------------------------------------------------------------------------
// SIMDID layout helpers (see SIMDID.x86.md)
// bits [3..0]  : Root ISA
// bits [7..4]  : Manufacture (ISA-specific)
// bits [15..8] : MainLevel (monotonic capability)
// bits [31..16]: AVX512 Feature
// bits [39..32]: AMX Feature
// bits [47..40]: AVX Feature
// bits [63..56]: uArchHi / Policy (optional)
// -----------------------------------------------------------------------------
#define SIMDID_SHIFT_ROOT 0
#define SIMDID_SHIFT_MANU 4
#define SIMDID_SHIFT_MAIN 8
#define SIMDID_SHIFT_AVX512 16
#define SIMDID_SHIFT_AMX 32
#define SIMDID_SHIFT_AVX 40
#define SIMDID_SHIFT_UARCH 56

#define SIMDID_MASK_ROOT 0xFULL
#define SIMDID_MASK_MANU (0xFULL << SIMDID_SHIFT_MANU)
#define SIMDID_MASK_MAIN (0xFFULL << SIMDID_SHIFT_MAIN)
#define SIMDID_MASK_AVX512 (0xFFFFULL << SIMDID_SHIFT_AVX512)
#define SIMDID_MASK_AMX (0xFFULL << SIMDID_SHIFT_AMX)
#define SIMDID_MASK_AVX (0xFFULL << SIMDID_SHIFT_AVX)
#define SIMDID_MASK_UARCH (0xFFULL << SIMDID_SHIFT_UARCH)

// Root ISA (low nibble)
#define SIMDID_ROOT_UNKNOWN 0x0
#define SIMDID_ROOT_X86_64 0x1
#define SIMDID_ROOT_AARCH64 0x2
#define SIMDID_ROOT_PPC64LE 0x3
#define SIMDID_ROOT_APPLESILICON 0x4
#define SIMDID_ROOT_RISCV64 0x5
#define SIMDID_ROOT_LOONGARCH 0x6

// Manufacture (x86-64 mapping; other ISAs may reinterpret)
#define SIMDID_MANU_UNKNOWN 0x0
#define SIMDID_MANU_INTEL 0x1
#define SIMDID_MANU_AMD 0x2
#define SIMDID_MANU_VIA 0x3

// MainLevel (ordinal, shared bits 8..15; not bit-flags)
#define SIMDID_MAIN_LVL_NONE 0x00u
#define SIMDID_MAIN_LVL_SSE 0x01u /* SSE1..SSE4.2 */
#define SIMDID_MAIN_LVL_AVX2 0x02u
#define SIMDID_MAIN_LVL_AVX512 0x03u
#define SIMDID_MAIN_LVL_AMX 0x04u
#define SIMDID_MAIN_LVL_AVX10 0x05u

#ifndef SIMDID_AVX512_BASE
#define SIMDID_AVX512_FP16 (1U << 29)
#define SIMDID_AVX512_BF16 (1U << 28)
#define SIMDID_AVX512_VNNI (1U << 27)
#define SIMDID_AVX512_VBMI2 (1U << 26)
#define SIMDID_AVX512_VBMI (1U << 25)
#define SIMDID_AVX512_AI_RESERVED (1U << 24)
#define SIMDID_AVX512_BASE (1U << 20)
#define SIMDID_AVX512_IMPL_SHIFT 22
#define SIMDID_AVX512_IMPL_MASK (0x3U << SIMDID_AVX512_IMPL_SHIFT)
#define SIMDID_AVX512_IMPL_UNKNOWN (0x0U << SIMDID_AVX512_IMPL_SHIFT)
#define SIMDID_AVX512_IMPL_DUAL (0x1U << SIMDID_AVX512_IMPL_SHIFT)
#define SIMDID_AVX512_IMPL_NATIVE (0x2U << SIMDID_AVX512_IMPL_SHIFT)
#endif

#ifndef SIMDID_AVX_BASE
#define SIMDID_AVX (1ULL << 40)
#define SIMDID_AVX2 (1ULL << 41)
#define SIMDID_AVX_FMA (1ULL << 42)
#define SIMDID_AVX_F16C (1ULL << 43)
#endif

#ifndef SIMDID_AMX_TILE
#define SIMDID_AMX_TILE (1ULL << 32)
#define SIMDID_AMX_INT8 (1ULL << 33)
#define SIMDID_AMX_BF16 (1ULL << 34)
#endif

// Pack helpers (define locally to avoid missing macros when included standalone)
#ifndef SIMDID_PACK_ROOT
#define SIMDID_PACK_ROOT(root) (((uint64_t)(root) & 0xFULL) << SIMDID_SHIFT_ROOT)
#define SIMDID_PACK_MANU(manu) (((uint64_t)(manu) & 0xFULL) << SIMDID_SHIFT_MANU)
#define SIMDID_PACK_MAIN(main) (((uint64_t)(main) & 0xFFULL) << SIMDID_SHIFT_MAIN)
#define SIMDID_PACK_AVX512(avx512_bits) \
        (((uint64_t)(avx512_bits) & 0xFFFFULL) << SIMDID_SHIFT_AVX512)
#define SIMDID_PACK_AMX(amx_bits) (((uint64_t)(amx_bits) & 0xFFULL) << SIMDID_SHIFT_AMX)
#define SIMDID_PACK_AVX(avx_bits) (((uint64_t)(avx_bits) & 0xFFULL) << SIMDID_SHIFT_AVX)
#define SIMDID_PACK_UARCH(uarch_hi) (((uint64_t)(uarch_hi) & 0xFFULL) << SIMDID_SHIFT_UARCH)
#endif

/* Pre-shifted helpers for convenience when constructing SIMDID */
#define SIMDID_MAIN_ENC_NONE SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_NONE)
#define SIMDID_MAIN_ENC_SSE SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_SSE)
#define SIMDID_MAIN_ENC_AVX2 SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_AVX2)
#define SIMDID_MAIN_ENC_AVX512 SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_AVX512)
#define SIMDID_MAIN_ENC_AMX SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_AMX)
#define SIMDID_MAIN_ENC_AVX10 SIMDID_PACK_MAIN(SIMDID_MAIN_LVL_AVX10)

#pragma region matix function

INTERNAL void f32_normalize(VEC(float) * source, VEC(float) * target);

DECLARE_BUF_PROC_LEFT_RIGHT_ANS(float, sse, , add);
DECLARE_BUF_PROC_LEFT_RIGHT_ANS(float, avx, , add);
DECLARE_BUF_PROC_LEFT_RIGHT_ANS(float, 512, , add);

DECLARE_BUF_PROC_LEFT_RIGHT_ANS(double, sse, , add);
DECLARE_BUF_PROC_LEFT_RIGHT_ANS(double, avx, , add);
DECLARE_BUF_PROC_LEFT_RIGHT_ANS(double, 512, , add);

DECLARE_BUF_PROC_LEFT_RIGHT_ANS(float, sse, , sub);
DECLARE_BUF_PROC_LEFT_RIGHT_ANS(float, avx, , sub);
DECLARE_BUF_PROC_LEFT_RIGHT_ANS(float, 512, , sub);

DECLARE_BUF_PROC_LEFT_RIGHT_ANS(double, sse, , sub);
DECLARE_BUF_PROC_LEFT_RIGHT_ANS(double, avx, , sub);
DECLARE_BUF_PROC_LEFT_RIGHT_ANS(double, 512, , sub);

DECLARE_KER_PROC_RIGHT_ANS(float, sse, , transpose);
DECLARE_KER_PROC_RIGHT_ANS(float, avx, , transpose);
DECLARE_KER_PROC_RIGHT_ANS(float, 512, , transpose);

DECLARE_BUF_PROC_RIGHT_ANS_SCALAR(float, sse, , scalar_mul);
DECLARE_BUF_PROC_RIGHT_ANS_SCALAR(float, avx, , scalar_mul);
DECLARE_BUF_PROC_RIGHT_ANS_SCALAR(float, 512, , scalar_mul);

DECLARE_BUF_PROC_RIGHT_ANS_SCALAR(double, sse, , scalar_mul);
DECLARE_BUF_PROC_RIGHT_ANS_SCALAR(double, avx, , scalar_mul);
DECLARE_BUF_PROC_RIGHT_ANS_SCALAR(double, 512, , scalar_mul);

DECLARE_BUF_PROC_ANS_SCALAR(float, sse, , fill);
DECLARE_BUF_PROC_ANS_SCALAR(float, avx, , fill);
DECLARE_BUF_PROC_ANS_SCALAR(float, 512, , fill);

DECLARE_BUF_PROC_ANS_SCALAR(double, sse, , fill);
DECLARE_BUF_PROC_ANS_SCALAR(double, avx, , fill);
DECLARE_BUF_PROC_ANS_SCALAR(double, 512, , fill);

DECLARE_PNL_PROC_ANS(float, sse, , clear);
DECLARE_PNL_PROC_ANS(float, avx, , clear);
DECLARE_PNL_PROC_ANS(float, 512, , clear);

DECLARE_PNL_PROC_ANS(double, sse, , clear);
DECLARE_PNL_PROC_ANS(double, avx, , clear);
DECLARE_PNL_PROC_ANS(double, 512, , clear);

DECLARE_KER_PROC_LEFT_RIGHT_ANS(float, sse, , fma);
DECLARE_KER_PROC_LEFT_RIGHT_ANS(float, avx, , fma);
DECLARE_KER_PROC_LEFT_RIGHT_ANS(float, avx, _fma, fma);
DECLARE_KER_PROC_LEFT_RIGHT_ANS(float, 512, , fma);

DECLARE_KER_PROC_LEFT_RIGHT_ANS(double, sse, , fma);
DECLARE_KER_PROC_LEFT_RIGHT_ANS(double, avx, , fma);
DECLARE_KER_PROC_LEFT_RIGHT_ANS(double, avx, _fma, fma);
DECLARE_KER_PROC_LEFT_RIGHT_ANS(double, 512, , fma);

DECLARE_KERTILE_PROC_LEFT_RIGHT_ANS(float, sse, , fma);
DECLARE_KERTILE_PROC_LEFT_RIGHT_ANS(float, avx, , fma);
DECLARE_KERTILE_PROC_LEFT_RIGHT_ANS(float, avx, _fma, fma);
DECLARE_KERTILE_PROC_LEFT_RIGHT_ANS(float, 512, , fma);

DECLARE_KERTILE_PROC_LEFT_RIGHT_ANS(double, sse, , fma);
DECLARE_KERTILE_PROC_LEFT_RIGHT_ANS(double, avx, , fma);
DECLARE_KERTILE_PROC_LEFT_RIGHT_ANS(double, avx, _fma, fma);
DECLARE_KERTILE_PROC_LEFT_RIGHT_ANS(double, 512, , fma);

#pragma endregion

#pragma region scaler_simd

INTERNAL uint64_t SN_DECL factorial_simd_sse(int n);

#pragma endregion

#pragma region dot_product

INTERNAL void FORCEINLINE SN_DECL dot_41_sse(MAT_KERNEL(float) const *transpose,
                                             __m128 const *vector, __m128 *ans);
INTERNAL void SN_DECL dot_41_avx(MAT_KERNEL(float) const *transpose, __m128 const *vector,
                                 __m128 *ans);
INTERNAL void SN_DECL dot_42_sse(MAT_KERNEL(float) const *transpose, __m128 const *vector,
                                 __m128 *ans);
INTERNAL void SN_DECL dot_42_avx(MAT_KERNEL(float) const *transpose, __m128 const *vector,
                                 __m128 *ans);

#pragma endregion

#pragma region string

INTERNAL int SN_DECL city_hash_simplify_sse(byte const *str, int length);
INTERNAL int SN_DECL index_pair_sse(short const *stram, uint32_t target, int length);

#pragma endregion

#else

typedef enum intrinsic_level { SIMD0 = 0 } intrinsic_level;

#endif
