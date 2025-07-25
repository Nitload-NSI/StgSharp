//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="MatrixIndexLabel.cs"
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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Math
{
    public readonly struct MatrixIndexLabel
    {

        private readonly bool _isRange;
        public readonly Range R;

        internal MatrixIndexLabel(Index i)
        {
            _isRange = false;
            R = new Range(i, Index.End);
        }

        internal MatrixIndexLabel(Range r)
        {
            _isRange = true;
            R = r;
        }

        public (int offset,int legnth) GetOffsetAndLength(int CountOfCollection)
        {
            return _isRange ?
                    R.GetOffsetAndLength(CountOfCollection) : (R.Start.GetOffset(CountOfCollection), 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Length(int CountOfCollection)
        {
            return _isRange ? R.GetOffsetAndLength(CountOfCollection).Length : 1;
        }

        public int Offset(int CountOfCollection)
        {
            return _isRange ?
                    R.GetOffsetAndLength(CountOfCollection).Offset : R.Start
                                                                      .GetOffset(CountOfCollection);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator MatrixIndexLabel(Index i)
        {
            return new MatrixIndexLabel(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator MatrixIndexLabel(Range r)
        {
            return new MatrixIndexLabel(r);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator MatrixIndexLabel(int i)
        {
            return new MatrixIndexLabel(i);
        }

    }
}
