//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Image.cs"
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
using StgSharp.Internal;
using StgSharp.Graphics.OpenGL;
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics
{
    public class Image : ISSDSerializable, IImageProvider
    {

        private byte[] _data;
        private ImageInfo _rawInfo;
        private int _pixelUpdateCount = 0;

        internal unsafe Image()
        {
            _rawInfo = new ImageInfo();
        }

        internal Image( ImageInfo information )
        {
            _rawInfo = information;
            _data = Array.Empty<byte>();

            //int length = _rawInfo.width * _rawInfo.height * _rawInfo.channel;
        }

        public (int width, int height) Size
        {
            get => (_rawInfo.Width, _rawInfo.Height);
        }

        public byte[] PixelBuffer
        {
            get
            {
                if( ( _data.Length == 0 ) || ( _data == null ) ) {
                    GetBytes();
                    return _data!;
                }
                return _data;
            }
        }

        public ImageChannel Channel => _rawInfo.Channel;

        /// <summary>
        /// Hash code of time pixels updated last time.
        /// </summary>
        public int PixelUpdateCount
        {
            get => _pixelUpdateCount;
            internal set => _pixelUpdateCount = value;
        }

        public int ChannelCount => _rawInfo.ChannelCount;

        public int Height
        {
            get => _rawInfo.Height;
            internal set => _rawInfo.Height = value;
        }

        public int Width
        {
            get => _rawInfo.Width;
            internal set => _rawInfo.Width = value;
        }

        public int PixelSize => ImageInfo.GetPixelSize(
            _rawInfo.pixelLayout, _rawInfo.Channel );

        public PixelChannelLayout PixelLayout
        {
            get => _rawInfo.pixelLayout;
            private set => _rawInfo.pixelLayout = value;
        }

        public SerializableTypeCode SSDTypeCode => SerializableTypeCode.PixelImage;

        internal ref byte[] Data
        {
            get => ref _data;
        }

        public void FromBytes( byte[] stream ) { }

        public static unsafe Image FromFile( string route, ImageLoader loader )
        {
            Image ret = new Image();
            fixed( ImageInfo* iptr = &ret._rawInfo ) {
                InternalIO.InternalLoadImage( route, iptr, loader );
            }
            return ret;
        }

        public static Image FromMemory(
            (int width, int height) size,
            ImageChannel channel,
            byte[] stream )
        {
            ImageInfo _info = new ImageInfo
            {
                Height = size.height,
                Width = size.width,
                Channel = channel,
                StreamPtr = IntPtr.Zero
            };
            Image ret = new Image( _info );
            ret.PixelLayout = PixelChannelLayout.Byte;
            ret._data = stream;
            return ret;
        }

        public static Image FromMemory(
            (int width, int height) size,
            ImageChannel channel,
            PixelChannelLayout layout,
            byte[] stream )
        {
            ImageInfo _info = new ImageInfo
            {
                Height = size.height,
                Width = size.width,
                Channel = channel,
                StreamPtr = IntPtr.Zero
            };
            Image ret = new Image( _info );
            ret.PixelLayout = layout;
            ret._data = stream;
            return ret;
        }

        public unsafe byte[] GetBytes()
        {
            if( _data.Length == 0 ) {
                int size = Width * Height * ImageInfo.GetPixelSize(
                    PixelLayout, Channel );
                _data = new Span<byte>( ( byte* )_rawInfo.StreamPtr,
                                        size ).ToArray();
                fixed( ImageInfo* pptr = &_rawInfo ) {
                    InternalIO.InternalUnloadImage( pptr );
                }
                _rawInfo.StreamPtr = IntPtr.Zero;
            }
            return _data;
        }

        public Image ProvideImage() => this;

        internal static Image FromMemory(
            ImageInfo info,
            byte[] data,
            int operationCount )
        {
            return new Image
            {
                _rawInfo = info,
                _data = data,
                _pixelUpdateCount = operationCount
            };
        }

        byte[] ISSDSerializable.GetBytes()
        {
            throw new NotImplementedException();
        }

    }

}

namespace StgSharp.Graphics.OpenGL
{
    public static partial class GlHelper
    {

    }
}