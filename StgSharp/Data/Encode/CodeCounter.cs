//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="CodeCounter.cs"
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Data
{
    public static class CodeSeparator
    {

        public static List<(T, ulong)> CountCodeFrequency<T>(T[] rawArray) where T : struct
        {
            Dictionary<T, ulong> frequencyCollection = new Dictionary<T, ulong>();

            foreach (var item in rawArray)
            {
                if (frequencyCollection.ContainsKey(item))
                {
                    frequencyCollection[item] += 1;
                }
                else
                {
                    frequencyCollection.Add(item, 1);
                }
            }

            return frequencyCollection.OrderByDescending(pair => pair.Value)
                .Select(pair => (pair.Key, pair.Value)).ToList();
        }
        public static unsafe T[] SeparateCode<T>(string filePath) where T : struct
        {
            byte[] allByte = File.ReadAllBytes(filePath);
            List<T> rawList = new List<T>();

            fixed (byte* allBytePtr = allByte)
            {
                for (int i = 0; i < allByte.Length; i += Marshal.SizeOf<T>())
                {
                    T t = new T();
                    t = *(T*)(allBytePtr + i);
                    rawList.Add(t);
                }
            }
            return rawList.ToArray();
        }

        public static unsafe T[] SeparateCode<T>(byte[] stream) where T : struct
        {
            List<T> rawList = new List<T>();

            fixed (byte* allBytePtr = stream)
            {
                for (int i = 0; i < stream.Length; i += Marshal.SizeOf<T>())
                {
                    T t = new T();
                    t = *(T*)(allBytePtr + i);
                    rawList.Add(t);
                }
            }
            return rawList.ToArray();
        }

    }
}
