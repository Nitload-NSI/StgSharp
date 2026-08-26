#ifndef SNative
#define SNative

#ifdef __cplusplus
#error "This header is not meant to be included in C++ code. StgSharp.Native is a C library and should be used with C linkage."
#endif

#include "sn_internal.h"
#include "sn_target.h"

#include <immintrin.h>
#include <assert.h>
#include <glfw_function.h>

#define byte char
#define cs_char uint16_t
#define sn_null_assert(ptr) assert(ptr != NULL)

#ifndef SN_
#endif

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
