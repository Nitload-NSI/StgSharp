//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="mat3.cs"
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
using StgSharp.Math;

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp
{
    internal static unsafe partial class InternalIO
    {

        #region det

        [DllImport(InternalIO.SSC_libname, EntryPoint = "det_mat3",
            CallingConvention = CallingConvention.Cdecl)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static extern unsafe float det_mat3(Mat3* mat);

        #endregion

        #region transpose

        [DllImport(InternalIO.SSC_libname, EntryPoint = "transpose3to4",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void Transpose3to4_internal(Mat3* source, Mat4* target);

        [DllImport("StgsharpGra[hic", EntryPoint = "transpose3to4",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void Transpose3to3_internal(Mat3* source, Mat3* target);

        [DllImport("StgsharpGra[hic", EntryPoint = "transpose3to2",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void Transpose3to2_internal(Mat3* source, Mat2* target);

        #endregion


    }

}


namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit, Size = 12 * sizeof(float), Pack = 16)]
    public struct Mat3 : IEquatable<Mat3>, IMat
    {

        [FieldOffset(0)] internal Vector4 colum0;
        [FieldOffset(4 * sizeof(float))] internal Vector4 colum1;
        [FieldOffset(8 * sizeof(float))] internal Vector4 colum2;

        [FieldOffset(0 * sizeof(float))] internal float m00;
        [FieldOffset(1 * sizeof(float))] internal float m10;
        [FieldOffset(2 * sizeof(float))] internal float m20;
        [FieldOffset(3 * sizeof(float))] internal float m30;

        [FieldOffset(4 * sizeof(float))] internal float m01;
        [FieldOffset(5 * sizeof(float))] internal float m11;
        [FieldOffset(6 * sizeof(float))] internal float m21;
        [FieldOffset(7 * sizeof(float))] internal float m31;

        [FieldOffset(8 * sizeof(float))] internal float m02;
        [FieldOffset(9 * sizeof(float))] internal float m12;
        [FieldOffset(10 * sizeof(float))] internal float m22;
        [FieldOffset(11 * sizeof(float))] internal float m32;

        public override bool Equals(object? obj)
        {
            return (obj is Mat3 mat) && Equals(mat);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Mat3 other)
        {
            return colum0.Equals(other.colum0) &&
                   colum1.Equals(other.colum1) &&
                   colum2.Equals(other.colum2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Mat3 left, Mat3 right)
        {
            return !(left == right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Mat3 left, Mat3 right)
        {
            return left.Equals(right);
        }

    }
}
