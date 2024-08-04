using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.Graphics
{
    public class TextureProvider : IImageProvider
    {
        private Image _tex;
        private vec2d[] _texCoord;

        public Span<vec2d> TextureCoordinate
        {
            get => new Span<vec2d>(_texCoord);
        }

        public Image ProvideImage()
        {
            return _tex;
        }

        public TextureProvider(Image image, vec2d[] coord)
        {
            _tex = image;
            _texCoord = coord;
        }

        public ref vec2d this[int index]
        {
            get => ref _texCoord[index];
        }

        public void UpdateFromImage(Image i)
        {
            if (i == _tex)
            {
                return;
            }
            if (i.Size == _tex.Size)
            {
                _tex = i;
            }
            else
            {
                throw new ArgumentException($"Size of {nameof(i)} does not equals to current image.");
            }

        }

        public void ReadImage(IImageProvider provider)
        {
            _tex = provider.ProvideImage();
        }

    }
}
