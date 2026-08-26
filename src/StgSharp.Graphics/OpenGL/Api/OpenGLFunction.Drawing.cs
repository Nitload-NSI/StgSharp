// -----------------------------------------------------------------------------
// file="OpenGLFunction.Drawing"
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
        public void Clear(
                    MaskBufferBit mask
        )
        {
            _context->glClear((uint)mask);
        }

        public void DepthMaskAccess(
                    bool isWrite
        )
        {
            _context->glDepthMask(isWrite ? (byte)1 : (byte)0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable(
                    glOperation operation
        )
        {
            _context->glDisable((uint)operation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void DrawArrays(
                           GeometryType mode,
                           int first,
                           int count
        )
        {
            _context->glDrawArrays((uint)mode, first, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void DrawElements(
                           GeometryType mode,
                           int count,
                           TypeCode type,
                           IntPtr ptr
        )
        {
            uint a = type switch
            {
                TypeCode.UInt32 => glConst.UNSIGNED_INT,
                TypeCode.UInt16 => glConst.UNSIGNED_SHORT,
                TypeCode.Byte => glConst.UNSIGNED_BYTE,
                _ => throw new ArgumentException(
                    $"Parameter error in parameter {nameof(type)} method DrawElements. Only {typeof(uint).Name},{typeof(ushort).Name},{typeof(byte).Name} types are supported.")
            };
            _context->glDrawElements((uint)mode, count, a, (nint*)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawElementsInstanced(
                    GeometryType mode,
                    int count,
                    TypeCode type,
                    IntPtr ptr,
                    int amount
        )
        {
            uint a = type switch
            {
                TypeCode.UInt32 => glConst.UNSIGNED_INT,
                TypeCode.UInt16 => glConst.UNSIGNED_SHORT,
                TypeCode.Byte => glConst.UNSIGNED_BYTE,
                _ => throw new ArgumentException(
                    $"Parameter error in parameter {nameof(type)} method DrawElements. Only {typeof(uint).Name},{typeof(ushort).Name},{typeof(byte).Name} types are supported.")
            };
            _context->glDrawElementsInstanced((uint)mode, count, a, (nint*)ptr, amount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable(
                    glOperation operation
        )
        {
            _context->glEnable((uint)operation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PolygonMode(
                    FaceMode mode
        )
        {
            _context->glPolygonMode(glConst.FRONT_AND_BACK, (uint)mode);
        }

    }
}
