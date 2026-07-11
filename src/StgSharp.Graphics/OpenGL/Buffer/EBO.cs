// -----------------------------------------------------------------------------
// file="EBO"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Graphics;
using StgSharp.Graphics.OpenGL;

using StgSharp.Mathematics;

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics.OpenGL
{
    /// <summary>
    ///   A collection of handle to Element BufferHandle Object in OpenglFunc. Each <see ///  
    ///   langword="uint" /> value indexed from instance of this class this is the only handle to
    ///   one Element BufferHandle Object.
    /// </summary>
    public sealed unsafe class ElementBuffer : GlBufferObjectBase
    {

        internal ElementBuffer(
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
            GL.BindBuffer(BufferType.ElementArrayBuffer, _bufferHandle[index]);
        }

        public void SetValue<T>(
                    int index,
                    T[] bufferArray,
                    BufferUsage usage
        ) where T : unmanaged, INumber<T>
        {
            GL.BindBuffer(BufferType.ElementArrayBuffer, _bufferHandle[index]);
            GL.SetBufferData<T>(BufferType.ElementArrayBuffer, bufferArray, usage);
        }

        public void SetValue<T>(
                    int index,
                    ReadOnlySpan<T> bufferSpan,
                    BufferUsage usage
        ) where T : unmanaged, INumber<T>
        {
            GL.BindBuffer(BufferType.ElementArrayBuffer, _bufferHandle[index]);
            GL.SetBufferData<T>(BufferType.ElementArrayBuffer, bufferSpan, usage);
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