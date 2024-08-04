using StgSharp.Graphics.OpenGL;

using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Graphics.OpenGL
{
    public abstract class GlBufferObjectBase : BufferObjectBase
    {
        private readonly glRenderStream _contextBinding;

        protected GlBufferObjectBase(glRenderStream contextBinding)
        {
            binding = contextBinding;
            _contextBinding = contextBinding;
        }

        protected GlFunction GL => _contextBinding.GL;

    }
}
