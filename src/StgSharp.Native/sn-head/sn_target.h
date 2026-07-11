#ifndef SN_TARGET
#define SN_TARGET

#include "sn_internal.h"
#include <stdint.h>

#define SN_OS_WINDOWS 0
#define SN_OS_LINUX 1
#define SN_OS_MACOS 2
#define SN_OS_IOS 3
#define SN_OS_ANDROID 4
#define SN_OS_UNKNOWN 5

#define SN_ARCH_X86_64 0
#define SN_ARCH_ARM64 1
#define SN_ARCH_RISCV64 2
#define SN_ARCH_PPC64le 3

#if defined(_M_X64) || defined(_M_AMD64) || defined(__x86_64__) || defined(__amd64__)
#define SN_CURRENT_ARCH SN_ARCH_X86_64

#elif defined(_M_ARM64) || defined(__aarch64__)
#define SN_CURRENT_ARCH SN_ARCH_ARM64

#elif defined(_M_ARM) || defined(__arm__)
#define SN_CURRENT_ARCH SN_ARCH_ARM32

#elif defined(_M_IX86) || defined(__i386__)
#define SN_CURRENT_ARCH SN_ARCH_X86_32

#elif defined(__powerpc64__) && defined(__LITTLE_ENDIAN__)
#define SN_CURRENT_ARCH SN_ARCH_PPC64LE

#elif defined(__riscv) && (__riscv_xlen == 64)
#define SN_CURRENT_ARCH SN_ARCH_RISCV64

#else
#error "SN: Unsupported target architecture"
#endif

/*
 * Target selection policy:
 * - Default (no SN_EXTERN_TARGET): fixed Windows + x86_64.
 * - External (SN_EXTERN_TARGET): build system must provide SN_OS and SN_ARCH.
 */
#ifndef SN_EXTERN_TARGET

#define SN_OS_TARGET SN_OS_WINDOWS
#define SN_ARCH_TARGET SN_ARCH_X86_64

#else

#ifndef SN_OS_TARGET
#error "SN_EXTERN_TARGET is defined but SN_OS is not provided."
#endif

#ifndef SN_ARCH_TARGET
#error "SN_EXTERN_TARGET is defined but SN_ARCH is not provided."
#endif

/* Validate externally supplied values are within known enum constants. */
#if (SN_OS_TARGET != SN_OS_WINDOWS) && (SN_OS_TARGET != SN_OS_LINUX) && \
        (SN_OS_TARGET != SN_OS_MACOS) && (SN_OS_TARGET != SN_OS_IOS) && \
        (SN_OS_TARGET != SN_OS_ANDROID) && (SN_OS_TARGET != SN_OS_UNKNOWN)
#error "SN_OS has an invalid value. Use SN_OS_<...> enum constants."
#endif

#if (SN_ARCH_TARGET != SN_ARCH_X86_64) && (SN_ARCH_TARGET != SN_ARCH_ARM64) && \
        (SN_ARCH_TARGET != SN_ARCH_RISCV64) && (SN_ARCH_TARGET != SN_ARCH_PPC64le)
#error "SN_ARCH has an invalid value. Use SN_ARCH_<...> enum constants."
#endif

#endif

#define SN_IS_OS(os) ((SN_OS_TARGET) == (os))

#define SN_IS_ARCH(arch) ((SN_ARCH_TARGET) == (arch))

/*
 * Per-function ISA selection is required for runtime-dispatched kernels that
 * mix multiple x86 feature levels in one build target.
 */
#if defined(__clang__) && ((SN_ARCH_TARGET) == SN_ARCH_X86_64)
#define SN_CLANG_TARGET_ATTR(options) __attribute__((target(options)))
#else
#define SN_CLANG_TARGET_ATTR(options)
#endif

typedef union SIMDID {
        uint64_t mask;
        char make_byte[8];
} SIMDID;

#endif // SN_TARGET
