//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="VAO.cs"
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
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.Graphics
{
    public unsafe class VertexArray : BufferObjectBase
    {

        internal VertexArray(int n, Form binding)
        {
            this.binding = binding.graphicContextID;
            _bufferHandle = GL.GenVertexArrays(n);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void Bind(int index)
        {
            GL.BindVertexArray(_bufferHandle[index]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BindNull()
        {
            GL.BindVertexArray(glHandle.Zero);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVertexAttribute(uint index, int vertexLength, TypeCode dataType, bool isNomalized, uint stride, int pointer)
        {
            GL.SetVertexAttribute(
                index, vertexLength, dataType,
                isNomalized,
                stride, pointer);
        }

        protected override sealed void Dispose(bool disposing)
        {
            if (disposing)
            {
                GL.DeleteVertexArrays(_bufferHandle);
                _bufferHandle.Release();
            }
        }

    }
}