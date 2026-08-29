// -----------------------------------------------------------------------------
// file="Vec4"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace StgSharp.Mathematics.Numeric.Graphics
{
    public static class Vec4
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector128<T> AsVector128Unsafe<T>(this Vec4<T> source) where T: unmanaged, INumber<T>
        {
            Unsafe.SkipInit(out Vector128<T> result);
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector128<T>, byte>(ref result), source);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector256<T> AsVector256Unsafe<T>(this Vec4<T> source) where T: unmanaged, INumber<T>
        {
            Unsafe.SkipInit(out Vector256<T> result);
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector256<T>, byte>(ref result), source);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vec4<T> FromVector128Unsafe<T>(Vector128<T> source) where T: unmanaged, INumber<T>
        {
            return Unsafe.As<Vector128<T>, Vec4<T>>(ref source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vec4<T> FromVector256Unsafe<T>(Vector256<T> source) where T: unmanaged, INumber<T>
        {
            return Unsafe.As<Vector256<T>, Vec4<T>>(ref source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec4<float> ConvertToSingle(Vec4<int> source)
        {
            return FromVector128Unsafe(Vector128.ConvertToSingle(source.AsVector128Unsafe()));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec4<float> ConvertToSingle(Vec4<uint> source)
        {
            return FromVector128Unsafe(Vector128.ConvertToSingle(source.AsVector128Unsafe()));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec4<double> ConvertToDouble(Vec4<long> source)
        {
            return FromVector256Unsafe(Vector256.ConvertToDouble(source.AsVector256Unsafe()));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec4<double> ConvertToDouble(Vec4<ulong> source)
        {
            return FromVector256Unsafe(Vector256.ConvertToDouble(source.AsVector256Unsafe()));
        }

    }
}
