//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="glFunction.v3_3"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.Graphics.OpenGL;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics.OpenGL
{
    public unsafe partial class OpenGLFunction
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ActiveTexture(
                    uint texture
        )
        {
            Context.glActiveTexture(texture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AttachShader(
                    GlHandle program,
                    GlHandle shader
        )
        {
            Context.glAttachShader(program.Value, shader.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BeginConditionalRender(
                    uint id,
                    uint mode
        )
        {
            Context.glBeginConditionalRender(id, mode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BeginQuery(
                    uint target,
                    uint id
        )
        {
            Context.glBeginQuery(target, id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BeginTransformFeedback(
                    uint primitiveMode
        )
        {
            Context.glBeginTransformFeedback(primitiveMode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindAttribLocation(
                    GlHandle program,
                    uint index,
                    ReadOnlySpan<byte> name
        )
        {
            fixed (byte* namePtr = name) {
                Context.glBindAttribLocation(program.Value, index, namePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindBuffer(
                    uint target,
                    GlHandle buffer
        )
        {
            Context.glBindBuffer(target, buffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindBufferBase(
                    uint target,
                    uint index,
                    GlHandle buffer
        )
        {
            Context.glBindBufferBase(target, index, buffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindBufferRange(
                    uint target,
                    uint index,
                    GlHandle buffer,
                    nint offset,
                    nuint size
        )
        {
            Context.glBindBufferRange(target, index, buffer.Value, offset, size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindFragDataLocation(
                    GlHandle program,
                    uint color,
                    ReadOnlySpan<byte> name
        )
        {
            fixed (byte* namePtr = name) {
                Context.glBindFragDataLocation(program.Value, color, namePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindFragDataLocationIndexed(
                    GlHandle program,
                    uint colorNumber,
                    uint index,
                    ReadOnlySpan<byte> name
        )
        {
            fixed (byte* namePtr = name) {
                Context.glBindFragDataLocationIndexed(program.Value, colorNumber, index, namePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindFramebuffer(
                    uint target,
                    GlHandle framebuffer
        )
        {
            Context.glBindFramebuffer(target, framebuffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindRenderbuffer(
                    uint target,
                    GlHandle renderbuffer
        )
        {
            Context.glBindRenderbuffer(target, renderbuffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindSampler(
                    uint unit,
                    GlHandle sampler
        )
        {
            Context.glBindSampler(unit, sampler.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindTexture(
                    uint target,
                    GlHandle texture
        )
        {
            Context.glBindTexture(target, texture.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindVertexArray(
                    uint array
        )
        {
            Context.glBindVertexArray(array);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlendColor(
                    float red,
                    float green,
                    float blue,
                    float alpha
        )
        {
            Context.glBlendColor(red, green, blue, alpha);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlendEquation(
                    uint mode
        )
        {
            Context.glBlendEquation(mode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlendEquationSeparate(
                    uint modeRGB,
                    uint modeAlpha
        )
        {
            Context.glBlendEquationSeparate(modeRGB, modeAlpha);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlendFunc(
                    uint sfactor,
                    uint dfactor
        )
        {
            Context.glBlendFunc(sfactor, dfactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlendFuncSeparate(
                    uint sfactorRGB,
                    uint dfactorRGB,
                    uint sfactorAlpha,
                    uint dfactorAlpha
        )
        {
            Context.glBlendFuncSeparate(sfactorRGB, dfactorRGB, sfactorAlpha, dfactorAlpha);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlitFramebuffer(
                    int srcX0,
                    int srcY0,
                    int srcX1,
                    int srcY1,
                    int dstX0,
                    int dstY0,
                    int dstX1,
                    int dstY1,
                    uint mask,
                    uint filter
        )
        {
            Context.glBlitFramebuffer(srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask,
                                      filter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BufferData(
                    uint target,
                    nuint size,
                    ReadOnlySpan<IntPtr> data,
                    uint usage
        )
        {
            fixed (IntPtr* dataPtr = data) {
                Context.glBufferData(target, size, dataPtr, usage);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BufferSubData(
                    uint target,
                    nint offset,
                    nuint size,
                    ReadOnlySpan<IntPtr> data
        )
        {
            fixed (IntPtr* dataPtr = data) {
                Context.glBufferSubData(target, offset, size, dataPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint CheckFramebufferStatus(
                    uint target
        )
        {
            return Context.glCheckFramebufferStatus(target);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClampColor(
                    uint target,
                    uint clamp
        )
        {
            Context.glClampColor(target, clamp);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear(
                    uint mask
        )
        {
            Context.glClear(mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearBufferfi(
                    uint buffer,
                    int drawbuffer,
                    float depth,
                    int stencil
        )
        {
            Context.glClearBufferfi(buffer, drawbuffer, depth, stencil);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearBufferfv(
                    uint buffer,
                    int drawbuffer,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glClearBufferfv(buffer, drawbuffer, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearBufferiv(
                    uint buffer,
                    int drawbuffer,
                    ReadOnlySpan<int> value
        )
        {
            fixed (int* valuePtr = value) {
                Context.glClearBufferiv(buffer, drawbuffer, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearBufferuiv(
                    uint buffer,
                    int drawbuffer,
                    ReadOnlySpan<uint> value
        )
        {
            fixed (uint* valuePtr = value) {
                Context.glClearBufferuiv(buffer, drawbuffer, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearColor(
                    float red,
                    float green,
                    float blue,
                    float alpha
        )
        {
            Context.glClearColor(red, green, blue, alpha);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearDepth(
                    double depth
        )
        {
            Context.glClearDepth(depth);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearStencil(
                    int s
        )
        {
            Context.glClearStencil(s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ClientWaitSync(
                    glSync sync,
                    uint flags,
                    IntPtr timeout
        )
        {
            return Context.glClientWaitSync(sync, flags, timeout);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ColorMask(
                    byte red,
                    byte green,
                    byte blue,
                    byte alpha
        )
        {
            Context.glColorMask(red, green, blue, alpha);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ColorMaski(
                    uint index,
                    byte r,
                    byte g,
                    byte b,
                    byte a
        )
        {
            Context.glColorMaski(index, r, g, b, a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CompileShader(
                    GlHandle shader
        )
        {
            Context.glCompileShader(shader.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CompressedTexImage1D(
                    uint target,
                    int level,
                    uint internalformat,
                    int width,
                    int border,
                    int imageSize,
                    ReadOnlySpan<IntPtr> data
        )
        {
            fixed (IntPtr* dataPtr = data) {
                Context.glCompressedTexImage1D(target, level, internalformat, width, border,
                                               imageSize, dataPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CompressedTexImage2D(
                    uint target,
                    int level,
                    uint internalformat,
                    int width,
                    int height,
                    int border,
                    int imageSize,
                    ReadOnlySpan<IntPtr> data
        )
        {
            fixed (IntPtr* dataPtr = data) {
                Context.glCompressedTexImage2D(target, level, internalformat, width, height, border,
                                               imageSize, dataPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CompressedTexImage3D(
                    uint target,
                    int level,
                    uint internalformat,
                    int width,
                    int height,
                    int depth,
                    int border,
                    int imageSize,
                    ReadOnlySpan<IntPtr> data
        )
        {
            fixed (IntPtr* dataPtr = data) {
                Context.glCompressedTexImage3D(target, level, internalformat, width, height, depth,
                                               border, imageSize, dataPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CompressedTexSubImage1D(
                    uint target,
                    int level,
                    int xoffset,
                    int width,
                    uint format,
                    int imageSize,
                    ReadOnlySpan<IntPtr> data
        )
        {
            fixed (IntPtr* dataPtr = data) {
                Context.glCompressedTexSubImage1D(target, level, xoffset, width, format, imageSize,
                                                  dataPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CompressedTexSubImage2D(
                    uint target,
                    int level,
                    int xoffset,
                    int yoffset,
                    int width,
                    int height,
                    uint format,
                    int imageSize,
                    ReadOnlySpan<IntPtr> data
        )
        {
            fixed (IntPtr* dataPtr = data) {
                Context.glCompressedTexSubImage2D(target, level, xoffset, yoffset, width, height,
                                                  format, imageSize, dataPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CompressedTexSubImage3D(
                    uint target,
                    int level,
                    int xoffset,
                    int yoffset,
                    int zoffset,
                    int width,
                    int height,
                    int depth,
                    uint format,
                    int imageSize,
                    ReadOnlySpan<IntPtr> data
        )
        {
            fixed (IntPtr* dataPtr = data) {
                Context.glCompressedTexSubImage3D(target, level, xoffset, yoffset, zoffset, width,
                                                  height, depth, format, imageSize, dataPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyBufferSubData(
                    uint readTarget,
                    uint writeTarget,
                    nint readOffset,
                    nint writeOffset,
                    nuint size
        )
        {
            Context.glCopyBufferSubData(readTarget, writeTarget, readOffset, writeOffset, size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTexImage1D(
                    uint target,
                    int level,
                    uint internalformat,
                    int x,
                    int y,
                    int width,
                    int border
        )
        {
            Context.glCopyTexImage1D(target, level, internalformat, x, y, width, border);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTexImage2D(
                    uint target,
                    int level,
                    uint internalformat,
                    int x,
                    int y,
                    int width,
                    int height,
                    int border
        )
        {
            Context.glCopyTexImage2D(target, level, internalformat, x, y, width, height, border);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTexSubImage1D(
                    uint target,
                    int level,
                    int xoffset,
                    int x,
                    int y,
                    int width
        )
        {
            Context.glCopyTexSubImage1D(target, level, xoffset, x, y, width);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTexSubImage2D(
                    uint target,
                    int level,
                    int xoffset,
                    int yoffset,
                    int x,
                    int y,
                    int width,
                    int height
        )
        {
            Context.glCopyTexSubImage2D(target, level, xoffset, yoffset, x, y, width, height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTexSubImage3D(
                    uint target,
                    int level,
                    int xoffset,
                    int yoffset,
                    int zoffset,
                    int x,
                    int y,
                    int width,
                    int height
        )
        {
            Context.glCopyTexSubImage3D(target, level, xoffset, yoffset, zoffset, x, y, width,
                                        height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GlHandle CreateProgram()
        {
            return GlHandle.Create(Context.glCreateProgram());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint CreateShader(
                    ShaderType type
        )
        {
            return Context.glCreateShader((uint)type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CullFace(
                    uint mode
        )
        {
            Context.glCullFace(mode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteBuffers(
                    int n,
                    ReadOnlySpan<uint> buffers
        )
        {
            fixed (uint* buffersPtr = buffers) {
                Context.glDeleteBuffers(n, buffersPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteFramebuffers(
                    int n,
                    ReadOnlySpan<uint> framebuffers
        )
        {
            fixed (uint* framebuffersPtr = framebuffers) {
                Context.glDeleteFramebuffers(n, framebuffersPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteProgram(
                    GlHandle program
        )
        {
            Context.glDeleteProgram(program.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteQueries(
                    int n,
                    ReadOnlySpan<uint> ids
        )
        {
            fixed (uint* idsPtr = ids) {
                Context.glDeleteQueries(n, idsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteRenderbuffers(
                    int n,
                    ReadOnlySpan<uint> renderbuffers
        )
        {
            fixed (uint* renderbuffersPtr = renderbuffers) {
                Context.glDeleteRenderbuffers(n, renderbuffersPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteSamplers(
                    int count,
                    ReadOnlySpan<uint> samplers
        )
        {
            fixed (uint* samplersPtr = samplers) {
                Context.glDeleteSamplers(count, samplersPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteShader(
                    GlHandle shader
        )
        {
            Context.glDeleteShader(shader.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteSync(
                    glSync sync
        )
        {
            Context.glDeleteSync(sync);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteTextures(
                    int n,
                    ReadOnlySpan<uint> textures
        )
        {
            fixed (uint* texturesPtr = textures) {
                Context.glDeleteTextures(n, texturesPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteVertexArrays(
                    int n,
                    ReadOnlySpan<uint> arrays
        )
        {
            fixed (uint* arraysPtr = arrays) {
                Context.glDeleteVertexArrays(n, arraysPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DepthFunc(
                    uint func
        )
        {
            Context.glDepthFunc(func);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DepthMask(
                    byte flag
        )
        {
            Context.glDepthMask(flag);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DepthRange(
                    double n,
                    double f
        )
        {
            Context.glDepthRange(n, f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DetachShader(
                    GlHandle program,
                    GlHandle shader
        )
        {
            Context.glDetachShader(program.Value, shader.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable(
                    uint cap
        )
        {
            Context.glDisable(cap);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disablei(
                    uint target,
                    uint index
        )
        {
            Context.glDisablei(target, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableVertexAttribArray(
                    uint index
        )
        {
            Context.glDisableVertexAttribArray(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawArrays(
                    uint mode,
                    int first,
                    int count
        )
        {
            Context.glDrawArrays(mode, first, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawArraysInstanced(
                    uint mode,
                    int first,
                    int count,
                    int instancecount
        )
        {
            Context.glDrawArraysInstanced(mode, first, count, instancecount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawBuffer(
                    uint buf
        )
        {
            Context.glDrawBuffer(buf);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawBuffers(
                    int n,
                    ReadOnlySpan<uint> bufs
        )
        {
            fixed (uint* bufsPtr = bufs) {
                Context.glDrawBuffers(n, bufsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawElements(
                    uint mode,
                    int count,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> indices
        )
        {
            fixed (IntPtr* indicesPtr = indices) {
                Context.glDrawElements(mode, count, (uint)type, indicesPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawElementsBaseVertex(
                    uint mode,
                    int count,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> indices,
                    int basevertex
        )
        {
            fixed (IntPtr* indicesPtr = indices) {
                Context.glDrawElementsBaseVertex(mode, count, (uint)type, indicesPtr, basevertex);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawElementsInstanced(
                    uint mode,
                    int count,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> indices,
                    int instancecount
        )
        {
            fixed (IntPtr* indicesPtr = indices) {
                Context.glDrawElementsInstanced(mode, count, (uint)type, indicesPtr, instancecount);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawElementsInstancedBaseVertex(
                    uint mode,
                    int count,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> indices,
                    int instancecount,
                    int basevertex
        )
        {
            fixed (IntPtr* indicesPtr = indices) {
                Context.glDrawElementsInstancedBaseVertex(mode, count, (uint)type, indicesPtr,
                                                          instancecount, basevertex);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawRangeElements(
                    uint mode,
                    uint start,
                    uint end,
                    int count,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> indices
        )
        {
            fixed (IntPtr* indicesPtr = indices) {
                Context.glDrawRangeElements(mode, start, end, count, (uint)type, indicesPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawRangeElementsBaseVertex(
                    uint mode,
                    uint start,
                    uint end,
                    int count,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> indices,
                    int basevertex
        )
        {
            fixed (IntPtr* indicesPtr = indices) {
                Context.glDrawRangeElementsBaseVertex(mode, start, end, count, (uint)type,
                                                      indicesPtr, basevertex);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable(
                    uint cap
        )
        {
            Context.glEnable(cap);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enablei(
                    uint target,
                    uint index
        )
        {
            Context.glEnablei(target, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnableVertexAttribArray(
                    uint index
        )
        {
            Context.glEnableVertexAttribArray(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EndConditionalRender()
        {
            Context.glEndConditionalRender();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EndQuery(
                    uint target
        )
        {
            Context.glEndQuery(target);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EndTransformFeedback()
        {
            Context.glEndTransformFeedback();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public glSync FenceSync(
                      uint condition,
                      uint flags
        )
        {
            return Context.glFenceSync(condition, flags);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Finish()
        {
            Context.glFinish();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Flush()
        {
            Context.glFlush();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FlushMappedBufferRange(
                    uint target,
                    nint offset,
                    nuint length
        )
        {
            Context.glFlushMappedBufferRange(target, offset, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FramebufferRenderbuffer(
                    uint target,
                    uint attachment,
                    uint renderbuffertarget,
                    GlHandle renderbuffer
        )
        {
            Context.glFramebufferRenderbuffer(target, attachment, renderbuffertarget,
                                              renderbuffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FramebufferTexture(
                    uint target,
                    uint attachment,
                    GlHandle texture,
                    int level
        )
        {
            Context.glFramebufferTexture(target, attachment, texture.Value, level);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FramebufferTexture1D(
                    uint target,
                    uint attachment,
                    uint textarget,
                    GlHandle texture,
                    int level
        )
        {
            Context.glFramebufferTexture1D(target, attachment, textarget, texture.Value, level);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FramebufferTexture2D(
                    uint target,
                    uint attachment,
                    uint textarget,
                    GlHandle texture,
                    int level
        )
        {
            Context.glFramebufferTexture2D(target, attachment, textarget, texture.Value, level);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FramebufferTexture3D(
                    uint target,
                    uint attachment,
                    uint textarget,
                    GlHandle texture,
                    int level,
                    int zoffset
        )
        {
            Context.glFramebufferTexture3D(target, attachment, textarget, texture.Value, level,
                                           zoffset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FramebufferTextureLayer(
                    uint target,
                    uint attachment,
                    GlHandle texture,
                    int level,
                    int layer
        )
        {
            Context.glFramebufferTextureLayer(target, attachment, texture.Value, level, layer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FrontFace(
                    uint mode
        )
        {
            Context.glFrontFace(mode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenBuffers(
                    int n,
                    uint* buffers
        )
        {
            Context.glGenBuffers(n, buffers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenerateMipmap(
                    uint target
        )
        {
            Context.glGenerateMipmap(target);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenFramebuffers(
                    int n,
                    uint* framebuffers
        )
        {
            Context.glGenFramebuffers(n, framebuffers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenQueries(
                    int n,
                    uint* ids
        )
        {
            Context.glGenQueries(n, ids);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenRenderbuffers(
                    int n,
                    uint* renderbuffers
        )
        {
            Context.glGenRenderbuffers(n, renderbuffers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenSamplers(
                    int count,
                    uint* samplers
        )
        {
            Context.glGenSamplers(count, samplers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenTextures(
                    int n,
                    uint* textures
        )
        {
            Context.glGenTextures(n, textures);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenVertexArrays(
                    int n,
                    uint* arrays
        )
        {
            Context.glGenVertexArrays(n, arrays);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetActiveAttrib(
                    GlHandle program,
                    uint index,
                    int bufSize,
                    int* length,
                    int* size,
                    uint* type,
                    byte* name
        )
        {
            Context.glGetActiveAttrib(program.Value, index, bufSize, length, size, type, name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetActiveUniform(
                    GlHandle program,
                    uint index,
                    int bufSize,
                    int* length,
                    int* size,
                    uint* type,
                    byte* name
        )
        {
            Context.glGetActiveUniform(program.Value, index, bufSize, length, size, type, name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetActiveUniformBlockiv(
                    GlHandle program,
                    uint uniformBlockIndex,
                    uint pname,
                    int* @params
        )
        {
            Context.glGetActiveUniformBlockiv(program.Value, uniformBlockIndex, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetActiveUniformBlockName(
                    GlHandle program,
                    uint uniformBlockIndex,
                    int bufSize,
                    int* length,
                    byte* uniformBlockName
        )
        {
            Context.glGetActiveUniformBlockName(program.Value, uniformBlockIndex, bufSize, length,
                                                uniformBlockName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetActiveUniformName(
                    GlHandle program,
                    uint uniformIndex,
                    int bufSize,
                    int* length,
                    byte* uniformName
        )
        {
            Context.glGetActiveUniformName(program.Value, uniformIndex, bufSize, length,
                                           uniformName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetActiveUniformsiv(
                    GlHandle program,
                    int uniformCount,
                    ReadOnlySpan<uint> uniformIndices,
                    uint pname,
                    int* @params
        )
        {
            fixed (uint* uniformIndicesPtr = uniformIndices) {
                Context.glGetActiveUniformsiv(program.Value, uniformCount, uniformIndicesPtr, pname,
                                              @params);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetAttachedShaders(
                    GlHandle program,
                    int maxCount,
                    int* count,
                    uint* shaders
        )
        {
            Context.glGetAttachedShaders(program.Value, maxCount, count, shaders);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetAttribLocation(
                   GlHandle program,
                   ReadOnlySpan<byte> name
        )
        {
            fixed (byte* namePtr = name) {
                return Context.glGetAttribLocation(program.Value, namePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetBooleani_v(
                    uint target,
                    uint index,
                    byte* data
        )
        {
            Context.glGetBooleani_v(target, index, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetBooleanv(
                    uint pname,
                    byte* data
        )
        {
            Context.glGetBooleanv(pname, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetBufferParameteri64v(
                    uint target,
                    uint pname,
                    IntPtr* @params
        )
        {
            Context.glGetBufferParameteri64v(target, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetBufferParameteriv(
                    uint target,
                    uint pname,
                    int* @params
        )
        {
            Context.glGetBufferParameteriv(target, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetBufferPointerv(
                    uint target,
                    uint pname,
                    IntPtr* @params
        )
        {
            Context.glGetBufferPointerv(target, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetBufferSubData(
                    uint target,
                    nint offset,
                    nuint size,
                    IntPtr* data
        )
        {
            Context.glGetBufferSubData(target, offset, size, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetCompressedTexImage(
                    uint target,
                    int level,
                    IntPtr* img
        )
        {
            Context.glGetCompressedTexImage(target, level, img);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetDoublev(
                    uint pname,
                    double* data
        )
        {
            Context.glGetDoublev(pname, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetError()
        {
            return Context.glGetError();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetFloatv(
                    uint pname,
                    float* data
        )
        {
            Context.glGetFloatv(pname, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetFragDataIndex(
                   GlHandle program,
                   ReadOnlySpan<byte> name
        )
        {
            fixed (byte* namePtr = name) {
                return Context.glGetFragDataIndex(program.Value, namePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetFragDataLocation(
                   GlHandle program,
                   ReadOnlySpan<byte> name
        )
        {
            fixed (byte* namePtr = name) {
                return Context.glGetFragDataLocation(program.Value, namePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetFramebufferAttachmentParameteriv(
                    uint target,
                    uint attachment,
                    uint pname,
                    int* @params
        )
        {
            Context.glGetFramebufferAttachmentParameteriv(target, attachment, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetInteger64i_v(
                    uint target,
                    uint index,
                    IntPtr* data
        )
        {
            Context.glGetInteger64i_v(target, index, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetInteger64v(
                    uint pname,
                    IntPtr* data
        )
        {
            Context.glGetInteger64v(pname, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetIntegeri_v(
                    uint target,
                    uint index,
                    int* data
        )
        {
            Context.glGetIntegeri_v(target, index, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetIntegerv(
                    uint pname,
                    int* data
        )
        {
            Context.glGetIntegerv(pname, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetMultisamplefv(
                    uint pname,
                    uint index,
                    float* val
        )
        {
            Context.glGetMultisamplefv(pname, index, val);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetProgramInfoLog(
                    GlHandle program,
                    int bufSize,
                    int* length,
                    byte* infoLog
        )
        {
            Context.glGetProgramInfoLog(program.Value, bufSize, length, infoLog);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetProgramiv(
                    GlHandle program,
                    uint pname,
                    int* @params
        )
        {
            Context.glGetProgramiv(program.Value, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetQueryiv(
                    uint target,
                    uint pname,
                    int* @params
        )
        {
            Context.glGetQueryiv(target, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetQueryObjecti64v(
                    uint id,
                    uint pname,
                    IntPtr* @params
        )
        {
            Context.glGetQueryObjecti64v(id, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetQueryObjectiv(
                    uint id,
                    uint pname,
                    int* @params
        )
        {
            Context.glGetQueryObjectiv(id, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetQueryObjectui64v(
                    uint id,
                    uint pname,
                    IntPtr* @params
        )
        {
            Context.glGetQueryObjectui64v(id, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetQueryObjectuiv(
                    uint id,
                    uint pname,
                    uint* @params
        )
        {
            Context.glGetQueryObjectuiv(id, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetRenderbufferParameteriv(
                    uint target,
                    uint pname,
                    int* @params
        )
        {
            Context.glGetRenderbufferParameteriv(target, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetSamplerParameterfv(
                    GlHandle sampler,
                    uint pname,
                    float* @params
        )
        {
            Context.glGetSamplerParameterfv(sampler.Value, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetSamplerParameterIiv(
                    GlHandle sampler,
                    uint pname,
                    int* @params
        )
        {
            Context.glGetSamplerParameterIiv(sampler.Value, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetSamplerParameterIuiv(
                    GlHandle sampler,
                    uint pname,
                    uint* @params
        )
        {
            Context.glGetSamplerParameterIuiv(sampler.Value, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetSamplerParameteriv(
                    GlHandle sampler,
                    uint pname,
                    int* @params
        )
        {
            Context.glGetSamplerParameteriv(sampler.Value, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetShaderInfoLog(
                    GlHandle shader,
                    int bufSize,
                    int* length,
                    byte* infoLog
        )
        {
            Context.glGetShaderInfoLog(shader.Value, bufSize, length, infoLog);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetShaderiv(
                    GlHandle shader,
                    uint pname,
                    int* @params
        )
        {
            Context.glGetShaderiv(shader.Value, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetShaderSource(
                    GlHandle shader,
                    int bufSize,
                    int* length,
                    byte* source
        )
        {
            Context.glGetShaderSource(shader.Value, bufSize, length, source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetString(
                      uint name
        )
        {
            return Marshal.PtrToStringAuto((nint)Context.glGetString(name)) ?? string.Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetStringi(
                      uint name,
                      uint index
        )
        {
            return Marshal.PtrToStringAuto((nint)Context.glGetStringi(name, index)) ?? string.Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetSynciv(
                    glSync sync,
                    uint pname,
                    int count,
                    int* length,
                    int* values
        )
        {
            Context.glGetSynciv(sync, pname, count, length, values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTexImage(
                    uint target,
                    int level,
                    uint format,
                    ShaderType type,
                    IntPtr* pixels
        )
        {
            Context.glGetTexImage(target, level, format, (uint)type, pixels);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTexLevelParameterfv(
                    uint target,
                    int level,
                    uint pname,
                    float* @params
        )
        {
            Context.glGetTexLevelParameterfv(target, level, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTexLevelParameteriv(
                    uint target,
                    int level,
                    uint pname,
                    int* @params
        )
        {
            Context.glGetTexLevelParameteriv(target, level, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTexParameterfv(
                    uint target,
                    uint pname,
                    float* @params
        )
        {
            Context.glGetTexParameterfv(target, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTexParameterIiv(
                    uint target,
                    uint pname,
                    int* @params
        )
        {
            Context.glGetTexParameterIiv(target, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTexParameterIuiv(
                    uint target,
                    uint pname,
                    uint* @params
        )
        {
            Context.glGetTexParameterIuiv(target, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTexParameteriv(
                    uint target,
                    uint pname,
                    int* @params
        )
        {
            Context.glGetTexParameteriv(target, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTransformFeedbackVarying(
                    GlHandle program,
                    uint index,
                    int bufSize,
                    int* length,
                    int* size,
                    uint* type,
                    byte* name
        )
        {
            Context.glGetTransformFeedbackVarying(program.Value, index, bufSize, length, size, type,
                                                  name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetUniformBlockIndex(
                    GlHandle program,
                    ReadOnlySpan<byte> uniformBlockName
        )
        {
            fixed (byte* uniformBlockNamePtr = uniformBlockName) {
                return Context.glGetUniformBlockIndex(program.Value, uniformBlockNamePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetUniformfv(
                    GlHandle program,
                    int location,
                    float* @params
        )
        {
            Context.glGetUniformfv(program.Value, location, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetUniformIndices(
                    GlHandle program,
                    int uniformCount,
                    ReadOnlySpan<byte> uniformNames,
                    uint* uniformIndices
        )
        {
            fixed (byte* uniformNamesPtr = uniformNames) {
                Context.glGetUniformIndices(program.Value, uniformCount, uniformNamesPtr,
                                            uniformIndices);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetUniformiv(
                    GlHandle program,
                    int location,
                    int* @params
        )
        {
            Context.glGetUniformiv(program.Value, location, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUniformLocation(
                   GlHandle program,
                   ReadOnlySpan<byte> name
        )
        {
            fixed (byte* namePtr = name) {
                return Context.glGetUniformLocation(program.Value, namePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetUniformuiv(
                    GlHandle program,
                    int location,
                    uint* @params
        )
        {
            Context.glGetUniformuiv(program.Value, location, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetVertexAttribdv(
                    uint index,
                    uint pname,
                    double* @params
        )
        {
            Context.glGetVertexAttribdv(index, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetVertexAttribfv(
                    uint index,
                    uint pname,
                    float* @params
        )
        {
            Context.glGetVertexAttribfv(index, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetVertexAttribIiv(
                    uint index,
                    uint pname,
                    int* @params
        )
        {
            Context.glGetVertexAttribIiv(index, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetVertexAttribIuiv(
                    uint index,
                    uint pname,
                    uint* @params
        )
        {
            Context.glGetVertexAttribIuiv(index, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetVertexAttribiv(
                    uint index,
                    uint pname,
                    int* @params
        )
        {
            Context.glGetVertexAttribiv(index, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetVertexAttribPointerv(
                    uint index,
                    uint pname,
                    IntPtr* pointer
        )
        {
            Context.glGetVertexAttribPointerv(index, pname, pointer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Hint(
                    uint target,
                    uint mode
        )
        {
            Context.glHint(target, mode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsBuffer(
                    GlHandle buffer
        )
        {
            return Context.glIsBuffer(buffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsEnabled(
                    uint cap
        )
        {
            return Context.glIsEnabled(cap);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsEnabledi(
                    uint target,
                    uint index
        )
        {
            return Context.glIsEnabledi(target, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsFramebuffer(
                    GlHandle framebuffer
        )
        {
            return Context.glIsFramebuffer(framebuffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsProgram(
                    GlHandle program
        )
        {
            return Context.glIsProgram(program.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsQuery(
                    uint id
        )
        {
            return Context.glIsQuery(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsRenderbuffer(
                    GlHandle renderbuffer
        )
        {
            return Context.glIsRenderbuffer(renderbuffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsSampler(
                    GlHandle sampler
        )
        {
            return Context.glIsSampler(sampler.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsShader(
                    GlHandle shader
        )
        {
            return Context.glIsShader(shader.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsSync(
                    glSync sync
        )
        {
            return Context.glIsSync(sync);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsTexture(
                    GlHandle texture
        )
        {
            return Context.glIsTexture(texture.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsVertexArray(
                    uint array
        )
        {
            return Context.glIsVertexArray(array);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LineWidth(
                    float width
        )
        {
            Context.glLineWidth(width);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LinkProgram(
                    GlHandle program
        )
        {
            Context.glLinkProgram(program.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogicOp(
                    uint opcode
        )
        {
            Context.glLogicOp(opcode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MapBuffer(
                    uint target,
                    uint access
        )
        {
            Context.glMapBuffer(target, access);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MapBufferRange(
                    uint target,
                    nint offset,
                    nuint length,
                    uint access
        )
        {
            Context.glMapBufferRange(target, offset, length, access);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MultiDrawArrays(
                    uint mode,
                    ReadOnlySpan<int> first,
                    ReadOnlySpan<int> count,
                    int drawcount
        )
        {
            fixed (int* firstPtr = first)
            {
                fixed (int* countPtr = count) {
                    Context.glMultiDrawArrays(mode, firstPtr, countPtr, drawcount);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MultiDrawElements(
                    uint mode,
                    ReadOnlySpan<int> count,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> indices,
                    int drawcount
        )
        {
            fixed (int* countPtr = count)
            {
                fixed (IntPtr* indicesPtr = indices) {
                    Context.glMultiDrawElements(mode, countPtr, (uint)type, indicesPtr, drawcount);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MultiDrawElementsBaseVertex(
                    uint mode,
                    ReadOnlySpan<int> count,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> indices,
                    int drawcount,
                    ReadOnlySpan<int> basevertex
        )
        {
            fixed (int* countPtr = count)
            {
                fixed (IntPtr* indicesPtr = indices)
                {
                    fixed (int* basevertexPtr = basevertex) {
                        Context.glMultiDrawElementsBaseVertex(mode, countPtr, (uint)type,
                                                              indicesPtr, drawcount, basevertexPtr);
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PixelStoref(
                    uint pname,
                    float param
        )
        {
            Context.glPixelStoref(pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PixelStorei(
                    uint pname,
                    int param
        )
        {
            Context.glPixelStorei(pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PointParameterf(
                    uint pname,
                    float param
        )
        {
            Context.glPointParameterf(pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PointParameterfv(
                    uint pname,
                    ReadOnlySpan<float> @params
        )
        {
            fixed (float* @paramsPtr = @params) {
                Context.glPointParameterfv(pname, @paramsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PointParameteri(
                    uint pname,
                    int param
        )
        {
            Context.glPointParameteri(pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PointParameteriv(
                    uint pname,
                    ReadOnlySpan<int> @params
        )
        {
            fixed (int* @paramsPtr = @params) {
                Context.glPointParameteriv(pname, @paramsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PointSize(
                    float size
        )
        {
            Context.glPointSize(size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PolygonMode(
                    uint face,
                    uint mode
        )
        {
            Context.glPolygonMode(face, mode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PolygonOffset(
                    float factor,
                    float units
        )
        {
            Context.glPolygonOffset(factor, units);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PrimitiveRestartIndex(
                    uint index
        )
        {
            Context.glPrimitiveRestartIndex(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProvokingVertex(
                    uint mode
        )
        {
            Context.glProvokingVertex(mode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void QueryCounter(
                    uint id,
                    uint target
        )
        {
            Context.glQueryCounter(id, target);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadBuffer(
                    uint src
        )
        {
            Context.glReadBuffer(src);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadPixels(
                    int x,
                    int y,
                    int width,
                    int height,
                    uint format,
                    ShaderType type,
                    IntPtr* pixels
        )
        {
            Context.glReadPixels(x, y, width, height, format, (uint)type, pixels);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RenderbufferStorage(
                    uint target,
                    uint internalformat,
                    int width,
                    int height
        )
        {
            Context.glRenderbufferStorage(target, internalformat, width, height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RenderbufferStorageMultisample(
                    uint target,
                    int samples,
                    uint internalformat,
                    int width,
                    int height
        )
        {
            Context.glRenderbufferStorageMultisample(target, samples, internalformat, width,
                                                     height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SampleCoverage(
                    float value,
                    byte invert
        )
        {
            Context.glSampleCoverage(value, invert);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SampleMaski(
                    uint maskNumber,
                    uint mask
        )
        {
            Context.glSampleMaski(maskNumber, mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SamplerParameterf(
                    GlHandle sampler,
                    uint pname,
                    float param
        )
        {
            Context.glSamplerParameterf(sampler.Value, pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SamplerParameterfv(
                    GlHandle sampler,
                    uint pname,
                    ReadOnlySpan<float> param
        )
        {
            fixed (float* paramPtr = param) {
                Context.glSamplerParameterfv(sampler.Value, pname, paramPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SamplerParameteri(
                    GlHandle sampler,
                    uint pname,
                    int param
        )
        {
            Context.glSamplerParameteri(sampler.Value, pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SamplerParameterIiv(
                    GlHandle sampler,
                    uint pname,
                    ReadOnlySpan<int> param
        )
        {
            fixed (int* paramPtr = param) {
                Context.glSamplerParameterIiv(sampler.Value, pname, paramPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SamplerParameterIuiv(
                    GlHandle sampler,
                    uint pname,
                    ReadOnlySpan<uint> param
        )
        {
            fixed (uint* paramPtr = param) {
                Context.glSamplerParameterIuiv(sampler.Value, pname, paramPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SamplerParameteriv(
                    GlHandle sampler,
                    uint pname,
                    ReadOnlySpan<int> param
        )
        {
            fixed (int* paramPtr = param) {
                Context.glSamplerParameteriv(sampler.Value, pname, paramPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Scissor(
                    int x,
                    int y,
                    int width,
                    int height
        )
        {
            Context.glScissor(x, y, width, height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ShaderSource(
                    GlHandle shader,
                    int count,
                    ReadOnlySpan<byte> @string,
                    ReadOnlySpan<int> length
        )
        {
            fixed (byte* stringPtr = @string)
            {
                byte* sPtr = stringPtr;
                fixed (int* lengthPtr = length) {
                    Context.glShaderSource(shader.Value, count, &sPtr, lengthPtr);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StencilFunc(
                    uint func,
                    int @ref,
                    uint mask
        )
        {
            Context.glStencilFunc(func, @ref, mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StencilFuncSeparate(
                    uint face,
                    uint func,
                    int @ref,
                    uint mask
        )
        {
            Context.glStencilFuncSeparate(face, func, @ref, mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StencilMask(
                    uint mask
        )
        {
            Context.glStencilMask(mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StencilMaskSeparate(
                    uint face,
                    uint mask
        )
        {
            Context.glStencilMaskSeparate(face, mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StencilOp(
                    uint fail,
                    uint zfail,
                    uint zpass
        )
        {
            Context.glStencilOp(fail, zfail, zpass);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StencilOpSeparate(
                    uint face,
                    uint sfail,
                    uint dpfail,
                    uint dppass
        )
        {
            Context.glStencilOpSeparate(face, sfail, dpfail, dppass);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexBuffer(
                    uint target,
                    uint internalformat,
                    GlHandle buffer
        )
        {
            Context.glTexBuffer(target, internalformat, buffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexImage1D(
                    uint target,
                    int level,
                    int internalformat,
                    int width,
                    int border,
                    uint format,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> pixels
        )
        {
            fixed (IntPtr* pixelsPtr = pixels) {
                Context.glTexImage1D(target, level, internalformat, width, border, format,
                                     (uint)type, pixelsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexImage2D(
                    uint target,
                    int level,
                    int internalformat,
                    int width,
                    int height,
                    int border,
                    uint format,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> pixels
        )
        {
            fixed (IntPtr* pixelsPtr = pixels) {
                Context.glTexImage2D(target, level, internalformat, width, height, border, format,
                                     (uint)type, pixelsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexImage2DMultisample(
                    uint target,
                    int samples,
                    uint internalformat,
                    int width,
                    int height,
                    byte fixedsamplelocations
        )
        {
            Context.glTexImage2DMultisample(target, samples, internalformat, width, height,
                                            fixedsamplelocations);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexImage3D(
                    uint target,
                    int level,
                    int internalformat,
                    int width,
                    int height,
                    int depth,
                    int border,
                    uint format,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> pixels
        )
        {
            fixed (IntPtr* pixelsPtr = pixels) {
                Context.glTexImage3D(target, level, internalformat, width, height, depth, border,
                                     format, (uint)type, pixelsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexImage3DMultisample(
                    uint target,
                    int samples,
                    uint internalformat,
                    int width,
                    int height,
                    int depth,
                    byte fixedsamplelocations
        )
        {
            Context.glTexImage3DMultisample(target, samples, internalformat, width, height, depth,
                                            fixedsamplelocations);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexParameterf(
                    uint target,
                    uint pname,
                    float param
        )
        {
            Context.glTexParameterf(target, pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexParameterfv(
                    uint target,
                    uint pname,
                    ReadOnlySpan<float> @params
        )
        {
            fixed (float* @paramsPtr = @params) {
                Context.glTexParameterfv(target, pname, @paramsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexParameteri(
                    uint target,
                    uint pname,
                    int param
        )
        {
            Context.glTexParameteri(target, pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexParameterIiv(
                    uint target,
                    uint pname,
                    ReadOnlySpan<int> @params
        )
        {
            fixed (int* @paramsPtr = @params) {
                Context.glTexParameterIiv(target, pname, @paramsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexParameterIuiv(
                    uint target,
                    uint pname,
                    ReadOnlySpan<uint> @params
        )
        {
            fixed (uint* @paramsPtr = @params) {
                Context.glTexParameterIuiv(target, pname, @paramsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexParameteriv(
                    uint target,
                    uint pname,
                    ReadOnlySpan<int> @params
        )
        {
            fixed (int* @paramsPtr = @params) {
                Context.glTexParameteriv(target, pname, @paramsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexSubImage1D(
                    uint target,
                    int level,
                    int xoffset,
                    int width,
                    uint format,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> pixels
        )
        {
            fixed (IntPtr* pixelsPtr = pixels) {
                Context.glTexSubImage1D(target, level, xoffset, width, format, (uint)type,
                                        pixelsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexSubImage2D(
                    uint target,
                    int level,
                    int xoffset,
                    int yoffset,
                    int width,
                    int height,
                    uint format,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> pixels
        )
        {
            fixed (IntPtr* pixelsPtr = pixels) {
                Context.glTexSubImage2D(target, level, xoffset, yoffset, width, height, format,
                                        (uint)type, pixelsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexSubImage3D(
                    uint target,
                    int level,
                    int xoffset,
                    int yoffset,
                    int zoffset,
                    int width,
                    int height,
                    int depth,
                    uint format,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> pixels
        )
        {
            fixed (IntPtr* pixelsPtr = pixels) {
                Context.glTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height,
                                        depth, format, (uint)type, pixelsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TransformFeedbackVaryings(
                    GlHandle program,
                    int count,
                    ReadOnlySpan<byte> varyings,
                    uint bufferMode
        )
        {
            fixed (byte* varyingsPtr = varyings) {
                Context.glTransformFeedbackVaryings(program.Value, count, varyingsPtr, bufferMode);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform1f(
                    int location,
                    float v0
        )
        {
            Context.glUniform1f(location, v0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform1fv(
                    int location,
                    int count,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glUniform1fv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform1i(
                    int location,
                    int v0
        )
        {
            Context.glUniform1i(location, v0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform1iv(
                    int location,
                    int count,
                    ReadOnlySpan<int> value
        )
        {
            fixed (int* valuePtr = value) {
                Context.glUniform1iv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform1ui(
                    int location,
                    uint v0
        )
        {
            Context.glUniform1ui(location, v0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform1uiv(
                    int location,
                    int count,
                    ReadOnlySpan<uint> value
        )
        {
            fixed (uint* valuePtr = value) {
                Context.glUniform1uiv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform2f(
                    int location,
                    float v0,
                    float v1
        )
        {
            Context.glUniform2f(location, v0, v1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform2fv(
                    int location,
                    int count,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glUniform2fv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform2i(
                    int location,
                    int v0,
                    int v1
        )
        {
            Context.glUniform2i(location, v0, v1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform2iv(
                    int location,
                    int count,
                    ReadOnlySpan<int> value
        )
        {
            fixed (int* valuePtr = value) {
                Context.glUniform2iv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform2ui(
                    int location,
                    uint v0,
                    uint v1
        )
        {
            Context.glUniform2ui(location, v0, v1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform2uiv(
                    int location,
                    int count,
                    ReadOnlySpan<uint> value
        )
        {
            fixed (uint* valuePtr = value) {
                Context.glUniform2uiv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform3f(
                    int location,
                    float v0,
                    float v1,
                    float v2
        )
        {
            Context.glUniform3f(location, v0, v1, v2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform3fv(
                    int location,
                    int count,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glUniform3fv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform3i(
                    int location,
                    int v0,
                    int v1,
                    int v2
        )
        {
            Context.glUniform3i(location, v0, v1, v2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform3iv(
                    int location,
                    int count,
                    ReadOnlySpan<int> value
        )
        {
            fixed (int* valuePtr = value) {
                Context.glUniform3iv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform3ui(
                    int location,
                    uint v0,
                    uint v1,
                    uint v2
        )
        {
            Context.glUniform3ui(location, v0, v1, v2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform3uiv(
                    int location,
                    int count,
                    ReadOnlySpan<uint> value
        )
        {
            fixed (uint* valuePtr = value) {
                Context.glUniform3uiv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform4f(
                    int location,
                    float v0,
                    float v1,
                    float v2,
                    float v3
        )
        {
            Context.glUniform4f(location, v0, v1, v2, v3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform4fv(
                    int location,
                    int count,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glUniform4fv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform4i(
                    int location,
                    int v0,
                    int v1,
                    int v2,
                    int v3
        )
        {
            Context.glUniform4i(location, v0, v1, v2, v3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform4iv(
                    int location,
                    int count,
                    ReadOnlySpan<int> value
        )
        {
            fixed (int* valuePtr = value) {
                Context.glUniform4iv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform4ui(
                    int location,
                    uint v0,
                    uint v1,
                    uint v2,
                    uint v3
        )
        {
            Context.glUniform4ui(location, v0, v1, v2, v3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform4uiv(
                    int location,
                    int count,
                    ReadOnlySpan<uint> value
        )
        {
            fixed (uint* valuePtr = value) {
                Context.glUniform4uiv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformBlockBinding(
                    GlHandle program,
                    uint uniformBlockIndex,
                    uint uniformBlockBinding
        )
        {
            Context.glUniformBlockBinding(program.Value, uniformBlockIndex, uniformBlockBinding);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformMatrix2fv(
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glUniformMatrix2fv(location, count, transpose, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformMatrix2x3fv(
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glUniformMatrix2x3fv(location, count, transpose, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformMatrix2x4fv(
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glUniformMatrix2x4fv(location, count, transpose, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformMatrix3fv(
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glUniformMatrix3fv(location, count, transpose, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformMatrix3x2fv(
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glUniformMatrix3x2fv(location, count, transpose, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformMatrix3x4fv(
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glUniformMatrix3x4fv(location, count, transpose, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformMatrix4fv(
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glUniformMatrix4fv(location, count, transpose, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformMatrix4x2fv(
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glUniformMatrix4x2fv(location, count, transpose, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformMatrix4x3fv(
                    int location,
                    int count,
                    byte transpose,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                Context.glUniformMatrix4x3fv(location, count, transpose, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte UnmapBuffer(
                    uint target
        )
        {
            return Context.glUnmapBuffer(target);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UseProgram(
                    GlHandle program
        )
        {
            Context.glUseProgram(program.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ValidateProgram(
                    GlHandle program
        )
        {
            Context.glValidateProgram(program.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib1d(
                    uint index,
                    double x
        )
        {
            Context.glVertexAttrib1d(index, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib1dv(
                    uint index,
                    ReadOnlySpan<double> v
        )
        {
            fixed (double* vPtr = v) {
                Context.glVertexAttrib1dv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib1f(
                    uint index,
                    float x
        )
        {
            Context.glVertexAttrib1f(index, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib1fv(
                    uint index,
                    ReadOnlySpan<float> v
        )
        {
            fixed (float* vPtr = v) {
                Context.glVertexAttrib1fv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib1s(
                    uint index,
                    short x
        )
        {
            Context.glVertexAttrib1s(index, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib1sv(
                    uint index,
                    ReadOnlySpan<short> v
        )
        {
            fixed (short* vPtr = v) {
                Context.glVertexAttrib1sv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib2d(
                    uint index,
                    double x,
                    double y
        )
        {
            Context.glVertexAttrib2d(index, x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib2dv(
                    uint index,
                    ReadOnlySpan<double> v
        )
        {
            fixed (double* vPtr = v) {
                Context.glVertexAttrib2dv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib2f(
                    uint index,
                    float x,
                    float y
        )
        {
            Context.glVertexAttrib2f(index, x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib2fv(
                    uint index,
                    ReadOnlySpan<float> v
        )
        {
            fixed (float* vPtr = v) {
                Context.glVertexAttrib2fv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib2s(
                    uint index,
                    short x,
                    short y
        )
        {
            Context.glVertexAttrib2s(index, x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib2sv(
                    uint index,
                    ReadOnlySpan<short> v
        )
        {
            fixed (short* vPtr = v) {
                Context.glVertexAttrib2sv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib3d(
                    uint index,
                    double x,
                    double y,
                    double z
        )
        {
            Context.glVertexAttrib3d(index, x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib3dv(
                    uint index,
                    ReadOnlySpan<double> v
        )
        {
            fixed (double* vPtr = v) {
                Context.glVertexAttrib3dv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib3f(
                    uint index,
                    float x,
                    float y,
                    float z
        )
        {
            Context.glVertexAttrib3f(index, x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib3fv(
                    uint index,
                    ReadOnlySpan<float> v
        )
        {
            fixed (float* vPtr = v) {
                Context.glVertexAttrib3fv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib3s(
                    uint index,
                    short x,
                    short y,
                    short z
        )
        {
            Context.glVertexAttrib3s(index, x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib3sv(
                    uint index,
                    ReadOnlySpan<short> v
        )
        {
            fixed (short* vPtr = v) {
                Context.glVertexAttrib3sv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4bv(
                    uint index,
                    ReadOnlySpan<sbyte> v
        )
        {
            fixed (sbyte* vPtr = v) {
                Context.glVertexAttrib4bv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4d(
                    uint index,
                    double x,
                    double y,
                    double z,
                    double w
        )
        {
            Context.glVertexAttrib4d(index, x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4dv(
                    uint index,
                    ReadOnlySpan<double> v
        )
        {
            fixed (double* vPtr = v) {
                Context.glVertexAttrib4dv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4f(
                    uint index,
                    float x,
                    float y,
                    float z,
                    float w
        )
        {
            Context.glVertexAttrib4f(index, x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4fv(
                    uint index,
                    ReadOnlySpan<float> v
        )
        {
            fixed (float* vPtr = v) {
                Context.glVertexAttrib4fv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4iv(
                    uint index,
                    ReadOnlySpan<int> v
        )
        {
            fixed (int* vPtr = v) {
                Context.glVertexAttrib4iv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4Nbv(
                    uint index,
                    ReadOnlySpan<sbyte> v
        )
        {
            fixed (sbyte* vPtr = v) {
                Context.glVertexAttrib4Nbv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4Niv(
                    uint index,
                    ReadOnlySpan<int> v
        )
        {
            fixed (int* vPtr = v) {
                Context.glVertexAttrib4Niv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4Nsv(
                    uint index,
                    ReadOnlySpan<short> v
        )
        {
            fixed (short* vPtr = v) {
                Context.glVertexAttrib4Nsv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4Nub(
                    uint index,
                    byte x,
                    byte y,
                    byte z,
                    byte w
        )
        {
            Context.glVertexAttrib4Nub(index, x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4Nubv(
                    uint index,
                    ReadOnlySpan<byte> v
        )
        {
            fixed (byte* vPtr = v) {
                Context.glVertexAttrib4Nubv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4Nuiv(
                    uint index,
                    ReadOnlySpan<uint> v
        )
        {
            fixed (uint* vPtr = v) {
                Context.glVertexAttrib4Nuiv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4Nusv(
                    uint index,
                    ReadOnlySpan<ushort> v
        )
        {
            fixed (ushort* vPtr = v) {
                Context.glVertexAttrib4Nusv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4s(
                    uint index,
                    short x,
                    short y,
                    short z,
                    short w
        )
        {
            Context.glVertexAttrib4s(index, x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4sv(
                    uint index,
                    ReadOnlySpan<short> v
        )
        {
            fixed (short* vPtr = v) {
                Context.glVertexAttrib4sv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4ubv(
                    uint index,
                    ReadOnlySpan<byte> v
        )
        {
            fixed (byte* vPtr = v) {
                Context.glVertexAttrib4ubv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4uiv(
                    uint index,
                    ReadOnlySpan<uint> v
        )
        {
            fixed (uint* vPtr = v) {
                Context.glVertexAttrib4uiv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4usv(
                    uint index,
                    ReadOnlySpan<ushort> v
        )
        {
            fixed (ushort* vPtr = v) {
                Context.glVertexAttrib4usv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribDivisor(
                    uint index,
                    uint divisor
        )
        {
            Context.glVertexAttribDivisor(index, divisor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI1i(
                    uint index,
                    int x
        )
        {
            Context.glVertexAttribI1i(index, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI1iv(
                    uint index,
                    ReadOnlySpan<int> v
        )
        {
            fixed (int* vPtr = v) {
                Context.glVertexAttribI1iv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI1ui(
                    uint index,
                    uint x
        )
        {
            Context.glVertexAttribI1ui(index, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI1uiv(
                    uint index,
                    ReadOnlySpan<uint> v
        )
        {
            fixed (uint* vPtr = v) {
                Context.glVertexAttribI1uiv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI2i(
                    uint index,
                    int x,
                    int y
        )
        {
            Context.glVertexAttribI2i(index, x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI2iv(
                    uint index,
                    ReadOnlySpan<int> v
        )
        {
            fixed (int* vPtr = v) {
                Context.glVertexAttribI2iv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI2ui(
                    uint index,
                    uint x,
                    uint y
        )
        {
            Context.glVertexAttribI2ui(index, x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI2uiv(
                    uint index,
                    ReadOnlySpan<uint> v
        )
        {
            fixed (uint* vPtr = v) {
                Context.glVertexAttribI2uiv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI3i(
                    uint index,
                    int x,
                    int y,
                    int z
        )
        {
            Context.glVertexAttribI3i(index, x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI3iv(
                    uint index,
                    ReadOnlySpan<int> v
        )
        {
            fixed (int* vPtr = v) {
                Context.glVertexAttribI3iv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI3ui(
                    uint index,
                    uint x,
                    uint y,
                    uint z
        )
        {
            Context.glVertexAttribI3ui(index, x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI3uiv(
                    uint index,
                    ReadOnlySpan<uint> v
        )
        {
            fixed (uint* vPtr = v) {
                Context.glVertexAttribI3uiv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI4bv(
                    uint index,
                    ReadOnlySpan<sbyte> v
        )
        {
            fixed (sbyte* vPtr = v) {
                Context.glVertexAttribI4bv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI4i(
                    uint index,
                    int x,
                    int y,
                    int z,
                    int w
        )
        {
            Context.glVertexAttribI4i(index, x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI4iv(
                    uint index,
                    ReadOnlySpan<int> v
        )
        {
            fixed (int* vPtr = v) {
                Context.glVertexAttribI4iv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI4sv(
                    uint index,
                    ReadOnlySpan<short> v
        )
        {
            fixed (short* vPtr = v) {
                Context.glVertexAttribI4sv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI4ubv(
                    uint index,
                    ReadOnlySpan<byte> v
        )
        {
            fixed (byte* vPtr = v) {
                Context.glVertexAttribI4ubv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI4ui(
                    uint index,
                    uint x,
                    uint y,
                    uint z,
                    uint w
        )
        {
            Context.glVertexAttribI4ui(index, x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI4uiv(
                    uint index,
                    ReadOnlySpan<uint> v
        )
        {
            fixed (uint* vPtr = v) {
                Context.glVertexAttribI4uiv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI4usv(
                    uint index,
                    ReadOnlySpan<ushort> v
        )
        {
            fixed (ushort* vPtr = v) {
                Context.glVertexAttribI4usv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribIPointer(
                    uint index,
                    int size,
                    ShaderType type,
                    int stride,
                    ReadOnlySpan<IntPtr> pointer
        )
        {
            fixed (IntPtr* pointerPtr = pointer) {
                Context.glVertexAttribIPointer(index, size, (uint)type, stride, pointerPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribP1ui(
                    uint index,
                    ShaderType type,
                    byte normalized,
                    uint value
        )
        {
            Context.glVertexAttribP1ui(index, (uint)type, normalized, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribP1uiv(
                    uint index,
                    ShaderType type,
                    byte normalized,
                    ReadOnlySpan<uint> value
        )
        {
            fixed (uint* valuePtr = value) {
                Context.glVertexAttribP1uiv(index, (uint)type, normalized, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribP2ui(
                    uint index,
                    ShaderType type,
                    byte normalized,
                    uint value
        )
        {
            Context.glVertexAttribP2ui(index, (uint)type, normalized, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribP2uiv(
                    uint index,
                    ShaderType type,
                    byte normalized,
                    ReadOnlySpan<uint> value
        )
        {
            fixed (uint* valuePtr = value) {
                Context.glVertexAttribP2uiv(index, (uint)type, normalized, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribP3ui(
                    uint index,
                    ShaderType type,
                    byte normalized,
                    uint value
        )
        {
            Context.glVertexAttribP3ui(index, (uint)type, normalized, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribP3uiv(
                    uint index,
                    ShaderType type,
                    byte normalized,
                    ReadOnlySpan<uint> value
        )
        {
            fixed (uint* valuePtr = value) {
                Context.glVertexAttribP3uiv(index, (uint)type, normalized, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribP4ui(
                    uint index,
                    ShaderType type,
                    byte normalized,
                    uint value
        )
        {
            Context.glVertexAttribP4ui(index, (uint)type, normalized, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribP4uiv(
                    uint index,
                    ShaderType type,
                    byte normalized,
                    ReadOnlySpan<uint> value
        )
        {
            fixed (uint* valuePtr = value) {
                Context.glVertexAttribP4uiv(index, (uint)type, normalized, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribPointer(
                    uint index,
                    int size,
                    ShaderType type,
                    byte normalized,
                    int stride,
                    ReadOnlySpan<IntPtr> pointer
        )
        {
            fixed (IntPtr* pointerPtr = pointer) {
                Context.glVertexAttribPointer(index, size, (uint)type, normalized, stride,
                                              pointerPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Viewport(
                    int x,
                    int y,
                    int width,
                    int height
        )
        {
            Context.glViewport(x, y, width, height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WaitSync(
                    glSync sync,
                    uint flags,
                    IntPtr timeout
        )
        {
            Context.glWaitSync(sync, flags, timeout);
        }

    }
}
