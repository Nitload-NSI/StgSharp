using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Math.Linear
{
    [StructLayout(LayoutKind.Sequential)]
    public struct mat6 : IMat
    {
        internal Vector4 vec0;
        internal Vector4 vec1;
        internal Vector4 vec2;
        internal Vector4 vec3;
        internal Vector4 vec4;
        internal Vector4 vec5;
    }
}
