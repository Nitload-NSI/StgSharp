#ifdef _MSC_VER
#pragma once
#endif

#include <xmmintrin.h>

#ifndef SSC_INTERNAL
#define SSC_INTERNAL

#ifdef _MSC_VER
#define _CRT_SECURE_NO_WARNINGS 1
#define WIN32_LEAN_AND_MEAN
#define SSCDECL __cdecl
#define SSCAPI __declspec(dllexport)
#define FORCEINLINE __forceinline
#include <windows.h>
#elif defined(__clang__) || defined(__GNUC__)
#define SSCDECL __attribute__((cdecl))
#define SSCAPI __attribute__((dllexport))
#define FORCEINLINE __attribute__((always_inline))
#else
#define SSCAPI
#define SSCDECL
#define FORCEINLINE
#endif

#define SSCAPI_DEFINE

#define PRIVATE static
#define INTERNAL extern

#endif //
