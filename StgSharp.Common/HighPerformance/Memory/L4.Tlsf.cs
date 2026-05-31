//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="L4.Tlsf"
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
using StgSharp.HighPerformance.ProcessorAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.Memory
{
    public partial class L4
    {

        private unsafe class TLSF
        {

            public const int MaxCacheLineCount = 8192;

            public static readonly nuint RequestedAllocation = (nuint)MaxCacheLineCount * CacheLineMetadata.SizeOf;

            private readonly short[] _bucket;
            private readonly CacheLineMetadata* _spareMemory;
            private readonly int _align;
            private readonly int _maxLevel;

            private int _metadataEnd;
            private readonly Ptr64 _basePtr;
            private readonly ulong _maxAllocSize;
            private readonly ulong _totalSize;

            internal TLSF(nuint byteSize, ulong maxBlockSize, void* externalPointer)
            {
                _totalSize = byteSize;
                _basePtr = (Ptr64)(ulong)externalPointer;
                _align = 4096;                                          // align of a memory page
                _maxAllocSize = ulong.Max(64, (~64UL) & (ulong.Min(maxBlockSize + 63, CacheLineMetadata.Mask48Bits)));
                _maxLevel = int.MaxValue;
                _maxLevel = GetLevel(_maxAllocSize);
                _bucket = new int[GetLevel(_maxAllocSize) + 2];
                _spareMemory = (CacheLineMetadata*)(ulong)(_basePtr + _totalSize);
                _metadataEnd = 0;

                // init spare memory node

                _spareMemory->Position = _basePtr;
                _spareMemory->SizeForSpare = _totalSize - CacheLineMetadata.SizeOf;
                _spareMemory->NextLevel = 0;
                _spareMemory->NextNear = 0;
                _spareMemory->PreviousLevel = 0;
                _spareMemory->PreviousNear = 0;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int GetLevel(ulong size)
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
            private void RecycleNode(CacheLineMetadata* node)
            {
                short nodeIdx = (short)(_spareMemory - node);
                int level = GetLevel(node->SizeForAllocator);
                short nextLevelIdx = node->NextLevel;
                short prevLevelIdx = node->PreviousLevel;
                RemoveFromBucket(level, node, nodeIdx);

                // remove from position chain
                short prevNearIdx = node->PreviousNear;
                short nextNearIdx = node->NextNear;
                CacheLineMetadata* prevNear = &_spareMemory[prevNearIdx];
                prevNear->NextNear = nextNearIdx;
                if (nextNearIdx != 0)
                {
                    CacheLineMetadata* nextNear = &_spareMemory[nextNearIdx];
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

                short nextIdx = _spareMemory->NextLevel;
                if (nextIdx != 0)
                {
                    CacheLineMetadata* next = &_spareMemory[nextIdx];
                    next->PreviousLevel = nodeIdx;
                }
                node->NextLevel = nextIdx;
                _spareMemory->NextLevel = nodeIdx;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void RemoveFromBucket(CacheLineMetadata* node, int nodeIdx)
            {
                RemoveFromBucket(GetLevel(node->SizeForAllocator), node, nodeIdx);
            }

            private void RemoveFromBucket(int level, CacheLineMetadata* node, int nodeIdx)
            {
                int currentLevel = level;
                short currentNextLevelIdx = node->NextLevel;
                short currentPrevLevelIdx = node->PreviousLevel;

                if (_bucket[currentLevel] == nodeIdx)
                {
                    _bucket[currentLevel] = currentNextLevelIdx;
                } else if (currentPrevLevelIdx != 0)
                {
                    CacheLineMetadata* prevLevel = &_spareMemory[currentPrevLevelIdx];
                    prevLevel->NextLevel = currentNextLevelIdx;
                }
                if (currentNextLevelIdx != 0)
                {
                    CacheLineMetadata* nextLevel = &_spareMemory[currentNextLevelIdx];
                    nextLevel->PreviousLevel = currentPrevLevelIdx;
                }
                node->PreviousLevel = 0;
                node->NextLevel = 0;
            }

            // Assuming node to remove is head f bucket
            private void RemoveFromBucketHead(int level, CacheLineMetadata* node, int nodeIdx)
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

            private bool TryAllocNewChunk(ulong size, out CacheLineMetadata* meta)
            {
                short idx = _spareMemory->NextLevel;
                ulong spareSize = _spareMemory->SizeForAllocator;
                if (idx != 0)
                {
                    if (spareSize < size)
                    {
                        meta = null;
                        return false;
                    }
                    meta = &_spareMemory[idx];
                    short nextSpareIdx = meta->NextLevel;
                    if (nextSpareIdx != 0)
                    {
                        CacheLineMetadata* nextSpare = &_spareMemory[nextSpareIdx];
                        nextSpare->PreviousLevel = 0;
                    }
                    _spareMemory->NextLevel = nextSpareIdx;
                } else
                if (!TryGetNewMetadata(out meta)) {
                    return false;
                }
                spareSize -= size;
                meta->SizeForAllocator = size;
                meta->Position = _spareMemory->Position;
                meta->State = AllocationState.Free;
                meta->PreviousLevel = 0;
                meta->NextLevel = 0;
                _spareMemory->SizeForAllocator = spareSize;
                _spareMemory->Position += size;

                // insert the new chunk to position chain
                short nearIdx = _spareMemory->PreviousNear;
                CacheLineMetadata* near = &_spareMemory[nearIdx];
                near->NextNear = idx;
                meta->PreviousNear = nearIdx;
                _spareMemory->PreviousNear = idx;
                meta->NextNear = 0;
                return true;
            }

            private bool TryGetNewMetadata(out CacheLineMetadata* meta)
            {
                ulong spareSize = _spareMemory->SizeForAllocator;
                if (_metadataEnd >= MaxCacheLineCount)
                {
                    meta = null;
                    return false;
                }
                _metadataEnd++;
                meta = &_spareMemory[_metadataEnd];
                return true;
            }

            private bool TryTakeBucketEntry(ulong minimumSize, out CacheLineMetadata* entry)
            {
                int level = GetLevel(minimumSize);

                // first try to find in the target level
                {
                    int idx = _bucket[level];
                    while (idx != 0)
                    {
                        CacheLineMetadata* candidate = &_spareMemory[idx];
                        if (candidate->SizeForAllocator >= minimumSize)
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
                        CacheLineMetadata* candidate = &_spareMemory[idx];
                        RemoveFromBucketHead(i, candidate, idx);
                        entry = candidate;
                        return true;
                    }
                }

                entry = null;
                return false;
            }

            #region alloc and free

            public int Alloc(nuint size)
            {
                ulong allocSize = ulong.Max(64, (~63UL) & ((ulong)size + 63));
                if (allocSize > _maxAllocSize) {
                    throw new OverflowException("Requested allocation size exceeds the maximum block size.");
                }
                if (!TryTakeBucketEntry(allocSize, out CacheLineMetadata* entry) &&
                    !TryAllocNewChunk(allocSize, out entry)) {
                    throw new OverflowException("No available ");
                }
                short entryIdx = (short)(_spareMemory - entry);
                ulong entrySize = entry->SizeForAllocator;
                if (entrySize < allocSize) {
                    throw new OverflowException("Bucket returned an undersized block.");
                }
                ulong remain = entrySize - allocSize;

                // slice tail
                if (remain > ulong.Min(4096UL, entrySize >> 7) && TryGetNewMetadata(out CacheLineMetadata* newEntry))
                {
                    short newEntryIdx = (short)(_spareMemory - newEntry);
                    newEntry->Position = entry->Position + allocSize;
                    newEntry->SizeForAllocator = remain;
                    newEntry->State = AllocationState.Free;
                    int newLevel = GetLevel(remain);
                    short bucketHeadIdx = _bucket[newLevel];
                    if (bucketHeadIdx != 0)
                    {
                        // if there is already an entry in the bucket, link it to the new entry
                        CacheLineMetadata* bucketHead = &_spareMemory[bucketHeadIdx];
                        bucketHead->PreviousLevel = newEntryIdx;
                    }
                    newEntry->NextLevel = bucketHeadIdx;

                    newEntry->PreviousLevel = 0;
                    _bucket[newLevel] = newEntryIdx;

                    // push to position chain
                    short afterIdx = entry->NextNear;
                    CacheLineMetadata* after = &_spareMemory[afterIdx];
                    newEntry->NextNear = afterIdx;
                    after->PreviousNear = newEntryIdx;
                    entry->NextNear = newEntryIdx;
                    newEntry->PreviousNear = entryIdx;
                }
                entry->State = AllocationState.Alloc;
                if (entry->Position - (ulong)_basePtr > _totalSize) {
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
                    CacheLineMetadata* current = &_spareMemory[currentIdx];
                    if (current->State == AllocationState.Free)
                    {
                        // scan next
                        Ptr64 size = 0;
                        int nextIdx = current->NextNear;
                        while (nextIdx != 0)
                        {
                            CacheLineMetadata* next = &_spareMemory[nextIdx];
                            if (next->State != AllocationState.Free)
                            {
                                break;
                            }
                            size += next->SizeForAllocator;
                            RecycleNode(next);
                            nextIdx = current->NextNear;
                        }
                        if (size > 0)
                        {
                            current->SizeForAllocator += size;
                            RemoveFromBucket(current, currentIdx);
                            int newLevel = GetLevel(current->SizeForAllocator);
                            short bucketHeadIdx = _bucket[newLevel];
                            if (bucketHeadIdx != 0)
                            {
                                CacheLineMetadata* bucketHead = &_spareMemory[bucketHeadIdx];
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

            public void Free(int handle, int collectScanWindow = 4)
            {
                CacheLineMetadata* head = &_spareMemory[handle];
                short headIdx = (short)(_spareMemory - head);
                Ptr64 size = head->SizeForAllocator;
                for (int i = 0; i < collectScanWindow; i++)
                {
                    // forward scan, previous direction
                    int forwardIdx = head->PreviousNear;
                    if (forwardIdx == 0)
                    {
                        break;
                    }
                    CacheLineMetadata* prev = &_spareMemory[forwardIdx];
                    if (prev->State != AllocationState.Free)
                    {
                        break;
                    }
                    size += prev->SizeForAllocator;
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
                    CacheLineMetadata* prev = &_spareMemory[nextIdx];
                    if (prev->State != AllocationState.Free)
                    {
                        break;
                    }
                    size += prev->SizeForAllocator;
                    RecycleNode(prev);
                }

                // recycle to level;
                head->SizeForAllocator = size;
                head->State = AllocationState.Free;
                head->PreviousLevel = 0;
                int level = GetLevel(head->SizeForAllocator);
                short bucketHeadIdx = _bucket[level];
                head->NextLevel = 0;
                if (bucketHeadIdx != 0)
                {
                    CacheLineMetadata* bucketHead = &_spareMemory[bucketHeadIdx];
                    bucketHead->PreviousLevel = headIdx;
                    head->NextLevel = bucketHeadIdx;
                }
                _bucket[level] = headIdx;
                return;
            }

            public bool TryAlloc(nuint size, out int h)
            {
                ulong allocSize = ulong.Max(64, (~63UL) & ((ulong)size + 63));
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
                ulong entrySize = entry->SizeForAllocator;
                if (entrySize < allocSize)
                {
                    h = -1;
                    return false;
                }
                ulong remain = entrySize - allocSize;

                // slice tail
                if (remain > (entrySize >> 7) && TryGetNewMetadata(out CacheLineMetadata* newEntry))
                {
                    short newEntryIdx = (short)(_spareMemory - newEntry);
                    newEntry->Position = entry->Position + allocSize;
                    newEntry->SizeForAllocator = remain;
                    newEntry->State = AllocationState.Free;
                    int newLevel = GetLevel(remain);
                    short bucketHeadIdx = _bucket[newLevel];
                    if (bucketHeadIdx != 0)
                    {
                        // if there is already an entry in the bucket, link it to the new entry
                        CacheLineMetadata* bucketHead = &_spareMemory[bucketHeadIdx];
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
                    CacheLineMetadata* after = &_spareMemory[afterIdx];
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
                h = entryIdx;
                return true;
            }

            #endregion
        }

    }
}
