using System;
using System.Runtime.Serialization;

namespace StgSharp.Geometries
{
    public unsafe class ToUpperDimensionException : Exception
    {
        public ToUpperDimensionException()
        {
        }

        public ToUpperDimensionException(string message) : base(message)
        {
        }

        public ToUpperDimensionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ToUpperDimensionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    /// <summary>
    /// 当几何体调用了过多的CalcVec方法时，产生UnusedVertexException异常
    /// </summary>
    public class UnusedVertexException : Exception
    {
        public UnusedVertexException()
        {
        }

        public UnusedVertexException(string message) : base(message)
        {
        }

        public UnusedVertexException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnusedVertexException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
