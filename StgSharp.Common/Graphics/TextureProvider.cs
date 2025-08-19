//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="TextureProvider.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
using StgSharp.Mathematics;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.Graphics
{
    public class TextureProvider : IImageProvider
    {

        private Vec2[] _texCoord;
        private Image _tex;

        public TextureProvider( Image image, Vec2[] coord )
        {
            _tex = image;
            _texCoord = coord;
        }

        public ref Vec2 this[ int index ]
        {
            get => ref _texCoord[ index ];
        }

        public Span<Vec2> TextureCoordinate
        {
            get => new Span<Vec2>( _texCoord );
        }

        public Image ProvideImage()
        {
            return _tex;
        }

        public void ReadImage( IImageProvider provider )
        {
            _tex = provider.ProvideImage();
        }

        public void UpdateFromImage( [NotNull]Image i )
        {
            if( i == _tex ) {
                return;
            }
            if( i.Size == _tex.Size )
            {
                _tex = i;
            } else
            {
                throw new ArgumentException(
                    $"Size of {nameof(i)} does not equals to current image." );
            }
        }

    }
}
