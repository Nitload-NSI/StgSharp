using StgSharp.Controlling;
using StgSharp.Math;
using System.Numerics;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{

    [StructLayout(LayoutKind.Explicit, Size = 16, Pack = 16)]
    public struct Vec4d
    {
        [FieldOffset(0)] internal Vector4 vec;

        [FieldOffset(0)]public float X;
        [FieldOffset(4)]public float Y;
        [FieldOffset(8)]public float Z;
        [FieldOffset(12)]public float W;

        unsafe public Vec4d(
            float x, float y, float z, float w)
        {
            X = x; Y = y; Z = z; W = w;
        }
    }
}
