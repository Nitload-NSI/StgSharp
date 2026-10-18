
#ifndef NFN_INTERNAL
#define NFN_INTERNAL

#include <xmmintrin.h>

#define UNSAFE_AS(type, expr) *(type *)&(expr)

#ifdef _MSC_VER
#define _CRT_SECURE_NO_WARNINGS 1
#define WIN32_LEAN_AND_MEAN
#define NFN_DECL __cdecl
#define NFN_MAT_DECL __cdecl __forceinline
#define NFN_API __declspec(dllexport)
#define FORCEINLINE __forceinline
#define NOINLINE __declspec(noinline)
#include <windows.h>
#elif defined(__clang__) || defined(__GNUC__)
#define NFN_DECL __attribute__((cdecl))
#define NFN_MAT_DECL __attribute__((cdecl)) __attribute__((always_inline))
#define NFN_API __attribute__((dllexport))
#define FORCEINLINE __attribute__((always_inline))
#define NOINLINE __attribute__((noinline))
#else
#define NFN_API
#define NFN_DECL
#define NFN_MAT_DECL
#define FORCEINLINE
#define NOINLINE
#endif

#if defined(_MSC_VER)
#define NFN_ALIGNAS(n) __declspec(align(n))
#else
#define NFN_ALIGNAS(n) __attribute__((aligned(n)))
#endif

#define NFN_APIDEF

#define PRIVATE static
#define INTERNAL extern

#define u64 uint64_t

#endif //
