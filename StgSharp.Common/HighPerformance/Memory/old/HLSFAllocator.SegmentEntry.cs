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
        /// <returns>
        ///   True if two mem blocks are adjacent.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool AssertAdjacentTo(
                            Entry* front,
                            Entry* back
        )
        {
            // Only a predicate; never throw here. Callers rely on false to skip merging.
            return front->Position + front->Size == back->Position;
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
                     Entry* entry,
                     int maxScanRange = 4
        )
        {
            if (entry == null) {
                return;
            }


            nuint size = entry->Size;
            int originLevel = entry->Level;
            int segment = GetSegmentFromLevel(size, originLevel);

            for (int i = 0; i < maxScanRange; i++)
            {
                // previous direction merge
                Entry* p = entry->PreviousNear;
                if ((p == null) ||
                    (p == _spareMemory) ||
                    (p->State != EntryState.Empty) ||
                    (p->Level != entry->Level) ||
                    !AssertAdjacentTo(p, entry))
                {
                    break;
                }

                // Console.WriteLine($"{(ulong)p->Size} merged to {(ulong)entry->Size} previous");
                entry->Size += p->Size;
                entry->Position = p->Position;
                RemoveFromBucket(originLevel, GetSegmentFromLevel(p->Size, originLevel), (BucketNode*)p->Position);
                RemoveFromPositionChain(p);
            }

            for (int i = 0; i < maxScanRange; i++)
            {
                // next direction merge
                Entry* n = entry->NextNear;
                if ((n == null) ||
                    (n == _spareMemory) ||
                    (n->State != EntryState.Empty) ||
                    (n->Level != entry->Level) ||
                    !AssertAdjacentTo(entry, n))
                {
                    break;
                }

                // Console.WriteLine($"{(ulong)n->Size} merged to {(ulong)entry->Size} next");
                entry->Size += n->Size;
                RemoveFromBucket(originLevel, GetSegmentFromLevel(n->Size, originLevel), (BucketNode*)n->Position);
                RemoveFromPositionChain(n);
            }

            // originLevel = int.Min(9, originLevel);
            entry->Level = GetLevelFromSize(entry->Size);

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
        private void SetEntryLevel(
                     Entry* entry,
                     nuint position,
                     nuint size
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

            [FieldOffset(0)] internal Entry* NextNear;      // Position linked list: next pointer
            [FieldOffset(8)] internal Entry* PreviousNear;  // Position linked list: previous pointer

            [FieldOffset(32)] internal int Level;           // Level of the block in the allocator
            [FieldOffset(16)] internal nuint Size;           // Block size information
            [FieldOffset(36)] public EntryState State;
            [FieldOffset(24)] public nuint Position;        // Memory block position

        }

        [StructLayout(LayoutKind.Explicit, Size = 32)]
        internal struct Metadata
        {

            [FieldOffset(0)] internal int NextLevel;
            [FieldOffset(4)] internal int NextNear;
            [FieldOffset(8)] internal int PreviousLevel;
            [FieldOffset(12)] internal int PreviousNear;
            [FieldOffset(16)] internal nuint Size;

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
