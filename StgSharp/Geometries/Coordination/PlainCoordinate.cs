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
    public class PlainCoordinate : ICoord
    {

        public PlainCoordinate(
            vec3d origin,
            vec3d xAxis,
            vec3d yAxis
            )
        {
            CoordMat = Matrix44.Unit;
            Linear.Orthogonalize(ref xAxis, ref yAxis);

            CoordMat.colum0 = new vec4d(xAxis, 0);
            CoordMat.colum1 = new vec4d(yAxis, 0);
            CoordMat.colum2 = new vec4d(Vec3d.Cross(xAxis, yAxis), 0);

            Origin = origin;
        }


        public new vec3d Z_axis
        {
            get => Vec3d.Cross(X_axis, Y_axis);
            set => throw new InvalidOperationException();
        }

    }
}
