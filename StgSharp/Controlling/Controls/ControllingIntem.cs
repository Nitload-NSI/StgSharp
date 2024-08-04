using StgSharp.Geometries;
using StgSharp.Graphics;
using StgSharp.Math;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Controls
{
    public interface ControllingItem: IEnumerable<PlainGeometryMesh>
    {
        public abstract vec2d Position { get; set; }
        public abstract Rectangle BoundingBox { get; set; }

        public abstract ReadOnlySpan<vec4d> TextureBox { get; }
        public abstract bool IsEntity { get; }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
