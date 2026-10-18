// -----------------------------------------------------------------------------
// file="SwissX86DisassemblyBench"
// Project: nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Nitload.Common.HighPerformance.Memory;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;

namespace Nitload.Benchmark
{
    [DisassemblyDiagnoser(maxDepth: 3, exportHtml: true)]
    public unsafe class SwissX86DisassemblyBench
    {

        private const int ResidentCount = 8192;

        private byte* _buffer;
        private L4.SwissTable _table;
        private byte* _readBuffer;
        private byte* _writeBuffer;
        private L4.AddressSource _hitKey;
        private L4.AddressSource _missKey;
        private ulong _value;

        [GlobalSetup]
        public void Setup()
        {
            if (!Sse42.X64.IsSupported)
                throw new PlatformNotSupportedException("The Swiss X86 disassembly probe requires x64 SSE4.2.");

            _buffer = (byte*)NativeMemory.AlignedAlloc((nuint)L4.BufferSize, 64);
            if (_buffer == null)
                throw new OutOfMemoryException("Could not allocate Swiss disassembly buffers.");

            _table = L4.SwissTable.Create(_buffer);
            for (int index = 0; index < ResidentCount; index++)
            {
                if (!L4.SwissWriterX86.TryAddOrSet(_table.WriteBufferForLockFreeBenchmark,
                                                   MakeKey(index), (ulong)index + 1))
                    throw new InvalidOperationException($"Failed to seed Swiss key {index}.");
            }

            _table.SwapBuffers();
            _readBuffer = _table.ReadBufferForLockFreeBenchmark;
            _writeBuffer = _table.WriteBufferForLockFreeBenchmark;
            for (int index = 0; index < ResidentCount; index++)
            {
                if (!L4.SwissWriterX86.TryAddOrSet(_writeBuffer, MakeKey(index), (ulong)index + 1))
                    throw new InvalidOperationException($"Failed to seed inactive Swiss key {index}.");
            }

            _hitKey = MakeKey(ResidentCount / 2);
            _missKey = MakeKey(ResidentCount + 1);
            _value = 0x1234_5678;

            if (!L4.SwissReaderX86.TryGet(_readBuffer, _hitKey, out _) ||
                L4.SwissReaderX86.TryGet(_readBuffer, _missKey, out _))
                throw new InvalidOperationException("Swiss reader setup validation failed.");
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _table.Dispose();
            NativeMemory.AlignedFree(_buffer);
            _buffer = null;
        }

        [Benchmark]
        public bool RawReaderHit()
        {
            return L4.SwissReaderX86.TryGet(_readBuffer, _hitKey, out _value);
        }

        [Benchmark]
        public bool RawReaderMiss()
        {
            return L4.SwissReaderX86.TryGet(_readBuffer, _missKey, out _value);
        }

        [Benchmark]
        public bool RawWriterUpsertExisting()
        {
            return L4.SwissWriterX86.TryAddOrSet(_writeBuffer, _hitKey, _value);
        }

        [Benchmark]
        public bool RawWriterRemoveAndReinsert()
        {
            return L4.SwissWriterX86.TryRemove(_writeBuffer, _hitKey, out ulong previousValue) &&
                   L4.SwissWriterX86.TryAddOrSet(_writeBuffer, _hitKey, previousValue);
        }

        [Benchmark]
        public bool TableReaderHit()
        {
            return _table.TryGet(_hitKey, out _value);
        }

        [Benchmark]
        public bool TableReaderMiss()
        {
            return _table.TryGet(_missKey, out _value);
        }

        [Benchmark]
        public bool TableWriterUpsertExisting()
        {
            return _table.TryAddOrSet(_hitKey, _value);
        }

        [Benchmark]
        public bool TableWriterRemoveAndReinsert()
        {
            return _table.TryRemove(_hitKey, out ulong previousValue) &&
                   _table.TryAddOrSet(_hitKey, previousValue);
        }

        private static L4.AddressSource MakeKey(int index)
        {
            return new L4.AddressSource
            {
                Address = 0x1000_0000UL + ((ulong)index * 64UL),
                Description = ((ulong)index * 0x9e37_79b9UL) ^ 0xd1b5_4a32_d192_ed03UL
            };
        }

    }
}