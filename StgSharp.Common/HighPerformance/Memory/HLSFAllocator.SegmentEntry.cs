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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AcquireEntrySpinLock(Entry* entry)
        {
            AcquireEntryBothLocks(entry);
        }

        private bool AssertBackwardBoundary(ulong current, Entry* target, int level)
        {
            int size = (level * 2) + 6;
            ulong align = current >> size;
            return align == target->Position >> size && align == (current + target->Size) >> size;
        }

        private bool AssertForwardBoundary(ulong current, Entry* target, int level)
        {
            int size = (level * 2) + 6;
            ulong align = current >> size;
            return align == target->Position >> size && align == (current - target->Size) >> size;
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
            int originLevel = GetLevelFromSize(size);
            int segment = DetermineSegmentIndex(size, originLevel);

            if (!TryAcquireEntrySpinLock(entry)) {
                return;
            }
            if (!TryRemoveFromBucket(originLevel, segment, (BucketNode*)entry->Position)) { }
            try
            {
                _ = TrySetEntryState(entry, EntryState.Empty, EntryState.Merging);

                // current Level merge
                while (originLevel <= entry->Level)
                {
                    ulong offset = entry->Position - (ulong)m_Buffer;

                    // backward merging 
                    while (true)
                    {
                        ulong p = entry->PreviousNear;
                        Entry* prev = (Entry*)p;
                        if (prev == null ||                                                 // null ref
                            prev == _spareMemory ||                                         // point at end of list
                            GetEntryState(prev) != EntryState.Empty ||                      // not empty
                            !IsAdjacentTo(entry, prev) ||                                   // not nearby
                            entry->Level != prev->Level ||                                  // not same level
                            !AssertBackwardBoundary(offset, prev, entry->Level) ||          // not aligned if merge
                            !TryAcquireEntrySpinLock(prev))                                 // occupied by threads
                        {
                            break;
                        }
                        ulong pp = prev->NextNear;
                        Entry* prevPrev = (Entry*)pp;
                        if (!TryAcquireEntryNextLock(prevPrev))
                        {
                            break;
                        }
                        try
                        {
                            if (!TryRemoveFromBucket(originLevel, DetermineSegmentIndex(prev->Size, originLevel), (BucketNode*)prev->Position))
                            {
                                break;
                            }
                            entry->Position = prev->Position; // Update position to the previous entry
                            entry->Size += prev->Size;       // Merge sizes
                            prevPrev->NextNear = (ulong)entry;
                            RecycleEntry(prev);
                        }
                        finally
                        {
                            ReleaseEntryNextLock(prevPrev);
                            ReleaseEntrySpinLock(prev);
                        }
                    }

                    // forward merging
                    while (true)
                    {
                        ulong n = entry->NextNear;
                        Entry* next = (Entry*)n;
                        if (next == null ||                                                 // null ref
                            next == _spareMemory ||                                         // point at end of list
                            GetEntryState(next) != EntryState.Empty ||                      // not empty
                            !IsAdjacentTo(entry, next) ||                                   // not nearby
                            entry->Level != next->Level ||                                  // not same level
                            !AssertBackwardBoundary(offset, next, entry->Level) ||              // not aligned if merge
                            !TryAcquireEntrySpinLock(next))                                 // occupied by threads
                        {
                            break;
                        }
                        ulong nn = next->NextNear;
                        Entry* nextNext = (Entry*)nn;
                        if (!TryAcquireEntryPrevLock(nextNext))
                        {
                            break;
                        }
                        try
                        {
                            if (!TryRemoveFromBucket(originLevel, DetermineSegmentIndex(next->Size, originLevel), (BucketNode*)next->Position))
                            {
                                break;
                            }
                            entry->Size += next->Size;
                            entry->NextNear = next->NextNear;
                            RecycleEntry(next);
                        }
                        finally
                        {
                            ReleaseEntrySpinLock(next);
                        }
                    }

                    entry->Level = GetLevelFromSize(entry->Size);
                    if (originLevel == entry->Level)
                    {
                        break;
                    }
                    originLevel = entry->Level;
                }

                _ = TrySetEntryState(entry, EntryState.Merging, EntryState.Empty);
                return;
            }
            finally
            {
                ReleaseEntrySpinLock(entry);
            }
        }

        private void RecycleEntry(Entry* entry)
        {
            if (entry is null) {
                return;
            }
            entry->PreviousNear = EmptyHandle;
            entry->Position = EmptyHandle;
            _entries.Free((nuint)entry);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReleaseEntrySpinLock(Entry* entry)
        {
            ReleaseEntryBothLocks(entry);
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
                _entries.Free((nuint)entry);
            }
            finally
            {
                ReleaseEntrySpinLock(entry);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryAcquireEntrySpinLock(Entry* entry)
        {
            return TryAcquireEntryBothLocks(entry);
        }

        /// <summary>
        ///   Entry structure - streamlined version (only position and state information) Stored in
        ///   external SLAB
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size = 44)]
        internal struct Entry
        {

            [FieldOffset(28)] internal int Level;           // Level of the block in the allocator
            [FieldOffset(40)] internal int NextLock;

            [FieldOffset(36)] internal int PrevLock;
            [FieldOffset(32)] internal int State;           // Node state (using int for atomic operations)
            [FieldOffset(24)] internal uint Size;           // Block size information
            [FieldOffset(8)]  internal ulong NextNear;      // Position linked list: next pointer
            [FieldOffset(16)] internal ulong PreviousNear;  // Position linked list: previous pointer
            [FieldOffset(0)]  public nuint Position;        // Memory block position

        }

        // 更新后的EntryState常量类 - 使用int类型
        private static class EntryState
        {

            public const int Allocated = 1;       // Already allocated to user
            public const int Empty = 0;           // Free, can be allocated
            public const int Merging = 3;         // Currently being merged
            public const int ThreadOccupied = 2;  // Occupied by thread (being operated on)

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

        // Entry 状态操作
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TrySetEntryState(Entry* entry, int expectedState, int newState)
        {
            return Interlocked.CompareExchange(ref entry->State, newState, expectedState) == expectedState;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetEntryState(Entry* entry)
        {
            return entry != null ? Volatile.Read(ref entry->State) : 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetEntryState(Entry* entry, int newState)
        {
            if (entry != null) {
                Volatile.Write(ref entry->State, newState);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsEntryEmpty(Entry* entry)
        {
            return GetEntryState(entry) == EntryState.Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsEntryAllocated(Entry* entry)
        {
            return GetEntryState(entry) == EntryState.Allocated;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsEntryThreadOccupied(Entry* entry)
        {
            return GetEntryState(entry) == EntryState.ThreadOccupied;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsEntryMerging(Entry* entry)
        {
            return GetEntryState(entry) == EntryState.Merging;
        }

        // Entry 前驱锁操作（控制PreviousNear字段）
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryAcquireEntryPrevLock(Entry* entry)
        {
            if (entry == null) {
                return true;
            }

            int thread = Environment.CurrentManagedThreadId;
            return Interlocked.CompareExchange(ref entry->PrevLock, thread, 0) == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AcquireEntryPrevLock(Entry* entry)
        {
            if (entry == null) {
                return;
            }

            int thread = Environment.CurrentManagedThreadId;
            if (Volatile.Read(ref entry->PrevLock) != thread)
            {
                while (Interlocked.CompareExchange(ref entry->PrevLock, thread, 0) != 0) {
                    Thread.Sleep(0);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReleaseEntryPrevLock(Entry* entry)
        {
            if (entry == null) {
                return;
            }

            int thread = Environment.CurrentManagedThreadId;
            if (Volatile.Read(ref entry->PrevLock) == thread) {
                Volatile.Write(ref entry->PrevLock, 0);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryAcquireEntryNextLock(Entry* entry)
        {
            if (entry == null) {
                return true;
            }

            int thread = Environment.CurrentManagedThreadId;
            return Interlocked.CompareExchange(ref entry->NextLock, thread, 0) == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AcquireEntryNextLock(Entry* entry)
        {
            if (entry == null) {
                return;
            }

            int thread = Environment.CurrentManagedThreadId;
            if (Volatile.Read(ref entry->NextLock) != thread)
            {
                while (Interlocked.CompareExchange(ref entry->NextLock, thread, 0) != 0) {
                    Thread.Sleep(0);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReleaseEntryNextLock(Entry* entry)
        {
            if (entry == null) {
                return;
            }

            int thread = Environment.CurrentManagedThreadId;
            if (Volatile.Read(ref entry->NextLock) == thread) {
                Volatile.Write(ref entry->NextLock, 0);
            }
        }

        // Entry 双锁组合操作
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryAcquireEntryBothLocks(Entry* entry)
        {
            if (entry == null) {
                return true;
            }

            if (TryAcquireEntryPrevLock(entry))
            {
                if (TryAcquireEntryNextLock(entry))
                {
                    return true;
                } else
                {
                    ReleaseEntryPrevLock(entry); // 回滚
                    return false;
                }
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReleaseEntryBothLocks(Entry* entry)
        {
            if (entry == null) {
                return;
            }

            ReleaseEntryPrevLock(entry);
            ReleaseEntryNextLock(entry);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AcquireEntryBothLocks(Entry* entry)
        {
            if (entry == null) {
                return;
            }

            AcquireEntryPrevLock(entry);
            AcquireEntryNextLock(entry);
        }

        #endregion
    }
}
