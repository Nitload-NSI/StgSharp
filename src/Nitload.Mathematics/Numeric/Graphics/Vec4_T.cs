// -----------------------------------------------------------------------------
// file="Vec4_T"
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
    ///   A four-dimensional numeric vector.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 16)]
    public struct Vec4<T> : IUnmanagedVector<Vec4<T>> where T : unmanaged, INumber<T>
    {

        public T W;

        public T X;
        public T Y;
        public T Z;

        public Vec4(
               Vec3<T> v3,
               T w
        )
        {
            X = v3.X;
            Y = v3.Y;
            Z = v3.Z;
            W = w;
        }

        public Vec4(
               T x,
               T y,
               T z,
               T w
        )
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vec2<T> XY
        {
            get => Unsafe.As<T, Vec2<T>>(ref X);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Vec3<T> XYZ
        {
            get => Unsafe.As<T, Vec3<T>>(ref X);
            set
            {
                T w = W;
                Unsafe.As<T, Vec3<T>>(ref X) = value;
                W = w;
            }
        }

        public static Vec4<T> One => new(T.One, T.One, T.One, T.One);

        public static Vec4<T> Zero => new(T.Zero, T.Zero, T.Zero, T.Zero);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec4<T> Add(
                              Vec4<T> left,
                              Vec4<T> right
        )
        {
            return left + right;
        }

        public ReadOnlySpan<T> AsSpan()
        {
            return MemoryMarshal.CreateReadOnlySpan(ref X, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T Dot(
                          Vec4<T> right
        )
        {
            if (Unsafe.SizeOf<T>() == 4)
            {
                Vec4<T> product = Vec4.FromVector128Unsafe(this.AsVector128Unsafe() * right.AsVector128Unsafe());
                return product.X + product.Y + product.Z + product.W;
            }
            if (Unsafe.SizeOf<T>() == 8)
            {
                Vec4<T> product = Vec4.FromVector256Unsafe(this.AsVector256Unsafe() * right.AsVector256Unsafe());
                return product.X + product.Y + product.Z + product.W;
            }
            throw GraphicVector.ThrowUnsupportedTypeException<T>();
        }

        public override bool Equals(
                             object? obj
        )
        {
            return (obj is Vec4<T> v) && (v == this);
        }

        public static Vec4<T> FromSpan(
                              ReadOnlySpan<T> span
        )
        {
            if (span.Length < 4) {
                throw new ArgumentException("Span length must be at least 4.", nameof(span));
            }

            Unsafe.SkipInit(out Vec4<T> result);
            Span<T> target = MemoryMarshal.CreateSpan(ref result.X, 4);
            span[..4].CopyTo(target);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T>.Enumerator GetEnumerator()
        {
            return AsSpan().GetEnumerator();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z, W);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec4<T> operator -(
                                       Vec4<T> left,
                                       Vec4<T> right
        )
        {
            if (Unsafe.SizeOf<T>() == 4) {
                return Vec4.FromVector128Unsafe(left.AsVector128Unsafe() - right.AsVector128Unsafe());
            }
            if (Unsafe.SizeOf<T>() == 8) {
                return Vec4.FromVector256Unsafe(left.AsVector256Unsafe() - right.AsVector256Unsafe());
            }
            throw GraphicVector.ThrowUnsupportedTypeException<T>();
        }

        public static bool operator !=(
                                    Vec4<T> left,
                                    Vec4<T> right
        )
        {
            return !(left == right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec4<T> operator *(
                                       Vec4<T> vector,
                                       T scalar
        )
        {
            if (Unsafe.SizeOf<T>() == 4) {
                return Vec4.FromVector128Unsafe(vector.AsVector128Unsafe() * Vector128.Create(scalar));
            }
            if (Unsafe.SizeOf<T>() == 8) {
                return Vec4.FromVector256Unsafe(vector.AsVector256Unsafe() * Vector256.Create(scalar));
            }
            throw GraphicVector.ThrowUnsupportedTypeException<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec4<T> operator *(
                                       T scalar,
                                       Vec4<T> vector
        )
        {
            return vector * scalar;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T operator *(
                                 Vec4<T> left,
                                 Vec4<T> right
        )
        {
            return left.Dot(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec4<T> operator /(
                                       Vec4<T> vector,
                                       T scalar
        )
        {
            if (Unsafe.SizeOf<T>() == 4) {
                return Vec4.FromVector128Unsafe(vector.AsVector128Unsafe() / Vector128.Create(scalar));
            }
            if (Unsafe.SizeOf<T>() == 8) {
                return Vec4.FromVector256Unsafe(vector.AsVector256Unsafe() / Vector256.Create(scalar));
            }
            throw GraphicVector.ThrowUnsupportedTypeException<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec4<T> operator +(
                                       Vec4<T> left,
                                       Vec4<T> right
        )
        {
            if (Unsafe.SizeOf<T>() == 4) {
                return Vec4.FromVector128Unsafe(left.AsVector128Unsafe() + right.AsVector128Unsafe());
            }
            if (Unsafe.SizeOf<T>() == 8) {
                return Vec4.FromVector256Unsafe(left.AsVector256Unsafe() + right.AsVector256Unsafe());
            }
            throw GraphicVector.ThrowUnsupportedTypeException<T>();
        }

        public static bool operator ==(
                                    Vec4<T> left,
                                    Vec4<T> right
        )
        {
            if (Unsafe.SizeOf<T>() == 4) {
                return left.AsVector128Unsafe() == right.AsVector128Unsafe();
            }
            if (Unsafe.SizeOf<T>() == 8) {
                return left.AsVector256Unsafe() == right.AsVector256Unsafe();
            }
#pragma warning disable CA1065
            throw GraphicVector.ThrowUnsupportedTypeException<T>();
#pragma warning restore CA1065
        }

    }
}
