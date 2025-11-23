//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="HLSFAllocator.BucketNode"
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
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class HybridLayerSegregatedFitAllocator
    {

        #region BucketNode Operations

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNullOrEmpty(BucketNode* node)
        {
            return node == null;
        }

            #endregion

        private void PushLevel(int levelIndex, int sizeIndex, BucketNode* node)
        {
            if (node == null || node->EntryRef == null) {
                return;
            }

            int index = (3 * levelIndex) + sizeIndex;
            if (_bucketHeads[index] == EmptyHandle)
            {
                _bucketHeads[index] = (nuint)node;
                node->IsInBucket = true;
                return;
            } else
            {
                ulong head = _bucketHeads[index];
                node->NextLevel = head;
                BucketNode* headNode = (BucketNode*)head;
                node->IsInBucket = true;
                if (headNode != null) {
                    headNode->PreviousLevel = (ulong)node;
                }
                return;
            }
        }

        private void RemoveFromBucket(int levelIndex, int sizeIndex, BucketNode* node)
        {
            int index = (3 * levelIndex) + sizeIndex;

            if ((BucketNode*)_bucketHeads[index] == node)
            {
                if (TryPopLevel(levelIndex, sizeIndex, out _)) {
                    return;
                }
            }

            ulong p = node->PreviousLevel;
            ulong n = node->NextLevel;
            BucketNode* previous = (BucketNode*)p;
            BucketNode* next = (BucketNode*)n;

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

        private bool TryPopLevel(int levelIndex, int sizeIndex, out BucketNode* node)
        {
            int index = (3 * levelIndex) + sizeIndex;

            ulong h = _bucketHeads[index];
            BucketNode* head = (BucketNode*)h;

            if (head == null)
            {
                node = null;
                return false;
            }

            BucketNode* next = (BucketNode*)head->NextLevel;

            if (next == null)
            {
                _bucketHeads[index] = EmptyHandle;
                node = head;
                return true;
            }
            _bucketHeads[index] = head->NextLevel;
            node = head;
            node->IsInBucket = false;
            node->NextLevel = EmptyHandle;
            if (next != null)
            {
                Console.WriteLine("////////////// catching error ///////////////");
                Console.WriteLine($"ref next: {(ulong)next}");
                Console.WriteLine($"base pos: {(ulong)m_Buffer}");
                Console.WriteLine(_size);
                next->PreviousLevel = EmptyHandle;
                Console.WriteLine("///////////// no error caught ///////////////");
            }
            return true;
        }

        private bool TryRemoveFromBucket(int levelIndex, int sizeIndex, BucketNode* node)
        {
            int index = (3 * levelIndex) + sizeIndex;
            if ((BucketNode*) _bucketHeads[index] == node) {
                return TryPopLevel(levelIndex, sizeIndex, out _);
            }

            ulong p = node->PreviousLevel;
            ulong n = node->NextLevel;
            BucketNode* previous = (BucketNode*)p;
            BucketNode* next = (BucketNode*)n;

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

        /// <summary>
        ///   BucketNode structure - specialized node for bucket linked lists Stored in main
        ///   allocation area (user-overwritable region)
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size = 40)]
        internal struct BucketNode
        {

            [FieldOffset(0)] internal Entry* EntryRef;     // Pointer to corresponding Entry
            [FieldOffset(32)] internal bool IsInBucket;     // Whether in bucket

            [FieldOffset(8)]  internal ulong _nextLevel;     // Bucket stack linked list: next pointer
            [FieldOffset(16)] internal ulong LockBuffer;
            [FieldOffset(24)]  internal ulong PreviousLevel; // Bucket stack linked list: previous pointer

            internal ulong NextLevel
            {
                get
                {
                    Console.WriteLine($"get next level {_nextLevel}");
                    return _nextLevel;
                }
                set
                {
                    Console.WriteLine($"NextLevel set to {value}.");
                    _nextLevel = value;
                }
            }

        }

    }
}
