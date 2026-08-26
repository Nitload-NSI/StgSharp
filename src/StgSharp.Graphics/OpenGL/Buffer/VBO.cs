// -----------------------------------------------------------------------------
// file="VBO"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

namespace StgSharp.Graphics.OpenGL
{
    /// <summary>A set of OpenGL vertex-buffer handles.</summary>
    public sealed record class VertexBuffer(
        GlHandle[] Handles
    )
    {

        public int Count => Handles.Length;

        public GlHandle this[int index] => Handles[index];

    }
}
