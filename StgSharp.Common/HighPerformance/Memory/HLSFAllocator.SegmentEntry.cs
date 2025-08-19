//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="HLSFAllocator.SegmentEntry.cs"
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

        private static void AcquireEntrySpinLock(Entry* entry)
        {
            int thread = Environment.CurrentManagedThreadId;
            if (entry != null && Volatile.Read(ref entry->SpinLock) != thread)
            {
                while (Interlocked.CompareExchange(ref entry->SpinLock, thread, 0) != 0) {
                    Thread.Sleep(0);
                }
            }
        }

        private bool AssertAlignBoundry(ulong current, Entry* target, int level)
        {
            int size = (level * 2) + 6;
            ulong align = current >> size;
            return align == target->Position >> size && align == target->Size;
        }

        /// <summary>
        ///   Determine segment index based on size and originLevel
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int DetermineSegmentIndex(uint size, int level)
        {
            return (int)size / levelSize[level];
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
        ///   stretagey, any fail in acquiring spin lock will end a merging process.
        /// </summary>
        private void MergeAndRemoveFromBuckets(Entry* entry)
        {
            if (entry == null) {
                return;
            }

            Entry* result = entry;
            uint size = entry->Size;
            int originLevel = -1;
            int segment = DetermineSegmentIndex(size, originLevel);
            if (TryAcquireEntrySpinLock(entry)) {
                return;
            }

            // current Level merge
            while (originLevel != entry->Level)
            {
                ulong offset = entry->Position - (ulong)m_Buffer;
                AcquireBucketNodeSpinLock((BucketNode*)entry->Position);

                while (true)
                {
                    ulong n = entry->NextNear;
                    Entry* near = (Entry*)n;
                    if (near == null ||
                        near->State != EntryState.Empty ||
                        !IsAdjacentTo(entry, near) ||
                        entry->Level != near->Level ||
                        !AssertAlignBoundry(offset, near, entry->Level) ||
                        !TryAcquireEntrySpinLock(near))
                    {
                        break;
                    }
                    RemoveFromBucket(originLevel, , (BucketNode*)near->Position);
                }

                // forward merging

                while (true)
                {
                    ulong n = entry->NextNear;
                    Entry* near = (Entry*)n;
                    if (near == null ||
                        near->State != EntryState.Empty ||
                        !IsAdjacentTo(entry, near) ||
                        entry->Level != near->Level ||
                        !AssertAlignBoundry(offset, near, entry->Level) ||
                        !TryAcquireEntrySpinLock(near))
                    {
                        break;
                    }
                }
            }
        }

        private void MergeEntry(Entry* remain, Entry* other)
        {
            if (other == null) {
                return;
            }

            // Determine merged starting position and size
            nuint newPosition = remain->Position < other->Position ? other->Position : other->Position;
            uint newSize = remain->Size + other->Size;

            // Update current Entry
            remain->Position = newPosition;
            remain->Size = newSize;
        }

        private void RecycleEntry(Entry* entry)
        {
            if (entry is null) {
                return;
            }
            entry-> PreviousNear = EmptyHandle;
            entry-> Position = EmptyHandle;
            _nodes.Free((nuint)entry);
        }

        private static void ReleaseEntrySpinLock(Entry* entry)
        {
            int thread = Environment.CurrentManagedThreadId;
            if (entry != null && Volatile.Read(ref entry->SpinLock) != 0) {
                _ = Interlocked.CompareExchange(ref entry->SpinLock, 0, thread);
            }
        }

        /// <summary>
        ///   Remove node from position linked list
        /// </summary>
        private void RemoveFromPositionChain(Entry* entry)
        {
            AcquireEntrySpinLock(entry);
            try
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
                _nodes.Free((nuint)entry);
            }
            finally
            {
                ReleaseEntrySpinLock(entry);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryAcquireEntrySpinLock(Entry* entry)
        {
            int thread = Environment.CurrentManagedThreadId;
            return entry == null ||
                   (Interlocked.CompareExchange(ref entry->SpinLock, thread,
                                               0) == 0) ||
                   Volatile.Read(ref entry->SpinLock) == thread;
        }

        /// <summary>
        ///   Entry structure - streamlined version (only position and state information) Stored in
        ///   external SLAB
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size = 40)]
        internal struct Entry
        {

            [FieldOffset(36)] internal int Level;           // Level of the block in the allocator
            [FieldOffset(24)] internal int SpinLock;        // Spin lock for thread safety protection
            [FieldOffset(28)] internal int State;           // Node state  
            [FieldOffset(32)] internal uint Size;           // Block size information
            [FieldOffset(8)]  internal ulong NextNear;      // Position linked list: next pointer
            [FieldOffset(16)] internal ulong PreviousNear;  // Position linked list: previous pointer
            [FieldOffset(0)]  public nuint Position;        // Memory block position

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsNullOrEmptyIndex(ulong index)
            {
                return index == EmptyHandle;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsNullOrEmptyIndex(Entry* entry)
            {
                return entry == null;
            }

            /// <summary>
            ///   Merge two adjacent Entries (caller must ensure they are adjacent and both Empty
            ///   state)
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void MergeWith(Entry* other) { }

        }

        private static class EntryState
        {

            public const int Allocated = 1;       // Already allocated to user
            public const int Empty = 0;           // Free, can be allocated
            public const int ThreadOccupied = 2;  // Occupied by thread (being operated on)

        }

    }
}
