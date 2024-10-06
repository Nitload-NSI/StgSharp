//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="TextureBufferImage.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the ¡°Software¡±), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED ¡°AS IS¡±, 
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

using StgSharp;
using StgSharp.Graphics.OpenGL;
using StgSharp.Math;

using System;

namespace StgSharp.Graphics
{
    public class TextureImage : IImageProvider
    {

        private byte[] _data;
        private ImageInfo _imageInfo;
        private int _pixelUpdateCount = 0;

        public TextureImage(
            Vec2 size,
            ImageChannel channel,
            PixelChannelLayout layout,
            int bufferSize )
        {
            this._imageInfo = new ImageInfo
            {
                Width = ( int )size.X,
                Height = ( int )size.Y,
                Channel = channel,
                pixelLayout = layout,
            };
            if( bufferSize < _imageInfo.Width * _imageInfo.Height * GlHelper.GetPixelSize(
                layout, channel ) ) {
                throw new OverflowException(
                    "Size of data buffer is smaller than default image size." );
            }
            _data = new byte[bufferSize];
        }

        public int Width
        {
            get => _imageInfo.Width;
            internal set => _imageInfo.Width = value;
        }

        public int Height
        {
            get => _imageInfo.Height;
            internal set => _imageInfo.Height = value;
        }

        public Image ProvideImage()
        {
            return Image.FromMemory( _imageInfo, _data, _pixelUpdateCount );
        }

        public void Resize( int width, int height )
        {
            if( _data.Length < width * height * GlHelper.GetPixelSize(
                _imageInfo.pixelLayout, _imageInfo.Channel ) ) {
                throw new OverflowException(
                    "Size of image is larger than capacity of sata buffer." );
            }
            Width = width;
            Height = height;
            _pixelUpdateCount++;
        }

    }
}