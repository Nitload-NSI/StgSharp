using StgSharp.Math;
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;

namespace StgSharp.Geometries
{
    public unsafe class Point
    {
        internal vec3d position;
        internal readonly bool isVertex;

        public Point() { }

        internal Point(vec3d p)
        {
            position = p;
        }

        internal Point(Vector4 vector)
        {
            position = new vec3d(vector);
        }

        public Point(float x, float y, float z)
        {
            position = new vec3d(x, y, z);
        }

        public vec3d Position
        {
            get { return position; }
            set { position = value; }
        }

        public float X
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public float Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public float Z
        {
            get { return position.Z; }
            set { position.Z = value; }
        }


        public bool IsVertex => isVertex;


        /// <summary>
        /// Update the coordinate of the point. If this is a moving point,
        /// (Points on sides or surface if other geometry items)
        /// please do not define any motion here, codes will be ignored by program
        /// </summary>
        /// <param name="tick">Time passes since the program begin, counted by frames</param>
        public virtual void Update(uint tick) { }

        public bool IsInGeometry(PlainGeometry geo)
        {
            throw new NotImplementedException();
        }

        public bool IsOnPlain(PlainGeometry geo)
        {
            return position * geo.GetPlain().plainParameter == 1;
        }



    }
}
