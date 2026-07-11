// -----------------------------------------------------------------------------
// file="IGeometry"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphics;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Geometries
{
    public interface IGeometry
    {

        public Point this[
                     int index
        ] { get; set; }

        #pragma warning disable CA1819 
        public Vec4[] VertexStream { get; }
 #pragma warning restore CA1819 

        public CoordinationBase Coordination { get; }

        public int VertexCount { get; }

        public ReadOnlySpan<int> VertexIndices { get; }

    }
}
