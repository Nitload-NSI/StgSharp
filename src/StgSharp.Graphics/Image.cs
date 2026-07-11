// -----------------------------------------------------------------------------
// file="Image"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Graphics.OpenGL;

using System;
using System.Runtime.CompilerServices;

namespace StgSharp.Graphics
{
    public class Image : IImageProvider
    {

        private byte[] _data;
        private ImageInfo _rawInfo;
        private int _pixelUpdateCount = 0;

        internal unsafe Image()
        {
            _rawInfo = new ImageInfo();
        }

        internal Image(
                 ImageInfo information
        )
        {
            _rawInfo = information;
            _data = [];

            // int length = _rawInfo.width * _rawInfo.height * _rawInfo.channel;
        }

        public (int width, int height) Size => (_rawInfo.Width, _rawInfo.Height);

        public byte[] PixelBuffer
        {
            get
            {
                if ((_data.Length == 0) || (_data == null))
                {
                    GetBytes();
                    return _data!;
                }
                return _data;
            }
        }

        public ImageChannel Channel => _rawInfo.Channel;

        /// <summary>
        ///   Hash code of time pixels updated last time.
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

        public int PixelSize => ImageInfo.GetPixelSize(_rawInfo.pixelLayout, _rawInfo.Channel);

        public PixelChannelLayout PixelLayout
        {
            get => _rawInfo.pixelLayout;
            private set => _rawInfo.pixelLayout = value;
        }

        internal ref byte[] Data => ref _data;

        public void FromBytes(
                    byte[] stream
        ) { }

        public static unsafe Image FromFile(
                                   string route,
                                   ImageLoader loader
        )
        {
            Image ret = new Image();
            fixed (ImageInfo* iptr = &ret._rawInfo) {
                GraphicFramework.InternalLoadImage(route, iptr, loader);
            }
            return ret;
        }

        public static Image FromMemory(
                            (int width, int height) size,
                            ImageChannel channel,
                            byte[] stream
        )
        {
            ImageInfo _info = new ImageInfo
            {
                Height = size.height,
                Width = size.width,
                Channel = channel,
                StreamPtr = IntPtr.Zero
            };
            Image ret = new Image(_info);
            ret.PixelLayout = PixelChannelLayout.Byte;
            ret._data = stream;
            return ret;
        }

        public static Image FromMemory(
                            (int width, int height) size,
                            ImageChannel channel,
                            PixelChannelLayout layout,
                            byte[] stream
        )
        {
            ImageInfo _info = new ImageInfo
            {
                Height = size.height,
                Width = size.width,
                Channel = channel,
                StreamPtr = IntPtr.Zero
            };
            Image ret = new Image(_info);
            ret.PixelLayout = layout;
            ret._data = stream;
            return ret;
        }

        public unsafe byte[] GetBytes()
        {
            if (_data.Length == 0)
            {
                int size = Width * Height * ImageInfo.GetPixelSize(PixelLayout, Channel);
                _data = new Span<byte>((byte*)_rawInfo.StreamPtr, size).ToArray();
                fixed (ImageInfo* pptr = &_rawInfo) {
                    GraphicFramework.InternalUnloadImage(pptr);
                }
                _rawInfo.StreamPtr = IntPtr.Zero;
            }
            return _data;
        }

        public Image ProvideImage()
        {
            return this;
        }

        internal static Image FromMemory(
                              ImageInfo info,
                              byte[] data,
                              int operationCount
        )
        {
            return new Image
            {
                _rawInfo = info,
                _data = data,
                _pixelUpdateCount = operationCount
            };
        }

    }
}