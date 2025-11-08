//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallel.Await"
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
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public static partial class MatrixParallel
    {

        private static AwaitingQueue _awaitingLeaderHandle;

        private static volatile int _createGeneration = 0;
        private static int _sortState;

        public static MatrixParallelThread[] LeadParallel(int recommendCount)
        {
            int min = int.Min(recommendCount, _threads.Capacity);
            min /= 2;
            MatrixParallelThread[] ret;
            if (_threads.Count < min)
            {
                WaitHandle handle = new WaitHandle
                (
                    _createGeneration,
                    min,
                    recommendCount
                );
                _awaitingLeaderHandle.Enqueue(handle);
                int cycleGeneration = _createGeneration;
                do
                {
                    Thread.Sleep(0);

                    if (_createGeneration != cycleGeneration)
                    {
                        if (Interlocked.Exchange(ref _sortState, 1) == 0)
                        {
                            if (_awaitingLeaderHandle.TryPopIfRefEqual(cycleGeneration, ref handle))
                            {
                                // Sort is implemented in TryPopIfRefEqual
                                ret = _threads.PopRange(handle.BestCount);
                                _createGeneration++;
                                _ = Interlocked.Exchange(ref _sortState, 0);
                                return ret;
                            }
                            _ = Interlocked.Exchange(ref _sortState, 0);
                        } else
                        {
                            cycleGeneration = _createGeneration;
                        }
                    }
                } while (true);
            } else
            {
                // directly get the leader
                while (true)
                {
                    if (Interlocked.Exchange(ref _sortState, 1) == 0)
                    {
                        ret = _threads.PopRange(recommendCount);
                        _createGeneration++;
                        _ = Interlocked.Exchange(ref _sortState, 0);
                        return ret;
                    }
                }
            }

            // return ret;
        }

        public static void ReturnParallelResources(ref MatrixParallelThread[] pool)
        {
            _threads.PushRange(pool);
            pool = null!;
        }

        private record class WaitHandle(int generation, int least, int best)
        {

            public int CreateGeneration { get; init; } = generation;

            public int LeastCount { get; init; } = least;

            public int BestCount { get; init; } = best;

            public int Priority { get; private set; }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void UpdatePriority(int currentGeneration)
            {
                Priority = (currentGeneration - CreateGeneration) * (MatrixParallel._threads.Capacity - LeastCount);
            }

        }

    }
}
