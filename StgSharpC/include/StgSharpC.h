#ifndef SSG
#define SSG




#define SSG_Api

#include "ssgc_framework.h"
#include "ssgc_internal.h"
#include "glad_gl.h"
#include "glad_wgl.h"
#include "glfw3.h"
#include <immintrin.h>

#pragma comment(lib,"glfw3.lib")

#if __cplusplus
extern "C"
{
#endif // _cplusplus

#pragma region  debug

    SSCAPI void printShaderCode(char* const* code);

#pragma endregion


#pragma region Internal

    extern __m128* defaultVecPtr;
#define ALIGN(simdVec) _mm_loadu_ps(&simdVec)

#pragma endregion

    SSCAPI char* infolog[512];


    SSCAPI void _cdecl initGL(int majorVersion, int minorVersion);
    SSCAPI int _cdecl initGLAD(void);
    SSCAPI unsigned int _cdecl loadShaderCode(char* const shaderCode, GLint shadertype);
    SSCAPI unsigned int _cdecl linkShaderProgram(GLuint shadertype);
    SSCAPI void _cdecl setCurrentGLContext(GLFWwindow* window, GladGLContext* contextPtr);
    SSCAPI char* _cdecl readLog(void);

    //linear related function

    SSCAPI __m128* __cdecl set_m128ptr_default();

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

