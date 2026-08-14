# StgSharp

![StgSharp Logo](STG%23LOGO.png)

StgSharp is an experimental .NET framework for graphics, numerical computing, language analysis, and other performance-sensitive applications. It began as the foundation of a next-generation STG (shoot 'em up) engine, but the current repository focuses on reusable low-level infrastructure rather than a complete, ready-to-use game engine.

## Overview

The framework combines managed C# APIs with a C native library. Its current work includes OpenGL bindings and rendering infrastructure, generic matrix and vector types, SIMD-accelerated native kernels, custom allocators and collections, regular-language analysis, and state-oriented UI infrastructure. StgSharp targets .NET 8; platform and architecture coverage varies by module, with Windows and Linux as the principal native targets.

## Core Modules

### StgSharp.Common

The foundational library containing shared functionality:

- **High Performance**: Native-memory allocators and processor abstractions
- **Collections**: Specialized data structures and containers
- **Threading and Timing**: Synchronization primitives, task helpers, and time providers
- **Pipeline and State**: Processing pipelines and reusable state-machine primitives
- **Environment**: Platform abstraction and system integration

### StgSharp.Mathematics (Numeric)

Numerics and linear algebra built on top of the core library:

- **Matrix & Vector Types**: High-performance math primitives
- **Numeric Utilities**: Core numeric helpers and algorithms
- **SIMD-Friendly Kernels**: Data layouts optimized for hardware acceleration

### StgSharp.RegularAnalysis

Nitload General Regular Analysis (NGRA) infrastructure for recognizing and processing regular languages:

- **Regex/Automata Core**: Regular expression processing and automata utilities
- **Parsing Primitives**: Reusable tokenizer and grammar building blocks
- **Analysis Pipeline**: Structured processing for language-like inputs

NGRA is a foundation for higher-level language tooling. It does not replace `StgSharp.Script`; the planned Script implementation depends on NGRA and has not started yet. The source currently under `StgSharp.Script` is retained from the earlier EXPRESS implementation and is not part of the active core solution.

### StgSharp.Graphics

Managed graphics infrastructure built on Common and Mathematics:

- **OpenGL Bindings**: Broad OpenGL 3.3-4.6 API bindings and context management
- **Rendering Infrastructure**: Render streams, viewports, textures, framebuffers, and geometry
- **Shader Support**: Shader compilation, program management, and shader-generation utilities
- **Window Integration**: GLFW-based window and input access

### StgSharp.Native

Native library integration:

- **GLFW Integration**: Native loading and interoperation used by the graphics layer
- **SIMD Operations**: Hardware-accelerated vector operations
- **Matrix Kernels**: Optimized matrix computation routines
- **Platform Runtime**: Native thread and platform support for Windows and Linux

### StgSharp.TerminalDialogue

Terminal-based user interface application and components:

- **Interactive Terminal**: Command-line UI components
- **Dialogue System**: User interaction and input handling
- **Terminal Graphics**: Text-based UI rendering

> Note: `StgSharp.Script` is planned as a consumer of NGRA and has not started yet. Its existing EXPRESS sources belong to the earlier implementation. `StgSharp.Model` is currently an empty placeholder while its design is being reconsidered.

## Key Features

### High-Performance Computing

- **Parallel Matrix Operations**: Multi-threaded matrix computations with optimized scheduling
- **SIMD Acceleration**: Hardware-optimized vector operations
- **Memory Management**: Arena, slab, and TLSF (Two-Layer Segregated Fit) allocators
- **Parallel Runtime**: Dedicated scheduling and worker infrastructure for matrix operations

#### TLSF Benchmark Snapshot (BenchmarkDotNet)

```
BenchmarkDotNet v0.15.8, Windows 10 (10.0.19045.7548)
Proxmox VE guest, 30 vCPUs, host CPU reported as Intel Xeon Gold 6242R 3.10GHz
.NET SDK 10.0.301
  [Host] : .NET 8.0.28 (8.0.28, 8.0.2826.26413), X64 RyuJIT x86-64-v4

Toolchain=InProcessEmitToolchain  InvocationCount=3  IterationCount=15
LaunchCount=1  UnrollFactor=1  WarmupCount=6
```

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|------- |-----:|------:|-------:|------:|--------:|
| Libc | 129.96 ns | 10.123 ns | 9.469 ns | 1.00 | 0.10 |
| Tlsf | 81.39 ns | 1.039 ns | 0.971 ns | 0.63 | 0.05 |

This benchmark was run inside a Proxmox VE virtual machine configured with 30 vCPUs. BenchmarkDotNet reports those virtual CPUs as 30 logical and 30 physical processors; that is the guest topology rather than the physical topology of the Xeon Gold 6242R host CPU, whose nominal topology is 20 cores and 40 threads.

The benchmark reports normalized time per allocation/free operation. Each iteration uses a 512 MB arena and 256 batches of 2,048 allocations, drawing request sizes from 64, 100, 200, 400, 1,024, and 4,096 bytes and freeing them in a deterministic shuffled order. TLSF uses a local coalescing window of four blocks. In this run TLSF used about 63% of the libc baseline time, or approximately 1.60x the throughput. The result describes this fixed small-block workload only; it is not a general-purpose allocator comparison.

### Graphics and Rendering

- **OpenGL Integration**: Broad OpenGL 3.3-4.6 bindings
- **Cross-Platform Direction**: Windows and Linux native targets, with feature coverage dependent on platform
- **Rendering Pipeline**: Flexible and extensible graphics framework
- **Shader Management**: Dynamic shader compilation and management

### Framework Infrastructure

- **State Machines**: Reusable state and transition primitives
- **Processing Pipelines**: Schedulable nodes and typed connections
- **Window and Input Access**: Low-level GLFW integration
- **Image and Texture Handling**: Image loading and GPU texture infrastructure

### Data Processing

- **NGRA**: Regex analysis, intermediate representation, optimization, and source generation
- **EXPRESS (legacy/inactive)**: The repository retains an earlier experimental EXPRESS parser/compiler under `StgSharp.Script`
- **Script (planned)**: A future language layer intended to build on NGRA; development has not started

## Requirements

- .NET 8.0 or later
- Windows 10/11 or Linux
- x64 or ARM64 architecture
- OpenGL 3.3+ support

## Installation

StgSharp is currently in development and requires compilation from source. **NuGet packages are not yet available** - you must build the project from source code.

### Prerequisites

- CMake 3.16 or later
- Clang and a Ninja-compatible build environment for the supported C targets
- **Windows (optional)**: Visual Studio may open the provided `.vcxproj` for navigation and editing
- .NET 8.0 SDK or later
- Git for source code retrieval

### Building from Source

1. **Clone the repository**:

   ```bash
   git clone https://github.com/Nitload-NSI/StgSharp.git
   cd StgSharp
   ```

2. **Build the native library (recommended toolchain)**:

   ```bash
   cmake --preset clang-release -S src/StgSharp.Native
   cmake --build cmake_build/clang-release
   ```

   The supplied presets use Clang with Ninja and place native output under the repository's platform-specific `bin` directory. Use `clang-debug` for a debug build.

3. **Build the managed projects**:

   ```bash
   dotnet build StgSharp.sln
   ```

   Managed builds copy an existing native binary into their output directory. They do not invoke CMake automatically, so build `StgSharp.Native` first when native functionality is required.

### Build Configuration

- **Debug**: Full debugging symbols, unoptimized
- **Release**: Optimized for performance

### Notes

- CMake is the maintained build path for `StgSharp.Native`; the checked-in presets primarily target Clang
- The Visual Studio C project may be opened with MSVC tooling, but MSVC IntelliSense behavior and MSVC-produced binaries are not maintained or guaranteed
- Windows and Linux platform implementations exist; actual feature and architecture coverage may differ
- x86-64 currently has the most complete optimized kernel coverage
- Some features may not be available on all platforms

## Quick Start

### Basic Graphics Application

```csharp

/*To be implemented*/

```

### Matrix Operations

```csharp
using StgSharp.HighPerformance.Memory;
using StgSharp.Mathematics.Numeric;

// Matrix storage is allocated explicitly from a native-memory arena.
using var allocator = new TwoLayerSegregatedFitAllocator(64 * 1024 * 1024);

var matrix = Matrix<float>.Create(
    4,
    4,
    MatrixLayout.DenseRectangle,
    allocator);

matrix[0, 0] = 1.0f;
```

General matrix arithmetic does not currently provide operator overloads. Because matrix storage and result ownership involve explicit native-memory control, arithmetic operations are exposed through explicit computation APIs as they are implemented.

### Memory Management

```csharp
using StgSharp.HighPerformance.Memory;

// Create a 64 MiB TLSF arena
using var allocator = new TwoLayerSegregatedFitAllocator(64 * 1024 * 1024);

// Allocate memory
var handle = allocator.Alloc(1024);
Span<byte> buffer = handle.AsSpan();
buffer[0] = 42;

allocator.Free(handle);
```


## Documentation

### Technical Documentation

- [TLSF Allocator](src/StgSharp.Common/HighPerformance/Memory/InroductionToTLSF.md) - Arena layout, allocation behavior, and benchmark methodology
- [Native Library Build](src/StgSharp.Native/README.md) - Native build and source-layout notes
- [Native Library Naming](src/StgSharp.Native/naming.md) - Native library file naming conventions

## Contributing

StgSharp is developed by Nitload Space. Contributions are welcome, particularly in:

- Performance optimizations
- Cross-platform compatibility
- Documentation improvements
- New feature implementations

## License

StgSharp is licensed under the MIT License. See [LICENSE](LICENSE) for details.

## Future Plans

- **Vulkan Support**: Planned for version 2.0
- **Enhanced Cross-Platform**: Improved Linux and mobile support
- **Performance Optimizations**: Further SIMD and parallel processing improvements
- **Extended Language Support**: Additional domain-specific languages

## Support

For questions, issues, or contributions, please refer to the project repository or contact the development team.
