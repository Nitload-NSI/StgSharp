// -----------------------------------------------------------------------------
// file="Vec3"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Nitload.Mathematics.Numeric.Graphics
{
    public static class Vec3
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
<<<<<<< HEAD
        public static Vec3<double> ConvertToDouble(Vec3<long> source)
=======
        public static Vec3<double> ConvertToDouble(
                                   Vec3<long> source
        )
>>>>>>> 8755185e78a8091ca252c1342249110180446be3
        {
            return FromVector256Unsafe(Vector256.ConvertToDouble(source.AsVector256Safe()));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
<<<<<<< HEAD
        public static Vec3<double> ConvertToDouble(Vec3<ulong> source)
=======
        public static Vec3<double> ConvertToDouble(
                                   Vec3<ulong> source
        )
>>>>>>> 8755185e78a8091ca252c1342249110180446be3
        {
            return FromVector256Unsafe(Vector256.ConvertToDouble(source.AsVector256Safe()));
        }

<<<<<<< HEAD
        #region cast

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3<float> ConvertToSingle(Vec3<int> source)
=======
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3<float> ConvertToSingle(
                                  Vec3<int> source
        )
>>>>>>> 8755185e78a8091ca252c1342249110180446be3
        {
            return FromVector128Unsafe(Vector128.ConvertToSingle(source.AsVector128Safe()));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
<<<<<<< HEAD
        public static Vec3<float> ConvertToSingle(Vec3<uint> source)
=======
        public static Vec3<float> ConvertToSingle(
                                  Vec3<uint> source
        )
>>>>>>> 8755185e78a8091ca252c1342249110180446be3
        {
            return FromVector128Unsafe(Vector128.ConvertToSingle(source.AsVector128Safe()));
        }

<<<<<<< HEAD
        public static bool IsParallel<T>(Vec3<T> left, Vec3<T> right) where T: unmanaged,INumber<T>
=======
        public static Vec3<T> FromSpan<T>(
                              ReadOnlySpan<T> span
        ) where T : unmanaged, INumber<T>
        {
            if (span.Length < 3) {
                throw new ArgumentException("Span length must be at least 3.", nameof(span));
            }

            return new(span[0], span[1], span[2]);
        }

        public static bool IsParallel<T>(
                           Vec3<T> left,
                           Vec3<T> right
        ) where T : unmanaged,INumber<T>
>>>>>>> 8755185e78a8091ca252c1342249110180446be3
        {
            return Cross(left, right).Equals(T.Zero);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector128<T> AsVector128Safe<T>(
                                     this Vec3<T> source
        ) where T : unmanaged, INumber<T>
        {
            Vector128<T> result = Vector128<T>.Zero;
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector128<T>, byte>(ref result), source);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector128<T> AsVector128Unsafe<T>(
                                     this Vec3<T> source
        ) where T : unmanaged, INumber<T>
        {
            Unsafe.SkipInit(out Vector128<T> result);
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector128<T>, byte>(ref result), source);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector256<T> AsVector256Safe<T>(
                                     this Vec3<T> source
        ) where T : unmanaged, INumber<T>
        {
            Vector256<T> result = Vector256<T>.Zero;
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector256<T>, byte>(ref result), source);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector256<T> AsVector256Unsafe<T>(
                                     this Vec3<T> source
        ) where T : unmanaged, INumber<T>
        {
            Unsafe.SkipInit(out Vector256<T> result);
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector256<T>, byte>(ref result), source);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vec3<T> FromVector128Unsafe<T>(
                                Vector128<T> source
        ) where T : unmanaged, INumber<T>
        {
            return Unsafe.As<Vector128<T>, Vec3<T>>(ref source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vec3<T> FromVector256Unsafe<T>(
                                Vector256<T> source
        ) where T : unmanaged, INumber<T>
        {
            return Unsafe.As<Vector256<T>, Vec3<T>>(ref source);
        }

<<<<<<< HEAD
#ednregion

=======
>>>>>>> 8755185e78a8091ca252c1342249110180446be3
        #region cross

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3<T> Cross<T>(
                              Vec3<T> left,
                              Vec3<T> right
        ) where T : unmanaged, INumber<T>
        {
            if (typeof(T) == typeof(float))
            {
                Vec3<float> result = CrossSingle(
                    Unsafe.As<Vec3<T>, Vec3<float>>(ref left),
                    Unsafe.As<Vec3<T>, Vec3<float>>(ref right));
                return Unsafe.As<Vec3<float>, Vec3<T>>(ref result);
            }
            if (typeof(T) == typeof(double))
            {
                Vec3<double> result = CrossDouble(
                    Unsafe.As<Vec3<T>, Vec3<double>>(ref left),
                    Unsafe.As<Vec3<T>, Vec3<double>>(ref right));
                return Unsafe.As<Vec3<double>, Vec3<T>>(ref result);
            }
            if (typeof(T) == typeof(int))
            {
                Vec3<int> result = CrossInt32(
                    Unsafe.As<Vec3<T>, Vec3<int>>(ref left),
                    Unsafe.As<Vec3<T>, Vec3<int>>(ref right));
                return Unsafe.As<Vec3<int>, Vec3<T>>(ref result);
            }
            if (typeof(T) == typeof(uint))
            {
                Vec3<uint> result = CrossUInt32(
                    Unsafe.As<Vec3<T>, Vec3<uint>>(ref left),
                    Unsafe.As<Vec3<T>, Vec3<uint>>(ref right));
                return Unsafe.As<Vec3<uint>, Vec3<T>>(ref result);
            }
            if (typeof(T) == typeof(ulong))
            {
                Vec3<ulong> result = CrossUInt64(
                    Unsafe.As<Vec3<T>, Vec3<ulong>>(ref left),
                    Unsafe.As<Vec3<T>, Vec3<ulong>>(ref right));
                return Unsafe.As<Vec3<ulong>, Vec3<T>>(ref result);
            }
            if (typeof(T) == typeof(long))
            {
                Vec3<long> result = CrossInt64(
                    Unsafe.As<Vec3<T>, Vec3<long>>(ref left),
                    Unsafe.As<Vec3<T>, Vec3<long>>(ref right));
                return Unsafe.As<Vec3<long>, Vec3<T>>(ref result);
            }

            throw new NotSupportedException($"Cross product is not supported for {typeof(T)}.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vec3<double> CrossDouble(
                                    Vec3<double> left,
                                    Vec3<double> right
        )
        {
            Vector256<double> l = left.AsVector256Safe();
            Vector256<double> r = right.AsVector256Safe();
            Vector256<long> yzx = Vector256.Create(1L, 2L, 0L, 3L);
            Vector256<long> zxy = Vector256.Create(2L, 0L, 1L, 3L);
            Vector256<double> result = (Vector256.Shuffle(l, yzx) * Vector256.Shuffle(r, zxy)) - (Vector256.Shuffle(l, zxy) * Vector256.Shuffle(r, yzx));
            return FromVector256Unsafe(result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vec3<int> CrossInt32(
                                 Vec3<int> left,
                                 Vec3<int> right
        )
        {
            Vector128<int> l = left.AsVector128Safe();
            Vector128<int> r = right.AsVector128Safe();
            Vector128<int> yzx = Vector128.Create(1, 2, 0, 3);
            Vector128<int> zxy = Vector128.Create(2, 0, 1, 3);
            Vector128<int> result = (Vector128.Shuffle(l, yzx) * Vector128.Shuffle(r, zxy)) - (Vector128.Shuffle(l, zxy) * Vector128.Shuffle(r, yzx));
            return FromVector128Unsafe(result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vec3<float> CrossSingle(
                                   Vec3<float> left,
                                   Vec3<float> right
        )
        {
            Vector128<float> l = left.AsVector128Safe();
            Vector128<float> r = right.AsVector128Safe();
            Vector128<int> yzx = Vector128.Create(1, 2, 0, 3);
            Vector128<int> zxy = Vector128.Create(2, 0, 1, 3);
            Vector128<float> result = (Vector128.Shuffle(l, yzx) * Vector128.Shuffle(r, zxy)) - (Vector128.Shuffle(l, zxy) * Vector128.Shuffle(r, yzx));
            return FromVector128Unsafe(result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vec3<uint> CrossUInt32(
                                  Vec3<uint> left,
                                  Vec3<uint> right
        )
        {
            Vector128<uint> l = left.AsVector128Safe();
            Vector128<uint> r = right.AsVector128Safe();
            Vector128<uint> yzx = Vector128.Create(1U, 2U, 0U, 3U);
            Vector128<uint> zxy = Vector128.Create(2U, 0U, 1U, 3U);
            Vector128<uint> result = (Vector128.Shuffle(l, yzx) * Vector128.Shuffle(r, zxy)) - (Vector128.Shuffle(l, zxy) * Vector128.Shuffle(r, yzx));
            return FromVector128Unsafe(result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vec3<ulong> CrossUInt64(
                                   Vec3<ulong> left,
                                   Vec3<ulong> right
        )
        {
            Vector256<ulong> l = left.AsVector256Safe();
            Vector256<ulong> r = right.AsVector256Safe();
            Vector256<ulong> yzx = Vector256.Create(1UL, 2UL, 0UL, 3UL);
            Vector256<ulong> zxy = Vector256.Create(2UL, 0UL, 1UL, 3UL);
            Vector256<ulong> result = (Vector256.Shuffle(l, yzx) * Vector256.Shuffle(r, zxy)) - (Vector256.Shuffle(l, zxy) * Vector256.Shuffle(r, yzx));
            return FromVector256Unsafe(result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vec3<long> CrossInt64(
                                  Vec3<long> left,
                                  Vec3<long> right
        )
        {
            Vector256<long> l = left.AsVector256Safe();
            Vector256<long> r = right.AsVector256Safe();
            Vector256<long> yzx = Vector256.Create(1U, 2U, 0U, 3U);
            Vector256<long> zxy = Vector256.Create(2U, 0U, 1U, 3U);
            Vector256<long> result = (Vector256.Shuffle(l, yzx) * Vector256.Shuffle(r, zxy)) - (Vector256.Shuffle(l, zxy) * Vector256.Shuffle(r, yzx));
            return FromVector256Unsafe(result);
        }

        #endregion
    }
}
