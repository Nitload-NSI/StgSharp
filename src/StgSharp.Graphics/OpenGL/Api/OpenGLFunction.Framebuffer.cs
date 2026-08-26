// -----------------------------------------------------------------------------
// file="OpenGLFunction.Framebuffer"
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

        /// <summary>Generate framebuffer object handles into the supplied storage.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GenFrameBuffers(
                    Span<GlHandle> framebuffers
        )
        {
            if (framebuffers.IsEmpty) {
                return;
            }

            fixed (GlHandle* framebuffersPtr = framebuffers) {
                Context.glGenFramebuffers(framebuffers.Length, (uint*)framebuffersPtr);
            }
        }

        /// <summary>Delete framebuffer object handles.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteFramebuffers(
                    ReadOnlySpan<GlHandle> framebuffers
        )
        {
            fixed (GlHandle* framebuffersPtr = framebuffers) {
                Context.glDeleteFramebuffers(framebuffers.Length, (uint*)framebuffersPtr);
            }
        }

        /// <summary>
        ///   Bind a framebuffer object to a framebuffer target.
        /// </summary>
        /// <param name="target">The framebuffer target of the binding operation.</param>
        /// <param name="handle">The framebuffer object handle to bind.</param>
        /// <seealso href="https://docs.gl/gl3/glBindFramebuffer" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindFrameBuffer(
                    FrameBufferTarget target,
                    GlHandle handle
        )
        {
            _context->glBindFramebuffer((uint)target, handle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindRenderBuffer(
                    GlHandle handle
        )
        {
            _context->glBindRenderbuffer(glConst.RENDERBUFFER, handle.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FrameBufferStatus CheckFrameBufferStatus(
                                 FrameBufferTarget target
        )
        {
            return (FrameBufferStatus)_context->glCheckFramebufferStatus((uint)target);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CombineFrameBufferRenderBuffer(
                    FrameBufferAttachment attachment,
                    GlHandle rboHandle
        )
        {
            _context->glFramebufferRenderbuffer(glConst.FRAMEBUFFER, (uint)attachment,
                                                glConst.RENDERBUFFER, rboHandle.Value);
        }

        /// <summary>Delete one renderbuffer object.</summary>
        public unsafe void DeleteRenderBuffer(
                           GlHandle bufferHandle
        )
        {
            _context->glDeleteRenderbuffers(1, &bufferHandle.Value);
        }

        /// <summary>Delete renderbuffer object handles.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeleteRenderbuffers(
                    ReadOnlySpan<GlHandle> renderbuffers
        )
        {
            fixed (GlHandle* renderbuffersPtr = renderbuffers) {
                Context.glDeleteRenderbuffers(renderbuffers.Length, (uint*)renderbuffersPtr);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FrameBufferTexture2d(
                    FrameBufferTarget target,
                    uint attachment,
                    Texture2DTarget texTarget,
                    GlHandle textureHandle,
                    int level
        )
        {
            _context->glFramebufferTexture2D((uint)target, attachment, (uint)texTarget,
                                             textureHandle.Value, level);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe GlHandle[] GenRenderBuffer(
                                 int count
        )
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count);

            GlHandle[] h = new GlHandle[count];
            if (h.Length == 0) {
                return h;
            }

            fixed (GlHandle* hptr = h) {
                _context->glGenRenderbuffers(count, (uint*)hptr);
            }
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetRenderBufferStorage(
                           RenderBufferInternalFormat internalFormat,
                           (int width, int height) size
        )
        {
            _context->glRenderbufferStorage(glConst.RENDERBUFFER, (uint)internalFormat, size.width,
                                            size.height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetRenderBufferStorage(
                           RenderBufferInternalFormat internalFormat,
                           (uint width, uint height) size
        )
        {
            _context->glRenderbufferStorage(glConst.RENDERBUFFER, (uint)internalFormat,
                                            (int)size.width, (int)size.height);
        }

    }
}
