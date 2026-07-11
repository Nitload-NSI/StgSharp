// -----------------------------------------------------------------------------
// file="VAO"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.Graphics.OpenGL
{
    public sealed unsafe class VertexArray : GlBufferObjectBase
    {

        internal VertexArray(
                 int n,
                 glRender binding
        )
            : base(binding)
        {
            _bufferHandle = GL.GenVertexArrays(n);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sealed override void Bind(
                                    int index
        )
        {
            GL.BindVertexArray(_bufferHandle[index]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BindNull()
        {
            OpenGLFunction.CurrentGL.BindVertexArray(GlHandle.Zero);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVertexAttribute(
                    uint attributeIndex,
                    int vertexLength,
                    TypeCode dataType,
                    bool isNomalized,
                    int stride,
                    int pointer
        )
        {
            GL.SetVertexAttribute(attributeIndex, vertexLength, dataType, isNomalized, stride,
                                  pointer);
        }

        protected sealed override void Dispose(
                                       bool disposing
        )
        {
            if (disposing) {
                GL.DeleteVertexArrays(_bufferHandle);
            }
        }

    }
}