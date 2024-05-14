//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Vec4d.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
using StgSharp.Controlling;
using StgSharp.Data;
using StgSharp.Math;

using System;
using System.Data.Common;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{
    public static class Vec4d
    {

        public static vec4d One
        {
            get => new vec4d(Vector4.One);
        }
        public static vec4d Zero
        {
            get => new vec4d(Vector4.Zero);
        }

    }

    [StructLayout(LayoutKind.Explicit, Size = 16, Pack = 16)]
    public struct vec4d : IEquatable<vec4d>, IVector
    {

        [FieldOffset(0)] internal unsafe fixed float num[4];

        [FieldOffset(0)] internal M128 reg;

        [FieldOffset(0)] internal Vector4 vec;
        [FieldOffset(12)] public float W;

        [FieldOffset(0)] public float X;
        [FieldOffset(4)] public float Y;
        [FieldOffset(8)] public float Z;

        internal vec4d(Vector4 v)
        {
            vec = v;
        }

        public vec4d(vec3d v, float w)
        {
            vec = v.vec;
            W = w;
        }

        public vec4d(float x, float y, float z, float w)
        {
            vec = new Vector4(x, y, z, w);
        }

        public unsafe vec3d XYZ
        {
            get => new vec3d(vec);
            set
            {
                float w = W;
                vec = value.vec;
                W = w;
            }
        }

        public override bool Equals(object obj)
        {
            return (obj is vec4d v) ? (v == this) : false;
        }

        public override int GetHashCode()
        {
            return vec.GetHashCode();
        }

        public override string ToString()
        {
            return vec.ToString();
        }

        public static bool operator !=(vec4d left, vec4d right)
        {
            return !(left == right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static vec4d operator *(Matrix44 mat, vec4d vec)
        {
            mat.InternalTranspose();
            return new vec4d(
                Vector4.Dot(mat.transpose.colum0, vec.vec),
                Vector4.Dot(mat.transpose.colum1, vec.vec),
                Vector4.Dot(mat.transpose.colum2, vec.vec),
                Vector4.Dot(mat.transpose.colum3, vec.vec)
                );
        }

        public static bool operator ==(vec4d left, vec4d right)
        {
            return left.Equals(right);
        }

        public static implicit operator M128(vec4d vec)
        {
            return vec.reg;
        }

        bool IEquatable<vec4d>.Equals(vec4d other)
        {
            return vec == other.vec;
        }

    }
}