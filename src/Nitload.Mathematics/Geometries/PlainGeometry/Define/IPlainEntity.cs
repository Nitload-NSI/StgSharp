// -----------------------------------------------------------------------------
// file="IPlainEntity"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Mathematics;
using Nitload.Mathematics.Numeric.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitload.Geometries
{
    public interface IPlainEntity
    {

        public Vec2<float> CenterPosition { get; set; }

        public bool CollideWith(
                    IPlainEntity entity
        );

    }

    public class PlainEntity : IPlainEntity
    {

        private PlainGeometry _shape;

        public PlainEntity(
               PlainGeometry shape
        )
        {
            _shape = shape;
        }

        bool IPlainEntity.CollideWith(
                          IPlainEntity entity
        )
        {
            throw new NotImplementedException();
        }

        Vec2<float> IPlainEntity.CenterPosition
        {
            get => _shape.Coordination.LocalOrigin.Coord.XY;
            set => throw new InvalidOperationException();
        }

    }
}
