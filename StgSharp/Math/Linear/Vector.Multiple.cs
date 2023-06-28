namespace StgSharp.Math
{
    public static unsafe partial class VectorCalc
    {
        private static float getMatrixMember(float* p1, float* p2)
        {
            float result = 0;

            for (int i = 0; i < 4; i++)
            {
                result += *p1 * *p2;
                p1 += 1;
                p2 += 3;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colum"></param>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static matrix2x2 multiple(this colum2 colum, vec2d vec)
        {
            return new matrix2x2(
                colum.X * vec.X, colum.X * vec.Y,
                colum.Y * vec.X, colum.Y * vec.Y
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vec1"></param>
        /// <param name="vec2"></param>
        /// <returns></returns>
        public static float multiple(this vec2d vec1, vec2d vec2)
        {
            return vec1.X * vec2.X + vec1.Y * vec2.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="colum"></param>
        /// <returns></returns>
        public static float multiple(this vec2d vec, colum2 colum)
        {
            return vec.X * colum.X + vec.Y * colum.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static unsafe vec2d multiple(this vec2d vec, matrix2x2 matrix)
        {
            return new vec2d(
                vec.X * matrix.row1.X + vec.Y * matrix.row2.X,
                vec.X * matrix.row1.Y + vec.Y * matrix.row2.Y
                );
        }

        public static float multiple(this vec3d vec, colum3 colum)
        {
            return vec.X * colum.X + vec.Y * colum.Y + vec.Z * colum.Z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static unsafe vec3d multiple(this vec2d vec, matrix2x3 matrix)
        {
            float* p = &matrix.row1.X;

            return new vec3d(
                vec.X * *(p + 0) + vec.Y * *(p + 3),
                vec.X * *(p + 1) + vec.Y * *(p + 4),
                vec.X * *(p + 2) + vec.Y * *(p + 5)
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static unsafe vec4d multiple(this vec2d vec, matrix2x4 matrix)
        {
            float* p = &matrix.row1.X;

            return new vec4d(
                vec.X * *(p + 0) + vec.Y * *(p + 4),
                vec.X * *(p + 1) + vec.Y * *(p + 5),
                vec.X * *(p + 2) + vec.Y * *(p + 6),
                vec.X * *(p + 3) + vec.Y * *(p + 7)
                );
        }

        public static unsafe float multiple(this vec4d vec, colum4 colum)
        {
            return vec.X * colum.X + vec.Y * colum.Y + vec.Z * colum.W + vec.W * colum.W;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="colum"></param>
        /// <returns></returns>
        public static unsafe colum2 multiple(this matrix2x2 matrix, colum2 colum)
        {
            float* p1 = &matrix.row1.X;
            float* p2 = &colum.X;

            return new colum2(
                *(p1 + 0) * *(p2 + 0) + *(p1 + 1) * *(p2 + 1),
                *(p1 + 2) * *(p2 + 0) + *(p1 + 3) * *(p2 + 1)
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static unsafe matrix2x2 multiple(this matrix2x2 matrix1, matrix2x2 matrix2)
        {
            float* a11 = &matrix1.row1.X;
            float* a12 = &matrix1.row1.Y;
            float* a21 = &matrix1.row2.X;
            float* a22 = &matrix1.row2.Y;

            float* b11 = &matrix2.row1.X;
            float* b12 = &matrix2.row1.Y;
            float* b21 = &matrix2.row2.X;
            float* b22 = &matrix2.row2.Y;

            var s1 = *a21 + *a22;
            var s2 = s1 - *a11;
            var s3 = *a11 - *a21;
            var s4 = *a12 - s2;

            var t1 = *b12 - *b11;
            var t2 = *b22 - t1;
            var t3 = *b22 - *b12;
            var t4 = t2 - *b21;

            var m1 = *a11 * *b11;
            var m2 = *a12 * *b21;
            var m3 = s4 * *b22;
            var m4 = *a22 * t4;
            var m5 = s1 * t1;
            var m6 = s2 * t2;
            var m7 = s3 * t3;

            var u1 = m1 + m2;
            var u2 = m1 + m6;
            var u3 = u2 + m7;
            var u4 = u2 + m5;
            var u5 = u4 + m3;
            var u6 = u3 - m4;
            var u7 = u3 + m5;

            return new matrix2x2(u1, u5, u6, u7);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static unsafe matrix2x3 multiple(this matrix2x2 matrix1, matrix2x3 matrix2)

        {
            float* p1 = &matrix1.row1.X;
            float* p2 = &matrix2.row1.X;

            float a11 = *p1;
            float a12 = *(p1 + 1);
            float a21 = *(p1 + 2);
            float a22 = *(p1 + 3);

            vec2d b11 = new vec2d(*(p2 + 0), *(p2 + 1));
            vec2d b12 = new vec2d(*(p2 + 2), 0);
            vec2d b21 = new vec2d(*(p2 + 3), *(p2 + 4));
            vec2d b22 = new vec2d(*(p2 + 5), 0);

            var s1 = a21 + a22;
            var s2 = s1 - a11;
            var s3 = a11 - a21;
            var s4 = a12 - s2;

            var t1 = b12 - b11;
            var t2 = b22 - t1;
            var t3 = b22 - b12;
            var t4 = t2 - b21;

            var m1 = b11 * a11;
            var m2 = b21 * a12;
            var m3 = b22 * s4;
            var m4 = t4 * a22;
            var m5 = t1 * s1;
            var m6 = t2 * s2;
            var m7 = t3 * s3;

            var u1 = m1 + m2;
            var u2 = m1 + m6;
            var u3 = u2 + m7;
            var u4 = u2 + m5;
            var u5 = u4 + m3;
            var u6 = u3 - m4;
            var u7 = u3 + m5;

            return new matrix2x3(
                u1.X, u1.Y, u5.X,
                u6.X, u6.Y, u7.X
                );

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static unsafe matrix2x4 multiple(this matrix2x2 matrix1, matrix2x4 matrix2)
        {
            float* p1 = &matrix1.row1.X;
            float* p2 = &matrix2.row1.X;

            float a11 = *p1;
            float a12 = *(p1 + 1);
            float a21 = *(p1 + 2);
            float a22 = *(p1 + 3);

            vec2d b11 = new vec2d(*(p2 + 0), *(p2 + 1));
            vec2d b12 = new vec2d(*(p2 + 2), *(p2 + 3));
            vec2d b21 = new vec2d(*(p2 + 4), *(p2 + 5));
            vec2d b22 = new vec2d(*(p2 + 6), *(p2 + 7));

            var s1 = a21 + a22;
            var s2 = s1 - a11;
            var s3 = a11 - a21;
            var s4 = a12 - s2;

            var t1 = b12 - b11;
            var t2 = b22 - t1;
            var t3 = b22 - b12;
            var t4 = t2 - b21;

            var m1 = b11 * a11;
            var m2 = b21 * a12;
            var m3 = b22 * s4;
            var m4 = t4 * a22;
            var m5 = t1 * s1;
            var m6 = t2 * s2;
            var m7 = t3 * s3;

            var u1 = m1 + m2;
            var u2 = m1 + m6;
            var u3 = u2 + m7;
            var u4 = u2 + m5;
            var u5 = u4 + m3;
            var u6 = u3 - m4;
            var u7 = u3 + m5;

            return new matrix2x4(
                u1.X, u1.Y, u5.X, u5.Y,
                u6.X, u6.Y, u7.X, u7.Y
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static unsafe colum2 multiple(this matrix2x3 matrix, colum3 vec)
        {
            return new colum2(
                matrix.row1.X * vec.X + matrix.row1.Y * vec.Y + matrix.row1.Z * vec.Z,
                matrix.row2.X * vec.X + matrix.row2.Y * vec.Y + matrix.row2.Z * vec.Z
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static unsafe matrix2x2 multiple(this matrix2x3 matrix1, matrix3x2 matrix2)
        {
            float* p1 = &matrix1.row1.X;
            float* p2 = &matrix2.row1.X;

            matrix2x2 a1 = new matrix2x2(*(p1 + 0), *(p1 + 1), *(p1 + 3), *(p1 + 4));
            colum2 a2 = new colum2(*(p1 + 2), *(p1 + 5));

            matrix2x2 b1 = new matrix2x2(*(p2 + 0), *(p2 + 1), *(p2 + 2), *(p2 + 3));
            vec2d b2 = new vec2d(*(p2 + 4), *(p2 + 5));

            return a1.multiple(b1) + a2.multiple(b2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static unsafe matrix2x3 multiple(this matrix2x3 matrix1, matrix3x3 matrix2)
        {
            float* p1 = &matrix1.row1.X;
            float* p2 = &matrix2.row1.X;

            var a1 = new matrix2x2(*(p1 + 0), *(p1 + 1), *(p1 + 3), *(p1 + 4));
            var a2 = new colum2(*(p1 + 2), *(p1 + 5));

            var b11 = new matrix2x2(*(p2 + 0), *(p2 + 1), *(p2 + 3), *(p2 + 4));
            var b12 = new colum2(*(p2 + 2), *(p2 + 5));
            var b21 = new vec2d(*(p2 + 6), *(p2 + 7));
            var b22 = *(p2 + 8);

            var c1 = a1.multiple(b11) + a2.multiple(b21);
            var c2 = a1.multiple(b12) + a2 * b22;

            return new matrix2x3(
                c1.row1.X, c1.row1.Y, c2.X,
                c1.row2.X, c1.row2.Y, c2.Y
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static unsafe matrix2x4 multiple(this matrix2x3 matrix1, matrix3x4 matrix2)
        {
            float* p1 = &matrix1.row1.X;
            float* p2 = &matrix2.row1.X;

            var a1 = new matrix2x2(*(p1 + 0), *(p1 + 1), *(p1 + 3), *(p1 + 4));
            var a2 = new colum2(*(p1 + 2), *(p1 + 5));

            var b11 = new matrix2x2(*(p2 + 0), *(p2 + 1), *(p2 + 4), *(p2 + 5));
            var b12 = new matrix2x2(*(p2 + 2), *(p2 + 3), *(p2 + 6), *(p2 + 7));
            var b21 = new vec2d(*(p2 + 8), *(p2 + 9));
            var b22 = new vec2d(*(p2 + 10), *(p2 + 11));

            var c1 = a1.multiple(b11) + a2.multiple(b21);
            var c2 = a1.multiple(b12) + a2.multiple(b22);

            return new matrix2x4(
                new vec4d(c1.row1, c2.row1),
                new vec4d(c1.row2, c2.row2)
                );
        }

        public static unsafe colum2 multiple(this matrix2x4 matrix, colum4 colum)
        {
            return new colum2(
                matrix.row1.multiple(colum),
                matrix.row2.multiple(colum)
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static unsafe matrix2x2 multiple(this matrix2x4 matrix1, matrix4x2 matrix2)
        {
            float* p1 = &matrix1.row1.X;
            float* p2 = &matrix2.row1.X;

            vec2d a11 = new vec2d(*(p1 + 0), *(p1 + 1));
            vec2d a12 = new vec2d(*(p1 + 2), *(p1 + 3));
            vec2d a21 = new vec2d(*(p1 + 4), *(p1 + 5));
            vec2d a22 = new vec2d(*(p1 + 6), *(p1 + 7));

            colum2 b11 = new colum2(*(p2 + 0), *(p2 + 2));
            colum2 b12 = new colum2(*(p2 + 1), *(p2 + 3));
            colum2 b21 = new colum2(*(p2 + 4), *(p2 + 6));
            colum2 b22 = new colum2(*(p2 + 5), *(p2 + 7));

            var s1 = a21 + a22;
            var s2 = s1 - a11;
            var s3 = a11 - a21;
            var s4 = a12 - s2;

            var t1 = b12 - b11;
            var t2 = b22 - t1;
            var t3 = b22 - b12;
            var t4 = t2 - b21;

            var m1 = a11.multiple(b11);
            var m2 = a12.multiple(b21);
            var m3 = s4.multiple(b22);
            var m4 = a22.multiple(t4);
            var m5 = s1.multiple(t1);
            var m6 = s2.multiple(t2);
            var m7 = s3.multiple(t3);

            var u1 = m1 + m2;
            var u2 = m1 + m6;
            var u3 = u2 + m7;
            var u4 = u2 + m5;
            var u5 = u4 + m3;
            var u6 = u3 - m4;
            var u7 = u3 + m5;

            return new matrix2x2(u1, u5, u6, u7);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static unsafe matrix2x3 multiple(this matrix2x4 matrix1, matrix4x3 matrix2)
        {
            float* p1 = &matrix1.row1.X;
            float* p2 = &matrix2.row1.X;

            return new matrix2x3(
                getMatrixMember(p1, p2), getMatrixMember(p1, p2 + 1), getMatrixMember(p1, p2 + 2),
                getMatrixMember(p1 + 4, p2), getMatrixMember(p1 + 4, p2 + 1), getMatrixMember(p1 + 4, p2 + 2)
                );


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static unsafe matrix2x4 multiple(this matrix2x4 matrix1, matrix4x4 matrix2)
        {

            matrix2x2 a1 = new matrix2x2(matrix1.row1.Point1, matrix1.row2.Point1);
            matrix2x2 a2 = new matrix2x2(matrix1.row1.Point2, matrix1.row2.Point2);

            matrix2x2 b11 = new matrix2x2(matrix2.row1.Point1, matrix2.row2.Point1);
            matrix2x2 b12 = new matrix2x2(matrix2.row1.Point2, matrix2.row2.Point2);
            matrix2x2 b21 = new matrix2x2(matrix2.row3.Point1, matrix2.row4.Point1);
            matrix2x2 b22 = new matrix2x2(matrix2.row3.Point2, matrix2.row4.Point2);

            var c1 = a1.multiple(b11) + a2.multiple(b21);
            var c2 = a1.multiple(b12) + a2.multiple(b22);

            return new matrix2x4(
                new vec4d(c1.row1, c2.row1),
                new vec4d(c1.row2, c2.row2));
        }


        public static unsafe colum3 multiple(this matrix3x2 matrix, colum2 colum)
        {
            return new colum3(
                matrix.row1.multiple(colum),
                matrix.row1.multiple(colum),
                matrix.row1.multiple(colum)
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static unsafe matrix3x2 multiple(this matrix3x2 matrix1, matrix2x2 matrix2)
        {
            var a1 = new matrix2x2(matrix1.row1, matrix1.row2);
            var a2 = matrix1.row3;

            var c1 = a1.multiple(matrix2);

            return new matrix3x2(
                c1.row1, c1.row2, a2.multiple(matrix2)
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static unsafe matrix3x3 multiple(this matrix3x2 matrix1, matrix2x3 matrix2)
        {
            float* p1 = &matrix1.row1.X;
            float* p2 = &matrix2.row1.X;

            matrix2x2 a1 = new matrix2x2(*(p1 + 0), *(p1 + 1), *(p1 + 2), *(p1 + 3));
            vec2d a2 = new vec2d(*(p1 + 4), *(p1 + 5));

            matrix2x2 b1 = new matrix2x2(*(p2 + 0), *(p2 + 1), *(p2 + 3), *(p2 + 4));
            colum2 b2 = new colum2(*(p2 + 2), *(p2 + 5));

            var m11 = a1.multiple(b1);
            var m12 = a1.multiple(b2);
            var m21 = a2.multiple(b1);
            var m22 = a2.multiple(b2);

            float* p3 = &m11.row1.X;

            return new matrix3x3(
                *(p3 + 0), *(p3 + 1), m12.X,
                *(p3 + 2), *(p3 + 3), m12.Y,
                m21.X, m21.Y, m22
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static unsafe matrix3x4 multiple(this matrix3x2 matrix1, matrix2x4 matrix2)
        {
            var a1 = new matrix2x2(matrix1.row1, matrix1.row2);
            var a2 = matrix1.row3;

            var b1 = new matrix2x2(matrix2.row1.Point1, matrix2.row2.Point1);
            var b2 = new matrix2x2(matrix2.row1.Point2, matrix2.row2.Point2);

            var c11 = a1.multiple(b1);
            var c12 = a1.multiple(b2);
            var c21 = a2.multiple(b1);
            var c22 = a2.multiple(b2);

            return new matrix3x4(
                new vec4d(c11.row1, c12.row1),
                new vec4d(c11.row2, c12.row2),
                new vec4d(c21, c22)
                );
        }

        public static unsafe colum3 multiple(this matrix3x3 matrix, colum3 colum)
        {
            return new colum3(
                matrix.row1.multiple(colum),
                matrix.row2.multiple(colum),
                matrix.row3.multiple(colum)
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static unsafe matrix3x2 multiple(this matrix3x3 matrix1, matrix3x2 matrix2)
        {
            float* p1 = &matrix1.row1.X;

            var a11 = new matrix2x2(*(p1 + 0), *(p1 + 1), *(p1 + 3), *(p1 + 4));
            var a12 = new colum2(*(p1 + 2), *(p1 + 5));
            var a21 = new vec2d(*(p1 + 6), *(p1 + 7));
            var a22 = *(p1 + 8);

            var b1 = new matrix2x2(matrix2.row1, matrix2.row2);
            var b2 = matrix2.row3;

            var c1 = a11.multiple(b1) + a12.multiple(b2);
            var c2 = a21.multiple(b1) + b2 * a22;

            return new matrix3x2(c1.row1, c1.row2, c2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static unsafe matrix3x3 multiple(this matrix3x3 matrix1, matrix3x3 matrix2)
        {
            float* p1 = &matrix1.row1.X;
            float* p2 = &matrix2.row1.X;

            matrix2x2 a11 = new matrix2x2(*(p1 + 0), *(p1 + 1), *(p1 + 3), *(p1 + 4));
            colum2 a12 = new colum2(*(p1 + 2), *(p1 + 5));
            vec2d a21 = new vec2d(*(p1 + 6), *(p1 + 7));
            float a22 = *(p1 + 8);

            matrix2x2 b11 = new matrix2x2(*(p2 + 0), *(p2 + 1), *(p2 + 3), *(p2 + 4));
            colum2 b12 = new colum2(*(p2 + 2), *(p2 + 5));
            vec2d b21 = new vec2d(*(p2 + 6), *(p2 + 7));
            float b22 = *(p2 + 8);

            var c11 = a11.multiple(b11) + a12.multiple(b21);
            var c12 = a11.multiple(b12) + a12 * b22;
            var c21 = a21.multiple(b11) + b21 * a22;
            var c22 = a21.multiple(b12) + a22 * b22;

            float* p = &(c11.row1.X);

            return new matrix3x3(
                *(p + 0), *(p + 1), c12.X,
                *(p + 2), *(p + 3), c12.Y,
                c21.X, c21.Y, c22
                );
        }

        public static unsafe matrix3x4 multiple(this matrix3x3 matrix1, matrix3x4 matrix2)
        {
            float* p1 = &matrix1.row1.X;
            float* p2 = &matrix2.row1.X;

            var a11 = new matrix2x2(*(p1 + 0), *(p1 + 1), *(p1 + 3), *(p1 + 4));
            var a12 = new colum2(*(p1 + 2), *(p1 + 5));
            var a21 = new vec2d(*(p1 + 6), *(p1 + 7));
            var a22 = *(p1 + 8);

            var b11 = new matrix2x2(matrix2.row1.Point1, matrix2.row2.Point1);
            var b12 = new matrix2x2(matrix2.row1.Point2, matrix2.row2.Point2);
            var b21 = matrix2.row3.Point1;
            var b22 = matrix2.row3.Point2;

            var c11 = a11.multiple(b11) + a12.multiple(b21);
            var c12 = a11.multiple(b12) + a12.multiple(b22);
            var c21 = a21.multiple(b11) + b21 * a22;
            var c22 = a21.multiple(b12) + b22 * a22;


            return new matrix3x4(
                new vec4d(c11.row1, c12.row1),
                new vec4d(c11.row2, c12.row2),
                new vec4d(c21, c22)
                );

        }

        public static unsafe colum3 multiple(this matrix3x4 matrix, colum4 colum)
        {
            return new colum3(
                matrix.row1.multiple(colum),
                matrix.row2.multiple(colum),
                matrix.row3.multiple(colum)
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static unsafe matrix3x2 multiple(this matrix3x4 matrix1, matrix4x2 matrix2)
        {
            float* p1 = &matrix1.row1.X;
            float* p2 = &matrix2.row1.X;

            matrix2x2 a11 = new matrix2x2(*(p1 + 0), *(p1 + 1), *(p1 + 4), *(p1 + 5));
            matrix2x2 a12 = new matrix2x2(*(p1 + 2), *(p1 + 3), *(p1 + 6), *(p1 + 7));
            vec2d a21 = new vec2d(*(p1 + 8), *(p1 + 9));
            vec2d a22 = new vec2d(*(p1 + 10), *(p1 + 11));

            matrix2x2 b1 = new matrix2x2(*(p2 + 0), *(p2 + 1), *(p2 + 2), *(p2 + 3));
            matrix2x2 b2 = new matrix2x2(*(p2 + 4), *(p2 + 5), *(p2 + 6), *(p2 + 7));

            var c1 = a11.multiple(b1) + a12.multiple(b2);
            var c2 = a21.multiple(b1) + a22.multiple(b2);

            return new matrix3x2(c1.row1, c1.row2, c2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static unsafe matrix3x3 multiple(this matrix3x4 matrix1, matrix4x3 matrix2)
        {
            float* p1 = &matrix1.row1.X;
            float* p2 = &matrix2.row1.X;

            matrix2x2 a11 = new matrix2x2(
                *(p1 + 0), *(p1 + 1), *(p1 + 4), *(p1 + 5)
                );
            matrix2x2 a12 = new matrix2x2(
                *(p1 + 2), *(p1 + 3), *(p1 + 6), *(p1 + 7)
                );
            vec2d a21 = new vec2d(*(p1 + 8), *(p1 + 9));
            vec2d a22 = new vec2d(*(p1 + 10), *(p1 + 11));

            matrix2x2 b11 = new matrix2x2(
                *(p2 + 0), *(p2 + 1), *(p2 + 3), *(p2 + 4)
                );
            colum2 b12 = new colum2(*(p2 + 2), *(p2 + 5));
            matrix2x2 b21 = new matrix2x2(
                *(p2 + 6), *(p2 + 7), *(p2 + 9), *(p2 + 10)
                );
            colum2 b22 = new colum2(*(p2 + 8), *(p2 + 11));

            var c11 = a11.multiple(b11) + a12.multiple(b21);
            var c12 = a11.multiple(b12) + a12.multiple(b22);
            var c21 = a21.multiple(b11) + a22.multiple(b21);
            var c22 = a21.multiple(b12) + a22.multiple(b22);

            float* pc1 = &c11.row1.X;

            return new matrix3x3(
                *(pc1 + 0), *(pc1 + 1), c12.X,
                *(pc1 + 2), *(pc1 + 3), c12.Y,
                c21.X, c21.Y, c22
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static unsafe matrix3x4 multiple(this matrix3x4 matrix1, matrix4x4 matrix2)
        {
            float* p1 = &matrix1.row1.X;
            float* p2 = &matrix2.row1.X;

            var a11 = new matrix2x2(matrix1.row1.Point1, matrix1.row2.Point1);
            var a12 = new matrix2x2(matrix1.row1.Point2, matrix1.row2.Point2);
            var a21 = matrix1.row3.Point1;
            var a22 = matrix1.row3.Point2;

            var b11 = new matrix2x2(matrix2.row1.Point1, matrix2.row2.Point1);
            var b12 = new matrix2x2(matrix2.row1.Point2, matrix2.row2.Point2);
            var b21 = new matrix2x2(matrix2.row3.Point1, matrix2.row4.Point1);
            var b22 = new matrix2x2(matrix2.row3.Point2, matrix2.row4.Point2);

            var c1 = a11.multiple(b11) + a12.multiple(b21);
            var c2 = a11.multiple(b12) + a12.multiple(b22);
            var c3 = a21.multiple(b11) + a22.multiple(b21);
            var c4 = a21.multiple(b12) + a22.multiple(b22);

            return new matrix3x4(
                new vec4d(c1.row1, c2.row1),
                new vec4d(c1.row2, c2.row2),
                new vec4d(c3, c4)
                );
        }


        public static unsafe matrix4x2 multiple(this matrix4x2 matrix1, matrix2x2 matrix2)
        {
            float* p = &matrix1.row1.X;
            var a11 = new colum2(*(p + 0), *(p + 2));
            var a12 = new colum2(*(p + 1), *(p + 3));
            var a21 = new colum2(*(p + 4), *(p + 6));
            var a22 = new colum2(*(p + 5), *(p + 7));

            var b11 = matrix2.row1.X;
            var b12 = matrix2.row1.Y;
            var b21 = matrix2.row2.X;
            var b22 = matrix2.row2.Y;

            var s1 = a21 + a22;
            var s2 = s1 - a11;
            var s3 = a11 - a21;
            var s4 = a12 - s2;

            var t1 = b12 - b11;
            var t2 = b22 - t1;
            var t3 = b22 - b12;
            var t4 = t2 - b21;

            var m1 = a11 * b11;
            var m2 = a12 * b21;
            var m3 = s4 * b22;
            var m4 = a22 * t4;
            var m5 = s1 * t1;
            var m6 = s2 * t2;
            var m7 = s3 * t3;

            var u1 = m1 + m2;
            var u2 = m1 + m6;
            var u3 = u2 + m7;
            var u4 = u2 + m5;
            var u5 = u4 + m3;
            var u6 = u3 - m4;
            var u7 = u3 + m5;

            return new matrix4x2(
                new vec2d(u1.X, u5.X),
                new vec2d(u1.Y, u5.Y),
                new vec2d(u6.X, u7.X),
                new vec2d(u6.Y, u7.Y)
                );
        }

        public static unsafe matrix4x3 multiple(this matrix4x2 matrix1, matrix2x3 matrix2)
        {
            float* p = &matrix2.row1.X;

            var a1 = new matrix2x2(matrix1.row1, matrix1.row2);
            var a2 = new matrix2x2(matrix1.row3, matrix1.row4);

            var b1 = new matrix2x2(*(p + 0), *(p + 1), *(p + 3), *(p + 4));
            var b2 = new colum2(*(p + 2), *(p + 5));

            var c11 = a1.multiple(b1);
            var c12 = a1.multiple(b2);
            var c21 = a2.multiple(b1);
            var c22 = a2.multiple(b2);

            float* pc1 = &c11.row1.X;
            float* pc2 = &c21.row1.X;

            return new matrix4x3(
                *(pc1 + 0), *(pc1 + 1), c12.X,
                *(pc1 + 2), *(pc1 + 3), c12.Y,
                *(pc2 + 0), *(pc2 + 1), c22.X,
                *(pc2 + 2), *(pc2 + 3), c22.Y
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static unsafe matrix4x4 multiple(this matrix4x2 matrix1, matrix2x4 matrix2)
        {
            float* p1 = &matrix1.row1.X;
            float* p2 = &matrix2.row1.X;

            colum2 a11 = new colum2(*(p1 + 0), *(p1 + 2));
            colum2 a12 = new colum2(*(p1 + 1), *(p1 + 3));
            colum2 a21 = new colum2(*(p1 + 4), *(p1 + 6));
            colum2 a22 = new colum2(*(p1 + 5), *(p1 + 7));

            vec2d b11 = new vec2d(*(p2 + 0), *(p2 + 1));
            vec2d b12 = new vec2d(*(p2 + 2), *(p2 + 3));
            vec2d b21 = new vec2d(*(p2 + 4), *(p2 + 5));
            vec2d b22 = new vec2d(*(p2 + 6), *(p2 + 7));

            var s1 = a21 + a22;
            var s2 = s1 - a11;
            var s3 = a11 - a21;
            var s4 = a12 - s2;

            var t1 = b12 - b11;
            var t2 = b22 - t1;
            var t3 = b22 - b12;
            var t4 = t2 - b21;

            var m1 = a11.multiple(b11);
            var m2 = a12.multiple(b21);
            var m3 = s4.multiple(b22);
            var m4 = a22.multiple(t4);
            var m5 = s1.multiple(t1);
            var m6 = s2.multiple(t2);
            var m7 = s3.multiple(t3);

            var u1 = m1 + m2;
            var u2 = m1 + m6;
            var u3 = u2 + m7;
            var u4 = u2 + m5;
            var u5 = u4 + m3;
            var u6 = u3 - m4;
            var u7 = u3 + m5;

            return new matrix4x4(
                new vec4d(u1.row1.X, u1.row1.Y, u5.row1.X, u5.row1.Y),
                new vec4d(u1.row2.X, u1.row2.Y, u5.row2.X, u5.row2.Y),
                new vec4d(u6.row1.X, u6.row1.Y, u7.row1.X, u7.row1.Y),
                new vec4d(u6.row2.X, u6.row2.Y, u7.row2.X, u7.row2.Y)
                );
        }

        public static unsafe matrix4x2 multiple(this matrix4x3 matrix1, matrix3x2 matrix2)
        {
            float* p1 = &matrix1.row1.X;
            float* p2 = &matrix2.row1.X;

            var a11 = new matrix2x2(*(p1 + 0), *(p1 + 1), *(p1 + 3), *(p1 + 4));
            var a12 = new colum2(*(p1 + 2), *(p1 + 5));
            var a21 = new matrix2x2(*(p1 + 6), *(p1 + 7), *(p1 + 9), *(p1 + 10));
            var a22 = new colum2(*(p1 + 8), *(p1 + 11));

            var b1 = new matrix2x2(*(p2 + 0), *(p2 + 0), *(p2 + 0), *(p2 + 0));
            var b2 = matrix2.row3;

            var c1 = a11.multiple(b1) + a12.multiple(b2);
            var c2 = a21.multiple(b1) + a22.multiple(b2);

            return new matrix4x2(c1.row1, c1.row2, c2.row1, c2.row2);

        }

        public static unsafe matrix4x3 multiple(this matrix4x3 matrix1, matrix3x3 matrix2)
        {
            float* p1 = &matrix1.row1.X;
            float* p2 = &matrix2.row1.X;

            var a11 = new matrix2x2(*(p1 + 0), *(p1 + 1), *(p1 + 3), *(p1 + 4));
            var a12 = new colum2(*(p1 + 2), *(p1 + 5));
            var a21 = new matrix2x2(*(p1 + 6), *(p1 + 7), *(p1 + 9), *(p1 + 10));
            var a22 = new colum2(*(p1 + 8), *(p1 + 11));

            var b11 = new matrix2x2(*(p2 + 0), *(p2 + 1), *(p2 + 3), *(p2 + 4));
            var b12 = new colum2(*(p2 + 2), *(p2 + 5));
            var b21 = new vec2d(*(p2 + 9), *(p2 + 10));
            var b22 = matrix2.row3.Z;

            var c11 = a11.multiple(b11) + a12.multiple(b21);
            var c12 = a11.multiple(b12) + a12 * b22;
            var c21 = a21.multiple(b11) + a22.multiple(b21);
            var c22 = a21.multiple(b12) + a22 * b22;

            float* p3 = &c11.row1.X;
            float* p4 = &c21.row1.X;

            return new matrix4x3(
                *(p3 + 0), *(p3 + 1), c12.X,
                *(p3 + 2), *(p3 + 3), c12.Y,
                *(p4 + 0), *(p4 + 1), c22.X,
                *(p4 + 2), *(p4 + 3), c22.Y
                );
        }

        public static unsafe matrix4x4 multiple(this matrix4x3 matrix1, matrix3x4 matrix2)
        {
            float* p1 = &matrix1.row1.X;
            float* p2 = &matrix2.row1.X;

            var a11 = new matrix2x2(*(p1 + 0), *(p1 + 1), *(p1 + 3), *(p1 + 4));
            var a12 = new colum2(*(p1 + 2), *(p1 + 5));
            var a21 = new matrix2x2(*(p1 + 6), *(p1 + 7), *(p1 + 9), *(p1 + 10));
            var a22 = new colum2(*(p1 + 8), *(p1 + 11));

            var b11 = new matrix2x2(*(p2 + 0), *(p2 + 1), *(p2 + 4), *(p2 + 5));
            var b12 = new matrix2x2(*(p2 + 2), *(p2 + 3), *(p2 + 6), *(p2 + 7));
            var b21 = matrix2.row3.Point1;
            var b22 = matrix2.row3.Point2;

            var c11 = a11.multiple(b11) + a12.multiple(b21);
            var c12 = a11.multiple(b12) + a12.multiple(b22);
            var c21 = a21.multiple(b11) + a22.multiple(b21);
            var c22 = a21.multiple(b12) + a22.multiple(b22);

            return new matrix4x4(
                new vec4d(c11.row1, c12.row1),
                new vec4d(c11.row2, c12.row2),
                new vec4d(c21.row1, c22.row1),
                new vec4d(c21.row2, c22.row2)
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static unsafe matrix4x4 multiple(this matrix4x4 matrix1, matrix4x4 matrix2)
        {
            float* p1 = &matrix1.row1.X;
            float* p2 = &matrix2.row1.X;

            matrix2x2 a11 = new matrix2x2(*p1, *(p1 + 1), *(p1 + 4), *(p1 + 5));
            matrix2x2 a12 = new matrix2x2(*(p1 + 2), *(p1 + 3), *(p1 + 6), *(p1 + 7));
            matrix2x2 a21 = new matrix2x2(*(p1 + 8), *(p1 + 9), *(p1 + 12), *(p1 + 13));
            matrix2x2 a22 = new matrix2x2(*(p1 + 10), *(p1 + 11), *(p1 + 14), *(p1 + 15));

            matrix2x2 b11 = new matrix2x2(*p1, *(p1 + 1), *(p1 + 4), *(p1 + 5));
            matrix2x2 b12 = new matrix2x2(*(p1 + 2), *(p1 + 3), *(p1 + 6), *(p1 + 7));
            matrix2x2 b21 = new matrix2x2(*(p1 + 8), *(p1 + 9), *(p1 + 12), *(p1 + 13));
            matrix2x2 b22 = new matrix2x2(*(p1 + 10), *(p1 + 11), *(p1 + 14), *(p1 + 15));

            var s1 = a21 + a22;
            var s2 = s1 - a11;
            var s3 = a11 - a21;
            var s4 = a12 - s2;

            var t1 = b12 - b11;
            var t2 = b22 - t1;
            var t3 = b22 - b12;
            var t4 = t2 - b21;

            var m1 = a11.multiple(b11);
            var m2 = a12.multiple(b21);
            var m3 = s4.multiple(b22);
            var m4 = a22.multiple(t4);
            var m5 = s1.multiple(t1);
            var m6 = s2.multiple(t2);
            var m7 = s3.multiple(t3);

            var u1 = m1 + m2;
            var u2 = m1 + m6;
            var u3 = u2 + m7;
            var u4 = u2 + m5;
            var u5 = u4 + m3;
            var u6 = u3 - m4;
            var u7 = u3 + m5;

            return new matrix4x4(
                new vec4d(u1.row1.X, u1.row1.Y, u5.row1.X, u5.row1.Y),
                new vec4d(u1.row2.X, u1.row2.Y, u5.row2.X, u5.row2.Y),
                new vec4d(u6.row1.X, u6.row1.Y, u7.row1.X, u7.row1.Y),
                new vec4d(u6.row2.X, u6.row2.Y, u7.row2.X, u7.row2.Y)
                );
        }



    }
}
