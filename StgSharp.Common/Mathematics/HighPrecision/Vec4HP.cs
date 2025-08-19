//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Vec4HP.cs"
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
using StgSharp.HighPerformance;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.HighPrecision
{
    [StructLayout(LayoutKind.Explicit, Pack =16)]
    public  struct Vec4HP : IFixedVector<Vec4HP>
    {

        [FieldOffset(0)] private M256 buffer;
        [FieldOffset(0)] private Vector256<float> clrBuffer;
        [FieldOffset(3 * sizeof(double))] public double W;
        [FieldOffset(0 * sizeof(double))] public double X;
        [FieldOffset(1 * sizeof(double))] public double Y;
        [FieldOffset(2 * sizeof(double))] public double Z;

        public Vec4HP(double x, double y, double z, double w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
            Unsafe.SkipInit(out buffer);
            Unsafe.SkipInit(out clrBuffer);
        }

        public static Vec4HP Zero => new Vec4HP(0, 0, 0, 0);

        public static Vec4HP One => new Vec4HP(1, 1, 1, 1);

    }
}
