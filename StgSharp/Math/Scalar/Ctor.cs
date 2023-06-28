using System;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{
    public unsafe delegate float ScalarCalcHandler(float* x);

    public static unsafe partial class Calc
    {
        [DllImport("Kernel32.dll", EntryPoint = "VirtualAlloc")]
        private static extern IntPtr VirtualAlloc(IntPtr address, int size, uint allocType, uint protect);
        [DllImport("Kernel32.dll", EntryPoint = "VirtualFree")]
        private static extern bool VirtualFree(IntPtr address, int size, uint freeType);

        const uint MEM_COMMIT = 0x1000;
        const uint MEM_RESERVE = 0x2000;
        const uint PAGE_EXECUTE_READWRITE = 0x40;
        const uint MEM_RELEASE = 0x8000;

        static Calc()
        {
        }
    }
}
