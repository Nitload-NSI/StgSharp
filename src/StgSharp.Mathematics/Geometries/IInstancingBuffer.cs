// -----------------------------------------------------------------------------
// file="IInstancingBuffer"
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
    public interface IInstancingBuffer
    {

        public Span<Vec4> CoordAndRotationSpan { get; }

        public Span<float> ScalingSpan { get; }

        internal IGeometry TypicalShape { get; }

        internal List<IInstancing> InstanceList { get; }

        internal List<Vec4> CoordAndRotationList { get; }

        internal List<float> ScalingList { get; }

    }
}
