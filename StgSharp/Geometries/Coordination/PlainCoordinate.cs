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
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace StgSharp.Geometries
{
    public class PlainCoordinate : CoordinationBase
    {
        public PlainCoordinate(
            CoordinationBase local,
            Point origin,
            vec3d xAxis,
            vec3d yAxis
            ) : base(local)
        {
            CoordMat = Matrix44.Unit;

            CoordMat.colum0 = new vec4d(xAxis, 0);
            CoordMat.colum1 = new vec4d(yAxis, 0);
            CoordMat.colum2 = new vec4d(Linear.Normalize(Vec3d.Cross(xAxis, yAxis)), 0);

            LocalOrigin = origin;
        }

        private static PlainCoordinate statndardPlainCoordination = new PlainCoordinate(
            null, new Point(0,0,0), 
            new vec3d(1,0,0),
            new vec3d(0, 1, 0)
            );

        public static PlainCoordinate StandardPlainCoordination => statndardPlainCoordination;
    }
}
