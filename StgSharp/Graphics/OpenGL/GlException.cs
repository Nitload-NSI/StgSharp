//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="glException.cs"
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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace StgSharp.Graphics.OpenGL
{
    public sealed class GlErrorOverflowException : Exception
    {

        public GlErrorOverflowException( int errorCount, uint errorCode )
            : base(
            "Too many opengl error detected at certain position. " + $"The amount of errors exceeds {errorCount}. " + $"Code of the last error is {errorCode}" ) { }

    }

    public sealed class GlExecutionException : Exception
    {

        public GlExecutionException( uint errorCode )
            : base( $"Critical OpenGL error: {errorCode}" ) { }

    }

    public sealed class GlArrayFormatException : Exception
    {

        internal GlArrayFormatException( string message ) : base( message ) { }

        internal GlArrayFormatException(
            SerializationInfo info,
            StreamingContext context )
            : base( info, context ) { }

        internal GlArrayFormatException(
            string message,
            Exception innerException )
            : base( message, innerException ) { }

        public GlArrayFormatException(
            PixelChannelLayout layout,
            string arrayname )
            : base( $"{arrayname} cannot be used as layout of {layout}" ) { }

    }

    public static partial class GlHelper
    {

        public static bool CheckArrayFormat(
            this Array array,
            PixelChannelLayout layout )
        {
            return layout switch
            {
                PixelChannelLayout.Byte or PixelChannelLayout.UByte332 or PixelChannelLayout.UByte233Rev => ( array is byte[] ) || ( array is sbyte[] ),
                PixelChannelLayout.Short => array is short[],
                PixelChannelLayout.UShort => array is ushort[],
                PixelChannelLayout.Int => array is int[],
                PixelChannelLayout.Float => array is float[],
                PixelChannelLayout.UShort4444 => ( array is ushort[] ) || ( array is byte[] ),
                PixelChannelLayout.UByte => ( array is byte[] ) || ( array is sbyte[] ),
                PixelChannelLayout.UInt => array is uint[],
                PixelChannelLayout.UShort565 or
                PixelChannelLayout.UShort565Rev or
                PixelChannelLayout.UShort4444Rev or
                PixelChannelLayout.UShort5551 or
                PixelChannelLayout.UShort1555Rev => array is ushort[],
                PixelChannelLayout.UInt8888Rev or
                PixelChannelLayout.UInt1010102 or
                PixelChannelLayout.UInt2101010Rev => array is int[],
                PixelChannelLayout.UInt8888 => array is uint[] or byte[],
                _ => false,
            };
        }

    }
}
