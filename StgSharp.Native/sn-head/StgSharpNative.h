#ifdef _MSC_VER
#pragma once
#endif

#ifndef SNative
#define SNative

#include "sn_internal.h"
#include "sn_target.h"

#define STBIDEF SN_API
#include "../lib/stbi/stb_image.h"

#include "gl.h"
// #include "wgl.h"
#define GLFWAPI INTERNAL
#include "glfw\glfw3.h"
#include <immintrin.h>
#include <assert.h>
#include <glfw_function.h>

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

extern SN_API char infolog[512];
SN_API int SN_DECL glCheckShaderStat(GladGLContext *context, uint64_t shaderHandle, int key,
                                     char **logRef);
SN_API void SN_DECL initGL(int majorVersion, int minorVersion);
SN_API void SN_DECL loadImageData(char *location, Image *out, imageLoader loader);
SN_API GLFWglproc SN_DECL loadGlfuncDefault(char *procName);
SN_API void SN_DECL unloadImageData(Image *out);
SN_API char *SN_DECL readLog(void);
SN_API void SN_DECL load_glfw_functions(GLFWFunctionTable *table);
SN_API void SN_DECL load_intrinsic_function(void *intrinsic_context, uint64_t id);
SN_API SIMDID SN_DECL sn_get_simd_level_global();
/* Per-core variant: always re-executes CPUID, never caches.
 * Must be called on the target core (after affinity is set). */
SN_API SIMDID SN_DECL sn_get_simd_level_local();
SN_API SIMDID SN_DECL sn_get_unite_simd(SIMDID left, SIMDID right);
SN_API int SN_DECL sn_compare_simd(SIMDID left, SIMDID right);
SN_API void SN_DECL sn_set_thread_affinity(int core_id, int numa_id);
SN_API void* SN_DECL sn_numa_alloc(size_t size, int numa_id);
SN_API void SN_DECL sn_numa_free(void *ptr, size_t size, int numa_id);
SN_API int SN_DECL sn_get_numa_each(int numa_id, uint64_t *internal, uint32_t *global);
SN_API int SN_DECL sn_get_numa_overall(int *out_numa_count, int *count_of_each_numa);
#endif
