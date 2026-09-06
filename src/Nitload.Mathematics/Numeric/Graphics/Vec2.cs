// -----------------------------------------------------------------------------
// file="Vec2"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Nitload.Mathematics.Numeric.Graphics
{
    public static class Vec2
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector128<T> AsVector128Safe<T>(this Vec2<T> source) where T: unmanaged, INumber<T>
        {
            Vector128<T> result = Vector128<T>.Zero;
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector128<T>, byte>(ref result), source);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector128<T> AsVector128Unsafe<T>(this Vec2<T> source) where T: unmanaged, INumber<T>
        {
            Unsafe.SkipInit(out Vector128<T> result);
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector128<T>, byte>(ref result), source);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vec2<T> FromVector128Safe<T>(Vector128<T> source) where T: unmanaged, INumber<T>
        {
            return Unsafe.As<Vector128<T>, Vec2<T>>(ref source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vec2<T> FromVector128Unsafe<T>(Vector128<T> source) where T: unmanaged, INumber<T>
        {
            return Unsafe.As<Vector128<T>, Vec2<T>>(ref source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2<float> ConvertToSingle(Vec2<int> source)
        {
            return FromVector128Unsafe(Vector128.ConvertToSingle(source.AsVector128Safe()));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2<float> ConvertToSingle(Vec2<uint> source)
        {
            return FromVector128Unsafe(Vector128.ConvertToSingle(source.AsVector128Safe()));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2<double> ConvertToDouble(Vec2<long> source)
        {
            return FromVector128Unsafe(Vector128.ConvertToDouble(source.AsVector128Safe()));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2<double> ConvertToDouble(Vec2<ulong> source)
        {
            return FromVector128Unsafe(Vector128.ConvertToDouble(source.AsVector128Safe()));
        }

    }
}
