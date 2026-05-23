//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="TlsfAllocatorBench"
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
using BenchmarkDotNet.Attributes;
using StgSharp.HighPerformance.Memory;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Tlsf = StgSharp.HighPerformance.Memory.TwoLayerSegregatedFitAllocator;

namespace StgSharp.Benchmark
{
    [MemoryDiagnoser]
    public class TlsfAllocatorBench
    {

        private const uint Alignment = 64;

        private const int AllocationCount = 2048;

        private const int BatchCount = 256;

        private const int TotalOperationsPerInvoke = BatchCount * AllocationCount;

        private static readonly int[] AllocSizes = [
            64,
            100,
            200,
            400,
            1024,
            4096
        ];
        private readonly int[] _freeOrderByBatch = new int[TotalOperationsPerInvoke];
        private readonly uint[] _requestSizesByBatch = new uint[TotalOperationsPerInvoke];
        private readonly List<nint> _libcLocalList = new();
        private readonly List<Tlsf.Handle> _tlsfLocalList = new();
        private Random _random = new(42);
        private TwoLayerSegregatedFitAllocator _tlsfAllocator = null!;

        [GlobalCleanup]
        public void Cleanup()
        {
            _tlsfAllocator?.Dispose();
            _tlsfLocalList.Clear();
            _tlsfLocalList.TrimExcess();
            _libcLocalList.Clear();
            _libcLocalList.TrimExcess();
        }

        [Benchmark(OperationsPerInvoke = TotalOperationsPerInvoke)]
        public void Tlsf()
        {
            for (int batch = 0; batch < BatchCount; batch++) {
                RunTlsf(batch, _tlsfAllocator, 4);
            }
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            _tlsfAllocator?.Dispose();
            _tlsfAllocator = null!;
            _tlsfLocalList.Clear();
            _libcLocalList.Clear();
        }

        [IterationSetup]
        public void IterationSetup()
        {
            _tlsfAllocator?.Dispose();
            _random = new Random(42);
            _tlsfAllocator = new TwoLayerSegregatedFitAllocator(GetArenaBytes(), align:(int)Alignment);
            BuildIterationWorkload();
            _tlsfLocalList.Clear();
            _libcLocalList.Clear();
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = TotalOperationsPerInvoke)]
        public unsafe void Libc()
        {
            for (int batch = 0; batch < BatchCount; batch++) {
                RunLibc(batch);
            }
        }

        private static uint AlignSize(
                            uint size
        )
        {
            uint mask = Alignment - 1;
            return (size + mask) & ~mask;
        }

        private void BuildIterationWorkload()
        {
            for (int batch = 0; batch < BatchCount; batch++)
            {
                int batchOffset = batch * AllocationCount;
                for (int i = 0; i < AllocationCount; i++)
                {
                    _requestSizesByBatch[batchOffset + i] = AlignSize((uint)AllocSizes[_random.Next(AllocSizes.Length)]);
                    _freeOrderByBatch[batchOffset + i] = i;
                }

                ShuffleFreeOrder(batchOffset, AllocationCount);
            }
        }

        private static nuint GetArenaBytes()
        {
            const int defaultMb = 512;
            const int minMb = 64;
            int mb = defaultMb;

            string? env = Environment.GetEnvironmentVariable("TLSF_BENCH_ARENA_MB");
            if (string.IsNullOrWhiteSpace(env)) {
                env = Environment.GetEnvironmentVariable("HLSF_BENCH_ARENA_MB");
            }
            if (!string.IsNullOrWhiteSpace(env) &&
                int.TryParse(env, out int parsed) &&
                parsed >= minMb) {
                mb = parsed;
            }

            return (nuint)mb * 1024u * 1024u;
        }

        private void RunTlsf(
                     int batch,
                     TwoLayerSegregatedFitAllocator allocator,
                     int scanWindow = 4
        )
        {
            int batchOffset = batch * AllocationCount;
            _tlsfLocalList.Clear();

            for (int i = 0; i < AllocationCount; i++)
            {
                uint size = _requestSizesByBatch[batchOffset + i];
                Tlsf.Handle allocate = allocator.Alloc(size);
                _tlsfLocalList.Add(allocate);
            }

            for (int i = 0; i < _tlsfLocalList.Count; i++) {
                allocator.Free(_tlsfLocalList[_freeOrderByBatch[batchOffset + i]], scanWindow);
            }
        }

        private unsafe void RunLibc(
                            int batch
        )
        {
            int batchOffset = batch * AllocationCount;
            _libcLocalList.Clear();

            for (int i = 0; i < AllocationCount; i++)
            {
                uint size = _requestSizesByBatch[batchOffset + i];
                void* allocate = NativeMemory.AlignedAlloc(size, Alignment);
                if (allocate == null) {
                    throw new OutOfMemoryException("NativeMemory.AlignedAlloc returned null.");
                }
                _libcLocalList.Add((nint)allocate);
            }

            for (int i = 0; i < _libcLocalList.Count; i++) {
                NativeMemory.AlignedFree((void*)_libcLocalList[_freeOrderByBatch[batchOffset + i]]);
            }
        }

        private void ShuffleFreeOrder(
                     int offset,
                     int count
        )
        {
            for (int i = count - 1; i > 0; i--)
            {
                int swapIndex = _random.Next(i + 1);
                int left = offset + i;
                int right = offset + swapIndex;
                (_freeOrderByBatch[left], _freeOrderByBatch[right]) =
                    (_freeOrderByBatch[right], _freeOrderByBatch[left]);
            }
        }

    }
}
