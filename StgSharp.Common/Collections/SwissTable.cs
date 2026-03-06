//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="SwissTable"
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
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;

namespace StgSharp.Collections
{
    /// <summary>
    ///
    /// </summary>
    public sealed unsafe partial class SwissTable
    {

        private const int BucketCount = 1 << EntryMaskWidth;
        private const int BucketSize = 16;
        private const uint CrcMagic = 0xffff9961; // Magic number for CRC32C, used in hash computation
        private const int EntryMaskWidth = 10; // 1024 entries total, 16 per bucket
        public const int MaxCapacity = BucketCount * BucketSize;   // 256 entries per bucket, 1024 buckets

        private readonly Entry* _entries;

        private SwissTable(Entry* buffer)
        {
            _entries = buffer;
        }

        public static SwissTable Create()
        {
            Entry* buffer = (Entry*)NativeMemory.AlignedAlloc((nuint)Unsafe.SizeOf<Entry>(), 16);
            for (int i = 0; i < BucketCount; i++)
            {
                // Set all control bytes to 0xFF (empty)
                Unsafe.InitBlock(&(*buffer)[i].Control, 0xFF, BucketSize);
            }
            return new SwissTable(buffer);
        }

        public bool TryAddOrSet(nint key, nint value, out bool isNewKey)
        {
            uint hash = BitOperations.Crc32C(CrcMagic, (ulong)key);
            ushort entry = (ushort)(hash & (BucketCount - 1));
            Bucket* bucket = &(*_entries)[entry];
            return IBucketScanner.Current.TrySet(key, hash, bucket, value, out isNewKey);
        }

        public bool TryGet(nint key, out nint value)
        {
            uint hash = BitOperations.Crc32C(CrcMagic, (ulong)key);
            ushort entry = (ushort)(hash & (BucketCount - 1));
            Bucket* bucket = &(*_entries)[entry];
            return IBucketScanner.Current.TryFind(key, hash, bucket, out value);
            /*
            * hash = CRC32 of key
            * entry = low 8 of hash
            * ctrl  = 8-15 of hash
            * index = rest of hash
            * find value of hash
            */
        }

        public bool TryRemove(nint key, out nint value)
        {
            uint hash = BitOperations.Crc32C(CrcMagic, (ulong)key);
            ushort entry = (ushort)(hash & (BucketCount - 1));
            Bucket* bucket = &(*_entries)[entry];
            return IBucketScanner.Current.TryRemove(key, hash, bucket, out value);
        }

        ~SwissTable()
        {
            NativeMemory.AlignedFree(_entries);
        }

        [InlineArray(BucketCount)]
        private struct Entry
        {

            public Bucket Bucket;

        }

        private unsafe struct Bucket
        {

            public BucketControl Control;
            public BucketKeyStorage Keys;
            public BucketValueStorage Values;

        }

        private interface IBucketScanner
        {

            static IBucketScanner Current { get; } = Create();

            bool TryFind(nint key, uint Hash, Bucket* bucket, out nint value);

            unsafe bool TryRemove(nint key, uint hash, Bucket* bucket, out nint value);

            bool TrySet(nint key, uint hash, Bucket* bucket, nint value, out bool isNewKey);

            private static IBucketScanner Create()
            {
                if (Sse42.IsSupported)
                {
                    return new BucketScannerX86();
                } else if (ArmBase.IsSupported)
                {
                    throw new NotSupportedException("ARM architecture is currently not supported for bucket scanning. Please use an x86 platform.");
                } else
                {
                    throw new NotSupportedException("No suitable bucket scanner implementation found for the current platform.");
                }
            }

        }

        [InlineArray(BucketSize)]
        private struct BucketControl
        {

            public byte Control;

        }

        [InlineArray(BucketSize)]
        private struct BucketKeyStorage
        {

            public nint value;

        }

        [InlineArray(BucketSize)]
        private struct BucketValueStorage
        {

            public nint value;

        }

    }
}
