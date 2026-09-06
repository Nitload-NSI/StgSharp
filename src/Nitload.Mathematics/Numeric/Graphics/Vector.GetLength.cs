// -----------------------------------------------------------------------------
// file="Vector.GetLength"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace Nitload.Mathematics.Numeric.Graphics
{
    public static unsafe partial class Linear
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetLength<T>(
                        this Vec3<T> vec
        ) where T : unmanaged, INumber<T>, IRootFunctions<T>
        {
            return T.Sqrt(vec.Dot(vec));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetLength<T>(
                        this Vec2<T> vec
        ) where T : unmanaged, INumber<T>, IRootFunctions<T>
        {
            return T.Sqrt(vec.Dot(vec));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetLength<T>(
                        this Vec4<T> vec
        ) where T : unmanaged, INumber<T>, IRootFunctions<T>
        {
            return T.Sqrt(vec.Dot(vec));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetLength(
                            this Vec2<int> vec
        )
        {
            return Vec2.ConvertToSingle(vec).GetLength();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetLength(
                            this Vec2<uint> vec
        )
        {
            return Vec2.ConvertToSingle(vec).GetLength();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetLength(
                             this Vec2<long> vec
        )
        {
            return Vec2.ConvertToDouble(vec).GetLength();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetLength(
                             this Vec2<ulong> vec
        )
        {
            return Vec2.ConvertToDouble(vec).GetLength();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetLength(
                            this Vec3<int> vec
        )
        {
            return Vec3.ConvertToSingle(vec).GetLength();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetLength(
                            this Vec3<uint> vec
        )
        {
            return Vec3.ConvertToSingle(vec).GetLength();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetLength(
                             this Vec3<long> vec
        )
        {
            return Vec3.ConvertToDouble(vec).GetLength();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetLength(
                             this Vec3<ulong> vec
        )
        {
            return Vec3.ConvertToDouble(vec).GetLength();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetLength(
                            this Vec4<int> vec
        )
        {
            return Vec4.ConvertToSingle(vec).GetLength();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetLength(
                            this Vec4<uint> vec
        )
        {
            return Vec4.ConvertToSingle(vec).GetLength();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetLength(
                             this Vec4<long> vec
        )
        {
            return Vec4.ConvertToDouble(vec).GetLength();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetLength(
                             this Vec4<ulong> vec
        )
        {
            return Vec4.ConvertToDouble(vec).GetLength();
        }

    }
}
