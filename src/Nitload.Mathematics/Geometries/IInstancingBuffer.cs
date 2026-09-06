// -----------------------------------------------------------------------------
// file="IInstancingBuffer"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Mathematics;
using Nitload.Mathematics.Numeric.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace Nitload.Geometries
{
    public interface IInstancingBuffer
    {

        public Span<Vec4<float>> CoordAndRotationSpan { get; }

        public Span<float> ScalingSpan { get; }

        internal IGeometry TypicalShape { get; }

        internal List<IInstancing> InstanceList { get; }

        internal List<Vec4<float>> CoordAndRotationList { get; }

        internal List<float> ScalingList { get; }

    }
}
