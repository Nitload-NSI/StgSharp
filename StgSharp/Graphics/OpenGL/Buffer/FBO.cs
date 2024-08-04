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
    public enum FrameBufferTarget:uint
#pragma warning restore CA1028 
#pragma warning restore CA1008 
    {
        Read = GLconst.READ_FRAMEBUFFER,
        All = GLconst.FRAMEBUFFER,
        Draw = GLconst.DRAW_FRAMEBUFFER,
    }

    public class FrameBuffer : GlBufferObjectBase
    {
        internal unsafe FrameBuffer(int count, glRenderStream binding):base(binding)
        {
            _bufferHandle = GL.GenFrameBuffers(count);
        }

        /// <summary>
        /// Bind the frame buffer object with name frame buffer to the frame buffer target specified by target.
        /// <see langword="FrameBufferTarget.All"/> will be set as default target.
        /// </summary>
        /// <param name="index">Index of object to be binded in handle set</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe override void Bind(int index)
        {
            GL.BindFrameBuffer(FrameBufferTarget.All,_bufferHandle[index]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BindNull()
        {
            GlFunction.CurrentGL.BindFrameBuffer(FrameBufferTarget.All,GlHandle.Zero);
        }

        /// <summary>
        /// Bind the frame buffer object with name frame buffer to the frame buffer target specified by target.
        /// Target of this frame buffer should be set manually.
        /// </summary>
        /// <param name="target">The frame buffer target of the binding operation.</param>
        /// <param name="index">Index of object to be binded in handle set</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Bind(FrameBufferTarget target, int index)
        {
            GL.BindFrameBuffer(target, _bufferHandle[index]);
        }

        protected override void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }
    }
}
