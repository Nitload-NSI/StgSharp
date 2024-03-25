//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="BasicOps.cs"
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
using System.Globalization;
using System.Runtime.CompilerServices;

namespace StgSharp.Math
{
    public static unsafe partial class Scaler
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe float Abs(float x)
        {
            uint* a = (uint*)&x;
            *a &= 0b_0111_1111_1111_1111_1111_1111_1111_1111;
            return *(float*)a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Abs(int x)
        {
            uint* a = (uint*)&x;
            *a &= 0b_0111_1111_1111_1111_1111_1111_1111_1111;
            return *(int*)a;
        }

        internal static string GetListSeparator(IFormatProvider formatProvider)
        {
            if (formatProvider is CultureInfo cultureInfo)
            {
                return cultureInfo.TextInfo.ListSeparator;
            }

            if (formatProvider?.GetFormat(typeof(TextInfo)) is TextInfo textInfo)
            {
                return textInfo.ListSeparator;
            }

            return CultureInfo.CurrentCulture.TextInfo.ListSeparator;
        }

    }
}