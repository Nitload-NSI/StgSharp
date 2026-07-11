// -----------------------------------------------------------------------------
// file="Vector.Cross"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics.Graphics;

namespace StgSharp.Mathematics.Graphics
{
    public static unsafe partial class Linear
    {

        public static float Cross(
                            Vec2 vec1,
                            Vec2 vec2
        )
        {
            return (vec1.X * vec2.Y) - (vec1.Y * vec2.X);
        }

        public static Vec3 Cross(
                           Vec3 vec1,
                           Vec3 vec2
        )
        {
            return new Vec3(
                (vec1.Y * vec2.Z) - (vec1.Z * vec2.Y),
                (vec1.X * vec2.Z) - (vec1.Z * vec2.X),
                (vec1.X * vec2.Y) - (vec1.Y * vec2.X));
        }

    }
}
