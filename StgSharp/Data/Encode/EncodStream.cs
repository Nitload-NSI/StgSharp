//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="EncodStream.cs"
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Data
{
    public class EncodeStream
    {

        byte[] buffer;
        BitArray bufferArray;
        byte count;
        List<byte> data;

        public EncodeStream()
        {
            count = 0;
            buffer = new byte[1];
            data = new List<byte>();
            bufferArray = new BitArray(8, true);
        }

        public List<byte> EncodeData
=> data;

        public int Length => data.Count;

        public string Output => Encoding.UTF8.GetString(data.ToArray());

        public void WriteBits<T>(HuffmanKey<T> key) where T : struct
        {
            BitArray bit = key.GetBitArray();
            for (int i = 0; i < bit.Count; i++)
            {
                if (count == 8)
                {
                    bufferArray.CopyTo(buffer, 0);
                    data.Add(buffer[0]);
                    count = 0;
                }
                bufferArray[count] = bit[i];
                count++;
            }
        }

    }


}
