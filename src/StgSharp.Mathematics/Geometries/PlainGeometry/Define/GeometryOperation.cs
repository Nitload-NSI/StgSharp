// -----------------------------------------------------------------------------
// file="GeometryOperation"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphics;

using System.Runtime.CompilerServices;

namespace StgSharp.Geometries
{
    public static unsafe class GeometryOperation
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 DefaultMotion(
                           int tick
        )
        {
            return default(Vec3);
        }

    }
}
