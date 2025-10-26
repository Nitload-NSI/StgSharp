StgSharp.Native layout

Conventions:
- src/        : project C sources and CMakeLists.txt calling add_library/add_executable
- include/    : public headers (if used)
- external/   : third-party libraries, each library lives in its own folder
    - <libname>/
        - head/   : header files for the library
        - lib/    : static or shared lib files (or CMake build for that lib)

The root `CMakeLists.txt` in this folder includes `ExternalLibs.cmake` if present.
