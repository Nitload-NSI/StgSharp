//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="VecAny.cs"
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Math
{
    [CollectionBuilder(typeof(VecAnyBuilder), nameof(VecAnyBuilder.Build))]
    public class VecAny : IEnumerable<float>
    {

        private float[] _values;

        internal VecAny(ReadOnlySpan<float> values)
        {
            _values = values.ToArray();
        }

        public VecAny(int size)
        {
            if (size <= 0) {
                throw new ArgumentOutOfRangeException(
                    nameof(size), "Size must be greater than zero.");
            }
            _values = new float[size];
        }

        public VecAny? this[ReadOnlySpan<MatrixIndexLabel> span]
        {
            get
            {
                if (span.Length == 0) {
                    return null;
                }
                int size = 0;
                foreach (MatrixIndexLabel label in span) {
                    size += label.Length(_values.Length);
                }
                return new VecAny(size);
            }
        }

        public IEnumerator<float> GetEnumerator()
        {
            return ((IEnumerable<float>)_values).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

    }

    public class VecAnyBuilder
    {

        public static VecAny Build(ReadOnlySpan<float> span)
        {
            return new VecAny(span);
        }

    }
}
