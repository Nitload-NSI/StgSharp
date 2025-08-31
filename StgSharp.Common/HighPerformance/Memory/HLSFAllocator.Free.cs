//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="HLSFAllocator.Free.cs"
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
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using hlsfHandle = StgSharp.HighPerformance.Memory.HybridLayerSegregatedFitAllocationHandle;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class HybridLayerSegregatedFitAllocator
    {

        /// <summary>
        ///   Free allocated memory block and perform adjacent block merging
        /// </summary>
        /// <param name="handle">
        ///   Handle to the allocated memory block
        /// </param>
        public void Free(hlsfHandle handle)
        {
            Entry* e = handle.EntryHandle;
            if (e == null) {
                return;
            }
            if (e->State != EntryState.Allocated) {
                return;
            }

            // Set to ThreadOccupied to prevent other operations
            e->State = EntryState.Empty;

            // Try merge with adjacent free blocks and remove them from buckets
            MergeAndRemoveFromBuckets(e);

            // Calculate final level and segment for the merged block
            uint finalSize = e->Size;
            int finalLevel = GetLevelFromSize(finalSize);
            int finalSegment = DetermineSegmentIndex(finalSize, finalLevel);

            // Set the merged entry state to Empty (ready to be reused)
            e->State = EntryState.Empty;

            // Push the merged entry back to appropriate bucket
            PushLevel(finalLevel, finalSegment, (BucketNode*)e->Position);
        }

    }
}
