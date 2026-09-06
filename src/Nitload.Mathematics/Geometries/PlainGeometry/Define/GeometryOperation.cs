// -----------------------------------------------------------------------------
// file="GeometryOperation"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Mathematics;
using Nitload.Mathematics.Numeric.Graphics;

using System.Runtime.CompilerServices;

namespace Nitload.Geometries
{
    public static unsafe class GeometryOperation
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3<float> DefaultMotion(
                                  int tick
        )
        {
            return Vec3<float>.Zero;
        }

    }
}
