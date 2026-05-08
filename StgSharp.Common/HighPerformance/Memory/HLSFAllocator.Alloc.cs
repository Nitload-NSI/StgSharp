//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="HLSFAllocator.Alloc"
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
using System.IO;
using System.Numerics;
using System.Reflection.Metadata;
using hlsfHandle = StgSharp.HighPerformance.Memory.HlsfHandle;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class HybridLayerSegregatedFitAllocator
    {

        public partial HlsfHandle Alloc(
                                  nuint size
        )
        {
            if (size > _maxAllocSize) {
                throw new InvalidOperationException("Requested size exceeds maximum allocatable size.");
            }
            /*
            int level = GetLevelFromSize(size);
            int segmentIndex = GetSegmentFromLevel(size, level);

            int i = level, s = segmentIndex;
            /**/

            Entry* entry = null;
            nuint requestedOffset = GetAllocSize(size);
            (int level, int segment) = GetIndexPairOnce(size);
            int index = (3 * level) + segment;
            int i = index;
            while ((i < _bucketHeads.Length) && !TryPopLevelForAllocation(index, out entry)) {
                i++;
            }
            if ((i > _maxLevel) && !TryAllocateNewBlock(requestedOffset, out entry)) {
                throw new OverflowException("Out of memory: Unable to allocate new 16MB block or find suitable bucket for allocation.");
            }
            if (entry != null)
            {
                if (requestedOffset > entry->Size) {
                    throw new OverflowException("Out of memory: insufficient block size returned from spare memory.");
                }
                nuint remainder = entry->Size - requestedOffset;
                SetEntryLevel(entry, entry->Position, (uint)requestedOffset);
                entry->State = EntryState.Alloc;
                if (remainder > 0) {
                    OrganizeMemory(entry->NextNear, remainder);
                }
            }
            return new(entry, size);
        }

        public bool TryAlloc(
                    nuint size,
                    out HlsfHandle handle
        )
        {
            if (size > _maxAllocSize)
            {
                handle = default;
                return false;
            }
            /*
            int level = GetLevelFromSize(size);
            int segmentIndex = GetSegmentFromLevel(size, level);

            int i = level, s = segmentIndex;
            /**/

            Entry* entry = null;
            nuint requestedOffset = GetAllocSize(size);
            (int level, int segment) = GetIndexPairOnce(size);
            int index = (3 * level) + segment;
            int i = index;
            while ((i < _bucketHeads.Length) && !TryPopLevelForAllocation(index, out entry)) {
                i++;
            }
            if ((i > _maxLevel) && !TryAllocateNewBlock(requestedOffset, out entry)) {
                throw new OverflowException("Out of memory: Unable to allocate new 16MB block or find suitable bucket for allocation.");
            }

            if (entry != null)
            {
                if (requestedOffset > entry->Size) {
                    throw new OverflowException("Out of memory: insufficient block size returned from spare memory.");
                }
                nuint remainder = entry->Size - requestedOffset;
                SetEntryLevel(entry, entry->Position, (uint)requestedOffset);
                entry->State = EntryState.Alloc;
                if (remainder > 0) {
                    OrganizeMemory(entry->NextNear, remainder);
                }
            }
            handle = new(entry, size);
            return true;
        }

        /// <summary>
        ///   Slice the memory block into smaller blocks and add them to the appropriate buckets.
        ///   Notice: this method is not responsible for freeing the original BucketNode
        /// </summary>
        /// <param name="e">
        ///   The next entry nearby the part to slice
        /// </param>
        /// <param name="sizeToSlice">
        ///
        /// </param>
        private void OrganizeMemory(
                     Entry* e,
                     nuint sizeToSlice
        )
        {
            Entry* next = e;
            nuint remain = sizeToSlice;                                          // remained size to slice
            nuint offset = e->Position - sizeToSlice - ((nuint)m_Buffer);     // offset of buffer to slice
            nuint end = e->Position - ((nuint)m_Buffer);                      // end offset of buffer to slice

            int i = (BitOperations.TrailingZeroCount(offset) - 6) / 2;
        }

        /// <summary>
        ///   Try to allocate a new 16MB block when all buckets are empty
        /// </summary>
        /// <param name="size">
        ///   Size of memory block to be allocated. Notice: Size is assumed as times of 64.
        /// </param>
        /// <param name="handle">
        ///   Output handle to the allocated Entry
        /// </param>
        /// <returns>
        ///   True if allocation succeeded
        /// </returns>
        private bool TryAllocateNewBlock(
                     nuint size,
                     out Entry* handle
        )
        {
            nuint pos = _spareMemory->Position;
            nuint remain = _spareMemory->Size;
            if (remain <= 0)
            {
                handle = null;
                return false;
            }
            size = nuint.Min(size, remain);


            // build new entry
            Entry* e = (Entry*)_entries.Allocate();
            SetEntryLevel(e, pos, size);
            InsertEntryBefore(e, _spareMemory);
            e->State = EntryState.Empty;

            // set fields of spare memory
            _spareMemory->Size -= size;
            _spareMemory->Position += size;


            handle = e;
            return true;
        }

        /// <summary>
        ///   Helper method for allocation - converts BucketNode to Entry
        /// </summary>
        private bool TryPopLevelForAllocation(
                     int bucketIndex,
                     out Entry* entry
        )
        {
            if (TryPopBucket(bucketIndex, out BucketNode* bucketNode))
            {
                entry = bucketNode->EntryRef;
                return entry != null;
            }
            entry = null;
            return false;
        }

        /// <summary>
        ///   Helper method for allocation - converts BucketNode to Entry
        /// </summary>
        private bool TryPopLevelForAllocation(
                     int levelIndex,
                     int sizeIndex,
                     out Entry* entry
        )
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
