//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Cordinate.cs"
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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Geometries
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PartialPlainCoordinate
    {

        internal Matrix32 axis;

        /// <summary>
        /// The origin of this coordinate.
        /// </summary>
        public vec3d origin;

        public PartialPlainCoordinate
            (
            vec3d origin,
            vec3d horiziontial,
            vec3d vertical
            )
        {
            this.origin = origin;
            axis = new Matrix32();
#if NET5_0_OR_GREATER
            Unsafe.SkipInit(out axis.mat);
#endif
            axis.colum0 = horiziontial;
            axis.colum1 = vertical;
        }

        /// <summary>
        /// The horizontial axis of the coordinate.
        /// Point to the right direction.
        /// </summary>
        public vec3d axis_H => axis.colum0;

        /// <summary>
        /// The vertical axis of the coordinate.
        /// Point to the top direction.
        /// </summary>
        public vec3d axis_V => axis.colum1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public vec3d GetRealLocate(vec2d raletiveLocation)
        {
            return origin + axis.VerticalVecMultiply(raletiveLocation);
        }

    }
}
