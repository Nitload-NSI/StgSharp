// -----------------------------------------------------------------------------
// file="glFunction.v4_6"
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
        public void MultiDrawArraysIndirectCount(
                    uint mode,
                    ReadOnlySpan<IntPtr> indirect,
                    nint drawcount,
                    int maxdrawcount,
                    int stride
        )
        {
            fixed (IntPtr* indirectPtr = indirect) {
                _context->glMultiDrawArraysIndirectCount(mode, indirectPtr, drawcount, maxdrawcount,
                                                         stride);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MultiDrawElementsIndirectCount(
                    uint mode,
                    ShaderType type,
                    ReadOnlySpan<IntPtr> indirect,
                    nint drawcount,
                    int maxdrawcount,
                    int stride
        )
        {
            fixed (IntPtr* indirectPtr = indirect) {
                _context->glMultiDrawElementsIndirectCount(mode, (uint)type, indirectPtr, drawcount,
                                                           maxdrawcount, stride);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PolygonOffsetClamp(
                    float factor,
                    float units,
                    float clamp
        )
        {
            _context->glPolygonOffsetClamp(factor, units, clamp);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SpecializeShader(
                    GlHandle shader,
                    ReadOnlySpan<byte> pEntryPoint,
                    uint numSpecializationConstants,
                    ReadOnlySpan<uint> pConstantIndex,
                    ReadOnlySpan<uint> pConstantValue
        )
        {
            fixed (byte* pEntryPointPtr = pEntryPoint)
            {
                fixed (uint* pConstantIndexPtr = pConstantIndex)
                {
                    fixed (uint* pConstantValuePtr = pConstantValue) {
                        _context->glSpecializeShader(shader.Value, pEntryPointPtr,
                                                     numSpecializationConstants, pConstantIndexPtr,
                                                     pConstantValuePtr);
                    }
                }
            }
        }

    }
}
