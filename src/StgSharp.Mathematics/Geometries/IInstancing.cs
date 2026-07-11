// -----------------------------------------------------------------------------
// file="IInstancing"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Geometries
{
    public interface IInstancing
    {

        public float Rotation { get => GlobalBuffer.CoordAndRotationList[BufferId].W; set
                                                                                      {
                                                                                          Vec4 temp = GlobalBuffer.CoordAndRotationList[
                                                                                                      BufferId];
                                                                                          temp.W = value;
                                                                                          GlobalBuffer.CoordAndRotationList[
                                                                                                      BufferId] = temp;
                                                                                      } }

        public float Scale { get => GlobalBuffer.ScalingList[BufferId]; set => GlobalBuffer.ScalingList[BufferId] =
                                                                               value; }

        public GeometryMotion Motion { get; set; }

        public IInstancingBuffer GlobalBuffer { get; internal set; }

        public int BufferId { get; internal set; }

        public Vec3 CenterPositionGlobal { get; internal set; }

        public Vec3 Coord { get => GlobalBuffer.CoordAndRotationList[BufferId].XYZ; set => GlobalBuffer.CoordAndRotationList[BufferId] =
                                                                                           new Vec4(value.reg); }

        public void Move()
        {
            (Vec3 coord, float rotation) = (Coord, Rotation);
            Motion.RunMotion(ref coord, ref rotation);
            (Coord, Rotation) = (coord, rotation);
        }

    }
}
