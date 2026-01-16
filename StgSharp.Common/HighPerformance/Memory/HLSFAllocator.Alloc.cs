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
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Numerics;
using System.Runtime.InteropServices;
using hlsfHandle = StgSharp.HighPerformance.Memory.HybridLayerSegregatedFitAllocationHandle;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class HybridLayerSegregatedFitAllocator
    {

        public hlsfHandle Alloc(
                          uint size
        )
        {
            if (size > _maxSize) {
                throw new InvalidOperationException("Requested size exceeds maximum allocatable size.");
            }

            int level = GetLevelFromSize(size);
            int segmentIndex = DetermineSegmentIndex(_levelSizeArray, size, level);

            int i = level, s = segmentIndex;
            Entry* handle;
            while (!TryPopLevelForAllocation(i, s, out handle))
            {
                if (++s >= 3)
                {
                    s = 0;
                    i++;
                }
                if (i > MaxLevel)
                {
                    if (TryPopLevelForAllocation(10, 1, out handle))
                    {
                        break;
                    } else if (TryAllocateNew32MBBlock(out handle))
                    {
                        break;
                    }
                    throw new OverflowException("Out of memory: Unable to allocate new 16MB block or find suitable bucket for allocation.");
                }
            }

            if (handle != null)
            {
                int requestedOffset = (segmentIndex + 1) * _levelSizeArray[level];
                if (requestedOffset > handle->Size) {
                    throw new OverflowException("Out of memory: insufficient block size returned from spare memory.");
                }
                long remainder = handle->Size - requestedOffset;
                SetEntryLevel(handle, handle->Position, (uint)requestedOffset);
                handle->State = EntryState.Alloc;
                if (remainder > 0) {
                    SliceMemory(handle->NextNear, remainder);
                }
            }
            return new hlsfHandle(handle, size);
        }

        /// <summary>
        ///   Slice the memory block into smaller blocks and add them to the appropriate buckets
        ///   Notice: this method is not responsible for freeing the original BucketNode
        /// </summary>
        /// <param name="e">
        ///
        /// </param>
        /// <param name="sizeToSlice">
        ///
        /// </param>
        private void SliceMemory(
                     Entry* e,
                     long sizeToSlice
        )
        {
            Entry* next = e;
            long remain = sizeToSlice;                                          // remained size to slice
            long offset = (long)e->Position - sizeToSlice - (long)m_Buffer;     // offset of buffer to slice
            long end = (long)e->Position - (long)m_Buffer;                      // end offset of buffer to slice

            int i = (BitOperations.TrailingZeroCount(offset) - 6) / 2;
            i = int.Min(10, i);
            for (; i < _levelSizeArray.Length; i++)
            {
                int size = _levelSizeArray[i];
                int upper = size * 4;
                long edge = (offset + upper - 1) / upper * upper;
                int count = (int)((long.Min(edge, end) - offset) / size);
                if (count > 0)
                {
                    long newSize = count * size;

                    Entry* newEntry = (Entry*)_entries.Allocate();
                    InsertEntryBefore(newEntry, next);
                    SetEntryLevel(newEntry, (nuint)offset + (nuint)m_Buffer, newSize);
                    newEntry->State = EntryState.Empty;

                    BucketNode* b = (BucketNode*)newEntry->Position;
                    b->EntryRef = newEntry;
                    PushBucketToLevel(i, count - 1, b);
                    offset += newSize;
                    remain -= newSize;
                }
                if (remain <= 0) {
                    return;
                }
            }

            if (i > MaxLevel)
            {
                int size = _levelSizeArray[^1];
                int upper = size * 4;
                long edge = end / upper * upper;
                int count = (int)((long.Min(edge, end) - offset) / upper);
                if (count > 0)
                {
                    long newSize = (long)count * (long)upper;

                    Entry* newEntry = (Entry*)_entries.Allocate();
                    InsertEntryBefore(newEntry, next);
                    SetEntryLevel(newEntry, (nuint)offset + (nuint)m_Buffer, newSize);
                    newEntry->State = EntryState.Empty;

                    BucketNode* b = (BucketNode*)newEntry->Position;
                    b->EntryRef = newEntry;
                    PushBucketToLevel(i, count - 1, b);
                    offset += newSize;
                    remain -= newSize;
                    if (remain <= 0) {
                        return;
                    }
                }
            }

            for (i--; i >= 0; i--)
            {
                int size = _levelSizeArray[i];
                long edge = end / size * size;
                int count = (int)((long.Min(edge, end) - offset) / size);
                if (count > 0)
                {
                    long newSize = count * size;

                    Entry* newEntry = (Entry*)_entries.Allocate();
                    InsertEntryBefore(newEntry, next);
                    SetEntryLevel(newEntry, (nuint)offset + (nuint)m_Buffer, newSize);
                    newEntry->State = EntryState.Empty;

                    BucketNode* b = (BucketNode*)newEntry->Position;
                    b->EntryRef = newEntry;
                    b->NextLevel = null;
                    b->PreviousLevel = null;
                    PushBucketToLevel(i, count - 1, b);
                    offset += newSize;
                    remain -= newSize;
                }
                if (remain <= 0) {
                    return;
                }
            }
        }

        /// <summary>
        ///   Try to allocate a new 16MB block when all buckets are empty
        /// </summary>
        /// <param name="handle">
        ///   Output handle to the allocated Entry
        /// </param>
        /// <returns>
        ///   True if allocation succeeded
        /// </returns>
        private bool TryAllocateNew32MBBlock(
                     out Entry* handle
        )
        {
            nuint pos = _spareMemory->Position;
            long remain = _spareMemory->Size;
            if (remain <= 0)
            {
                handle = null;
                return false;
            }
            long size = remain > 32 * 1024 * 1024L ? 32 * 1024 * 1024L : remain;


            // build new entry
            Entry* e = (Entry*)_entries.Allocate();
            SetEntryLevel(e, pos, size);
            InsertEntryBefore(e, _spareMemory);
            e->State = EntryState.Empty;

            // set fields of spare memory
            _spareMemory->Size -= size;
            _spareMemory->Position += (nuint)size;


            handle = e;
            return true;
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
