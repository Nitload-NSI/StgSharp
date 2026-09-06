#ifndef NIF_NATIVE_H
#define NIF_NATIVE_H

#ifdef __cplusplus
#error "This header is not meant to be included in C++ code. Nitload.Native is a C library and should be used with C linkage."
#endif

#include "nif_internal.h"
#include "nif_target.h"

#include <immintrin.h>
#include <assert.h>
#include <glfw_function.h>

#define byte char
#define cs_char uint16_t
#define nif_null_assert(ptr) assert(ptr != NULL)

#ifndef NIF_
#endif

NIF_API void NIF_DECL nif_load_glfw_functions(GLFWFunctionTable *table);
NIF_API void NIF_DECL nif_load_intrinsic_function(void *intrinsic_context, uint64_t id);
NIF_API SIMDID NIF_DECL nif_get_simd_level_global();
/* Per-core variant: always re-executes CPUID, never caches.
 * Must be called on the target core (after affinity is set). */
NIF_API SIMDID NIF_DECL nif_get_simd_level_local();
NIF_API SIMDID NIF_DECL nif_get_unite_simd(SIMDID left, SIMDID right);
NIF_API int NIF_DECL nif_compare_simd(SIMDID left, SIMDID right);
NIF_API void NIF_DECL nif_set_thread_affinity(int core_id, int numa_id);
NIF_API void* NIF_DECL nif_numa_alloc(size_t size, int numa_id);
NIF_API void NIF_DECL nif_numa_free(void *ptr, size_t size, int numa_id);
NIF_API int NIF_DECL nif_get_numa_each(int numa_id, uint64_t *internal, uint32_t *global);
NIF_API int NIF_DECL nif_get_numa_overall(int *out_numa_count, int *count_of_each_numa);
#endif
