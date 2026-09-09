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

        private Buffer _buffer;

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

        public T this[
                 int index
        ]
        {
            get => _buffer[index];
            set => _buffer[index] = value;
        }

        public T X
        {
            get => _buffer[0];
            set => _buffer[0] = value;
        }

        public T Y
        {
            get => _buffer[1];
            set => _buffer[1] = value;
        }

        public T Z
        {
            get => _buffer[2];
            set => _buffer[2] = value;
        }

        public T W
        {
            get => _buffer[3];
            set => _buffer[3] = value;
        }

        public Vec2<T> XY
        {
            get => Unsafe.As<T, Vec2<T>>(ref _buffer[0]);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Vec3<T> XYZ
        {
            get => Unsafe.As<T, Vec3<T>>(ref _buffer[0]);
            set
            {
                T w = W;
                Unsafe.As<T, Vec3<T>>(ref _buffer[0]) = value;
                W = w;
            }
        }

        public Vec3<T> ZXY
        {
            readonly get
            {
                if (Unsafe.SizeOf<T>() == 4)
                {
                    Vector128<T> source = this.AsVector128Unsafe();
                    Vector128<int> bits = Unsafe.As<Vector128<T>, Vector128<int>>(ref source);
                    Vector128<int> result = Vector128.Shuffle(bits, Vector128.Create(2, 0, 1, 3));
                    Vector128<T> shuffled = Unsafe.As<Vector128<int>, Vector128<T>>(ref result);
                    return Vec3.FromVector128Unsafe(shuffled);
                }
                if (Unsafe.SizeOf<T>() == 8)
                {
                    Vector256<T> source = this.AsVector256Unsafe();
                    Vector256<long> bits = Unsafe.As<Vector256<T>, Vector256<long>>(ref source);
                    Vector256<long> result = Vector256.Shuffle(bits, Vector256.Create(2L, 0L, 1L,
                                                                                      3L));
                    Vector256<T> shuffled = Unsafe.As<Vector256<long>, Vector256<T>>(ref result);
                    return Vec3.FromVector256Unsafe(shuffled);
                }
                throw GraphicVector.ThrowUnsupportedTypeException<T>();
            }
            set
            {
                T w = W;
                if (Unsafe.SizeOf<T>() == 4)
                {
                    Vector128<T> source = value.AsVector128Unsafe();
                    Vector128<int> bits = Unsafe.As<Vector128<T>, Vector128<int>>(ref source);
                    Vector128<int> result = Vector128.Shuffle(bits, Vector128.Create(1, 2, 0, 3));
                    Vector128<T> shuffled = Unsafe.As<Vector128<int>, Vector128<T>>(ref result);
                    Unsafe.As<T, Vec3<T>>(ref _buffer[0]) = Vec3.FromVector128Unsafe(shuffled);
                } else if (Unsafe.SizeOf<T>() == 8)
                {
                    Vector256<T> source = value.AsVector256Unsafe();
                    Vector256<long> bits = Unsafe.As<Vector256<T>, Vector256<long>>(ref source);
                    Vector256<long> result = Vector256.Shuffle(bits, Vector256.Create(1L, 2L, 0L,
                                                                                      3L));
                    Vector256<T> shuffled = Unsafe.As<Vector256<long>, Vector256<T>>(ref result);
                    Unsafe.As<T, Vec3<T>>(ref _buffer[0]) = Vec3.FromVector256Unsafe(shuffled);
                } else
                {
                    throw GraphicVector.ThrowUnsupportedTypeException<T>();
                }
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
            return MemoryMarshal.CreateReadOnlySpan(ref _buffer[0], 4);
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
            } else if (Unsafe.SizeOf<T>() == 8)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T>.Enumerator GetEnumerator()
        {
            return AsSpan().GetEnumerator();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z, W);
        }

        public Vec4<T> Shuffle(
                       int t0,
                       int t1,
                       int t2,
                       int t3
        )
        {
            if (Unsafe.SizeOf<T>() == 4)
            {
                Vector128<T> v = this.AsVector128Unsafe();
                Vector128<int> bits = Unsafe.As<Vector128<T>, Vector128<int>>(ref v);
                Vector128<int> result = Vector128.Shuffle(bits, Vector128.Create(t0, t1, t2, t3));
                return Vec4.FromVector128Unsafe(Unsafe.As<Vector128<int>, Vector128<T>>(ref result));
            } else if (Unsafe.SizeOf<T>() == 8)
            {
                Vector256<T> v = this.AsVector256Unsafe();
                Vector256<long> bits = Unsafe.As<Vector256<T>, Vector256<long>>(ref v);
                Vector256<long> result = Vector256.Shuffle(bits, Vector256.Create(t0, t1, t2, t3));
                return Vec4.FromVector256Unsafe(Unsafe.As<Vector256<long>, Vector256<T>>(ref result));
            } else
            {
                throw GraphicVector.ThrowUnsupportedTypeException<T>();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec4<T> operator -(
                                       Vec4<T> left,
                                       Vec4<T> right
        )
        {
            if (Unsafe.SizeOf<T>() == 4)
            {
                return Vec4.FromVector128Unsafe(left.AsVector128Unsafe() - right.AsVector128Unsafe());
            } else if (Unsafe.SizeOf<T>() == 8) {
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
            if (Unsafe.SizeOf<T>() == 4)
            {
                return Vec4.FromVector128Unsafe(vector.AsVector128Unsafe() * Vector128.Create(scalar));
            } else if (Unsafe.SizeOf<T>() == 8) {
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

        public static Vec4<T> operator *(
                                       GMatrix44<T> mat,
                                       Vec4<T> vec
        )
        {
            return (mat[0] * vec.X) + (mat[1] * vec.Y) + (mat[2] * vec.Z) + (mat[3] * vec.W);
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
            if (Unsafe.SizeOf<T>() == 4)
            {
                return Vec4.FromVector128Unsafe(vector.AsVector128Unsafe() / Vector128.Create(scalar));
            } else if (Unsafe.SizeOf<T>() == 8) {
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
            if (Unsafe.SizeOf<T>() == 4)
            {
                return Vec4.FromVector128Unsafe(left.AsVector128Unsafe() + right.AsVector128Unsafe());
            } else if (Unsafe.SizeOf<T>() == 8) {
                return Vec4.FromVector256Unsafe(left.AsVector256Unsafe() + right.AsVector256Unsafe());
            }
            throw GraphicVector.ThrowUnsupportedTypeException<T>();
        }

        public static bool operator ==(
                                    Vec4<T> left,
                                    Vec4<T> right
        )
        {
            if (Unsafe.SizeOf<T>() == 4)
            {
                return left.AsVector128Unsafe() == right.AsVector128Unsafe();
            } else if (Unsafe.SizeOf<T>() == 8) {
                return left.AsVector256Unsafe() == right.AsVector256Unsafe();
            }
#pragma warning disable CA1065
            throw GraphicVector.ThrowUnsupportedTypeException<T>();
#pragma warning restore CA1065
        }

        [InlineArray(4)]
        private struct Buffer
        {

            private T _element;

        }

    }
}
