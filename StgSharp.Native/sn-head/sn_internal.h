#ifdef _MSC_VER
#pragma once
#endif

#ifndef SN_INTERNAL
#define SN_INTERNAL

#include <xmmintrin.h>

#ifdef _MSC_VER
#define _CRT_SECURE_NO_WARNINGS 1
#define WIN32_LEAN_AND_MEAN
#define SN_DECL __cdecl
#define SN_API __declspec(dllexport)
#define FORCEINLINE __forceinline
#include <windows.h>
#elif defined(__clang__) || defined(__GNUC__)
#define SN_DECL __attribute__((cdecl))
#define SN_API __attribute__((dllexport))
#define FORCEINLINE __attribute__((always_inline))
#else
#define SN_API
#define SN_DECL
#define FORCEINLINE
#endif

#define SN_APIDEF

#define PRIVATE static
#define INTERNAL extern

#endif //
