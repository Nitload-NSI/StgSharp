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
using StgSharp.Data;

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit, Size = 16, Pack = 16)]
    public struct vec3d : IEquatable<vec3d>, IVector
    {

        [FieldOffset(0)] internal unsafe fixed float num[3];
        [FieldOffset(0)]
        internal M128 reg;

        [FieldOffset(0)]
        internal Vector3 v;

        [FieldOffset(0)]
        internal Vector4 vec;

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

        public vec3d(vec2d xy, float z)
        {
            vec = xy.vec;
            Z = z;
        }

        public vec3d(
            float x, float y, float z)
        {
#if NET5_0_OR_GREATER
            Unsafe.SkipInit(out vec);
#endif
            v = new Vector3(x, y, z);
        }

        public vec3d XYZ
        {
            get { return new vec3d(v); }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Dot(vec3d right)
        {
            return Vector3.Dot(v, right.v);
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
        public static vec3d operator -(vec3d vec)
        {
            return new vec3d(-vec.vec);
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
            return new vec3d(left.vec + right.vec);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(vec3d left, vec3d right)
        {
            return left.v == right.v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe implicit operator vec3d((float, float, float) tuple)
        {
            vec3d v = *(vec3d*)&tuple;
            v.vec.W = 0.0f;
            return v;
        }

    }

    public static class Vec3d
    {

        public static vec3d One => new vec3d(1, 1, 1);
        public static vec3d UnitX => new vec3d(1, 0, 0);
        public static vec3d UnitY => new vec3d(0, 1, 0);

        public static vec3d UnitZ => new vec3d(0, 0, 1);
        public static vec3d Zero => new vec3d(0, 0, 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static vec3d Cross(vec3d left, vec3d right)
        {
            return Linear.Cross(left, right);
        }

        public static bool IsParallel(vec3d left, vec3d right)
        {
            return Cross(left, right) == Zero;
        }

    }
}
