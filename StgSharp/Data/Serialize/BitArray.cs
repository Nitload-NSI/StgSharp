//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="BitArray.cs"
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
using System.IO;
using System.Linq;
using System.Text;

namespace StgSharp.Data
{
    public static partial class Serializer
    {

        public static void Deserialize(byte[] stream, out BitArray target)
        {
            List<bool> boolStream = new List<bool>(4 * stream.Length);
            int i = 0, count = 0;
            foreach (byte b in stream)
            {
                uint raw = b;
                for (i = 0; i < 4; i++)
                {
                    switch (raw & 0b11)
                    {
                        case (uint)BoolMap.True_0:
                            boolStream.Add(true);
                            break;
                        case (uint)BoolMap.False_0:
                            boolStream.Add(false);
                            break;
                        case (uint)BoolMap.None_0:
                            goto end;
                        default:
                            throw new Exception();
                    }
                    raw >>= 2;
                    count++;
                    continue;
                end:
                    {
                        break;
                    }
                }
            }
            while (boolStream.Count > count)
            {
                boolStream.RemoveAt(count);
            }
            target = new BitArray(boolStream.ToArray());
        }

        /// <summary>
        /// Serialize a <see cref="BitArray"/>, useful when writing 
        /// a Huffman tree index dictionary to file.
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static byte[] Serialize(BitArray bits)
        {
            int size = (bits.Count / 4) + 1;
            List<byte> ret = new List<byte>(size);
            int count = 0, total = 0; byte temp = new byte();
            for (int i = 0; i < bits.Count; i++)
            {
                if (count == 4)
                {
                    ret.Add(temp);
                    count = 0;
                    temp = 0;
                    total++;
                }
                switch (count)
                {
                    case 0:
                        temp |= (byte)(bits[i] ? BoolMap.True_0 : BoolMap.False_0);
                        break;
                    case 1:
                        temp |= (byte)(bits[i] ? BoolMap.True_1 : BoolMap.False_1);
                        break;
                    case 2:
                        temp |= (byte)(bits[i] ? BoolMap.True_2 : BoolMap.False_2);
                        break;
                    case 3:
                        temp |= (byte)(bits[i] ? BoolMap.True_3 : BoolMap.False_3);
                        break;
                    default:
                        throw new Exception();
                }
                count++;
            }
            for (; count < 4; count++)
            {
                switch (count)
                {
                    case 1:
                        temp |= (byte)BoolMap.None_1;
                        break;
                    case 2:
                        temp |= (byte)BoolMap.None_2;
                        break;
                    case 3:
                        temp |= (byte)BoolMap.None_3;
                        break;
                    default:
                        throw new Exception();
                }
                continue;
            }
            ret.Add(temp);
            return ret.ToArray();
        }

    }


    internal enum BoolMap : byte
    {
        False_0 = 0b00000010,
        True_0 = 0b00000001,
        None_0 = 0b00000011,
        False_1 = 0b00001000,
        True_1 = 0b00000100,
        None_1 = 0b00001100,
        False_2 = 0b00100000,
        True_2 = 0b00010000,
        None_2 = 0b00110000,
        False_3 = 0b10000000,
        True_3 = 0b01000000,
        None_3 = 0b11000000,
    }
}
