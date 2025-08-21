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

        private BufferExpansionLock _spareLock = new();

        public hlsfHandle Alloc(uint size)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan<nuint>(size, (nuint)levelSizeArray[^1] * 2);
            int level = GetLevelFromSize(size);
            int segmentIndex = DetermineSegmentIndex(size, level);

            int i = level, s = segmentIndex;
            if (TryPopLevelForAllocation(i, s, out Entry* handle))
            {
                SetEntryState(handle, EntryState.Allocated);
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
                if (i >= levelSizeArray.Length)
                {
                    if (!TryAllocateNew16MBBlock(out handle)) {
                        throw new OverflowException("Out of memory: Unable to allocate new 16MB block or find suitable bucket for allocation.");
                    }

                    Thread.Sleep(0);
                    goto fallback;
                }
            }

            try
            {
                if (handle != null)
                {
                    AcquireEntryNextLock(handle);
                    SliceMemory(handle, (segmentIndex + 1) * levelSizeArray[level]);
                    SetEntryState(handle, EntryState.Allocated);
                }

                return new hlsfHandle(handle, size);
            }
            finally
            {
                ReleaseEntryNextLock(handle);
            }
        }

        private void SliceMemory(Entry* e, int offset)
        {
            Entry* current = e;
            ulong n = e->NextNear;
            Entry* next = e;

            ulong originOffset = e->Position - (ulong)m_Buffer;
            ulong finalOffset = originOffset + (ulong)offset;
            int remainSize = (int)(e->Size - offset);

            AcquireEntryNextLock(e);
            AcquireEntryPrevLock(next);

            try
            {
                if ((originOffset & 63) != 0) {
                    throw new InvalidOperationException("Entry position offset is not aligned to 64 bytes.");
                }
                int minLevel = Math.Max((BitOperations.TrailingZeroCount(originOffset) - 6) / 2, 0);
                int i = minLevel >= levelSizeArray.Length ? levelSizeArray.Length - 1 : minLevel;
                for (; i < levelSizeArray.Length; i++)
                {
                    int levelSize = levelSizeArray[i];
                    int blockCount = remainSize / levelSize % 4;
                    if (blockCount == 0)
                    {
                        break;
                    }
                    int s = blockCount * levelSize;

                    // create and init new entry
                    Entry* newEntry = (Entry*)_entries.Allocate();
                    newEntry->Position = (nuint)((ulong)m_Buffer + finalOffset);
                    newEntry->Size = (uint)s;
                    newEntry->LockBuffer = 0;
                    BucketNode* bucket = (BucketNode*)newEntry->Position;
                    bucket->EntryRef = newEntry;
                    bucket->LockBuffer = 0;
                    newEntry->State = EntryState.ThreadOccupied;

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

                    // update lock and data
                    finalOffset += (ulong)s;
                    remainSize -= s;
                    ReleaseEntryNextLock(old);
                    AcquireEntryNextLock(current);
                }
                if (remainSize <= 0)
                {
                    ReleaseEntryNextLock(current);
                    ReleaseEntryPrevLock(next);
                    return;
                }
                for (; i >= 0; i--)
                {
                    int levelSize = levelSizeArray[i];
                    int blockCount = remainSize / levelSize % 4;
                    if (blockCount == 0)
                    {
                        continue;
                    }
                    int s = blockCount * levelSize;

                    // create and init new entry
                    Entry* newEntry = (Entry*)_entries.Allocate();
                    newEntry->LockBuffer = 0;
                    newEntry->Position = (nuint)((ulong)m_Buffer + finalOffset);
                    newEntry->Size = (uint)s;
                    BucketNode* bucket = (BucketNode*)newEntry->Position;
                    bucket->EntryRef = newEntry;
                    bucket->LockBuffer = 0;
                    newEntry->State = EntryState.ThreadOccupied;

                    // link new entry
                    newEntry->PreviousNear = (ulong)current;
                    current->NextNear = (ulong)newEntry;
                    newEntry->NextNear = n;
                    next->PreviousNear = (ulong)newEntry;

                    newEntry->Level = i;
                    PushLevel(i, blockCount - 1, bucket);

                    // set new entry
                    Entry* old = current;
                    current = newEntry;

                    // update lock and data
                    finalOffset += (ulong)s;
                    remainSize -= s;
                    ReleaseEntryNextLock(old);
                    AcquireEntryNextLock(current);
                    if (remainSize <= 0)
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                // Release locks in reverse order
                ReleaseEntryNextLock(current);
                ReleaseEntryPrevLock(next);
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
            nuint pos = Volatile.Read(ref _spareMemory->Position);
            nuint blockSize = (nuint)levelSizeArray[^1] * 2; // Use last level size (16MB)
            nuint newPos = pos + blockSize;

            ulong p;
            Entry* previous;
            do
            {
                p = Volatile.Read(ref _spareMemory->PreviousNear);
                previous = (Entry*)p;
                if (previous != null) {
                    AcquireEntryNextLock(previous);
                }
                AcquireEntryPrevLock(_spareMemory);
                break;
            } while (true);
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
            }

            // Try to allocate new Entry from slab allocator
            try
            {
                handle = (Entry*)_entries.Allocate();
                handle->Position = pos;
                handle->Size = (uint)blockSize;
                SetEntryState(handle, EntryState.ThreadOccupied);
                handle->Level = GetLevelFromSize((uint)blockSize);

                handle->PrevLock = 0;
                handle->NextLock = 0;

                // Update spare memory position
                _spareMemory->Position = newPos;
                BucketNode* b = (BucketNode*)handle->Position;
                b->EntryRef = handle;
                PushLevel(handle->Level, DetermineSegmentIndex((uint)blockSize, handle->Level), b);

                // update position linked list
                handle->NextNear = (ulong)_spareMemory;
                handle->PreviousNear = p;
                _spareMemory->PreviousNear = (ulong)handle;
                if (previous != null) {
                    previous->NextNear = (ulong)handle;
                }
                return true;
            }
            catch
            {
                handle = null;
                return false;
            }
            finally
            {
                ReleaseEntryNextLock(previous);
                ReleaseEntryPrevLock(_spareMemory);
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
