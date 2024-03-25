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
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics
{
    /// <summary>
    /// A collecion of handle to Element Buffer Object in OpenGL.
    /// Each <see langword="uint"/> value indexed from instance of this class this
    /// is the only handle to one Element Buffer Object.
    /// </summary>
    public unsafe class ElementBuffer : BufferObjectBase
    {

        internal ElementBuffer(int n, Form binding)
        {
            this.binding = binding;
            _bufferHandle = binding.GL.GenBuffers(n);
        }

        public override sealed void Bind(int index)
        {
            binding.GL.BindBuffer(BufferType.ELEMENT_ARRAY_BUFFER, _bufferHandle[index]);
        }

        public void SetValue<T>(int index, T[] bufferArray, BufferUsage usage)
            where T : struct, IConvertible
        {
            binding.GL.BindBuffer(BufferType.ELEMENT_ARRAY_BUFFER, _bufferHandle[index]);
            binding.GL.SetBufferData(BufferType.ELEMENT_ARRAY_BUFFER, bufferArray, usage);
        }

        protected override sealed void Dispose(bool disposing)
        {
            if (disposing)
            {
                binding.GL.DeleteBuffers(_bufferHandle);
                _bufferHandle.Release();
            }
        }

    }
}