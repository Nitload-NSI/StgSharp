//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallel.AwaitingQueue"
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

        private class AwaitingQueue
        {

            private readonly ConcurrentQueue<WaitHandle> _fallback = [];
            private Element _quickElements = new();
            private volatile int _count;
            private volatile int _lastGeneration;
            private readonly
#if NET9_0_OR_GREATER
                Lock
#else
                object
#endif
                sync = new();

            public AwaitingQueue() { }

            public void Enqueue(WaitHandle handle)
            {
                if (_count > 7)
                {
                    _fallback.Enqueue(handle);
                    Interlocked.Increment(ref _count);
                    return;
                }
                lock (sync)
                {
                    if (_count > 7)
                    {
                        _fallback.Enqueue(handle);
                        Interlocked.Increment(ref _count);
                        return;
                    }
                    _quickElements[_count] = handle;
                    Interlocked.Increment(ref _count);
                }
            }

            public bool TryDequeue(int generation, out WaitHandle handle)
            {
                if (_count == 0)
                {
                    _lastGeneration = generation;
                    handle = null!;
                    return false;
                }
                lock (sync)
                {
                    if (_lastGeneration == generation)
                    {
                        for (int i = 0; i < _count; i++) {
                            _quickElements[i].UpdatePriority(generation);
                        }
                        Element.Sort(_quickElements);
                    } else
                    {
                        _lastGeneration = generation;
                    }
                    handle = _quickElements[0];
                    _count--;
                    if (_count > 7) {
                        _ = _fallback.TryDequeue(out _quickElements[0]!);
                    }
                    return true;
                }
            }

            public bool TryPeek(int generation, out WaitHandle handle)
            {
                if (_count == 0)
                {
                    _lastGeneration = generation;
                    handle = null!;
                    return false;
                }
                lock (sync)
                {
                    for (int i = 0; i < _count; i++) {
                        _quickElements[i].UpdatePriority(generation);
                    }
                    Element.Sort(_quickElements);
                    _lastGeneration = generation;
                    handle = _quickElements[0];
                    _count--;
                    if (_count > 7) {
                        _ = _fallback.TryDequeue(out _quickElements[0]!);
                    }
                    return true;
                }
            }

            public bool TryPopIfRefEqual(int generation, ref WaitHandle handle)
            {
                if (_count == 0)
                {
                    handle = null!;
                    return false;
                }
                lock (sync)
                {
                    for (int i = 0; i < _count; i++) {
                        _quickElements[i].UpdatePriority(generation);
                    }
                    Element.Sort(_quickElements);
                    _lastGeneration = generation;
                    handle = _quickElements[0];
                    _count--;
                    if (!ReferenceEquals(_quickElements[0], handle)) {
                        return false;
                    }
                    _quickElements[0] = null!;
                    if (_count > 7) {
                        _ = _fallback.TryDequeue(out _quickElements[0]!);
                    }
                    return true;
                }
            }

        }

        [InlineArray(8)]
        private struct Element
        {

            private WaitHandle value;

            public static void Sort(Element e)
            {
                /*
                 * yet to do
                 */
            }

        }

    }
}

