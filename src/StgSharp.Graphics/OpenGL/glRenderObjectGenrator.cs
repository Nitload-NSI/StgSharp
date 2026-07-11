// -----------------------------------------------------------------------------
// file="glRenderObjectGenrator"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics.OpenGL
{
    public partial class glRender
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected AutoTextureGL CreateAutoTexture(
                                int count
        )
        {
            return new AutoTextureGL(count, this);
        }

        /// <summary>
        ///   Create a set of <see cref="ElementBuffer" />.
        /// </summary>
        /// <param _label="count">
        ///   Amount of EBO to be created.
        /// </param>
        /// <returns>
        ///
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected ElementBuffer CreateElementBuffer(
                                int count
        )
        {
            return new ElementBuffer(count, this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected FrameBuffer CreateFrameBuffer(
                              int count
        )
        {
            return new FrameBuffer(count, this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected RenderBuffer CreateRenderBuffer(
                               int count
        )
        {
            return new RenderBuffer(count, this);
        }

        /// <summary>
        ///   Create a sets of <see cref="TextureGL" />
        /// </summary>
        /// <param _label="count">
        ///   Amount of textures to be created.
        /// </param>
        /// <returns>
        ///
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected TextureGL CreateTexture(
                            int count
        )
        {
            return new TextureGL(count, this);
        }

        /// <summary>
        ///   Create a set of <see cref="VertexArray" />.
        /// </summary>
        /// <param _label="count">
        ///   Amount of VAO to be created.
        /// </param>
        /// <returns>
        ///
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected VertexArray CreateVertexArray(
                              int count
        )
        {
            return new VertexArray(count, this);
        }

        /// <summary>
        ///   Create a set of <see cref="VertexBuffer" />.
        /// </summary>
        /// <param _label="count">
        ///   Amount of Object to be created.
        /// </param>
        /// <returns>
        ///
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected VertexBuffer CreateVertexBuffer(
                               int count
        )
        {
            return new VertexBuffer(count, this);
        }

    }
}