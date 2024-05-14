//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="DataHead.cs"
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace StgSharp.Data
{

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct SSDSegmentHead
    {

        [FieldOffset(0)] internal M128 data;

        [FieldOffset(0)] public int globalID;
        [FieldOffset(4)] public int nextID;
        [FieldOffset(8)] public int previousHash;


        [FieldOffset(12)] internal byte size;
        [FieldOffset(13)] public byte selfDefine_1;
        [FieldOffset(14)] public byte selfDefine_2;
        [FieldOffset(15)] public byte selfDefine_3;


        public int Size
        {
            get => ((int)size) + 1;
            set
            {
                if ((value > 256) || (value < 0))
                {
                    throw new ArgumentOutOfRangeException();
                }
                size = (byte)(value - 1);
            }
        }

        public unsafe byte[] GetBytes()
        {
            byte[] ret = new byte[16];
            fixed (Vector4* vptr = &data.vec)
            {
                byte* bptr = (byte*)vptr;
                for (int i = 0; i < 15; i++)
                {
                    ret[i] = *bptr;
                }
            }
            return ret;
        }

    }


}
