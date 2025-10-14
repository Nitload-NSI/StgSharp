//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="PlainGeometryMesh"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.Geometries;
using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphic;

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

        public PlainGeometryMesh([NotNull] PlainGeometry shape, [NotNull] TextureProvider texture)
        {
            if (shape.VertexCount > texture.TextureCoordinate.Length) {
                throw new ArgumentException(
                    paramName:nameof(texture),
                    message:"Amount of texture coordination is too less");
            }
            _texture = texture;
            _shape = shape;
        }

        // private Image _texture;

        public PlainGeometryMesh(PlainGeometry shape, Image texture, Vec2[] texCoord)
        {
            ArgumentNullException.ThrowIfNull(shape);
            ArgumentNullException.ThrowIfNull(texCoord);

            if (shape.VertexCount > texCoord.Length) {
                throw new ArgumentException(
                    paramName:nameof(texCoord),
                    message:"Amount of texture coordination is too less");
            }
            _texture = new TextureProvider(texture, texCoord);
        }

        public Vec4[] VertexArray => _shape.VertexStream;

        public Image TextureImage
        {
            get => _texture.ProvideImage();
            internal set => _texture.ReadImage(value);
        }

        public ReadOnlySpan<int> MeshIndices => _shape.VertexIndices;

        public ReadOnlySpan<Vec4> TextureCoord => MemoryMarshal.Cast<Vec2, Vec4>(
            _texture.TextureCoordinate);

        public PlainGeometryMesh Resize(PlainGeometry newShape)
        {
            return new PlainGeometryMesh(newShape, _texture);
        }

    }
}
