// -----------------------------------------------------------------------------
// file="SwissTable"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

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
    public sealed unsafe partial class SwissTable : IDisposable
    {

        private const int BucketCount = 1 << EntryMaskWidth;
        private const int BucketSize = 16;
        private const uint CrcMagic = 0xffff9961; // Magic number for CRC32C, used in hash computation
        private const int EntryMaskWidth = 10; // 1024 entries total, 16 per bucket
        public const int MaxCapacity = BucketCount * BucketSize;   // 256 entries per bucket, 1024 buckets

        public static readonly nuint RequestedNativeMemorySize = (nuint)Unsafe.SizeOf<Entry>();

        private readonly Entry* _entries;
        private readonly Action<nuint> _freeHandle;
        private bool _disposed;

        private SwissTable(
                Entry* buffer,
                Action<nuint> freeHandle
        )
        {
            _entries = buffer;
            _freeHandle = freeHandle;
        }

        public static SwissTable Create()
        {
            Entry* buffer = (Entry*)NativeMemory.AlignedAlloc((nuint)Unsafe.SizeOf<Entry>(), 16);
            for (int i = 0; i < BucketCount; i++)
            {
                // Set all control bytes to 0xFF (empty)
                Unsafe.InitBlock(&(*buffer)[i].Control, 0xFF, BucketSize);
            }
            return new SwissTable(buffer, handle => NativeMemory.AlignedFree((void*)handle));
        }

        public static SwissTable Create(
                                 Func<nuint, nuint> allocator,
                                 Action<nuint> freeHandle
        )
        {
            Entry* buffer = (Entry*)allocator(RequestedNativeMemorySize);
            for (int i = 0; i < BucketCount; i++)
            {
                // Set all control bytes to 0xFF (empty)
                Unsafe.InitBlock(&(*buffer)[i].Control, 0xFF, BucketSize);
            }
            return new SwissTable(buffer, freeHandle);
        }

        public void Dispose()
        {
            if (_disposed) {
                return;
            }
            _disposed = true;
            _freeHandle((nuint)_entries);
            GC.SuppressFinalize(this);
        }

        public bool TryAddOrSet(
                    nuint key,
                    nuint value,
                    out bool isNewKey
        )
        {
            uint hash = BitOperations.Crc32C(CrcMagic, (ulong)key);
            ushort entry = (ushort)(hash & (BucketCount - 1));
            Bucket* bucket = &(*_entries)[entry];
            return IBucketScanner.Current.TrySet(key, hash, bucket, value, out isNewKey);
        }

        public bool TryGet(
                    nuint key,
                    out nuint value
        )
        {
            uint hash = BitOperations.Crc32C(CrcMagic, key);
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

        public bool TryRemove(
                    nuint key,
                    out nuint value
        )
        {
            uint hash = BitOperations.Crc32C(CrcMagic, key);
            ushort entry = (ushort)(hash & (BucketCount - 1));
            Bucket* bucket = &(*_entries)[entry];
            return IBucketScanner.Current.TryRemove(key, hash, bucket, out value);
        }

        internal static SwissTable Create(
                                   nuint bufferPtr,
                                   Action<nuint> FreeHandle
        )
        {
            Entry* buffer = (Entry*)bufferPtr;
            for (int i = 0; i < BucketCount; i++)
            {
                // Set all control bytes to 0xFF (empty)
                Unsafe.InitBlock(&(*buffer)[i].Control, 0xFF, BucketSize);
            }
            return new SwissTable(buffer, FreeHandle);
        }

        ~SwissTable()
        {
            Dispose();
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

            bool TryFind(
                 nuint key,
                 uint Hash,
                 Bucket* bucket,
                 out nuint value
            );

            unsafe bool TryRemove(
                        nuint key,
                        uint hash,
                        Bucket* bucket,
                        out nuint value
            );

            bool TrySet(
                 nuint key,
                 uint hash,
                 Bucket* bucket,
                 nuint value,
                 out bool isNewKey
            );

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

            public nuint value;

        }

        [InlineArray(BucketSize)]
        private struct BucketValueStorage
        {

            public nuint value;

        }

    }
}
