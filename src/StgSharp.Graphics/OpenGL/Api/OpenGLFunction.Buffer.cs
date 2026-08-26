// -----------------------------------------------------------------------------
// file="OpenGLFunction.Buffer"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

namespace StgSharp.Graphics.OpenGL
{
    public unsafe partial class OpenGLFunction
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindBuffer(
                    BufferType bufferType,
                    GlHandle handle
        )
        {
            _context->glBindBuffer((uint)bufferType, handle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteBuffer(
                    GlHandle handle
        )
        {
            _context->glDeleteBuffers(1, &handle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteBuffers(
                    ReadOnlySpan<GlHandle> handles
        )
        {
            fixed (GlHandle* handlesPtr = handles) {
                _context->glDeleteBuffers(handles.Length, (uint*)handlesPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GlHandle[] GenBuffers(
                          int count
        )
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count);

            GlHandle[] handles = new GlHandle[count];
            if (handles.Length == 0) {
                return handles;
            }

            fixed (GlHandle* handlesPtr = handles) {
                _context->glGenBuffers(count, (uint*)handlesPtr);
            }
            return handles;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetBufferData<T>(
                    BufferType bufferType,
                    ReadOnlySpan<T> bufferData,
                    BufferUsage usage
        ) where T : unmanaged
        {
            nint sizeInBytes = checked((nint)bufferData.Length * sizeof(T));
            fixed (T* bufferPtr = bufferData) {
                _context->glBufferData((uint)bufferType, sizeInBytes, bufferPtr, (uint)usage);
            }
        }

    }
}
