#ifdef _MSC_VER
#pragma once
#endif

#ifndef SNative
#define SNative

#include "sn_internal.h"

#define STBIDEF SN_API
#include "stb_image.h"

#include "gl.h"
#include "wgl.h"
#define GLFWAPI INTERNAL
#include "..\glfw\glfw3.h"
#include <immintrin.h>
#include <assert.h>
#include <glfw_function.h>

#ifndef SN_APIDEF

#ifdef _WIN32
#define SN_API __declspec(dllexport)
#elif defined _LINUX_
#define SN_API __attribute__((dllexport))
#elif defined _APPLE_
#define SN_API __attribute__((visibility("default")))
#else
#define SN_API
#endif

#endif // ! SN_APIDEF

#if __cplusplus
extern "C" {
#endif

#define byte char
#define cs_char uint16_t
#define sn_null_assert(ptr) assert(ptr != NULL)

#define ALIGN(simdVec) _mm_loadu_ps(&simdVec)
#define ALIGN256(simdVec) _mm256_loadu_ps(&simdVec)
#define ALIGN512(simdVec) _mm512_loadu_ps(&simdVec)

typedef struct Image {
        int width;
        int height;
        int channel;
        char *pixelPtr;
} Image;

typedef char *(*imageLoader)(char const *filename, int *x, int *y, int *channels_in_file,
                             int desired_channels);

SN_API char infolog[512];
SN_API int SN_DECL glCheckShaderStat(GladGLContext *context, uint64_t shaderHandle, int key,
                                     char **logRef);
SN_API void SN_DECL initGL(int majorVersion, int minorVersion);
SN_API unsigned int SN_DECL linkShaderProgram(GladGLContext *context, GLuint shadertype);
SN_API void SN_DECL loadImageData(char *location, Image *out, imageLoader loader);
SN_API GLFWglproc SN_DECL loadGlfuncDefault(char *procName);
SN_API void SN_DECL unloadImageData(Image *out);
SN_API char *SN_DECL readLog(void);
SN_API void SN_DECL load_glfw_functions(GLFWFunctionTable *table);
SN_API int SN_DECL load_intrinsic_function(void *intrinsic_context);

#ifdef __cplusplus
}
#endif

#endif
