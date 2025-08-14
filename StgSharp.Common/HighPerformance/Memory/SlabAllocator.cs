//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="SlabAllocator.cs"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the “Software”), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//     
// The above copyright notice and 
// this permission notice shall be included in all copies 
// or substantial portions of the Software.
//     
// THE SOFTWARE IS PROVIDED “AS IS”, 
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
// ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.HighPerformance.Memory;
using StgSharp.Threading;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.Memory
{
    public abstract class SlabAllocator<T> : IDisposable where T: unmanaged
    {

        public abstract nuint Allocate();

        public static SlabAllocator<T> Create(nuint count, BufferLayout layout)
        {
            return layout switch
            {
                BufferLayout.Sequential => new SequentialSlabAllocator<T>(count),
                BufferLayout.Chunked => new ChunkedSlabAllocator<T>(count),
                _ => throw new ArgumentOutOfRangeException(nameof(layout), layout, null)
            };
        }

        public abstract void Dispose();

        public abstract void EnterBufferReading();

        public abstract void ExitBufferReading();

        public abstract void Free(nuint index);

        public abstract SlabAllocationHandle<T> ReadAllocation();

    }

    public enum BufferLayout
    {

        Sequential,
        Chunked,
        HybridLayerSegregatedFit

    }
}
