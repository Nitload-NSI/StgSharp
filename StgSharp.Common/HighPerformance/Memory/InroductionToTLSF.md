# TLSF (Two-Layer Segregated Fit) Allocator

This document describes the current StgSharp native arena allocator, `StgSharp.HighPerformance.Memory.TwoLayerSegregatedFitAllocator`. The file name is historical; the implementation described below is TLSF rather than the older HLSF allocator.

## Overview

- Single contiguous native arena allocated up front
- Headerless returned blocks; allocator metadata is stored in 32-byte nodes carved from the tail of the same arena
- Requests rounded up to 64-byte granularity, with 64 bytes as the minimum block size
- Free blocks grouped by coarse size level derived from the highest set bit of the block size
- Exact-level allocation uses first-fit traversal inside the selected bucket; higher levels fall back to the head of the first non-empty bucket
- `Free` performs bounded local coalescing; `Collect` performs full free-run coalescing across the arena
- Single-threaded implementation; concurrent access requires external synchronization
- No arena growth and no implicit fallback to `NativeMemory.AlignedAlloc` for oversized requests

## Quick Use

```csharp
using StgSharp.HighPerformance.Memory;

using var tlsf = new TwoLayerSegregatedFitAllocator(64 * 1024 * 1024);
var handle = tlsf.Alloc(1024);

Span<byte> buffer = handle.AsSpan();
buffer[0] = 42;

tlsf.Free(handle);
```

## Constructors

- `TwoLayerSegregatedFitAllocator(nuint byteSize)`
- `TwoLayerSegregatedFitAllocator(nuint byteSize, uint maxBlockSize)`
- `TwoLayerSegregatedFitAllocator(nuint byteSize, int align)`
- `TwoLayerSegregatedFitAllocator(nuint byteSize, int align, nuint maxBlockSize)`

Behavior summary:

- `byteSize` defines the total arena size
- `maxBlockSize` defaults to 32 MB and defines the largest single request accepted by `Alloc` and `TryAlloc`
- `align` is passed to the backing `NativeMemory.AlignedAlloc` call for arena allocation; request sizes inside the arena are still quantized to 64-byte blocks

## Public API

### `Alloc(nuint size)`

- Rounds `size` up to the next 64-byte block, with a minimum of 64 bytes
- Throws `OverflowException` if the normalized size exceeds `maxBlockSize` or if the arena cannot provide a block
- Returns a `Handle` containing block address and block size
- Splits a sufficiently large tail remainder into a new free node and re-buckets that node

### `TryAlloc(nuint size, out Handle handle)`

- Uses the same size normalization and maximum block limit as `Alloc`
- Returns `false` when the request exceeds the configured limit or when the arena is exhausted
- Does not fall back to the system allocator

### `Free(Handle handle, int collectScanWindow = 4)`

- Marks the block as free
- Scans up to `collectScanWindow` neighbors toward lower addresses and then up to `collectScanWindow` neighbors toward higher addresses
- Merges any contiguous free neighbors found during those scans
- Reclassifies the merged block back into the coarse bucket set

### `Collect()`

- Walks the full position chain from low address to high address
- Merges consecutive free runs without the local scan-window limit used by `Free`
- Restores larger reusable spans and improves bucket coverage after fragmentation-heavy phases

### `Dispose()`

- Releases the backing arena through the stored free callback

### `Handle`

- Readonly value type carrying the allocator metadata reference, block address, and block size
- `Address` exposes the native pointer
- `Size` exposes the normalized block size
- `AsSpan(int range = -1)` exposes a span view over the block
- The indexer provides byte-level access by offset

## Allocation Model

### Level Classification

- `GetLevel(size)` maps the normalized size to `57 - BitOperations.LeadingZeroCount(size)` and clamps the result to `_maxLevel`
- Each bucket corresponds to one coarse size class rather than an exact size
- The target bucket can therefore contain blocks of different sizes within the same coarse level

### Allocation Path

1. Normalize the requested size to a 64-byte multiple.
2. Traverse the target bucket and select the first free block whose size is at least the requested size.
3. If the target bucket has no suitable block, pop the head of the first non-empty higher-level bucket.
4. If all buckets are empty, carve a fresh block from the spare region tracked by the arena sentinel.
5. Split the tail remainder when both remainder size and metadata availability allow it.

### Metadata Layout

- `Metadata` is a 32-byte node
- `NextNear` and `PreviousNear` form the address-ordered position chain
- `NextLevel` and `PreviousLevel` form either the bucket chain or the recycled-metadata chain, depending on node state
- The spare sentinel at the end of the arena tracks remaining virgin space, recycled metadata nodes, and position-chain anchors
- Returned blocks remain headerless from the caller's point of view

### Coalescing Model

- `Free` performs bounded local coalescing to keep the hot path short
- `Collect` performs full coalescing of consecutive free runs across the entire position chain
- Recycled metadata nodes are returned to the spare metadata list and reused by later splits or fresh arena chunks

## Complexity and Runtime Characteristics

- `Alloc`: O(1) bucket index calculation plus target-level first-fit traversal; higher-level fallback is O(1)
- `Free`: O(`collectScanWindow`) local merge cost
- `Collect`: O(number of metadata nodes in the position chain)
- Best steady-state behavior appears on repeated small and medium native allocations inside a pre-allocated arena

## BenchmarkDotNet (TLSF vs Libc)

Environment: .NET 8.0.26 (PerformanceDebug), x64 RyuJIT AVX2, Windows 11, AMD Ryzen 7 6800H, BenchmarkDotNet v0.15.8, InProcessEmitToolchain, LaunchCount=1, WarmupCount=6, IterationCount=15, InvocationCount=3.

Workload summary:

- Arena size: 512 MB
- Arena alignment: 64 bytes
- Allocation count per batch: 2048
- Batch count: 256
- Request set before normalization: 64, 100, 200, 400, 1024, 4096 bytes
- Free order: shuffled per batch
- `Free` local merge window: 4

Best observed run under the current harness:

| Method | Mean | Error | StdDev | Ratio | RatioSD | Allocated | Alloc Ratio |
|------- |-----:|------:|-------:|------:|--------:|----------:|------------:|
| Tlsf | 79.11 ns | 5.944 ns | 5.560 ns | 0.65 | 0.09 | - | NA |
| Libc | 123.64 ns | 15.955 ns | 14.144 ns | 1.01 | 0.16 | - | NA |

Worst observed run under the same harness:

| Method | Mean | Error | StdDev | Ratio | RatioSD | Allocated | Alloc Ratio |
|------- |-----:|------:|-------:|------:|--------:|----------:|------------:|
| Tlsf | 82.95 ns | 6.337 ns | 5.617 ns | 0.81 | 0.07 | - | NA |
| Libc | 102.63 ns | 8.590 ns | 7.173 ns | 1.00 | 0.09 | - | NA |

Interpretation:

- The current single-pass bucket selection fast path materially reduces allocator-side latency
- The reported ratio still drifts across repeated compile-and-run cycles because the libc baseline also moves under the current InProcess harness
- Recent repeated runs place the transient best case near 0.65 and the longer-running steady band near 0.75 to 0.80

## Operational Notes

- Maximum single-allocation size is fixed at construction time
- Arena growth is not implemented
- `Collect` is the global defragmentation pass; `Free` only performs bounded local coalescing
- The allocator is intended for explicit native-memory workflows rather than as a drop-in general-purpose heap replacement