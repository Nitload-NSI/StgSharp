using StgSharp.Geometries;
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics
{
    public class PlainGeometryMesh
    {
        private PlainGeometry _shape;
        private TextureProvider _texture;
        //private Image _texture;

        public PlainGeometryMesh(
            PlainGeometry shape, Image texture, vec2d[] texCoord
            )
        {
            if (shape==null)
            {
                throw new ArgumentNullException(nameof(shape));
            }
            if (texCoord == null)
            {
                throw new ArgumentNullException(nameof(texCoord));
            }

            if (shape.VertexCount > texCoord.Length)
            {
                throw new ArgumentException(paramName: nameof(texCoord), message: "Amount of texture coordination is too less");
            }
            _texture = new TextureProvider(texture,texCoord);
        }

        public PlainGeometryMesh([NotNull]PlainGeometry shape, [NotNull]TextureProvider texture)
        {
            if (shape.VertexCount > texture.TextureCoordinate.Length)
            {
                throw new ArgumentException(paramName: nameof(texture), message: "Amount of texture coordination is too less");
            }
            _texture = texture;
            _shape = shape;
        }

        public PlainGeometryMesh Resize(PlainGeometry newShape)
        {
            return new PlainGeometryMesh(newShape, _texture);
        }

        public ReadOnlySpan<int> MeshIndices => _shape.VertexIndices;

        public vec4d[] VertexArray => _shape.VertexStream;

        public Image TextureImage 
        {
            get => _texture.ProvideImage();
            internal set => _texture.ReadImage( value);
        }
        
        public ReadOnlySpan<vec4d> TextureCoord => MemoryMarshal.Cast<vec2d,vec4d>( _texture.TextureCoordinate);
    }
}
