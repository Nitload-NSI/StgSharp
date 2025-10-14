//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Vec3"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.HighPerformance;
using StgSharp.Mathematics.Numeric;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Mathematics.Graphic
{
    [CollectionBuilder(typeof(Vec3), nameof(FromSpan))]
    [StructLayout(LayoutKind.Explicit, Size = 16, Pack = 16)]
    public struct Vec3 : IEquatable<Vec3>, IUnmanagedVector<Vec3>, IEnumerable<float>
    {

        [FieldOffset(0)] internal unsafe fixed float num[3];
        [FieldOffset(0)] internal M128 reg;

        [FieldOffset(0)] internal Vector3 v;

        [FieldOffset(0)] public float X;
        [FieldOffset(4)] public float Y;
        [FieldOffset(8)] public float Z;

        internal Vec3(M128 vector)
        {
            Unsafe.SkipInit(out this);
            reg = vector;
            reg.Member<uint>(3) = 0;
        }

        internal Vec3(Vector3 vector)
        {
            v = vector;
        }

        public Vec3()
        {
            Unsafe.SkipInit(out this);
        }

        public Vec3(Vec2 xy, float z)
        {
            Unsafe.SkipInit(out this);
            reg = xy.reg;
            Z = z;
        }

        public Vec3(float x, float y, float z)
        {
            Unsafe.SkipInit(out this);
            v = new Vector3(x, y, z);
        }

        public unsafe Vec2 XY
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => new(reg);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Unsafe.As<Vector3, Vector2>(ref v) = value.v;
        }

        public Vec3 XYZ
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => new(v);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this = value;
        }

        public static Vec3 Zero => new(0, 0, 0);

        public static Vec3 One => new(1, 1, 1);

        public static Vec3 UnitX => new(1, 0, 0);

        public static Vec3 UnitY => new(0, 1, 0);

        public static Vec3 UnitZ => new(0, 0, 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 Cross(Vec3 left, Vec3 right)
        {
            return Linear.Cross(left, right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float Dot(Vec3 right)
        {
            return Vector3.Dot(v, right.v);
        }

        public readonly bool Equals(Vec3 other)
        {
            return reg == other.reg;
        }

        public override readonly bool Equals(object obj)
        {
            return obj is not null && obj is Vec3 vec && this == vec;
        }

        public static Vec3 FromSpan(ReadOnlySpan<float> span)
        {
            Vec3 v = new();
            span.CopyTo(MemoryMarshal.CreateSpan(ref v.X, 3));
            return v;
        }

        public unsafe IEnumerator<float> GetEnumerator()
        {
            for (int i = 0; i < 3; i++) {
                yield return reg.Member<float>(i);
            }
        }

        public override unsafe int GetHashCode()
        {
            fixed (float* add = &X) {
                return HashCode.Combine(X, Y, Z, (ulong)add);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float GetMaxValue()
        {
            float ret = X;
            ret = ret > Y ? ret : Y;
            ret = ret > Z ? ret : Z;
            return ret;
        }

        public static bool IsParallel(Vec3 left, Vec3 right)
        {
            return Cross(left, right) == Zero;
        }

        public override readonly string ToString()
        {
            return $"[{X},{Y},{Z}]";
        }

        public readonly string ToString(string sample)
        {
            return $"[{X.ToString(sample)},{Y.ToString(sample)},{Z.ToString(sample)}]";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator -(Vec3 vec)
        {
            return new Vec3(-vec.v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator -(Vec3 left, Vec3 right)
        {
            return new Vec3(left.v - right.v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vec3 left, Vec3 right)
        {
            return left.v != right.v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator *(Vec3 vec, float value)
        {
            return new Vec3(vec.v * value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float operator *(Vec3 left, Vec3 right)
        {
            return Vector3.Dot(left.v, right.v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator /(Vec3 vec, float value)
        {
            return new Vec3(vec.v / value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 operator +(Vec3 left, Vec3 right)
        {
            return new Vec3(left.v + right.v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vec3 left, Vec3 right)
        {
            return left.v == right.v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe implicit operator Vec3((float, float, float) tuple)
        {
            return Unsafe.As<(float,float,float), Vec3>(ref tuple);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
