# L4 Software Cache Design Specification

> Project: StgSharp  
> Author: Nitload  
> Version: Draft v0.3  
> Date: 2026-03-10

---

## 1. Purpose

### 1.1 Problem Background

StgSharp's matrix computation system is migrating from a "single global context table" to a
"per-thread context" architecture to adapt to the scheduling characteristics of hybrid-architecture
CPUs (e.g., Intel P-Core / E-Core). After the migration, each thread independently holds its own
computation context, but matrix data may still be allocated by the HLSF allocator or global
`malloc` on a remote NUMA node, resulting in significantly increased cross-node access latency.

At the same time, in matrix multiplication (FMA), the core row data of matrix A in the
checkerboard-shaped kernel grid layout must be repacked (Packed) into a micro-kernel-friendly
contiguous arrangement. In libraries such as OpenBLAS, this Pack operation is re-executed on
every tile switch, whereas L4 can cache the Pack results to avoid repeated reordering.

Additionally, the segmented accumulation (segment) of matrix multiplication requires storing
intermediate partial sums locally to avoid cross-NUMA writes to the target matrix on every
segment. L4 can serve as a local accumulation buffer for partial sums, writing them back in a
single operation after all segments complete locally.

### 1.2 Solution

The L4 cache is a purely software-implemented **data caching layer** logically positioned
between the CPU hardware caches (L1/L2/L3) and main memory. Its core responsibilities:

1. **Pack result caching**: Cache the repack results of FMA core rows in matrix A to avoid repeated Pack operations
2. **NUMA migration cache**: Migrate data from remote NUMA nodes to local memory and cache it, eliminating cross-node latency
3. **Segment accumulation buffer**: Temporarily store partial sums from FMA segmented computation to reduce remote write operations
4. **PLU intermediate data buffer**: Provide a local contiguous workspace for PLU decomposition

### 1.3 Design Goals

- Provide NUMA-friendly local data access for per-thread matrix computation contexts
- Fixed capacity (64 MB), no runtime expansion, deterministic latency
- Transparent pointer redirection: upper-layer code queries via original pointers; L4 returns local cache pointers
- Cache eviction policy based on **RRPV mask + windowed OPT prediction**
- Support both **StreamTransparent** and **CacheBuffer** working modes
- Support **writable cache lines** that invoke registered callback functions for pre-processing and write-back upon eviction or explicit flush

---

## 2. Overall Architecture

### 2.1 Three-Component Model

```
┌���─────────────────────────────────────────────────────────────────┐
│                              L4                                  │
│                                                                  │
│  ┌──────────────┐  ┌────────────────────┐  ┌──────────────────┐  │
│  │  CacheLine    │  │     L4 Core        │  │    Predictor     │  │
│  │  Handle       │  │     Cache          │  │    Prefetch      │  │
│  │              │  │                    │  │    Predictor     │  │
│  │ • local addr  │  │ • Swiss Table      │  │ • 2048-entry buf │  │
│  │ • ref count   │  │ • Slab Allocator   │  │ • threshold 1024 │  │
│  │ • Dispose     │  │ • RRPV mask        │  │ • windowed OPT   │  │
│  │   drops ref   │  │ • SIMD eviction    │  │ • active key set │  │
│  │              │  │ • writeback mgmt   │  │                  │  │
│  └──────────────┘  └────────────────────┘  └──────────────────┘  │
│                                                                  │
└──────────────────────────────────────────────────────────────────┘
```

### 2.2 Data Access Path

```
Compute thread accessing data:

  ┌─────────────────────────────────────────────────────────┐
  │  Read-only path: L4.Map(originKey)                      │
  │     ├─ StreamTransparent + no repack needed?            │
  │     │  └─ Yes → return original pointer directly        │
  │     │          (zero overhead)                          │
  │     │                                                   │
  │     └─ CacheBuffer mode                                 │
  │        ├─ Swiss Table lookup (2.6ns, lock-free)         │
  │        ├─ Hit → update RRPV=0, return CacheLine handle  │
  │        └─ Miss → SpinWait for Predictor prefetch        │
  ├─────────────────────────────────────────────────────────┤
  │  Writable path: L4.MapWritable(originKey, preprocess, writeback) │
  │     ├─ Hit → return existing partial sum cache line     │
  │     └─ Miss → allocate zero-initialized cache line,     │
  │        register writeback callbacks                     │
  │        cache line marked dirty (dirty=1)                │
  │        eviction invokes preprocess + writeback          │
  ├─────────────────────────────────────────────────────────┤
  │  After computation:                                     │
  │  3. CacheLine.Dispose() (refCount--)                    │
  │  4. L4.StepComplete()                                   │
  │     ├─ Predictor.AdvanceExecutionPoint()                │
  │     └─ Predictor.NeedsPrefetch?                         │
  │        └─ Yes → CAS-acquire lock, execute prefetch      │
  └─────────────────────────────────────────────────────────┘
```

### 2.3 Key Parameters

| Parameter              | Value                    | Description                                          |
|------------------------|--------------------------|------------------------------------------------------|
| Total cache capacity   | 64 MB                    | Per-NUMA-node contiguous memory                      |
| Cache line size        | 8192 Bytes               | = 64 × `double MAT_KERNEL`                           |
| Cache line count       | 8192                     | 64MB / 8192B                                         |
| Mapping table type     | Swiss Table              | 1024 buckets × 16 slots, SIMD-accelerated            |
| Cache line pool        | Slab Allocator           | Non-concurrent, fixed capacity, no auto-expansion    |
| Eviction policy        | RRPV mask + windowed OPT | SoA layout + SSE2 batch scan                         |
| Prefetch policy        | Predictor windowed OPT   | 2048-entry buffer, 1024-step lookahead               |
| Writeback policy       | Callback-driven          | Invokes registered callbacks on dirty eviction/flush |

---

## 3. Key Components

### 3.1 Swiss Table (Pointer Mapping Table)

**Responsibility**: Maintains the `original pointer → cache pointer` mapping.

- Key: original pointer `nint`, pointing to data allocated by HLSF/malloc on a remote node
- Value: cache pointer `nint`, pointing to locally Slab-allocated cache line
- Hash: hardware `CRC32C` instruction on the 64-bit pointer
  - Low 10 bits → bucket index (1024 buckets)
  - Bits 10-16 → H2 control byte (used for SIMD pre-filtering)
- Scan: SSE2 `pmovmskb` instruction for parallel comparison of 16 control bytes
- Tombstone mechanism: deleted entries set ctrl to `0xFE`, preserving probe chain integrity
- Capacity: 1024 × 16 = 16,384 mappings, significantly larger than total cache line count (8192 lines),
  ensuring extremely low hash collision rate

**Measured Performance** (Ryzen 7 6800H, .NET 8, BenchmarkDotNet):

| Operation                | Swiss Table  | Dictionary  | Speedup |
|--------------------------|-------------|-------------|---------|
| Zipfian Lookup (8192×)   | 20.0 μs     | 76.1 μs     | 3.8×   |
| Uniform Lookup (8192×)   | 18.6 μs     | 50.6 μs     | 2.7×   |
| Insert (4096 entries)    | 18.5 μs     | 46.5 μs     | 2.5×   |
| SteadyState (4096 ops)   | 25.3 μs     | 68.3 μs     | 2.7×   |

Single lookup latency ≈ 2.6 ns, GC allocation = 0.

### 3.2 Slab Allocator (Cache Line Pool)

**Responsibility**: Manages allocation and reclamation of fixed-size (8192B) cache lines.

- Mode: **Non-concurrent, fixed capacity** (`FixedSlabAllocator<T>` mode),
  reusing the existing `SlabAllocator.FromBuffer<T>()` interface
- No auto-expansion: capacity is fixed at initialization (64MB / 8192B = 8192 lines);
  allocation failure triggers eviction rather than expansion
- Allocation complexity: O(1), pops from free stack
- Reclamation complexity: O(1), pushes back to free stack
- Alignment: all cache lines are 64B aligned, ensuring no cross-hardware-cache-line boundaries

### 3.3 Eviction Metadata (SoA Layout + RRPV Mask + Writeback Callbacks)

**Responsibility**: Tracks per-cache-line reference state, heat, and writeback information to determine eviction priority.

Replaces the four-level circular linked list from v0.1 with parallel arrays in Structure-of-Arrays layout:

```
SoA Layout — Parallel Arrays

  _refFlags[i]       : byte      Reference count snapshot (0=free, non-zero=held)
  _evictionFlags[i]  : byte      dirty(1bit) + RRPV(2bit) + costClass(1bit)
  _originKeys[i]     : nuint     Original pointer (reverse-deregisters Swiss Table entry on eviction)
  _preprocess[i]     : nint      Writeback pre-processing function pointer (called before dirty eviction)
  _writeback[i]      : nint      Writeback function pointer (called during dirty eviction)
  _callbackCtx[i]    : nuint     Callback context (passed to preprocess/writeback)
```

**Eviction Flag Bit Layout** (`_evictionFlags[i]`):

The RRPV array uses byte storage for each cache line's usage state; the low 2 bits encode the RRPV value.

**RRPV Semantics**:

| RRPV | Meaning                         | Eviction Priority         |
|------|---------------------------------|---------------------------|
| 0    | Will be re-accessed very soon   | Lowest (do not evict)     |
| 1    | Will be accessed shortly        | Low                       |
| 2    | Will be accessed in a while     | Medium                    |
| 3    | May never be accessed again     | Highest (evict first)     |

**RRPV Update Events**:

| Event              | Operation                       | Description                                          |
|--------------------|---------------------------------|------------------------------------------------------|
| Load (insert)      | RRPV = 0                        | Freshly loaded data will be used soon                |
| Hit                | `_evictionFlags[i] = 0`         | Bits 1-2 cleared, RRPV reset to 0                    |
| Eviction scan      | Find lines with RRPV=3 to evict | If none found, increment all RRPV +1 (aging)         |

**Advantages over the v0.1 four-level circular linked list**:

| Comparison           | v0.1 Circular List                  | v0.3 RRPV Mask + Callbacks           |
|----------------------|-------------------------------------|--------------------------------------|
| Data structure       | 4 doubly-linked lists + next ptrs   | Byte array + function pointer arrays |
| Warm-up operation    | Unlink + relink (4 pointer writes)  | `+= 1` (1-byte write)                |
| Aging operation      | Per-node list migration             | SIMD batch +1 (16 entries/cycle)     |
| Eviction scan        | Iterate list head (pointer chasing) | SIMD pattern matching (16/cycle)     |
| Cache friendliness   | Poor (pointer chasing)              | Good (contiguous memory scan)        |
| Reference protection | None                                | Lines with refCount > 0 not evicted  |
| Writeback support    | None                                | Callback-function driven             |

### 3.4 Writeback Callback Mechanism

**Responsibility**: Provides flexible writeback strategies for writable cache lines, using native function pointers to avoid managed overhead.

**Design Principle**: All writable cache lines are treated as dirty by default. On eviction or explicit flush,
L4 itself does not manage the writeback strategy; instead, the bound predictor is responsible for implementing it.

The predictor is defined via the following interface:

```
    public interface IPrefetchPredict
    {

        int Predict(
            Span<CacheLinePrediction> node
        );

        bool Prefetch(
             nuint origin,
             ulong policy,
             nuint cache
        );

        void WriteBack(
             nuint origin,
             ulong policy,
             nuint cache
        );

    }
```

**Typical Callback Implementations**:

```
Scenario 1: Segment accumulation (FMA partial sum)
  preprocess = null  (no pre-processing needed; data is already a double array)
  writeback  = AccumulateAdd  (element-wise dst[i] += src[i], AVX2-vectorized)

Scenario 2: Writeback of packed intermediate results
  preprocess = UnpackKernelRow  (reverse-repack from Pack format to row-major order)
  writeback  = AccumulateAdd

Scenario 3: PLU decomposition intermediate data
  preprocess = null
  writeback  = DirectCopy  (direct overwrite memcpy)
```

### 3.5 CacheLine (Cache Line Handle)

**Responsibility**: A cache line access handle held by the compute thread; prevents in-use lines from being evicted via reference counting.

```csharp
public ref struct CacheLine : IDisposable
{
    private ref int _activeCount;    // L4 global active count
    private ref int _refCount;       // Per-line reference count
    private nuint   _handle;         // Local cache data address

    // Construction: Interlocked.Increment(ref _activeCount)
    //               Interlocked.Increment(ref _refCount)
    // Dispose:      Interlocked.Decrement on both counters
}
```

- `ref struct` ensures it cannot escape to the heap
- Reference counts use `Interlocked` atomic operations for thread safety
- Lines with `_refCount > 0` are skipped during eviction scans
- StreamTransparent mode returns a CacheLine that does not occupy a cache slot
- Both read-only and writable paths return the same CacheLine type,
  callers decide read/write behavior via `.Address`

### 3.6 Predictor (Prefetch Predictor)

**Responsibility**: Provides deterministic prefetch predictions for L4 based on the windowed OPT strategy.

**Core Principle**: Matrix computation task sequences are deterministic — the scheduler pre-allocates
task blocks `(kr, kc, zz)` for each thread, allowing precise computation of the cache line keys
required at each future step. This gives L4 a predictive capability that hardware caches cannot
possess: **direct visibility into the future**, rather than inferring the future from history as
strategies like Hawkeye do.

```
Predictor Sliding Window:

  Task sequence: ... [T_now] [T+1] [T+2] ... [T+1023] ... [T+2047]
                      ↑ execution point                  ↑ buffer tail

  Window capacity      = 2048 prediction entries
  Prefetch trigger threshold = trigger refill when prefetch frontier
                               leads execution point by fewer than 1024 steps
  Refill batch size    = 64 entries (avoids excessive single-refill latency)
```

**数据结构**：

```csharp
internal class Predictor
{
    private const int WindowCapacity = 2048;
    private const int RefillThreshold = 1024;

    private PrefetchEntry[] _buffer;      // Circular prediction buffer
    private int _head;                     // Prefetch frontier (prefetched up to here)
    private int _tail;                     // Production position (scheduler fills up to here)
    private int _executed;                 // Execution point (compute thread has completed up to here)

    private HashSet<nuint> _activeKeys;    // All active keys within the window (deduplicated)
}

struct PrefetchEntry
{
    public nuint Key;              // Cache line identifier
    public nuint SourceAddress;    // Source data address
    public bool  NeedsRepack;      // Whether non-contiguous repacking is needed
}
```

**Cooperation with Eviction Policy**:

```
Eviction priority (from highest to lowest priority):

  1. refCount==0, not in window, RRPV==3, dirty==0  ← First (zero-cost eviction)
  2. refCount==0, not in window, RRPV==3, dirty==1  ← Second (needs writeback but cold)
  3. refCount==0, not in window, RRPV<3,  dirty==0  ← Ordered by RRPV
  4. refCount==0, not in window, RRPV<3,  dirty==1  ← Ordered by RRPV
  5. refCount==0, in window                          ← Avoid evicting if possible
  6. refCount>0                                      ← Never evict
```

Windowed OPT means most eviction decisions do not depend on RRPV — "not in window" alone is
sufficient to determine evictability. RRPV provides fine-grained differentiation among
out-of-window lines; the dirty bit preferentially evicts clean lines at equal RRPV to avoid writeback overhead.

### 3.7 Concurrency Model

L4 uses a **no-extra-threads** design; all operations are performed by compute threads themselves:

| Operation                          | Executor                        | Concurrency                          |
|------------------------------------|---------------------------------|--------------------------------------|
| Lookup (Swiss Table read)          | Any compute thread              | ✅ Fully parallel, lock-free         |
| HIT RRPV update                    | Any compute thread              | ✅ Single-byte write, no sync needed |
| refCount increment/decrement       | Any compute thread              | ✅ Interlocked atomic operations     |
| Prefetch refresh (eviction + load) | CAS-winner thread               | Serial (only one thread enters)      |
| Dirty writeback                    | CAS-winner thread               | Serial (within eviction flow)        |
| FlushAllDirty                      | Single thread after computation | Serial (no concurrency)              |

**Prefetch Refresh CAS Lock**:

```csharp
private int _refreshLock;  // 0=free, 1=locked

private void TryRefreshPrefetch()
{
    if (Interlocked.CompareExchange(ref _refreshLock, 1, 0) != 0)
        return;  // Another thread is doing it; current thread continues computing
    try { /* eviction + prefetch */ }
    finally { Volatile.Write(ref _refreshLock, 0); }
}
```

**Swiss Table Concurrent Read Safety Guarantee**: The prefetch refresh thread follows strict write ordering
on insertion: write Slab data first, then write eviction metadata and callback pointers, `sfence`, then
write the Swiss Table control byte and key/value, ensuring that when a compute thread reads the control
byte, the data is already fully visible.

---

## 4. Operation Flows

### 4.1 Cache Lookup (Read-Only Hit)

1. Compute CRC32C hash of the original pointer
2. Swiss Table lookup: SIMD scan ctrl → compare key → return cache pointer
3. Update RRPV: `_evictionFlags[slot] &= 0xF9` (RRPV zeroed, dirty bit unchanged)
4. Construct CacheLine handle (refCount++)
5. Return CacheLine; upper-layer code accesses local data via `.Address`

### 4.2 Cache Lookup (Writable Hit)

1. Swiss Table lookup hits
2. Update RRPV: `_evictionFlags[slot] &= 0xF9`
3. Construct CacheLine handle (refCount++)
4. Return CacheLine; upper-layer code reads/writes the cache line via `.Address` (e.g., partial sum +=)

### 4.3 Cache Miss (Read-Only)

1. Swiss Table lookup misses
2. Compute thread enters `Thread.Sleep(0)` spin loop
3. Wait for Predictor-driven prefetch refresh to load the line into L4
4. Exit wait loop when Swiss Table shows a hit
5. Construct CacheLine handle and return

Under normal operation, the Predictor always leads the execution point by 1024+ steps,
misses occur only during cold start or extreme bursts.

### 4.4 Cache Miss (Writable)

1. Swiss Table lookup misses
2. Allocate a cache line from Slab
3. If Slab has no free slots → trigger eviction (see 4.6)
4. Zero-initialize the cache line (`Unsafe.InitBlock(dest, 0, 8192)`)
5. Register writeback callbacks: `_preprocess[slot]`, `_writeback[slot]`, `_callbackCtx[slot]`
6. Set eviction flags: `dirty=1, RRPV=0`
7. Write `_originKeys[slot]`
8. `sfence`
9. Write Swiss Table mapping
10. Construct CacheLine handle and return

### 4.5 Prefetch Refresh

Triggered by compute threads in `StepComplete()`; CAS lock ensures single-thread execution:

1. Synchronize refFlags snapshot (`_refCounts[i] > 0 → _refFlags[i] = 1`)
2. Retrieve pending prefetch entries from Predictor (up to 64 at a time)
3. For each entry:
   - Swiss Table hit → mark RRPV=0 (hot-line protection)
   - Swiss Table miss → execute load:
     a. Allocate cache line from Slab
     b. If Slab has no free slots → trigger eviction (see 4.6)
     c. Execute Pack reorder or memcpy based on `NeedsRepack` flag
     d. Write eviction metadata (originKey, RRPV=0, dirty=0, callbacks=null)
     e. `sfence`
     f. Write Swiss Table mapping

### 4.6 Cache Eviction (SIMD Scan + Writeback)

```
Eviction scan pseudocode:

  Pass 1: Find refCount==0, RRPV==3, dirty==0 (zero-cost eviction)
    for i in range(scanCursor, CacheLineCount), step=16:
      refChunk = SSE2.Load(refFlags + i)
      refFreeMask = SSE2.MoveMask(SSE2.CompareEqual(refChunk, zero))

      evChunk = SSE2.Load(evictionFlags + i)
      // RRPV==3 and dirty==0 → byte & 0x07 == 0x06
      masked = SSE2.And(evChunk, 0x07)
      coldCleanMask = SSE2.MoveMask(SSE2.CompareEqual(masked, 0x06))

      candidates = refFreeMask & coldCleanMask
      while candidates != 0:
        bit = TrailingZeroCount(candidates)
        idx = i + bit
        if originKeys[idx] not in Predictor.ActiveKeys:
          evict(idx)    // No writeback, discard directly
          return
        candidates &= candidates - 1

  Pass 2: Find refCount==0, RRPV==3, dirty==1 (needs writeback)
    Same as above, but match byte & 0x07 == 0x07
    evict triggers writeback callbacks

  Pass 3: Increment all RRPV+1 (aging), retry
    Up to 3 rounds
```

**逐出执行**：

```csharp
private void Evict(int slot)
{
    byte flags = _evictionFlags[slot];

    // Dirty writeback
    if ((flags & 0x01) != 0)
    {
        nuint origin = _originKeys[slot];
        nuint cached = SlotAddressFromIndex(slot);
        nuint ctx    = _callbackCtx[slot];

        // 1. Pre-processing (optional)
        var preprocess = (delegate* unmanaged<nuint, nuint, void>)_preprocess[slot];
        if (preprocess != null)
            preprocess(cached, ctx);

        // 2. Writeback
        var writeback = (delegate* unmanaged<nuint, nuint, nuint, void>)_writeback[slot];
        if (writeback != null)
            writeback(origin, cached, ctx);
    }

    // Standard eviction
    _map.TryRemove(_originKeys[slot], out _);
    _slab.Free(SlotAddressFromIndex(slot));
    _evictionFlags[slot] = 0;
    _originKeys[slot] = 0;
    _preprocess[slot] = 0;
    _writeback[slot] = 0;
    _callbackCtx[slot] = 0;
}
```

### 4.7 显式刷新

```csharp
/// <summary>
/// Explicitly writes back the specified dirty cache line. Called after all segments complete.
/// </summary>
public void FlushDirty(nuint originKey)
{
    if (_map.TryGet(originKey, out nuint addr))
    {
        int slot = SlotIndex(addr);
        if ((_evictionFlags[slot] & 0x01) != 0)
        {
            // Execute pre-processing + writeback
            InvokeWriteBack(slot);
            _evictionFlags[slot] &= 0xFE;  // Clear dirty bit
        }
    }
}

/// <summary>
/// Writes back all dirty cache lines in bulk. Called after entire matrix computation completes.
/// </summary>
public void FlushAllDirty()
{
    for (int i = 0; i < CacheLineCount; i++)
    {
        if ((_evictionFlags[i] & 0x01) != 0 && _originKeys[i] != 0)
        {
            InvokeWriteBack(i);
            _evictionFlags[i] &= 0xFE;
        }
    }
}
```

### 4.8 Working Mode Switch

```csharp
public enum L4Mode
{
    /// <summary>
    /// Pass-through mode: Map() returns the original pointer directly; L4 does not intervene.
    /// Suitable for single-NUMA + no-repack scenarios (e.g., vector addition, scalar multiplication).
    /// </summary>
    StreamTransparent,

    /// <summary>
    /// Buffer mode: full lookup/Pack/cache/eviction pipeline.
    /// Suitable for FMA scenarios requiring repacking or multi-NUMA scenarios.
    /// </summary>
    CacheBuffer
}
```

Mode is determined at construction time and does not change at runtime. Decision logic:

- Requires Pack reordering (FMA core rows of matrix A) → CacheBuffer
- Multiple NUMA nodes → CacheBuffer
- Requires segment accumulation → CacheBuffer
- Single NUMA + no reordering + no segment → StreamTransparent

---

## 5. Segment Accumulation Scenario

### 5.1 Problem

For matrix multiplication `C = A × B` when the K dimension is large, segmented accumulation is required:

```
segment 0:  C_partial  = A[kr, 0..KC]    × B[0..KC, kc]
segment 1:  C_partial += A[kr, KC..2KC]  × B[KC..2KC, kc]
...
segment N:  C_partial += A[kr, N*KC..K]  × B[N*KC..K, kc]

Final: C[kr, kc] = Σ all segment partial sums
```

If each segment's result is `+=` directly to the C matrix,
and C resides on a remote NUMA node, every segment incurs cross-node write latency.

### 5.2 L4 Solution

Temporarily store C's partial sums in L4's writable cache lines:

```
segment 0: c = l4.MapWritable(C_key, null, &AccumulateAdd)
           // First miss → allocate zero-initialized cache line
           FMA(c.Address, a.Address, b.Address)  // c += a*b
           c.Dispose()

segment 1: c = l4.MapWritable(C_key, null, &AccumulateAdd)
           // Hit! Returns existing partial sum cache line
           FMA(c.Address, a.Address, b.Address)  // c += a*b
           c.Dispose()

... after all segments complete ...

l4.FlushDirty(C_key)
// Calls AccumulateAdd(C_origin, cacheline, ctx)
// → Adds local partial sum back to remote C matrix in one shot
```

### 5.3 Benefits

```
Without segment buffering (OpenBLAS approach):
  Each segment writes C matrix across NUMA
  Remote writes per output tile = Zk times
  Per-write latency = ~200-300ns (cross-node)

With L4 segment buffering:
  All segment += operations complete locally in L4 cache lines
  Partial sums likely remain in L1/L2 (8KB << L1 capacity)
  Remote writes per output tile = 1 (final FlushDirty)
  Writeback uses AVX2-vectorized stores: 8192B = 32 × 256-bit stores

  Reduction ratio = Zk : 1
```

---

## 6. Uniqueness and Advantages

### 6.1 Deterministic Prefetch (Windowed OPT)

Essential difference from hardware cache replacement policies (LRU, RRIP, Hawkeye):

| Policy              | Information Source      | Prediction Method             | Accuracy                  |
|---------------------|-------------------------|-------------------------------|---------------------------|
| LRU                 | Past access order       | Least recently used           | Moderate                  |
| RRIP                | Insert/hit markers      | Predict reuse distance        | Moderate-High             |
| Hawkeye             | OPTgen retrospection    | Learn past to infer future    | High                      |
| **L4 Predictor**    | **Future task queue**   | **Direct future visibility**  | **Perfect** (within window) |

L4's Predictor is a **deterministic Oracle** — it does not predict; it knows.
Because matrix computation task sequences are fully determined at scheduler allocation time.

### 6.2 NUMA Adaptability

The same code automatically gains NUMA benefits on different hardware configurations:

| Hardware Configuration  | L4 Behavior                                                 |
|-------------------------|-------------------------------------------------------------|
| Single-socket CPU       | Pack cache + segment accumulation buffer                    |
| Dual-socket CPU         | Pack cache + NUMA migration cache + segment accum.          |
| Hybrid-architecture CPU | Pack cache + cross-domain migration cache + segment accum.  |

### 6.3 Callback-Driven Writeback Strategy

Using native function pointers for writeback avoids:

- Virtual method call overhead
- Interface dispatch overhead
- Delegate GC pressure
- Hard-coding specific writeback logic

Callers register `preprocess` and `writeback` callbacks at `MapWritable` time,
L4 invokes them directly through function pointers on eviction or Flush,
**zero managed overhead, zero virtual dispatch, performance equivalent to C function calls**.

### 6.4 Reference Count Protection

Through `CacheLine` handle reference counting, cache lines actively in use by compute threads are
absolutely never evicted (including writeback not being triggered). The SIMD eviction scan skips
lines with `refCount > 0` immediately via the `_refFlags` array, without checking linked lists or acquiring locks.

### 6.5 Cache Line Size Aligned to Matrix Kernel

The 8192B cache line size is not arbitrary:

- 1 `double MAT_KERNEL` (4×4 double matrix) = 128 Bytes
- 1 cache line = 64 `MAT_KERNEL` units
- Matrix multiplication micro-kernels operate on data sizes perfectly aligned to cache lines
- Compute kernel functions operate on cache lines as units; no cross-cache-line accesses occur

### 6.6 Fixed-Capacity Determinism

- Slab allocator never expands → memory footprint upper bound is known (64 MB)
- SIMD eviction scan → latency upper bound is known (at most 3 aging rounds × 512 SIMD comparisons)
- Swiss Table fixed 16,384 slots → lookup latency upper bound is known
- Writeback callback latency is controlled by the caller → predictable
- Worst-case L4 latency = lookup + eviction (incl. writeback) + allocation + memcpy, all statically computable

### 6.7 Reuse of Existing StgSharp Infrastructure

| L4 Component          | Reused StgSharp Infrastructure                        |
|-----------------------|-------------------------------------------------------|
| Mapping table         | `SwissTable` (implemented and benchmark-validated)    |
| Mapping table SIMD    | `IBucketScanner` + `SIMDID` architecture detection    |
| Cache line alloc      | `SlabAllocator.FromBuffer<T>()`                       |
| Hash computation      | Hardware `CRC32C` instruction                         |
| Memory allocation     | `NativeMemory.AlignedAlloc`                           |
| Future multi-platform | `IBucketScanner` + NRGA compilation pipeline         |

### 6.8 Originality Analysis

After surveying existing mainstream numerical computation libraries, L4's design has
**no direct precedent or equivalent in known public implementations**.

#### NUMA Strategy Comparison with Existing Libraries

| Library / Framework | NUMA Strategy                       | Software Cache Layer | Mapping Table   | Eviction Policy         | Dirty Writeback    |
|---------------------|-------------------------------------|---------------------|-----------------|-------------------------|--------------------|
| Intel MKL           | Thread pinning + `numactl`          | None                | None            | None                    | None               |
| OpenBLAS            | Thread affinity + block alignment   | None                | None            | None                    | None               |
| BLIS                | Multi-level block alignment L1/L2/L3| None                | None            | None                    | None               |
| Eigen               | No explicit NUMA handling           | None                | None            | None                    | None               |
| cuBLAS / rocBLAS    | GPU memory (hardware-solved)        | None                | None            | None                    | None               |
| **StgSharp L4**     | **Software-managed thread-local cache** | **Yes**         | **Swiss Table** | **RRPV + windowed OPT** | **Callback-driven** |

---

## 7. 从 v0.1 到 v0.3 的变更摘要

| 项目             | v0.1                          | v0.3                                 |
|-----------------|-------------------------------|---------------------------------------|
| 逐出数据结构     | 4 级环形双向链表               | RRPV 2-bit 掩码 + 8KB SoA 数组        |
| 逐出扫描方式     | 链表头指针遍历                 | SSE2 批量模式匹配（16 行/周期）         |
| 热���更新         | 链表节点摘插（4 指针写）        | 单字节位运算 `&= 0x03`                |
| 老化操作         | 逐节点移链表                   | SIMD 批量 +1                          |
| 引用保护         | 无                            | refCount > 0 时 SIMD 扫描跳过          |
| 预取策略         | 无                            | Predictor 窗口 OPT（2048 条缓冲）      |
| 线程模型         | L4 服务线程 + Command Queue    | 无额外线程，CAS 抢锁                   |
| 命令队列         | 链式 CommandCache 分段          | 不需要（预取刷新替代命令驱动）           |
| 缓存行元数据     | Slab 分配的 CacheLineHead 结构 | SoA 平行数组 + 函数指针数组             |
| 工作模式         | 单一模式                       | StreamTransparent / CacheBuffer 切换   |
| 写回策略         | 无（只读缓存）                  | dirty 位 + preprocess/writeback 回调   |
| 写回机制         | 无                            | 本机函数指针，零托管开销                |

---

## 8. 未来工作

- [ ] L4 根类型重构（组合 Swiss Table、Slab、SoA 元数据、Predictor）
- [ ] Predictor 与矩阵乘法调度器的集成
- [ ] Segment 累加写回的 AVX2 向量化实现
- [ ] SIMD 逐出扫描的 AVX2 升级（32 行/周期）
- [ ] Swiss Table ARM NEON 实现
- [ ] Swiss Table AVX-512 升级（64 槽扫描从 16 次降至 4 次）
- [ ] 墓碑累积监控与自动 rehash 策略
- [ ] 性能基准测试（NUMA 本地 vs 远端访问延迟对比）
- [ ] Segment 累加 vs 直接写回的 benchmark 对比
- [ ] 与逐线程矩阵上下文的集成验证
- [ ] 双路 6242R 环境下的 NUMA 搬运性能验证
- [ ] 远期：PPC64LE 平台的 VSX 原生 Swiss Table 扫描支持

---

## 9. 参考

- Google Abseil Swiss Tables Design Notes
- HP Labs RRIP (Re-Reference Interval Prediction) 论文
- Hawkeye: Efficient Cache Replacement via OPTgen (ISCA 2017)
- StgSharp HLSF Allocator 设计文档
- StgSharp SIMD Architecture (`SIMDID` 体系)
- StgSharp NRGA Roadmap
- StgSharp `SlabAllocator<T>` 实现
- StgSharp `SwissTable` 实现及 Benchmark
