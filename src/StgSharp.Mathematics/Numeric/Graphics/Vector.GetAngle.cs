// -----------------------------------------------------------------------------
// file="Vector.GetAngle"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics.Graphics;

namespace StgSharp.Mathematics.Graphics
{
    public static unsafe partial class Linear
    {

        public static float GetAngle(
                            Vec2 vec1,
                            Vec2 vec2
        )
        {
            return MathF.Acos(
                1 / (vec1.GetLength() * vec2.GetLength()));
        }

        public static float GetAngle(
                            Vec3 vec1,
                            Vec3 vec2
        )
        {
            return MathF.Acos(vec1 * vec2 / (vec1.GetLength() * vec2.GetLength()));
        }

    }
}
