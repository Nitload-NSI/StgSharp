//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Sqrt.cs"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the “Software”), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//     
// The above copyright notice and 
// this permission notice shall be included in all copies 
// or substantial portions of the Software.
//     
// THE SOFTWARE IS PROVIDED “AS IS”, 
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
// ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using System;
using System.Runtime.CompilerServices;

namespace StgSharp.Mathematics
{
    public static partial class Scaler
    {

        public static unsafe float SeedSqrt(float x)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(x);
            if (x <= 1e-12) {
                return 0;
            }
            uint a = *(uint*)&x;
            uint f = a;
            f >>= 23;
            f /= 2;
            f += 64;
            a -= 0b_0011_1111_0000_0000_0000_0000_0000_0000;
            uint e = a << 8;
            e >>= 8;
            e *= 0b_1101_0001;
            e >>= 24;
            e += 0b_0000_0000_0001_0000_1010_1000_1110_0010;
            f <<= 23;
            e += f;
            float result = *(float*)&e;
            result = (result + (x / result)) / 2;
            return (result + (x / result)) / 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sqrt(float x)
        {
            return MathF.Sqrt(x);
            /**/
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Sqrt(double x)
        {
            return System.Math.Sqrt(x);
        }

    }
}
