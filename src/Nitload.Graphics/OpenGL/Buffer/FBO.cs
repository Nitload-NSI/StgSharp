// -----------------------------------------------------------------------------
// file="FBO"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

namespace Nitload.Graphics.OpenGL
{
    /// <summary>A set of OpenGL framebuffer handles.</summary>
    public sealed record class FrameBuffer(
        GlHandle[] Handles
    )
    {

        public int Count => Handles.Length;

        public GlHandle this[int index] => Handles[index];

    }
}
