// -----------------------------------------------------------------------------
// file="SpaceCoordinate"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Mathematics;
using Nitload.Mathematics.Numeric.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace Nitload.Geometries
{
    public class SpaceCoordinate : CoordinationBase
    {

        public SpaceCoordinate(
               CoordinationBase localCoordination,
               Point origin,
               Vec3<float> xAxis,
               Vec3<float> yAxis,
               Vec3<float> zAxis
        )
            : base(localCoordination) { }

    }
}
