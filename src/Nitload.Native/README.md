# Nitload.Native

## Naming convention

The C implementation uses `nfn` for Nitload Framework Native: exported functions use
`nfn_`, macros and CMake cache options use `NFN_`, and native source folders and
headers use `nfn-` and `nfn_`. The shared library remains named `Nitload.Native`.
The longer prefix also distinguishes these names from Linux Netfilter's `nf_` names.

This migration renames the exported C symbols from `nif_*` to `nfn_*`. Native
binaries and managed assemblies must therefore be rebuilt and deployed together;
pre-migration P/Invoke entry points are not compatible with a newly built library.
The old `NIF_*` CMake cache options are no longer used. Reconfigure an existing
build tree with the corresponding `NFN_*` options, or use a fresh build tree.

## COMPILE

Though this project do has a .vcxproj file,  
you can only edit but not compile in visul studio.  
Currently you can only compile this lib via Clang and cmake.

## Build output and managed deployment

From the repository root:

```powershell
# On Windows, add your LLVM bin directory if clang is not already on PATH.
$env:PATH = 'C:\Program Files\LLVM\bin;' + $env:PATH
cmake --preset clang-debug -S src/Nitload.Native
cmake --build cmake_build/clang-debug --target Nitload.Native
```

The presets require `clang` on `PATH`. If an existing CMake cache was configured
with an absolute compiler path, make `clang` available on `PATH` before running
the preset again. The `NFN_*` options replace the former `NIF_*` options.

CMake writes `Nitload.Native.dll` and its linker PDB to `bin/win-x64` on Windows x64
(Linux uses `bin/linux-x64/libNitload.Native.so`). Debug and release presets currently
share this platform directory; the most recent native build is the deployed version.
`NFN_BUILD_OUTPUT_DIR` can override the destination. Managed builds pass their expected
RID as `NFN_EXPECTED_RID` so an incompatible native target is rejected before deployment.

`StgSharpDebug.csproj` enables `BuildNativeBeforeManaged` and `RequireNativeLibrary`.
Its build runs the incremental CMake build (`clang-debug` for Debug, `clang-release`
otherwise), then deploys the native library to the executable's actual `TargetDir`.
Other managed projects copy an existing native artifact without invoking CMake.
Managed-output staging never copies deployed native files back over the CMake artifact.

To use a prebuilt native artifact, pass `-p:BuildNativeBeforeManaged=false`. A matching
Clang/Ninja environment and the local GLFW static library are required for native builds.
Cross-target builds require a matching toolchain/preset; changing a managed platform alone
does not cross-compile the C library.

## clangd in VS Code

The `.clangd` file searches for `compile_commands.json` in `src/Nitload.Native`.
When using CMake Tools, set `cmake.copyCompileCommands` to
`${workspaceFolder}/src/Nitload.Native/compile_commands.json` so the active Debug or Release preset
updates that file after configuration. For command-line configuration, copy
`cmake_build/<preset>/compile_commands.json` to `src/Nitload.Native`. Restart
clangd after changing the database or renaming headers.

## Code Layout

Conventions:

- src/        : project C sources and CMakeLists.txt calling add_library/add_executable
- include/    : public headers (if used)
- external/   : third-party libraries, each library lives in its own folder
    - libname/
        - head/   : header files for the library
        - lib/    : static or shared lib files (or CMake build for that lib)

The root `CMakeLists.txt` in this folder includes `ExternalLibs.cmake` if present.
