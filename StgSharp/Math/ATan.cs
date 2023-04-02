using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Math
{
    public static unsafe partial class Calc
    {
        public static unsafe float ATan(float x)
        {
            float sign = 1, angle = 0;
            if (x < 0)
            {
                sign = -1;
                x = -x;
            }
            uint xBit = *(uint*)&x;
            int xExp = (int)(xBit >> 23) - 127;
            if (xExp >= 26)
            {
                return 1;
            }
            if (xExp <= -15)
            {
                return x;
            }
            if (xExp >= 0)
            {
                x = (x - 1) / (x + 1);
                angle = 0.7853981634f;
            }

            float v = 0.7853981634f * x + x * (1 - x) * (0.2447f+0.0663f * x);
            v += angle;
            v *= sign;

            return v;


        }


        public static unsafe float ATan(float x, float y)
        {


            float z = y / x;

            float sign = 1, angle = 0,angle2 = 0;
            if (x<0)
            {
                angle = -3.1415926535f ;
            }
            if (z < 0)
            {
                sign = -1;
                z = -z;
            }
            uint zBit = *(uint*)&z;
            int zExp = (int)(zBit >> 23) - 127;
            if (zExp >= 26)
            {
                return 1;
            }
            if (zExp <= -15)
            {
                return z;
            }
            if (zExp >= 0)
            {
                z = (z - 1) / (z + 1);
                angle2 = 0.7853981634f;
            }

            float v = 0.7853981634f * z + z * (1 - z) * (0.2447f + 0.0663f * z);
            v *= 57.29577951f;
            v += angle2;
            v += angle;
            v *= sign;

            return v;

        }
    }
}
