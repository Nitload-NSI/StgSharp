//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="glFunction.v4_4"
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
        public void BindBuffersBase(
                    uint target,
                    uint first,
                    int count,
                    ReadOnlySpan<uint> buffers
        )
        {
            fixed (uint* buffersPtr = buffers) {
                _context->glBindBuffersBase(target, first, count, buffersPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindBuffersRange(
                    uint target,
                    uint first,
                    int count,
                    ReadOnlySpan<uint> buffers,
                    ReadOnlySpan<nint> offsets,
                    ReadOnlySpan<nuint> sizes
        )
        {
            fixed (uint* buffersPtr = buffers)
            {
                fixed (nint* offsetsPtr = offsets)
                {
                    fixed (nuint* sizesPtr = sizes) {
                        _context->glBindBuffersRange(target, first, count, buffersPtr, offsetsPtr,
                                                     sizesPtr);
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindImageTextures(
                    uint first,
                    int count,
                    ReadOnlySpan<uint> textures
        )
        {
            fixed (uint* texturesPtr = textures) {
                _context->glBindImageTextures(first, count, texturesPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindSamplers(
                    uint first,
                    int count,
                    ReadOnlySpan<uint> samplers
        )
        {
            fixed (uint* samplersPtr = samplers) {
                _context->glBindSamplers(first, count, samplersPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindTextures(
                    uint first,
                    int count,
                    ReadOnlySpan<uint> textures
        )
        {
            fixed (uint* texturesPtr = textures) {
                _context->glBindTextures(first, count, texturesPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindVertexBuffers(
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
                        _context->glBindVertexBuffers(first, count, buffersPtr, offsetsPtr,
                                                      stridesPtr);
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BufferStorage(
                    uint target,
                    nuint size,
                    ReadOnlySpan<IntPtr> data,
                    uint flags
        )
        {
            fixed (IntPtr* dataPtr = data) {
                _context->glBufferStorage(target, size, dataPtr, flags);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearTexImage(
                    GlHandle texture,
                    int level,
                    uint format,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> data
        )
        {
            fixed (IntPtr* dataPtr = data) {
                _context->glClearTexImage(texture.Value, level, format, (uint)type, dataPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearTexSubImage(
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
                    ReadOnlySpan<IntPtr> data
        )
        {
            fixed (IntPtr* dataPtr = data) {
                _context->glClearTexSubImage(texture.Value, level, xoffset, yoffset, zoffset, width,
                                             height, depth, format, (uint)type, dataPtr);
            }
        }

    }
}
