//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="L4.CacheLine"
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
using StgSharp.HighPerformance.ProcessorAbstraction;
using System.Runtime.InteropServices;

using Hlsf = StgSharp.HighPerformance.Memory.HybridLayerSegregatedFitAllocator;

namespace StgSharp.HighPerformance.Memory
{
    public partial class L4
    {

        [InlineArray(CacheLineCount)]
        private struct PredictionArray
        {

            private int PredictionCount;

        }

        [InlineArray(CacheLineCount)]
        private struct MapArray
        {

            private int MapCount;

        }

        [StructLayout(LayoutKind.Explicit, Size = 64, Pack = 16)]
        private unsafe struct CacheLineHead
        {

            [FieldOffset(0)] internal Hlsf.Entry AllocatorEntry;
            [FieldOffset(0)] internal readonly M512 Buffer;

            #region reference counting and state information

            [FieldOffset(52)] internal int RefCount;
            [FieldOffset(60)] private readonly int Reverse;
            [FieldOffset(56)] internal readonly int Id;

            #endregion


            #region hlsf entry alias

            [FieldOffset(24)] public readonly nuint Address;
            [FieldOffset(16)] public readonly uint Size;

            #endregion

            #region cache line information

            [FieldOffset(50)] internal short Predictor;
            [FieldOffset(48)] internal ushort Profile;
            [FieldOffset(40)] internal nuint Origin;

        #endregion
        }

    }
}
