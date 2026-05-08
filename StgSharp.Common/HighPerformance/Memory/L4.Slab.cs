//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="L4.Slab"
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hlsf = StgSharp.HighPerformance.Memory.HybridLayerSegregatedFitAllocator;

namespace StgSharp.HighPerformance.Memory
{
    public partial class L4
    {

        private sealed unsafe class Slab : SlabAllocator<Hlsf.Entry>
        {

            private const int Capacity = L4.CacheLineCount;

            private int _maxUnused;
            private int _recycleCount;

            internal readonly CacheLineHead* _basePtr;
            internal readonly int* _indexReuse;

            public Slab(
                   nuint ptr
            )
            {
                _basePtr = (CacheLineHead*)ptr;
                _indexReuse = (int*)(_basePtr + Capacity);
                _maxUnused = 0;
                _recycleCount = 0;
            }

            public static nuint RequestedAllocation { get; } = ((nuint)(Unsafe.SizeOf<CacheLineHead>() + sizeof(int))) * Capacity;

            public override nuint Allocate()
            {
                if (_recycleCount > 0) {
                    return (nuint)(_basePtr + _indexReuse[_recycleCount - 1]);
                }
                if (_maxUnused >= Capacity) {
                    throw new OverflowException("Slab is full");
                }
                int index = _maxUnused;
                _maxUnused++;
                return (nuint)(_basePtr + index);
            }

            public override void Dispose()
            {
                GC.SuppressFinalize(this);
            }

            public override void EnterBufferReading() { }

            public override void ExitBufferReading() { }

            public override void Free(
                                 nuint index
            )
            {
                _indexReuse[_recycleCount] = (int)index;
                _recycleCount++;
            }

            public override SlabAllocationHandle<Hlsf.Entry> ReadAllocation()
            {
                return new(this, false);
            }

        }

    }
}
