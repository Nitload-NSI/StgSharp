using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Math.Linear
{
    [StructLayout(LayoutKind.Explicit, Size = 5 * 4 * sizeof(float))]
    public struct mat5 : IMat
    {
        [FieldOffset(0 * 4 * sizeof(float))] internal Vector4 vec0;
        [FieldOffset(1 * 4 * sizeof(float))] internal Vector4 vec1;
        [FieldOffset(2 * 4 * sizeof(float))] internal Vector4 vec2;
        [FieldOffset(3 * 4 * sizeof(float))] internal Vector4 vec3;
        [FieldOffset(4 * 4 * sizeof(float))] internal Vector4 vec4;
    }
}
