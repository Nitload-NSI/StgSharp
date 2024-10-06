#ifdef _MSC_VER
#pragma once
#endif

#include "xmmintrin.h"
/*
#include "GLAD/gl.h"
#include "glfw3.h"
#include "StgSharpC.h"
*/

#ifndef SSC_INTERNAL
#define SSC_INTERNAL

#ifdef _WIN32
#define WIN32_LEAN_AND_MEAN
#define SSCAPI __declspec(dllexport)
#include <windows.h>
#elif defined _LINUX_
#define SSCAPI __attribute__((dllexport))
#elif defined _APPLE_
#define SSCAPI __attribute__((visibility("default")))
#else
#define SSCAPI
#endif

#ifdef _WIN32
#define _CRT_SECURE_NO_WARNINGS 1
#endif

#define SSCDECL __cdecl

#define zeroVec _mm_setzero_ps()

#define SSCAPI_DEFINE

#define INTERNAL extern

#endif //
