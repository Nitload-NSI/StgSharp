//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="glFunction.v4_6"
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
