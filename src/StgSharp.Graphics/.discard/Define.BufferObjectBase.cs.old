// -----------------------------------------------------------------------------
// file="BufferObjectBase"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Graphics;
using StgSharp.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics
{
    /// <summary>
    ///   Interface of all kinds OpenGL BufferHandle objects
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
        public GlHandle this[
                        int index
        ] => _bufferHandle[index];

        /// <summary>
        ///   Bind a BufferHandle instance to OpenGL
        /// </summary>
        /// <param _label="index">
        ///   Index of handle of the object in this instance to be bind
        /// </param>
        public abstract void Bind(
                             int index
        );

        public void Dispose()
        {
            Dispose(disposing:true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"This is a {GetType().FullName}, containing {_bufferHandle.Length} buffer handles: {_bufferHandle.ToString()}.";
        }

        protected abstract void Dispose(
                                bool disposing
        );

        ~BufferObjectBase()
        {
            Dispose(disposing:false);
        }

    }
}
