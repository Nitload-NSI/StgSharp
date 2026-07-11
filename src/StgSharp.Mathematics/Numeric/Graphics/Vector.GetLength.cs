// -----------------------------------------------------------------------------
// file="Vector.GetLength"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics.Graphics;

namespace StgSharp.Mathematics.Graphics
{
    public static unsafe partial class Linear
    {

        public static float GetLength(
                            this Vec3 vec
        )
        {
            return MathF.Sqrt(vec.Dot(vec));
        }

        public static float GetLength(
                            this Vec2 vec
        )
        {
            return MathF.Sqrt(vec.Dot(vec));
        }

    }
}
