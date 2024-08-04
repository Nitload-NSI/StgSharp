using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Geometries
{
    public interface IInstancingBuffer
    {

        internal IGeometry TypicalShape { get; }

        internal List<IInstancing> InstanceList { get; }

        internal List<vec4d> CoordAndRotationList { get; }

        internal List<float> ScalingList { get; }

        public Span<vec4d> CoordAndRotationSpan { get; }

        public Span<float> ScalingSpan { get; }

        
    }

}
