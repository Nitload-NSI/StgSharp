# Source Projects

This directory contains the production source code, native runtime, supporting applications, and benchmarks for Nitload. Most managed projects target .NET 8. The native project is built separately with CMake and provides platform and SIMD functionality consumed by managed projects.

## Directory map

| Directory | Purpose |
| --- | --- |
| `Nitload.Common` | Shared collections, memory management, threading, pipelines, platform abstractions, and other foundational APIs. |
| `Nitload.Mathematics` | Numeric types, vectors, matrices, geometry, and managed access to accelerated math operations. |
| `Nitload.Graphics` | OpenGL bindings, rendering infrastructure, shaders, textures, windows, and input integration. |
| `Nitload.Native` | C native runtime, GLFW/STBI integration, allocators, platform code, and optimized SIMD/matrix kernels. See its own `README.md` for native build details. |
| `Nitload.RegularAnalysis` | NGRA regular-language analysis, regex processing, intermediate representations, and source-generation infrastructure. |
| `Nitload.UserInterface` | State-oriented user-interface framework and its incremental source generator. |
| `Nitload.TerminalDialogue` | Terminal user-interface components and an interactive terminal application. |
| `Nitload.Benchmark` | BenchmarkDotNet workloads for measuring selected runtime and allocation behavior. |
| `Nitload.Script` | Retained sources from the earlier EXPRESS implementation. The planned Script layer has not started and is expected to build on NGRA. |
| `Nitload.Model` | Placeholder project whose design is being reconsidered. |
| `Nitload.Tools` | Small retained comparison and inspection artifacts; it is not currently a buildable project. |

## Build notes

Build the native library before running managed components that require it:

```powershell
cmake --preset clang-release -S src/Nitload.Native
cmake --build cmake_build/clang-release
dotnet build Nitload.sln
```

Managed builds copy an existing native binary when one is available; they do not invoke CMake automatically. Generated OpenGL files under `Nitload.Graphics/OpenGL/Generated` should be updated through the generator in `tools/Nitload.GenerateGL`, not edited by hand.
