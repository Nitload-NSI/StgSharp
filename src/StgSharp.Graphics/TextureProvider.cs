// -----------------------------------------------------------------------------
// file="TextureProvider"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics.Graphics;

using System;
using System.Runtime.CompilerServices;

namespace StgSharp.Graphics
{
    public class TextureProvider
    {

        private Vec2[] _texCoord;
        private Image<Rgba8> _image;

        public TextureProvider(
               Image<Rgba8> image,
               Vec2[] coord
        )
        {
            ArgumentNullException.ThrowIfNull(image);
            ArgumentNullException.ThrowIfNull(coord);

            _image = image;
            _texCoord = coord;
        }

        public ref Vec2 this[
                        int index
        ]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref _texCoord[index];
        }

        public Span<Vec2> TextureCoordinate => new Span<Vec2>(_texCoord);

        public Image<Rgba8> Image
        {
            get => _image;
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                _image = value;
            }
        }

    }
}
