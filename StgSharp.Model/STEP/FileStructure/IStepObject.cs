using StgSharp.Blueprint;
using StgSharp.Geometries;
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Modeling.STEP
{
    public abstract class IStepObject: StepExpressionBase , IGeometry
    {
        public abstract Point this[int index] { get; set; }

        public abstract Vec4[] VertexStream { get; }
        public abstract CoordinationBase Coordination { get; }
        public abstract int VertexCount { get; }
        public abstract ReadOnlySpan<int> VertexIndices { get; }

    }
}
