using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Graphics
{
    internal unsafe struct VertexArray : IDisposable
    {
        internal uint[] _bufferID;

        public VertexArray(int n)
        {
            _bufferID = new uint[n];
            fixed (uint* id = _bufferID)
            {
                GL.gl.GenVertexArrays(n, id);
            }
        }

        public uint this[int n]
        {
            get
            {
                return _bufferID[n];
            }
        }

        public void Bind(int index)
        {
            GL.gl.BindVertexArray(_bufferID[index]);
        }

        public void Dispose()
        {
            fixed (uint* id = _bufferID)
            {
                GL.gl.DeleteVertexArrays(_bufferID.Length, id);
            }
        }


        public void SetVertexAttribute(uint index, int vertexLength, Type dataType, bool isNomalized, int stride, int pointer)
        {
            GL.gl.VertexAttribPointer(
                index, vertexLength, InternalIO.GLtype[dataType],
                isNomalized,
                stride, (void*)pointer);
        }
    }
}
