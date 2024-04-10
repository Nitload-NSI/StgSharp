#pragma once
#include "xmmintrin.h"
/*
#include "GLAD/gl.h"
#include "glfw3.h"
#include "StgSharpC.h"
*/

#ifndef SSG_INTERNAL
#define SSG_INTERNAL

#ifdef _WIN32

#define WIN32_LEAN_AND_MEAN             // �� Windows ͷ�ļ����ų�����ʹ�õ�����
// Windows ͷ�ļ�
#include <windows.h>

#define SSCAPI __declspec(dllexport)
#elif defined _LINUX_
#define SSCAPI __attribute__ ((dllexport))
#elif defined _APPLE_
#define SSCAPI __attribute__((visibility("default")))
#else
#define SSCAPI
#endif

#ifdef _WIN32
#define _CRT_SECURE_NO_WARNINGS 1
#endif

#define SSCDECL __cdecl

#define zeroVec (__m128){0,0,0,0}

#endif //
