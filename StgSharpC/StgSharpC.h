#ifndef SSC
#define SSC

#define SSC_Api

#include "ssgc_framework.h"
#include "ssgc_internal.h"
#include "gl.h"
#include "wgl.h"
#include "glfw3.h"
#include <immintrin.h>

#define STBIDEF SSCAPI
#include "stb_image.h"

#pragma comment(lib,"glfw3.lib")

#if __cplusplus
extern "C"
{
#endif // _cplusplus

#pragma region Internal

#define ALIGN(simdVec) _mm_loadu_ps(&simdVec)

    typedef struct Image {
        int width;
        int height;
        int channel;
        char* pixelPtr;
    }Image;

#pragma endregion

    typedef char* (*imageLoader)(char const* filename, int* x, int* y, int* channels_in_file, int desired_channels);

    SSCAPI char infolog[512];

    SSCAPI void SSCDECL initGL(int majorVersion, int minorVersion);
    SSCAPI unsigned int SSCDECL linkShaderProgram(GladGLContext* context, GLuint shadertype);
    SSCAPI void SSCDECL loadImageData(char* location, Image* out, imageLoader loader);
    SSCAPI GLFWglproc SSCDECL loadGlfuncDefault(char* procName);
    SSCAPI void SSCDECL unloadImageData(Image* out);
    SSCAPI char* SSCDECL readLog(void);

    //linear related function

    SSCAPI __m128* SSCDECL set_m128ptr_default();

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

#pragma endregion

#pragma region mat2

    SSCAPI _inline void __cdecl transpose2to3(matrix2map* source, matrix3map* target);
    SSCAPI _inline void __cdecl transpose2to4(matrix2map* source, matrix4map* target);
    SSCAPI _inline void __cdecl deinit_mat2(matrix2map* mat);

#pragma endregion

#pragma region mat3

    SSCAPI __inline void _cdecl transpose3to2(matrix3map* source, matrix2map* target);
    SSCAPI __inline void _cdecl transpose3to3(matrix3map* source, matrix3map* target);
    SSCAPI __inline void _cdecl transpose3to4(matrix3map* source, matrix4map* target);
    SSCAPI _inline float _cdecl det_mat3(matrix3map* mat);
    SSCAPI __inline float det_mat3_internal(__m128 const c1, __m128 const c2, __m128 const c3);

#pragma endregion

#pragma region mat4
    //type and function for 4 colum matrix

    SSCAPI __inline void __cdecl transpose4to2(matrix4map* source, matrix4map* target);
    SSCAPI __inline void __cdecl transpose4to4(matrix4map* matrix, matrix4map* target);

    SSCAPI __inline float __cdecl det_mat4(matrix4map* matPtr, matrix4map* transpose);

    SSCAPI __inline void __cdecl deinit_mat4(matrix4map* matPtr);

#pragma endregion

#ifdef  __cplusplus
}
#endif //  __cplusplus

#endif // !SSG
