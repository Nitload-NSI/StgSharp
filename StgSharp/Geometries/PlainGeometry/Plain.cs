//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Plain.cs"
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
    public unsafe class Plain
    {

        internal readonly float d = -1;
        internal vec3d origin;

        internal vec3d plainParameter;
        internal vec3d vec1;
        internal vec3d vec2;

        internal Plain(Vector4 origin, Vector4 p1, Vector4 p2)
        {
            this.origin = new vec3d(origin);

            vec1 = new vec3d(p1 - origin);
            vec2 = new vec3d(p2 - origin);
        }

        public Plain(vec3d o, vec3d v1, vec3d v2)
        {
            this.origin = o;
            this.vec1 = v1 - (v2 * (v1.Y / v2.Y));   //计算平面在xz平面上的方向投影
            this.vec2 = v2 - (vec1 * (v2.X / v1.X)); //计算平面在yz平面上的方向投影

            //|origin|-k1*|vec1|-k2*|vec2|计算plainParameter.Z；
            plainParameter.Z = origin.Z - (vec1.Z * (origin.X / vec1.X)) - (vec2.Z * (origin.Y / vec2.Y));
            //根据plainParameter.Z和对应的方向矢量计算plainParameter.X和plainParameter.Y
            plainParameter.X = vec1.X * (vec1.Z / plainParameter.Z);
            plainParameter.Y = vec2.Y * (vec2.Z / plainParameter.Z);

            plainParameter.X = 1 / plainParameter.X;
            plainParameter.Y = 1 / plainParameter.Y;
            plainParameter.Z = 1 / plainParameter.Z;
        }

        public Plain(Point p1, Point p2, Point p3)
        {
            vec3d origin = p1.Position;
            vec3d v1 = p2.Position - origin;
            vec3d v2 = p3.Position - origin;


            vec1 = v1 - (v2 * (v1.Y / v2.Y));   //计算平面在xz平面上的方向投影
            vec2 = v2 - (vec1 * (v2.X / v1.X)); //计算平面在yz平面上的方向投影

            //|origin|-k1*|vec1|-k2*|vec2|计算plainParameter.Z；
            plainParameter.Z = origin.Z - (vec1.Z * (origin.X / vec1.X)) - (vec2.Z * (origin.Y / vec2.Y));
            //根据plainParameter.Z和对应的方向矢量计算plainParameter.X和plainParameter.Y
            plainParameter.X = vec1.X * (vec1.Z / plainParameter.Z);
            plainParameter.Y = vec2.Y * (vec2.Z / plainParameter.Z);

            plainParameter.X = 1 / plainParameter.X;
            plainParameter.Y = 1 / plainParameter.Y;
            plainParameter.Z = 1 / plainParameter.Z;
        }

    }
}
