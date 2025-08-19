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
using StgSharp.Threading;

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

        private BufferExpansionLock _spareLock = new();

        public hlsfHandle Alloc(uint size)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan<nuint>(size, (nuint)levelSize[^1] * 2);
            int level = GetLevelFromSize(size);
            int segmentIndex = DetermineSegmentIndex(size, level);

            int i = level, s = segmentIndex;
            if (TryPopLevelForAllocation(i, s, out Entry* handle))
            {
                handle->State = EntryState.Allocated;
                return new hlsfHandle(handle, size);
            }

            fallback:
            i = level;
            s = segmentIndex;
            while (!TryPopLevelForAllocation(i, s, out handle))
            {
                if (++s >= 3)
                {
                    s = 0;
                    i++;
                }
                if (i >= levelSize.Length)
                {
                    if (TryAllocateNew16MBBlock(out handle))
                    {
                        break;
                    }
                    Thread.Sleep(0);
                    goto fallback;
                }
            }

            if (handle != null)
            {
                SliceMemory(handle, (segmentIndex + 1) * levelSize[level]);
                handle->State = EntryState.Allocated;
            }

            return new hlsfHandle(handle, size);
        }

        private void SliceMemory(Entry* e, int offset) { }

        /// <summary>
        ///   Try to allocate a new 16MB block when all buckets are empty
        /// </summary>
        /// <param name="handle">
        ///   Output handle to the allocated Entry
        /// </param>
        /// <returns>
        ///   True if allocation succeeded
        /// </returns>
        private bool TryAllocateNew16MBBlock(out Entry* handle)
        {
            nuint pos = Volatile.Read(ref _spareMemory->Position);
            nuint blockSize = (nuint)levelSize[^1] * 2; // Use last level size (16MB)
            nuint newPos = pos + blockSize;


            // Check if we have enough space
            if (newPos > ((nuint)m_Buffer) + _size)
            {
                handle = null;
                return false; // Out of memory
            }

            // Try to allocate new Entry from slab allocator
            try
            {
                handle = (Entry*)_nodes.Allocate();
                handle->Position = pos;
                handle->Size = (uint)blockSize;
                handle->State = EntryState.ThreadOccupied;
                handle->NextNear = EmptyHandle;
                handle->PreviousNear = EmptyHandle;
                handle->SpinLock = 0;


                // Update spare memory position
                _spareMemory->Position = newPos;

                return true;
            }
            catch
            {
                handle = null;
                return false;
            }
        }

        /// <summary>
        ///   Helper method for allocation - converts BucketNode to Entry
        /// </summary>
        private bool TryPopLevelForAllocation(int levelIndex, int sizeIndex, out Entry* entry)
        {
            if (TryPopLevel(levelIndex, sizeIndex, out BucketNode* bucketNode))
            {
                entry = bucketNode->EntryRef;
                return entry != null;
            }
            entry = null;
            return false;
        }

    }
}
