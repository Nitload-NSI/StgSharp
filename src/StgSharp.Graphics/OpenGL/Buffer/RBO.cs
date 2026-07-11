// -----------------------------------------------------------------------------
// file="RBO"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Graphics;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace StgSharp.Graphics.OpenGL
{
    public sealed class RenderBuffer : GlBufferObjectBase
    {

        public RenderBuffer(
               int count,
               glRender binding
        )
            : base(binding)
        {
            _bufferHandle = GL.GenRenderBuffer(count);
        }

        public override void Bind(
                             int index
        )
        {
            GL.BindRenderBuffer(this[index]);
        }

        public void Store(
                    RenderBufferInternalFormat format,
                    (int width, int height) size
        )
        {
            if ((size.width > binding.Width) || (size.height > binding.Height)) {
                World.LogWarning("FrameBuffer is larger than current canvas binding");
            }
        }

        protected override void Dispose(
                                bool disposing
        )
        {
            foreach (GlHandle item in _bufferHandle)
            {
                Console.WriteLine("removing a render buffer");
                GL.DeleteRenderBuffer(item);
            }
        }

    }
}
