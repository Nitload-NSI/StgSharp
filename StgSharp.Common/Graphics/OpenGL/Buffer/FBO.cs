//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="FBO.cs"
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
using StgSharp.Graphics.OpenGL;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics.OpenGL
{
    #pragma warning disable CA1008 
    #pragma warning disable CA1028 
    public enum FrameBufferTarget : uint
    #pragma warning restore CA1028 
    #pragma warning restore CA1008 
    {

        Read = glConst.READ_FRAMEBUFFER,
        All = glConst.FRAMEBUFFER,
        Draw = glConst.DRAW_FRAMEBUFFER,

    }

    public sealed class FrameBuffer : GlBufferObjectBase
    {

        internal unsafe FrameBuffer( int count, glRender binding )
            : base( binding )
        {
            _bufferHandle = GL.GenFrameBuffers( count );
        }

        /// <summary>
        ///   Bind the frame Buffer object with _label frame Buffer to the frame Buffer target
        ///   specified by target. <see langword="FrameBufferTarget.All" /> will be set as default
        ///   target.
        /// </summary>
        /// <param _label="index">
        ///   Index of object to be binded in handle set
        /// </param>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public override unsafe void Bind( int index )
        {
            GL.BindFrameBuffer( FrameBufferTarget.All, _bufferHandle[ index ] );
        }

        /// <summary>
        ///   Bind the frame Buffer object with _label frame Buffer to the frame Buffer target
        ///   specified by target. Target of this frame Buffer should be set manually.
        /// </summary>
        /// <param _label="target">
        ///   The frame Buffer target of the binding operation.
        /// </param>
        /// <param _label="index">
        ///   Index of object to be binded in handle set
        /// </param>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public unsafe void Bind( FrameBufferTarget target, int index )
        {
            GL.BindFrameBuffer( target, _bufferHandle[ index ] );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void BindNull()
        {
            OpenGLFunction.CurrentGL.BindFrameBuffer( FrameBufferTarget.All, GlHandle.Zero );
        }

        protected override void Dispose( bool disposing )
        {
            throw new NotImplementedException();
        }

    }
}
