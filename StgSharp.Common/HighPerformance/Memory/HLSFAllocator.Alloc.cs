//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="HLSFAllocator.Alloc"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
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
using Microsoft.Win32;

using StgSharp.Threading;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using hlsfHandle = StgSharp.HighPerformance.Memory.HybridLayerSegregatedFitAllocationHandle;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class HybridLayerSegregatedFitAllocator
    {

        public hlsfHandle Alloc(uint size)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan<nuint>(size, (nuint)levelSizeArray[^1] * 2);
            int level = GetLevelFromSize(size);
            int segmentIndex = DetermineSegmentIndex(size, level);

            int i = level, s = segmentIndex;
            Entry* handle;
            while (!TryPopLevelForAllocation(i, s, out handle))
            {
                if (++s >= 3)
                {
                    s = 0;
                    i++;
                }
                if (i >= levelSizeArray.Length)
                {
                    if (TryAllocateNew16MBBlock(out handle))
                    {
                        break;
                    }
                    throw new OverflowException("Out of memory: Unable to allocate new 16MB block or find suitable bucket for allocation.");
                }
            }

            if (handle != null) {
                SliceMemory(handle, (segmentIndex + 1) * levelSizeArray[level]);
            }
            handle->State = EntryState.Allocated;
            Console.WriteLine($"handle alloced at {handle->Position}");
            return new hlsfHandle(handle, size);
        }

        private void SliceMemory(Entry* e, int offset)
        {
            Entry* current = e;
            ulong n = e->NextNear;
            Entry* next = e;

            ulong originOffset = e->Position - (ulong)m_Buffer;
            ulong finalOffset = originOffset + (ulong)offset;
            ulong end = e->Position + e->Size;
            int remainSize = (int)(e->Size - offset);

            int minLevel = int.Max((BitOperations.TrailingZeroCount(finalOffset) - 6) / 2, 0);
            int i = minLevel >= levelSizeArray.Length ? levelSizeArray.Length - 1 : minLevel;

            for (; i < levelSizeArray.Length; i++)
            {
                int levelSize = levelSizeArray[i];
                ulong board = (finalOffset + (ulong)levelSize - 1) & ~((ulong)levelSize - 1);
                int blockCount = (int)((ulong.Min(board, end) - board) / (ulong)levelSize);
                if (blockCount == 0)
                {
                    break;
                }
                int s = blockCount * levelSize;

                // create and init new entry
                Entry* newEntry = (Entry*)_entries.Allocate();
                newEntry->Position = (nuint)((ulong)m_Buffer + finalOffset);
                newEntry->Size = (uint)s;
                BucketNode* bucket = (BucketNode*)newEntry->Position;
                bucket->EntryRef = newEntry;

                // link new entry
                newEntry->PreviousNear = (ulong)current;
                newEntry->NextNear = n;
                current->NextNear = (ulong)newEntry;
                next->PreviousNear = (ulong)newEntry;

                newEntry->Level = i;
                PushLevel(i, blockCount - 1, bucket);

                // set new entry
                Entry* old = current;
                current = newEntry;

                finalOffset += (ulong)s;
                remainSize -= s;
            }

            if (remainSize <= 0) {
                return;
            }

            for (i -= 1; i >= 0; i--)
            {
                int levelSize = levelSizeArray[i];
                ulong board = (finalOffset + (ulong)levelSize - 1) & ~((ulong)levelSize - 1);
                int blockCount = (int)((ulong.Min(board, end) - finalOffset) / (ulong)levelSize);
                if (blockCount == 0)
                {
                    continue;
                }
                int s = blockCount * levelSize;

                // create and init new entry
                Entry* newEntry = (Entry*)_entries.Allocate();
                newEntry->Position = (nuint)((ulong)m_Buffer + finalOffset);
                newEntry->Size = (uint)s;
                BucketNode* bucket = (BucketNode*)newEntry->Position;
                bucket->EntryRef = newEntry;

                // link new entry
                newEntry->PreviousNear = (ulong)current;
                newEntry->NextNear = n;
                current->NextNear = (ulong)newEntry;
                if (next != null) {
                    next->PreviousNear = (ulong)newEntry;
                }

                newEntry->Level = i;
                PushLevel(i, blockCount - 1, bucket);

                // set new entry
                Entry* old = current;
                current = newEntry;

                // update lock and data
                finalOffset += (ulong)s;
                remainSize -= s;
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
        private bool TryAllocateNew16MBBlock(out Entry* handle)
        {
            nuint pos = _spareMemory->Position;
            nuint blockSize = (nuint)levelSizeArray[^1] * 2; // Use last level size (16MB)
            nuint newPos = pos + blockSize;

            ulong p;
            Entry* previous;
            p = _spareMemory->PreviousNear;
            previous = (Entry*)p;
            if (_spareMemory->Size <= 0)
            {
                handle = null;
                return false; // No spare memory available
            }

            // Check if we have enough space
            if (newPos > ((nuint)m_Buffer) + _size)
            {
                newPos = (nuint)m_Buffer + _size; // Clamp to maximum buffer size
                blockSize = _spareMemory->Size;
                p = 0;
            }

            // Try to allocate new Entry from slab allocator
            handle = (Entry*)_entries.Allocate();
            handle->Position = pos;
            handle->Size = (uint)blockSize;
            handle->Level = GetLevelFromSize((uint)blockSize);

            // Update spare memory position
            _spareMemory->Position = newPos;
            BucketNode* b = (BucketNode*)handle->Position;
            b->EntryRef = handle;
            PushLevel(handle->Level, DetermineSegmentIndex((uint)blockSize, handle->Level), b);

            // update position linked list
            handle->NextNear = (ulong)_spareMemory;
            handle->PreviousNear = p;
            _spareMemory->PreviousNear = (ulong)handle; // Replace Volatile.Write with direct access
            if (previous != null) {
                previous->NextNear = (ulong)handle;
            }

            return true;
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
