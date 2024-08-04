using StgSharp.Controls;
using StgSharp.Geometries;
using StgSharp.Graphics;
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Controls
{
    public class DecorationBox : ControllingItem
    {
        private PlainGeometryMesh renderMesh;

        public ReadOnlySpan<vec4d> TextureBox
        {
            get => renderMesh.TextureCoord;
        }

        public vec2d Position { get ; set ; }
        public Rectangle BoundingBox { get ; set ; }

        public bool IsEntity => true;

        public IEnumerator<PlainGeometryMesh> GetEnumerator()
        {
            yield return renderMesh;
        }

        public DecorationBox(Rectangle bound, Image texture, vec2d[] texCoord)
        {
            renderMesh = new PlainGeometryMesh(BoundingBox, new TextureProvider(texture, texCoord));
        }
    }
}
