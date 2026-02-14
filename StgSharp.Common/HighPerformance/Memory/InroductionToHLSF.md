# HLSF (Hybrid Layer Segregated Fit) Allocator

A constant-time (O(1)) allocator for real-time, high-throughput scenarios. It uses layered size classes with segmented buckets and stores all metadata externally for clean user memory and better cache locality.

## Key Points

- O(1) allocate/free with 10 levels: 64B x 16MB; each level split into 1x/2x/3x segments
- External metadata (`Entry`, `BucketNode`) in dedicated SLABs; user blocks have no headers
- Configurable alignment (>=16B); maximum block size is configurable at construction time via the `maxBlockSize` parameter (default 32MB, 10 levels). The level array and bucket table are dynamically extended when a larger `maxBlockSize` is specified. There is no implicit fallback to `NativeMemory` for oversized requests.
- Single-threaded implementation (external sync required for multi-threaded use)

## Quick Use

```csharp
using var hlsf = new HybridLayerSegregatedFitAllocator(64 * 1024 * 1024);
var h = hlsf.Alloc(1024);
Span<byte> s = h.BufferHandle; s[0] = 42;
hlsf.Free(h);
```

## Core APIs

### Alloc(uint size)

- Purpose: Allocate a block of at least `size` bytes.
- Input: `size` in bytes. Must not exceed the `maxBlockSize` configured at construction time (default 32MB). If `size > _maxSize`, an `InvalidOperationException` is thrown.
- Output: `HybridLayerSegregatedFitAllocationHandle` with pointer access and `Span<byte>`.
- Behavior:
  - Classify into level/segment and pop a matching bucket; escalate segments and levels if empty.
  - Fallback to overflow bucket or carve 32MB from spare memory if all buckets are empty.
  - Mark selected `Entry` as Alloc; slice any remainder into `Empty` entries and push to buckets.
  - There is no implicit fallback to `NativeMemory.AlignedAlloc` for oversized requests — all allocations are served from the pre-allocated arena. The level array and bucket table are dynamically sized based on the `maxBlockSize` parameter.

### Free(HybridLayerSegregatedFitAllocationHandle handle)

- Purpose: Free a previously allocated block.
- Behavior:
  - Mark the corresponding `Entry` as `Empty`.
  - Merge with adjacent `Empty` entries in both directions (boundary and level constraints).
  - Remove merged neighbors from buckets, update position chain, and optionally promote level.
  - Re-enqueue the final `Entry` into the appropriate bucket.

### Collect()

- Purpose: Reclaim and rebalance free entries without touching allocated blocks.
- Behavior:
  - Consolidate fragmented regions where possible and improve bucket coverage.
  - Intended to be cheap; safe to call between benchmark phases.

### BenchmarkDotNet (HLSF vs Libc)

Environment: .NET 8.0.23 (Release), x64 RyuJIT AVX2, Windows 11, AMD Ryzen 7 6800H.

| Method | Mean | StdDev | Ratio | Allocated |
|------- |-----:|------:|------:|----------:|
| Hlsf | 838.4 µs | 40.6 µs | 1.00 | 64 B |
| Libc | 849.8 µs | 100.2 µs | 1.01 | 64 B |

Interpretation: HLSF matches libc allocation throughput under the same workload, with slightly tighter dispersion in this run.

> Note: BenchmarkDotNet also warned that the minimum iteration time was under 1ms; for more stable statistics, increase per-iteration work (e.g., higher `AllocationCount`).

## Detailed Design (summary)
- 
- Size classes: The default level size array is [64, 256, 1024, 4096, 16384, 65536, 262144, 1048576, 4194304, 16777216] (10 levels, 64B × 4^level). When `maxBlockSize` is specified at construction and exceeds the default range, additional levels are appended (each 4× the previous) to cover the requested maximum block size.
- Segments per level: 1x, 2x, 3x
- Buckets: index = 3 * level + segment (overflow bucket for level > 9)
- Metadata: Entry (external SLAB, position chain), BucketNode (user region, bucket chain)
- Position chain: circular doubly linked list anchored by spare sentinel
- Allocation: search buckets by level/segment with escalation; if none, carve 32MB from spare; set Alloc; slice remainder into buckets
- Slicing: align to level boundaries, pack contiguous blocks forward/backward to minimize overhead
- Free/Merge: bidirectional merge under boundary and level constraints; update buckets; optional level promotion
- Alignment: default 16B; internal blocks aligned by slicing/edge calculation
- Complexity: average O(1) with bounded levels and constant-time bucket ops

- `Entry` (external SLAB): linkage in position chain, `Position`, `Size`, `Level`, `State`
  - Invariants: near neighbors must form a consistent double-linked ring; `Level` setter validates size vs. level window
- `BucketNode` (in user region): `EntryRef`, `NextLevel`, `PreviousLevel`, `IsInBucket`
- Position chain: circular doubly linked list anchored by sentinel `_spareMemory` (`State.Spare`)

### Allocation Flow

1) Compute `level` and `segment` from requested `size`
2) Try pop bucket (`TryPopLevelForAllocation(level, segment, out entry)`) and escalate:
   - Advance `segment` 0��1��2, then `level++`, until `MaxLevel`
   - If still empty: try pop from overflow bucket (L=10, seg=1) or `TryAllocateNew32MBBlock`
3) On success: bound-check the slice offset; set `entry` to `Alloc` using `SetEntryLevel`
4) If remainder exists in the source region, call `SliceMemory(entry->NextNear, remainder)` to produce free chunks into buckets

### Slicing Logic (`SliceMemory`)

- Inputs: `next` entry position, `sizeToSlice`
- Compute `offset` and `end` relative to base buffer; pick starting level by trailing-zero count of `offset`:
  - `i = (BitOperations.TrailingZeroCount(offset) - 6) / 2`, clamped to `�� MaxLevel`
- Three phases to cover [offset, end):
  1) Forward levels i..N-1: for each level size `size` and `upper=size*4`, align to `edge=((offset+upper-1)/upper)*upper`, pack `count = ((min(edge,end) - offset) / size)` blocks
  2) If `i > MaxLevel`: pack in largest upper window on tail
  3) Backward i-1..0: fill remaining tail with smaller sizes
- For each packed region: allocate new `Entry`, insert before `next`, mark `Empty`, and push corresponding `BucketNode` into the `(level, segment)` bucket (`PushBucketToLevel(i, count-1, b)`)

### Free and Merge (`MergeAndRemoveFromBuckets`)

- Given an `Entry` to free (turned `Empty` by caller):
  - Merge left while neighbor `p` is `Empty`, same `Level`, and within boundary (`AssertBoundary`)
    - Remove `p` from bucket (`RemoveFromBucket`), expand current, fix `Position`, unlink `p`
  - Merge right with neighbor `n` by the same rule
  - Optionally promote `Level` if `Size �� levelSizeArray[originLevel] * 4`
  - Ensure `BucketNode.EntryRef` points to the merged entry; caller re-enqueues into the proper bucket

### Buckets

- `PushBucketToLevel(level, segment, node)`: push to bucket head, set `IsInBucket`, maintain prev/next
- `TryPopLevel(level, segment, out node)`: pop head; if empty, return false
- `RemoveFromBucket(level, segment, node)`: unlink from middle or use `TryPopLevel` if it is the head

### Large Allocations and Spare Memory

- Requests exceeding the configured `maxBlockSize` are rejected with `InvalidOperationException`. All allocations are served from the pre-allocated arena.
- When all buckets are empty, `TryAllocateNew32MBBlock` carves a region (up to 32MB) from `_spareMemory` to replenish the bucket system, builds an `Empty` entry, and advances the spare pointer

### Alignment

- Default `_align = 16`; `AlignOfLevel(level)` returns `min(levelSizeArray[level], _align)`
- Internal blocks align to level boundaries by design of slicing/edge calculations

### Complexity and Invariants

- Allocate/free are O(1) on average with bounded levels (��10) and constant-time bucket ops
- Invariants guarded by exceptions (e.g., `HLSFPositionChainBreakException`) and pointer consistency checks

## Profiling (hotspots, current workloads)

- SliceMemory ~22% (self ~17%) �� slicing and bucket enqueue dominate allocation path
- MergeAndRemoveFromBuckets ~13% (self ~12%) �� merge + removal dominate free path
- Collect ~7.6% (self ~7.5%) �� low overall cost

Focus: reduce per-slice math and bucket ops; add fast-paths for aligned regions; batch/lazy removals after merges. 