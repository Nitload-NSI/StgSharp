//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="SwissTable.X86"
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
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace StgSharp.Collections
{
    public partial class SwissTable
    {

        private unsafe class BucketScannerX86 : IBucketScanner
        {

            public bool TryFind(
                        nuint key,
                        uint hash,
                        Bucket* bucket,
                        out nuint value
            )
            {
                byte ctrl = (byte)((hash >> EntryMaskWidth) & 0x7F);
                Vector128<byte> ctrlVector = Vector128.Create(ctrl);
                Vector128<byte> bucketCtrlVector = Sse2.LoadVector128((byte*)&bucket->Control);
                Vector128<byte> cmpResult = Sse2.CompareEqual(ctrlVector, bucketCtrlVector);
                int mask = Sse2.MoveMask(cmpResult);
                while (mask != 0)
                {
                    int bitPos = BitOperations.TrailingZeroCount(mask);
                    int index = bitPos;
                    if (bucket->Keys[index] == key)
                    {
                        value = bucket->Values[index];
                        return true;
                    }
                    mask &= mask - 1;
                }
                value = default;
                return false;
            }

            public bool TryRemove(
                        nuint key,
                        uint hash,
                        Bucket* bucket,
                        out nuint value
            )
            {
                byte ctrl = (byte)((hash >> EntryMaskWidth) & 0x7F);
                Vector128<byte> ctrlVector = Vector128.Create(ctrl);

                Vector128<byte> bucketCtrl = Sse2.LoadVector128((byte*)&bucket->Control);
                Vector128<byte> matchMask = Sse2.CompareEqual(ctrlVector, bucketCtrl);
                int matches = Sse2.MoveMask(matchMask);

                while (matches != 0)
                {
                    int bitPos = BitOperations.TrailingZeroCount(matches);
                    int index = bitPos;
                    if (bucket->Keys[index] == key)
                    {
                        value = bucket->Values[index];
                        bucket->Control[index] = 0xFE;  // mark as DELETED
                        // no need to remove Key/Value
                        return true;
                    }
                    matches &= matches - 1;
                }
                value = default;
                return false;
            }

            public bool TrySet(
                        nuint key,
                        uint hash,
                        Bucket* bucket,
                        nuint value,
                        out bool isNewKey
            )
            {
                byte ctrl = (byte)((hash >> EntryMaskWidth) & 0x7F);         // H2: upper 8 bits (highest bit is naturally 0)
                Vector128<byte> ctrlVector = Vector128.Create(ctrl);

                Vector128<byte> bucketCtrl = Sse2.LoadVector128(
                        (byte*)&bucket->Control);

                // === Phase 1: Look up an existing key ===
                Vector128<byte> matchMask = Sse2.CompareEqual(ctrlVector, bucketCtrl);
                int matches = Sse2.MoveMask(matchMask);

                while (matches != 0)
                {
                    int bitPos = BitOperations.TrailingZeroCount(matches);
                    int index = bitPos;
                    if (bucket->Keys[index] == key)
                    {
                        // Key already exists -> update value
                        bucket->Values[index] = value;
                        isNewKey = false;
                        return true;
                    }
                    matches &= matches - 1;
                }

                // === Phase 2: Track an available insertion slot ===
                int emptyOrDeleted = Sse2.MoveMask(bucketCtrl);
                if (emptyOrDeleted == 0)
                {
                    isNewKey = false;
                    return false;  // bucket full
                }

                int slot = BitOperations.TrailingZeroCount(emptyOrDeleted);
                bucket->Control[slot] = ctrl;
                bucket->Keys[slot] = key;
                bucket->Values[slot] = value;
                isNewKey = true;
                return true;
            }

        }

    }
}
