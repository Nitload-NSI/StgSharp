using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Math
{
    public static unsafe partial class Vector
    {

        public static float Det(matrix2x2 array)
        {
            return Cross(array.row1,array.row2);
        }

        public static unsafe float Det(matrix3x3 array)
        {
            float* p = &array.row1.X;

            matrix2x2 matrix1 = new matrix2x2(
                *(p + 4), *(p + 5),
                *(p + 7), *(p + 8)
                );
            matrix2x2 matrix2 = new matrix2x2(
                *(p + 3), *(p + 5),
                *(p + 6), *(p + 8)
                );
            matrix2x2 matrix3 = new matrix2x2(
                *(p + 3), *(p + 4),
                *(p + 6), *(p + 7)
                );

            return 
                *p * Det(matrix1)
                - *(p + 1) * Det(matrix2)
                + *(p + 2) * Det(matrix3)
                ;
        }
    }
}
