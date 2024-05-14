//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ASin.cs"
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


namespace StgSharp.Math
{
    public static unsafe partial class Scaler
    {

        private const float
        piby2_tail = 7.5497894159e-08f, /* 0x33a22168 */
        hpiby2_head = 7.8539812565e-01f, /* 0x3f490fda */
        piby2 = 1.5707963705e+00f; /* 0x3fc90fdb */


        /// <summary>
        /// Reurns the value of ArcSin(x)
        /// </summary>
        /// <param name="x">The sine value of the angle</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static float ASin(float x)
        {
            return MathF.Asin(x);
            /*
            int sign = 1;
            float y;

            if (x < 0)
            {
                sign = -1;
                y = -x;
            }
            else
            {
                y = x;
            }

            if (x > 1)
            {
                throw new ArgumentException();
            }

            float r, s, u, v = 0.0f;
            bool transform = (y >= 0.5f);

            if (transform)
            {
                r = 0.5f * (1.0f - y);
                s = Scaler.Sqrt(r);
                y = s;
            }
            else
            {
                r = y * y;
            }
            u = r * (0.184161606965100694821398249421F +
                   (-0.0565298683201845211985026327361F +
                   (-0.0133819288943925804214011424456F -
                   0.00396137437848476485201154797087F * r) * r) * r) /
                  (1.10496961524520294485512696706F -
                   0.836411276854206731913362287293F * r);
            /*

            if (transform)
            {
                float c, s1, p, q;
                uint us;

                us = *(uint*)&s;
                uint uss = us & 0xffff0000;
                s1 = *(float*)&uss;

                c = (r - s1 * s1) / (s + s1);
                p = 2.0F * s * u - (piby2_tail - 2.0F * c);
                q = hpiby2_head - 2.0F * s1;
                v = hpiby2_head - (p - q);
            }
            else
            {
                v = y + y * u;
            }/*

            return sign * v;
            */
        }

    }
}
