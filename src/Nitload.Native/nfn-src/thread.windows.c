#include "nfn_target.h"
#include "NitloadFrameWork.h"

#if NFN_IS_OS(NFN_OS_WINDOWS)
#include <stdint.h>
#include <stdlib.h>
#include <string.h>
#include <windows.h>

static int nfn_windows_get_socket_map(uint32_t **out_socket_map, DWORD *out_group_count)
{
        DWORD group_count = GetActiveProcessorGroupCount();
        DWORD buffer_size = 0;
        uint32_t *socket_map;
        SYSTEM_LOGICAL_PROCESSOR_INFORMATION_EX *information;
        BYTE *cursor;
        uint32_t socket_id = 0;

        if (group_count == 0)
                return 0;

        socket_map = (uint32_t *)malloc((size_t)group_count * 64u * sizeof(uint32_t));
        if (socket_map == NULL)
                return 0;
        for (size_t i = 0; i < (size_t)group_count * 64u; i++)
                socket_map[i] = UINT32_MAX;

        GetLogicalProcessorInformationEx(RelationProcessorPackage, NULL, &buffer_size);
        if (GetLastError() != ERROR_INSUFFICIENT_BUFFER || buffer_size == 0) {
                free(socket_map);
                return 0;
        }

        information = (SYSTEM_LOGICAL_PROCESSOR_INFORMATION_EX *)malloc(buffer_size);
        if (information == NULL ||
            !GetLogicalProcessorInformationEx(RelationProcessorPackage, information, &buffer_size)) {
                free(information);
                free(socket_map);
                return 0;
        }

        cursor = (BYTE *)information;
        while (cursor < (BYTE *)information + buffer_size) {
                SYSTEM_LOGICAL_PROCESSOR_INFORMATION_EX *entry =
                        (SYSTEM_LOGICAL_PROCESSOR_INFORMATION_EX *)cursor;
                if (entry->Relationship == RelationProcessorPackage) {
                        PROCESSOR_RELATIONSHIP *package = &entry->Processor;
                        for (WORD i = 0; i < package->GroupCount; i++) {
                                WORD group = package->GroupMask[i].Group;
                                KAFFINITY mask = package->GroupMask[i].Mask;
                                if (group >= group_count)
                                        continue;
                                for (DWORD cpu = 0; cpu < 64; cpu++) {
                                        if ((mask & ((KAFFINITY)1 << cpu)) != 0)
                                                socket_map[(size_t)group * 64u + cpu] = socket_id;
                                }
                        }
                        socket_id++;
                }
                if (entry->Size == 0)
                        break;
                cursor += entry->Size;
        }

        free(information);
        *out_socket_map = socket_map;
        *out_group_count = group_count;
        return 1;
}

static int nfn_windows_get_processor_node(WORD group, BYTE cpu, USHORT *node_id)
{
        PROCESSOR_NUMBER processor;
        memset(&processor, 0, sizeof(processor));
        processor.Group = group;
        processor.Number = cpu;
        return GetNumaProcessorNodeEx(&processor, node_id) != 0;
}

/* Pin the calling thread using its processor group and group-local CPU index. */
NFN_API void NFN_DECL nfn_set_thread_affinity(int cpu_index, int processor_group_id)
{
        if (cpu_index < 0 || processor_group_id < 0) {
                GROUP_AFFINITY current;
                GetThreadGroupAffinity(GetCurrentThread(), &current);
                DWORD count = GetActiveProcessorCount(current.Group);
                GROUP_AFFINITY affinity;
                memset(&affinity, 0, sizeof(affinity));
                affinity.Group = current.Group;
                affinity.Mask = (count >= 64) ? ~(KAFFINITY)0 : ((KAFFINITY)1 << count) - 1;
                SetThreadGroupAffinity(GetCurrentThread(), &affinity, NULL);
                return;
        }

        if ((DWORD)processor_group_id >= GetActiveProcessorGroupCount() || cpu_index >= 64)
                return;
        USHORT node_id = 0;
        if (!nfn_windows_get_processor_node((WORD)processor_group_id, (BYTE)cpu_index, &node_id))
                return;

        GROUP_AFFINITY affinity;
        memset(&affinity, 0, sizeof(affinity));
        affinity.Group = (WORD)processor_group_id;
        affinity.Mask = (KAFFINITY)1 << cpu_index;
        SetThreadGroupAffinity(GetCurrentThread(), &affinity, NULL);
}

NFN_API int NFN_DECL nfn_get_numa_overall(int *out_numa_count, int *count_of_each_numa)
{
        ULONG highest;
        if (!GetNumaHighestNodeNumber(&highest))
                return 0;
        *out_numa_count = (int)highest + 1;

        if (count_of_each_numa != NULL) {
                for (ULONG n = 0; n <= highest; n++)
                        count_of_each_numa[n] = 0;
        }

        DWORD group_count = GetActiveProcessorGroupCount();
        for (DWORD group = 0; group < group_count; group++) {
                for (DWORD cpu = 0; cpu < 64; cpu++) {
                        USHORT node_id = 0;
                        if (nfn_windows_get_processor_node((WORD)group, (BYTE)cpu, &node_id) &&
                            count_of_each_numa != NULL && node_id <= highest)
                                count_of_each_numa[node_id]++;
                }
        }
        return 1;
}

NFN_API int NFN_DECL nfn_get_numa_each(int numa_id,
                                       uint32_t *processor_group_ids,
                                       uint32_t *socket_ids,
                                       uint32_t *cpu_indices,
                                       uint32_t *numa_node_ids)
{
        if (numa_id < 0 || processor_group_ids == NULL || socket_ids == NULL ||
            cpu_indices == NULL || numa_node_ids == NULL)
                return 0;

        uint32_t *socket_map = NULL;
        DWORD group_count = 0;
        if (!nfn_windows_get_socket_map(&socket_map, &group_count))
                return 0;

        int index = 0;
        for (DWORD group = 0; group < group_count; group++) {
                for (DWORD cpu = 0; cpu < 64; cpu++) {
                        USHORT node_id = 0;
                        if (!nfn_windows_get_processor_node((WORD)group, (BYTE)cpu, &node_id) ||
                            node_id != (USHORT)numa_id)
                                continue;

                        uint32_t socket_id = socket_map[(size_t)group * 64u + cpu];
                        if (socket_id == UINT32_MAX) {
                                free(socket_map);
                                return 0;
                        }
                        processor_group_ids[index] = (uint32_t)group;
                        socket_ids[index] = socket_id;
                        cpu_indices[index] = cpu;
                        numa_node_ids[index] = node_id;
                        index++;
                }
        }

        free(socket_map);
        return 1;
}


NFN_API void *NFN_DECL nfn_numa_alloc(size_t size, int numa_id)
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

NFN_API void NFN_DECL nfn_numa_free(void *ptr, size_t size, int numa_id)
{
        (void)size;
        (void)numa_id;

        if (ptr != NULL)
                VirtualFree(ptr, 0, MEM_RELEASE);
}

#endif
