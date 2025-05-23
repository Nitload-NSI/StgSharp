﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="glRenderObjectGenrator.cs"
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

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        protected AutoTextureGL CreateAutoTexture( int count )
        {
            return new AutoTextureGL( count, this );
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
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        protected ElementBuffer CreateElementBuffer( int count )
        {
            return new ElementBuffer( count, this );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        protected FrameBuffer CreateFrameBuffer( int count )
        {
            return new FrameBuffer( count, this );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        protected RenderBuffer CreateRenderBuffer( int count )
        {
            return new RenderBuffer( count, this );
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
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        protected TextureGL CreateTexture( int count )
        {
            return new TextureGL( count, this );
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
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        protected VertexArray CreateVertexArray( int count )
        {
            return new VertexArray( count, this );
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
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        protected VertexBuffer CreateVertexBuffer( int count )
        {
            return new VertexBuffer( count, this );
        }

    }
}