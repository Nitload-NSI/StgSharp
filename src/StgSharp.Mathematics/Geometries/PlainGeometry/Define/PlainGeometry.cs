// -----------------------------------------------------------------------------
// file="PlainGeometry"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp;
using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphics;

using System;
using System.ComponentModel;

namespace StgSharp.Geometries
{
    /// <summary>
    ///   Plain geometry only contains straight sides.
    /// </summary>
    public abstract class PlainGeometry : IGeometry
    {

        protected Vec4[] vertexMat;
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
            get => new Point(vertexMat[index]);
            set => vertexMat[index].vec = value.coordVec;
        }

        public int VertexCount => this.vertexMat.Length;

        public abstract ReadOnlySpan<int> VertexIndices { get; }

        #pragma warning disable CA1819 
        public Vec4[] VertexStream => vertexMat;

        public CoordinationBase Coordination => this.coordinate;
#pragma warning restore CA1819
    }
}