// -----------------------------------------------------------------------------
// file="Vector.Normalize"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Numerics;
using System.Runtime.CompilerServices;

namespace StgSharp.Mathematics.Numeric.Graphics
{
    public static partial class Linear
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2<T> Normalize<T>(this Vec2<T> vec) where T: unmanaged, INumber<T>, IRootFunctions<T>
        {
            return vec / T.Sqrt(vec.Dot(vec));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3<T> Normalize<T>(this Vec3<T> vec) where T: unmanaged, INumber<T>, IRootFunctions<T>
        {
            return vec / T.Sqrt(vec.Dot(vec));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec4<T> Normalize<T>(this Vec4<T> vec) where T: unmanaged, INumber<T>, IRootFunctions<T>
        {
            return vec / T.Sqrt(vec.Dot(vec));
        }

        public static Vec2<float> Normalize(this Vec2<int> vec) => Vec2.ConvertToSingle(vec).Normalize();

        public static Vec2<float> Normalize(this Vec2<uint> vec) => Vec2.ConvertToSingle(vec).Normalize();

        public static Vec2<double> Normalize(this Vec2<long> vec) => Vec2.ConvertToDouble(vec).Normalize();

        public static Vec2<double> Normalize(this Vec2<ulong> vec) => Vec2.ConvertToDouble(vec).Normalize();

        public static Vec3<float> Normalize(this Vec3<int> vec) => Vec3.ConvertToSingle(vec).Normalize();

        public static Vec3<float> Normalize(this Vec3<uint> vec) => Vec3.ConvertToSingle(vec).Normalize();

        public static Vec3<double> Normalize(this Vec3<long> vec) => Vec3.ConvertToDouble(vec).Normalize();

        public static Vec3<double> Normalize(this Vec3<ulong> vec) => Vec3.ConvertToDouble(vec).Normalize();

        public static Vec4<float> Normalize(this Vec4<int> vec) => Vec4.ConvertToSingle(vec).Normalize();

        public static Vec4<float> Normalize(this Vec4<uint> vec) => Vec4.ConvertToSingle(vec).Normalize();

        public static Vec4<double> Normalize(this Vec4<long> vec) => Vec4.ConvertToDouble(vec).Normalize();

        public static Vec4<double> Normalize(this Vec4<ulong> vec) => Vec4.ConvertToDouble(vec).Normalize();

    }
}
