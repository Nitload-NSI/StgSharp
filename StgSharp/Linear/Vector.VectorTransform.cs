using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.Math
{
    public static unsafe partial class Vector
    {
        public static vec3d To3D(this vec2d vec)
        {
            return new vec3d(vec.X, vec.Y, 0.0f);
        }

        public static  vec4d To4D(this vec2d Vec)
        {
            return new  vec4d(Vec.X, Vec.Y, 0, 0);
        }

        public static  vec4d To4D(this vec3d vec)
        {
            return new  vec4d(vec.X, vec.Y, vec.Z, 0);
        }

        public static vec2d To2D(vec3d vec, CordinatePlain plain)
        {
            switch (plain)
            {
                case CordinatePlain.XY:
                    return new vec2d(vec.X, vec.Y);
                    break;
                case CordinatePlain.YZ:
                    return new vec2d(vec.Y, vec.Z);
                    break;
                case CordinatePlain.XZ:
                    return new vec2d(vec.X, vec.Z);
                    break;
                default:
                    return default(vec2d);
                    break;
            }
        }

    }



}
