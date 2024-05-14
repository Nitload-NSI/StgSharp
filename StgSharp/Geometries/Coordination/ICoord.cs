using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Geometries
{
    public abstract class ICoord
    {
        private Matrix44 coordMat;
        private Matrix44 origin;

        public ICoord()
        {
        }

        //A matrix can convert vertex to global coord.
        public Matrix44 GlobalCoordTransformation
        {
            get 
            {
                return coordMat * origin;
            }
        }

        public vec3d Origin
        {
            get => origin.colum3.XYZ;
            internal set => origin.colum3.XYZ = value;
        }

        public vec3d X_axis
        {
            get => coordMat.colum0.XYZ;
            internal set => coordMat.colum0.XYZ = value;
        }

        public vec3d Y_axis
        {
            get => coordMat.colum1.XYZ;
            internal set => coordMat.colum1.XYZ = value;
        }

        public vec3d Z_axis
        {
            get => coordMat.colum2.XYZ;
            set => coordMat.colum2.XYZ = value;
        }

        protected ref Matrix44 CoordMat 
        {
            get => ref coordMat;
        }

        //TODO 几何体构建器需要重写
        public T BuildGeometry<T>(params vec4d[] defaultVertex) where T : IGeometry
        {
            T ret = Activator.CreateInstance<T>();
            ret.GetVertexHandle(this, ret.VertexCount);
            for (int i = 0; i < ret.VertexCount; i++)
            {
                ret[i] = defaultVertex[i];
            }
            return ret;
        }


    }
}
