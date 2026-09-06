// -----------------------------------------------------------------------------
// file="PlainGeometry"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload;
using Nitload.Mathematics;
using Nitload.Mathematics.Numeric.Graphics;

using System;
using System.ComponentModel;

namespace Nitload.Geometries
{
    /// <summary>
    ///   Plain geometry only contains straight sides.
    /// </summary>
    public abstract class PlainGeometry : IGeometry
    {

        protected Vec3<float>[] vertexMat;
        internal CoordinationBase coordinate;

        internal PlainGeometry(
                 CoordinationBase coordination
        )
        {
            coordinate = coordination;
        }

        public Point this[
                     int index
        ]
        {
            get => new(vertexMat[index]);
            set => vertexMat[index] = value.Coord;
        }

        public int VertexCount => this.vertexMat.Length;

        public abstract ReadOnlySpan<int> VertexIndices { get; }

        #pragma warning disable CA1819 
        public Vec3<float>[] VertexStream => vertexMat;

        public CoordinationBase Coordination => this.coordinate;
#pragma warning restore CA1819
    }
}
