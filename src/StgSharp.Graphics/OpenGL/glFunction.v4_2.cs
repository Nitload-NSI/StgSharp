// -----------------------------------------------------------------------------
// file="glFunction.v4_2"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Graphics.OpenGL;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Graphics.OpenGL
{
    public unsafe partial class OpenGLFunction
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindImageTexture(
                    uint unit,
                    GlHandle texture,
                    int level,
                    byte layered,
                    int layer,
                    uint access,
                    uint format
        )
        {
            _context->glBindImageTexture(unit, texture.Value, level, layered, layer, access,
                                         format);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawArraysInstancedBaseInstance(
                    uint mode,
                    int first,
                    int count,
                    int instancecount,
                    uint baseinstance
        )
        {
            _context->glDrawArraysInstancedBaseInstance(mode, first, count, instancecount,
                                                        baseinstance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawElementsInstancedBaseInstance(
                    uint mode,
                    int count,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> indices,
                    int instancecount,
                    uint baseinstance
        )
        {
            fixed (IntPtr* indicesPtr = indices) {
                _context->glDrawElementsInstancedBaseInstance(mode, count, (uint)type, indicesPtr,
                                                              instancecount, baseinstance);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawElementsInstancedBaseVertexBaseInstance(
                    uint mode,
                    int count,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> indices,
                    int instancecount,
                    int basevertex,
                    uint baseinstance
        )
        {
            fixed (IntPtr* indicesPtr = indices) {
                _context->glDrawElementsInstancedBaseVertexBaseInstance(mode, count, (uint)type,
                                                                        indicesPtr, instancecount,
                                                                        basevertex, baseinstance);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawTransformFeedbackInstanced(
                    uint mode,
                    uint id,
                    int instancecount
        )
        {
            _context->glDrawTransformFeedbackInstanced(mode, id, instancecount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawTransformFeedbackStreamInstanced(
                    uint mode,
                    uint id,
                    uint stream,
                    int instancecount
        )
        {
            _context->glDrawTransformFeedbackStreamInstanced(mode, id, stream, instancecount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetActiveAtomicCounterBufferiv(
                    GlHandle program,
                    GlHandle bufferIndex,
                    uint pname,
                    int* @params
        )
        {
            _context->glGetActiveAtomicCounterBufferiv(program.Value, bufferIndex.Value, pname,
                                                       @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetInternalformativ(
                    uint target,
                    uint internalformat,
                    uint pname,
                    int count,
                    int* @params
        )
        {
            _context->glGetInternalformativ(target, internalformat, pname, count, @params);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MemoryBarrier(
                    uint barriers
        )
        {
            _context->glMemoryBarrier(barriers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexStorage1D(
                    uint target,
                    int levels,
                    uint internalformat,
                    int width
        )
        {
            _context->glTexStorage1D(target, levels, internalformat, width);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexStorage2D(
                    uint target,
                    int levels,
                    uint internalformat,
                    int width,
                    int height
        )
        {
            _context->glTexStorage2D(target, levels, internalformat, width, height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexStorage3D(
                    uint target,
                    int levels,
                    uint internalformat,
                    int width,
                    int height,
                    int depth
        )
        {
            _context->glTexStorage3D(target, levels, internalformat, width, height, depth);
        }

    }
}
