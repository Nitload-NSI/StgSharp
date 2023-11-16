using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics
{
    public unsafe struct VertexBuffer : IDisposable
    {
        internal uint[] _bufferID;


        public VertexBuffer(int n)
        {
            _bufferID = new uint[n];
            fixed (uint* ptr = _bufferID)
            {
                GL.gl.GenBuffers(n, ptr);
            }
        }

        public uint this[int n]
        {
            get { return _bufferID[n]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Unbind()
        {
            GL.gl.BindBuffer(GLconst.ARRAY_BUFFER, 0);
        }

        public void Bind(int index)
        {
            GL.gl.BindBuffer(GLconst.ARRAY_BUFFER, _bufferID[index]);
        }

        public void Dispose()
        {
            fixed (uint* id = _bufferID)
            {
                GL.gl.DeleteBuffers(_bufferID.Length, id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bufferData"></param>
        /// <param name="readType"></param>
        public void SetValue<T>(int index, T bufferData, BufferUsage usage) where T : IMat
        {
            GL.gl.BindBuffer((int)BufferType.ARRAY_BUFFER, _bufferID[index]);
            GL.SetBufferData(BufferType.ARRAY_BUFFER, bufferData, usage);

        }

        public void SetValueArray<T>(int index, T[] bufferArray, BufferUsage usage) where T : struct, IConvertible
        {
            GL.gl.BindBuffer(GLconst.ARRAY_BUFFER, _bufferID[index]);
            GL.SetBufferData(BufferType.ARRAY_BUFFER, bufferArray, usage);
        }
    }
}
