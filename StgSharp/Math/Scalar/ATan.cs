//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ATan.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
using System;
using System.Runtime.CompilerServices;

namespace StgSharp.Math
{
    public static unsafe partial class Scaler
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe float ATan(float x)
        {
            return MathF.Atan(x);
            /*
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

            float v = 0.7853981634f * x + x * (1 - x) * (0.2447f + 0.0663f * x);
            v += angle;
            v *= sign;

            return v;


        }


        public static unsafe float ATan(float x, float y)
        {


            float z = y / x;

            float sign = 1, angle = 0, angle2 = 0;
            if (x < 0)
            {
                angle = -3.1415926535f;
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
            */
        }

    }
}
