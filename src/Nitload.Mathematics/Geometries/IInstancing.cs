// -----------------------------------------------------------------------------
// file="IInstancing"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Mathematics;
using Nitload.Mathematics.Numeric.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Nitload.Geometries
{
    public interface IInstancing
    {

        public float Rotation //
        { get => GlobalBuffer.CoordAndRotationList[BufferId].W; //
        set
        {
            Vec4<float> temp = GlobalBuffer.CoordAndRotationList[BufferId];
            temp.W = value;
            GlobalBuffer.CoordAndRotationList[BufferId] = temp;
        } }

        public float Scale { get => GlobalBuffer.ScalingList[BufferId]; set => GlobalBuffer.ScalingList[BufferId] =
                                                                               value; }

        public GeometryMotion Motion { get; set; }

        public IInstancingBuffer GlobalBuffer { get; internal set; }

        public int BufferId { get; internal set; }

        public Vec3<float> CenterPositionGlobal { get; internal set; }

        public Vec3<float> Coord { get => GlobalBuffer.CoordAndRotationList[BufferId].XYZ; set
                                                                                           {
                                                                                               Vec4<float> v = new(value, 0);
                                                                                               GlobalBuffer.CoordAndRotationList[BufferId] = v;
                                                                                           } }

        public void Move()
        {
            (Vec3<float> coord, float rotation) = (Coord, Rotation);
            Motion.RunMotion(ref coord, ref rotation);
            (Coord, Rotation) = (coord, rotation);
        }

    }
}
