// -----------------------------------------------------------------------------
// file="VAO"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

namespace Nitload.Graphics.OpenGL
{
    /// <summary>A set of OpenGL vertex-array handles.</summary>
    public sealed record class VertexArray(
        GlHandle[] Handles
    )
    {

        public int Count => Handles.Length;

        public GlHandle this[int index] => Handles[index];

    }
}
