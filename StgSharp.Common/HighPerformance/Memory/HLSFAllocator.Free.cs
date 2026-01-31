//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="HLSFAllocator.Free"
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
using System.Buffers;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using hlsfHandle = StgSharp.HighPerformance.Memory.HybridLayerSegregatedFitAllocationHandle;

namespace StgSharp.HighPerformance.Memory
{
    public enum FreePolicy
    {

        FullCollect,
        LazyCollect,
        NoCollect

    }

    public unsafe partial class HybridLayerSegregatedFitAllocator
    {

        /// <summary>
        ///   Minimal collect: linear scan, merge contiguous empty Blocks into one, push merged head
        ///   back to bucket. Caller-responsible design: no slicing or redistribution.
        /// </summary>
        public void Collect()
        {
            Entry* cur = _spareMemory->NextNear;
            long size = 0;
            while (cur != null && cur != _spareMemory)
            {
                // cur is not empty, skip
                if (cur->State != EntryState.Empty)
                {
                    cur = cur->NextNear;
                    continue;
                }

                // remove cur from bucket
                do
                {
                    size += cur->Size;
                    Entry* next = cur->NextNear;
                    RemoveFromBucket(cur->Level, DetermineSegmentIndex(_levelSizeArray, cur->Size, cur->Level), (BucketNode*)cur->Position);
                    RemoveFromPositionChain(cur);
                    cur = next;
                } while (cur != null && cur != _spareMemory && cur->State == EntryState.Empty);
                if (size > 0) {
                    SliceMemory(cur, size);
                }
            }
        }

        /// <summary>
        ///   Free allocated memory block and perform adjacent block merging
        /// </summary>
        /// <param name="handle">
        ///   Handle to the allocated memory block
        ///  <param name="collectPolicy">
        /// </param>
        public void Free(
                    hlsfHandle handle,
                    FreePolicy collectPolicy = FreePolicy.NoCollect
        )
        {
            Entry* e = handle.EntryHandle;
            if (e == null) {
                return;
            }
            if (e->State != EntryState.Alloc) {
                return;
            }

            // Mark as empty so it can be merged
            e->State = EntryState.Empty;

            // Try merge with adjacent free blocks and remove them from buckets
            if (collectPolicy == FreePolicy.LazyCollect)
            {
                MergeAndRemoveFromBuckets(e);
            }

            // Calculate final level and segment for the merged block
            long finalSize = e->Size;
            int finalLevel = GetLevelFromSize(finalSize);
            e->Level = finalLevel;
            int finalSegment = DetermineSegmentIndex(_levelSizeArray, finalSize, finalLevel);

            // Push the merged entry back to appropriate bucket
            PushBucketToLevel(finalLevel, finalSegment, (BucketNode*)e->Position);

            // Console.WriteLine("one block freed");
            if (collectPolicy == FreePolicy.FullCollect)
            {
                Collect();
            }
        }

    }
}
