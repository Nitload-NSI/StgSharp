//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="HlsfAllocatorBench"
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
using System.Runtime.InteropServices;

namespace StgSharp.Benchmark
{
    [MemoryDiagnoser]
    [InvocationCount(2048)]
    public class HlsfAllocatorBench
    {

        private const uint Alignment = 64;

        private const int AllocationCount = 2048;

        private static readonly int[] AllocSizes = [
            64,
            96,
            150,
            200,
            400,
            1024,
            4096
        ];
        private HybridLayerSegregatedFitAllocator _hlsfAllocator = null!;
        private readonly List<HybridLayerSegregatedFitAllocationHandle> _hlsfLocalList = new();
        private readonly List<nint> _libcLocalList = new();
        private Random _random = new(42);

        [GlobalCleanup]
        public void Cleanup()
        {
            _hlsfAllocator?.Dispose();
            _hlsfLocalList.Clear();
            _hlsfLocalList.TrimExcess();
            _libcLocalList.Clear();
            _libcLocalList.TrimExcess();
        }

        [Benchmark]
        public void Hlsf()
        {
            RunHlsf(_hlsfAllocator, FreePolicy.NoCollect);
        }

        [IterationSetup]
        public void IterationSetup()
        {
            _hlsfLocalList.Clear();
            _libcLocalList.Clear();
        }

        [Benchmark(Baseline = true)]
        public unsafe void Libc()
        {
            _libcLocalList.Clear();

            for (int i = 0; i < AllocationCount; i++)
            {
                uint size = AlignSize(GetNextRandomSize());
                void* allocate = NativeMemory.AlignedAlloc(size, Alignment);
                if (allocate == null) {
                    throw new OutOfMemoryException("NativeMemory.AlignedAlloc returned null.");
                }
                _libcLocalList.Add((nint)allocate);
            }

            for (int i = 0; i < _libcLocalList.Count; i++) {
                NativeMemory.AlignedFree((void*)_libcLocalList[i]);
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            _random = new Random(42);
            _hlsfAllocator = new HybridLayerSegregatedFitAllocator(GetArenaBytes(), align:(int)Alignment);
        }

        private static uint AlignSize(
                            uint size
        )
        {
            uint mask = Alignment - 1;
            return (size + mask) & ~mask;
        }

        private static nuint GetArenaBytes()
        {
            const int defaultMb = 512;
            const int minMb = 64;
            int mb = defaultMb;

            string? env = Environment.GetEnvironmentVariable("HLSF_BENCH_ARENA_MB");
            if (!string.IsNullOrWhiteSpace(env) &&
                int.TryParse(env, out int parsed) &&
                parsed >= minMb) {
                mb = parsed;
            }

            return (nuint)mb * 1024u * 1024u;
        }

        private uint GetNextRandomSize() => (uint)AllocSizes[_random.Next(AllocSizes.Length)];

        private void RunHlsf(
                     HybridLayerSegregatedFitAllocator allocator,
                     FreePolicy policy
        )
        {
            _hlsfLocalList.Clear();

            for (int i = 0; i < AllocationCount; i++)
            {
                uint size = AlignSize(GetNextRandomSize());
                HybridLayerSegregatedFitAllocationHandle allocate = allocator.Alloc(size);
                _hlsfLocalList.Add(allocate);
            }

            for (int i = 0; i < _hlsfLocalList.Count; i++) {
                allocator.Free(_hlsfLocalList[i], policy);
            }
        }

    }
}
