# HLSF (Hybrid Layer Segregated Fit) Allocator

A constant-time (O(1)) allocator for real-time, high-throughput scenarios. It uses layered size classes with segmented buckets and stores all metadata externally for clean user memory and better cache locality.

## Key Points
- O(1) allocate/free with 10 levels: 64B x 16MB; each level split into 1x/2x/3x segments
- External metadata (`Entry`, `BucketNode`) in dedicated SLABs; user blocks have no headers
- Configurable alignment (－16B); large block cap per allocation: 32MB
- Single-threaded implementation (external sync required for multi-threaded use)

## Quick Use

```csharp
using var hlsf = new HybridLayerSegregatedFitAllocator(64 * 1024 * 1024);
var h = hlsf.Alloc(1024);
Span<byte> s = h.BufferHandle; s[0] = 42;
hlsf.Free(h);
```

## Benchmarks (this run)

Environment: .NET 8 (Release), x64, single-thread, arena=3072 MiB. Results from `StgSharpDebug/HlsfStressTest`.

- Free throughput: 1KB ～ 262,144 ★ 27,062,260.6 free/s
- Alloc+free pairs: 256B ～ 5,000,000 ★ 19,256,409.9 ops/s
- Fragmentation cycle (「512MB): fast recovery in short cycle (see test for details)

Representative allocation throughput (higher is better):

| Size | Ops | alloc/s (no touch) | alloc/s (touch) |
|------|----:|--------------------:|----------------:|
| 64B   | 2,000,000 | 7,902,555.2  | 14,176,544.8 |
| 1KB   | 1,572,864 | 5,794,161.6  | 7,293,856.5  |
| 4KB   |   393,216 | 5,210,132.6  | 6,646,372.3  |
| 32KB  |    49,152 | 5,931,432.3  | 7,719,199.1  |
| 256KB |     6,144 | 14,663,484.5 | 10,425,929.1 |
| 512KB |     3,072 | 11,054,336.1 | 16,245,372.8 |
| 1MB   |     1,536 | 13,391,456.0 | 9,159,212.9  |
| 2MB   |       768 | 17,028,824.8 | 11,962,616.8 |
| 8MB   |       192 | 13,061,224.5 | 9,142,857.1  |
| 32MB  |        48 | 11,707,317.1 | 9,411,764.7  |

Notes:

- Peak no-touch at 2MB (「17.0M alloc/s). Peak touch at 512KB (「16.25M alloc/s).
- Non-aligned sizes (e.g., 768B, 3KB) typically lower due to slicing/boundary handling.

## Detailed Design

### Size Classes and Segments

- Level sizes: `levelSizeArray = [64, 256, 1024, 4096, 16384, 65536, 262144, 1048576, 4194304, 16777216]` (64B ， 4^L)
- Level selection:
  - `GetLevelFromSize(uint S)`: `level = max(0, (log2(S) - 6) / 2)` using `BitOperations.LeadingZeroCount`
- Segment selection (within a level):
  - `DetermineSegmentIndex(size, level) = (int)((size - 1) / levelSizeArray[level])` ★ 0/1/2
- Bucket index: `index = 3*level + segment` (for L ＋ 9); overflow bucket at `^1` for level > 9

### Metadata Structures

- `Entry` (external SLAB): linkage in position chain, `Position`, `Size`, `Level`, `State`
  - Invariants: near neighbors must form a consistent double-linked ring; `Level` setter validates size vs. level window
- `BucketNode` (in user region): `EntryRef`, `NextLevel`, `PreviousLevel`, `IsInBucket`
- Position chain: circular doubly linked list anchored by sentinel `_spareMemory` (`State.Spare`)

### Allocation Flow

1) Compute `level` and `segment` from requested `size`
2) Try pop bucket (`TryPopLevelForAllocation(level, segment, out entry)`) and escalate:
   - Advance `segment` 0★1★2, then `level++`, until `MaxLevel`
   - If still empty: try pop from overflow bucket (L=10, seg=1) or `TryAllocateNew32MBBlock`
3) On success: bound-check the slice offset; set `entry` to `Alloc` using `SetEntryLevel`
4) If remainder exists in the source region, call `SliceMemory(entry->NextNear, remainder)` to produce free chunks into buckets

### Slicing Logic (`SliceMemory`)

- Inputs: `next` entry position, `sizeToSlice`
- Compute `offset` and `end` relative to base buffer; pick starting level by trailing-zero count of `offset`:
  - `i = (BitOperations.TrailingZeroCount(offset) - 6) / 2`, clamped to `＋ MaxLevel`
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
  - Optionally promote `Level` if `Size － levelSizeArray[originLevel] * 4`
  - Ensure `BucketNode.EntryRef` points to the merged entry; caller re-enqueues into the proper bucket

### Buckets

- `PushBucketToLevel(level, segment, node)`: push to bucket head, set `IsInBucket`, maintain prev/next
- `TryPopLevel(level, segment, out node)`: pop head; if empty, return false
- `RemoveFromBucket(level, segment, node)`: unlink from middle or use `TryPopLevel` if it is the head

### Large Allocations and Spare Memory

- Requests > 32MB use `NativeMemory.AlignedAlloc(size, align)` and mark `State.Large`
- When buckets are empty, `TryAllocateNew32MBBlock` carves a 32MB region from `_spareMemory`, builds an `Empty` entry, and advances the spare pointer

### Alignment

- Default `_align = 16`; `AlignOfLevel(level)` returns `min(levelSizeArray[level], _align)`
- Internal blocks align to level boundaries by design of slicing/edge calculations

### Complexity and Invariants

- Allocate/free are O(1) on average with bounded levels (＋10) and constant-time bucket ops
- Invariants guarded by exceptions (e.g., `HLSFPositionChainBreakException`) and pointer consistency checks

## Profiling (hotspots, current workloads)

- `SliceMemory` 「22% (self 「17%) ！ slicing and bucket enqueue dominate allocation path
- `MergeAndRemoveFromBuckets` 「13% (self 「12%) ！ merge + removal dominate free path
- `Collect` 「7.6% (self 「7.5%) ！ low overall cost

Suggested focus: reduce per-slice math and bucket ops; add fast-paths for fully aligned regions; batch/lazy removals after merges.
