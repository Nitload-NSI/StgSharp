﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Sqrt.cs"
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
    public static partial class Scaler
    {

        public static unsafe float SeedSqrt( float x )
        {
            /*
             * 本方法采用分别对指数和小数处理的方法计算平方根
             * 将指数除以2即可获得结果的指数
             * （补充：指数最低位被加到小数位中以抵消一个多余的2 ^ 1的影响）
             * 小数部分则统一乘以2 ^ 16后由y = Sqrt(x)在1.5 * 2 ^ 16处的切线近似
             * 为简化操作，提高计算速度，切线左界被移动到远点
             * 在进一步简化操作后，计算精度达到0.5%，速度约为.Net库函数的3倍
             */

            if( x < 0 )
            {
                throw new ArgumentOutOfRangeException( "Input is smaller than zero!" );
            }
            if( x == 0 ) {
                return 0;
            }
            /*
            * 提取指数位并减半
            * 由于平方根仅对正数有效
            * 故不考虑正负号
            */

            if( x < 0 )
            {
                throw new ArgumentOutOfRangeException();
            }
            if( x <= 1e-12 ) {
                return 0;
            }

            //提取指数位并减去指数位中的127
            //由于平方根仅对正数有效
            //故不考虑正负号
            uint a = *( uint* )&x;
            uint f = a;
            f >>= 23;
            f /= 2;
            f += 64;

            //提取小数位
            //由于平方根仅对正数有效
            //故不考虑正负号

            a -= 0b_0011_1111_0000_0000_0000_0000_0000_0000;
            uint e = a << 8;
            e >>= 8;

            //线性近似
            e *= 0b_1101_0001;
            e >>= 24;
            e += 0b_0000_0000_0001_0000_1010_1000_1110_0010;

            //还原指数
            f <<= 23;
            e += f;

            float result = *( float* )&e;
            result = ( result + ( x / result ) ) / 2;
            return ( result + ( x / result ) ) / 2;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float Sqrt( float x )
        {
            return MathF.Sqrt( x );
            /**/
        }

    }
}
