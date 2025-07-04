﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="GemoryException.cs"
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
using System;
using System.Runtime.Serialization;

namespace StgSharp.Geometries
{
    public unsafe class ToUpperDimensionException : Exception
    {

        protected ToUpperDimensionException( SerializationInfo info, StreamingContext context )
            : base( info, context ) { }

        public ToUpperDimensionException() { }

        public ToUpperDimensionException( string message ) : base( message ) { }

        public ToUpperDimensionException( string message, Exception innerException )
            : base( message, innerException ) { }

    }

    /// <summary>
    ///   当几何体调用了过多的CalcVec方法时，产生UnusedVertexException异常
    /// </summary>
    public class UnusedVertexException : Exception
    {

        protected UnusedVertexException( SerializationInfo info, StreamingContext context )
            : base( info, context ) { }

        public UnusedVertexException() { }

        public UnusedVertexException( string message ) : base( message ) { }

        public UnusedVertexException( string message, Exception innerException )
            : base( message, innerException ) { }

    }
}
