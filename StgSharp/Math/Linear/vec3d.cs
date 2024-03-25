//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="vec3d.cs"
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

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit, Size = 16, Pack = 16)]
    public struct vec3d : IEquatable<vec3d>
    {

        [FieldOffset(0)]
        internal Vector4 vec;
        [FieldOffset(0)]
        internal Vector3 v;

        [FieldOffset(0)] public float X;
        [FieldOffset(4)] public float Y;
        [FieldOffset(8)] public float Z;

        internal vec3d(Vector4 vector)
        {
            vec = vector;
        }

        internal vec3d(Vector3 vector)
        {
            v = vector;
        }

        public vec3d(
            float x, float y, float z)
        {
#if NET5_0_OR_GREATER
            Unsafe.SkipInit(out vec);
#endif
            v = new Vector3(x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public vec3d Cross(vec3d right)
        {
            return new vec3d(
                (this.Y * right.Z) - (this.Z * right.Y),
                (this.X * right.Z) - (this.Z * right.X),
                (this.X * right.Y) - (this.Y * right.X)
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Dot(vec3d right)
        {
            Vector4 vector4 = this.vec * right.vec;
            return vector4.X
                + vector4.Y
                + vector4.Z;
        }

        public bool Equals(vec3d other)
        {
            return this.vec == other.vec;
        }

        public override bool Equals(object obj)
        {
            if (obj is vec3d)
            {
                return this == (vec3d)obj;
            }
            else
            {
                return false;
            }
        }

        public override unsafe int GetHashCode()
        {
            fixed (float* add = &X)
            {
                return HashCode.Combine(X, Y, Z, (ulong)add);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetMaxValue()
        {
            float ret = X;
            ret = (ret > Y) ? ret : Y;
            ret = (ret > Z) ? ret : Z;
            return ret;
        }

        public override string ToString()
        {
            return $"[{X},{Y},{Z}]";
        }

        public string ToString(string sample)
        {
            return $"[{X.ToString(sample)},{Y.ToString(sample)},{Z.ToString(sample)}]";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public vec3d Zero()
        {
            return new vec3d(0, 0, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static vec3d operator -(vec3d left, vec3d right)
        {
            return new vec3d(left.v - right.v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(vec3d left, vec3d right)
        {
            return left.v != right.v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static vec3d operator *(vec3d vec, float value)
        {
            return new vec3d(vec.v * value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float operator *(vec3d left, vec3d right)
        {
            return Vector3.Dot(left.v, right.v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static vec3d operator /(vec3d vec, float value)
        {
            return new vec3d(vec.v / value);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static vec3d operator +(vec3d left, vec3d right)
        {
            return new vec3d(
                left.vec + right.vec
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(vec3d left, vec3d right)
        {
            return left.v == right.v;
        }

    }

    public static class Vec3d
    {

        public static vec3d Unit => new vec3d(1, 1, 1);
        public static vec3d Zero => new vec3d(0, 0, 0);

    }
}
