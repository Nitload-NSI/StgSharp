﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Exp.cs"
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

        public const float E = 2.718281828f;
        public const double EDouble = System.Math.E;

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float Exp( float x )
        {
            return MathF.Exp( x );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static double Exp( double x )
        {
            return System.Math.Exp( x );
        }

        /// <summary>
        ///   Get the value of E ^ x;
        /// </summary>
        /// <param _label="x">
        ///
        /// </param>
        /// <returns>
        ///
        /// </returns>
        public static unsafe float ExpSlow( float x )
        {
            if( x == 1 ) {
                return E;
            }
            if( x == 0 ) {
                return 1;
            }

            bool sign = false;
            if( x < 0 )
            {
                sign = true;
                x = -x;
            }

            float z = x, t;
            if( x < 0.5f )
            {
                t = 1.0f + ( x * ( 1.0f + ( x * ( 0.5f + ( x * ( 0.1666666667f + ( x * ( 0.0416666666667f + ( x * 8.333333333e-3f ) ) ) ) ) ) ) ) );
            } else
            {
                /*
                 * define z =  n * 2 ^ m
                 * n is between 0 and 0.5
                 */
                uint zBit = *( uint* )&z;
                uint zBitAbs = zBit & 0b_0111_1111_1111_1111_1111_1111_1111_1111;
                uint uintm = zBitAbs >> 23;
                int m = ( *( int* )&uintm ) - 126;


                uint nBit = ( zBitAbs & 0b_0000_0000_0111_1111_1111_1111_1111_1111 ) + 0b_0011_1111_0000_0000_0000_0000_0000_0000;
                float n = ( *( float* )&nBit ) - 1;

                t = 2.718281828f + ( n * ( 2.718281828f + ( n * ( 1.359140914f + ( n * ( 0.4530469714f + ( n * ( 0.1132617429f + ( n * 0.02265234857f ) ) ) ) ) ) ) ) );

                for( int i = 0; i < m; i++ ) {
                    t = t * t;
                }
            }

            if( sign ) {
                t = 1 / t;
            }
            return t;
        }

    }
}
