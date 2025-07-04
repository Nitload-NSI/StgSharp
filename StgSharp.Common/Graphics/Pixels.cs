﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Pixels.cs"
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
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics
{
    public interface IPixel
    {

        public float Red { get; set; }

        public float Green { get; set; }

        public float Blue { get; set; }

        public float Alpha { get; set; }

        internal int Size { get; }

    }

    [StructLayout( LayoutKind.Explicit, Size = 32 )]
    public unsafe struct Pixel32RGBA : IPixel
    {

        [FieldOffset( 0 )] private unsafe fixed byte data[ 4 ];

        public int Size => 4;

        float IPixel.Alpha
        {
            get => data[ 3 ] / 256                    ;
            set => data[ 3 ] = ( byte )( value * 256 );
        }

        float IPixel.Blue
        {
            get => data[ 2 ] / 256                    ;
            set => data[ 2 ] = ( byte )( value * 256 );
        }

        float IPixel.Green
        {
            get => data[ 1 ] / 256                    ;
            set => data[ 1 ] = ( byte )( value * 256 );
        }

        float IPixel.Red
        {
            get => data[ 0 ] / 256                    ;
            set => data[ 0 ] = ( byte )( value * 256 );
        }

    }
}
