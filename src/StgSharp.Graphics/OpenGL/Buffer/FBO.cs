// -----------------------------------------------------------------------------
// file="FBO"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics.OpenGL
{
    #pragma warning disable CA1008 
    #pragma warning disable CA1028 
    public enum FrameBufferTarget : uint
    #pragma warning restore CA1028 
    #pragma warning restore CA1008 
    {

        Read = glConst.READ_FRAMEBUFFER,
        All = glConst.FRAMEBUFFER,
        Draw = glConst.DRAW_FRAMEBUFFER,

    }

    public sealed class FrameBuffer : GlBufferObjectBase
    {

        internal unsafe FrameBuffer(
                        int count,
                        glRender binding
        )
            : base(binding)
        {
            _bufferHandle = new GlHandle[count];
            GL.GenFrameBuffers(_bufferHandle);
        }

        /// <summary>
        ///   Bind the frame BufferHandle object with _label frame BufferHandle to the frame
        ///   BufferHandle target specified by target. <see langword="FrameBufferTarget.All" /> will
        ///   be set as default target.
        /// </summary>
        /// <param _label="index">
        ///   Index of object to be binded in handle set
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Bind(
                                    int index
        )
        {
            GL.BindFrameBuffer(FrameBufferTarget.All, _bufferHandle[index]);
        }

        /// <summary>
        ///   Bind the frame BufferHandle object with _label frame BufferHandle to the frame
        ///   BufferHandle target specified by target. Target of this frame BufferHandle should be
        ///   set manually.
        /// </summary>
        /// <param _label="target">
        ///   The frame BufferHandle target of the binding operation.
        /// </param>
        /// <param _label="index">
        ///   Index of object to be binded in handle set
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Bind(
                           FrameBufferTarget target,
                           int index
        )
        {
            GL.BindFrameBuffer(target, _bufferHandle[index]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BindNull()
        {
            OpenGLFunction.CurrentGL.BindFrameBuffer(FrameBufferTarget.All, GlHandle.Zero);
        }

        protected override void Dispose(
                                bool disposing
        )
        {
            throw new NotImplementedException();
        }

    }
}
