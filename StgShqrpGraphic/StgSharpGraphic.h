#ifndef SSG
#define SSG

#define SSG_Api

#ifdef SSG_Api
#define SSGC_API __declspec(dllexport)
#else
#define SSGC_API __declspec(dllimport)
#endif

#include "framework.h"
#include "GLenv/include/glad/glad.h"
#include "GLenv/include/GLFW/glfw3.h"
#include <immintrin.h>

#pragma region  debug



SSGC_API void getApiReporter(uint64_t reporterAddress);

#pragma endregion



#pragma region Internal

extern __m128* defaultVecPtr;
#define ALIGN(simdVec) _mm_loadu_ps((float*)&simdVec)




SSGC_API _inline float __cdecl mulps_sum_internal(__m128 left, __m128 right);
SSGC_API _inline float __cdecl mulps_total_internal(__m128 vec);

#pragma endregion






SSGC_API void _cdecl initGL(int majorVersion, int minorVersion);
SSGC_API int _cdecl initGLAD(void);



//linear related function




SSGC_API __m128* __cdecl set_m128ptr_default();





#pragma region mat define

typedef union matrix2map
{
    __m128 colum[2];
    float m[4][2];
}matrix2map;

typedef struct matrix3map
{
    __m128 colum[3];
    float m[4][3];
}matrix3map;

typedef union matrix4map {
    __m128 colum[4];
    float m[4][4];
}matrix4map;

typedef struct matrix4 {
    matrix4map matrix;
    matrix4map transpose;
}matrix4;

#pragma endregion




#pragma region mat2


SSGC_API _inline void __cdecl transpose2to3(matrix2map* source, matrix3map* target);
SSGC_API _inline void __cdecl transpose2to4(matrix2map* source, matrix4map* target);

SSGC_API _inline void __cdecl deinit_mat2(matrix2map* mat);

#pragma endregion


#pragma region mat3


SSGC_API __inline void _cdecl transpose3to2(matrix3map* source, matrix2map* target);
SSGC_API __inline void _cdecl transpose3to3(matrix3map* source, matrix3map* target);
SSGC_API __inline void _cdecl transpose3to4(matrix3map* source, matrix4map* target);

SSGC_API _inline float _cdecl det_mat3(matrix3map* mat);
SSGC_API __inline float det_mat3_internal(__m128 const c1, __m128 const c2, __m128 const c3);

#pragma endregion




#pragma region mat4
//type and function for 4 colum matrix


SSGC_API __inline void __cdecl transpose4to2(matrix4map* source, matrix4map* target);
SSGC_API __inline void __cdecl transpose4to4(matrix4map* matrix, matrix4map* target);

SSGC_API __inline float __cdecl det_mat4(matrix4map* matPtr, matrix4map* transpose);

SSGC_API __inline void __cdecl deinit_mat4(matrix4map* matPtr);

#pragma endregion











#endif // !SSG

