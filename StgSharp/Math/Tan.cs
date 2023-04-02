using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Math
{
    public static partial class Calc
    {

        public static float Tan(float x)
        {
            float refer = x / 360.0f;
            refer -= (int)refer;

            refer *= 40;
            int a = (int)refer;   //大分度，查表
            refer -= a;    //小分度，拟合

            float refer2 = refer * refer;

            float sin = (0.1570796327f * refer - 6.459640975e-4f * refer * refer2);
            float cos = (1 - 0.0123370055f * refer2 + 2.536695709e-5f * refer2 * refer2);

            return ( sinData[a] * cos + cosData[a] * sin )
                / ( cosData[a] * cos - sinData[a] * sin );
        }
    }
}
