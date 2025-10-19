//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ImageInfo"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate IntPtr ImageLoader(
                           string name,
                           ref int width,
                           ref int height,
                           ref int definedChannel,
                           int desiredChannel);

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
            set { channel = value; }
        }

        public int ChannelCount
        {
            get => GetChannelCount(channel);
        }

        public int Height
        {
            get => height;
            set => height = value;
        }

        public int StreamSize
        {
            get => width * height * GetPixelSize(
                pixelLayout, channel);
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

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 17 + width;
            hash = hash * 17 + height;
            hash = hash * 17 + (int)channel;
            return hash;
        }

        public override string ToString()
        {
            return $"{channel}\t{width}\t{height}\t{(ulong)StreamPtr}";
        }

        internal static int GetChannelCount(ImageChannel channel)
        {
            return channel switch
            {
                ImageChannel.SingleColor => 1,
                ImageChannel.DoubleColor => 2,
                ImageChannel.TripleColor => 3,
                ImageChannel.WithAlphaChannel => 4,
                _ => throw new InvalidCastException(
                    "Unknown color channel decoding setting.")
            };
        }

        internal static int GetPixelSize(PixelChannelLayout layout, ImageChannel channel)
        {
            return layout switch
            {
                PixelChannelLayout.UByte => sizeof(byte) * GetChannelCount(
                    channel),
                PixelChannelLayout.Byte => sizeof(byte) * GetChannelCount(
                    channel),
                PixelChannelLayout.UShort => sizeof(short) * GetChannelCount(
                    channel),
                PixelChannelLayout.Short => sizeof(short) * GetChannelCount(
                    channel),
                PixelChannelLayout.UInt => sizeof(int) * GetChannelCount(
                    channel),
                PixelChannelLayout.Int => sizeof(int) * GetChannelCount(
                    channel),
                PixelChannelLayout.Float => sizeof(float) * GetChannelCount(
                    channel),
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

    public enum ImageChannel : uint
    {

        SingleColor = glConst.RED,
        DoubleColor = glConst.RG,
        TripleColor = glConst.RGB,
        WithAlphaChannel = glConst.RGBA,

    }
}
