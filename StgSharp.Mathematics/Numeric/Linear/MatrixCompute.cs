//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixCompute"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
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
using StgSharp.HighPerformance.Memory;
using StgSharp.Internal;
using StgSharp.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public static partial class MatrixCompute
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe MatrixKernel* GetKernelAddressUnsafe<T>(
                                             MatrixKernel<T>* buffer,
                                             int colLength,
                                             int x,
                                             int y
        ) where T : unmanaged, INumber<T>
        {
            return (MatrixKernel*)(buffer + (colLength * x + y));
        }

    }
}
