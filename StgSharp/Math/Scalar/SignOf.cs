using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Math
{
    public static unsafe partial class Scaler
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int SignOf(float x)
        {
            if (x == 0)
            {
                return 0;
            }
            if (x>0)
            {
                return 1;
            }
            return -1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int SignOf(int x)
        {
            if (x == 0)
            {
                return 0;
            }
            if (x > 0)
            {
                return 1;
            }
            return -1;
        }
    }
}
