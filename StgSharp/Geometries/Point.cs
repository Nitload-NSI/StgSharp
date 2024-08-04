using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Geometries
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct Point
    {
        [FieldOffset(0)] private vec3d coord;
        [FieldOffset(0)] internal Vector4 coordVec;

        public Point(float x, float y, float z)
        {
            coordVec = new Vector4(x,y,z,1);
        }

        public Point(vec3d coord)
        {
            Coord = coord;
            coordVec.W = 1;
        }

        internal Point(vec4d vector)
        {
            coordVec = vector.vec;
            coordVec.W = 1;
        }

        public vec3d Coord
        {
            get => coord;
            set { coord = value; coordVec.W = 1; }
        }



    }
}
