//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Factorial"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using System;

namespace StgSharp.Mathematics
{
    public static partial class Scaler
    {

        private static readonly uint[] factorialResult = [
            1,
            1,
            2,
            6,
            24,
            120,
            720,
            5040,
            40320,
            362880,
            3628800,
            39916800,
            479001600
        ];

        public static unsafe ulong Factorial(int n)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(n, 0);
            if (n < 13) {
                return factorialResult[n];
            }
            if (n < 30) {
                return NativeIntrinsic.Intrinsic.factorial_simd(n);
            }
            double 
                dn = n,
                stir = Sqrt(2 * PiDouble * dn) * Pow(dn / EDouble, dn),
                correction = Exp(1 / (12 * dn));
            return (ulong)(stir * correction);
        }

    }
}
