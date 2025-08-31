//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixIndex.cs"
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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct MatrixIndex
    {

        public readonly int Begin;
        public readonly int End;

        private MatrixIndex(int begin, int end, bool safe = false)
        {
            Begin = begin;
            End = end;
        }

        internal MatrixIndex(int begin, int end)
        {
            if (begin > end) {
                throw new ArgumentException("Begin index cannot be greater than end index.");
            }

            Begin = begin;
            End = end;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator MatrixIndex((int, int) range)
        {
            return new(range.Item1, range.Item2, false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator MatrixIndex(int index)
        {
            return new(index, -1, false);
        }

    }
}
