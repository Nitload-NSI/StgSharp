// -----------------------------------------------------------------------------
// file="Vector.Normalize"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics.Graphics;
using System.Numerics;

namespace StgSharp.Mathematics.Graphics
{
    public static partial class Linear
    {

        public static Vec4 Normalize(
                           Vec4 vec
        )
        {
            return new Vec4
            {
                vec = Vector4.Normalize(vec.vec)
            };
        }

        public static Vec3 Normalize(
                           Vec3 vec
        )
        {
            return new Vec3
            {
                v = Vector3.Normalize(vec.v)
            };
        }

        public static unsafe void Normalize(
                                  ref Vec4 source,
                                  ref Vec4 target
        )
        {
            Vector4 s = source.vec;
            Vector4 t = target.vec;

            s = Vector4.Normalize(s);

            Vector4 projection = Vector4.Multiply(s, t);

            t -= projection;

            source.vec = s;
            target.vec = t;
        }

        public static unsafe void Normalize(
                                  ref Vec3 source,
                                  ref Vec3 target
        )
        {
            Vector3 s = source.v;
            Vector3 t = target.v;

            s = Vector3.Normalize(s);
            Vector3 projection = Vector3.Dot(t, s) / Vector3.Dot(s, s) * s;
            t -= projection;
            t = Vector3.Normalize(t);

            source.v = s;
            target.v = t;
        }

    }
}
