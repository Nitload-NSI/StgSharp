//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Point.cs"
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
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Geometries
{
    [StructLayout( LayoutKind.Explicit, Size = 16 )]
    public struct Point
    {

        [FieldOffset( 0 )] private Vec3 coord;
        [FieldOffset( 0 )] internal Vector4 coordVec;

        internal Point( Vec4 vector )
        {
            coordVec = vector.vec;
            coordVec.W = 1;
        }

        public Point( Vec3 coord )
        {
            Coord = coord;
            coordVec.W = 1;
        }

        public Point( float x, float y, float z )
        {
            coordVec = new Vector4( x, y, z, 1 );
        }

        public Vec3 Coord
        {
            get => coord;
            set
            {
                coord = value;
                coordVec.W = 1;
            }
        }

    }
}
