﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="BufferObjectBase.cs"
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics
{
    /// <summary>
    ///   Interface of all kinds OpenGL Buffer objects
    /// </summary>
    public abstract class BufferObjectBase : IDisposable
    {

        protected GlHandle[] _bufferHandle;
        protected RenderStream binding;

        /// <summary>
        ///   Get the only handle to one of the Object instance
        /// </summary>
        /// <param _label="index">
        ///
        /// </param>
        /// <returns>
        ///
        /// </returns>
        public GlHandle this[ int index ] => _bufferHandle[ index ];

        /// <summary>
        ///   Bind a Buffer instance to OpenGL
        /// </summary>
        /// <param _label="index">
        ///   Index of handle of the object in this instance to be bind
        /// </param>
        public abstract void Bind( int index );

        public void Dispose()
        {
            Dispose( disposing: true );
            GC.SuppressFinalize( this );
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"This is a {GetType().FullName}, containing {_bufferHandle.Length} buffer handles: {_bufferHandle.ToString()}.";
        }

        protected abstract void Dispose( bool disposing );

        ~BufferObjectBase()
        {
            Dispose( disposing: false );
        }

    }
}
