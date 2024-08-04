using StgSharp.Graphics.OpenGL;

using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;


namespace StgSharp.Graphics.OpenGL
{



    public class RenderBuffer : GlBufferObjectBase
    {
        public RenderBuffer(int count, glRenderStream binding):base(binding)
        {
            _bufferHandle = GL.GenRenderBuffer(count);
        }

        public override void Bind(int index)
        {
            GL.BindRenderBuffer(this[index]);
        }

        protected override void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }

        public void Store(RenderBufferInternalFormat format, (int width,int height) size)
        {
            if (size.width > binding.Width || size.height>binding.Height)
            {
                StgSharp.LogWarning("FrameBuffer is larger than current canvas binding");
            }
        }
    }
}
