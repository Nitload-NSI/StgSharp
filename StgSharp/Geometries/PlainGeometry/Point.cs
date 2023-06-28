using StgSharp.Math;
using System;
using System.Runtime.InteropServices;


namespace StgSharp.Geometries
{
    public unsafe class Point
    {
        internal vec3d* positionPtr;
        internal readonly bool isVertex;

        internal Point(vec3d* ptr)
        {
            positionPtr = ptr;
        }

        public Point()
        {
            IntPtr ptr = Marshal.AllocHGlobal(sizeof(vec3d));
            positionPtr = (vec3d*)ptr.ToPointer();
        }

        public vec3d Position
        {
            get { return *positionPtr; }
            set { *positionPtr = value; }
        }

        public vec3d* PositionPtr
        {
            get { return positionPtr; }
            set { positionPtr = value; }
        }

        public float X
        {
            get { return (*this.positionPtr).X; }
            set { (*this.positionPtr).X = value; }
        }

        public float Y
        {
            get { return (*this.positionPtr).Y; }
            set { (*this.positionPtr).Y = value; }
        }

        public float Z
        {
            get { return (*this.positionPtr).Z; }
            set { (*this.positionPtr).Z = value; }
        }


        public bool IsVertex => isVertex;


        /// <summary>
        /// Update the coordinate of the point. If this is a moving point,
        /// (Points on sides or surface if other geometry items)
        /// please do not define any motion here, codes will be ignored by program
        /// </summary>
        /// <param name="tick">Time passes since the program begin, counted by frames</param>
        public virtual void Update(uint tick) { }

        public bool IsInGeometry(IPlainGeometry geo)
        {
            throw new NotImplementedException();
        }

        public bool IsOnPlain(IPlainGeometry geo)
        {
            return (*this.positionPtr) * geo.GetPlain().plainParameter == 1;
        }



    }
}
