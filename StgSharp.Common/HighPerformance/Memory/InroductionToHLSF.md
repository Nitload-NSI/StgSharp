# HLSF (Hybrid Layer Segregated Fit) Allocator

A high-performance, O(1) memory allocator designed for real-time applications that demand predictable allocation and deallocation performance.

## Overview

The Hybrid Layer Segregated Fit (HLSF) allocator is a sophisticated memory management system that combines the benefits of segregated free lists with layered size classification. It provides constant-time allocation and deallocation operations while maintaining excellent memory utilization and low fragmentation.

## Key Features

### Performance Characteristics
- **O(1) Time Complexity**: Both allocation and deallocation operations execute in constant time
- **High Throughput**: Capable of handling millions of operations per second
- **Low Latency**: Predictable performance suitable for real-time applications
- **Excellent Deallocation Speed**: Superior free performance compared to traditional allocators

### Memory Management
- **External Metadata Storage**: All allocation metadata is stored in external SLAB allocators, keeping the main allocation area clean
- **Native Byte Alignment Control**: Configurable alignment (minimum 16 bytes) with hardware-optimized defaults
- **Large Block Support**: Single allocation limit of 32MB per request
- **Efficient Memory Utilization**: Low overhead with intelligent block merging

### Architecture
- **10-Level Hierarchy**: Size classes from 64 bytes to 16MB (64, 256, 1KB, 4KB, 16KB, 64KB, 256KB, 1MB, 4MB, 16MB)
- **3-Segment Segregation**: Each level is divided into 3 segments for better size distribution
- **Hybrid Approach**: Combines best aspects of segregated fit and layered allocation strategies

## Architecture Details

### Size Classification System

The allocator uses a logarithmic size classification system with 10 levels:

```
Level 0: 64 bytes      (2^6)
Level 1: 256 bytes     (2^8)  
Level 2: 1KB           (2^10)
Level 3: 4KB           (2^12)
Level 4: 16KB          (2^14)
Level 5: 64KB          (2^16)
Level 6: 256KB         (2^18)
Level 7: 1MB           (2^20)
Level 8: 4MB           (2^22)
Level 9: 16MB          (2^24)
```

Each level follows the pattern: `size = 64 * 4^level`

### Bucket Organization

Each size level is further divided into 3 segments based on exact size requirements:
- **Segment 0**: 1x level size
- **Segment 1**: 2x level size  
- **Segment 2**: 3x level size

This provides fine-grained size matching while maintaining O(1) lookup performance.

### External Metadata Design

Unlike traditional allocators that embed metadata within allocated blocks, HLSF stores all metadata externally:

- **Entry Structure (36 bytes)**: Contains position, size, level, and linkage information
- **BucketNode Structure (32 bytes)**: Manages free block organization with integrity verification
- **SLAB Storage**: Both structures are allocated from separate SLAB allocators for optimal cache performance

This design provides several advantages:
- Clean allocation areas free from metadata corruption
- Better cache locality for metadata operations
- Simplified debugging and memory inspection
- No metadata overhead in allocated blocks

## Performance Benchmarks

*Note: The following results are from informal testing and should be considered preliminary.*

### Single-threaded Performance (65,536 operations)

| Operation | Average Time | Throughput | Performance Rating |
|-----------|--------------|------------|-------------------|
| Allocation | 31-40ms | ~2M allocs/sec | Good |
| Free | 2-3ms | ~25M frees/sec | Excellent |

**Test Configuration:**
- Pool Size: 16MB
- Block Sizes: 64-1024 bytes (mixed)
- Platform: x64, .NET 8
- Iterations: 5 rounds

### Memory Efficiency

- **Metadata Overhead**: ~2.8% of total allocation
- **Alignment**: 16-byte default (configurable)
- **Fragmentation**: Low due to intelligent merging algorithms

## Usage Examples

### Basic Allocation

```csharp
// Create allocator with 64MB pool
using var allocator = new HybridLayerSegregatedFitAllocator(64 * 1024 * 1024);

// Allocate 1KB block
var handle = allocator.Alloc(1024);

// Use the allocated memory
Span<byte> memory = handle.BufferHandle;
memory[0] = 42;

// Free the block
allocator.Free(handle);
```

### Custom Alignment

```csharp
// Create allocator with 32-byte alignment
using var allocator = new HybridLayerSegregatedFitAllocator(
    byteSize: 32 * 1024 * 1024,
    align: 32
);

var handle = allocator.Alloc(256);
// Memory is guaranteed to be 32-byte aligned
```

### Bulk Operations

```csharp
var handles = new List<HybridLayerSegregatedFitAllocationHandle>();

// Allocate multiple blocks
for (int i = 0; i < 1000; i++)
{
    handles.Add(allocator.Alloc((uint)(64 + i % 960))); // 64-1023 bytes
}

// Free all blocks
foreach (var handle in handles)
{
    allocator.Free(handle);
}
```

## API Reference

### HybridLayerSegregatedFitAllocator

Main allocator class providing memory management functionality.

#### Constructors

```csharp
public HybridLayerSegregatedFitAllocator(nuint byteSize)
public HybridLayerSegregatedFitAllocator(nuint byteSize, int align)
```

#### Methods

```csharp
public HybridLayerSegregatedFitAllocationHandle Alloc(uint size)
public void Free(HybridLayerSegregatedFitAllocationHandle handle)
public void Dispose()
```

### HybridLayerSegregatedFitAllocationHandle

Handle representing an allocated memory block.

#### Properties

```csharp
public readonly byte* Pointer { get; }
public readonly Span<byte> BufferHandle { get; }
public readonly uint AllocSize { get; }
public readonly ref byte this[int index] { get; }
```

## Thread Safety

The current implementation is **single-threaded**. For multi-threaded applications, external synchronization is required. A concurrent version is planned for future releases.

## Comparison with Other Allocators

### vs TLSF (Two-Level Segregated Fit)

| Aspect | HLSF | TLSF | Notes |
|--------|------|------|-------|
| Allocation Speed | ~2M ops/sec | ~3-5M ops/sec | HLSF has optimization potential |
| Deallocation Speed | ~25M ops/sec | ~10-15M ops/sec | **HLSF advantage** |
| Memory Overhead | ~2.8% | ~1-2% | HLSF uses external metadata |
| Time Complexity | O(1) | O(1) | Equal |
| Code Complexity | Moderate | Low | HLSF more sophisticated |

### Strengths
- Superior deallocation performance
- Clean separation of data and metadata
- Configurable alignment support
- Excellent for allocation-heavy workloads

### Areas for Improvement
- Allocation speed optimization
- Memory overhead reduction
- Concurrent implementation

## Implementation Notes

### Alignment Guarantees

- Minimum alignment: 16 bytes
- All allocations respect specified alignment
- Internal blocks aligned to 64-byte boundaries for optimal cache performance

### Merging Strategy

The allocator employs an aggressive merging strategy:

1. **Immediate Merging**: Adjacent free blocks are merged during deallocation
2. **Boundary Checking**: Ensures merging respects level boundaries
3. **Bidirectional Merging**: Checks both forward and backward neighbors
4. **Level Promotion**: Merged blocks may be promoted to higher levels

## Future Roadmap

### Planned Optimizations

1. **Performance Enhancements**
   - Lookup table optimization for size classification
   - Bucket operation improvements
   - SIMD utilization for bulk operations

2. **Concurrent Support**
   - Thread-local caches
   - Lock-free bucket operations
   - NUMA-aware allocation

3. **Advanced Features**
   - Adaptive size level adjustment
   - Memory pressure handling
   - Statistics and profiling support

### Target Performance Goals

- Allocation Speed: 4-6M ops/sec (single-threaded)
- Memory Overhead: <1.5%
- Fragmentation Rate: <3%
- Concurrent Scaling: Linear up to 16 threads

## Building and Testing

The HLSF allocator is part of the StgSharp.Common library and requires:

- .NET 8 or later
- x64 architecture (no x86 support planned)
- Windows/Linux

### Running Benchmarks

```csharp
// See StgSharpDebug/Program.cs for comprehensive benchmark suite
var allocator = new HybridLayerSegregatedFitAllocator(16 * 1024 * 1024);
// Run allocation/deallocation cycles...
```

## License

This implementation is part of the StgSharp project and follows the project's licensing terms.

## Contributing

Performance improvements, bug fixes, and documentation enhancements are welcome. Please ensure all changes maintain the O(1) performance guarantees and include appropriate benchmarks.