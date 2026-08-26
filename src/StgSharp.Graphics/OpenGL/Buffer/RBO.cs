// -----------------------------------------------------------------------------
// file="RBO"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

namespace StgSharp.Graphics.OpenGL
{
    /// <summary>A set of OpenGL renderbuffer handles.</summary>
    public sealed record class RenderBuffer(
        GlHandle[] Handles
    )
    {

        public int Count => Handles.Length;

        public GlHandle this[int index] => Handles[index];

    }
}
