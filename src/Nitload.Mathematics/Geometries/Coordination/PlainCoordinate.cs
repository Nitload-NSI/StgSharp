// -----------------------------------------------------------------------------
// file="PlainCoordinate"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Mathematics;
using Nitload.Mathematics.Numeric.Graphics;

using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Nitload.Geometries
{
    public class PlainCoordinate : CoordinationBase
    {

        public PlainCoordinate(
               CoordinationBase local,
               Point origin,
               Vec3<float> xAxis,
               Vec3<float> yAxis
        )
            : base(local)
        {
            CoordMat = GMatrix44<float>.Unit;

            CoordMat[0] = new Vec4<float>(xAxis, 0);
            CoordMat[1] = new Vec4<float>(yAxis, 0);
            CoordMat[2] = new Vec4<float>(Linear.Normalize(Vec3.Cross(xAxis, yAxis)), 0);

            LocalOrigin = origin;
        }

        public static PlainCoordinate StandardPlainCoordination { get; } = new(default,
                                                                               new Point(0, 0, 0),
                                                                               new Vec3<float>(1, 0, 0),
                                                                               new Vec3<float>(0, 1, 0));

    }
}
