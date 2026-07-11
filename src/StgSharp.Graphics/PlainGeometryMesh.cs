// -----------------------------------------------------------------------------
// file="PlainGeometryMesh"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Geometries;
using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphics;

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

        public PlainGeometryMesh(
               [NotNull] PlainGeometry shape,
               [NotNull] TextureProvider texture
        )
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

        public PlainGeometryMesh(
               PlainGeometry shape,
               Image texture,
               Vec2[] texCoord
        )
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

        public PlainGeometryMesh Resize(
                                 PlainGeometry newShape
        )
        {
            return new PlainGeometryMesh(newShape, _texture);
        }

    }
}
