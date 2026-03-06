#include "sn_target.h"
#include "StgSharpNative.h"

#if SN_IS_OS(SN_OS_LINUX)
#define _GNU_SOURCE
#include <pthread.h>
#include <sched.h>
#include <unistd.h>
#endif

/* Pin the calling thread to a single logical processor.
 * core_id >= 0 : bind to that logical processor (global index, across groups).
 * core_id <  0 : clear affinity constraint within the current group. */
SN_API void SN_DECL sn_set_thread_affinity(int core_id)
{
#if SN_IS_OS(SN_OS_WINDOWS)
        if (core_id < 0) {
                /* Query which group the thread currently belongs to */
                GROUP_AFFINITY current;
                GetThreadGroupAffinity(GetCurrentThread(), &current);
                /* Build a full mask for that group's actual core count */
                DWORD count = GetActiveProcessorCount(current.Group);
                GROUP_AFFINITY ga;
                memset(&ga, 0, sizeof(ga));
                ga.Group = current.Group;
                ga.Mask  = (count >= 64) ? ~(KAFFINITY)0
                                         : ((KAFFINITY)1 << count) - 1;
                SetThreadGroupAffinity(GetCurrentThread(), &ga, NULL);
                return;
        }
        WORD group = (WORD)(core_id / 64);
        int  index = core_id % 64;
        DWORD count = GetActiveProcessorCount(group);
        if ((DWORD)index >= count)
                return;
        GROUP_AFFINITY ga;
        memset(&ga, 0, sizeof(ga));
        ga.Group = group;
        ga.Mask  = (KAFFINITY)1 << index;
        SetThreadGroupAffinity(GetCurrentThread(), &ga, NULL);

#elif SN_IS_OS(SN_OS_LINUX)
        cpu_set_t set;
        if (core_id < 0) {
                CPU_ZERO(&set);
                long ncpus = sysconf(_SC_NPROCESSORS_ONLN);
                for (long i = 0; i < ncpus; i++)
                        CPU_SET((int)i, &set);
        } else {
                CPU_ZERO(&set);
                CPU_SET(core_id, &set);
        }
        pthread_setaffinity_np(pthread_self(), sizeof(set), &set);
#endif
}
