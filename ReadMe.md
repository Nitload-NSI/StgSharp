# StgSharp

![StgSharp Logo](STG%23LOGO.png)

StgSharp is a modern C# framework that combines game engine capabilities with high-performance computing features. Originally designed as a next-generation STG (Shoot 'em Up) game engine, it has evolved to include advanced mathematical libraries, parallel computing capabilities, and comprehensive graphics APIs.

## Overview

StgSharp provides both low-level and high-level bindings to OpenGL, multimedia control, and device management, offering everything needed to create STG games and high-performance applications. The framework is built on .NET 8 and supports Windows and Linux platforms.

## Core Modules

### StgSharp.Common

The main library containing core functionality:

- **Mathematics**: High-performance linear algebra, matrix operations, and mathematical utilities
- **Graphics**: OpenGL bindings, rendering pipeline, and graphics framework
- **High Performance**: SIMD operations, memory management, and parallel computing
- **Collections**: Specialized data structures and containers
- **MVVM**: Model-View-ViewModel framework for UI applications
- **Entities**: Game entity system and behavior management
- **Environment**: Platform abstraction and system integration

### StgSharp.Script

General-purpose syntax analysis framework:

- **Syntax Analysis Framework**: Provides a flexible framework for parsing and analyzing various programming languages
- **EXPRESS Language Support**: Specialized support for ISO 10303-11 EXPRESS language syntax parsing
- **Abstract Syntax Trees**: Language parsing and analysis infrastructure
- **Token Processing**: Lexical analysis and token management
- **Grammar Parsing**: Extensible grammar parsing and validation system

### StgSharp.Model

Geometric model loading and interpretation:

- **Geometric Model Loading**: Responsible for loading and interpreting geometric models from various formats
- **STEP Format Support**: Limited support for ISO 10303 STEP file format processing
- **Entity Management**: Model entity parsing and geometric data interpretation
- **Data Pipeline**: Efficient geometric data processing and transformation
- **Schema Validation**: Model structure validation and verification

### StgSharp.Native

Native library integration:

- **GLFW Integration**: Cross-platform window and input management
- **SIMD Operations**: Hardware-accelerated vector operations
- **Matrix Kernels**: Optimized matrix computation routines
- **Context Management**: Graphics context and resource management

### StgSharp.TerminalDialogue

Terminal-based user interface:

- **Interactive Terminal**: Command-line interface components
- **Dialogue System**: User interaction and input handling
- **Terminal Graphics**: Text-based UI rendering

## Key Features

### High-Performance Computing

- **Parallel Matrix Operations**: Multi-threaded matrix computations with optimized scheduling
- **SIMD Acceleration**: Hardware-optimized vector operations
- **Memory Management**: Custom allocators including HLSF (Hybrid Layer Segregated Fit)
- **Thread Pool Management**: Efficient task scheduling and execution

#### HLSF Benchmark Snapshot (BenchmarkDotNet)

```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.7623)
AMD Ryzen 7 6800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.102
  [Host] : .NET 8.0.23 (8.0.2325.60607), X64 RyuJIT AVX2

Toolchain=InProcessEmitToolchain  InvocationCount=2048  UnrollFactor=1

```

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|------- |-----:|-----:|------:|------:|-------:|
| Hlsf | 76.23 µs | 0.355 µs | 0.315 µs | 0.44 | 0.01 |
| Libc | 172.98 µs | 3.042 µs | 2.988 µs | 1.00 | 0.02 |

This run shows HLSF significantly faster than libc under the current configuration. See the HLSF document for details and caveats.

### Graphics and Rendering

- **OpenGL Integration**: Complete OpenGL API coverage
- **Cross-Platform Support**: Windows and Linux compatibility
- **Rendering Pipeline**: Flexible and extensible graphics framework
- **Shader Management**: Dynamic shader compilation and management

### Game Engine Capabilities

- **Entity System**: Flexible game object management
- **State Machine**: Game state and behavior management
- **Input Handling**: Comprehensive input and control systems
- **Resource Management**: Efficient asset loading and management

### Data Processing

- **STEP File Support**: Industrial data format processing
- **EXPRESS Language**: Domain-specific language compilation
- **Pipeline Processing**: Efficient data transformation workflows

## Requirements

- .NET 8.0 or later
- Windows 10/11 or Linux
- x64 or ARM64 architecture
- OpenGL 3.3+ support

## Installation

StgSharp is currently in development and requires compilation from source. **NuGet packages are not yet available** - you must build the project from source code.

### Prerequisites

- **Windows**: Visual Studio 2022 or later with C++ development tools
- **Linux**: CMake and compatible C++ compiler (planned support)
- .NET 8.0 SDK or later
- Git for source code retrieval

### Building from Source

1. **Clone the repository**:
   ```bash
   git clone https://github.com/Nitload-NSI/StgSharp.git
   cd StgSharp
   ```

2. **Windows (Recommended)**:
   - Open `StgSharp.sln` in Visual Studio
   - Build the solution (Debug or Release configuration)
   - The native components will be automatically compiled

3. **Linux (Experimental)**:
   ```bash
   # Build .NET components only
   # Note: Native components are not yet supported on Linux
   dotnet build
   ```

### Build Configuration

- **Debug**: Full debugging symbols, unoptimized
- **Release**: Optimized for performance

### Notes

- Windows compilation with Visual Studio is fully supported and tested
- Linux support is experimental and limited to .NET components only
- Native components (StgSharp.Native) are currently Windows-only and require MSVC
- CMake support for native components is not yet implemented
- Some features may not be available on all platforms

## Quick Start

### Basic Graphics Application

```csharp

/*To be implemented*/

```

### Matrix Operations

```csharp
using StgSharp.Mathematics;

// Create matrices
var matrixA = Matrix<float>.Create(4, 4);
var matrixB = Matrix<float>.Create(4, 4);

// Perform parallel matrix multiplication
var result = matrixA * matrixB;
```

### Memory Management

```csharp
using StgSharp.HighPerformance.Memory;

// Create high-performance allocator
using var allocator = new HybridLayerSegregatedFitAllocator(64 * 1024 * 1024);

// Allocate memory
var handle = allocator.Alloc(1024);
// Use allocated memory
allocator.Free(handle);
```


## Documentation

### Technical Documentation
- [HLSF Allocator](StgSharp.Common/HighPerformance/Memory/InroductionToHLSF.md) - High-performance memory allocator documentation
- [Native Library Naming](StgSharp.Native/naming.md) - Native library file naming conventions

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