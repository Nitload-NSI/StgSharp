//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixEnumerator"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric.Runtime
{
    public enum PredictPolicy
    {

        Transparent = 0,
        BufferSequential = 1,
        SquareRowMajor = 2,
        SquareColMajor = 3,

    }

    public unsafe interface IMatrixCachePredictor<TSelf> : IL4Predict
        where TSelf : class, IMatrixCachePredictor<TSelf>
    {

        public static abstract PredictPolicy Policy { get; set; }

    }

    public sealed unsafe class BufferSequentialPredictor : IMatrixCachePredictor<BufferSequentialPredictor>
    {

        private readonly MatrixParallelTask* origin;
        private readonly int _kernelSize;

        public static PredictPolicy Policy { get; set; } = PredictPolicy.SquareRowMajor;

        public bool IsSamePolicy(
                    uint policy,
                    IL4Predict predict
        )
        {
            throw new NotImplementedException();
        }

        public PredictResult Predict(
                             Span<CacheLinePrediction> node,
                             out int count
        )
        {
            throw new NotImplementedException();
        }

        public void Prefetch(
                    nuint origin,
                    uint policy,
                    Span<byte> cache
        )
        {
            throw new NotImplementedException();
        }

        public void WriteBack(
                    nuint origin,
                    uint policy,
                    ReadOnlySpan<byte> cache
        )
        {
            throw new NotImplementedException();
        }

    }
}
