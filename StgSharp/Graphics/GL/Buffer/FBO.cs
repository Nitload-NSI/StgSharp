using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics
{
    public enum FrameBufferTarget:uint
    {
        Read = GLconst.READ_FRAMEBUFFER,
        All = GLconst.FRAMEBUFFER,
        Draw = GLconst.DRAW_FRAMEBUFFER,
    }

    public class FrameBuffer : BufferObjectBase
    {
        internal unsafe FrameBuffer(int count, IntPtr contextBinding)
        {
            binding = contextBinding;
            _bufferHandle = new glHandleSet(count);
            ((GLcontext*)binding)->glGenFramebuffers((uint)count,_bufferHandle.handle);
        }

        /// <summary>
        /// Bind the frame buffer object with name frame buffer to the frame buffer target specified by target.
        /// <see langword="FrameBufferTarget.All"/> will be set as default target.
        /// </summary>
        /// <param name="index">Index of object to be binded in handle set</param>
        public unsafe override void Bind(int index)
        {
            ((GLcontext*)binding)->glBindFramebuffer(GLconst.FRAMEBUFFER, _bufferHandle[index].Value);
        }

        /// <summary>
        /// Bind the frame buffer object with name frame buffer to the frame buffer target specified by target.
        /// Target of this frame buffer should be set manually.
        /// </summary>
        /// <param name="target">The frame buffer target of the binding operation.</param>
        /// <param name="index">Index of object to be binded in handle set</param>
        public unsafe void Bind(FrameBufferTarget target, int index)
        {
            ((GLcontext*)binding)->glBindFramebuffer((uint)target, _bufferHandle[index].Value);
        }

        protected override void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }
    }
}
