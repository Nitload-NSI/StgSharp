// -----------------------------------------------------------------------------
// file="PlainGeometryMesh"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Geometries;
using Nitload.Mathematics;
using Nitload.Mathematics.Numeric.Graphics;
using System;
using System.Runtime.InteropServices;

namespace Nitload.Graphics
{
    public class PlainGeometryMesh
    {

        private readonly PlainGeometry _shape;
        private readonly Vec2[] _textureCoordinates;
        private readonly Image<Rgba8> _textureImage;

        public PlainGeometryMesh(
               PlainGeometry shape,
               Image<Rgba8> texture,
               Vec2[] texCoord
        )
        {
            ArgumentNullException.ThrowIfNull(shape);
            ArgumentNullException.ThrowIfNull(texture);
            ArgumentNullException.ThrowIfNull(texCoord);

            if (shape.VertexCount > texCoord.Length) {
                throw new ArgumentException(
                    paramName:nameof(texCoord),
                    message:"Amount of texture coordination is too less");
            }
            _shape = shape;
            _textureImage = texture;
            _textureCoordinates = texCoord;
        }

        public Vec4[] VertexArray => _shape.VertexStream;

        public Image<Rgba8> TextureImage => _textureImage;

        public ReadOnlySpan<int> MeshIndices => _shape.VertexIndices;

        public ReadOnlySpan<Vec4> TextureCoord => MemoryMarshal.Cast<Vec2, Vec4>(
            _textureCoordinates);

        public PlainGeometryMesh Resize(
                                 PlainGeometry newShape
        )
        {
            return new PlainGeometryMesh(newShape, _textureImage, _textureCoordinates);
        }

    }
}
