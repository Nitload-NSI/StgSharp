// -----------------------------------------------------------------------------
// file="Vector.Orthogonalize"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Numerics;
using System.Runtime.CompilerServices;

namespace Nitload.Mathematics.Numeric.Graphics
{
    public static partial class Linear
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec4<T> Orthogonalize<T>(
                              this Vec4<T> vec
        ) where T : unmanaged, INumber<T>, IRootFunctions<T>
        {
            return vec.Normalize();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3<T> Orthogonalize<T>(
                              this Vec3<T> vec
        ) where T : unmanaged, INumber<T>, IRootFunctions<T>
        {
            return vec.Normalize();
        }

        /// <summary>
        ///   Normalizes two vectors and makes <paramref name="target" /> perpendicular to <paramref
        ///   name="source" />.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Orthogonalize<T>(
                           this ref Vec4<T> source,
                           ref Vec4<T> target
        ) where T : unmanaged, INumber<T>, IRootFunctions<T>
        {
            source = source.Normalize();
            target -= source * target.Dot(source);
            target = target.Normalize();
        }

        /// <summary>
        ///   Normalizes two vectors and makes <paramref name="target" /> perpendicular to <paramref
        ///   name="source" />.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Orthogonalize<T>(
                           this ref Vec3<T> source,
                           ref Vec3<T> target
        ) where T : unmanaged, INumber<T>, IRootFunctions<T>
        {
            source = source.Normalize();
            target -= source * target.Dot(source);
            target = target.Normalize();
        }

    }
}
