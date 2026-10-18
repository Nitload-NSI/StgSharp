// -----------------------------------------------------------------------------
// file="SwissX86Bench"
// Project: nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Nitload.Common.HighPerformance.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Threading;
using System.Threading.Tasks;

namespace Nitload.Benchmark
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public unsafe class SwissX86Bench
    {

        private const int QueryCount = 1 << 21;
        private const int ConcurrentQueryCount = QueryCount * 6;
        private const int MaximumResidentCount = 8192;

        private byte* _buffer;
        private L4.SwissTable _swiss;
        private Dictionary<L4.AddressSource, ulong> _dictionary = null!;
        private ConcurrentDictionary<L4.AddressSource, ulong> _concurrentDictionary = null!;
        private L4.AddressSource[] _hitQueries = [];
        private L4.AddressSource[] _missQueries = [];
        private L4.AddressSource[] _writerQueries = [];
        private ulong[] _writerValues = [];
        private byte* _readBuffer;
        private byte* _writeBuffer;
        private ParallelOptions _parallelOptions = null!;
        private Func<ulong> _parallelLocalInit = null!;
        private Func<int, ParallelLoopState, ulong, ulong> _swissParallelBody = null!;
        private Func<int, ParallelLoopState, ulong, ulong> _concurrentDictionaryParallelBody = null!;
        private Action<ulong> _parallelLocalFinally = null!;
        private long _parallelChecksum;

        [Params(25, 50, 100)]
        public int LoadPercent { get; set; }

        private int ResidentCount => MaximumResidentCount * LoadPercent / 100;

        [GlobalSetup]
        public void Setup()
        {
            if (!Sse42.X64.IsSupported)
                throw new PlatformNotSupportedException("The Swiss X86 benchmark requires x64 SSE4.2.");

            _buffer = (byte*)NativeMemory.AlignedAlloc((nuint)L4.BufferSize, 64);
            if (_buffer == null)
                throw new OutOfMemoryException("Could not allocate Swiss benchmark tables.");

            _swiss = L4.SwissTable.Create(_buffer);
            _dictionary = new Dictionary<L4.AddressSource, ulong>(ResidentCount);
            _concurrentDictionary = new ConcurrentDictionary<L4.AddressSource, ulong>(
                Math.Max(1, Math.Min(Environment.ProcessorCount, 8)),
                ResidentCount);

            L4.AddressSource[] residentKeys = new L4.AddressSource[ResidentCount];
            for (int i = 0; i < ResidentCount; i++)
            {
                L4.AddressSource key = MakeKey(i);
                ulong value = (ulong)i + 1;
                residentKeys[i] = key;
                if (!_swiss.TryAddOrSet(key, value))
                    throw new InvalidOperationException($"Swiss insertion failed while seeding key {i}.");
                _dictionary.Add(key, value);
                _concurrentDictionary.TryAdd(key, value);
            }

            _swiss.SwapBuffers();
            for (int i = 0; i < ResidentCount; i++)
            {
                if (!_swiss.TryAddOrSet(residentKeys[i], (ulong)i + 1))
                    throw new InvalidOperationException($"Swiss writer-buffer seeding failed for key {i}.");
            }

            _readBuffer = _swiss.ReadBufferForLockFreeBenchmark;
            _writeBuffer = _swiss.WriteBufferForLockFreeBenchmark;

            _hitQueries = new L4.AddressSource[QueryCount];
            _missQueries = new L4.AddressSource[QueryCount];
            _writerQueries = new L4.AddressSource[QueryCount];
            _writerValues = new ulong[QueryCount];
            Random random = new(0x51_15_2026);
            for (int i = 0; i < QueryCount; i++)
            {
                _hitQueries[i] = residentKeys[random.Next(ResidentCount)];
                _missQueries[i] = MakeKey(MaximumResidentCount + i);
                _writerQueries[i] = residentKeys[random.Next(ResidentCount)];
                _writerValues[i] = (ulong)i + 0x10000;
            }

            _parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = Math.Max(2, Math.Min(Environment.ProcessorCount, 8))
            };
            _parallelLocalInit = static() => 0UL;
            _swissParallelBody = ReadSwissParallel;
            _concurrentDictionaryParallelBody = ReadConcurrentDictionaryParallel;
            _parallelLocalFinally = MergeParallelChecksum;

            ValidateLookupResults();
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _swiss.Dispose();
            NativeMemory.AlignedFree(_buffer);
            _buffer = null;
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = QueryCount)]
        [BenchmarkCategory("Hit lookup")]
        public ulong DictionaryHitLookup()
        {
            ulong checksum = 0;
            for (int i = 0; i < QueryCount; i++)
                checksum += _dictionary[_hitQueries[i]];
            return checksum;
        }

        [Benchmark(OperationsPerInvoke = QueryCount)]
        [BenchmarkCategory("Hit lookup")]
        public ulong SwissLockFreeHitLookup()
        {
            ulong checksum = 0;
            for (int i = 0; i < QueryCount; i++)
            {
                if (!L4.SwissReaderX86.TryGet(_readBuffer, _hitQueries[i], out ulong value))
                    throw new InvalidOperationException("Swiss table missed a seeded hit key.");
                checksum += value;
            }
            return checksum;
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = QueryCount)]
        [BenchmarkCategory("Miss lookup")]
        public int DictionaryMissLookup()
        {
            int misses = 0;
            for (int i = 0; i < QueryCount; i++)
                misses += _dictionary.TryGetValue(_missQueries[i], out _) ? 0 : 1;
            return misses;
        }

        [Benchmark(OperationsPerInvoke = QueryCount)]
        [BenchmarkCategory("Miss lookup")]
        public int SwissLockFreeMissLookup()
        {
            int misses = 0;
            for (int i = 0; i < QueryCount; i++)
                misses += L4.SwissReaderX86.TryGet(_readBuffer, _missQueries[i], out _) ? 0 : 1;
            return misses;
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = ConcurrentQueryCount)]
        [BenchmarkCategory("Concurrent hit lookup")]
        public ulong ConcurrentDictionaryConcurrentHitLookup()
        {
            Interlocked.Exchange(ref _parallelChecksum, 0);
            Parallel.For(
                0,
            ConcurrentQueryCount,
                _parallelOptions,
                _parallelLocalInit,
                _concurrentDictionaryParallelBody,
                _parallelLocalFinally);
            return unchecked((ulong)Interlocked.Read(ref _parallelChecksum));
        }

        [Benchmark(OperationsPerInvoke = ConcurrentQueryCount)]
        [BenchmarkCategory("Concurrent hit lookup")]
        public ulong SwissLockFreeConcurrentHitLookup()
        {
            Interlocked.Exchange(ref _parallelChecksum, 0);
            Parallel.For(
                0,
            ConcurrentQueryCount,
                _parallelOptions,
                _parallelLocalInit,
                _swissParallelBody,
                _parallelLocalFinally);
            return unchecked((ulong)Interlocked.Read(ref _parallelChecksum));
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = QueryCount)]
        [BenchmarkCategory("Upsert existing")]
        public ulong DictionaryUpsertExisting()
        {
            for (int i = 0; i < QueryCount; i++)
                _dictionary[_writerQueries[i]] = _writerValues[i];
            return _dictionary[_writerQueries[^1]];
        }

        [Benchmark(OperationsPerInvoke = QueryCount)]
        [BenchmarkCategory("Upsert existing")]
        public ulong SwissUpsertExisting()
        {
            ulong failures = 0;
            for (int i = 0; i < QueryCount; i++)
                failures += L4.SwissWriterX86.TryAddOrSet(_writeBuffer, _writerQueries[i], _writerValues[i]) ? 0UL : 1UL;
            if (failures != 0 || !L4.SwissReaderX86.TryGet(_writeBuffer, _writerQueries[^1], out ulong value))
                throw new InvalidOperationException("Swiss upsert failed to preserve the final value.");
            return value;
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = QueryCount)]
        [BenchmarkCategory("Remove and reinsert")]
        public ulong DictionaryRemoveAndReinsert()
        {
            ulong checksum = 0;
            for (int i = 0; i < QueryCount; i++)
            {
                L4.AddressSource key = _writerQueries[i];
                ulong value = _dictionary[key];
                _dictionary.Remove(key);
                _dictionary.Add(key, value + 1);
                checksum += value;
            }
            return checksum;
        }

        [Benchmark(OperationsPerInvoke = QueryCount)]
        [BenchmarkCategory("Remove and reinsert")]
        public ulong SwissRemoveAndReinsert()
        {
            ulong checksum = 0;
            for (int i = 0; i < QueryCount; i++)
            {
                L4.AddressSource key = _writerQueries[i];
                if (!L4.SwissWriterX86.TryRemove(_writeBuffer, key, out ulong value) ||
                    !L4.SwissWriterX86.TryAddOrSet(_writeBuffer, key, value + 1)) {
                    throw new InvalidOperationException("Swiss remove/reinsert failed.");
                }
                checksum += value;
            }
            return checksum;
        }

        private static L4.AddressSource MakeKey(int index)
        {
            return new L4.AddressSource
            {
                Address = 0x1000_0000UL + ((ulong)index * 64UL),
                Description = ((ulong)index * 0x9e37_79b9UL) ^ 0xd1b5_4a32_d192_ed03UL
            };
        }

        private ulong ReadSwissParallel(int index, ParallelLoopState state, ulong checksum)
        {
            int queryIndex = index & (QueryCount - 1);
            if (!L4.SwissReaderX86.TryGet(_readBuffer, _hitQueries[queryIndex], out ulong value))
                throw new InvalidOperationException("Swiss table missed a seeded concurrent hit key.");
            return checksum + value;
        }

        private ulong ReadConcurrentDictionaryParallel(int index, ParallelLoopState state, ulong checksum)
        {
            int queryIndex = index & (QueryCount - 1);
            if (!_concurrentDictionary.TryGetValue(_hitQueries[queryIndex], out ulong value))
                throw new InvalidOperationException("ConcurrentDictionary missed a seeded hit key.");
            return checksum + value;
        }

        private void MergeParallelChecksum(ulong checksum)
        {
            _ = Interlocked.Add(ref _parallelChecksum, unchecked((long)checksum));
        }

        private void ValidateLookupResults()
        {
            for (int i = 0; i < _hitQueries.Length; i++)
            {
                bool dictionaryFound = _dictionary.TryGetValue(_hitQueries[i], out ulong dictionaryValue);
                bool swissFound = L4.SwissReaderX86.TryGet(_readBuffer, _hitQueries[i], out ulong swissValue);
                if (!dictionaryFound || !swissFound || dictionaryValue != swissValue)
                    throw new InvalidOperationException("Swiss and Dictionary hit results differ.");

                if (!_concurrentDictionary.TryGetValue(_hitQueries[i], out ulong concurrentValue) ||
                    concurrentValue != dictionaryValue)
                    throw new InvalidOperationException("ConcurrentDictionary and Dictionary hit results differ.");
            }

            for (int i = 0; i < _missQueries.Length; i++)
            {
                bool dictionaryFound = _dictionary.TryGetValue(_missQueries[i], out _);
                bool swissFound = L4.SwissReaderX86.TryGet(_readBuffer, _missQueries[i], out _);
                if (dictionaryFound || swissFound)
                    throw new InvalidOperationException("Swiss and Dictionary miss results differ.");
            }
        }

    }
}