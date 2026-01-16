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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class HybridLayerSegregatedFitAllocator
    {

        /// <summary>
        ///   Check if two Entries are adjacent in memory
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool AssertAdjacentTo(
                            Entry* current,
                            Entry* other
        )
        {
            // Only a predicate; never throw here. Callers rely on false to skip merging.
            return other->Position + (nuint)other->Size == current->Position ||
                   current->Position + (nuint)current->Size == other->Position;
        }

        /// <summary>
        ///   check out if a bucket is within the same higher-level boundary as currentOffset
        /// </summary>
        private bool AssertBoundary(
                     Entry* previous,
                     Entry* current,
                     int level
        )
        {
            level = int.Min(level, MaxLevel);
            int nextLevelSize = _levelSizeArray[level] * 4;
            ulong prevHead = previous->Position - (ulong)m_Buffer,
                currHead = current->Position - (ulong)m_Buffer;
            return (currHead / (ulong)nextLevelSize) == (prevHead / (ulong)nextLevelSize);
        }

        /// <summary>
        ///   Determine segment index based on size and originLevel
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int DetermineSegmentIndex(
                           int[] levelSizeArray,
                           long size,
                           int level
        )
        {
            level = int.Min(level, MaxLevel);
            return (int)((size - 1) / levelSizeArray[level]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void InsertEntryAfter(
                            Entry* entry,
                            Entry* position
        )
        {
            Entry* next = position->NextNear;
            entry->NextNear = next;
            entry->PreviousNear = position;
            next->PreviousNear = entry;
            position->NextNear = entry;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void InsertEntryBefore(
                            Entry* entry,
                            Entry* position
        )
        {
            Entry* prev = position->PreviousNear;
            entry->NextNear = position;
            entry->PreviousNear = prev;
            prev->NextNear = entry;
            position->PreviousNear = entry;
        }

        /// <summary>
        ///   Used in Free operations, efficiently remove from buckets and merge adjacent blocks.
        ///   Current Entry will serve as the final merged block. The Merge will be in lazy
        ///   strategy.
        /// </summary>
        private void MergeAndRemoveFromBuckets(
                     Entry* entry
        )
        {
            if (entry == null) {
                return;
            }


            long size = entry->Size;
            int originLevel = entry->Level;
            int segment = DetermineSegmentIndex(_levelSizeArray, size, originLevel);
            do
            {
                // previous direction merge
                Entry* p = entry->PreviousNear;
                if (p == null ||
                    p == _spareMemory ||
                    p->State != EntryState.Empty ||
                    p->Level != entry->Level ||
                    !AssertBoundary(p, entry, originLevel))
                {
                    break;
                }

                // Console.WriteLine($"{(ulong)p->Size} merged to {(ulong)entry->Size} previous");
                entry->Size += p->Size;
                entry->Position = p->Position;
                RemoveFromBucket(originLevel, DetermineSegmentIndex(_levelSizeArray, p->Size, originLevel), (BucketNode*)p->Position);
                RemoveFromPositionChain(p);
            } while (true);

            do
            {
                // next direction merge
                Entry* n = entry->NextNear;
                if (n == null ||
                    n == _spareMemory ||
                    n->State != EntryState.Empty ||
                    n->Level != entry->Level ||
                    !AssertBoundary(n, entry, originLevel))
                {
                    break;
                }

                // Console.WriteLine($"{(ulong)n->Size} merged to {(ulong)entry->Size} next");
                entry->Size += n->Size;
                RemoveFromBucket(originLevel, DetermineSegmentIndex(_levelSizeArray, n->Size, originLevel), (BucketNode*)n->Position);
                RemoveFromPositionChain(n);
            } while (true);
            originLevel = int.Min(9, originLevel);
            entry->Level += entry->Size >= _levelSizeArray[originLevel] * 4 ? 1 : 0;
            entry->Level &= 15;

            BucketNode* bucket = (BucketNode*)entry->Position;
            bucket->EntryRef = entry;
        }

        /// <summary>
        ///   Remove node from position linked list, this method does not recycle a BucketNode from
        ///   size bucket
        /// </summary>
        private void RemoveFromPositionChain(
                     Entry* entry
        )
        {
            Entry* prev = entry->PreviousNear;
            Entry* next = entry->NextNear;

            prev->NextNear = next;
            next->PreviousNear = prev;

            // Clear node pointers
            entry->NextNear = null;
            entry->PreviousNear = null;

            // Console.WriteLine("entry recycled.");
            _entries.Free((nuint)entry);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetEntryLevel(
                            Entry* entry,
                            nuint position,
                            long size
        )
        {
            entry ->Position = position;
            entry->Size = size;
            entry->Level = GetLevelFromSize(size);
        }

        /// <summary>
        ///   Entry structure - streamlined version (only position and state information) Stored in
        ///   external SLAB
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size = 40)]
        internal struct Entry
        {

            [FieldOffset(0)]  internal Entry* NextNear;      // Position linked list: next pointer
            [FieldOffset(8)] internal Entry* PreviousNear;  // Position linked list: previous pointer

            [FieldOffset(24)] internal int Level;           // Level of the block in the allocator
            [FieldOffset(16)] internal long Size;           // Block size information
            [FieldOffset(36)] public EntryState State;
            [FieldOffset(28)]  public nuint Position;        // Memory block position

        }

        internal enum EntryState : uint
        {

            Spare,
            Empty,
            Alloc,
            Large

        }

    }
}
