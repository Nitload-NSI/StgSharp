# Khronos OpenGL Registry input

This directory intentionally does not track the third-party Khronos registry files. Before building or running `StgSharp.GenerateGL`, supply a registry snapshot from the canonical [KhronosGroup/OpenGL-Registry](https://github.com/KhronosGroup/OpenGL-Registry) repository.

1. Clone or download one coherent revision of `KhronosGroup/OpenGL-Registry`.
2. Copy the contents of its `xml/` directory directly into this directory.
3. Keep the upstream directory layout unchanged.

The generator currently requires this exact path:

```text
tools/StgSharp.GenerateGL/xml/gl.xml
```

Do not introduce an additional directory level such as `xml/xml/gl.xml`. Files such as `glx.xml`, `wgl.xml`, `registry.rnc`, and the upstream registry scripts may be copied alongside `gl.xml`, but the current desktop OpenGL generator only consumes `gl.xml`.

For reproducible generation, use files from the same upstream commit and record that commit hash with the generated-code change. All copied files in this directory are ignored by Git except this README.
