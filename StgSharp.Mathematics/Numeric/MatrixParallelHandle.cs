//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallelPool"
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
using StgSharp.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public struct MatrixParallelHandle : IDisposable
    {

        private static readonly (int a, int b)[] LookupTable = [ (1, 1), // 7
            (0, 2), // 8  (no non-zero solution)
            (3, 0), // 9  (no non-zero solution)
            (2, 1), // 10
            (1, 2), // 11
            (0, 3), // 12 (no non-zero solution)
            (3, 1), // 13
            (2, 2), // 14
            (1, 3), // 15
            (4, 1), // 16
            (3, 2), // 17
            (2, 3), // 18
            (1, 4), // 19
            (4, 2), // 20
        ];

        private MatrixParallelThread[] _threads;
        private bool _disposed = false;
        private CapacityFixedStack<MatrixParallelWrap> _wraps;

        internal MatrixParallelHandle(MatrixParallelThread[] threadArray)
        {
            _threads = threadArray;
            IsMultithread = threadArray.Length > 1;
            SliceToWraps();
        }

        public ReadOnlySpan<MatrixParallelThread> Threads
        {
            get
            {
                if (_disposed) {
                    throw new ObjectDisposedException(typeof(MatrixParallelHandle).Name);
                }
                return _threads;
            }
        }

        internal IEnumerable<MatrixParallelWrap> Wraps
        {
            get
            {
                if (_disposed) {
                    throw new ObjectDisposedException(typeof(MatrixParallelHandle).Name);
                }
                return _wraps;
            }
        }

        internal ReadOnlySpan<MatrixParallelThread> ThreadsUnsafe
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _threads;
        }

        private bool IsMultithread { get; init; }

        public void Dispose()
        {
            if (_disposed) {
                return;
            }
            MatrixParallel.ReturnParallelResources(ref _threads);
            _disposed = true;
        }

        public MatrixParallelThread GetLeader()
        {
            Threads[0].Role = 2;
            return Threads[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (int count_3, int count_4) FastDecompose(int c)
        {
            /*
            * Thread wrapping to accelerate matrix operations by dividing threads into groups (pool).
            * For M pool, each containing an average of N threads, we satisfy the condition M * N = T - 4,
            * where T is the total thread _count of the CPU. Typically, for modern consumer CPUs, T is around 16.
            * For simplicity, let M * N = 16.
            *
            * The total time complexity for publishing tasks is O(M + N):
            * - O(M) accounts for distributing tasks across M pool.
            * - O(N) accounts for threads within each wrap fetching tasks from the wrap queue.
            *
            * Using the inequality Sqrt(M * N) <= (M + N) / 2 (derived from the arithmetic-geometric mean inequality),
            * the optimal configuration occurs when M = N. For M * N = 16, this gives M = N = 4.
            * 
            * To ensure compatibility across a wide range of CPU architectures, 
            * we allow each wrap to contain either 3 or 4 threads, providing flexibility while maintaining efficiency.
            */
            if (c < 7)
            {
                return (0, 0);
            }

            int t = c - 7;
            int k = t / 14;
            int d = t % 14;

            (int a0, int b0) = LookupTable[d];

            return (a0 + (2 * k), b0 + (2 * k));
        }

        private void SliceToWraps()
        {
            if (_disposed) {
                throw new ObjectDisposedException(typeof(MatrixParallelHandle).Name);
            }
            if (_wraps is not null) {
                return;
            }
            int count = _threads.Length;
            if (count < 6)
            {
                MatrixParallelWrap wrap = new MatrixParallelWrap(_threads, 0, count);
                _wraps = [ wrap ];
            }
            (int c3,int c4) = FastDecompose(_threads.Length);
            _wraps = new(c3 + c4);
            int begin = 0;
            for (int i = 0; i < c3; i++)
            {
                _wraps.Push(new MatrixParallelWrap(_threads, begin, 3));
                begin += 3;
            }
            for (int i = 0; i < c4; i++)
            {
                _wraps.Push(new MatrixParallelWrap(_threads, begin, 4));
                begin += 4;
            }
        }

    }
}
