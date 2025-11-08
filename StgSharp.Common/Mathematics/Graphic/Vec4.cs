//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Vec4"
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
using StgSharp.Internal.Intrinsic;
using StgSharp.Mathematics.Numeric;
using System;
using System.Data.Common;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Mathematics.Graphic
{
    [CollectionBuilder(builderType:typeof(Vec4), methodName: nameof(Vec4.FromSpan))]
    [StructLayout(LayoutKind.Explicit, Size = 16, Pack = 16)]
    public unsafe struct Vec4 : IEquatable<Vec4>, IUnmanagedVector<Vec4, float>
    {

        [FieldOffset(0)] internal unsafe fixed float num[4];

        [FieldOffset(0)] internal M128 reg;

        [FieldOffset(0)] internal Vector4 vec;
        [FieldOffset(12)] public float W;

        [FieldOffset(0)] public float X;
        [FieldOffset(4)] public float Y;
        [FieldOffset(8)] public float Z;

        internal Vec4(Vector4 v)
        {
            Unsafe.SkipInit(out X);
            Unsafe.SkipInit(out Y);
            Unsafe.SkipInit(out Z);
            Unsafe.SkipInit(out W);
            Unsafe.SkipInit(out reg);

            vec = v;
        }

        internal Vec4(M128 v)
        {
            Unsafe.SkipInit(out X);
            Unsafe.SkipInit(out Y);
            Unsafe.SkipInit(out Z);
            Unsafe.SkipInit(out W);
            Unsafe.SkipInit(out vec);

            reg = v;
        }

        public Vec4(Vec3 v3, float w)
        {
            Unsafe.SkipInit(out X);
            Unsafe.SkipInit(out Y);
            Unsafe.SkipInit(out Z);
            Unsafe.SkipInit(out W);
            reg = v3.reg;
            W = w;
        }

        public Vec4(float x, float y, float z, float w)
        {
            vec = new Vector4(x, y, z, w);
        }

        public unsafe Vec2 XY
        {
            get => new Vec2(reg);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public unsafe Vec3 XYZ
        {
            get => new Vec3(reg);
            set
            {
                float w = W;
                reg = value.reg;
                W = w;
            }
        }

        public static Vec4 One
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Vec4(1, 1, 1, 1);
        }

        public static Vec4 Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Vec4(0, 0, 0, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec4 Add(Vec4 left, Vec4 right)
        {
            return left + right;
        }

        public ReadOnlySpan<float> AsSpan()
        {
            return MemoryMarshal.CreateReadOnlySpan(ref reg.Member<float>(0), 4);
        }

        public override bool Equals(object obj)
        {
            return obj is Vec4 v && v == this;
        }

        public static Vec4 FromSpan(ReadOnlySpan<float> span)
        {
            return new Vec4
            {
                X = span[0],
                Y = span[1],
                Z = span[2],
                W = span[3]
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<float>.Enumerator GetEnumerator()
        {
            return AsSpan().GetEnumerator();
        }

        public override int GetHashCode()
        {
            return vec.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec4 Subtract(Vec4 left, Vec4 right)
        {
            return left - right;
        }

        public override string ToString()
        {
            return vec.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec4 operator -(Vec4 left, Vec4 right)
        {
            return new Vec4(left.vec - right.vec);
        }

        public static bool operator !=(Vec4 left, Vec4 right)
        {
            return !(left == right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec4 operator *(Matrix44 mat, Vec4 vec)
        {
            mat.InternalTranspose();
            return new Vec4(Vector4.Dot(mat.transpose.colum0, vec.vec), Vector4.Dot(mat.transpose.colum1, vec.vec),
                            Vector4.Dot(mat.transpose.colum2, vec.vec), Vector4.Dot(mat.transpose.colum3, vec.vec));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec4 operator +(Vec4 left, Vec4 right)
        {
            return new Vec4(left.vec + right.vec);
        }

        public static bool operator ==(Vec4 left, Vec4 right)
        {
            return left.vec == right.vec;
        }

        bool IEquatable<Vec4>.Equals(Vec4 other)
        {
            return vec == other.vec;
        }

    }
}