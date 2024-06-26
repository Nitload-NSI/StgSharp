﻿//-----------------------------------------------------------------------
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
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics
{
    public class Image : SSDSerializable
    {

        internal ImageInfo info;
        internal byte[] data;
        internal glHandle pixelArrayHandle;

        internal Image(ImageInfo information)
        {
            info = information;
            int length = info.width * info.height * info.channel;
            pixelArrayHandle = glHandle.FromPtr(information.pixelPtr);
        }

        public unsafe Image()
        {
            info = new ImageInfo();
        }

        public int Channel => info.channel;
        public int Height => info.height;

        public int Width => info.width;

        public override SerializableTypeCode SSDTypeCode => SerializableTypeCode.PixelImage;

        ~Image()
        {
            if (info.pixelPtr!= IntPtr.Zero)
            {
             //   Marshal.FreeHGlobal(info.pixelPtr);
            }
        }

        public unsafe override byte[] GetBytes()
        {
            if (data == null)
            {
                int size = Width * Height * Channel + 1 + 8;
                data = new byte[size];
                int i = 0;
                fixed (byte* bptr = data)
                {
                    *(int*)bptr = Width;
                    *(int*)(bptr + 4) = Height;
                    *(bptr + 8) = (byte)Channel;
                    byte* pptr = bptr;
                    for (; i < size; i += 16)
                    {
                        *(M128*)(pptr + i) = *(M128*)(info.pixelPtr + i);
                    }
                    for (; i < size; i++)
                    {
                        *(pptr + i) = *(byte*)(info.pixelPtr + i);
                    }
                }
            }
            Marshal.FreeHGlobal(info.pixelPtr);
            info.pixelPtr = IntPtr.Zero;
            return data;
        }

        public unsafe override byte[] GetBytes(out int length)
        {
            if (data == null)
            {
                int size = Width * Height * Channel + 1 + 8;
                data = new byte[size];
                int i = 0;
                fixed (byte* bptr = data)
                {
                    *(int*)bptr = Width;
                    *(int*)(bptr + 4) = Height;
                    *(bptr + 8) = (byte)Channel;
                    byte* pptr = bptr;
                    for (; i < size; i += 16)
                    {
                        *(M128*)(pptr + i) = *(M128*)(info.pixelPtr + i);
                    }
                    for (; i < size; i++)
                    {
                        *(pptr + i) = *(byte*)(info.pixelPtr + i);
                    }
                }
            }
            length = data.Length;
            Marshal.FreeHGlobal(info.pixelPtr);
            info.pixelPtr = IntPtr.Zero;
            return data;
        }

        internal unsafe override void BuildFromByteStream(byte[] stream)
        {
            info = new ImageInfo();
            fixed (byte* bptr = stream)
            {
                info.width = *(int*)bptr;
                info.height = *(int*)(bptr + 4);
                info.channel = *(bptr + 8);
                info.pixelPtr = IntPtr.Zero;
            }
            data = stream;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ImageInfo
    {
        internal int width;
        internal int height;
        internal int channel;
        internal IntPtr pixelPtr;


        public override string ToString()
        {
            return $"{channel}\t{width}\t{height}\t{pixelPtr}";
        }
    }
}