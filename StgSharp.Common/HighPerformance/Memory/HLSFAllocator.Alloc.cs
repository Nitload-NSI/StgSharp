//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="HLSFAllocator.Alloc.cs"
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using hlsfHandle = StgSharp.HighPerformance.Memory.HybridLayerSegregatedFitAllocationHandle;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class HybridLayerSegregatedFitAllocator
    {

        private Entry remain = new Entry();
        private nuint nextUnused16MB = 0;

        public hlsfHandle Alloc(uint size)
        {
            int level = GetLevelFromSize(size), i = level;
            int segmentCount = (int)size / levelSize[level] + 1, s = segmentCount;

            if (TryDequeueLevel(i, s, out Entry* handle))
            {
                handle->State = EntryState.Allocated;
                return new hlsfHandle(handle, size);
            }
            while (!TryDequeueLevel(i, s, out handle))
            {
                if (++s > 3)
                {
                    s = 0;
                    i++;
                }
                if (i >= MaxLevel)
                {
                    AllocateNew16MBBlock();
                    break;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pos">
        ///
        /// </param>
        /// <returns>
        ///   Index to entry's location in slab allocator
        /// </returns>
        private nuint AllocateNew16MBBlock()
        {
            ulong pos = Volatile.Read(ref nextUnused16MB);
            Entry* entry = (Entry*)_nodes.Allocate();
            using (_nodes.ReadAllocation())
            {
                entry->State = EntryState.ThreadOccupied;
                entry->NextNear = EmptyHandle;
                nuint pIndex = (nuint)Volatile.Read(ref _bucketEntry[pos]);
                if (Interlocked.CompareExchange(ref _bucketEntry[pos], (nuint)entry,
                                                EmptyHandle) ==
                    pIndex)
                {
                    // 
                }
            }
        }

    }
}
