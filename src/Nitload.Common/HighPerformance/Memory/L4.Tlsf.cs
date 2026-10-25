// -----------------------------------------------------------------------------
// file="L4.Tlsf"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Numerics;
using System.Reflection.Metadata;

namespace Nitload.Common.HighPerformance.Memory
{
    public partial class L4
    {

        /// <summary>
        ///   L4-specific TLSF allocator whose metadata nodes also serve as cache-line headers.
        /// </summary>
        /// <remarks>
        ///   Payload occupies the first arena region; a fixed metadata slab follows it and is
        ///   carved backward from its aligned high end. Handle 0 is the sentinel, and positive
        ///   handles are metadata slot indices. This layout is intentionally coupled to L4.
        /// </remarks>
        private unsafe struct TLSF
        {

            public const int MaxCacheLineCount = MaxActiveCacheLineCount;

            public static readonly nuint RequestedAllocation =
                    (((nuint)MaxCacheLineCount + 1u) * CacheLineMetadata.SizeOf) + CacheLineMetadata.SizeOf - 1u;
            private readonly CacheLineMetadata* _spareMemory;
            private readonly int _align;
            private readonly int _maxLevel;

            private int _metadataEnd;
            private readonly Ptr64 _basePtr;

            private fixed short _bucket[64];
            private readonly uint _maxAllocSize;
            private readonly uint _totalSize;

            internal TLSF(
                     uint byteSize,
                     uint maxBlockSize,
                     void* externalPointer
            )
            {
                _totalSize = byteSize;
                _basePtr = (Ptr64)(ulong)externalPointer;
                _align = 4096;                                          // align of a memory page
                _maxAllocSize = uint.Max(64, (~64U) & (uint.Min(maxBlockSize, CacheLineMetadata.MaxBufferSize) + 63U));
                _maxLevel = int.MaxValue;
                _maxLevel = GetLevel(_maxAllocSize);

                // _bucket = new short[GetLevel(_maxAllocSize) + 2];
                _bucket[63] = (short)(GetLevel(_maxAllocSize) + 2);

                // Metadata occupies the reservation after payload: handle 0 is the sentinel, and handles 1..MaxCacheLineCount are carved backward from the aligned high end until the slab is full.
                nuint metadataRegionEnd = (nuint)externalPointer + byteSize + RequestedAllocation;
                nuint alignedMetadataEnd = metadataRegionEnd & ~((nuint)CacheLineMetadata.SizeOf - 1u);
                _spareMemory = (CacheLineMetadata*)(alignedMetadataEnd - CacheLineMetadata.SizeOf);
                _metadataEnd = 0;

                // init spare memory node

                _spareMemory->Position = 0;
                _spareMemory->SizeMask = _totalSize;
                _spareMemory->NextLevel = 0;
                _spareMemory->NextNear = 0;
                _spareMemory->PreviousLevel = 0;
                _spareMemory->PreviousNear = 0;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal readonly CacheLineMetadata* MetadataAt(
                                                 int handle
            )
            {
                return _spareMemory - handle;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private readonly int GetLevel(
                                 uint size
            )
            {
                int origin = 25 - BitOperations.LeadingZeroCount(size);
                return int.Min(origin, _maxLevel);
            }

            /// <summary>
            ///   Put a metadata node back to free list and waiting for next allocation.
            /// </summary>
            /// <param name="node">
            ///
            /// </param>
            private void RecycleNode(
                         CacheLineMetadata* node
            )
            {
                short nodeIdx = (short)(_spareMemory - node);
                int level = GetLevel(node->SizeMask);
                short nextLevelIdx = node->NextLevel;
                short prevLevelIdx = node->PreviousLevel;
                RemoveFromBucket(level, node, nodeIdx);

                // remove from position chain
                short prevNearIdx = node->PreviousNear;
                short nextNearIdx = node->NextNear;
                CacheLineMetadata* prevNear = MetadataAt(prevNearIdx);
                prevNear->NextNear = nextNearIdx;
                if (nextNearIdx != 0)
                {
                    CacheLineMetadata* nextNear = MetadataAt(nextNearIdx);
                    nextNear->PreviousNear = prevNearIdx;
                } else
                {
                    _spareMemory->PreviousNear = prevNearIdx;
                }
                node->PreviousNear = 0;
                node->NextNear = 0;

                node->Position = 0;
                node->SizeMask = 0;
                node->State = AllocationState.SpareOrCollect;
                node->PreviousLevel = 0;

                short nextIdx = _spareMemory->NextLevel;
                if (nextIdx != 0)
                {
                    CacheLineMetadata* next = MetadataAt(nextIdx);
                    next->PreviousLevel = nodeIdx;
                }
                node->NextLevel = nextIdx;
                _spareMemory->NextLevel = nodeIdx;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void RemoveFromBucket(
                         CacheLineMetadata* node,
                         int nodeIdx
            )
            {
                RemoveFromBucket(GetLevel(node->SizeMask), node, nodeIdx);
            }

            private void RemoveFromBucket(
                         int level,
                         CacheLineMetadata* node,
                         int nodeIdx
            )
            {
                int currentLevel = level;
                short currentNextLevelIdx = node->NextLevel;
                short currentPrevLevelIdx = node->PreviousLevel;

                if (_bucket[currentLevel] == nodeIdx)
                {
                    _bucket[currentLevel] = currentNextLevelIdx;
                } else if (currentPrevLevelIdx != 0)
                {
                    CacheLineMetadata* prevLevel = MetadataAt(currentPrevLevelIdx);
                    prevLevel->NextLevel = currentNextLevelIdx;
                }
                if (currentNextLevelIdx != 0)
                {
                    CacheLineMetadata* nextLevel = MetadataAt(currentNextLevelIdx);
                    nextLevel->PreviousLevel = currentPrevLevelIdx;
                }
                node->PreviousLevel = 0;
                node->NextLevel = 0;
            }

            // Assuming node to remove is head f bucket
            private void RemoveFromBucketHead(
                         int level,
                         CacheLineMetadata* node,
                         int nodeIdx
            )
            {
                int currentLevel = level;
                short currentNextLevelIdx = node->NextLevel;

                // int currentPrevLevelIdx = 0;

                if (_bucket[currentLevel] == nodeIdx)
                {
                    _bucket[currentLevel] = currentNextLevelIdx;
                }
                /*
                else if (currentPrevLevelIdx != 0)
                {
                    Metadata* prevLevel = &_spareMemory[currentPrevLevelIdx];
                    prevLevel->NextLevel = currentNextLevelIdx;
                }
                /**/
                if (currentNextLevelIdx != 0)
                {
                    CacheLineMetadata* nextLevel = &_spareMemory[currentNextLevelIdx];
                    nextLevel->PreviousLevel = 0;
                }
                node->PreviousLevel = 0;
                node->NextLevel = 0;
            }

            private bool TryAllocNewChunk(
                         uint size,
                         out CacheLineMetadata* meta
            )
            {
                short idx = _spareMemory->NextLevel;
                uint spareSize = _spareMemory->SizeMask;
                if (idx != 0)
                {
                    if (spareSize < size)
                    {
                        meta = null;
                        return false;
                    }
                    meta = MetadataAt(idx);
                    short nextSpareIdx = meta->NextLevel;
                    if (nextSpareIdx != 0)
                    {
                        CacheLineMetadata* nextSpare = MetadataAt(nextSpareIdx);
                        nextSpare->PreviousLevel = 0;
                    }
                    _spareMemory->NextLevel = nextSpareIdx;
                } else
                if (!TryGetNewMetadata(out meta)) {
                    return false;
                }
                idx = (short)(_spareMemory - meta);
                spareSize -= size;
                meta->SizeMask = size;
                meta->Position = _spareMemory->Position;
                meta->State = AllocationState.Free;
                meta->PreviousLevel = 0;
                meta->NextLevel = 0;
                _spareMemory->SizeMask = spareSize;
                _spareMemory->Position += size;

                // insert the new chunk to position chain
                short nearIdx = _spareMemory->PreviousNear;
                CacheLineMetadata* near = MetadataAt(nearIdx);
                near->NextNear = idx;
                meta->PreviousNear = nearIdx;
                _spareMemory->PreviousNear = idx;
                meta->NextNear = 0;
                return true;
            }

            private bool TryGetNewMetadata(
                         out CacheLineMetadata* meta
            )
            {
                ulong spareSize = _spareMemory->SizeMask;
                if (_metadataEnd >= MaxCacheLineCount)
                {
                    meta = null;
                    return false;
                }
                _metadataEnd++;
                meta = MetadataAt(_metadataEnd);
                return true;
            }

            private bool TryTakeBucketEntry(
                         uint minimumSize,
                         out CacheLineMetadata* entry
            )
            {
                int level = GetLevel(minimumSize);

                // first try to find in the target level
                {
                    int idx = _bucket[level];
                    while (idx != 0)
                    {
                        CacheLineMetadata* candidate = MetadataAt(idx);
                        if (candidate->SizeMask >= minimumSize)
                        {
                            RemoveFromBucket(level, candidate, idx);
                            entry = candidate;
                            return true;
                        }
                        idx = candidate->NextLevel;
                    }
                }
                for (int i = level + 1; i < _bucket[63]; i++)
                {
                    int idx = _bucket[i];
                    if (idx != 0)
                    {
                        CacheLineMetadata* candidate = MetadataAt(idx);
                        RemoveFromBucketHead(i, candidate, idx);
                        entry = candidate;
                        return true;
                    }
                }

                entry = null;
                return false;
            }

            #region alloc and free

            public int Alloc(
                       uint size
            )
            {
                uint allocSize = uint.Max(64, (~63U) & (size + 63));
                if (allocSize > _maxAllocSize) {
                    throw new OverflowException("Requested allocation size exceeds the maximum block size.");
                }
                if (!TryTakeBucketEntry(allocSize, out CacheLineMetadata* entry) &&
                    !TryAllocNewChunk(allocSize, out entry)) {
                    throw new OverflowException("No available ");
                }
                short entryIdx = (short)(_spareMemory - entry);
                uint entrySize = entry->SizeMask;
                if (entrySize < allocSize) {
                    throw new OverflowException("Bucket returned an undersized block.");
                }
                uint remain = entrySize - allocSize;

                // slice tail
                if (remain > ulong.Min(4096UL, entrySize >> 7) &&
                    TryGetNewMetadata(out CacheLineMetadata* newEntry))
                {
                    short newEntryIdx = (short)(_spareMemory - newEntry);
                    newEntry->Position = entry->Position + allocSize;
                    newEntry->SizeMask = remain;
                    newEntry->State = AllocationState.Free;
                    int newLevel = GetLevel(remain);
                    short bucketHeadIdx = _bucket[newLevel];
                    if (bucketHeadIdx != 0)
                    {
                        // if there is already an entry in the bucket, link it to the new entry
                        CacheLineMetadata* bucketHead = MetadataAt(bucketHeadIdx);
                        bucketHead->PreviousLevel = newEntryIdx;
                    }
                    newEntry->NextLevel = bucketHeadIdx;

                    newEntry->PreviousLevel = 0;
                    _bucket[newLevel] = newEntryIdx;

                    // push to position chain
                    short afterIdx = entry->NextNear;
                    CacheLineMetadata* after = MetadataAt(afterIdx);
                    newEntry->NextNear = afterIdx;
                    after->PreviousNear = newEntryIdx;
                    entry->NextNear = newEntryIdx;
                    newEntry->PreviousNear = entryIdx;
                }
                entry->State = AllocationState.Alloc;
                if (entry->Position > _totalSize) {
                    throw new OverflowException("Allocated memory address exceeds the maximum block size.");
                }
                return entryIdx;
            }

            public void Collect()
            {
                short currentIdx = _spareMemory->NextNear;
                do
                {
                    if (currentIdx == 0)
                    {
                        break;
                    }
                    CacheLineMetadata* current = MetadataAt(currentIdx);
                    if (current->State == AllocationState.Free)
                    {
                        // scan next
                        uint size = 0;
                        int nextIdx = current->NextNear;
                        while (nextIdx != 0)
                        {
                            CacheLineMetadata* next = MetadataAt(nextIdx);
                            if (next->State != AllocationState.Free)
                            {
                                break;
                            }
                            size += next->SizeMask;
                            RecycleNode(next);
                            nextIdx = current->NextNear;
                        }
                        if (size > 0)
                        {
                            current->SizeMask += size;
                            RemoveFromBucket(current, currentIdx);
                            int newLevel = GetLevel(current->SizeMask);
                            short bucketHeadIdx = _bucket[newLevel];
                            if (bucketHeadIdx != 0)
                            {
                                CacheLineMetadata* bucketHead = MetadataAt(bucketHeadIdx);
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
                        CacheLineMetadata* head,
                        int collectScanWindow = 4
            )
            {
                short headIdx = (short)(_spareMemory - head);
                uint size = head->SizeMask;
                for (int i = 0; i < collectScanWindow; i++)
                {
                    // forward scan, previous direction
                    int forwardIdx = head->PreviousNear;
                    if (forwardIdx == 0)
                    {
                        break;
                    }
                    CacheLineMetadata* prev = MetadataAt(forwardIdx);
                    if (prev->State != AllocationState.Free)
                    {
                        break;
                    }
                    size += prev->SizeMask;
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
                    CacheLineMetadata* prev = MetadataAt(nextIdx);
                    if (prev->State != AllocationState.Free)
                    {
                        break;
                    }
                    size += prev->SizeMask;
                    RecycleNode(prev);
                }

                // recycle to level;
                head->SizeMask = size;
                head->PredictCount = 0;
                head->MapCount = 0;
                head->ConsumeCount = 0;
                head->State = AllocationState.Free;
                head->PreviousLevel = 0;
                int level = GetLevel(head->SizeMask);
                short bucketHeadIdx = _bucket[level];
                head->NextLevel = 0;
                if (bucketHeadIdx != 0)
                {
                    CacheLineMetadata* bucketHead = MetadataAt(bucketHeadIdx);
                    bucketHead->PreviousLevel = headIdx;
                    head->NextLevel = bucketHeadIdx;
                }
                _bucket[level] = headIdx;
                return;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Free(
                        ulong handle,
                        int collectScanWindow = 4
            )
            {
                CacheLineMetadata* head = (CacheLineMetadata*)handle;
                Free(head, collectScanWindow);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Free(
                        int handle,
                        int collectScanWindow = 4
            )
            {
                CacheLineMetadata* head = MetadataAt(handle);
                Free(head, collectScanWindow);
            }

            public bool TryAlloc(
                        uint size,
                        out int h
            )
            {
                uint allocSize = uint.Max(64, (~63U) & (size + 63));
                if (allocSize > _maxAllocSize)
                {
                    h = -1;
                    return false;
                }
                if (!TryTakeBucketEntry(allocSize, out CacheLineMetadata* entry) &&
                    !TryAllocNewChunk(allocSize, out entry))
                {
                    h = -1;
                    return false;
                }
                short entryIdx = (short)(_spareMemory - entry);
                uint entrySize = entry->SizeMask;
                if (entrySize < allocSize)
                {
                    h = -1;
                    return false;
                }
                uint remain = entrySize - allocSize;

                // slice tail
                if (remain > (entrySize >> 7) && TryGetNewMetadata(out CacheLineMetadata* newEntry))
                {
                    short newEntryIdx = (short)(_spareMemory - newEntry);
                    newEntry->Position = entry->Position + allocSize;
                    newEntry->SizeMask = remain;
                    newEntry->State = AllocationState.Free;
                    int newLevel = GetLevel(remain);
                    short bucketHeadIdx = _bucket[newLevel];
                    if (bucketHeadIdx != 0)
                    {
                        // if there is already an entry in the bucket, link it to the new entry
                        CacheLineMetadata* bucketHead = MetadataAt(bucketHeadIdx);
                        newEntry->NextLevel = bucketHeadIdx;
                        bucketHead->PreviousLevel = newEntryIdx;
                    } else
                    {
                        newEntry->NextLevel = 0;
                    }
                    newEntry->PreviousLevel = 0;
                    _bucket[newLevel] = newEntryIdx;

                    // push to position chain
                    short afterIdx = entry->NextNear;
                    CacheLineMetadata* after = MetadataAt(afterIdx);
                    newEntry->NextNear = afterIdx;
                    after->PreviousNear = newEntryIdx;
                    entry->NextNear = newEntryIdx;
                    newEntry->PreviousNear = entryIdx;
                }
                entry->State = AllocationState.Alloc;
                if (entry->Position > _totalSize)
                {
                    h = default;
                    throw new OverflowException("Allocated memory address exceeds the maximum block size.");
                }
                h = entryIdx;
                return true;
            }

            #endregion
        }

    }
}
