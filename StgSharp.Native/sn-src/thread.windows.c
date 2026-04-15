#include "sn_target.h"
#include "StgSharpNative.h"

#if SN_IS_OS(SN_OS_WINDOWS)

/* Pin the calling thread to a single logical processor.
 * core_id >= 0 : bind to that logical processor (global index, across groups).
 * core_id <  0 : clear affinity constraint within the current group. */
SN_API void SN_DECL sn_set_thread_affinity(int core_id, int numa_id)
{
        if (core_id < 0 || numa_id < 0) {
                /* Query which group the thread currently belongs to */
                GROUP_AFFINITY current;
                GetThreadGroupAffinity(GetCurrentThread(), &current);
                /* Build a full mask for that group's actual core count */
                DWORD count = GetActiveProcessorCount(current.Group);
                GROUP_AFFINITY ga;
                memset(&ga, 0, sizeof(ga));
                ga.Group = current.Group;
                ga.Mask = (count >= 64) ? ~(KAFFINITY)0 : ((KAFFINITY)1 << count) - 1;
                SetThreadGroupAffinity(GetCurrentThread(), &ga, NULL);
                return;
        }
        int index = core_id % 64;
        DWORD count = GetActiveProcessorCount(numa_id);
        if ((DWORD)index >= count)
                return;
        GROUP_AFFINITY ga;
        memset(&ga, 0, sizeof(ga));
        ga.Group = (WORD)numa_id;
        ga.Mask = (KAFFINITY)1 << index;
        SetThreadGroupAffinity(GetCurrentThread(), &ga, NULL);
}

SN_API int SN_DECL sn_get_numa_overall(int *out_numa_count, int *count_of_each_numa)
{
        ULONG highest;
        if (!GetNumaHighestNodeNumber(&highest))
                return 0;
        *out_numa_count = (int)highest + 1;

        for (UCHAR n = 0; n <= (UCHAR)highest; n++) {
                ULONGLONG mask = 0;
                GetNumaNodeProcessorMask(n, &mask);
                count_of_each_numa[n] = (int)__popcnt64(mask);
        }
        return 1;
}

SN_API int SN_DECL sn_get_numa_each(int numa_id, uint64_t *internal, uint32_t *global)
{
        ULONGLONG mask = 0;
        if (!GetNumaNodeProcessorMask((UCHAR)numa_id, &mask))
                return 0;

        int intra = 0;
        while (mask) {
                DWORD idx;
                _BitScanForward64(&idx, mask);
                if (internal != NULL)
                        internal[intra] = (uint64_t)intra;
                global[intra] = (uint32_t)idx;
                intra++;
                mask &= mask - 1;
        }
        return 1;
}


SN_API void *SN_DECL sn_numa_alloc(size_t size, int numa_id)
{
        if (size == 0)
                return NULL;

        if (numa_id < 0)
                return VirtualAlloc(NULL, size, MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);

        ULONG highest = 0;
        if (!GetNumaHighestNodeNumber(&highest) || (ULONG)numa_id > highest)
                return NULL;

        return VirtualAllocExNuma(GetCurrentProcess(), NULL, size, MEM_RESERVE | MEM_COMMIT,
                                  PAGE_READWRITE, (DWORD)numa_id);
}

SN_API void SN_DECL sn_numa_free(void *ptr, size_t size, int numa_id)
{
        (void)size;
        (void)numa_id;

        if (ptr != NULL)
                VirtualFree(ptr, 0, MEM_RELEASE);
}

#endif
