//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Vec2.cs"
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
using StgSharp.HighPerformance;

using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{
    /// <summary>
    ///   A two dimension vector defined by two elements. Vec2 in World are default used as colum
    ///   vector.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 16, Pack = 16)]
    public struct Vec2 : IFixedVector<Vec2>
    {

        [FieldOffset(0)] internal unsafe fixed float num[2];

        [FieldOffset(0)]
        internal M128 reg;

        [FieldOffset(0)]
        internal Vector2 v;

        [FieldOffset(0)]
        public float X;
        [FieldOffset(4)]
        public float Y;

        internal Vec2(Vector2 vec)
        {
            Unsafe.SkipInit(out reg);
            Unsafe.SkipInit(out X);
            Unsafe.SkipInit(out Y);
            v = vec;
        }

        internal Vec2(M128 vec)
        {
            Unsafe.SkipInit(out reg);
            Unsafe.SkipInit(out X);
            Unsafe.SkipInit(out Y);
            reg = vec;
            reg.Write<ulong>(1, 0);
        }

        public Vec2(float x, float y)
        {
            v = new Vector2(x, y);
        }

        public static Vec2 Unit => new Vec2(1, 1);

        public static Vec2 Zero => new Vec2(0, 0);

        public static Vec2 One
        {
            get => new Vec2(1, 1);
        }

        public Vec2 XY
        {
            get => this;
            set => this = value;
        }

        public Vec3 XYZ
        {
            get => new Vec3(reg);
            set
            {
                reg = value.reg;
                reg.Write<ulong>(1, 0);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Cross(Vec2 right)
        {
            return (X * right.Y) - (Y * right.X);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Dot(Vec2 vec)
        {
            return Vector2.Dot(v, vec.v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2 operator -(Vec2 left, Vec2 right)
        {
            return new Vec2(left.v - right.v);
        }

        public static bool operator !=(Vec2 left, Vec2 right)
        {
            return !(left == right);
        }

        public static Vec2 operator *(Vec2 vec, float value)
        {
            return new Vec2(vec.v * value);
        }
        public static Vec2 operator *(float value, Vec2 vec)
        {
            return new Vec2(vec.v * value);
        }

        public static Vec2 operator /(Vec2 vec, float value)
        {
            return new Vec2(vec.v / value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2 operator +(Vec2 left, Vec2 right)
        {
            return new Vec2(left.v + right.v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vec2 left, Vec2 right)
        {
            return left.v == right.v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vec2((float, float) tuple)
        {
            return new Vec2(tuple.Item1, tuple.Item2);
        }

    }
}
