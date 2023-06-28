using System;

namespace StgSharp.Math
{
    public static unsafe partial class Calc
    {


        public static unsafe float ACos(float x)
        {

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
                s = Calc.Sqrt(r);
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
            /**/

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
            }/**/

            v = sign * v;

            return piby2 - v;
        }
    }
}
