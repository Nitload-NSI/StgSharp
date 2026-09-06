#ifndef NIF_TARGET
#define NIF_TARGET

#include "nif_internal.h"
#include <stdint.h>

#define NIF_OS_WINDOWS 0
#define NIF_OS_LINUX 1
#define NIF_OS_MACOS 2
#define NIF_OS_IOS 3
#define NIF_OS_ANDROID 4
#define NIF_OS_UNKNOWN 5

#define NIF_ARCH_X86_64 0
#define NIF_ARCH_ARM64 1
#define NIF_ARCH_RISCV64 2
#define NIF_ARCH_PPC64le 3

#if defined(_M_X64) || defined(_M_AMD64) || defined(__x86_64__) || defined(__amd64__)
#define NIF_CURRENT_ARCH NIF_ARCH_X86_64

#elif defined(_M_ARM64) || defined(__aarch64__)
#define NIF_CURRENT_ARCH NIF_ARCH_ARM64

#elif defined(_M_ARM) || defined(__arm__)
#define NIF_CURRENT_ARCH NIF_ARCH_ARM32

#elif defined(_M_IX86) || defined(__i386__)
#define NIF_CURRENT_ARCH NIF_ARCH_X86_32

#elif defined(__powerpc64__) && defined(__LITTLE_ENDIAN__)
#define NIF_CURRENT_ARCH NIF_ARCH_PPC64LE

#elif defined(__riscv) && (__riscv_xlen == 64)
#define NIF_CURRENT_ARCH NIF_ARCH_RISCV64

#else
#error "SN: Unsupported target architecture"
#endif

/*
 * Target selection policy:
 * - Default (no NIF_EXTERN_TARGET): fixed Windows + x86_64.
 * - External (NIF_EXTERN_TARGET): build system must provide NIF_OS and NIF_ARCH.
 */
#ifndef NIF_EXTERN_TARGET

#define NIF_OS_TARGET NIF_OS_WINDOWS
#define NIF_ARCH_TARGET NIF_ARCH_X86_64

#else

#ifndef NIF_OS_TARGET
#error "NIF_EXTERN_TARGET is defined but NIF_OS is not provided."
#endif

#ifndef NIF_ARCH_TARGET
#error "NIF_EXTERN_TARGET is defined but NIF_ARCH is not provided."
#endif

/* Validate externally supplied values are within known enum constants. */
#if (NIF_OS_TARGET != NIF_OS_WINDOWS) && (NIF_OS_TARGET != NIF_OS_LINUX) && \
        (NIF_OS_TARGET != NIF_OS_MACOS) && (NIF_OS_TARGET != NIF_OS_IOS) && \
        (NIF_OS_TARGET != NIF_OS_ANDROID) && (NIF_OS_TARGET != NIF_OS_UNKNOWN)
#error "NIF_OS has an invalid value. Use NIF_OS_<...> enum constants."
#endif

#if (NIF_ARCH_TARGET != NIF_ARCH_X86_64) && (NIF_ARCH_TARGET != NIF_ARCH_ARM64) && \
        (NIF_ARCH_TARGET != NIF_ARCH_RISCV64) && (NIF_ARCH_TARGET != NIF_ARCH_PPC64le)
#error "NIF_ARCH has an invalid value. Use NIF_ARCH_<...> enum constants."
#endif

#endif

#define NIF_IS_OS(os) ((NIF_OS_TARGET) == (os))

#define NIF_IS_ARCH(arch) ((NIF_ARCH_TARGET) == (arch))

/*
 * Per-function ISA selection is required for runtime-dispatched kernels that
 * mix multiple x86 feature levels in one build target.
 */
#if defined(__clang__) && ((NIF_ARCH_TARGET) == NIF_ARCH_X86_64)
#define NIF_CLANG_TARGET_ATTR(options) __attribute__((target(options)))
#else
#define NIF_CLANG_TARGET_ATTR(options)
#endif

typedef union SIMDID {
        uint64_t mask;
        char make_byte[8];
} SIMDID;

#endif // NIF_TARGET
