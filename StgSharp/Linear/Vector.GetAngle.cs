using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Math
{
    public static unsafe partial class Vector
    {
        public static float GetAngle(vec2d vec1, vec2d vec2)
        {
            return Calc.ACos(
                vec1 .multiple(vec2) 
                / (vec1.GetLength() * vec2.GetLength())
                );
        }

        public static float GetAngle(vec3d vec1, vec3d vec2)
        {
            return Calc.ACos(
                vec1 * vec2
                / (vec1.GetLength() * vec2.GetLength())
                );
        }

    }
}
