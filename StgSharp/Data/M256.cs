using StgSharp.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Math.HighPrecession
{

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct M256:IMemoryType
    {
        private M128 v0;
        private M128 v1;


        T IMemoryType.Read<T>(int index)
        {
            fixed (M128* mptr = &v0)
            {
                return *(((T*)mptr) + index);
            }
        }

        void IMemoryType.Write<T>(int index, T value)
        {
            fixed (M128* mptr = &v0)
            {
                *(((T*)mptr) + index) = value;
            }
        }
    }
}
