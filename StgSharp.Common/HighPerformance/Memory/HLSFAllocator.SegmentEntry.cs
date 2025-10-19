//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="HLSFAllocator.SegmentEntry"
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
using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class HybridLayerSegregatedFitAllocator
    {

        private bool AssertBackwardBoundary(ulong currentOffset, Entry* target, int level)
        {
            int nextLevelSize = levelSizeArray[level] * 4;
            ulong targetHead = target->Position - (ulong)m_Buffer;
            return (currentOffset / (ulong)nextLevelSize) == (targetHead / (ulong)nextLevelSize);
        }

        private bool AssertForwardBoundary(ulong currentOffset, Entry* target, int level)
        {
            int nextLevelSize = levelSizeArray[level] * 4;
            ulong targetTail = target->Position - (ulong)m_Buffer + target->Size - 1;
            return (currentOffset / (ulong)nextLevelSize) == (targetTail / (ulong)nextLevelSize);
        }

        /// <summary>
        ///   Determine segment index based on size and originLevel
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int DetermineSegmentIndex(uint size, int level)
        {
            return (int)(size - 1) / levelSizeArray[level];
        }

        /// <summary>
        ///   Check if two Entries are adjacent in memory
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsAdjacentTo(Entry* current, Entry* other)
        {
            // Check if this immediately follows other
            return other->Position + other->Size == current->Position ||
                   current->Position + current->Size == other->Position;
        }

        /// <summary>
        ///   Used in Free operations, efficiently remove from buckets and merge adjacent blocks.
        ///   Current Entry will serve as the final merged block. The Merge will be in lazy
        ///   strategy, any fail in acquiring spin lock will end a merging process.
        /// </summary>
        private void MergeAndRemoveFromBuckets(Entry* entry)
        {
            if (entry == null) {
                return;
            }

            Entry* result = entry;
            uint size = entry->Size;
            int originLevel = entry->Level;
            int segment = DetermineSegmentIndex(size, originLevel);

            RemoveFromBucket(originLevel, segment, (BucketNode*)entry->Position);

            do
            {
                originLevel = entry->Level;
                while (true)
                {
                    // backward merging - simplified to single attempt
                    ulong p = entry->PreviousNear;
                    Entry* prev = (Entry*)p;


                    // 计算当前块相对于分配空间基址的偏移量
                    ulong currentOffset = entry->Position - (ulong)m_Buffer;

                    if (prev == null ||
                        prev == _spareMemory ||
                        !AssertBackwardBoundary(currentOffset, prev, originLevel) ||
                        !IsAdjacentTo(entry, prev) ||
                        entry->Level != prev->Level ||
                        !TryRemoveFromBucket(originLevel, DetermineSegmentIndex(prev->Size, originLevel), (BucketNode*)prev->Position))
                    {
                        break;
                    }

                    // Update linked list
                    Entry* prevPrev = (Entry*)prev->PreviousNear;
                    if (prevPrev != null) {
                        prevPrev->NextNear = (ulong)entry;
                    }
                    entry->PreviousNear = (ulong)prevPrev;

                    // Merge
                    entry->Position = prev->Position;
                    entry->Size += prev->Size;
                    RecycleEntry(prev);
                }

                while (true)
                {
                    // forward merging - simplified to single attempt
                    ulong n = entry->NextNear;
                    Entry* next = (Entry*)n;

                    ulong currentOffset = entry->Position - (ulong)m_Buffer;

                    if (next == null ||
                        next == _spareMemory ||
                        !AssertForwardBoundary(currentOffset, next, entry->Level) ||
                        !IsAdjacentTo(entry, next) ||
                        entry->Level != next->Level ||
                        !TryRemoveFromBucket(originLevel, DetermineSegmentIndex(next->Size, originLevel), (BucketNode*)next->Position))
                    {
                        break;
                    }

                    // Update linked list
                    Entry* nextNext = (Entry*)next->NextNear;
                    if (nextNext != null) {
                        nextNext->PreviousNear = (ulong)entry;
                    }
                    entry->NextNear = (ulong)nextNext;

                    // Merge
                    entry->Size += next->Size;
                    RecycleEntry(next);
                }
                entry->Level = GetLevelFromSize(entry->Size);
            } while (originLevel < entry->Level);
        }

        private void RecycleEntry(Entry* entry)
        {
            if (entry is null) {
                return;
            }

            entry->PreviousNear = EmptyHandle;
            entry->NextNear = EmptyHandle;
            _entries.Free((nuint)entry);
        }

        /// <summary>
        ///   Remove node from position linked list
        /// </summary>
        private void RemoveFromPositionChain(Entry* entry)
        {
            Entry* prev = (Entry*)entry->PreviousNear;
            Entry* next = (Entry*)entry->NextNear;

            if (prev != null) {
                prev->NextNear = entry->NextNear;
            }
            if (next != null) {
                next->PreviousNear = entry->PreviousNear;
            }

            // Clear node pointers
            entry->NextNear = EmptyHandle;
            entry->PreviousNear = EmptyHandle;
            _entries.Free((nuint)entry);
        }

        /// <summary>
        ///   Entry structure - streamlined version (only position and state information) Stored in
        ///   external SLAB
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size = 36)]
        internal struct Entry
        {

            [FieldOffset(28)] internal int Level;           // Level of the block in the allocator
            [FieldOffset(24)] internal uint Size;           // Block size information
            [FieldOffset(32)] internal ulong LockBuffer;
            [FieldOffset(8)]  internal ulong NextNear;      // Position linked list: next pointer
            [FieldOffset(16)] internal ulong PreviousNear;  // Position linked list: previous pointer
            [FieldOffset(32)] public EntryState State;
            [FieldOffset(0)]  public nuint Position;        // Memory block position

        }

        internal enum EntryState : uint
        {

            Empty,
            Allocated

        }

        #region Entry Operations

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNullOrEmptyEntryIndex(ulong index)
        {
            return index == EmptyHandle;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNullOrEmptyEntryIndex(Entry* entry)
        {
            return entry == null;
        }

        #endregion
    }
}
