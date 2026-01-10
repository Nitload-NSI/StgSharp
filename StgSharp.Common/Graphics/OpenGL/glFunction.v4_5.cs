//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="glFunction.v4_5"
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

namespace StgSharp.Graphics.OpenGL
{
    public unsafe partial class OpenGLFunction
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindTextureUnit(
                    uint unit,
                    GlHandle texture
        )
        {
            _context->glBindTextureUnit(unit, texture.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BlitNamedFramebuffer(
                    GlHandle readFramebuffer,
                    GlHandle drawFramebuffer,
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
            _context->glBlitNamedFramebuffer(readFramebuffer.Value, drawFramebuffer.Value, srcX0,
                                             srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask,
                                             filter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint CheckNamedFramebufferStatus(
                    GlHandle framebuffer,
                    uint target
        )
        {
            return _context->glCheckNamedFramebufferStatus(framebuffer.Value, target);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearNamedBufferData(
                    GlHandle buffer,
                    uint internalformat,
                    uint format,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> data
        )
        {
            fixed (IntPtr* dataPtr = data) {
                _context->glClearNamedBufferData(buffer.Value, internalformat, format, (uint)type,
                                                 dataPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearNamedBufferSubData(
                    GlHandle buffer,
                    uint internalformat,
                    nint offset,
                    nuint size,
                    uint format,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> data
        )
        {
            fixed (IntPtr* dataPtr = data) {
                _context->glClearNamedBufferSubData(buffer.Value, internalformat, offset, size,
                                                    format, (uint)type, dataPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearNamedFramebufferfi(
                    GlHandle framebuffer,
                    uint buffer,
                    int drawbuffer,
                    float depth,
                    int stencil
        )
        {
            _context->glClearNamedFramebufferfi(framebuffer.Value, buffer, drawbuffer, depth,
                                                stencil);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearNamedFramebufferfv(
                    GlHandle framebuffer,
                    uint buffer,
                    int drawbuffer,
                    ReadOnlySpan<float> value
        )
        {
            fixed (float* valuePtr = value) {
                _context->glClearNamedFramebufferfv(framebuffer.Value, buffer, drawbuffer,
                                                    valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearNamedFramebufferiv(
                    GlHandle framebuffer,
                    uint buffer,
                    int drawbuffer,
                    ReadOnlySpan<int> value
        )
        {
            fixed (int* valuePtr = value) {
                _context->glClearNamedFramebufferiv(framebuffer.Value, buffer, drawbuffer,
                                                    valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearNamedFramebufferuiv(
                    GlHandle framebuffer,
                    uint buffer,
                    int drawbuffer,
                    ReadOnlySpan<uint> value
        )
        {
            fixed (uint* valuePtr = value) {
                _context->glClearNamedFramebufferuiv(framebuffer.Value, buffer, drawbuffer,
                                                     valuePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClipControl(
                    uint origin,
                    uint depth
        )
        {
            _context->glClipControl(origin, depth);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CompressedTextureSubImage1D(
                    GlHandle texture,
                    int level,
                    int xoffset,
                    int width,
                    uint format,
                    int imageSize,
                    ReadOnlySpan<IntPtr> data
        )
        {
            fixed (IntPtr* dataPtr = data) {
                _context->glCompressedTextureSubImage1D(texture.Value, level, xoffset, width,
                                                        format, imageSize, dataPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CompressedTextureSubImage2D(
                    GlHandle texture,
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
                _context->glCompressedTextureSubImage2D(texture.Value, level, xoffset, yoffset,
                                                        width, height, format, imageSize, dataPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CompressedTextureSubImage3D(
                    GlHandle texture,
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
                _context->glCompressedTextureSubImage3D(texture.Value, level, xoffset, yoffset,
                                                        zoffset, width, height, depth, format,
                                                        imageSize, dataPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyNamedBufferSubData(
                    GlHandle readBuffer,
                    GlHandle writeBuffer,
                    nint readOffset,
                    nint writeOffset,
                    nuint size
        )
        {
            _context->glCopyNamedBufferSubData(readBuffer.Value, writeBuffer.Value, readOffset,
                                               writeOffset, size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTextureSubImage1D(
                    GlHandle texture,
                    int level,
                    int xoffset,
                    int x,
                    int y,
                    int width
        )
        {
            _context->glCopyTextureSubImage1D(texture.Value, level, xoffset, x, y, width);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTextureSubImage2D(
                    GlHandle texture,
                    int level,
                    int xoffset,
                    int yoffset,
                    int x,
                    int y,
                    int width,
                    int height
        )
        {
            _context->glCopyTextureSubImage2D(texture.Value, level, xoffset, yoffset, x, y, width,
                                              height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTextureSubImage3D(
                    GlHandle texture,
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
            _context->glCopyTextureSubImage3D(texture.Value, level, xoffset, yoffset, zoffset, x, y,
                                              width, height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CreateBuffers(
                    int n,
                    uint* buffers
        )
        {
            _context->glCreateBuffers(n, buffers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CreateFramebuffers(
                    int n,
                    uint* framebuffers
        )
        {
            _context->glCreateFramebuffers(n, framebuffers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CreateProgramPipelines(
                    int n,
                    uint* pipelines
        )
        {
            _context->glCreateProgramPipelines(n, pipelines);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CreateQueries(
                    uint target,
                    int n,
                    uint* ids
        )
        {
            _context->glCreateQueries(target, n, ids);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CreateRenderbuffers(
                    int n,
                    uint* renderbuffers
        )
        {
            _context->glCreateRenderbuffers(n, renderbuffers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CreateSamplers(
                    int n,
                    uint* samplers
        )
        {
            _context->glCreateSamplers(n, samplers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CreateTextures(
                    uint target,
                    int n,
                    uint* textures
        )
        {
            _context->glCreateTextures(target, n, textures);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CreateTransformFeedbacks(
                    int n,
                    uint* ids
        )
        {
            _context->glCreateTransformFeedbacks(n, ids);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CreateVertexArrays(
                    int n,
                    uint* arrays
        )
        {
            _context->glCreateVertexArrays(n, arrays);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableVertexArrayAttrib(
                    uint vaobj,
                    uint index
        )
        {
            _context->glDisableVertexArrayAttrib(vaobj, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnableVertexArrayAttrib(
                    uint vaobj,
                    uint index
        )
        {
            _context->glEnableVertexArrayAttrib(vaobj, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FlushMappedNamedBufferRange(
                    GlHandle buffer,
                    nint offset,
                    nuint length
        )
        {
            _context->glFlushMappedNamedBufferRange(buffer.Value, offset, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenerateTextureMipmap(
                    GlHandle texture
        )
        {
            _context->glGenerateTextureMipmap(texture.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetCompressedTextureImage(
                    GlHandle texture,
                    int level,
                    int bufSize,
                    IntPtr* pixels
        )
        {
            _context->glGetCompressedTextureImage(texture.Value, level, bufSize, pixels);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetCompressedTextureSubImage(
                    GlHandle texture,
                    int level,
                    int xoffset,
                    int yoffset,
                    int zoffset,
                    int width,
                    int height,
                    int depth,
                    int bufSize,
                    IntPtr* pixels
        )
        {
            _context->glGetCompressedTextureSubImage(texture.Value, level, xoffset, yoffset,
                                                     zoffset, width, height, depth, bufSize,
                                                     pixels);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetGraphicsResetStatus()
        {
            return _context->glGetGraphicsResetStatus();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetNamedBufferParameteri64v(
                    GlHandle buffer,
                    uint pname,
                    IntPtr* @params
        )
        {
            _context->glGetNamedBufferParameteri64v(buffer.Value, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetNamedBufferParameteriv(
                    GlHandle buffer,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetNamedBufferParameteriv(buffer.Value, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetNamedBufferPointerv(
                    GlHandle buffer,
                    uint pname,
                    IntPtr* @params
        )
        {
            _context->glGetNamedBufferPointerv(buffer.Value, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetNamedBufferSubData(
                    GlHandle buffer,
                    nint offset,
                    nuint size,
                    IntPtr* data
        )
        {
            _context->glGetNamedBufferSubData(buffer.Value, offset, size, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetNamedFramebufferAttachmentParameteriv(
                    GlHandle framebuffer,
                    uint attachment,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetNamedFramebufferAttachmentParameteriv(framebuffer.Value, attachment,
                                                                 pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetNamedFramebufferParameteriv(
                    GlHandle framebuffer,
                    uint pname,
                    int* param
        )
        {
            _context->glGetNamedFramebufferParameteriv(framebuffer.Value, pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetNamedRenderbufferParameteriv(
                    GlHandle renderbuffer,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetNamedRenderbufferParameteriv(renderbuffer.Value, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetnCompressedTexImage(
                    uint target,
                    int lod,
                    int bufSize,
                    IntPtr* pixels
        )
        {
            _context->glGetnCompressedTexImage(target, lod, bufSize, pixels);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetnTexImage(
                    uint target,
                    int level,
                    uint format,
                    ShaderType type,
                    int bufSize,
                    IntPtr* pixels
        )
        {
            _context->glGetnTexImage(target, level, format, (uint)type, bufSize, pixels);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetnUniformdv(
                    GlHandle program,
                    int location,
                    int bufSize,
                    double* @params
        )
        {
            _context->glGetnUniformdv(program.Value, location, bufSize, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetnUniformfv(
                    GlHandle program,
                    int location,
                    int bufSize,
                    float* @params
        )
        {
            _context->glGetnUniformfv(program.Value, location, bufSize, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetnUniformiv(
                    GlHandle program,
                    int location,
                    int bufSize,
                    int* @params
        )
        {
            _context->glGetnUniformiv(program.Value, location, bufSize, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetnUniformuiv(
                    GlHandle program,
                    int location,
                    int bufSize,
                    uint* @params
        )
        {
            _context->glGetnUniformuiv(program.Value, location, bufSize, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetQueryBufferObjecti64v(
                    uint id,
                    GlHandle buffer,
                    uint pname,
                    nint offset
        )
        {
            _context->glGetQueryBufferObjecti64v(id, buffer.Value, pname, offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetQueryBufferObjectiv(
                    uint id,
                    GlHandle buffer,
                    uint pname,
                    nint offset
        )
        {
            _context->glGetQueryBufferObjectiv(id, buffer.Value, pname, offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetQueryBufferObjectui64v(
                    uint id,
                    GlHandle buffer,
                    uint pname,
                    nint offset
        )
        {
            _context->glGetQueryBufferObjectui64v(id, buffer.Value, pname, offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetQueryBufferObjectuiv(
                    uint id,
                    GlHandle buffer,
                    uint pname,
                    nint offset
        )
        {
            _context->glGetQueryBufferObjectuiv(id, buffer.Value, pname, offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTextureImage(
                    GlHandle texture,
                    int level,
                    uint format,
                    ShaderType type,
                    int bufSize,
                    IntPtr* pixels
        )
        {
            _context->glGetTextureImage(texture.Value, level, format, (uint)type, bufSize, pixels);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTextureLevelParameterfv(
                    GlHandle texture,
                    int level,
                    uint pname,
                    float* @params
        )
        {
            _context->glGetTextureLevelParameterfv(texture.Value, level, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTextureLevelParameteriv(
                    GlHandle texture,
                    int level,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetTextureLevelParameteriv(texture.Value, level, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTextureParameterfv(
                    GlHandle texture,
                    uint pname,
                    float* @params
        )
        {
            _context->glGetTextureParameterfv(texture.Value, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTextureParameterIiv(
                    GlHandle texture,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetTextureParameterIiv(texture.Value, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTextureParameterIuiv(
                    GlHandle texture,
                    uint pname,
                    uint* @params
        )
        {
            _context->glGetTextureParameterIuiv(texture.Value, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTextureParameteriv(
                    GlHandle texture,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetTextureParameteriv(texture.Value, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTextureSubImage(
                    GlHandle texture,
                    int level,
                    int xoffset,
                    int yoffset,
                    int zoffset,
                    int width,
                    int height,
                    int depth,
                    uint format,
                    ShaderType type,
                    int bufSize,
                    IntPtr* pixels
        )
        {
            _context->glGetTextureSubImage(texture.Value, level, xoffset, yoffset, zoffset, width,
                                           height, depth, format, (uint)type, bufSize, pixels);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTransformFeedbacki_v(
                    uint xfb,
                    uint pname,
                    uint index,
                    int* param
        )
        {
            _context->glGetTransformFeedbacki_v(xfb, pname, index, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTransformFeedbacki64_v(
                    uint xfb,
                    uint pname,
                    uint index,
                    IntPtr* param
        )
        {
            _context->glGetTransformFeedbacki64_v(xfb, pname, index, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetTransformFeedbackiv(
                    uint xfb,
                    uint pname,
                    int* param
        )
        {
            _context->glGetTransformFeedbackiv(xfb, pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetVertexArrayIndexed64iv(
                    uint vaobj,
                    uint index,
                    uint pname,
                    IntPtr* param
        )
        {
            _context->glGetVertexArrayIndexed64iv(vaobj, index, pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetVertexArrayIndexediv(
                    uint vaobj,
                    uint index,
                    uint pname,
                    int* param
        )
        {
            _context->glGetVertexArrayIndexediv(vaobj, index, pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetVertexArrayiv(
                    uint vaobj,
                    uint pname,
                    int* param
        )
        {
            _context->glGetVertexArrayiv(vaobj, pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InvalidateNamedFramebufferData(
                    GlHandle framebuffer,
                    int numAttachments,
                    ReadOnlySpan<uint> attachments
        )
        {
            fixed (uint* attachmentsPtr = attachments) {
                _context->glInvalidateNamedFramebufferData(framebuffer.Value, numAttachments, attachmentsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InvalidateNamedFramebufferSubData(
                    GlHandle framebuffer,
                    int numAttachments,
                    ReadOnlySpan<uint> attachments,
                    int x,
                    int y,
                    int width,
                    int height
        )
        {
            fixed (uint* attachmentsPtr = attachments) {
                _context->glInvalidateNamedFramebufferSubData(framebuffer.Value, numAttachments,
                                                              attachmentsPtr, x, y, width, height);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MapNamedBuffer(
                    GlHandle buffer,
                    uint access
        )
        {
            _context->glMapNamedBuffer(buffer.Value, access);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MapNamedBufferRange(
                    GlHandle buffer,
                    nint offset,
                    nuint length,
                    uint access
        )
        {
            _context->glMapNamedBufferRange(buffer.Value, offset, length, access);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MemoryBarrierByRegion(
                    uint barriers
        )
        {
            _context->glMemoryBarrierByRegion(barriers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void NamedBufferData(
                    GlHandle buffer,
                    nuint size,
                    ReadOnlySpan<IntPtr> data,
                    uint usage
        )
        {
            fixed (IntPtr* dataPtr = data) {
                _context->glNamedBufferData(buffer.Value, size, dataPtr, usage);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void NamedBufferStorage(
                    GlHandle buffer,
                    nuint size,
                    ReadOnlySpan<IntPtr> data,
                    uint flags
        )
        {
            fixed (IntPtr* dataPtr = data) {
                _context->glNamedBufferStorage(buffer.Value, size, dataPtr, flags);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void NamedBufferSubData(
                    GlHandle buffer,
                    nint offset,
                    nuint size,
                    ReadOnlySpan<IntPtr> data
        )
        {
            fixed (IntPtr* dataPtr = data) {
                _context->glNamedBufferSubData(buffer.Value, offset, size, dataPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void NamedFramebufferDrawBuffer(
                    GlHandle framebuffer,
                    uint buf
        )
        {
            _context->glNamedFramebufferDrawBuffer(framebuffer.Value, buf);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void NamedFramebufferDrawBuffers(
                    GlHandle framebuffer,
                    int n,
                    ReadOnlySpan<uint> bufs
        )
        {
            fixed (uint* bufsPtr = bufs) {
                _context->glNamedFramebufferDrawBuffers(framebuffer.Value, n, bufsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void NamedFramebufferParameteri(
                    GlHandle framebuffer,
                    uint pname,
                    int param
        )
        {
            _context->glNamedFramebufferParameteri(framebuffer.Value, pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void NamedFramebufferReadBuffer(
                    GlHandle framebuffer,
                    uint src
        )
        {
            _context->glNamedFramebufferReadBuffer(framebuffer.Value, src);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void NamedFramebufferRenderbuffer(
                    GlHandle framebuffer,
                    uint attachment,
                    uint renderbuffertarget,
                    GlHandle renderbuffer
        )
        {
            _context->glNamedFramebufferRenderbuffer(framebuffer.Value, attachment,
                                                     renderbuffertarget, renderbuffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void NamedFramebufferTexture(
                    GlHandle framebuffer,
                    uint attachment,
                    GlHandle texture,
                    int level
        )
        {
            _context->glNamedFramebufferTexture(framebuffer.Value, attachment, texture.Value,
                                                level);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void NamedFramebufferTextureLayer(
                    GlHandle framebuffer,
                    uint attachment,
                    GlHandle texture,
                    int level,
                    int layer
        )
        {
            _context->glNamedFramebufferTextureLayer(framebuffer.Value, attachment, texture.Value,
                                                     level, layer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void NamedRenderbufferStorage(
                    GlHandle renderbuffer,
                    uint internalformat,
                    int width,
                    int height
        )
        {
            _context->glNamedRenderbufferStorage(renderbuffer.Value, internalformat, width, height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void NamedRenderbufferStorageMultisample(
                    GlHandle renderbuffer,
                    int samples,
                    uint internalformat,
                    int width,
                    int height
        )
        {
            _context->glNamedRenderbufferStorageMultisample(renderbuffer.Value, samples,
                                                            internalformat, width, height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadnPixels(
                    int x,
                    int y,
                    int width,
                    int height,
                    uint format,
                    ShaderType type,
                    int bufSize,
                    IntPtr* data
        )
        {
            _context->glReadnPixels(x, y, width, height, format, (uint)type, bufSize, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureBarrier()
        {
            _context->glTextureBarrier();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureBuffer(
                    GlHandle texture,
                    uint internalformat,
                    GlHandle buffer
        )
        {
            _context->glTextureBuffer(texture.Value, internalformat, buffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureBufferRange(
                    GlHandle texture,
                    uint internalformat,
                    GlHandle buffer,
                    nint offset,
                    nuint size
        )
        {
            _context->glTextureBufferRange(texture.Value, internalformat, buffer.Value, offset,
                                           size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureParameterf(
                    GlHandle texture,
                    uint pname,
                    float param
        )
        {
            _context->glTextureParameterf(texture.Value, pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureParameterfv(
                    GlHandle texture,
                    uint pname,
                    ReadOnlySpan<float> param
        )
        {
            fixed (float* paramPtr = param) {
                _context->glTextureParameterfv(texture.Value, pname, paramPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureParameteri(
                    GlHandle texture,
                    uint pname,
                    int param
        )
        {
            _context->glTextureParameteri(texture.Value, pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureParameterIiv(
                    GlHandle texture,
                    uint pname,
                    ReadOnlySpan<int> @params
        )
        {
            fixed (int* @paramsPtr = @params) {
                _context->glTextureParameterIiv(texture.Value, pname, @paramsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureParameterIuiv(
                    GlHandle texture,
                    uint pname,
                    ReadOnlySpan<uint> @params
        )
        {
            fixed (uint* @paramsPtr = @params) {
                _context->glTextureParameterIuiv(texture.Value, pname, @paramsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureParameteriv(
                    GlHandle texture,
                    uint pname,
                    ReadOnlySpan<int> param
        )
        {
            fixed (int* paramPtr = param) {
                _context->glTextureParameteriv(texture.Value, pname, paramPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureStorage1D(
                    GlHandle texture,
                    int levels,
                    uint internalformat,
                    int width
        )
        {
            _context->glTextureStorage1D(texture.Value, levels, internalformat, width);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureStorage2D(
                    GlHandle texture,
                    int levels,
                    uint internalformat,
                    int width,
                    int height
        )
        {
            _context->glTextureStorage2D(texture.Value, levels, internalformat, width, height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureStorage2DMultisample(
                    GlHandle texture,
                    int samples,
                    uint internalformat,
                    int width,
                    int height,
                    byte fixedsamplelocations
        )
        {
            _context->glTextureStorage2DMultisample(texture.Value, samples, internalformat, width,
                                                    height, fixedsamplelocations);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureStorage3D(
                    GlHandle texture,
                    int levels,
                    uint internalformat,
                    int width,
                    int height,
                    int depth
        )
        {
            _context->glTextureStorage3D(texture.Value, levels, internalformat, width, height,
                                         depth);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureStorage3DMultisample(
                    GlHandle texture,
                    int samples,
                    uint internalformat,
                    int width,
                    int height,
                    int depth,
                    byte fixedsamplelocations
        )
        {
            _context->glTextureStorage3DMultisample(texture.Value, samples, internalformat, width,
                                                    height, depth, fixedsamplelocations);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureSubImage1D(
                    GlHandle texture,
                    int level,
                    int xoffset,
                    int width,
                    uint format,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> pixels
        )
        {
            fixed (IntPtr* pixelsPtr = pixels) {
                _context->glTextureSubImage1D(texture.Value, level, xoffset, width, format,
                                              (uint)type, pixelsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureSubImage2D(
                    GlHandle texture,
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
                _context->glTextureSubImage2D(texture.Value, level, xoffset, yoffset, width, height,
                                              format, (uint)type, pixelsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureSubImage3D(
                    GlHandle texture,
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
                _context->glTextureSubImage3D(texture.Value, level, xoffset, yoffset, zoffset,
                                              width, height, depth, format, (uint)type, pixelsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TransformFeedbackBufferBase(
                    uint xfb,
                    uint index,
                    GlHandle buffer
        )
        {
            _context->glTransformFeedbackBufferBase(xfb, index, buffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TransformFeedbackBufferRange(
                    uint xfb,
                    uint index,
                    GlHandle buffer,
                    nint offset,
                    nuint size
        )
        {
            _context->glTransformFeedbackBufferRange(xfb, index, buffer.Value, offset, size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte UnmapNamedBuffer(
                    GlHandle buffer
        )
        {
            return _context->glUnmapNamedBuffer(buffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexArrayAttribBinding(
                    uint vaobj,
                    uint attribindex,
                    uint bindingindex
        )
        {
            _context->glVertexArrayAttribBinding(vaobj, attribindex, bindingindex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexArrayAttribFormat(
                    uint vaobj,
                    uint attribindex,
                    int size,
                    ShaderType type,
                    byte normalized,
                    uint relativeoffset
        )
        {
            _context->glVertexArrayAttribFormat(vaobj, attribindex, size, (uint)type, normalized,
                                                relativeoffset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexArrayAttribIFormat(
                    uint vaobj,
                    uint attribindex,
                    int size,
                    ShaderType type,
                    uint relativeoffset
        )
        {
            _context->glVertexArrayAttribIFormat(vaobj, attribindex, size, (uint)type,
                                                 relativeoffset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexArrayAttribLFormat(
                    uint vaobj,
                    uint attribindex,
                    int size,
                    ShaderType type,
                    uint relativeoffset
        )
        {
            _context->glVertexArrayAttribLFormat(vaobj, attribindex, size, (uint)type,
                                                 relativeoffset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexArrayBindingDivisor(
                    uint vaobj,
                    uint bindingindex,
                    uint divisor
        )
        {
            _context->glVertexArrayBindingDivisor(vaobj, bindingindex, divisor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexArrayElementBuffer(
                    uint vaobj,
                    GlHandle buffer
        )
        {
            _context->glVertexArrayElementBuffer(vaobj, buffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexArrayVertexBuffer(
                    uint vaobj,
                    uint bindingindex,
                    GlHandle buffer,
                    nint offset,
                    int stride
        )
        {
            _context->glVertexArrayVertexBuffer(vaobj, bindingindex, buffer.Value, offset, stride);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexArrayVertexBuffers(
                    uint vaobj,
                    uint first,
                    int count,
                    ReadOnlySpan<uint> buffers,
                    ReadOnlySpan<nint> offsets,
                    ReadOnlySpan<int> strides
        )
        {
            fixed (uint* buffersPtr = buffers)
            {
                fixed (nint* offsetsPtr = offsets)
                {
                    fixed (int* stridesPtr = strides) {
                        _context->glVertexArrayVertexBuffers(vaobj, first, count, buffersPtr,
                                                             offsetsPtr, stridesPtr);
                    }
                }
            }
        }

    }
}
