using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit, Size = 32, Pack = 32)]
    internal struct M256
    {
        [FieldOffset(0)] public float single_0;
        [FieldOffset(4)] public float single_1;
        [FieldOffset(8)] public float single_2;
        [FieldOffset(12)] public float single_3;
        [FieldOffset(16)] public float single_4;
        [FieldOffset(20)] public float single_5;
        [FieldOffset(24)] public float single_6;
        [FieldOffset(28)] public float single_7;

        [FieldOffset(0)] public long uint64_0;
        [FieldOffset(8)] public long uint64_1;
        [FieldOffset(16)] public long uint64_2;
        [FieldOffset(24)] public long uint64_3;

        [FieldOffset(0)] public double double_0;
        [FieldOffset(8)] public double double_1;
        [FieldOffset(16)] public double double_2;
        [FieldOffset(24)] public double double_3;

        [FieldOffset(0)] public IntPtr ptr_0;
        [FieldOffset(8)] public IntPtr ptr_1;
        [FieldOffset(16)] public IntPtr ptr_2;
        [FieldOffset(24)] public IntPtr ptr_3;
    }
}
