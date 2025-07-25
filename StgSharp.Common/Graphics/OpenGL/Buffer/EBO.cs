//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="EBO.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
using StgSharp.Graphics;
using StgSharp.Graphics.OpenGL;
using StgSharp.Internal.OpenGL;
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics.OpenGL
{
    /// <summary>
    ///   A collection of handle to Element Buffer Object in OpenglFunc. Each <see langword="uint"
    ///   /> value indexed from instance of this class this is the only handle to one Element Buffer
    ///   Object.
    /// </summary>
    public sealed unsafe class ElementBuffer : GlBufferObjectBase
    {

        internal ElementBuffer(int n, glRender binding)
            : base(binding)
        {
            _bufferHandle = GL.GenBuffers(n);
        }

        public override sealed void Bind(int index)
        {
            GL.BindBuffer(BufferType.ElementArrayBuffer, _bufferHandle[index]);
        }

        public void SetValue<T>(int index, T[] bufferArray, BufferUsage usage)
            where T: unmanaged, INumber<T>
        {
            GL.BindBuffer(BufferType.ElementArrayBuffer, _bufferHandle[index]);
            GL.SetBufferData<T>(BufferType.ElementArrayBuffer, bufferArray, usage);
        }

        public void SetValue<T>(int index, ReadOnlySpan<T> bufferSpan, BufferUsage usage)
            where T: unmanaged, INumber<T>
        {
            GL.BindBuffer(BufferType.ElementArrayBuffer, _bufferHandle[index]);
            GL.SetBufferData<T>(BufferType.ElementArrayBuffer, bufferSpan, usage);
        }

        protected override sealed void Dispose(bool disposing)
        {
            if (disposing) {
                GL.DeleteBuffers(_bufferHandle);
            }
        }

    }
}