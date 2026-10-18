#ifndef NFN_TARGET
#define NFN_TARGET

#include "nfn_internal.h"
#include <stdint.h>

#define NFN_OS_WINDOWS 0
#define NFN_OS_LINUX 1
#define NFN_OS_MACOS 2
#define NFN_OS_IOS 3
#define NFN_OS_ANDROID 4
#define NFN_OS_UNKNOWN 5

#define NFN_ARCH_X86_64 0
#define NFN_ARCH_ARM64 1
#define NFN_ARCH_RISCV64 2
#define NFN_ARCH_PPC64le 3

#if defined(_M_X64) || defined(_M_AMD64) || defined(__x86_64__) || defined(__amd64__)
#define NFN_CURRENT_ARCH NFN_ARCH_X86_64

#elif defined(_M_ARM64) || defined(__aarch64__)
#define NFN_CURRENT_ARCH NFN_ARCH_ARM64

#elif defined(_M_ARM) || defined(__arm__)
#define NFN_CURRENT_ARCH NFN_ARCH_ARM32

#elif defined(_M_IX86) || defined(__i386__)
#define NFN_CURRENT_ARCH NFN_ARCH_X86_32

#elif defined(__powerpc64__) && defined(__LITTLE_ENDIAN__)
#define NFN_CURRENT_ARCH NFN_ARCH_PPC64LE

#elif defined(__riscv) && (__riscv_xlen == 64)
#define NFN_CURRENT_ARCH NFN_ARCH_RISCV64

#else
#error "SN: Unsupported target architecture"
#endif

/*
 * Target selection policy:
 * - Default (no NFN_EXTERN_TARGET): fixed Windows + x86_64.
 * - External (NFN_EXTERN_TARGET): build system must provide NFN_OS and NFN_ARCH.
 */
#ifndef NFN_EXTERN_TARGET

#define NFN_OS_TARGET NFN_OS_WINDOWS
#define NFN_ARCH_TARGET NFN_ARCH_X86_64

#else

#ifndef NFN_OS_TARGET
#error "NFN_EXTERN_TARGET is defined but NFN_OS is not provided."
#endif

#ifndef NFN_ARCH_TARGET
#error "NFN_EXTERN_TARGET is defined but NFN_ARCH is not provided."
#endif

/* Validate externally supplied values are within known enum constants. */
#if (NFN_OS_TARGET != NFN_OS_WINDOWS) && (NFN_OS_TARGET != NFN_OS_LINUX) && \
        (NFN_OS_TARGET != NFN_OS_MACOS) && (NFN_OS_TARGET != NFN_OS_IOS) && \
        (NFN_OS_TARGET != NFN_OS_ANDROID) && (NFN_OS_TARGET != NFN_OS_UNKNOWN)
#error "NFN_OS has an invalid value. Use NFN_OS_<...> enum constants."
#endif

#if (NFN_ARCH_TARGET != NFN_ARCH_X86_64) && (NFN_ARCH_TARGET != NFN_ARCH_ARM64) && \
        (NFN_ARCH_TARGET != NFN_ARCH_RISCV64) && (NFN_ARCH_TARGET != NFN_ARCH_PPC64le)
#error "NFN_ARCH has an invalid value. Use NFN_ARCH_<...> enum constants."
#endif

#endif

#define NFN_IS_OS(os) ((NFN_OS_TARGET) == (os))

#define NFN_IS_ARCH(arch) ((NFN_ARCH_TARGET) == (arch))

/*
 * Per-function ISA selection is required for runtime-dispatched kernels that
 * mix multiple x86 feature levels in one build target.
 */
#if defined(__clang__) && ((NFN_ARCH_TARGET) == NFN_ARCH_X86_64)
#define NFN_CLANG_TARGET_ATTR(options) __attribute__((target(options)))
#else
#define NFN_CLANG_TARGET_ATTR(options)
#endif

typedef union SIMDID {
        uint64_t mask;
        char make_byte[8];
} SIMDID;

#endif // NFN_TARGET
