//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="HLSFAllocator.BucketNode"
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
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class HybridLayerSegregatedFitAllocator
    {

        #region BucketNode Operations

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNullOrEmpty(BucketNode* node)
        {
            return node == null || node->Magic != BucketNode.MAGIC_NUMBER;
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
                node->Magic = BucketNode.MAGIC_NUMBER;
                return;
            } else
            {
                ulong head = _bucketHeads[index];
                node->NextLevel = head;
                BucketNode* headNode = (BucketNode*)head;
                node->IsInBucket = true;
                node->Magic = BucketNode.MAGIC_NUMBER;
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

            ulong n = head->NextLevel;
            BucketNode* next = (BucketNode*)n;

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
            if (next != null) {
                next->PreviousLevel = EmptyHandle;
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

    }
}
