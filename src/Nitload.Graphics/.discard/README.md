# StgSharp.Graphics retired sources

This directory contains source snapshots retired during the Graphics/OpenGL
reorganization on 2026-08-26. Files use the `.cs.old` suffix so that SDK-style
project globbing does not compile them.

## Retired without an active replacement

- `framework.cs.old`: unused GL/debug delegate declarations.
- `Vulkan.*.cs.old`: unreferenced Vulkan placeholders.
- `OpenGL.glRenderStream.ThreadSecurity.cs.old`: unused thread/context fields.
- `OpenGL.Generated.GlCommandHistory.g.cs.old`: generator report data that does
  not belong in the runtime assembly.

## Split before retirement

- `gl.cs.old`: image P/Invokes were temporarily split out and later retired;
  legacy OpenGL native helpers were replaced with direct calls through the
  OpenGL function table.
- `glfw.dtruct.cs.old`: reduced to the four opaque GLFW types currently used by
  the wrapper in `glfw.struct.cs`.
- `OpenGL.glType.cs.old`: only `glSync` remains active in `OpenGL/glSync.cs`.
- `ShaderEdit.ShaderGenerator.cs.old`: only the temporary
  `InternalShaderType` compatibility enum remains active.
- `GraphicInitializer.cs.old`: merged into the correctly named `GraphicModule`
  in `glfw.cs`.
- `OpenGL.glApiManager.cs.old` and `OpenGL.glApiManager.Const.cs.old`: the active
  compatibility loader retains only core-version loading; the unused extension
  classification table and 619 write-only flags were retired.

## Retired during the image simplification

- `Image.cs.old`: the former image/decoder/OpenGL-format aggregate.
- `ImageInfo.cs.old` and `Image.Native.cs.old`: the mismatched native decoder
  state and its P/Invoke bridge.
- `Pixels.cs.old`: the unused and incorrectly sized legacy pixel abstraction.
- `CPUanimation.ImageHelper.cs.old`: incomplete resizing and movement logic;
  image transformations now belong to a caller-selected codec or image library.
- `CPUanimation.IImageProvider.cs.old`: the redundant image-returns-itself
  provider contract.
- `TexturePipeline.cs.old`: an unused pipeline payload tied to the retired
  non-generic image type.
- `OpenGL.Buffer.AutoTexture.cs.old`: the unused texture cache depended on image
  mutation counters and also contained incorrect slot/recycling bookkeeping.

## Retired during the data-only OpenGL resource migration

- `Define.BufferObjectBase.cs.old` and
  `OpenGL.Buffer.GLBufferObjectBase.cs.old`: the former resource base classes
  injected `glRender`/`OpenGLFunction` and attempted context-bound disposal from
  resource objects and finalizers.
- `OpenGL.glRenderObjectGenrator.cs.old`: the former factories implicitly
  created resources through a render-stream instance.
- `OpenGL.ShaderEdit.IglConvertable.cs.old`, `ShaderEdit.ShaderStruct.cs.old`,
  and `ShaderEdit.InternalShaderType.cs.old`: the obsolete shader conversion
  layer submitted uniforms through the global `CurrentGL` context.

Active OpenGL resource and shader types are passive handle records. Creation,
binding, upload, compilation, use, and deletion are explicitly submitted to an
`OpenGLFunction` instance.

The active `Image<TPixel>` is only a tightly packed managed bitmap buffer. It
does not load files, convert formats, resize pixels, or retain native memory.

These files are historical references, not candidates for restoration as a
group. Restore individual behavior only after checking it against the new
Graphics and OpenGL architecture.
