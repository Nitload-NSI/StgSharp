// dllmain.cpp : 定义 DLL 应用程序的入口点。

#ifdef _WIN32
#include "ssgc_framework.h"
#endif // __

#include "StgSharpC.h"

#ifdef _WIN32
BOOL APIENTRY DllMain(HMODULE hModule,
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