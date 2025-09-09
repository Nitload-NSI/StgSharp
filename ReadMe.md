# StgSharp

![Stg#Logo](STG%23LOGO.png "Stg#LOGO")

## Introduction

StgSharp was originally designed as a next-generation STG (shooting game) engine, but now it has evolved to cover both game development and high-performance computing (HPC) scenarios. The project includes low-level and high-level OpenGL bindings, multimedia processing, device control, and a variety of modules for math, geometry, parallel computing, and more.

**Note:** StgSharp is currently in an early and exploratory stage. Many features are experimental, and the project direction covers both game engine and HPC functionalities. The modules are diverse and not yet fully integrated. Usage examples are not provided at this time, as the functionality is broad and not yet stable enough for typical use cases.


## Core Modules

The StgSharp engine consists of the following main modules:

### 1. StgSharp.Common - Core Library
- **High-Performance Math Library**: Scalar operations, linear algebra, matrix operations, and mathematical functions
- **Graphics Rendering System**: Complete OpenGL support including OpenGL 4.6 core functionality
- **MVVM Framework**: Windowed application framework based on MVVM pattern
- **High-Performance Computing**: SIMD optimization, HLSF memory allocator, parallel computing support
- **Geometry System**: 2D/3D geometry, collision detection, coordinate transformations
- **Entity System**: Game entity management, particle systems, launchers
- **Pipeline System**: Multi-threaded pipeline scheduler with concurrent processing support

### 2. StgSharp.Script - Scripting System
- **EXPRESS Language Support**: Compilation and parsing support for [EXPRESS language](https://www.expresslang.org/) (ISO 10303-11)
- **Syntax Analyzer**: Complete lexical and syntax analysis functionality
- **Code Generation**: Convert EXPRESS code to C# code
- **Built-in Type System**: Support for integers, real numbers, booleans, strings, and other basic types

### 3. StgSharp.Model - Model System
- **STEP File Support**: Support for STEP format 3D model files
- **Geometry Data Processing**: Model geometry processing and optimization

### 4. StgSharp.Native - Native Library
- **GLFW Bindings**: Cross-platform viewport management based on [GLFW](https://www.glfw.org/)
- **Native Function Interface**: Low-level system calls and hardware acceleration functionality

### 5. StgSharp.TerminalDialogue - Terminal Dialogue System
- **Command Line Interface**: Terminal interaction functionality
- **Dialogue Management**: Support for complex dialogue flow control

## Technical Features

- **Cross-Platform Support**: Windows, Linux, and other platforms
- **High-Performance Rendering**: Hardware-accelerated rendering based on OpenGL
- **Memory Optimization**: HLSF memory allocator providing O(1) time complexity memory management
- **Parallel Computing**: Multi-threaded parallel computing and SIMD instruction optimization
- **Modern C# Features**: Uses .NET 8.0 and C# preview features

## Installation and Usage

### System Requirements

- **.NET 8.0** or higher
- **Windows 10/11** or **Linux** operating system
- **OpenGL 4.6** compatible graphics drivers
- **Visual Studio 2022** or **JetBrains Rider** (recommended)

### Building the Project

1. Clone the repository:
```bash
git clone https://github.com/Nitload-NSI/StgSharp.git
cd StgSharp
```

2. Open the solution in Visual Studio:
   - Open `StgSharp.sln` in Visual Studio 2022
   - The solution will automatically restore NuGet packages
   - Build the solution using `Build > Build Solution` or press `Ctrl+Shift+B`

**Note**: Currently, only Visual Studio build is supported. Command-line build scripts are not yet implemented.

### Project Structure

```
StgSharp/
├── StgSharp.Common/          # Core library
├── StgSharp.Script/          # Scripting system
├── StgSharp.Model/           # Model system
├── StgSharp.Native/          # Native library
├── StgSharp.TerminalDialogue/ # Terminal dialogue system
└── StgSgharp.Generator/      # Code generator
```


## Development Roadmap

### Short-term Goals

- **Vulkan Support**: Plan to add Vulkan graphics API support in version 2.0
- **Performance Optimization**: Further optimize memory allocation and rendering performance
- **Documentation**: Provide more detailed API documentation and usage tutorials
- **Sample Projects**: Create complete game sample projects

### Long-term Goals

- **Cross-Platform Expansion**: Enhanced support for more operating systems
- **Graphics API Diversity**: Support for DirectX, Metal, and other graphics APIs
- **Editor Tools**: Develop visual editor tools
- **Community Ecosystem**: Establish plugin system and community contribution mechanisms

## Contributing

We welcome community contributions! If you would like to contribute to the StgSharp project, please:

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact


- **GitHub Issues**: [Submit issues or suggestions](https://github.com/Nitload-NSI/StgSharp/issues)
- **Update Log**: Check [UpdateBlog.md](UpdateBlog.md) for the latest updates

## Documentation

This section provides links to technical documents and guides for different namespaces and modules in StgSharp. For more details, please refer to the corresponding folders and files in the repository. If you have questions or suggestions about documentation, feel free to submit an issue or pull request.

### Documentation Index

#### General
  [UpdateBlog.md](UpdateBlog.md): Project update log    
  [LICENSE](LICENSE): Project license    
  [StgSharp.Native/naming.md](StgSharp.Native/naming.md): Native module naming conventions

#### StgSharp.Common
- StgSharp.HighPerformance  
    [HLSF Introduction](docs/HLSF.md): Details about the HLSF memory allocator

#### StgSharp.Native


