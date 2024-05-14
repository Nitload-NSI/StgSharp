//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Dictionary.cs"
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
using System.Text;

namespace StgSharp.Data
{

    /// <summary>
    /// A class contains a sets of methods to serialize several types
    /// </summary>
    public static partial class Serializer
    {
        /**/

        public static void Deserialize(byte[] data, out Dictionary<byte, BitArray> dict)
        {
            dict = new Dictionary<byte, BitArray>();
            int stat = 0; byte key = 0;
            byte[] raw = new byte[0];
            foreach (var b in data)
            {
                switch (stat)
                {
                    case 0: //getting key
                        if (raw.Length != 0)
                        {
                            BitArray bits;
                            Deserialize(raw, out bits);
                            dict.Add(key, bits);
                        }
                        key = b;
                        stat -= 1;
                        break;
                    case -1: //getting length
                        raw = new byte[b];
                        stat = b;
                        break;
                    default:
                        raw[raw.Length - stat] = b;
                        stat--;
                        break;
                }
            }
        }

        /**/
        public static byte[] Serialize(Dictionary<byte, BitArray> table)
        {
            using (var stream = new MemoryStream())
            {
                foreach (var pair in table)
                {
                    stream.WriteByte(pair.Key);

                    byte[] keyCode = Serialize(pair.Value);
                    stream.WriteByte((byte)keyCode.Length);

                    stream.Write(keyCode, 0, keyCode.Length);
                }
                return stream.ToArray();
            }
        }

    }
}
