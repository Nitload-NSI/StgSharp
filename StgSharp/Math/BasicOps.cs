using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Numerics;

namespace StgSharp.Math
{
    public static unsafe partial class Calc
    {

        public static unsafe float Abs(float x)
        {
            uint* a = (uint*)&x;
            *a &= 0b_0111_1111_1111_1111_1111_1111_1111_1111;
            return *(float*)a;

            

        }



    }
}
