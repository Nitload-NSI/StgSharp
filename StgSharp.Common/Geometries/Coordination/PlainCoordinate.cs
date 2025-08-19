//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="PlainCoordinate.cs"
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
using StgSharp.Mathematics;

using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace StgSharp.Geometries
{
    public class PlainCoordinate : CoordinationBase
    {

        private static PlainCoordinate statndardPlainCoordination = new PlainCoordinate(
            null, new Point( 0, 0, 0 ), new Vec3( 1, 0, 0 ), new Vec3( 0, 1, 0 ) );

        public PlainCoordinate( CoordinationBase local, Point origin, Vec3 xAxis, Vec3 yAxis )
            : base( local )
        {
            CoordMat = Matrix44.Unit;

            CoordMat.colum0 = new Vec4( xAxis, 0 );
            CoordMat.colum1 = new Vec4( yAxis, 0 );
            CoordMat.colum2 = new Vec4( Linear.Normalize( Vec3.Cross( xAxis, yAxis ) ), 0 );

            LocalOrigin = origin;
        }

        public static PlainCoordinate StandardPlainCoordination => statndardPlainCoordination;

    }
}
