// -----------------------------------------------------------------------------
// file="EBO"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

namespace Nitload.Graphics.OpenGL
{
    /// <summary>A set of OpenGL element-buffer handles.</summary>
    public sealed record class ElementBuffer(
        GlHandle[] Handles
    )
    {

        public int Count => Handles.Length;

        public GlHandle this[int index] => Handles[index];

    }
}
