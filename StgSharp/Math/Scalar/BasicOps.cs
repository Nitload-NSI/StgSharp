using System;
using System.Globalization;

namespace StgSharp.Math
{
    public static unsafe partial class Calc
    {

        public static unsafe float Abs(float x)
        {
            uint* a = (uint*)&x;
            *a &= 0b_0111_1111_1111_1111_1111_1111_1111_1111;
            return *(float*)a;
        }

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
