// -----------------------------------------------------------------------------
// file="FBO"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

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

    /// <summary>A set of OpenGL framebuffer handles.</summary>
    public sealed record class FrameBuffer(
        GlHandle[] Handles
    )
    {

        public int Count => Handles.Length;

        public GlHandle this[int index] => Handles[index];

    }
}
