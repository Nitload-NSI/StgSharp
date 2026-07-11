#ifndef SN_INTRINSIC_CONTEXT_BASE
#define SN_INTRINSIC_CONTEXT_BASE


// -----------------------------------------------------------------------------
// SIMDID layout helpers (see SIMDID.x86.md)
// bits [3..0]  : Root ISA
// bits [7..4]  : Manufacture (ISA-specific)
// bits [15..8] : MainLevel (monotonic capability)
// -----------------------------------------------------------------------------
#define SIMDID_SHIFT_ROOT 0
#define SIMDID_SHIFT_MANU 4
#define SIMDID_SHIFT_MAIN 8


#ifndef SIMDID_PACK_ROOT
#define SIMDID_PACK_ROOT(root) (((uint64_t)(root) & 0xFULL) << SIMDID_SHIFT_ROOT)
#define SIMDID_PACK_MANU(manu) (((uint64_t)(manu) & 0xFULL) << SIMDID_SHIFT_MANU)
#define SIMDID_PACK_MAIN(main) (((uint64_t)(main) & 0xFFULL) << SIMDID_SHIFT_MAIN)
#endif

// Root ISA (low nibble)
#define SIMDID_ROOT_UNKNOWN 0x0     // cannot find root isa, error branch
#define SIMDID_ROOT_X86_64 0x1      // support
#define SIMDID_ROOT_AARCH64 0x2     // schedualed
#define SIMDID_ROOT_PPC64LE 0x3     // willing, but no plan yet
#define SIMDID_ROOT_RISCV64 0x4     // willing, but no plan yet
#define SIMDID_ROOT_LOONGARCH 0x5   // willing, but no plan yet

#define SIMDID_MASK_ROOT 0xFULL
#define SIMDID_MASK_MANU (0xFULL << SIMDID_SHIFT_MANU)
#define SIMDID_MASK_MAIN (0xFFULL << SIMDID_SHIFT_MAIN)

#endif