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
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;

namespace StgSharp.Geometries
{
    public unsafe class Point
    {

        internal readonly bool isVertex;
        internal vec3d position;

        internal Point(vec3d p)
        {
            position = p;
        }

        internal Point(Vector4 vector)
        {
            position = new vec3d(vector);
        }

        public Point() { }

        public Point(float x, float y, float z)
        {
            position = new vec3d(x, y, z);
        }


        public bool IsVertex => isVertex;

        public vec3d Position
        {
            get => position;
            set => position = value;
        }

        public float X
        {
            get => position.X;
            set => position.X = value;
        }

        public float Y
        {
            get => position.Y;
            set => position.Y = value;
        }

        public float Z
        {
            get => position.Z;
            set => position.Z = value;
        }

        public bool IsInGeometry(PlainGeometry geo)
        {
            throw new NotImplementedException();
        }

        public bool IsOnPlain(PlainGeometry geo)
        {
            return position * geo.GetPlain().plainParameter == 1;
        }


        /// <summary>
        /// Update the coordinate of the point. If this is a moving point,
        /// (Points on sides or surface if other geometry items)
        /// please do not define any motion here, codes will be ignored by program
        /// </summary>
        /// <param name="tick">Time passes since the program begin, counted by frames</param>
        public virtual void Update(uint tick) { }



    }
}
