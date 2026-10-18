#ifndef NFN_NATIVE_H
#define NFN_NATIVE_H

#ifdef __cplusplus
#error "This header is not meant to be included in C++ code. Nitload.Native is a C library and should be used with C linkage."
#endif

#include "nfn_internal.h"
#include "nfn_target.h"

#include <immintrin.h>
#include <assert.h>
#include <glfw_function.h>

#define byte char
#define cs_char uint16_t
#define nfn_null_assert(ptr) assert(ptr != NULL)

#ifndef NFN_
#endif

NFN_API void NFN_DECL   nfn_load_glfw_functions(GLFWFunctionTable *table);
NFN_API void NFN_DECL   nfn_load_intrinsic_function(void *intrinsic_context, uint64_t id);
NFN_API SIMDID NFN_DECL nfn_get_simd_level_global();
/* Per-core variant: always re-executes CPUID, never caches.
 * Must be called on the target core (after affinity is set). */
NFN_API SIMDID NFN_DECL nfn_get_simd_level_local();
NFN_API SIMDID NFN_DECL nfn_get_unite_simd(SIMDID left, SIMDID right);
NFN_API int NFN_DECL    nfn_compare_simd(SIMDID left, SIMDID right);
NFN_API void NFN_DECL   nfn_set_thread_affinity(int cpu_index, int processor_group_id);
NFN_API void *NFN_DECL  nfn_numa_alloc(size_t size, int numa_id);
NFN_API void NFN_DECL   nfn_numa_free(void *ptr, size_t size, int numa_id);
NFN_API int NFN_DECL    nfn_get_numa_each(int numa_id, uint32_t *processor_group_ids,
                                          uint32_t *socket_ids, uint32_t *cpu_indices,
                                          uint32_t *numa_node_ids);
NFN_API int NFN_DECL    nfn_get_numa_overall(int *out_numa_count, int *count_of_each_numa);
#endif
