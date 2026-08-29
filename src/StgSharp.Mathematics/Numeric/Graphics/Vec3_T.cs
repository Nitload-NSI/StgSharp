// -----------------------------------------------------------------------------
// file="Vec3_T"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics.Numeric;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace StgSharp.Mathematics.Numeric.Graphics
{
    /// <summary>
    ///   A tightly packed three-dimensional numeric vector.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 16)]
    public struct Vec3<T> : IUnmanagedVector<Vec3<T>>, IEquatable<Vec3<T>>, IEnumerable<T>
        where T: unmanaged, INumber<T>
    {

        public T X;
        public T Y;
        public T Z;

        public Vec3(Vec2<T> xy, T z)
        {
            X = xy.X;
            Y = xy.Y;
            Z = z;
        }

        public Vec3(T x, T y, T z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vec2<T> XY
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => new(X, Y);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Vec3<T> XYZ
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => this;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this = value;
        }

        public static Vec3<T> Zero => new(T.Zero, T.Zero, T.Zero);

        public static Vec3<T> One => new(T.One, T.One, T.One);

        public static Vec3<T> UnitX => new(T.One, T.Zero, T.Zero);

        public static Vec3<T> UnitY => new(T.Zero, T.One, T.Zero);

        public static Vec3<T> UnitZ => new(T.Zero, T.Zero, T.One);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T Dot(Vec3<T> right)
        {
            if (Unsafe.SizeOf<T>() == 8)
            {
                Vec4<T> v = Vec4.FromVector256Unsafe(this.AsVector256Unsafe() * right.AsVector256Unsafe());
                return v.X + v.Y + v.Z;
            }
            if (Unsafe.SizeOf<T>() == 4)
            {
                Vec4<T> v = Vec4.FromVector128Unsafe(this.AsVector128Unsafe() * right.AsVector128Unsafe());
                return v.X + v.Y + v.Z;
            }
            throw GraphicVector.ThrowUnsupportedTypeException<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Vec3<T> other)
        {
            if (Unsafe.SizeOf<T>() == 4) {
                return this.AsVector128Safe() == other.AsVector128Safe();
            }
            if (Unsafe.SizeOf<T>() == 8) {
                return this.AsVector256Safe() == other.AsVector256Safe();
            }
            throw GraphicVector.ThrowUnsupportedTypeException<T>();
        }

        public override readonly bool Equals(object? obj)
        {
            return obj is Vec3<T> other && Equals(other);
        }

        public static Vec3<T> FromSpan(ReadOnlySpan<T> span)
        {
            if (span.Length < 3) {
                throw new ArgumentException("Span length must be at least 3.", nameof(span));
            }

            return new(span[0], span[1], span[2]);
        }

        public readonly IEnumerator<T> GetEnumerator()
        {
            yield return X;
            yield return Y;
            yield return Z;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T GetMaxValue()
        {
            return T.Max(X, T.Max(Y, Z));
        }

        public static bool IsParallel(Vec3<T> left, Vec3<T> right)
        {
            return Vec3.Cross(left, right) == Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3<T> operator -(Vec3<T> value)
        {
            if (Unsafe.SizeOf<T>() == 4) {
                return Vec3.FromVector128Unsafe(Vector128<T>.Zero - value.AsVector128Unsafe());
            }
            if (Unsafe.SizeOf<T>() == 8) {
                return Vec3.FromVector256Unsafe(Vector256<T>.Zero - value.AsVector256Unsafe());
            }
            throw GraphicVector.ThrowUnsupportedTypeException<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3<T> operator -(Vec3<T> left, Vec3<T> right)
        {
            if (Unsafe.SizeOf<T>() == 4)
            {
                Vector128<T> left_v128 = left.AsVector128Unsafe();
                Vector128<T> right_v128 = right.AsVector128Unsafe();
                return Vec3.FromVector128Unsafe(left_v128 - right_v128);
            }
            if (Unsafe.SizeOf<T>() == 8)
            {
                Vector256<T> left_v256 = left.AsVector256Unsafe();
                Vector256<T> right_v256 = right.AsVector256Unsafe();
                return Vec3.FromVector256Unsafe(left_v256 - right_v256);
            }
            throw GraphicVector.ThrowUnsupportedTypeException<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vec3<T> left, Vec3<T> right)
        {
            return !left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3<T> operator *(Vec3<T> vector, T scalar)
        {
            if (Unsafe.SizeOf<T>() == 4)
            {
                Vector128<T> vector_v128 = vector.AsVector128Unsafe();
                Vector128<T> scalar_v128 = Vector128.Create(scalar);
                return Vec3.FromVector128Unsafe(vector_v128 * scalar_v128);
            }
            if (Unsafe.SizeOf<T>() == 8)
            {
                Vector256<T> vector_v256 = vector.AsVector256Unsafe();
                Vector256<T> scalar_v256 = Vector256.Create(scalar);
                return Vec3.FromVector256Unsafe(vector_v256 * scalar_v256);
            }
            throw GraphicVector.ThrowUnsupportedTypeException<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3<T> operator *(T scalar, Vec3<T> vector)
        {
            return vector * scalar;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T operator *(Vec3<T> left, Vec3<T> right)
        {
            return left.Dot(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3<T> operator /(Vec3<T> vector, T scalar)
        {
            if (Unsafe.SizeOf<T>() == 4)
            {
                Vector128<T> vector_v128 = vector.AsVector128Unsafe();
                Vector128<T> scalar_v128 = Vector128.Create(scalar);
                return Vec3.FromVector128Unsafe(vector_v128 / scalar_v128);
            }
            if (Unsafe.SizeOf<T>() == 8)
            {
                Vector256<T> vector_v256 = vector.AsVector256Unsafe();
                Vector256<T> scalar_v256 = Vector256.Create(scalar);
                return Vec3.FromVector256Unsafe(vector_v256 / scalar_v256);
            }
            throw GraphicVector.ThrowUnsupportedTypeException<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3<T> operator +(Vec3<T> left, Vec3<T> right)
        {
            if (Unsafe.SizeOf<T>() == 4)
            {
                Vector128<T> left_v128 = left.AsVector128Unsafe();
                Vector128<T> right_v128 = right.AsVector128Unsafe();
                return Vec3.FromVector128Unsafe(left_v128 + right_v128);
            }
            if (Unsafe.SizeOf<T>() == 8)
            {
                Vector256<T> left_v256 = left.AsVector256Unsafe();
                Vector256<T> right_v256 = right.AsVector256Unsafe();
                return Vec3.FromVector256Unsafe(left_v256 + right_v256);
            }
            throw GraphicVector.ThrowUnsupportedTypeException<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vec3<T> left, Vec3<T> right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vec3<T>((T X, T Y, T Z) tuple)
        {
            return new(tuple.X, tuple.Y, tuple.Z);
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
