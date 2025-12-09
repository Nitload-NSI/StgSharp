#include "StgSharpNative.h"
#include "sn_matkernel.h"
#include <stdint.h>

#ifdef _WIN32

/* Define and initialize a global zero kernel aligned to 64 bytes for universal ISA usage */
#ifdef __cplusplus
extern "C" {
#endif
__declspec(align(64)) sn_zero_kernel_u64 SN_ZERO_KERNEL;
#ifdef __cplusplus
}
#endif

static void init_zero_kernel(void)
{
        for (int c = 0; c < 4; ++c) {
                for (int r = 0; r < 4; ++r) {
                        SN_ZERO_KERNEL.m[r][c] = 0;
                }
        }
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
        switch (ul_reason_for_call) {
        case DLL_PROCESS_ATTACH:
                init_zero_kernel();
                return 1;
        case DLL_THREAD_ATTACH:
                return 1;
        case DLL_THREAD_DETACH:
                return 1;
        case DLL_PROCESS_DETACH:
                return 1;
                break;
        }
        return TRUE;
}
#else
attribute((constructor)) void DllMain()
{
        init_zero_kernel();
}

#endif // _WIN32
