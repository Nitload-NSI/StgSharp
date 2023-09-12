// dllmain.cpp : 定义 DLL 应用程序的入口点。


#ifdef _WIN32
#include "framework.h"
#endif // __


#include "StgSharpGraphic.h"


#ifdef _WIN32
BOOL APIENTRY DllMain(HMODULE hModule,
    DWORD  ul_reason_for_call,
    LPVOID lpReserved
)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        do
        {
            defaultVecPtr = _aligned_malloc(sizeof(__m128),16);
        } while (defaultVecPtr == NULL);


    case DLL_THREAD_ATTACH:


    case DLL_THREAD_DETACH:


    case DLL_PROCESS_DETACH:



        break;
    }
    return TRUE;
}
#else 
attribute((constructor)) void DllMain()
{
    
}

#endif // _WIN32


__m128* defaultVecPtr = NULL;





