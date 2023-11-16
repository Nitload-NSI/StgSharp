using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics
{
    internal unsafe class ElementBuffer : IDisposable
    {
        uint[] _bufferID;


        public ElementBuffer(int n)
        {
            _bufferID = new uint[n];
            fixed (uint* id = _bufferID)
            {
                GL.gl.GenBuffers(n, id);
            }
        }

        public uint this[int n]
        {
            get { return _bufferID[n]; }
        }

        public void Bind(int index)
        {
            GL.gl.BindBuffer(GLconst.ELEMENT_ARRAY_BUFFER, _bufferID[index]);
        }


        public void SetValue<T>(int index, T[] bufferArray, BufferUsage usage)
            where T : struct, IConvertible
        {
            GL.gl.BindBuffer(GLconst.ELEMENT_ARRAY_BUFFER, _bufferID[index]);
            GL.SetBufferData(BufferType.ELEMENT_ARRAY_BUFFER, bufferArray, usage);
        }

        public void Dispose()
        {
            fixed (uint* id = _bufferID)
            {
                GL.gl.DeleteBuffers(1, id);
            }
        }


    }
}
