// -----------------------------------------------------------------------------
// file="PlainCoordinate"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphics;

using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace StgSharp.Geometries
{
    public class PlainCoordinate : CoordinationBase
    {

        public PlainCoordinate(
               CoordinationBase local,
               Point origin,
               Vec3 xAxis,
               Vec3 yAxis
        )
            : base(local)
        {
            CoordMat = GraphicsMatrix.Unit;

            CoordMat.column[0] = new Vec4(xAxis, 0);
            CoordMat.column[1] = new Vec4(yAxis, 0);
            CoordMat.column[2] = new Vec4(Linear.Normalize(Vec3.Cross(xAxis, yAxis)), 0);

            LocalOrigin = origin;
        }

        public static PlainCoordinate StandardPlainCoordination { get; } = new(null,
                                                                               new Point(0, 0, 0),
                                                                               new Vec3(1, 0, 0),
                                                                               new Vec3(0, 1, 0));

    }
}
