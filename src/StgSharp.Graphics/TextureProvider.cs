// -----------------------------------------------------------------------------
// file="TextureProvider"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics.Graphics;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace StgSharp.Graphics
{
    public class TextureProvider : IImageProvider
    {

        private Vec2[] _texCoord;
        private Image _tex;

        public TextureProvider(
               Image image,
               Vec2[] coord
        )
        {
            _tex = image;
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

        public Image ProvideImage()
        {
            return _tex;
        }

        public void ReadImage(
                    IImageProvider provider
        )
        {
            _tex = provider.ProvideImage();
        }

        public void UpdateFromImage(
                    [NotNull]Image i
        )
        {
            if (i == _tex) {
                return;
            }
            if (i.Size == _tex.Size)
            {
                _tex = i;
            } else
            {
                throw new ArgumentException(
                    $"Size of {nameof(i)} does not equals to current image.");
            }
        }

    }
}
