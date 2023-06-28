namespace StgSharp.Math
{
    public static unsafe partial class Calc
    {
        /// <summary>
        /// calculate the value of x^y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Returns the value of x^y</returns>
        public static unsafe float Pow(float x, float y)
        {
            /* 
             * x^y = E ^ ( log( x ) * y )
             * Now define z = | log(x) * y | = m * n  
             * and m is an integer, and 0.5 < |n| < 1
             * then E ^ z = ( E ^ n ) ^ m
             * 
             * 
             * As a float number can be expressed in (1+e)*(2^f)
             * n = (1+e)/2
             * m = 2 ^ (f + 1)
             * 
             */
            float z, t;
            if (x == 1 || y == 0)
            {
                return 1;
            }
            if (x == 1 && y == 0)
            {
                return 1.0f / 0;
            }
            z = Calc.Log(x) * y;

            bool sign = false;
            if (z < 0)
            {
                sign = true;
                z = -z;
            }

            if (z < 0.5f)
            {
                t = 1.0f + z * (
                    1.0f + z * (
                    0.5f + z * (
                    0.1666666667f + z * (
                    0.0416666666667f + z *
                    8.333333333e-3f))));
            }
            else
            {
                uint zBit = *(uint*)&z;
                uint zBitAbs = zBit & 0b_0111_1111_1111_1111_1111_1111_1111_1111;
                uint uintm = (zBitAbs >> 23);
                int m = (*(int*)&uintm) - 126;


                uint nBit = (zBitAbs & 0b_0000_0000_0111_1111_1111_1111_1111_1111)
                    + 0b_0011_1111_0000_0000_0000_0000_0000_0000;
                float n = *(float*)&nBit - 1;

                t = 2.718281828f + n * (
                    2.718281828f + n * (
                    1.359140914f + n * (
                    0.4530469714f + n * (
                    0.1132617429f + n *
                    0.02265234857f))));

                for (int i = 0; i < m; i++)
                {
                    t = t * t;
                }


            }
            if (sign)
            {
                t = 1 / t;
            }

            return t;
        }
    }
}
