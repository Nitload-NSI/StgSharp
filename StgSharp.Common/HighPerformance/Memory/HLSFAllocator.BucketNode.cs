//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="HLSFAllocator.BucketNode.cs"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the ¡°Software¡±), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//     
// The above copyright notice and 
// this permission notice shall be included in all copies 
// or substantial portions of the Software.
//     
// THE SOFTWARE IS PROVIDED ¡°AS IS¡±, 
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class HybridLayerSegregatedFitAllocator
    {

        private static void AcquireBucketNodeSpinLock(BucketNode* node)
        {
            int thread = Environment.CurrentManagedThreadId;
            if (node != null && Volatile.Read(ref node->SpinLock) != thread)
            {
                while (Interlocked.CompareExchange(ref node->SpinLock, thread, 0) != 0) {
                    Thread.Sleep(0);
                }
            }
        }

        /// <summary>
        ///   Initialize bucket system (called in constructor)
        /// </summary>
        private void InitializeBucketSystem()
        {
            _bucketHeads = new ulong[levelSize.Length * 3];
        }

        private void PushLevel(int levelIndex, int sizeIndex, BucketNode* node)
        {
            if (node == null || node->EntryRef == null) {
                return;
            }

            int index = (3 * levelIndex) + sizeIndex;
            if (Interlocked.CompareExchange(ref _bucketHeads[index], (nuint)node, EmptyHandle) == EmptyHandle)
            {
                node->Mask = true;
                node->Magic = BucketNode.MAGIC_NUMBER;
                return;
            } else
            {
                while (true)
                {
                    AcquireBucketNodeSpinLock(node);
                    ulong head = Volatile.Read(ref _bucketHeads[index]);
                    node->NextLevel = head;
                    BucketNode* headNode = (BucketNode*)head;
                    try
                    {
                        if (Interlocked.CompareExchange(ref _bucketHeads[index], (nuint)node, head) == head)
                        {
                            if (!TryAcquireBucketNodeSpinLock(headNode))
                            {
                                // reset state and retry
                                _ = Interlocked.CompareExchange(ref _bucketHeads[index], head, (nuint)node);
                                Thread.Sleep(0);
                                continue;
                            }
                            try
                            {
                                node->Mask = true;
                                node->Magic = BucketNode.MAGIC_NUMBER;
                                headNode->PreviousLevel = (ulong)node;
                                return;
                            }
                            finally
                            {
                                ReleaseBucketNodeSpinLock(headNode);
                            }
                        } else
                        {
                            Thread.Sleep(0);
                            continue;
                        }
                    }
                    finally
                    {
                        ReleaseBucketNodeSpinLock(node);
                    }
                }
            }
        }

        private static void ReleaseBucketNodeSpinLock(BucketNode* node)
        {
            int thread = Environment.CurrentManagedThreadId;
            if (node != null && Volatile.Read(ref node->SpinLock) != 0) {
                _ = Interlocked.CompareExchange(ref node->SpinLock, 0, thread);
            }
        }

        private void RemoveFromBucket(int levelIndex, int sizeIndex, BucketNode* node)
        {
            int index = (3 * levelIndex) + sizeIndex;
            while (true)
            {
                // top of stack
                if ((BucketNode*)Volatile.Read(ref _bucketHeads[index]) == node && TryAcquireBucketNodeSpinLock(node))
                {
                    _ = TryPopLevel(levelIndex, sizeIndex, out _);
                } else
                {
                    AcquireBucketNodeSpinLock(node);
                    ulong p = Volatile.Read(ref node->PreviousLevel), n = Volatile.Read(ref node->NextLevel);
                    BucketNode* previous = (BucketNode*)p, next = (BucketNode*)n;
                    try
                    {
                        if (next is null)
                        {
                            // bottom of stack
                            if (!TryAcquireBucketNodeSpinLock(previous))
                            {
                                Thread.Sleep(0);
                                continue;
                            }
                            previous->NextLevel = EmptyHandle;
                            node->PreviousLevel = EmptyHandle;
                            return;
                        }
                        if (!(TryAcquireBucketNodeSpinLock(previous) && TryAcquireBucketNodeSpinLock(next)))
                        {
                            // middle of stack
                            Thread.Sleep(0);
                            continue;
                        }
                        previous ->NextLevel = n;
                        next->PreviousLevel = p;
                        node->NextLevel = EmptyHandle;
                        node->PreviousLevel = EmptyHandle;
                        return;
                    }
                    finally
                    {
                        ReleaseBucketNodeSpinLock(node);
                        ReleaseBucketNodeSpinLock(previous);
                        ReleaseBucketNodeSpinLock(next);
                    }
                }
            }
        }

        private static bool TryAcquireBucketNodeSpinLock(BucketNode* node)
        {
            int thread = Environment.CurrentManagedThreadId;
            return (node == null) ||
                   (Interlocked.CompareExchange(ref node->SpinLock, thread,
                                                0) == 0) ||
                   (Volatile.Read(ref node->SpinLock) == thread);
        }

        private bool TryPopLevel(int levelIndex, int sizeIndex, out BucketNode* node)
        {
            int index = (3 * levelIndex) + sizeIndex;
            while (true)
            {
                ulong h = Volatile.Read(ref _bucketHeads[index]);
                BucketNode* head = (BucketNode*)h;
                BucketNode* next = null;
                try
                {
                    if (head == null)
                    {
                        node = null;
                        return false;
                    }
                    if (!TryAcquireBucketNodeSpinLock(head))
                    {
                        Thread.Sleep(0);
                        continue;
                    }
                    ulong n = Volatile.Read(ref head->NextLevel);
                    next = (BucketNode*)n;
                    if (!TryAcquireBucketNodeSpinLock(next))
                    {
                        Thread.Sleep(0);
                        continue;
                    }
                    if (Interlocked.CompareExchange(ref _bucketHeads[index], head->NextLevel, h) == h)
                    {
                        node = head;
                        node->Mask = false;
                        node->NextLevel = EmptyHandle;
                        next->PreviousLevel = EmptyHandle;
                        return true;
                    } else
                    {
                        Thread.Sleep(0);
                        continue;
                    }
                }
                finally
                {
                    ReleaseBucketNodeSpinLock(head);
                    ReleaseBucketNodeSpinLock(next);
                }
            }
        }

        /// <summary>
        ///   BucketNode structure - specialized node for bucket linked lists Stored in main
        ///   allocation area (user-overwritable region)
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size = 36)]
        internal struct BucketNode
        {

            public const uint MAGIC_NUMBER = 0xBEEFCAFE; // Magic number for node integrity verification

            [FieldOffset(16)] internal Entry* EntryRef;     // Pointer to corresponding Entry
            [FieldOffset(32)] internal int SpinLock;        // Spin lock for bucket head operation
            [FieldOffset(24)] internal uint Magic;          // Magic number verification
            [FieldOffset(28)] internal uint Mask;     // Whether in bucket
            [FieldOffset(0)]  internal ulong NextLevel;     // Bucket stack linked list: next pointer
            [FieldOffset(8)]  internal ulong PreviousLevel; // Bucket stack linked list: previous pointer

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsNullOrEmpty(BucketNode* node)
            {
                return node == null || node->Magic != MAGIC_NUMBER;
            }

            #region node mask

            public const uint AllOperating = 0b_11100000_00000000_00000000_00000111;
            public const uint Empty = 0b_10000000_00000000_00000000_00000000;
            public const uint InBucket = 0b_10000000_00000000_00000000_00000000;
            public const uint LeftOperating = 0b_01000000_00000000_00000000_00000100;
            public const uint ReadyForRemove = 0b_01000000_00000000_00000000_00000000;
            public const uint RightOperating = 0b_00100000_00000000_00000000_00000001;
            public const uint SelfOperating = 0b_00000000_00000000_00000000_00000010;

            private bool BucketNodeIsInBucket
            {
[MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => (Mask & InBucket) != 0;
[MethodImpl(MethodImplOptions.AggressiveInlining)]
                set => Mask |= value ? InBucket : 0;
            }

            #endregion
        }

    }
}
