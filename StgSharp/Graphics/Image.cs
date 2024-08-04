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
using StgSharp.Data;
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

        internal byte[] data;
        internal ImageInfo info;

        public PixelChannelLayout PixelLayout
        {
            get => info.pixelLayout;
            private set => info.pixelLayout = value;
        }

        internal unsafe Image()
        {
            info = new ImageInfo();
        }

        internal Image(ImageInfo information)
        {
            info = information;
            data = Array.Empty<byte>();
            //int length = info.width * info.height * info.channel;
        }

        public (int width, int height) Size
        {
            get => (info.Width, info.Height);
        }

        public ImageChannel Channel => info.Channel;
        public int ChannelCount => info.ChannelCount;
        public int Height
        {
            get => info.Height;
            internal set => info.Height = value;
        }

        public int Width
        {
            get => info.Width;
            internal set => info.Width = value;
        }

        public int PixelSize => GlHelper.GetPixelSize(info.pixelLayout, info.Channel);

        public SerializableTypeCode SSDTypeCode => SerializableTypeCode.PixelImage;

        public void FromBytes(byte[] stream)
        {

        }

        public static unsafe Image FromFile(string route, ImageLoader loader)
        {
            Image ret = new Image();
            fixed (ImageInfo* iptr = &ret.info)
            {
                InternalIO.InternalLoadImage(route, iptr, loader);
            }
            return ret;
        }

        public static Image FromMemory((int width, int height) size, ImageChannel channel, PixelChannelLayout layout, byte[] stream)
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
            ret.data = stream;
            return ret;
        }

        public static Image FromMemory((int width, int height) size, ImageChannel channel, byte[] stream)
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
            ret.data = stream;
            return ret;
        }

        public unsafe byte[] GetBytes()
        {
                if (data.Length == 0)
                {
                    int size = Width * Height * GlHelper.GetPixelSize(PixelLayout, Channel);
                    data = new Span<byte>((byte*)info.StreamPtr, size).ToArray();
                    fixed (ImageInfo* pptr = &info)
                    {
                        InternalIO.InternalUnloadImage(pptr);
                    }
                    info.StreamPtr = IntPtr.Zero;
                }
                return data;
        }

        public byte[] PixelBuffer
        {
            get{
                if ((data.Length == 0) || (data == null))
                {
                    GetBytes();
                    return data!;
                }
                return data;
            }
        }
        public Image ProvideImage() => this;

        byte[] ISSDSerializable.GetBytes()
        {
            throw new NotImplementedException();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ImageInfo
    {

        private ImageChannel channel;
        private int height;
        private int width;
        private IntPtr pixelPtr;
        internal PixelChannelLayout pixelLayout;

        public ImageChannel Channel
        {
            get => channel;
            set
            {
                channel = value;
            }
        }

        public int ChannelCount
        {
            get => GlHelper.GetChannelCount(channel);
        }

        public int Height
        {
            get => height;
            set => height = value;
        }

        public int StreamSize
        {
            get => width * height * GlHelper.GetPixelSize(pixelLayout, channel);
        }

        public int Width
        {
            get => width;
            set => width = value;
        }

        public IntPtr StreamPtr
        {
            get => pixelPtr;
            internal set => pixelPtr = value;
        }

        public override string ToString()
        {
            return $"{channel}\t{width}\t{height}\t{(ulong)StreamPtr}";
        }

    }

    public enum ImageChannel : uint
    {
        SingleColor = GLconst.RED,
        DoubleColor = GLconst.RG,
        TripleColor = GLconst.RGB,
        WithAlphaChannel = GLconst.RGBA,
    }
}

namespace StgSharp.Graphics.OpenGL
{
    public static partial class GlHelper
    {

        internal static int GetChannelCount(ImageChannel channel)
        {
            return channel switch
            {
                ImageChannel.SingleColor => 1,
                ImageChannel.DoubleColor => 2,
                ImageChannel.TripleColor => 3,
                ImageChannel.WithAlphaChannel => 4,
                _ => throw new InvalidCastException("Unknown color channel decoding setting.")
            };
        }

        internal static int GetPixelSize(PixelChannelLayout layout, ImageChannel cahnnel)
        {
            return layout switch
            {
                PixelChannelLayout.UByte => sizeof(byte) * GetChannelCount(cahnnel),
                PixelChannelLayout.Byte => sizeof(byte) * GetChannelCount(cahnnel),
                PixelChannelLayout.UShort => sizeof(short) * GetChannelCount(cahnnel),
                PixelChannelLayout.Short => sizeof(short) * GetChannelCount(cahnnel),
                PixelChannelLayout.UInt => sizeof(int) * GetChannelCount(cahnnel),
                PixelChannelLayout.Int => sizeof(int) * GetChannelCount(cahnnel),
                PixelChannelLayout.Float => sizeof(float) * GetChannelCount(cahnnel),
                PixelChannelLayout.UByte332 => sizeof(byte),
                PixelChannelLayout.UByte233Rev => sizeof(byte),
                PixelChannelLayout.UShort565 => sizeof(short),
                PixelChannelLayout.UShort565Rev => sizeof(short),
                PixelChannelLayout.UShort4444 => sizeof(short),
                PixelChannelLayout.UShort4444Rev => sizeof(short),
                PixelChannelLayout.UShort5551 => sizeof(short),
                PixelChannelLayout.UShort1555Rev => sizeof(short),
                PixelChannelLayout.UInt8888 => sizeof(int),
                PixelChannelLayout.UInt8888Rev => sizeof(int),
                PixelChannelLayout.UInt1010102 => sizeof(int),
                PixelChannelLayout.UInt2101010Rev => sizeof(int),
                _ => throw new ArgumentException("Unknown layout type"),
            };
        }
    }
}