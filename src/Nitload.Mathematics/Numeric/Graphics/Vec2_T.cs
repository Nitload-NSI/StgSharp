// -----------------------------------------------------------------------------
// file="Vec2_T"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Mathematics.Numeric;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace Nitload.Mathematics.Numeric.Graphics
{
    /// <summary>
    ///   A two dimension vector defined by two elements. Vec2 in World are default used as colum
    ///   vector.
    /// </summary>
    /// <typeparam name="T">
    ///
    /// </typeparam>
    [StructLayout(LayoutKind.Sequential, Pack = 16)]
    public struct Vec2<T>(T x, T y) : IUnmanagedVector<Vec2<T>> where T: unmanaged, INumber<T>
    {

        public T X = x;
        public T Y = y;

        public static Vec2<T> Unit => new(T.One, T.One);

        public static Vec2<T> Zero => new(T.Zero, T.Zero);

        public static Vec2<T> One => new(T.One, T.One);

        public Vec2<T> XY
        {
            readonly get => this;
            set => this = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Cross(Vec2<T> right)
        {
            return (X * right.Y) - (Y * right.X);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T Dot(Vec2<T> vec)
        {
            Vector128<T> leftVec = this.AsVector128Unsafe();
            Vector128<T> rightVec = vec.AsVector128Unsafe();
            Vector128<T> result_v128 = leftVec * rightVec;
            Vec2<T> result = Unsafe.As<Vector128<T>, Vec2<T>>(ref result_v128);
            return result.X + result.Y;
        }

        public override readonly bool Equals(object? obj)
        {
            return (obj is Vec2<T> other) && (other == this);
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2<T> operator -(Vec2<T> left, Vec2<T> right)
        {
            Vector128<T> leftVec = left.AsVector128Unsafe();
            Vector128<T> rightVec = right.AsVector128Unsafe();
            return Vec2.FromVector128Unsafe(leftVec - rightVec);
        }

        public static bool operator !=(Vec2<T> left, Vec2<T> right)
        {
            return !(left == right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2<T> operator *(Vec2<T> vec, T value)
        {
            Vector128<T> leftVec = vec.AsVector128Unsafe();
            return Vec2.FromVector128Unsafe(leftVec * value);
        }

        public static Vec2<T> operator *(T value, Vec2<T> vec)
        {
            Vector128<T> leftVec = vec.AsVector128Unsafe();
            return Vec2.FromVector128Unsafe(leftVec * value);
        }

        public static Vec2<T> operator /(Vec2<T> vec, T value)
        {
            Vector128<T> leftVec = vec.AsVector128Unsafe();
            return Vec2.FromVector128Unsafe(leftVec / value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2<T> operator +(Vec2<T> left, Vec2<T> right)
        {
            Vector128<T> leftVec = left.AsVector128Unsafe();
            Vector128<T> rightVec = right.AsVector128Unsafe();
            return Vec2.FromVector128Unsafe(leftVec + rightVec);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vec2<T> left, Vec2<T> right)
        {
            if (Unsafe.SizeOf<T>() == 4) {
                return Unsafe.As<Vec2<T>, ulong>(ref left) == Unsafe.As<Vec2<T>, ulong>(ref right);
            }
            if (Unsafe.SizeOf<T>() == 8) {
                return Unsafe.As<Vec2<T>, Vector128<T>>(ref left) == Unsafe.As<Vec2<T>, Vector128<T>>(ref right);
            }
#pragma warning disable CA1065
            throw new NotSupportedException($"Element type {typeof(T).Name} is not supported.");
#pragma warning restore CA1065
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vec2<T>((T, T) tuple)
        {
            return new Vec2<T>(tuple.Item1, tuple.Item2);
        }

    }
}
