// -----------------------------------------------------------------------------
// file="GemoryException"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace StgSharp.Geometries
{
    public unsafe class ToUpperDimensionException : Exception
    {

        protected ToUpperDimensionException(
                  SerializationInfo info,
                  StreamingContext context
        )
            : base(info, context) { }

        public ToUpperDimensionException() { }

        public ToUpperDimensionException(
               string message
        )
            : base(message) { }

        public ToUpperDimensionException(
               string message,
               Exception innerException
        )
            : base(message, innerException) { }

    }

    /// <summary>
    ///   当几何体调用了过多的CalcVec方法时，产生UnusedVertexException异常
    /// </summary>
    public class UnusedVertexException : Exception
    {

        protected UnusedVertexException(
                  SerializationInfo info,
                  StreamingContext context
        )
            : base(info, context) { }

        public UnusedVertexException() { }

        public UnusedVertexException(
               string message
        )
            : base(message) { }

        public UnusedVertexException(
               string message,
               Exception innerException
        )
            : base(message, innerException) { }

    }
}
