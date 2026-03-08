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
            _context->glActiveTexture(texture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AttachShader(
                    GlHandle program,
                    GlHandle shader
        )
        {
            _context->glAttachShader(program.Value, shader.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BeginConditionalRender(
                    uint id,
                    uint mode
        )
        {
            _context->glBeginConditionalRender(id, mode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BeginQuery(
                    uint target,
                    uint id
        )
        {
            _context->glBeginQuery(target, id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BeginTransformFeedback(
                    uint primitiveMode
        )
        {
            _context->glBeginTransformFeedback(primitiveMode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindAttribLocation(
                    GlHandle program,
                    uint index,
                    ReadOnlySpan<byte> name
        )
        {
            fixed (byte* namePtr = name) {
                _context->glBindAttribLocation(program.Value, index, namePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindBuffer(
                    uint target,
                    GlHandle buffer
        )
        {
            _context->glBindBuffer(target, buffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindBufferBase(
                    uint target,
                    uint index,
                    GlHandle buffer
        )
        {
            _context->glBindBufferBase(target, index, buffer.Value);
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
            _context->glBindBufferRange(target, index, buffer.Value, offset, size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindFragDataLocation(
                    GlHandle program,
                    uint color,
                    ReadOnlySpan<byte> name
        )
        {
            fixed (byte* namePtr = name) {
                _context->glBindFragDataLocation(program.Value, color, namePtr);
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
                _context->glBindFragDataLocationIndexed(program.Value, colorNumber, index, namePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindFramebuffer(
                    uint target,
                    GlHandle framebuffer
        )
        {
            _context->glBindFramebuffer(target, framebuffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindRenderbuffer(
                    uint target,
                    GlHandle renderbuffer
        )
        {
            _context->glBindRenderbuffer(target, renderbuffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindSampler(
                    uint unit,
                    GlHandle sampler
        )
        {
            _context->glBindSampler(unit, sampler.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindTexture(
                    uint target,
                    GlHandle texture
        )
        {
            _context->glBindTexture(target, texture.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindVertexArray(
                    uint array
        )
        {
            _context->glBindVertexArray(array);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlendColor(
                    float red,
                    float green,
                    float blue,
                    float alpha
        )
        {
            _context->glBlendColor(red, green, blue, alpha);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlendEquation(
                    uint mode
        )
        {
            _context->glBlendEquation(mode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlendEquationSeparate(
                    uint modeRGB,
                    uint modeAlpha
        )
        {
            _context->glBlendEquationSeparate(modeRGB, modeAlpha);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlendFunc(
                    uint sfactor,
                    uint dfactor
        )
        {
            _context->glBlendFunc(sfactor, dfactor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlendFuncSeparate(
                    uint sfactorRGB,
                    uint dfactorRGB,
                    uint sfactorAlpha,
                    uint dfactorAlpha
        )
        {
            _context->glBlendFuncSeparate(sfactorRGB, dfactorRGB, sfactorAlpha, dfactorAlpha);
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
            _context->glBlitFramebuffer(srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask,
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
                _context->glBufferData(target, size, dataPtr, usage);
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
                _context->glBufferSubData(target, offset, size, dataPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint CheckFramebufferStatus(
                    uint target
        )
        {
            return _context->glCheckFramebufferStatus(target);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClampColor(
                    uint target,
                    uint clamp
        )
        {
            _context->glClampColor(target, clamp);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear(
                    uint mask
        )
        {
            _context->glClear(mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearBufferfi(
                    uint buffer,
                    int drawbuffer,
                    float depth,
                    int stencil
        )
        {
            _context->glClearBufferfi(buffer, drawbuffer, depth, stencil);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearBufferfv(
                    uint buffer,
                    int drawbuffer,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                _context->glClearBufferfv(buffer, drawbuffer, valuePtr);
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
                _context->glClearBufferiv(buffer, drawbuffer, valuePtr);
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
                _context->glClearBufferuiv(buffer, drawbuffer, valuePtr);
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
            _context->glClearColor(red, green, blue, alpha);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearDepth(
                    double depth
        )
        {
            _context->glClearDepth(depth);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearStencil(
                    int s
        )
        {
            _context->glClearStencil(s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ClientWaitSync(
                    glSync sync,
                    uint flags,
                    IntPtr timeout
        )
        {
            return _context->glClientWaitSync(sync, flags, timeout);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ColorMask(
                    byte red,
                    byte green,
                    byte blue,
                    byte alpha
        )
        {
            _context->glColorMask(red, green, blue, alpha);
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
            _context->glColorMaski(index, r, g, b, a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CompileShader(
                    GlHandle shader
        )
        {
            _context->glCompileShader(shader.Value);
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
                _context->glCompressedTexImage1D(target, level, internalformat, width, border,
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
                _context->glCompressedTexImage2D(target, level, internalformat, width, height, border,
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
                _context->glCompressedTexImage3D(target, level, internalformat, width, height, depth,
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
                _context->glCompressedTexSubImage1D(target, level, xoffset, width, format, imageSize,
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
                _context->glCompressedTexSubImage2D(target, level, xoffset, yoffset, width, height,
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
                _context->glCompressedTexSubImage3D(target, level, xoffset, yoffset, zoffset, width,
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
            _context->glCopyBufferSubData(readTarget, writeTarget, readOffset, writeOffset, size);
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
            _context->glCopyTexImage1D(target, level, internalformat, x, y, width, border);
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
            _context->glCopyTexImage2D(target, level, internalformat, x, y, width, height, border);
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
            _context->glCopyTexSubImage1D(target, level, xoffset, x, y, width);
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
            _context->glCopyTexSubImage2D(target, level, xoffset, yoffset, x, y, width, height);
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
            _context->glCopyTexSubImage3D(target, level, xoffset, yoffset, zoffset, x, y, width,
                                        height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GlHandle CreateProgram()
        {
            return GlHandle.Create(_context->glCreateProgram());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint CreateShader(
                    ShaderType type
        )
        {
            return _context->glCreateShader((uint)type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CullFace(
                    uint mode
        )
        {
            _context->glCullFace(mode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteBuffers(
                    int n,
                    ReadOnlySpan<uint> buffers
        )
        {
            fixed (uint* buffersPtr = buffers) {
                _context->glDeleteBuffers(n, buffersPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteFramebuffers(
                    int n,
                    ReadOnlySpan<uint> framebuffers
        )
        {
            fixed (uint* framebuffersPtr = framebuffers) {
                _context->glDeleteFramebuffers(n, framebuffersPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteProgram(
                    GlHandle program
        )
        {
            _context->glDeleteProgram(program.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteQueries(
                    int n,
                    ReadOnlySpan<uint> ids
        )
        {
            fixed (uint* idsPtr = ids) {
                _context->glDeleteQueries(n, idsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteRenderbuffers(
                    int n,
                    ReadOnlySpan<uint> renderbuffers
        )
        {
            fixed (uint* renderbuffersPtr = renderbuffers) {
                _context->glDeleteRenderbuffers(n, renderbuffersPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteSamplers(
                    int count,
                    ReadOnlySpan<uint> samplers
        )
        {
            fixed (uint* samplersPtr = samplers) {
                _context->glDeleteSamplers(count, samplersPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteShader(
                    GlHandle shader
        )
        {
            _context->glDeleteShader(shader.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteSync(
                    glSync sync
        )
        {
            _context->glDeleteSync(sync);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteTextures(
                    int n,
                    ReadOnlySpan<uint> textures
        )
        {
            fixed (uint* texturesPtr = textures) {
                _context->glDeleteTextures(n, texturesPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteVertexArrays(
                    int n,
                    ReadOnlySpan<uint> arrays
        )
        {
            fixed (uint* arraysPtr = arrays) {
                _context->glDeleteVertexArrays(n, arraysPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DepthFunc(
                    uint func
        )
        {
            _context->glDepthFunc(func);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DepthMask(
                    byte flag
        )
        {
            _context->glDepthMask(flag);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DepthRange(
                    double n,
                    double f
        )
        {
            _context->glDepthRange(n, f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DetachShader(
                    GlHandle program,
                    GlHandle shader
        )
        {
            _context->glDetachShader(program.Value, shader.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable(
                    uint cap
        )
        {
            _context->glDisable(cap);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disablei(
                    uint target,
                    uint index
        )
        {
            _context->glDisablei(target, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableVertexAttribArray(
                    uint index
        )
        {
            _context->glDisableVertexAttribArray(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawArrays(
                    uint mode,
                    int first,
                    int count
        )
        {
            _context->glDrawArrays(mode, first, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawArraysInstanced(
                    uint mode,
                    int first,
                    int count,
                    int instancecount
        )
        {
            _context->glDrawArraysInstanced(mode, first, count, instancecount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawBuffer(
                    uint buf
        )
        {
            _context->glDrawBuffer(buf);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawBuffers(
                    int n,
                    ReadOnlySpan<uint> bufs
        )
        {
            fixed (uint* bufsPtr = bufs) {
                _context->glDrawBuffers(n, bufsPtr);
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
                _context->glDrawElements(mode, count, (uint)type, indicesPtr);
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
                _context->glDrawElementsBaseVertex(mode, count, (uint)type, indicesPtr, basevertex);
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
                _context->glDrawElementsInstanced(mode, count, (uint)type, indicesPtr, instancecount);
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
                _context->glDrawElementsInstancedBaseVertex(mode, count, (uint)type, indicesPtr,
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
                _context->glDrawRangeElements(mode, start, end, count, (uint)type, indicesPtr);
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
                _context->glDrawRangeElementsBaseVertex(mode, start, end, count, (uint)type,
                                                      indicesPtr, basevertex);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable(
                    uint cap
        )
        {
            _context->glEnable(cap);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enablei(
                    uint target,
                    uint index
        )
        {
            _context->glEnablei(target, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnableVertexAttribArray(
                    uint index
        )
        {
            _context->glEnableVertexAttribArray(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EndConditionalRender()
        {
            _context->glEndConditionalRender();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EndQuery(
                    uint target
        )
        {
            _context->glEndQuery(target);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EndTransformFeedback()
        {
            _context->glEndTransformFeedback();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public glSync FenceSync(
                      uint condition,
                      uint flags
        )
        {
            return _context->glFenceSync(condition, flags);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Finish()
        {
            _context->glFinish();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Flush()
        {
            _context->glFlush();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FlushMappedBufferRange(
                    uint target,
                    nint offset,
                    nuint length
        )
        {
            _context->glFlushMappedBufferRange(target, offset, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FramebufferRenderbuffer(
                    uint target,
                    uint attachment,
                    uint renderbuffertarget,
                    GlHandle renderbuffer
        )
        {
            _context->glFramebufferRenderbuffer(target, attachment, renderbuffertarget,
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
            _context->glFramebufferTexture(target, attachment, texture.Value, level);
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
            _context->glFramebufferTexture1D(target, attachment, textarget, texture.Value, level);
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
            _context->glFramebufferTexture2D(target, attachment, textarget, texture.Value, level);
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
            _context->glFramebufferTexture3D(target, attachment, textarget, texture.Value, level,
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
            _context->glFramebufferTextureLayer(target, attachment, texture.Value, level, layer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FrontFace(
                    uint mode
        )
        {
            _context->glFrontFace(mode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenBuffers(
                    int n,
                    uint* buffers
        )
        {
            _context->glGenBuffers(n, buffers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenerateMipmap(
                    Texture2DTarget target
        )
        {
            _context->glGenerateMipmap((uint)target);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenFrameBuffers(
                    Span<GlHandle> frameBuffers
        )
        {
            fixed (GlHandle* hPtr = frameBuffers) {
                _context->glGenFramebuffers(frameBuffers.Length, (uint*)hPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenQueries(
                    int n,
                    uint* ids
        )
        {
            _context->glGenQueries(n, ids);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenRenderbuffers(
                    int n,
                    uint* renderbuffers
        )
        {
            _context->glGenRenderbuffers(n, renderbuffers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenSamplers(
                    int count,
                    uint* samplers
        )
        {
            _context->glGenSamplers(count, samplers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenTextures(
                    int n,
                    uint* textures
        )
        {
            _context->glGenTextures(n, textures);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenVertexArrays(
                    int n,
                    uint* arrays
        )
        {
            _context->glGenVertexArrays(n, arrays);
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
            _context->glGetActiveAttrib(program.Value, index, bufSize, length, size, type, name);
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
            _context->glGetActiveUniform(program.Value, index, bufSize, length, size, type, name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetActiveUniformBlockiv(
                    GlHandle program,
                    uint uniformBlockIndex,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetActiveUniformBlockiv(program.Value, uniformBlockIndex, pname, @params);
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
            _context->glGetActiveUniformBlockName(program.Value, uniformBlockIndex, bufSize, length,
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
            _context->glGetActiveUniformName(program.Value, uniformIndex, bufSize, length,
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
                _context->glGetActiveUniformsiv(program.Value, uniformCount, uniformIndicesPtr, pname,
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
            _context->glGetAttachedShaders(program.Value, maxCount, count, shaders);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetAttribLocation(
                   GlHandle program,
                   ReadOnlySpan<byte> name
        )
        {
            fixed (byte* namePtr = name) {
                return _context->glGetAttribLocation(program.Value, namePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetBooleani_v(
                    uint target,
                    uint index,
                    byte* data
        )
        {
            _context->glGetBooleani_v(target, index, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetBooleanv(
                    uint pname,
                    byte* data
        )
        {
            _context->glGetBooleanv(pname, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetBufferParameteri64v(
                    uint target,
                    uint pname,
                    IntPtr* @params
        )
        {
            _context->glGetBufferParameteri64v(target, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetBufferParameteriv(
                    uint target,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetBufferParameteriv(target, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetBufferPointerv(
                    uint target,
                    uint pname,
                    IntPtr* @params
        )
        {
            _context->glGetBufferPointerv(target, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetBufferSubData(
                    uint target,
                    nint offset,
                    nuint size,
                    IntPtr* data
        )
        {
            _context->glGetBufferSubData(target, offset, size, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetCompressedTexImage(
                    uint target,
                    int level,
                    IntPtr* img
        )
        {
            _context->glGetCompressedTexImage(target, level, img);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetDoublev(
                    uint pname,
                    double* data
        )
        {
            _context->glGetDoublev(pname, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetError()
        {
            return _context->glGetError();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetFloatv(
                    uint pname,
                    float* data
        )
        {
            _context->glGetFloatv(pname, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetFragDataIndex(
                   GlHandle program,
                   ReadOnlySpan<byte> name
        )
        {
            fixed (byte* namePtr = name) {
                return _context->glGetFragDataIndex(program.Value, namePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetFragDataLocation(
                   GlHandle program,
                   ReadOnlySpan<byte> name
        )
        {
            fixed (byte* namePtr = name) {
                return _context->glGetFragDataLocation(program.Value, namePtr);
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
            _context->glGetFramebufferAttachmentParameteriv(target, attachment, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetInteger64i_v(
                    uint target,
                    uint index,
                    IntPtr* data
        )
        {
            _context->glGetInteger64i_v(target, index, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetInteger64v(
                    uint pname,
                    IntPtr* data
        )
        {
            _context->glGetInteger64v(pname, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetIntegeri_v(
                    uint target,
                    uint index,
                    int* data
        )
        {
            _context->glGetIntegeri_v(target, index, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetIntegerv(
                    uint pname,
                    int* data
        )
        {
            _context->glGetIntegerv(pname, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetMultisamplefv(
                    uint pname,
                    uint index,
                    float* val
        )
        {
            _context->glGetMultisamplefv(pname, index, val);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetProgramInfoLog(
                    GlHandle program,
                    int bufSize,
                    int* length,
                    byte* infoLog
        )
        {
            _context->glGetProgramInfoLog(program.Value, bufSize, length, infoLog);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetProgramiv(
                    GlHandle program,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetProgramiv(program.Value, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetQueryiv(
                    uint target,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetQueryiv(target, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetQueryObjecti64v(
                    uint id,
                    uint pname,
                    IntPtr* @params
        )
        {
            _context->glGetQueryObjecti64v(id, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetQueryObjectiv(
                    uint id,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetQueryObjectiv(id, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetQueryObjectui64v(
                    uint id,
                    uint pname,
                    IntPtr* @params
        )
        {
            _context->glGetQueryObjectui64v(id, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetQueryObjectuiv(
                    uint id,
                    uint pname,
                    uint* @params
        )
        {
            _context->glGetQueryObjectuiv(id, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetRenderbufferParameteriv(
                    uint target,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetRenderbufferParameteriv(target, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetSamplerParameterfv(
                    GlHandle sampler,
                    uint pname,
                    float* @params
        )
        {
            _context->glGetSamplerParameterfv(sampler.Value, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetSamplerParameterIiv(
                    GlHandle sampler,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetSamplerParameterIiv(sampler.Value, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetSamplerParameterIuiv(
                    GlHandle sampler,
                    uint pname,
                    uint* @params
        )
        {
            _context->glGetSamplerParameterIuiv(sampler.Value, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetSamplerParameteriv(
                    GlHandle sampler,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetSamplerParameteriv(sampler.Value, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetShaderInfoLog(
                    GlHandle shader,
                    int bufSize,
                    int* length,
                    byte* infoLog
        )
        {
            _context->glGetShaderInfoLog(shader.Value, bufSize, length, infoLog);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetShaderiv(
                    GlHandle shader,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetShaderiv(shader.Value, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetShaderSource(
                    GlHandle shader,
                    int bufSize,
                    int* length,
                    byte* source
        )
        {
            _context->glGetShaderSource(shader.Value, bufSize, length, source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetString(
                      uint name
        )
        {
            return Marshal.PtrToStringAuto((nint)_context->glGetString(name)) ?? string.Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetStringi(
                      uint name,
                      uint index
        )
        {
            return Marshal.PtrToStringAuto((nint)_context->glGetStringi(name, index)) ?? string.Empty;
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
            _context->glGetSynciv(sync, pname, count, length, values);
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
            _context->glGetTexImage(target, level, format, (uint)type, pixels);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTexLevelParameterfv(
                    uint target,
                    int level,
                    uint pname,
                    float* @params
        )
        {
            _context->glGetTexLevelParameterfv(target, level, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTexLevelParameteriv(
                    uint target,
                    int level,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetTexLevelParameteriv(target, level, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTexParameterfv(
                    uint target,
                    uint pname,
                    float* @params
        )
        {
            _context->glGetTexParameterfv(target, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTexParameterIiv(
                    uint target,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetTexParameterIiv(target, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTexParameterIuiv(
                    uint target,
                    uint pname,
                    uint* @params
        )
        {
            _context->glGetTexParameterIuiv(target, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTexParameteriv(
                    uint target,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetTexParameteriv(target, pname, @params);
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
            _context->glGetTransformFeedbackVarying(program.Value, index, bufSize, length, size, type,
                                                  name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetUniformBlockIndex(
                    GlHandle program,
                    ReadOnlySpan<byte> uniformBlockName
        )
        {
            fixed (byte* uniformBlockNamePtr = uniformBlockName) {
                return _context->glGetUniformBlockIndex(program.Value, uniformBlockNamePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetUniformfv(
                    GlHandle program,
                    int location,
                    float* @params
        )
        {
            _context->glGetUniformfv(program.Value, location, @params);
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
                _context->glGetUniformIndices(program.Value, uniformCount, uniformNamesPtr,
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
            _context->glGetUniformiv(program.Value, location, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUniformLocation(
                   GlHandle program,
                   ReadOnlySpan<byte> name
        )
        {
            fixed (byte* namePtr = name) {
                return _context->glGetUniformLocation(program.Value, namePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetUniformuiv(
                    GlHandle program,
                    int location,
                    uint* @params
        )
        {
            _context->glGetUniformuiv(program.Value, location, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetVertexAttribdv(
                    uint index,
                    uint pname,
                    double* @params
        )
        {
            _context->glGetVertexAttribdv(index, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetVertexAttribfv(
                    uint index,
                    uint pname,
                    float* @params
        )
        {
            _context->glGetVertexAttribfv(index, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetVertexAttribIiv(
                    uint index,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetVertexAttribIiv(index, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetVertexAttribIuiv(
                    uint index,
                    uint pname,
                    uint* @params
        )
        {
            _context->glGetVertexAttribIuiv(index, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetVertexAttribiv(
                    uint index,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetVertexAttribiv(index, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetVertexAttribPointerv(
                    uint index,
                    uint pname,
                    IntPtr* pointer
        )
        {
            _context->glGetVertexAttribPointerv(index, pname, pointer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Hint(
                    uint target,
                    uint mode
        )
        {
            _context->glHint(target, mode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsBuffer(
                    GlHandle buffer
        )
        {
            return _context->glIsBuffer(buffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsEnabled(
                    uint cap
        )
        {
            return _context->glIsEnabled(cap);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsEnabledi(
                    uint target,
                    uint index
        )
        {
            return _context->glIsEnabledi(target, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsFramebuffer(
                    GlHandle framebuffer
        )
        {
            return _context->glIsFramebuffer(framebuffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsProgram(
                    GlHandle program
        )
        {
            return _context->glIsProgram(program.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsQuery(
                    uint id
        )
        {
            return _context->glIsQuery(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsRenderbuffer(
                    GlHandle renderbuffer
        )
        {
            return _context->glIsRenderbuffer(renderbuffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsSampler(
                    GlHandle sampler
        )
        {
            return _context->glIsSampler(sampler.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsShader(
                    GlHandle shader
        )
        {
            return _context->glIsShader(shader.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsSync(
                    glSync sync
        )
        {
            return _context->glIsSync(sync);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsTexture(
                    GlHandle texture
        )
        {
            return _context->glIsTexture(texture.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte IsVertexArray(
                    uint array
        )
        {
            return _context->glIsVertexArray(array);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LineWidth(
                    float width
        )
        {
            _context->glLineWidth(width);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LinkProgram(
                    GlHandle program
        )
        {
            _context->glLinkProgram(program.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogicOp(
                    uint opcode
        )
        {
            _context->glLogicOp(opcode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MapBuffer(
                    uint target,
                    uint access
        )
        {
            _context->glMapBuffer(target, access);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MapBufferRange(
                    uint target,
                    nint offset,
                    nuint length,
                    uint access
        )
        {
            _context->glMapBufferRange(target, offset, length, access);
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
                    _context->glMultiDrawArrays(mode, firstPtr, countPtr, drawcount);
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
                    _context->glMultiDrawElements(mode, countPtr, (uint)type, indicesPtr, drawcount);
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
                        _context->glMultiDrawElementsBaseVertex(mode, countPtr, (uint)type,
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
            _context->glPixelStoref(pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PixelStorei(
                    uint pname,
                    int param
        )
        {
            _context->glPixelStorei(pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PointParameterf(
                    uint pname,
                    float param
        )
        {
            _context->glPointParameterf(pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PointParameterfv(
                    uint pname,
                    ReadOnlySpan<float> @params
        )
        {
            fixed (float* @paramsPtr = @params) {
                _context->glPointParameterfv(pname, @paramsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PointParameteri(
                    uint pname,
                    int param
        )
        {
            _context->glPointParameteri(pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PointParameteriv(
                    uint pname,
                    ReadOnlySpan<int> @params
        )
        {
            fixed (int* @paramsPtr = @params) {
                _context->glPointParameteriv(pname, @paramsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PointSize(
                    float size
        )
        {
            _context->glPointSize(size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PolygonMode(
                    uint face,
                    uint mode
        )
        {
            _context->glPolygonMode(face, mode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PolygonOffset(
                    float factor,
                    float units
        )
        {
            _context->glPolygonOffset(factor, units);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PrimitiveRestartIndex(
                    uint index
        )
        {
            _context->glPrimitiveRestartIndex(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProvokingVertex(
                    uint mode
        )
        {
            _context->glProvokingVertex(mode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void QueryCounter(
                    uint id,
                    uint target
        )
        {
            _context->glQueryCounter(id, target);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadBuffer(
                    uint src
        )
        {
            _context->glReadBuffer(src);
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
            _context->glReadPixels(x, y, width, height, format, (uint)type, pixels);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RenderbufferStorage(
                    uint target,
                    uint internalformat,
                    int width,
                    int height
        )
        {
            _context->glRenderbufferStorage(target, internalformat, width, height);
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
            _context->glRenderbufferStorageMultisample(target, samples, internalformat, width,
                                                     height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SampleCoverage(
                    float value,
                    byte invert
        )
        {
            _context->glSampleCoverage(value, invert);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SampleMaski(
                    uint maskNumber,
                    uint mask
        )
        {
            _context->glSampleMaski(maskNumber, mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SamplerParameterf(
                    GlHandle sampler,
                    uint pname,
                    float param
        )
        {
            _context->glSamplerParameterf(sampler.Value, pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SamplerParameterfv(
                    GlHandle sampler,
                    uint pname,
                    ReadOnlySpan<float> param
        )
        {
            fixed (float* paramPtr = param) {
                _context->glSamplerParameterfv(sampler.Value, pname, paramPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SamplerParameteri(
                    GlHandle sampler,
                    uint pname,
                    int param
        )
        {
            _context->glSamplerParameteri(sampler.Value, pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SamplerParameterIiv(
                    GlHandle sampler,
                    uint pname,
                    ReadOnlySpan<int> param
        )
        {
            fixed (int* paramPtr = param) {
                _context->glSamplerParameterIiv(sampler.Value, pname, paramPtr);
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
                _context->glSamplerParameterIuiv(sampler.Value, pname, paramPtr);
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
                _context->glSamplerParameteriv(sampler.Value, pname, paramPtr);
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
            _context->glScissor(x, y, width, height);
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
                    _context->glShaderSource(shader.Value, count, &sPtr, lengthPtr);
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
            _context->glStencilFunc(func, @ref, mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StencilFuncSeparate(
                    uint face,
                    uint func,
                    int @ref,
                    uint mask
        )
        {
            _context->glStencilFuncSeparate(face, func, @ref, mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StencilMask(
                    uint mask
        )
        {
            _context->glStencilMask(mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StencilMaskSeparate(
                    uint face,
                    uint mask
        )
        {
            _context->glStencilMaskSeparate(face, mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StencilOp(
                    uint fail,
                    uint zfail,
                    uint zpass
        )
        {
            _context->glStencilOp(fail, zfail, zpass);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StencilOpSeparate(
                    uint face,
                    uint sfail,
                    uint dpfail,
                    uint dppass
        )
        {
            _context->glStencilOpSeparate(face, sfail, dpfail, dppass);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexBuffer(
                    uint target,
                    uint internalformat,
                    GlHandle buffer
        )
        {
            _context->glTexBuffer(target, internalformat, buffer.Value);
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
                _context->glTexImage1D(target, level, internalformat, width, border, format,
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
                _context->glTexImage2D(target, level, internalformat, width, height, border, format,
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
            _context->glTexImage2DMultisample(target, samples, internalformat, width, height,
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
                _context->glTexImage3D(target, level, internalformat, width, height, depth, border,
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
            _context->glTexImage3DMultisample(target, samples, internalformat, width, height, depth,
                                            fixedsamplelocations);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexParameterf(
                    uint target,
                    uint pname,
                    float param
        )
        {
            _context->glTexParameterf(target, pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexParameterfv(
                    uint target,
                    uint pname,
                    ReadOnlySpan<float> @params
        )
        {
            fixed (float* @paramsPtr = @params) {
                _context->glTexParameterfv(target, pname, @paramsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexParameteri(
                    uint target,
                    uint pname,
                    int param
        )
        {
            _context->glTexParameteri(target, pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexParameterIiv(
                    uint target,
                    uint pname,
                    ReadOnlySpan<int> @params
        )
        {
            fixed (int* @paramsPtr = @params) {
                _context->glTexParameterIiv(target, pname, @paramsPtr);
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
                _context->glTexParameterIuiv(target, pname, @paramsPtr);
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
                _context->glTexParameteriv(target, pname, @paramsPtr);
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
                _context->glTexSubImage1D(target, level, xoffset, width, format, (uint)type,
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
                _context->glTexSubImage2D(target, level, xoffset, yoffset, width, height, format,
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
                _context->glTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height,
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
                _context->glTransformFeedbackVaryings(program.Value, count, varyingsPtr, bufferMode);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform1f(
                    int location,
                    float v0
        )
        {
            _context->glUniform1f(location, v0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform1fv(
                    int location,
                    int count,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                _context->glUniform1fv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform1i(
                    int location,
                    int v0
        )
        {
            _context->glUniform1i(location, v0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform1iv(
                    int location,
                    int count,
                    ReadOnlySpan<int> value
        )
        {
            fixed (int* valuePtr = value) {
                _context->glUniform1iv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform1ui(
                    int location,
                    uint v0
        )
        {
            _context->glUniform1ui(location, v0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform1uiv(
                    int location,
                    int count,
                    ReadOnlySpan<uint> value
        )
        {
            fixed (uint* valuePtr = value) {
                _context->glUniform1uiv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform2f(
                    int location,
                    float v0,
                    float v1
        )
        {
            _context->glUniform2f(location, v0, v1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform2fv(
                    int location,
                    int count,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                _context->glUniform2fv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform2i(
                    int location,
                    int v0,
                    int v1
        )
        {
            _context->glUniform2i(location, v0, v1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform2iv(
                    int location,
                    int count,
                    ReadOnlySpan<int> value
        )
        {
            fixed (int* valuePtr = value) {
                _context->glUniform2iv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform2ui(
                    int location,
                    uint v0,
                    uint v1
        )
        {
            _context->glUniform2ui(location, v0, v1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform2uiv(
                    int location,
                    int count,
                    ReadOnlySpan<uint> value
        )
        {
            fixed (uint* valuePtr = value) {
                _context->glUniform2uiv(location, count, valuePtr);
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
            _context->glUniform3f(location, v0, v1, v2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform3fv(
                    int location,
                    int count,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                _context->glUniform3fv(location, count, valuePtr);
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
            _context->glUniform3i(location, v0, v1, v2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform3iv(
                    int location,
                    int count,
                    ReadOnlySpan<int> value
        )
        {
            fixed (int* valuePtr = value) {
                _context->glUniform3iv(location, count, valuePtr);
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
            _context->glUniform3ui(location, v0, v1, v2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform3uiv(
                    int location,
                    int count,
                    ReadOnlySpan<uint> value
        )
        {
            fixed (uint* valuePtr = value) {
                _context->glUniform3uiv(location, count, valuePtr);
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
            _context->glUniform4f(location, v0, v1, v2, v3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform4fv(
                    int location,
                    int count,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                _context->glUniform4fv(location, count, valuePtr);
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
            _context->glUniform4i(location, v0, v1, v2, v3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform4iv(
                    int location,
                    int count,
                    ReadOnlySpan<int> value
        )
        {
            fixed (int* valuePtr = value) {
                _context->glUniform4iv(location, count, valuePtr);
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
            _context->glUniform4ui(location, v0, v1, v2, v3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Uniform4uiv(
                    int location,
                    int count,
                    ReadOnlySpan<uint> value
        )
        {
            fixed (uint* valuePtr = value) {
                _context->glUniform4uiv(location, count, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UniformBlockBinding(
                    GlHandle program,
                    uint uniformBlockIndex,
                    uint uniformBlockBinding
        )
        {
            _context->glUniformBlockBinding(program.Value, uniformBlockIndex, uniformBlockBinding);
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
                _context->glUniformMatrix2fv(location, count, transpose, valuePtr);
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
                _context->glUniformMatrix2x3fv(location, count, transpose, valuePtr);
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
                _context->glUniformMatrix2x4fv(location, count, transpose, valuePtr);
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
                _context->glUniformMatrix3fv(location, count, transpose, valuePtr);
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
                _context->glUniformMatrix3x2fv(location, count, transpose, valuePtr);
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
                _context->glUniformMatrix3x4fv(location, count, transpose, valuePtr);
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
                _context->glUniformMatrix4fv(location, count, transpose, valuePtr);
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
                _context->glUniformMatrix4x2fv(location, count, transpose, valuePtr);
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
                _context->glUniformMatrix4x3fv(location, count, transpose, valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte UnmapBuffer(
                    uint target
        )
        {
            return _context->glUnmapBuffer(target);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UseProgram(
                    GlHandle program
        )
        {
            _context->glUseProgram(program.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ValidateProgram(
                    GlHandle program
        )
        {
            _context->glValidateProgram(program.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib1d(
                    uint index,
                    double x
        )
        {
            _context->glVertexAttrib1d(index, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib1dv(
                    uint index,
                    ReadOnlySpan<double> v
        )
        {
            fixed (double* vPtr = v) {
                _context->glVertexAttrib1dv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib1f(
                    uint index,
                    float x
        )
        {
            _context->glVertexAttrib1f(index, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib1fv(
                    uint index,
                    ReadOnlySpan<float> v
        )
        {
            fixed (float* vPtr = v) {
                _context->glVertexAttrib1fv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib1s(
                    uint index,
                    short x
        )
        {
            _context->glVertexAttrib1s(index, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib1sv(
                    uint index,
                    ReadOnlySpan<short> v
        )
        {
            fixed (short* vPtr = v) {
                _context->glVertexAttrib1sv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib2d(
                    uint index,
                    double x,
                    double y
        )
        {
            _context->glVertexAttrib2d(index, x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib2dv(
                    uint index,
                    ReadOnlySpan<double> v
        )
        {
            fixed (double* vPtr = v) {
                _context->glVertexAttrib2dv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib2f(
                    uint index,
                    float x,
                    float y
        )
        {
            _context->glVertexAttrib2f(index, x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib2fv(
                    uint index,
                    ReadOnlySpan<float> v
        )
        {
            fixed (float* vPtr = v) {
                _context->glVertexAttrib2fv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib2s(
                    uint index,
                    short x,
                    short y
        )
        {
            _context->glVertexAttrib2s(index, x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib2sv(
                    uint index,
                    ReadOnlySpan<short> v
        )
        {
            fixed (short* vPtr = v) {
                _context->glVertexAttrib2sv(index, vPtr);
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
            _context->glVertexAttrib3d(index, x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib3dv(
                    uint index,
                    ReadOnlySpan<double> v
        )
        {
            fixed (double* vPtr = v) {
                _context->glVertexAttrib3dv(index, vPtr);
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
            _context->glVertexAttrib3f(index, x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib3fv(
                    uint index,
                    ReadOnlySpan<float> v
        )
        {
            fixed (float* vPtr = v) {
                _context->glVertexAttrib3fv(index, vPtr);
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
            _context->glVertexAttrib3s(index, x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib3sv(
                    uint index,
                    ReadOnlySpan<short> v
        )
        {
            fixed (short* vPtr = v) {
                _context->glVertexAttrib3sv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4bv(
                    uint index,
                    ReadOnlySpan<sbyte> v
        )
        {
            fixed (sbyte* vPtr = v) {
                _context->glVertexAttrib4bv(index, vPtr);
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
            _context->glVertexAttrib4d(index, x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4dv(
                    uint index,
                    ReadOnlySpan<double> v
        )
        {
            fixed (double* vPtr = v) {
                _context->glVertexAttrib4dv(index, vPtr);
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
            _context->glVertexAttrib4f(index, x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4fv(
                    uint index,
                    ReadOnlySpan<float> v
        )
        {
            fixed (float* vPtr = v) {
                _context->glVertexAttrib4fv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4iv(
                    uint index,
                    ReadOnlySpan<int> v
        )
        {
            fixed (int* vPtr = v) {
                _context->glVertexAttrib4iv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4Nbv(
                    uint index,
                    ReadOnlySpan<sbyte> v
        )
        {
            fixed (sbyte* vPtr = v) {
                _context->glVertexAttrib4Nbv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4Niv(
                    uint index,
                    ReadOnlySpan<int> v
        )
        {
            fixed (int* vPtr = v) {
                _context->glVertexAttrib4Niv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4Nsv(
                    uint index,
                    ReadOnlySpan<short> v
        )
        {
            fixed (short* vPtr = v) {
                _context->glVertexAttrib4Nsv(index, vPtr);
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
            _context->glVertexAttrib4Nub(index, x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4Nubv(
                    uint index,
                    ReadOnlySpan<byte> v
        )
        {
            fixed (byte* vPtr = v) {
                _context->glVertexAttrib4Nubv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4Nuiv(
                    uint index,
                    ReadOnlySpan<uint> v
        )
        {
            fixed (uint* vPtr = v) {
                _context->glVertexAttrib4Nuiv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4Nusv(
                    uint index,
                    ReadOnlySpan<ushort> v
        )
        {
            fixed (ushort* vPtr = v) {
                _context->glVertexAttrib4Nusv(index, vPtr);
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
            _context->glVertexAttrib4s(index, x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4sv(
                    uint index,
                    ReadOnlySpan<short> v
        )
        {
            fixed (short* vPtr = v) {
                _context->glVertexAttrib4sv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4ubv(
                    uint index,
                    ReadOnlySpan<byte> v
        )
        {
            fixed (byte* vPtr = v) {
                _context->glVertexAttrib4ubv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4uiv(
                    uint index,
                    ReadOnlySpan<uint> v
        )
        {
            fixed (uint* vPtr = v) {
                _context->glVertexAttrib4uiv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttrib4usv(
                    uint index,
                    ReadOnlySpan<ushort> v
        )
        {
            fixed (ushort* vPtr = v) {
                _context->glVertexAttrib4usv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribDivisor(
                    uint index,
                    uint divisor
        )
        {
            _context->glVertexAttribDivisor(index, divisor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI1i(
                    uint index,
                    int x
        )
        {
            _context->glVertexAttribI1i(index, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI1iv(
                    uint index,
                    ReadOnlySpan<int> v
        )
        {
            fixed (int* vPtr = v) {
                _context->glVertexAttribI1iv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI1ui(
                    uint index,
                    uint x
        )
        {
            _context->glVertexAttribI1ui(index, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI1uiv(
                    uint index,
                    ReadOnlySpan<uint> v
        )
        {
            fixed (uint* vPtr = v) {
                _context->glVertexAttribI1uiv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI2i(
                    uint index,
                    int x,
                    int y
        )
        {
            _context->glVertexAttribI2i(index, x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI2iv(
                    uint index,
                    ReadOnlySpan<int> v
        )
        {
            fixed (int* vPtr = v) {
                _context->glVertexAttribI2iv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI2ui(
                    uint index,
                    uint x,
                    uint y
        )
        {
            _context->glVertexAttribI2ui(index, x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI2uiv(
                    uint index,
                    ReadOnlySpan<uint> v
        )
        {
            fixed (uint* vPtr = v) {
                _context->glVertexAttribI2uiv(index, vPtr);
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
            _context->glVertexAttribI3i(index, x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI3iv(
                    uint index,
                    ReadOnlySpan<int> v
        )
        {
            fixed (int* vPtr = v) {
                _context->glVertexAttribI3iv(index, vPtr);
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
            _context->glVertexAttribI3ui(index, x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI3uiv(
                    uint index,
                    ReadOnlySpan<uint> v
        )
        {
            fixed (uint* vPtr = v) {
                _context->glVertexAttribI3uiv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI4bv(
                    uint index,
                    ReadOnlySpan<sbyte> v
        )
        {
            fixed (sbyte* vPtr = v) {
                _context->glVertexAttribI4bv(index, vPtr);
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
            _context->glVertexAttribI4i(index, x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI4iv(
                    uint index,
                    ReadOnlySpan<int> v
        )
        {
            fixed (int* vPtr = v) {
                _context->glVertexAttribI4iv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI4sv(
                    uint index,
                    ReadOnlySpan<short> v
        )
        {
            fixed (short* vPtr = v) {
                _context->glVertexAttribI4sv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI4ubv(
                    uint index,
                    ReadOnlySpan<byte> v
        )
        {
            fixed (byte* vPtr = v) {
                _context->glVertexAttribI4ubv(index, vPtr);
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
            _context->glVertexAttribI4ui(index, x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI4uiv(
                    uint index,
                    ReadOnlySpan<uint> v
        )
        {
            fixed (uint* vPtr = v) {
                _context->glVertexAttribI4uiv(index, vPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribI4usv(
                    uint index,
                    ReadOnlySpan<ushort> v
        )
        {
            fixed (ushort* vPtr = v) {
                _context->glVertexAttribI4usv(index, vPtr);
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
                _context->glVertexAttribIPointer(index, size, (uint)type, stride, pointerPtr);
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
            _context->glVertexAttribP1ui(index, (uint)type, normalized, value);
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
                _context->glVertexAttribP1uiv(index, (uint)type, normalized, valuePtr);
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
            _context->glVertexAttribP2ui(index, (uint)type, normalized, value);
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
                _context->glVertexAttribP2uiv(index, (uint)type, normalized, valuePtr);
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
            _context->glVertexAttribP3ui(index, (uint)type, normalized, value);
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
                _context->glVertexAttribP3uiv(index, (uint)type, normalized, valuePtr);
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
            _context->glVertexAttribP4ui(index, (uint)type, normalized, value);
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
                _context->glVertexAttribP4uiv(index, (uint)type, normalized, valuePtr);
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
                _context->glVertexAttribPointer(index, size, (uint)type, normalized, stride,
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
            _context->glViewport(x, y, width, height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WaitSync(
                    glSync sync,
                    uint flags,
                    IntPtr timeout
        )
        {
            _context->glWaitSync(sync, flags, timeout);
        }

    }
}
