//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="glFunction.v4_3"
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
        public void BindVertexBuffer(
                    uint bindingindex,
                    GlHandle buffer,
                    nint offset,
                    int stride
        )
        {
            _context->glBindVertexBuffer(bindingindex, buffer.Value, offset, stride);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearBufferData(
                    uint target,
                    uint internalformat,
                    uint format,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> data
        )
        {
            fixed (IntPtr* dataPtr = data) {
                _context->glClearBufferData(target, internalformat, format, (uint)type, dataPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearBufferSubData(
                    uint target,
                    uint internalformat,
                    nint offset,
                    nuint size,
                    uint format,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> data
        )
        {
            fixed (IntPtr* dataPtr = data) {
                _context->glClearBufferSubData(target, internalformat, offset, size, format,
                                               (uint)type, dataPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyImageSubData(
                    uint srcName,
                    uint srcTarget,
                    int srcLevel,
                    int srcX,
                    int srcY,
                    int srcZ,
                    uint dstName,
                    uint dstTarget,
                    int dstLevel,
                    int dstX,
                    int dstY,
                    int dstZ,
                    int srcWidth,
                    int srcHeight,
                    int srcDepth
        )
        {
            _context->glCopyImageSubData(srcName, srcTarget, srcLevel, srcX, srcY, srcZ, dstName,
                                         dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight,
                                         srcDepth);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DebugMessageCallback(
                    IntPtr callback,
                    ReadOnlySpan<IntPtr> userParam
        )
        {
            fixed (IntPtr* userParamPtr = userParam) {
                _context->glDebugMessageCallback(callback, userParamPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DebugMessageControl(
                    uint source,
                    ShaderType type,
                    uint severity,
                    int count,
                    ReadOnlySpan<uint> ids,
                    byte enabled
        )
        {
            fixed (uint* idsPtr = ids) {
                _context->glDebugMessageControl(source, (uint)type, severity, count, idsPtr,
                                                enabled);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DebugMessageInsert(
                    uint source,
                    ShaderType type,
                    uint id,
                    uint severity,
                    int length,
                    ReadOnlySpan<byte> buf
        )
        {
            fixed (byte* bufPtr = buf) {
                _context->glDebugMessageInsert(source, (uint)type, id, severity, length, bufPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DispatchCompute(
                    uint num_groups_x,
                    uint num_groups_y,
                    uint num_groups_z
        )
        {
            _context->glDispatchCompute(num_groups_x, num_groups_y, num_groups_z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DispatchComputeIndirect(
                    nint indirect
        )
        {
            _context->glDispatchComputeIndirect(indirect);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FramebufferParameteri(
                    uint target,
                    uint pname,
                    int param
        )
        {
            _context->glFramebufferParameteri(target, pname, param);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetDebugMessageLog(
                    uint count,
                    int bufSize,
                    uint* sources,
                    uint* types,
                    uint* ids,
                    uint* severities,
                    int* lengths,
                    byte* messageLog
        )
        {
            return _context->glGetDebugMessageLog(count, bufSize, sources, types, ids, severities,
                                                  lengths, messageLog);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetFramebufferParameteriv(
                    uint target,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetFramebufferParameteriv(target, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetInternalformati64v(
                    uint target,
                    uint internalformat,
                    uint pname,
                    int count,
                    IntPtr* @params
        )
        {
            _context->glGetInternalformati64v(target, internalformat, pname, count, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetObjectLabel(
                    uint identifier,
                    uint name,
                    int bufSize,
                    int* length,
                    byte* label
        )
        {
            _context->glGetObjectLabel(identifier, name, bufSize, length, label);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetObjectPtrLabel(
                    ReadOnlySpan<IntPtr> ptr,
                    int bufSize,
                    int* length,
                    byte* label
        )
        {
            fixed (IntPtr* ptrPtr = ptr) {
                _context->glGetObjectPtrLabel(ptrPtr, bufSize, length, label);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetProgramInterfaceiv(
                    GlHandle program,
                    uint programInterface,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetProgramInterfaceiv(program.Value, programInterface, pname, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetProgramResourceIndex(
                    GlHandle program,
                    uint programInterface,
                    ReadOnlySpan<byte> name
        )
        {
            fixed (byte* namePtr = name) {
                return _context->glGetProgramResourceIndex(program.Value, programInterface, namePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetProgramResourceiv(
                    GlHandle program,
                    uint programInterface,
                    uint index,
                    int propCount,
                    ReadOnlySpan<uint> props,
                    int count,
                    int* length,
                    int* @params
        )
        {
            fixed (uint* propsPtr = props) {
                _context->glGetProgramResourceiv(program.Value, programInterface, index, propCount,
                                                 propsPtr, count, length, @params);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetProgramResourceLocation(
                   GlHandle program,
                   uint programInterface,
                   ReadOnlySpan<byte> name
        )
        {
            fixed (byte* namePtr = name) {
                return _context->glGetProgramResourceLocation(program.Value, programInterface, namePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetProgramResourceLocationIndex(
                   GlHandle program,
                   uint programInterface,
                   ReadOnlySpan<byte> name
        )
        {
            fixed (byte* namePtr = name) {
                return _context->glGetProgramResourceLocationIndex(program.Value, programInterface, namePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetProgramResourceName(
                    GlHandle program,
                    uint programInterface,
                    uint index,
                    int bufSize,
                    int* length,
                    byte* name
        )
        {
            _context->glGetProgramResourceName(program.Value, programInterface, index, bufSize,
                                               length, name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InvalidateBufferData(
                    GlHandle buffer
        )
        {
            _context->glInvalidateBufferData(buffer.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InvalidateBufferSubData(
                    GlHandle buffer,
                    nint offset,
                    nuint length
        )
        {
            _context->glInvalidateBufferSubData(buffer.Value, offset, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InvalidateFramebuffer(
                    uint target,
                    int numAttachments,
                    ReadOnlySpan<uint> attachments
        )
        {
            fixed (uint* attachmentsPtr = attachments) {
                _context->glInvalidateFramebuffer(target, numAttachments, attachmentsPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InvalidateSubFramebuffer(
                    uint target,
                    int numAttachments,
                    ReadOnlySpan<uint> attachments,
                    int x,
                    int y,
                    int width,
                    int height
        )
        {
            fixed (uint* attachmentsPtr = attachments) {
                _context->glInvalidateSubFramebuffer(target, numAttachments, attachmentsPtr, x, y,
                                                     width, height);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InvalidateTexImage(
                    GlHandle texture,
                    int level
        )
        {
            _context->glInvalidateTexImage(texture.Value, level);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InvalidateTexSubImage(
                    GlHandle texture,
                    int level,
                    int xoffset,
                    int yoffset,
                    int zoffset,
                    int width,
                    int height,
                    int depth
        )
        {
            _context->glInvalidateTexSubImage(texture.Value, level, xoffset, yoffset, zoffset,
                                              width, height, depth);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MultiDrawArraysIndirect(
                    uint mode,
                    ReadOnlySpan<IntPtr> indirect,
                    int drawcount,
                    int stride
        )
        {
            fixed (IntPtr* indirectPtr = indirect) {
                _context->glMultiDrawArraysIndirect(mode, indirectPtr, drawcount, stride);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MultiDrawElementsIndirect(
                    uint mode,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> indirect,
                    int drawcount,
                    int stride
        )
        {
            fixed (IntPtr* indirectPtr = indirect) {
                _context->glMultiDrawElementsIndirect(mode, (uint)type, indirectPtr, drawcount,
                                                      stride);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ObjectLabel(
                    uint identifier,
                    uint name,
                    int length,
                    ReadOnlySpan<byte> label
        )
        {
            fixed (byte* labelPtr = label) {
                _context->glObjectLabel(identifier, name, length, labelPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ObjectPtrLabel(
                    ReadOnlySpan<IntPtr> ptr,
                    int length,
                    ReadOnlySpan<byte> label
        )
        {
            fixed (IntPtr* ptrPtr = ptr)
            {
                fixed (byte* labelPtr = label) {
                    _context->glObjectPtrLabel(ptrPtr, length, labelPtr);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PopDebugGroup()
        {
            _context->glPopDebugGroup();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PushDebugGroup(
                    uint source,
                    uint id,
                    int length,
                    ReadOnlySpan<byte> message
        )
        {
            fixed (byte* messagePtr = message) {
                _context->glPushDebugGroup(source, id, length, messagePtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ShaderStorageBlockBinding(
                    GlHandle program,
                    uint storageBlockIndex,
                    uint storageBlockBinding
        )
        {
            _context->glShaderStorageBlockBinding(program.Value, storageBlockIndex, storageBlockBinding);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexBufferRange(
                    uint target,
                    uint internalformat,
                    GlHandle buffer,
                    nint offset,
                    nuint size
        )
        {
            _context->glTexBufferRange(target, internalformat, buffer.Value, offset, size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexStorage2DMultisample(
                    uint target,
                    int samples,
                    uint internalformat,
                    int width,
                    int height,
                    byte fixedsamplelocations
        )
        {
            _context->glTexStorage2DMultisample(target, samples, internalformat, width, height,
                                                fixedsamplelocations);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexStorage3DMultisample(
                    uint target,
                    int samples,
                    uint internalformat,
                    int width,
                    int height,
                    int depth,
                    byte fixedsamplelocations
        )
        {
            _context->glTexStorage3DMultisample(target, samples, internalformat, width, height,
                                                depth, fixedsamplelocations);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TextureView(
                    GlHandle texture,
                    uint target,
                    GlHandle origtexture,
                    uint internalformat,
                    uint minlevel,
                    uint numlevels,
                    uint minlayer,
                    uint numlayers
        )
        {
            _context->glTextureView(texture.Value, target, origtexture.Value, internalformat,
                                    minlevel, numlevels, minlayer, numlayers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribBinding(
                    uint attribindex,
                    uint bindingindex
        )
        {
            _context->glVertexAttribBinding(attribindex, bindingindex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribFormat(
                    uint attribindex,
                    int size,
                    ShaderType type,
                    byte normalized,
                    uint relativeoffset
        )
        {
            _context->glVertexAttribFormat(attribindex, size, (uint)type, normalized,
                                           relativeoffset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribIFormat(
                    uint attribindex,
                    int size,
                    ShaderType type,
                    uint relativeoffset
        )
        {
            _context->glVertexAttribIFormat(attribindex, size, (uint)type, relativeoffset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttribLFormat(
                    uint attribindex,
                    int size,
                    ShaderType type,
                    uint relativeoffset
        )
        {
            _context->glVertexAttribLFormat(attribindex, size, (uint)type, relativeoffset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexBindingDivisor(
                    uint bindingindex,
                    uint divisor
        )
        {
            _context->glVertexBindingDivisor(bindingindex, divisor);
        }

    }
}
