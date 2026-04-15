#define _GNU_SOURCE

#include "sn_target.h"

#if SN_IS_OS(SN_OS_LINUX)
#include <errno.h>
#include <inttypes.h>
#include <limits.h>
#include <linux/mempolicy.h>
#include <pthread.h>
#include <sched.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/mman.h>
#include <sys/syscall.h>
#include <unistd.h>

typedef int (*sn_linux_id_visitor)(int value, void *context);

typedef struct sn_linux_counter_context {
        int count;
} sn_linux_counter_context;

typedef struct sn_linux_max_context {
        int max_value;
} sn_linux_max_context;

typedef struct sn_linux_fill_cpu_context {
        uint64_t *internal;
        uint32_t *global;
        int index;
} sn_linux_fill_cpu_context;

typedef struct sn_linux_fill_node_count_context {
        int *count_of_each_numa;
} sn_linux_fill_node_count_context;

static int sn_linux_read_first_line(char const *path, char *buffer, size_t buffer_size)
{
        FILE *file = fopen(path, "r");
        if (file == NULL)
                return 0;

        char *line = fgets(buffer, (int)buffer_size, file);
        fclose(file);
        return line != NULL;
}

static int sn_linux_parse_nonnegative_int(char const **cursor, int *value)
{
        char *end = NULL;
        uintmax_t parsed_value = 0;

        if (**cursor == '-')
                return 0;

        errno = 0;
        parsed_value = strtoumax(*cursor, &end, 10);
        if (end == *cursor || errno == ERANGE || parsed_value > (uintmax_t)INT_MAX)
                return 0;

        *cursor = end;
        *value = (int)parsed_value;
        return 1;
}

static int sn_linux_visit_id_list(char const *text, sn_linux_id_visitor visitor, void *context)
{
        char const *cursor = text;

        while (*cursor != '\0') {
                while (*cursor == ' ' || *cursor == '\t' || *cursor == '\n' || *cursor == ',')
                        cursor++;

                if (*cursor == '\0')
                        break;

                int begin = 0;
                if (!sn_linux_parse_nonnegative_int(&cursor, &begin))
                        return 0;

                int finish = begin;

                if (*cursor == '-') {
                        cursor++;
                        if (!sn_linux_parse_nonnegative_int(&cursor, &finish))
                                return 0;
                }

                if (finish < begin)
                        return 0;

                for (int value = begin;; value++) {
                        if (!visitor(value, context))
                                return 0;

                        if (value == finish)
                                break;
                }
        }

        return 1;
}

static int sn_linux_count_id(int value, void *context)
{
        (void)value;
        ((sn_linux_counter_context *)context)->count++;
        return 1;
}

static int sn_linux_track_max_id(int value, void *context)
{
        sn_linux_max_context *max_context = (sn_linux_max_context *)context;
        if (value > max_context->max_value)
                max_context->max_value = value;
        return 1;
}

static int sn_linux_fill_cpu_id(int value, void *context)
{
        sn_linux_fill_cpu_context *fill_context = (sn_linux_fill_cpu_context *)context;

        if (fill_context->internal != NULL)
                fill_context->internal[fill_context->index] = (uint64_t)fill_context->index;
        if (fill_context->global != NULL)
                fill_context->global[fill_context->index] = (uint32_t)value;

        fill_context->index++;
        return 1;
}

static int sn_linux_count_cpu_list(char const *path)
{
        char buffer[256];
        if (!sn_linux_read_first_line(path, buffer, sizeof(buffer)))
                return 0;

        sn_linux_counter_context counter = { 0 };
        if (!sn_linux_visit_id_list(buffer, sn_linux_count_id, &counter))
                return 0;

        return counter.count;
}

static int sn_linux_fill_node_cpu_count(int value, void *context)
{
        char path[128];
        snprintf(path, sizeof(path), "/sys/devices/system/node/node%d/cpulist", value);

        ((sn_linux_fill_node_count_context *)context)->count_of_each_numa[value] =
                sn_linux_count_cpu_list(path);
        return 1;
}

static int sn_linux_node_exists(int numa_id)
{
        char path[128];
        snprintf(path, sizeof(path), "/sys/devices/system/node/node%d", numa_id);
        return access(path, F_OK) == 0;
}

static size_t sn_linux_round_up_to_page_size(size_t size)
{
        int64_t page_size_value = (int64_t)sysconf(_SC_PAGESIZE);
        size_t effective_page_size = (page_size_value > 0) ? (size_t)page_size_value : 4096u;

        size_t rounded = size + effective_page_size - 1;
        if (rounded < size)
                return 0;

        return rounded - (rounded % effective_page_size);
}

static int sn_linux_bind_memory_to_node(void *address, size_t size, int numa_id, uint32_t flags)
{
#if defined(SYS_mbind)
        size_t bits_per_word = sizeof(unsigned long) * 8u;
        size_t word_count = ((size_t)numa_id / bits_per_word) + 1u;
        unsigned long *node_mask = (unsigned long *)calloc(word_count, sizeof(unsigned long));
        if (node_mask == NULL)
                return 0;

        node_mask[(size_t)numa_id / bits_per_word] = 1ul << ((size_t)numa_id % bits_per_word);

        int syscall_result = (int)syscall(SYS_mbind, address, size, MPOL_BIND | MPOL_F_STATIC_NODES,
                                          node_mask, (unsigned long)numa_id + 1ul, flags);
        free(node_mask);
        return syscall_result == 0;
#else
        (void)address;
        (void)size;
        (void)numa_id;
        (void)flags;
        return 0;
#endif
}

static int sn_linux_populate_writable_mapping(void *address, size_t size)
{
#if defined(MADV_POPULATE_WRITE)
        if (madvise(address, size, MADV_POPULATE_WRITE) == 0)
                return 1;

        return errno == EINVAL;
#else
        (void)address;
        (void)size;
        return 1;
#endif
}

/* Pin the calling thread to a single logical processor.
 * core_id >= 0 : bind to that logical processor (global index, across groups).
 * core_id <  0 : clear affinity constraint within the current group. */
SN_API void SN_DECL sn_set_thread_affinity(int core_id, int numa_id)
{
        int cpu_count = (int)sysconf(_SC_NPROCESSORS_ONLN);
        if (cpu_count <= 0)
                return;

        cpu_set_t *set = CPU_ALLOC(cpu_count);
        size_t size = CPU_ALLOC_SIZE(cpu_count);
        if (set == NULL)
                return;

        CPU_ZERO_S(size, set);

        if (core_id < 0) {
                for (int i = 0; i < cpu_count; i++)
                        CPU_SET_S((int)i, size, set);
        } else {
                CPU_SET_S(core_id, size, set);
        }

        pthread_setaffinity_np(pthread_self(), size, set);
        CPU_FREE(set);
}

SN_API int SN_DECL sn_get_numa_overall(int *out_numa_count, int *count_of_each_numa)
{
        char buffer[256];
        if (!sn_linux_read_first_line("/sys/devices/system/node/online", buffer, sizeof(buffer)))
                return 0;

        sn_linux_max_context max_context = { -1 };
        if (!sn_linux_visit_id_list(buffer, sn_linux_track_max_id, &max_context) ||
            max_context.max_value < 0)
                return 0;

        if (out_numa_count != NULL)
                *out_numa_count = max_context.max_value + 1;

        if (count_of_each_numa != NULL) {
                for (int index = 0; index <= max_context.max_value; index++)
                        count_of_each_numa[index] = 0;

                sn_linux_fill_node_count_context fill_context = { count_of_each_numa };
                if (!sn_linux_visit_id_list(buffer, sn_linux_fill_node_cpu_count, &fill_context))
                        return 0;
        }

        return 1;
}

SN_API int SN_DECL sn_get_numa_each(int numa_id, uint64_t *internal, uint32_t *global)
{
        if (numa_id < 0)
                return 0;

        char path[128];
        char buffer[256];
        snprintf(path, sizeof(path), "/sys/devices/system/node/node%d/cpulist", numa_id);
        if (!sn_linux_read_first_line(path, buffer, sizeof(buffer)))
                return 0;

        sn_linux_fill_cpu_context fill_context = { internal, global, 0 };
        return sn_linux_visit_id_list(buffer, sn_linux_fill_cpu_id, &fill_context);
}

SN_API void *SN_DECL sn_numa_alloc(size_t size, int numa_id)
{
        size_t allocation_size = sn_linux_round_up_to_page_size(size);
        if (allocation_size == 0)
                return NULL;

        int mmap_flags = MAP_PRIVATE | MAP_ANONYMOUS;
#if defined(MAP_POPULATE)
        if (numa_id >= 0)
                mmap_flags |= MAP_POPULATE;
#endif

        void *memory = mmap(NULL, allocation_size, PROT_READ | PROT_WRITE, mmap_flags, -1, 0);
        if (memory == MAP_FAILED)
                return NULL;

        if (numa_id < 0)
                return memory;

        uint32_t bind_flags = 0u;
#if defined(MAP_POPULATE)
        bind_flags = MPOL_MF_MOVE | MPOL_MF_STRICT;
#endif

        if (!sn_linux_node_exists(numa_id) ||
            !sn_linux_bind_memory_to_node(memory, allocation_size, numa_id, bind_flags) ||
            !sn_linux_populate_writable_mapping(memory, allocation_size)) {
                munmap(memory, allocation_size);
                return NULL;
        }

        return memory;
}

SN_API void SN_DECL sn_numa_free(void *ptr, size_t size, int numa_id)
{
        (void)numa_id;

        if (ptr == NULL || size == 0)
                return;

        size_t allocation_size = sn_linux_round_up_to_page_size(size);
        if (allocation_size != 0)
                munmap(ptr, allocation_size);
}
#endif
