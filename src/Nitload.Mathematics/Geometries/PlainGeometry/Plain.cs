// -----------------------------------------------------------------------------
// file="Plain"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Mathematics;
using Nitload.Mathematics.Numeric.Graphics;

using System.Numerics;

namespace Nitload.Geometries
{
    public unsafe class Plain
    {

        internal readonly float d = -1;
        internal Vec3<float> _origin;

        internal Vec3<float> plainParameter;
        internal Vec3<float> vec1;
        internal Vec3<float> vec2;

        public Plain(
               Vec3<float> o,
               Vec3<float> v1,
               Vec3<float> v2
        )
        {
            this._origin = o;
            this.vec1 = v1 - (v2 * (v1.Y / v2.Y));   //xz
            this.vec2 = v2 - (vec1 * (v2.X / v1.X)); //yz

            // |_origin|-k1*|vec1|-k2*|vec2| --> plainParameter.Z；
            plainParameter.Z = _origin.Z - (vec1.Z * (_origin.X / vec1.X)) - (vec2.Z * (_origin.Y / vec2.Y));

            // plainParameter.Z -> plainParameter.X, plainParameter.Y
            plainParameter.X = vec1.X * (vec1.Z / plainParameter.Z);
            plainParameter.Y = vec2.Y * (vec2.Z / plainParameter.Z);

            plainParameter.X = 1 / plainParameter.X;
            plainParameter.Y = 1 / plainParameter.Y;
            plainParameter.Z = 1 / plainParameter.Z;
        }

        public Plain(
               Point p1,
               Point p2,
               Point p3
        )
        {
            Vec3<float> origin = p1.Coord;
            Vec3<float> v1 = p2.Coord - origin;
            Vec3<float> v2 = p3.Coord - origin;


            vec1 = v1 - (v2 * (v1.Y / v2.Y));   //xz
            vec2 = v2 - (vec1 * (v2.X / v1.X)); //yz

            // |_origin|-k1*|vec1|-k2*|vec2| --> plainParameter.Z；
            plainParameter.Z = origin.Z - (vec1.Z * (origin.X / vec1.X)) - (vec2.Z * (origin.Y / vec2.Y));

            // plainParameter.Z --> plainParameter.X, plainParameter.Y
            plainParameter.X = vec1.X * (vec1.Z / plainParameter.Z);
            plainParameter.Y = vec2.Y * (vec2.Z / plainParameter.Z);

            plainParameter.X = 1 / plainParameter.X;
            plainParameter.Y = 1 / plainParameter.Y;
            plainParameter.Z = 1 / plainParameter.Z;
        }

    }
}
