//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Rectangle.cs"
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

using System.Numerics;

namespace StgSharp.Geometries
{
    public class Rectangle : Parallelogram
    {

        public Rectangle(
            float v0x, float v0y, float v0z,
            float v1x, float v1y, float v1z,
            float v2x, float v2y, float v2z,
            float v3x, float v3y, float v3z
            ) : base(
                v0x, v0y, v0z,
                v1x, v1y, v1z,
                v2x, v2y, v2z,
                v3x, v3y, v3z
                )
        {
#if DEBUG
            vec2d side0 = new vec2d( vertexMat.colum0 - vertexMat.colum1);
            vec2d side1 = new vec2d( vertexMat.colum2 - vertexMat.colum1);
            if (side0.Cross(side1) == 0)
            {
                throw new ArgumentException();
            }
#endif
        }

        /// <summary>
        /// Calculate movement to center
        /// </summary>
        /// <param name="tick">Current time tick</param>
        /// <returns></returns>
        public virtual vec3d MovCenter(uint tick)
        {
            return default(vec3d);
        }



    }
}
