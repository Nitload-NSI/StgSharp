//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="RBO"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.Graphics.OpenGL;

using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace StgSharp.Graphics.OpenGL
{
    public sealed class RenderBuffer : GlBufferObjectBase
    {

        public RenderBuffer(int count, glRender binding)
            : base(binding)
        {
            _bufferHandle = GL.GenRenderBuffer(count);
        }

        public override void Bind(int index)
        {
            GL.BindRenderBuffer(this[index]);
        }

        public void Store(RenderBufferInternalFormat format, (int width, int height) size)
        {
            if ((size.width > binding.Width) || (size.height > binding.Height)) {
                World.LogWarning("FrameBuffer is larger than current canvas binding");
            }
        }

        protected override void Dispose(bool disposing)
        {
            foreach (GlHandle item in _bufferHandle)
            {
                Console.WriteLine("removing a render buffer");
                GL.DeleteRenderBuffer(item);
            }
        }

    }
}
