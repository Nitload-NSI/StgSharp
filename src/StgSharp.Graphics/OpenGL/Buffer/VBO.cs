// -----------------------------------------------------------------------------
// file="VBO"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Graphics;
using StgSharp.Mathematics;
using StgSharp.Mathematics.Numeric;

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

        internal VertexBuffer(
                 int n,
                 glRender binding
        )
            : base(binding)
        {
            _bufferHandle = GL.GenBuffers(n);
        }

        public sealed override void Bind(
                                    int index
        )
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
        ///   Set _data to current vertex BufferHandle object
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
        public void WriteScalerData<T>(
                    int index,
                    T[] bufferArray,
                    BufferUsage usage
        ) where T : unmanaged, INumber<T>
        {
            GL.BindBuffer(BufferType.ArrayBuffer, _bufferHandle[index]);
            GL.SetBufferData(BufferType.ArrayBuffer, bufferArray, usage);
        }

        /// <summary>
        ///   Set _data to current vertex BufferHandle object
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
        public void WriteScalerData<T>(
                    int index,
                    ReadOnlySpan<T> scalerSpan,
                    BufferUsage usage
        ) where T : unmanaged,INumber<T>
        {
            GL.BindBuffer(BufferType.ArrayBuffer, _bufferHandle[index]);
            GL.SetBufferData(BufferType.ArrayBuffer, scalerSpan, usage);
        }

        /// <summary>
        ///   Set data to current vertex BufferHandle object
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
        public void WriteVectorData<T>(
                    int index,
                    ReadOnlySpan<T> vectorSpan,
                    BufferUsage usage
        ) where T : unmanaged, IUnmanagedVector<T>
        {
            GL.BindBuffer(BufferType.ArrayBuffer, _bufferHandle[index]);
            GL.SetBufferVectorData(BufferType.ArrayBuffer, vectorSpan, usage);
        }

        /// <summary>
        ///   Set _data to current vertex BufferHandle object
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
        public void WriteVectorData<T>(
                    int index,
                    T[] vectorArray,
                    BufferUsage usage
        ) where T : unmanaged, IUnmanagedVector<T>
        {
            GL.BindBuffer(BufferType.ArrayBuffer, _bufferHandle[index]);
            GL.SetBufferVectorData(BufferType.ArrayBuffer, vectorArray, usage);
        }

        protected sealed override void Dispose(
                                       bool disposing
        )
        {
            if (disposing) {
                GL.DeleteBuffers(_bufferHandle);
            }
        }

    }
}