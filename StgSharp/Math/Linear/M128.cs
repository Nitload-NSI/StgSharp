//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="M128.cs"
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
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if NET5_0_OR_GREATER

using System.Runtime.Intrinsics;

#endif

using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit, Pack = 16)]
    public unsafe struct M128
    {

        [FieldOffset(0)] internal Vector4 vec;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Read<T>(int index) where T : struct
        {
            fixed (Vector4* p = &vec)
            {
                return *(((T*)p) + index);
            }
        }

        public override string ToString()
        {
            return string.Empty;
        }

        public string ToString(int sample)
        {
            string str = string.Empty;
            for (int i = 15; i >= 0; i--)
            {
                str += $"{Convert.ToString(this.Read<byte>(i), sample)} ";
            }
            return str;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(int index, T value) where T : struct
#if NET8_0_OR_GREATER
            , INumber
#endif
        {
            fixed (Vector4* p = &vec)
            {
                *(((T*)p) + index) = value;
            }
        }

#if NET5_0_OR_GREATER

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector128<byte>(M128 m)
        {
            return Vector128.As<float, byte>(m.vec.AsVector128());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector128<sbyte>(M128 m)
        {
            return Vector128.As<float, sbyte>(m.vec.AsVector128());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector128<short>(M128 m)
        {
            return Vector128.As<float, short>(m.vec.AsVector128());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector128<ushort>(M128 m)
        {
            return Vector128.As<float, ushort>(m.vec.AsVector128());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector128<int>(M128 m)
        {
            return Vector128.As<float, int>(m.vec.AsVector128());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector128<uint>(M128 m)
        {
            return Vector128.As<float, uint>(m.vec.AsVector128());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector128<long>(M128 m)
        {
            return Vector128.As<float, long>(m.vec.AsVector128());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector128<ulong>(M128 m)
        {
            return Vector128.As<float, ulong>(m.vec.AsVector128());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector128<float>(M128 m)
        {
            return Vector128.As<float, float>(m.vec.AsVector128());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector128<double>(M128 m)
        {
            return Vector128.As<float, double>(m.vec.AsVector128());
        }

#endif
    }
}