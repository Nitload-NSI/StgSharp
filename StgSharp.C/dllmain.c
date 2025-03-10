﻿
#include "StgSharpC.h"

#ifdef _WIN32

// clang-format off

BOOL APIENTRY DllMain(
        HMODULE hModule,
        DWORD  ul_reason_for_call,
        LPVOID lpReserved
)
{
        switch (ul_reason_for_call)
        {
        case DLL_PROCESS_ATTACH:
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
}

#endif // _WIN32

// clang-format on
