// -----------------------------------------------------------------------------
// file="GLBufferObjectBase"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Graphics.OpenGL;

using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Graphics.OpenGL
{
    public abstract class GlBufferObjectBase : BufferObjectBase
    {

        private readonly glRender _contextBinding;

        protected GlBufferObjectBase(
                  glRender contextBinding
        )
        {
            binding = contextBinding;
            _contextBinding = contextBinding;
        }

        protected OpenGLFunction GL => _contextBinding.GL;

    }
}
