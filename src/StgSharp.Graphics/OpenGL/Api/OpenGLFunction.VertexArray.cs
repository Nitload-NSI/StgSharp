// -----------------------------------------------------------------------------
// file="OpenGLFunction.VertexArray"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Graphics;

using System;
using System.Runtime.CompilerServices;

namespace StgSharp.Graphics.OpenGL
{
    public unsafe partial class OpenGLFunction
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindVertexArray(
                    GlHandle handle
        )
        {
            _context->glBindVertexArray(handle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void DeleteVertexArrays(
                           ReadOnlySpan<GlHandle> arrays
        )
        {
            fixed (GlHandle* arraysPtr = arrays) {
                _context->glDeleteVertexArrays(arrays.Length, (uint*)arraysPtr);
            }
        }

        public unsafe GlHandle[] GenVertexArrays(
                                 int count
        )
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count);

            GlHandle[] h = new GlHandle[count];
            if (h.Length == 0) {
                return h;
            }

            fixed (GlHandle* hptr = h) {
                _context->glGenVertexArrays(count, (uint*)hptr);
            }
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVertexAttribute(
                    uint index,
                    int size,
                    TypeCode t,
                    bool normalized,
                    int stride,
                    int pointer
        )
        {
            uint type = t switch
            {
                TypeCode.Single => glConst.FLOAT,
                TypeCode.Int32 => glConst.INT,
                TypeCode.UInt32 => glConst.UNSIGNED_INT,
                TypeCode.Int16 => glConst.SHORT,
                TypeCode.UInt16 => glConst.UNSIGNED_SHORT,
                TypeCode.SByte => glConst.BYTE,
                TypeCode.Byte => glConst.UNSIGNED_BYTE,
                _ => throw new ArgumentOutOfRangeException(nameof(t), t,
                    "The type cannot be used as an OpenGL vertex attribute."),
            };
            _context->glVertexAttribPointer(index, size, type, (byte)(normalized ? 1 : 0), stride,
                                            (nint*)pointer);
            _context->glEnableVertexAttribArray(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VertexAttributeDivisor(
                    uint layoutIndex,
                    uint divisor
        )
        {
            _context->glVertexAttribDivisor(layoutIndex, divisor);
        }

    }
}
