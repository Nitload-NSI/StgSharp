﻿//-----------------------------------------------------------------------
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
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics.OpenGL
{
    public sealed unsafe class VertexBuffer : GlBufferObjectBase
    {

        internal VertexBuffer(int n, glRender binding)
            : base(binding)
        {
            _bufferHandle = GL.GenBuffers(n);
        }

        public override sealed void Bind(int index)
        {
            GL.BindBuffer(BufferType.ArrayBuffer, _bufferHandle[index]);
        }

        /// <summary>
        ///
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unbind()
        {
            GL.BindBuffer(BufferType.ArrayBuffer, GlHandle.Zero);
        }

        /// <summary>
        ///   Set _data to current vertex Buffer object
        /// </summary>
        /// <typeparam _label="TItem">
        ///   Type of bufferData
        /// </typeparam>
        /// <param _label="index">
        ///   Index to find certain VBO in this instance
        /// </param>
        /// <param _label="bufferData">
        ///   Data to write in
        /// </param>
        /// <param _label="usage">
        ///   How OpenGL use these _data, defined by <see cref="BufferUsage" />
        /// </param>
        public void WriteMatrixData<T>(int index, T bufferData, BufferUsage usage)
            where T: unmanaged, IMatrix<T>
        {
            GL.BindBuffer(BufferType.ArrayBuffer, _bufferHandle[index]);
            GL.SetBufferData(BufferType.ArrayBuffer, bufferData, usage);
        }

        /// <summary>
        ///   Set _data to current vertex Buffer object
        /// </summary>
        /// <typeparam _label="TItem">
        ///   Type of bufferData
        /// </typeparam>
        /// <param _label="index">
        ///   Index to find certain VBO in this instance
        /// </param>
        /// <param _label="bufferArray">
        ///   Data to write in
        /// </param>
        /// <param _label="usage">
        ///   How OpenGL use these _data, defined by <see cref="BufferUsage" />
        /// </param>
        public void WriteScalerData<T>(int index, T[] bufferArray, BufferUsage usage)
            where T: unmanaged, INumber<T>
        {
            GL.BindBuffer(BufferType.ArrayBuffer, _bufferHandle[index]);
            GL.SetBufferData(BufferType.ArrayBuffer, bufferArray, usage);
        }

        /// <summary>
        ///   Set _data to current vertex Buffer object
        /// </summary>
        /// <typeparam _label="TItem">
        ///   Type of bufferData
        /// </typeparam>
        /// <param _label="index">
        ///   Index to find certain VBO in this instance
        /// </param>
        /// <param _label="scalerSpan">
        ///   A <see cref="Span{T}" /> of _data to write in
        /// </param>
        /// <param _label="usage">
        ///   How OpenGL use these _data, defined by <see cref="BufferUsage" />
        /// </param>
        public void WriteScalerData<T>(int index, ReadOnlySpan<T> scalerSpan, BufferUsage usage)
            where T: unmanaged,INumber<T>
        {
            GL.BindBuffer(BufferType.ArrayBuffer, _bufferHandle[index]);
            GL.SetBufferData(BufferType.ArrayBuffer, scalerSpan, usage);
        }

        /// <summary>
        ///   Set data to current vertex Buffer object
        /// </summary>
        /// <typeparam _label="TItem">
        ///   Type of bufferData
        /// </typeparam>
        /// <param _label="index">
        ///   Index to find certain VBO in this instance
        /// </param>
        /// <param _label="vectorSpan">
        ///   A <see cref="Span{T}" /> of data to write in
        /// </param>
        /// <param _label="usage">
        ///   How OpenGL use these data, defined by <see cref="BufferUsage" />
        /// </param>
        public void WriteVectorData<T>(int index, ReadOnlySpan<T> vectorSpan, BufferUsage usage)
            where T: unmanaged, IFixedVector<T>
        {
            GL.BindBuffer(BufferType.ArrayBuffer, _bufferHandle[index]);
            GL.SetBufferVectorData(BufferType.ArrayBuffer, vectorSpan, usage);
        }

        /// <summary>
        ///   Set _data to current vertex Buffer object
        /// </summary>
        /// <typeparam _label="TItem">
        ///   Type of bufferData
        /// </typeparam>
        /// <param _label="index">
        ///   Index to find certain VBO in this instance
        /// </param>
        /// <param _label="vectorArray">
        ///   A array of _data to write in
        /// </param>
        /// <param _label="usage">
        ///   How OpenGL use these _data, defined by <see cref="BufferUsage" />
        /// </param>
        public void WriteVectorData<T>(int index, T[] vectorArray, BufferUsage usage)
            where T: unmanaged, IFixedVector<T>
        {
            GL.BindBuffer(BufferType.ArrayBuffer, _bufferHandle[index]);
            GL.SetBufferVectorData(BufferType.ArrayBuffer, vectorArray, usage);
        }

        protected override sealed void Dispose(bool disposing)
        {
            if (disposing) {
                GL.DeleteBuffers(_bufferHandle);
            }
        }

    }
}