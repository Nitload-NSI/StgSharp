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

        private readonly Vec2<float>[] _textureCoordinates;
        private readonly Image<Rgba8> _textureImage;

        private readonly PlainGeometry _shape;

        public PlainGeometryMesh(
               PlainGeometry shape,
               Image<Rgba8> texture,
               Vec2<float>[] texCoord
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

        public Vec3<float>[] VertexArray => _shape.VertexStream;

        public Image<Rgba8> TextureImage => _textureImage;

        public ReadOnlySpan<int> MeshIndices => _shape.VertexIndices;

        public ReadOnlySpan<Vec2<float>> TextureCoord => _textureCoordinates;

        public PlainGeometryMesh Resize(
                                 PlainGeometry newShape
        )
        {
            return new PlainGeometryMesh(newShape, _textureImage, _textureCoordinates);
        }

    }
}
