//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="TwoLayerSegregatedFitAllocator"
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
using StgSharp.HighPerformance.Memory;
using StgSharp.HighPerformance.ProcessorAbstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static StgSharp.HighPerformance.Memory.TwoLayerSegregatedFitAllocator;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe class TwoLayerSegregatedFitAllocator : IDisposable
    {

        private const ulong Mask48Bits = 0x0000FFFFFFFFFFFF;

        private readonly int[] _bucket;
        private readonly Metadata* _spareMemory;
        private readonly Action<nuint> _freeHandle;
        private bool disposedValue;
        private readonly int _align;
        private readonly int _maxLevel;

        private int _metadataEnd;
        private readonly Ptr64 _basePtr;
        private readonly ulong _maxAllocSize;
        private readonly ulong _totalSize;

        internal TwoLayerSegregatedFitAllocator(
                 nuint byteSize,
                 int align,
                 ulong maxBlockSize,
                 Func<nuint, int, nuint> allocator,
                 Action<nuint> freeHandle
        )
            : this(byteSize, align, maxBlockSize, (void*)allocator(byteSize, align), freeHandle) { }

        internal TwoLayerSegregatedFitAllocator(
                 nuint byteSize,
                 int align,
                 ulong maxBlockSize,
                 void* externalPointer,
                 Action<nuint> freeHandle
        )
        {
            _totalSize = byteSize;
            _basePtr = (Ptr64)(ulong)externalPointer;
            _align = align;
            _maxAllocSize = ulong.Max(64, (~64UL) & (ulong.Min(maxBlockSize + 63, Mask48Bits)));
            _maxLevel = int.MaxValue;
            _maxLevel = GetLevel(_maxAllocSize);
            _bucket = new int[GetLevel(_maxAllocSize) + 2];
            _spareMemory = &((Metadata*)(ulong)(_basePtr + _totalSize))[-1];
            _freeHandle = freeHandle ?? (static_ => { });
            _metadataEnd = 0;

            // init spare memory node

            _spareMemory->Position = _basePtr;
            _spareMemory->SizeForSpare = _totalSize - Metadata.SizeOf;
            _spareMemory->NextLevel = 0;
            _spareMemory->NextNear = 0;
            _spareMemory->PreviousLevel = 0;
            _spareMemory->PreviousNear = 0;
        }

        public TwoLayerSegregatedFitAllocator(
               nuint byteSize
        )
            : this(byteSize, 16, 32U * 1024U * 1024U, NativeMemory.AlignedAlloc(byteSize, 16),
                   ptr => NativeMemory.AlignedFree((void*)ptr)) { }

        public TwoLayerSegregatedFitAllocator(
               nuint byteSize,
               uint maxBlockSize
        )
            : this(byteSize, 16, maxBlockSize, NativeMemory.AlignedAlloc(byteSize, 16),
                   ptr => NativeMemory.AlignedFree((void*)ptr)) { }

        public TwoLayerSegregatedFitAllocator(
               nuint byteSize,
               int align
        )
            : this(byteSize, align, 32U * 1024U * 1024U, NativeMemory.AlignedAlloc(byteSize, 16),
                   ptr => NativeMemory.AlignedFree((void*)ptr)) { }

        public TwoLayerSegregatedFitAllocator(
               nuint byteSize,
               int align,
               nuint maxBlockSize
        )
            : this(byteSize, align, maxBlockSize, NativeMemory.AlignedAlloc(byteSize, 16),
                   ptr => NativeMemory.AlignedFree((void*)ptr)) { }

        public void Dispose()
        {
            Dispose(disposing:true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(
                               bool disposing
        )
        {
            if (!disposedValue)
            {
                if (disposing) {
                    _freeHandle((nuint)_basePtr);
                }

                disposedValue = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetLevel(
                    ulong size
        )
        {
            int origin = 57 - BitOperations.LeadingZeroCount(size);
            return int.Min(origin, _maxLevel);
        }

        /// <summary>
        ///   Put a metadata node back to free list and waiting for next allocation.
        /// </summary>
        /// <param name="node">
        ///
        /// </param>
        private void RecycleNode(
                     Metadata* node
        )
        {
            int nodeIdx = (int)(_spareMemory - node);
            int level = GetLevel(node->Size);
            int nextLevelIdx = node->NextLevel;
            int prevLevelIdx = node->PreviousLevel;
            RemoveFromBucket(level, node, nodeIdx);

            // remove from position chain
            int prevNearIdx = node->PreviousNear;
            int nextNearIdx = node->NextNear;
            Metadata* prevNear = &_spareMemory[-prevNearIdx];
            prevNear->NextNear = nextNearIdx;
            if (nextNearIdx != 0)
            {
                Metadata* nextNear = &_spareMemory[-nextNearIdx];
                nextNear->PreviousNear = prevNearIdx;
            } else
            {
                _spareMemory->PreviousNear = prevNearIdx;
            }
            node->PreviousNear = 0;
            node->NextNear = 0;

            node->Position = 0;
            node->SizeForSpare = 0;
            node->State = AllocationState.SpareOrCollect;
            node->PreviousLevel = 0;

            int nextIdx = _spareMemory->NextLevel;
            if (nextIdx != 0)
            {
                Metadata* next = &_spareMemory[-nextIdx];
                next->PreviousLevel = nodeIdx;
            }
            node->NextLevel = nextIdx;
            _spareMemory->NextLevel = nodeIdx;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveFromBucket(
                     Metadata* node,
                     int nodeIdx
        )
        {
            RemoveFromBucket(GetLevel(node->Size), node, nodeIdx);
        }

        private void RemoveFromBucket(
                     int level,
                     Metadata* node,
                     int nodeIdx
        )
        {
            int currentLevel = level;
            int currentNextLevelIdx = node->NextLevel;
            int currentPrevLevelIdx = node->PreviousLevel;

            if (_bucket[currentLevel] == nodeIdx)
            {
                _bucket[currentLevel] = currentNextLevelIdx;
            } else if (currentPrevLevelIdx != 0)
            {
                Metadata* prevLevel = &_spareMemory[-currentPrevLevelIdx];
                prevLevel->NextLevel = currentNextLevelIdx;
            }
            if (currentNextLevelIdx != 0)
            {
                Metadata* nextLevel = &_spareMemory[-currentNextLevelIdx];
                nextLevel->PreviousLevel = currentPrevLevelIdx;
            }
            node->PreviousLevel = 0;
            node->NextLevel = 0;
        }

        // Assuming node to remove is head f bucket
        private void RemoveFromBucketHead(
                     int level,
                     Metadata* node,
                     int nodeIdx
        )
        {
            int currentLevel = level;
            int currentNextLevelIdx = node->NextLevel;

            // int currentPrevLevelIdx = 0;

            if (_bucket[currentLevel] == nodeIdx)
            {
                _bucket[currentLevel] = currentNextLevelIdx;
            }
            /*
            else if (currentPrevLevelIdx != 0)
            {
                Metadata* prevLevel = &_spareMemory[-currentPrevLevelIdx];
                prevLevel->NextLevel = currentNextLevelIdx;
            }
            /**/
            if (currentNextLevelIdx != 0)
            {
                Metadata* nextLevel = &_spareMemory[-currentNextLevelIdx];
                nextLevel->PreviousLevel = 0;
            }
            node->PreviousLevel = 0;
            node->NextLevel = 0;
        }

        private bool TryAllocNewChunk(
                     ulong size,
                     out Metadata* meta
        )
        {
            int idx = _spareMemory->NextLevel;
            ulong spareSize = _spareMemory->Size;
            if (idx != 0)
            {
                if (spareSize < size)
                {
                    meta = null;
                    return false;
                }
                meta = &_spareMemory[-idx];
                int nextSpareIdx = meta->NextLevel;
                if (nextSpareIdx != 0)
                {
                    Metadata* nextSpare = &_spareMemory[-nextSpareIdx];
                    nextSpare->PreviousLevel = 0;
                }
                _spareMemory->NextLevel = nextSpareIdx;
            } else
            {
                if (spareSize < Metadata.SizeOf + size)
                {
                    meta = null;
                    return false;
                }
                _metadataEnd++;
                idx = _metadataEnd;
                meta = &_spareMemory[-idx];
                spareSize -= Metadata.SizeOf;
            }
            spareSize -= size;
            meta->Size = size;
            meta->Position = _spareMemory->Position;
            meta->State = AllocationState.Free;
            meta->PreviousLevel = 0;
            meta->NextLevel = 0;
            _spareMemory->Size = spareSize;
            _spareMemory->Position += size;

            // insert the new chunk to position chain
            int nearIdx = _spareMemory->PreviousNear;
            Metadata* near = &_spareMemory[-nearIdx];
            near->NextNear = idx;
            meta->PreviousNear = nearIdx;
            _spareMemory->PreviousNear = idx;
            meta->NextNear = 0;
            return true;
        }

        private bool TryGetNewMetadata(
                     out Metadata* meta
        )
        {
            ulong spareSize = _spareMemory->Size;
            if (spareSize < Metadata.SizeOf)
            {
                meta = null;
                return false;
            }
            _metadataEnd++;
            meta = &_spareMemory[-_metadataEnd];
            spareSize -= Metadata.SizeOf;
            _spareMemory->Size = spareSize;
            return true;
        }

        private bool TryTakeBucketEntry(
                     ulong minimumSize,
                     out Metadata* entry
        )
        {
            int level = GetLevel(minimumSize);

            // first try to find in the target level
            {
                int idx = _bucket[level];
                while (idx != 0)
                {
                    Metadata* candidate = &_spareMemory[-idx];
                    if (candidate->Size >= minimumSize)
                    {
                        RemoveFromBucket(level, candidate, idx);
                        entry = candidate;
                        return true;
                    }
                    idx = candidate->NextLevel;
                }
            }
            for (int i = level + 1; i < _bucket.Length; i++)
            {
                int idx = _bucket[i];
                if (idx != 0)
                {
                    Metadata* candidate = &_spareMemory[-idx];
                    RemoveFromBucketHead(i, candidate, idx);
                    entry = candidate;
                    return true;
                }
            }

            entry = null;
            return false;
        }

        [StructLayout(LayoutKind.Explicit)]
        public readonly struct Handle
        {

            [FieldOffset(0)] internal readonly Metadata* _meta;
            [FieldOffset(8)] public readonly Ptr64 Address;
            [FieldOffset(16)] public readonly Ptr64 Size;

            internal Handle(
                     Metadata* meta,
                     Ptr64 baseAddress
            )
            {
                _meta = meta;
                Address = (ulong)meta->Position;
                Size = meta->Size;
            }

            [Obsolete("You should not create instance from default. Alloc from allocator please")]
            public Handle() { }

            public ref byte this[
                            int offset
            ] => ref ((byte*)(ulong)Address)[offset];

            public Span<byte> AsSpan(
                              int range = -1
            )
            {
                return new Span<byte>((byte*)(ulong)Address, range < 0 ? (int)(ulong)Size : range);
            }

        }

        internal enum AllocationState : byte
        {

            SpareOrCollect = 0,
            Free = 1,
            Alloc = 2,

        }

        [StructLayout(LayoutKind.Explicit, Size = 32)]
        internal struct Metadata
        {

            [FieldOffset(16)] private ulong PositionMask;
            [FieldOffset(24)] private ulong SizeMask;

            [FieldOffset(30)] internal AllocationState State;
            [FieldOffset(31)] internal sbyte Level;

            [FieldOffset(0)] public int NextLevel;
            [FieldOffset(4)] public int NextNear;
            [FieldOffset(8)] public int PreviousLevel;
            [FieldOffset(12)] public int PreviousNear;

            public Ptr64 Position
            {
                readonly get => PositionMask;
                set => PositionMask = value;
            }

            public Ptr64 Size
            {
                readonly get => (Ptr64)(SizeMask & Mask48Bits);
                set => SizeMask = (SizeMask & ~Mask48Bits) | ((ulong)value & Mask48Bits);
            }

            public Ptr64 SizeForSpare
            {
                readonly get => SizeMask;
                set => SizeMask = value;
            }

            public static uint SizeOf { get; } = (uint)Unsafe.SizeOf<Metadata>();

        }

        #region alloc and free

        public Handle Alloc(
                      nuint size
        )
        {
            ulong allocSize = ulong.Max(64, (~63UL) & ((ulong)size + 63));
            if (allocSize > _maxAllocSize) {
                throw new OverflowException("Requested allocation size exceeds the maximum block size.");
            }
            if (!TryTakeBucketEntry(allocSize, out Metadata* entry) &&
                !TryAllocNewChunk(allocSize, out entry)) {
                throw new OverflowException("No available ");
            }
            int entryIdx = (int)(_spareMemory - entry);
            ulong entrySize = entry->Size;
            if (entrySize < allocSize) {
                throw new OverflowException("Bucket returned an undersized block.");
            }
            ulong remain = entrySize - allocSize;

            // slice tail
            if (remain > (entrySize >> 7) && TryGetNewMetadata(out Metadata* newEntry))
            {
                int newEntryIdx = (int)(_spareMemory - newEntry);
                newEntry->Position = entry->Position + allocSize;
                newEntry->Size = remain;
                newEntry->State = AllocationState.Free;
                int newLevel = GetLevel(remain);
                int bucketHeadIdx = _bucket[newLevel];
                if (bucketHeadIdx != 0)
                {
                    // if there is already an entry in the bucket, link it to the new entry
                    Metadata* bucketHead = &_spareMemory[-bucketHeadIdx];
                    bucketHead->PreviousLevel = newEntryIdx;
                }
                newEntry->NextLevel = bucketHeadIdx;

                newEntry->PreviousLevel = 0;
                _bucket[newLevel] = newEntryIdx;

                // push to position chain
                int afterIdx = entry->NextNear;
                Metadata* after = &_spareMemory[-afterIdx];
                newEntry->NextNear = afterIdx;
                after->PreviousNear = newEntryIdx;
                entry->NextNear = newEntryIdx;
                newEntry->PreviousNear = entryIdx;
            }
            entry->State = AllocationState.Alloc;
            if (entry->Position - (ulong)_basePtr > _totalSize) {
                throw new OverflowException("Allocated memory address exceeds the maximum block size.");
            }
            return new Handle(entry, _basePtr);
        }

        public void Collect()
        {
            int currentIdx = _spareMemory->NextNear;
            do
            {
                if (currentIdx == 0)
                {
                    break;
                }
                Metadata* current = &_spareMemory[-currentIdx];
                if (current->State == AllocationState.Free)
                {
                    // scan next
                    Ptr64 size = 0;
                    int nextIdx = current->NextNear;
                    while (nextIdx != 0)
                    {
                        Metadata* next = &_spareMemory[-nextIdx];
                        if (next->State != AllocationState.Free)
                        {
                            break;
                        }
                        size += next->Size;
                        RecycleNode(next);
                        nextIdx = current->NextNear;
                    }
                    if (size > 0)
                    {
                        current->Size += size;
                        RemoveFromBucket(current, currentIdx);
                        int newLevel = GetLevel(current->Size);
                        int bucketHeadIdx = _bucket[newLevel];
                        if (bucketHeadIdx != 0)
                        {
                            Metadata* bucketHead = &_spareMemory[-bucketHeadIdx];
                            bucketHead->PreviousLevel = currentIdx;
                        }
                        current->NextLevel = bucketHeadIdx;
                        current->PreviousLevel = 0;
                        _bucket[newLevel] = currentIdx;
                    }
                }
                currentIdx = current->NextNear;
            } while (true);
        }

        public void Free(
                    Handle handle,
                    int collectScanWindow = 4
        )
        {
            Metadata* head = handle._meta;
            int headIdx = (int)(_spareMemory - head);
            Ptr64 size = handle.Size;
            for (int i = 0; i < collectScanWindow; i++)
            {
                // forward scan, previous direction
                int forwardIdx = head->PreviousNear;
                if (forwardIdx == 0)
                {
                    break;
                }
                Metadata* prev = &_spareMemory[-forwardIdx];
                if (prev->State != AllocationState.Free)
                {
                    break;
                }
                size += prev->Size;
                head->Position = prev->Position;
                RecycleNode(prev);
            }
            for (int i = 0; i < collectScanWindow; i++)
            {
                // backward scan, next direction
                int nextIdx = head->NextNear;
                if (nextIdx == 0)
                {
                    break;
                }
                Metadata* prev = &_spareMemory[-nextIdx];
                if (prev->State != AllocationState.Free)
                {
                    break;
                }
                size += prev->Size;
                RecycleNode(prev);
            }

            // recycle to level;
            head->Size = size;
            head->State = AllocationState.Free;
            head->PreviousLevel = 0;
            int level = GetLevel(head->Size);
            int bucketHeadIdx = _bucket[level];
            head->NextLevel = 0;
            if (bucketHeadIdx != 0)
            {
                Metadata* bucketHead = &_spareMemory[-bucketHeadIdx];
                bucketHead->PreviousLevel = headIdx;
                head->NextLevel = bucketHeadIdx;
            }
            _bucket[level] = headIdx;
            return;
        }

        public bool TryAlloc(
                    nuint size,
                    out Handle h
        )
        {
            ulong allocSize = ulong.Max(64, (~63UL) & ((ulong)size + 63));
            if (allocSize > _maxAllocSize)
            {
                h = default;
                return false;
            }
            if (!TryTakeBucketEntry(allocSize, out Metadata* entry) &&
                !TryAllocNewChunk(allocSize, out entry))
            {
                h = default;
                return false;
            }
            int entryIdx = (int)(_spareMemory - entry);
            ulong entrySize = entry->Size;
            if (entrySize < allocSize)
            {
                h = default;
                return false;
            }
            ulong remain = entrySize - allocSize;

            // slice tail
            if (remain > (entrySize >> 7) && TryGetNewMetadata(out Metadata* newEntry))
            {
                int newEntryIdx = (int)(_spareMemory - newEntry);
                newEntry->Position = entry->Position + allocSize;
                newEntry->Size = remain;
                newEntry->State = AllocationState.Free;
                int newLevel = GetLevel(remain);
                int bucketHeadIdx = _bucket[newLevel];
                if (bucketHeadIdx != 0)
                {
                    // if there is already an entry in the bucket, link it to the new entry
                    Metadata* bucketHead = &_spareMemory[-bucketHeadIdx];
                    newEntry->NextLevel = bucketHeadIdx;
                    bucketHead->PreviousLevel = newEntryIdx;
                } else
                {
                    newEntry->NextLevel = 0;
                }
                newEntry->PreviousLevel = 0;
                _bucket[newLevel] = newEntryIdx;

                // push to position chain
                int afterIdx = entry->NextNear;
                Metadata* after = &_spareMemory[-afterIdx];
                newEntry->NextNear = afterIdx;
                after->PreviousNear = newEntryIdx;
                entry->NextNear = newEntryIdx;
                newEntry->PreviousNear = entryIdx;
            }
            entry->State = AllocationState.Alloc;
            if (entry->Position - (ulong)_basePtr > _totalSize)
            {
                h = default;
                throw new OverflowException("Allocated memory address exceeds the maximum block size.");
            }
            h = new Handle(entry, _basePtr);
            return true;
        }

        #endregion
    }
}
