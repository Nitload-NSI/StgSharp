// -----------------------------------------------------------------------------
// file="SpaceCoordinate"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Geometries
{
    public class SpaceCoordinate : CoordinationBase
    {

        public SpaceCoordinate(
               CoordinationBase localCoordination,
               Point origin,
               Vec3 xAxis,
               Vec3 yAxis,
               Vec3 zAxis
        )
            : base(localCoordination) { }

    }
}
