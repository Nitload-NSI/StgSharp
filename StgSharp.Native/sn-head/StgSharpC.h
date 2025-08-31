#ifdef _MSC_VER
#pragma once
#endif

#ifndef SSC
#define SSC

#include "ssc_internal.h"

#define STBIDEF SSCAPI
#include "stb_image.h"

#include "gl.h"
#include "wgl.h"
#include "glfw3.h"
#include <immintrin.h>
#include <assert.h>

#ifndef SSCAPI_DEFINE

#ifdef _WIN32
#define SSCAPI __declspec(dllexport)
#elif defined _LINUX_
#define SSCAPI __attribute__((dllexport))
#elif defined _APPLE_
#define SSCAPI __attribute__((visibility("default")))
#else
#define SSCAPI
#endif

#endif // ! SSCAPI_DEFINE

#if __cplusplus
extern "C" {
#endif

#define byte char
#define cs_char uint16_t
#define ssc_null_assert(ptr) assert(ptr != NULL)

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

SSCAPI char infolog[512];
SSCAPI int SSCDECL glCheckShaderStat(GladGLContext *context, uint64_t shaderHandle, int key,
                                     char **logRef);
SSCAPI void SSCDECL initGL(int majorVersion, int minorVersion);
SSCAPI unsigned int SSCDECL linkShaderProgram(GladGLContext *context, GLuint shadertype);
SSCAPI void SSCDECL loadImageData(char *location, Image *out, imageLoader loader);
SSCAPI GLFWglproc SSCDECL loadGlfuncDefault(char *procName);
SSCAPI void SSCDECL unloadImageData(Image *out);
SSCAPI char *SSCDECL readLog(void);

#pragma endregion

SSCAPI int SSCDECL load_intrinsic_function(void *intrinsic_context);

#ifdef __cplusplus
}
#endif

#endif
