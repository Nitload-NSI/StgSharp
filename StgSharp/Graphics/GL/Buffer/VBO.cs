//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="VBO.cs"
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
using System.Net.Security;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics
{
    public unsafe class VertexBuffer : BufferObjectBase
    {

        internal VertexBuffer(int n, Form binding)
        {
            this.binding = binding.graphicContextID;
            _bufferHandle = GL.GenBuffers(n);
        }

        public override sealed void Bind(int index)
        {
            GL.BindBuffer(BufferType.ARRAY_BUFFER, _bufferHandle[index]);
        }

        /// <summary>
        /// Set data to current vertex buffer object
        /// </summary>
        /// <typeparam name="T">Type of bufferData</typeparam>
        /// <param name="index">Index to find certain VBO in this instance</param>
        /// <param name="bufferData">Data to write in</param>
        /// <param name="usage">How OpenGL use these data, defined by <see cref="BufferUsage"/></param>
        public void SetValue<T>(int index, T bufferData, BufferUsage usage) where T : struct, IMat
        {
            GL.BindBuffer(BufferType.ARRAY_BUFFER, _bufferHandle[index]);
            GL.SetBufferData(BufferType.ARRAY_BUFFER, bufferData, usage);
        }

        /// <summary>
        /// Set data to current vertex buffer object
        /// </summary>
        /// <typeparam name="T">Type of bufferData</typeparam>
        /// <param name="index">Index to find certain VBO in this instance</param>
        /// <param name="bufferArray">A array of data to write in</param>
        /// <param name="usage">How OpenGL use these data, defined by <see cref="BufferUsage"/></param>
        public void SetValueArray<T>(int index, T[] bufferArray, BufferUsage usage) where T : struct, IConvertible
        {
            GL.BindBuffer(BufferType.ARRAY_BUFFER, _bufferHandle[index]);
            GL.SetBufferData(BufferType.ARRAY_BUFFER, bufferArray, usage);
        }

        /// <summary>
        ///
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unbind()
        {
            GL.BindBuffer(BufferType.ARRAY_BUFFER, glHandle.Zero);
        }

        protected override sealed void Dispose(bool disposing)
        {
            if (disposing)
            {
                GL.DeleteBuffers(_bufferHandle);
                _bufferHandle.Release();
            }
        }

    }
}