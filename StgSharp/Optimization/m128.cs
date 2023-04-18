using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Optimization
{
    [StructLayout(LayoutKind.Explicit,Size =16)]
    public struct m128
    {
        [FieldOffset(0)] public float float0;
        [FieldOffset(4)] public float float1;
        [FieldOffset(8)] public float float2;
        [FieldOffset(12)] public float float3;
        
        [FieldOffset(0)] public double double0;
        [FieldOffset(8)] public double double1;

        [FieldOffset(0)] public int int0;
        [FieldOffset(1)] public int int1;
        [FieldOffset(2)] public int int2;
        [FieldOffset(3)] public int int3;

        [FieldOffset(0)] public byte byte0;
        [FieldOffset(1)] public byte byte1;
        [FieldOffset(2)] public byte byte2;
        [FieldOffset(3)] public byte byte3;
        [FieldOffset(4)] public byte byte4;
        [FieldOffset(5)] public byte byte5;
        [FieldOffset(6)] public byte byte6;
        [FieldOffset(7)] public byte byte7;
        [FieldOffset(8)] public byte byte8;
        [FieldOffset(9)] public byte byte9;
        [FieldOffset(10)] public byte byte10;
        [FieldOffset(11)] public byte byte11;
        [FieldOffset(12)] public byte byte12;
        [FieldOffset(13)] public byte byte13;
        [FieldOffset(14)] public byte byte14;
        [FieldOffset(15)] public byte byte15;
        [FieldOffset(16)] public byte byte16;


        public int[] ToIntArray()
        {
            return new int[4] { int0, int1, int2, int3 };
        }

        public float[] ToFloatArray()
        {
            return new float[4] { float0, float1, float2, float3 };
        }

        public byte[] ToByteArray()
        {
            return new byte[16] 
            { 
                byte0, byte1, byte2, byte3,
                byte4, byte5, byte6, byte7,
                byte8, byte9, byte10, byte11,
                byte12, byte13, byte14, byte15,
            };
        }
    }
}
