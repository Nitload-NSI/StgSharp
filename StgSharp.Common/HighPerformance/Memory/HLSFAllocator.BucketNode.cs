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
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class HybridLayerSegregatedFitAllocator
    {

        private void PushBucketToLevel(int levelIndex, int sizeIndex, BucketNode* node)
        {
            if (node == null || node->EntryRef == null) {
                return;
            }

            Index index = (levelIndex > MaxLevel) ? ^1 : (3 * levelIndex) + sizeIndex;
            if (_bucketHeads[index] is null)
            {
                _bucketHeads[index] = node;
                node->IsInBucket = true;
                node->NextLevel = null;
                node->PreviousLevel = null;

                // Console.WriteLine($"Push to empty level of {levelIndex} {sizeIndex}, bucket pos {(ulong)node}");
                return;
            } else
            {
                BucketNode* head = _bucketHeads[index];
                node->NextLevel = head;
                BucketNode* headNode = head;
                node->IsInBucket = true;
                _bucketHeads[index] = node;

                // Console.WriteLine($"Push to head of level of {levelIndex} {sizeIndex}, bucket pos {(ulong)node}, previous head is at {head}");
                if (headNode != null)
                {
                    headNode->PreviousLevel = node;
                }
                return;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveFromBucket(int levelIndex, int sizeIndex, BucketNode* node)
        {
            Index index = (levelIndex > MaxLevel) ? ^1 : (3 * levelIndex) + sizeIndex;
            if (_bucketHeads[index] == node)
            {
                if (TryPopLevel(levelIndex, sizeIndex, out _)) {
                    return;
                }
            }
            BucketNode* previous = node->PreviousLevel;
            BucketNode* next = node->NextLevel;

            if (previous != null) {
                previous->NextLevel = next;
            }
            if (next != null) {
                next->PreviousLevel = previous;
            }

            node->NextLevel = (BucketNode*)null;
            node->PreviousLevel = (BucketNode*)null;
            node->IsInBucket = false;
            return;
        }

        private bool TryPopLevel(int levelIndex, int sizeIndex, out BucketNode* node)
        {
            Index index = levelIndex > MaxLevel ? ^1 : (3 * levelIndex) + sizeIndex;

            BucketNode* head = _bucketHeads[index];

            if (head == null)
            {
                node = null;
                return false;
            }

            // Console.WriteLine($"level popped from level {levelIndex} {sizeIndex}, node is at {(ulong)head}");
            BucketNode* next = head->NextLevel;

            if (next == null)
            {
                _bucketHeads[index] = null;
                node = head;
                return true;
            }
            _bucketHeads[index] = head->NextLevel;
            node = head;
            node->IsInBucket = false;
            node->NextLevel = null;
            if (next != null) {
                next->PreviousLevel = null;
            }
            return true;
        }

        private bool TryRemoveFromBucket(int levelIndex, int sizeIndex, BucketNode* node)
        {
            Index index = levelIndex > 9 ? ^1 : (3 * levelIndex) + sizeIndex;
            if (_bucketHeads[index] == node)
            {
                bool ok = TryPopLevel(levelIndex, sizeIndex, out _);
                return ok;
            }

            BucketNode* previous = node->PreviousLevel;
            BucketNode* next = node->NextLevel;

            if (previous != null) {
                previous->NextLevel = next;
            }
            if (next != null) {
                next->PreviousLevel = previous;
            }

            node->NextLevel = null;
            node->PreviousLevel = null;
            node->IsInBucket = false;
            node->EntryRef = null;
            return true;
        }

        /// <summary>
        ///   BucketNode structure - specialized node for bucket linked lists Stored in main
        ///   allocation area (user-overwritable region)
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        internal struct BucketNode
        {

            [FieldOffset(0)] internal Entry* EntryRef;     // Pointer to corresponding Entry
            // [FieldOffset(16)] internal ulong LockBuffer;
            [FieldOffset(8)] internal  BucketNode* NextLevel;     // Bucket stack linked list: next pointer
            [FieldOffset(24)] internal BucketNode* PreviousLevel; // Bucket stack linked list: previous pointer
            [FieldOffset(32)] internal bool IsInBucket;     // Whether in bucket

        }

    }
}
