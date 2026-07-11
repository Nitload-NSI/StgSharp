// -----------------------------------------------------------------------------
// file="ParticleOnPlain"
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
    public class ParticleOnPlain : IInstancing, IPlainEntity
    {

        public ParticleOnPlain(
               PlainInstancingBuffer<ParticleOnPlain> buffer
        )
        {
            if (buffer == null) {
                throw new ArgumentNullException(paramName:nameof(buffer));
            }
            GlobalBuffer = buffer;
            BufferId = buffer.CreateInstanceID();
            ((IInstancingBuffer)buffer).InstanceList.Add(this);
        }

        public GeometryMotion Motion { get; set; }

        public IInstancingBuffer GlobalBuffer { get; set; }

        public int BufferId { get; set; }

        Vec3 IInstancing.CenterPositionGlobal { get; set; }

        bool IPlainEntity.CollideWith(
                          IPlainEntity entity
        )
        {
            throw new NotImplementedException();
        }

        Vec2 IPlainEntity.CenterPosition
        {
            get => GlobalBuffer.CoordAndRotationList[BufferId].XY;
            set => throw new NotImplementedException();
        }

    }
}
