# Introduction to L4

This document describes the current L4 implementation and separates it from proposed design directions that are not yet implemented or verified. The codebase is evolving; source comments and this status section should be checked when making architectural decisions.

## 1. Purpose and Responsibilities

An `L4` instance is bound to one NUMA node. It owns a NUMA-local arena that contains the payload allocator and the double-buffered Swiss mapping table.

The main responsibilities are:

- **L4** manages local allocation, source mapping, predictor traversal, buffer publication, and phase coordination.
- **TLSF** allocates packed payload blocks from the L4 arena.
- **Swiss** stores and looks up `AddressSource` keys and their cache-line metadata references. It does not interpret source layouts.
- **Predictors** interpret their own source descriptions and produce packed data through `Prefetch`.

L4 does not infer a predictor's packed format. The current implementation has no automatic writeback protocol; any format-specific writeback or rollback must be explicit.

## 2. Arena, Sources, and Handles

The L4 constructor allocates one NUMA-local region. The payload allocator and its metadata occupy the first region; the Swiss table's two buffers follow it. The default payload capacity is `8192 * 8192` bytes, the default maximum cache-line allocation is 8192 bytes, and payload capacity is page-aligned to 4096 bytes.

`AddressSource` is a 16-byte struct with these fields:

| Field | Type | Meaning |
| --- | --- | --- |
| `Description` | `uint` | Predictor-defined source/layout description |
| `Size` | `uint` | Requested packed-data size |
| `Address` | `ulong` | Source address or identity |

The current equality and hash implementations use `Address` and `Description`; `Size` is stored in the struct but is not part of key identity. Callers should account for this behavior when constructing keys.

Each allocated block has a 32-byte `CacheLineMetadata` node containing allocator links, payload position and size information, and predict/map/consume counters. `L4.Handle` is a 16-byte value containing the mapped payload address and a metadata reference. Disposing a handle increments its consume count.

The current implementation limits an L4 to 8192 active cache-line entries. This entry limit is separate from the byte capacity of the arena and from the predictor-ring capacity.

## 3. Swiss Mapping Table

The table has 1024 groups with 16 slots per group. Each group has 16 control bytes, 16 `AddressSource` keys, and 16 64-bit values. A value refers to the corresponding cache-line metadata.

The current x64 hash uses CRC32C over `Address` and `Description`. Control-byte comparisons use SSE2, and the Swiss backend requires SSE4.2. Other ISA backends are not implemented.

Each table maintains a published read buffer and an inactive write buffer:

- `TryGet` reads the published-buffer pointer once using `Volatile.Read` and searches that snapshot.
- Writers update the inactive buffer.
- `SwapBuffers` atomically publishes the write buffer and makes the old published buffer the next write buffer.
- `TryRehash` copies or rebuilds the inactive buffer after a swap, depending on the tombstone count.

Atomic pointer exchange alone does not make the old buffer safe to reuse. The caller must enforce a phase barrier so readers no longer access the old buffer before it is overwritten.

## 4. Predictor Interface and Ring

`IL4Predictor` exposes an `Id`, `PredictMission`, and `Prefetch`. `IL4Predictor<TMission>` adds a typed `TryGetNextMission` API. `PredictMission` is synchronous and receives a caller-owned `Span<AddressSource>`; L4 currently supplies a four-element stack-allocated span. The span must not escape the call.

The current predictor registry is a fixed 256-entry ring stored directly in `L4`; there is no separate `PredictorMonitor` type. `RegistPredictor`, `DropPredictor`, and the ring cursors provide basic registration, removal, and ordered traversal. The full registration lifecycle and ownership rules are not yet complete.

The current `Predict()` path traverses predictors synchronously. For each predicted source, it either updates the predict count of an existing entry or allocates a cache line, calls that predictor's `Prefetch`, and adds the source-to-metadata mapping to the inactive Swiss buffer. When the active-entry limit is reached, it attempts eviction of entries whose consume and predict counts match.

This code is a prediction pipeline implementation, not just an empty stub. However, the repository does not currently contain an in-tree concrete `IL4Predictor` implementation wired into the full path, so end-to-end prediction behavior remains incomplete.

## 5. Map and Publication Flow

`Map` uses the remaining prediction count to select a lookup path. The current thresholds are:

| Remaining predictions | Current behavior |
| --- | --- |
| Below 512 | A caller may acquire the prediction-writer lock and synchronously run `Predict()`. If predictions are produced, it closes the swap gate, waits for counted readers, swaps buffers, updates the prediction count, and rehashes. Contending callers use the counted lookup path. |
| From 512 to below `512 + 2 * LogicalCoreCount` for the bound NUMA node | Use a lookup path protected by the swap gate and reader counter. |
| At or above that upper threshold | Use the direct lookup path without incrementing the reader counter. |

The counted reader protocol waits for the swap gate to open, increments the reader counter, then checks the gate again. If the gate closed in between, it decrements the counter and retries. The swap writer closes the gate and waits for counted readers to leave before exchanging buffers.

The direct, uncounted fast path depends on prediction lead and phase timing. Those assumptions do not by themselves prove that a preempted reader cannot still access a buffer when it is reused. The safety invariant between this path and buffer reuse remains to be verified.

## 6. Entry Lifetime, Eviction, and Writeback

Predict, map, and consume counts represent different stages. `Map` increments the map count, and disposing a returned handle increments the consume count. During prediction, existing entries have their predict count increased. Eviction currently checks whether consume and predict counts match before removing an entry and freeing its metadata.

L4 must not decode predictor-specific packed data. If evicting an entry while work is still in flight would lose required state, that entry must be protected until the work is completed, rolled back, or explicitly discarded. Any software writeback must be routed to the responsible predictor; it is not inferred from the Swiss key/value entry.

The code does not yet provide a complete in-flight batch model that retains predictor identity and writeback context until associated work completes. A single owner field would also be insufficient if multiple predictors can claim the same entry; shared ownership and writeback routing need an explicit design.

## 7. Implemented and Unresolved

Implemented in source:

- NUMA-bound L4 arena allocation with TLSF payload allocation and embedded allocator metadata.
- A double-buffered Swiss table with atomic publication.
- An x64 CRC32C hash and SSE2 control-byte scanning, guarded by SSE4.2 support checks.
- A fixed predictor ring, basic registration/removal methods, and a synchronous prediction traversal.
- Map reader-gate/counter logic and prediction thresholds.
- Cache-line metadata with predict, map, and consume counters.

Not yet implemented or not yet verified end to end:

- A separate predictor monitor with stable registration order, lifecycle states, and explicit ownership rules.
- Concrete predictors wired through registration, prediction, mapping, and workload completion.
- An in-flight batch store that retains predictor identity and completion/writeback context.
- A complete writeback or rollback protocol and eviction protection for shared or unfinished work.
- Proof or stress tests that the uncounted Map fast path cannot overlap reuse of its published buffer.
- Concurrency tests covering registration, publication, Map, eviction, handle disposal, and L4 disposal.

## 8. Design Directions

The intended architecture distinguishes three resources that must not be conflated:

- **Predictor registry:** ordered predictors waiting to produce work. A capacity of 256 is currently used by L4's ring; a separate monitor is a possible future abstraction.
- **Swiss entries:** source-to-cache-line metadata mappings and lookup state.
- **In-flight batches:** predictor-produced work that has not completed and may still require predictor identity, mission state, or writeback context.

Predictor calls are currently synchronous. If a future design hands predicted work to another thread or NUMA node, it must copy data out of the caller-provided span into storage with an explicit lifetime. Any asynchronous queue should be bounded and must not block the Map fast path.

One previously discussed Swiss-layout option is to widen each value from 8 to 16 bytes and organize it with the 16-byte `AddressSource` as a 32-byte key/value record. Two such records would fit in a 64-byte cache line. This is only a storage-layout proposal; it does not change the current 16-slot probe group and has not been implemented or benchmarked.
