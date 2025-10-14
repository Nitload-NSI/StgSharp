//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Plain"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphic;

using System.Numerics;

namespace StgSharp.Geometries
{
    public unsafe class Plain
    {

        internal readonly float d = -1;
        internal Vec3 _origin;

        internal Vec3 plainParameter;
        internal Vec3 vec1;
        internal Vec3 vec2;

        public Plain(Vec3 o, Vec3 v1, Vec3 v2)
        {
            this._origin = o;
            this.vec1 = v1 - (v2 * (v1.Y / v2.Y));   //xz
            this.vec2 = v2 - (vec1 * (v2.X / v1.X)); //yz

            // |_origin|-k1*|vec1|-k2*|vec2| --> plainParameter.Z；
            plainParameter.Z = _origin.Z - (vec1.Z * (_origin.X / vec1.X)) - (vec2.Z * (_origin.Y / vec2.Y));

            // plainParameter.Z -> plainParameter.X, plainParameter.Y
            plainParameter.X = vec1.X * (vec1.Z / plainParameter.Z);
            plainParameter.Y = vec2.Y * (vec2.Z / plainParameter.Z);

            plainParameter.X = 1 / plainParameter.X;
            plainParameter.Y = 1 / plainParameter.Y;
            plainParameter.Z = 1 / plainParameter.Z;
        }

        public Plain(Point p1, Point p2, Point p3)
        {
            Vec3 origin = p1.Coord;
            Vec3 v1 = p2.Coord - origin;
            Vec3 v2 = p3.Coord - origin;


            vec1 = v1 - (v2 * (v1.Y / v2.Y));   //xz
            vec2 = v2 - (vec1 * (v2.X / v1.X)); //yz

            // |_origin|-k1*|vec1|-k2*|vec2| --> plainParameter.Z；
            plainParameter.Z = origin.Z - (vec1.Z * (origin.X / vec1.X)) - (vec2.Z * (origin.Y / vec2.Y));

            // plainParameter.Z --> plainParameter.X, plainParameter.Y
            plainParameter.X = vec1.X * (vec1.Z / plainParameter.Z);
            plainParameter.Y = vec2.Y * (vec2.Z / plainParameter.Z);

            plainParameter.X = 1 / plainParameter.X;
            plainParameter.Y = 1 / plainParameter.Y;
            plainParameter.Z = 1 / plainParameter.Z;
        }

    }
}
