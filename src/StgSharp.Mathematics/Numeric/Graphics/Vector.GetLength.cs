// -----------------------------------------------------------------------------
// file="Vector.GetLength"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics.Numeric.Graphics;

namespace StgSharp.Mathematics.Numeric.Graphics
{
    public static unsafe partial class Linear
    {

        public static T GetLength<T>(
                        this Vec3<T> vec
        ) where T : unmanaged, INumber<T>, IRootFunctions<T>
        {
            return T.Sqrt(vec.Dot(vec));
        }

        public static T GetLength<T>(
                        this Vec2<T> vec
        ) where T : unmanaged, INumber<T>, IRootFunctions<T>
        {
            return T.Sqrt(vec.Dot(vec));
        }

        public static T GetLength<T>(
                        this Vec4<T> vec
        ) where T : unmanaged, INumber<T>, IRootFunctions<T>
        {
            return T.Sqrt(vec.Dot(vec));
        }

        public static float GetLength(this Vec2<int> vec) => Vec2.ConvertToSingle(vec).GetLength();

        public static float GetLength(this Vec2<uint> vec) => Vec2.ConvertToSingle(vec).GetLength();

        public static double GetLength(this Vec2<long> vec) => Vec2.ConvertToDouble(vec).GetLength();

        public static double GetLength(this Vec2<ulong> vec) => Vec2.ConvertToDouble(vec).GetLength();

        public static float GetLength(this Vec3<int> vec) => Vec3.ConvertToSingle(vec).GetLength();

        public static float GetLength(this Vec3<uint> vec) => Vec3.ConvertToSingle(vec).GetLength();

        public static double GetLength(this Vec3<long> vec) => Vec3.ConvertToDouble(vec).GetLength();

        public static double GetLength(this Vec3<ulong> vec) => Vec3.ConvertToDouble(vec).GetLength();

        public static float GetLength(this Vec4<int> vec) => Vec4.ConvertToSingle(vec).GetLength();

        public static float GetLength(this Vec4<uint> vec) => Vec4.ConvertToSingle(vec).GetLength();

        public static double GetLength(this Vec4<long> vec) => Vec4.ConvertToDouble(vec).GetLength();

        public static double GetLength(this Vec4<ulong> vec) => Vec4.ConvertToDouble(vec).GetLength();

    }
}
