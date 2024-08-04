using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Geometries
{
    public abstract class CoordinationBase
    {
        private CoordinationBase localCoordination;
        private Matrix44 coordMat;
        private bool globalCoordAvailable;


        public CoordinationBase(
            CoordinationBase localCoordination)
        {
            this.localCoordination = localCoordination;
            coordMat = new Matrix44();
        }

        public virtual Point LocalOrigin
        {
            get => new Point( coordMat.colum3.XYZ);
            internal set => coordMat.colum3.XYZ = value.Coord;
        }

        public virtual vec3d LocalX
        {
            get => coordMat.colum0.XYZ;
            internal set => coordMat.colum0.XYZ = value;
        }

        public virtual vec3d LocalY
        {
            get => coordMat.colum1.XYZ;
            internal set => coordMat.colum1.XYZ = value;
        }

        public virtual vec3d LocalZ
        {
            get => coordMat.colum2.XYZ;
            set => coordMat.colum2.XYZ = value;
        }

        protected ref Matrix44 CoordMat 
        {
            get => ref coordMat;
        }

    }
}
