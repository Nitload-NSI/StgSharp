// -----------------------------------------------------------------------------
// file="Vec2"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.HighPerformance.ProcessorAbstraction;
using StgSharp.Mathematics;
using StgSharp.Mathematics.Numeric;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Mathematics.Graphics
{
    /// <summary>
    ///   A two dimension vector defined by two elements. Vec2 in World are default used as colum
    ///   vector.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 16, Pack = 16)]
    public struct Vec2 : IUnmanagedVector<Vec2>
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

        internal Vec2(
                 Vector2 vec
        )
        {
            Unsafe.SkipInit(out reg);
            Unsafe.SkipInit(out X);
            Unsafe.SkipInit(out Y);
            v = vec;
        }

        internal Vec2(
                 M128 vec
        )
        {
            Unsafe.SkipInit(out reg);
            Unsafe.SkipInit(out X);
            Unsafe.SkipInit(out Y);
            reg = vec;
            reg.Member<ulong>(1) = 0;
        }

        public Vec2(
               float x,
               float y
        )
        {
            v = new Vector2(x, y);
        }

        public static Vec2 Unit => new Vec2(1, 1);

        public static Vec2 Zero => new Vec2(0, 0);

        public static Vec2 One => new Vec2(1, 1);

        public Vec2 XY
        {
            readonly get => this;
            set => this = value;
        }

        public Vec3 XYZ
        {
            readonly get => new(reg);
            set
            {
                reg = value.reg;
                reg.Member<ulong>(1) = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Cross(
                     Vec2 right
        )
        {
            return X * right.Y - Y * right.X;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Dot(
                     Vec2 vec
        )
        {
            return Vector2.Dot(v, vec.v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2 operator -(
                                    Vec2 left,
                                    Vec2 right
        )
        {
            return new Vec2(left.v - right.v);
        }

        public static bool operator !=(
                                    Vec2 left,
                                    Vec2 right
        )
        {
            return !(left == right);
        }

        public static Vec2 operator *(
                                    Vec2 vec,
                                    float value
        )
        {
            return new Vec2(vec.v * value);
        }
        public static Vec2 operator *(
                                    float value,
                                    Vec2 vec
        )
        {
            return new Vec2(vec.v * value);
        }

        public static Vec2 operator /(
                                    Vec2 vec,
                                    float value
        )
        {
            return new Vec2(vec.v / value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2 operator +(
                                    Vec2 left,
                                    Vec2 right
        )
        {
            return new Vec2(left.v + right.v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(
                                    Vec2 left,
                                    Vec2 right
        )
        {
            return left.v == right.v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vec2(
                                        (float, float) tuple
        )
        {
            return new Vec2(tuple.Item1, tuple.Item2);
        }

    }
}
