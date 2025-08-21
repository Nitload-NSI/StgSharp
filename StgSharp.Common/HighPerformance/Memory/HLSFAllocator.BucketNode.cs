//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="HLSFAllocator.BucketNode.cs"
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
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class HybridLayerSegregatedFitAllocator
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AcquireBucketNodeSpinLock(BucketNode* node)
        {
            AcquireBucketNodeBothLocks(node);
        }

        /// <summary>
        ///   Initialize bucket system (called in constructor)
        /// </summary>
        private void InitializeBucketSystem()
        {
            _bucketHeads = new ulong[levelSizeArray.Length * 3];
        }

        private void PushLevel(int levelIndex, int sizeIndex, BucketNode* node)
        {
            if (node == null || node->EntryRef == null) {
                return;
            }

            int index = (3 * levelIndex) + sizeIndex;
            if (Interlocked.CompareExchange(ref _bucketHeads[index], (nuint)node, EmptyHandle) == EmptyHandle)
            {
                node->IsInBucket = true;
                node->Magic = BucketNode.MAGIC_NUMBER;
                return;
            } else
            {
                while (true)
                {
                    AcquireBucketNodeNextLock(node);
                    ulong head = Volatile.Read(ref _bucketHeads[index]);
                    node->NextLevel = head;
                    BucketNode* headNode = (BucketNode*)head;

                    try
                    {
                        if (Interlocked.CompareExchange(ref _bucketHeads[index], (nuint)node, head) == head)
                        {
                            if (headNode != null && !TryAcquireBucketNodePrevLock(headNode))
                            {
                                // reset state and retry
                                _ = Interlocked.CompareExchange(ref _bucketHeads[index], head, (nuint)node);
                                Thread.Sleep(0);
                                continue;
                            }
                            try
                            {
                                node->IsInBucket = true;
                                node->Magic = BucketNode.MAGIC_NUMBER;
                                if (headNode != null) {
                                    headNode->PreviousLevel = (ulong)node;
                                }
                                return;
                            }
                            finally
                            {
                                if (headNode != null) {
                                    ReleaseBucketNodePrevLock(headNode);
                                }
                            }
                        } else
                        {
                            Thread.Sleep(0);
                            continue;
                        }
                    }
                    finally
                    {
                        ReleaseBucketNodeNextLock(node);
                    }
                }
            }
        }

        /// <summary>
        ///   根据锁类型释放BucketNode锁
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReleaseBucketNodeLockByType(BucketNode* node, byte lockType)
        {
            switch (lockType)
            {
                case 0:
                    ReleaseBucketNodePrevLock(node);
                    return;  // PrevLock
                case 1:
                    ReleaseBucketNodeBothLocks(node);
                    return; // BothLocks
                case 2:
                    ReleaseBucketNodeNextLock(node);
                    return;  // NextLock
                default:
                    return;
            }
        }

        /// <summary>
        ///   按地址顺序释放BucketNode锁
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReleaseOrderedBucketLocks(BucketNode* previous, BucketNode* node, BucketNode* next)
        {
            if (previous != null) {
                ReleaseBucketNodeNextLock(previous);
            }
            if (node != null) {
                ReleaseBucketNodeBothLocks(node);
            }
            if (next != null) {
                ReleaseBucketNodePrevLock(next);
            }
        }

        private void RemoveFromBucket(int levelIndex, int sizeIndex, BucketNode* node)
        {
            int index = (3 * levelIndex) + sizeIndex;
            while (true)
            {
                if ((BucketNode*)Volatile.Read(ref _bucketHeads[index]) == node)
                {
                    if (TryPopLevel(levelIndex, sizeIndex, out _)) {
                        return;
                    }
                    Thread.Sleep(0);
                    continue;
                }

                ulong p = Volatile.Read(ref node->PreviousLevel);
                ulong n = Volatile.Read(ref node->NextLevel);
                BucketNode* previous = (BucketNode*)p;
                BucketNode* next = (BucketNode*)n;

                if (!TryAcquireOrderedBucketLocks(previous, node, next))
                {
                    Thread.Sleep(0);
                    continue;
                }

                try
                {
                    if (Volatile.Read(ref node->PreviousLevel) != p || Volatile.Read(ref node->NextLevel) != n)
                    {
                        Thread.Sleep(0);
                        continue;
                    }

                    if (previous != null) {
                        previous->NextLevel = n;
                    }
                    if (next != null) {
                        next->PreviousLevel = p;
                    }

                    node->NextLevel = EmptyHandle;
                    node->PreviousLevel = EmptyHandle;
                    node->IsInBucket = false;
                    return;
                }
                finally
                {
                    ReleaseOrderedBucketLocks(previous, node, next);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryAcquireBucketNodeSpinLock(BucketNode* node)
        {
            return TryAcquireBucketNodeBothLocks(node);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryAcquireOrderedBucketLocks(BucketNode* previous, BucketNode* node, BucketNode* next)
        {
            if (previous != null && node != null && next != null)
            {
                // previous -> node -> next
                return TryAcquireBucketNodeNextLock(previous) &&
                       TryAcquireBucketNodeBothLocks(node) &&
                       TryAcquireBucketNodePrevLock(next);
            } else if (previous != null && node != null) {
                return TryAcquireBucketNodeNextLock(previous) && TryAcquireBucketNodeBothLocks(node);
            }
            return true;
        }

        private bool TryPopLevel(int levelIndex, int sizeIndex, out BucketNode* node)
        {
            int index = (3 * levelIndex) + sizeIndex;
            while (true)
            {
                ulong h = Volatile.Read(ref _bucketHeads[index]);
                BucketNode* head = (BucketNode*)h;

                if (head == null)
                {
                    node = null;
                    return false;
                }

                if (!TryAcquireBucketNodeNextLock(head))
                {
                    Thread.Sleep(0);
                    continue;
                }

                ulong n = Volatile.Read(ref head->NextLevel);
                BucketNode* next = (BucketNode*)n;

                if (next == null)
                {
                    if (Interlocked.CompareExchange(ref _bucketHeads[index], EmptyHandle, h) == h)
                    {
                        node = head;
                        return true;
                    }
                    node = null;
                    return false;
                }
                try
                {
                    if (!TryAcquireBucketNodePrevLock(next))
                    {
                        Thread.Sleep(0);
                        continue;
                    }

                    try
                    {
                        if (Interlocked.CompareExchange(ref _bucketHeads[index], head->NextLevel, h) == h)
                        {
                            node = head;
                            node->IsInBucket = false;
                            node->NextLevel = EmptyHandle;
                            if (next != null) {
                                next->PreviousLevel = EmptyHandle;
                            }
                            return true;
                        } else
                        {
                            Thread.Sleep(0);
                            continue;
                        }
                    }
                    finally
                    {
                        if (next != null) {
                            ReleaseBucketNodePrevLock(next);
                        }
                    }
                }
                finally
                {
                    ReleaseBucketNodeNextLock(head);
                }
            }
        }

        private bool TryRemoveFromBucket(int levelIndex, int sizeIndex, BucketNode* node)
        {
            int index = (3 * levelIndex) + sizeIndex;
            if ((BucketNode*)Volatile.Read(ref _bucketHeads[index]) == node)
            {
                if (TryPopLevel(levelIndex, sizeIndex, out _)) {
                    return true;
                }
                return false;
            }

            ulong p = Volatile.Read(ref node->PreviousLevel);
            ulong n = Volatile.Read(ref node->NextLevel);
            BucketNode* previous = (BucketNode*)p;
            BucketNode* next = (BucketNode*)n;

            if (!TryAcquireOrderedBucketLocks(previous, node, next)) {
                return false;
            }

            try
            {
                if (Volatile.Read(ref node->PreviousLevel) != p || Volatile.Read(ref node->NextLevel) != n) {
                    return false;
                }

                if (previous != null) {
                    previous->NextLevel = n;
                }
                if (next != null) {
                    next->PreviousLevel = p;
                }

                node->NextLevel = EmptyHandle;
                node->PreviousLevel = EmptyHandle;
                node->IsInBucket = false;
                return true;
            }
            finally
            {
                ReleaseOrderedBucketLocks(previous, node, next);
            }
        }

        /// <summary>
        ///   BucketNode structure - specialized node for bucket linked lists Stored in main
        ///   allocation area (user-overwritable region)
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size = 40)]
        internal struct BucketNode
        {

            public const uint MAGIC_NUMBER = 0xBEEFCAFE; // Magic number for node integrity verification

            [FieldOffset(16)] internal Entry* EntryRef;     // Pointer to corresponding Entry
            [FieldOffset(28)] internal bool IsInBucket;     // Whether in bucket
            [FieldOffset(36)] internal int NextLock;
            [FieldOffset(32)] internal int PrevLock;
            [FieldOffset(24)] internal uint Magic;          // Magic number verification
            [FieldOffset(32)] internal ulong LockBuffer;

            [FieldOffset(0)]  internal ulong NextLevel;     // Bucket stack linked list: next pointer
            [FieldOffset(8)]  internal ulong PreviousLevel; // Bucket stack linked list: previous pointer

        }

        #region BucketNode Operations

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNullOrEmpty(BucketNode* node)
        {
            return node == null || node->Magic != BucketNode.MAGIC_NUMBER;
        }

        // BucketNode 前驱锁操作（控制PreviousLevel字段）
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryAcquireBucketNodePrevLock(BucketNode* node)
        {
            if (node == null) {
                return true;
            }

            int thread = Environment.CurrentManagedThreadId;
            return Interlocked.CompareExchange(ref node->PrevLock, thread, 0) == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AcquireBucketNodePrevLock(BucketNode* node)
        {
            if (node == null) {
                return;
            }

            int thread = Environment.CurrentManagedThreadId;
            if (Volatile.Read(ref node->PrevLock) != thread)
            {
                while (Interlocked.CompareExchange(ref node->PrevLock, thread, 0) != 0) {
                    Thread.Sleep(0);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReleaseBucketNodePrevLock(BucketNode* node)
        {
            if (node == null) {
                return;
            }

            int thread = Environment.CurrentManagedThreadId;
            if (Volatile.Read(ref node->PrevLock) == thread) {
                Volatile.Write(ref node->PrevLock, 0);
            }
        }

        // BucketNode 后继锁操作（控制NextLevel字段）
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryAcquireBucketNodeNextLock(BucketNode* node)
        {
            if (node == null) {
                return true;
            }

            int thread = Environment.CurrentManagedThreadId;
            return Interlocked.CompareExchange(ref node->NextLock, thread, 0) == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AcquireBucketNodeNextLock(BucketNode* node)
        {
            if (node == null) {
                return;
            }

            int thread = Environment.CurrentManagedThreadId;
            if (Volatile.Read(ref node->NextLock) != thread)
            {
                while (Interlocked.CompareExchange(ref node->NextLock, thread, 0) != 0) {
                    Thread.Sleep(0);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReleaseBucketNodeNextLock(BucketNode* node)
        {
            if (node == null) {
                return;
            }

            int thread = Environment.CurrentManagedThreadId;
            if (Volatile.Read(ref node->NextLock) == thread) {
                Volatile.Write(ref node->NextLock, 0);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryAcquireBucketNodeBothLocks(BucketNode* node)
        {
            if (node == null) {
                return true;
            }

            if (TryAcquireBucketNodePrevLock(node))
            {
                if (TryAcquireBucketNodeNextLock(node))
                {
                    return true;
                } else
                {
                    ReleaseBucketNodePrevLock(node); // 回滚
                    return false;
                }
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReleaseBucketNodeBothLocks(BucketNode* node)
        {
            if (node == null) {
                return;
            }

            ReleaseBucketNodePrevLock(node);
            ReleaseBucketNodeNextLock(node);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AcquireBucketNodeBothLocks(BucketNode* node)
        {
            if (node == null) {
                return;
            }

            AcquireBucketNodePrevLock(node);
            AcquireBucketNodeNextLock(node);
        }

        #endregion
    }
}
