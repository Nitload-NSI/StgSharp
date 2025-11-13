//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallel.AwaitingQueue"
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
        /*
         * Because caller of this class has its own sync object
         * this queue will use no sync object internally.
        */

        private class AwaitingQueue
        {

            private Element _quickElements = new();
            private volatile int _count;
            private readonly Queue<WaitHandle> _fallback = [];

            public AwaitingQueue() { }

            public void Enqueue(WaitHandle handle)
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

            public bool TryDequeue(int generation, out WaitHandle handle)
            {
                if (_count == 0)
                {
                    handle = null!;
                    return false;
                }
                for (int i = 0; i < _count; i++) {
                    _quickElements[i].UpdatePriority(generation);
                }
                Element.Sort(ref _quickElements);
                handle = _quickElements[0];
                _count--;
                _quickElements[0] = default!;
                if (_count > 7) {
                    _ = _fallback.TryDequeue(out _quickElements[0]!);
                }
                return true;
            }

            public bool TryPeek(int generation, out WaitHandle handle)
            {
                if (_count == 0)
                {
                    handle = null!;
                    return false;
                }
                for (int i = 0; i < _count; i++) {
                    _quickElements[i].UpdatePriority(generation);
                }

                Element.Sort(ref _quickElements);
                handle = _quickElements[0];
                return true;
            }

            public bool TryPopIfRefEqual(int generation, in WaitHandle handle)
            {
                if (_count == 0) {
                    return false;
                }
                for (int i = 0; i < _count; i++) {
                    _quickElements[i].UpdatePriority(generation);
                }
                Element.Sort(ref _quickElements);
                if (!ReferenceEquals(_quickElements[0], handle)) {
                    return false;
                }
                _count--;
                _quickElements[0] = null!;
                if (_count > 7) {
                    _ = _fallback.TryDequeue(out _quickElements[0]!);
                }
                return true;
            }

        }

        [InlineArray(8)]
        private struct Element
        {

            private WaitHandle value;

            public static void Sort(ref Element e)
            {
                int maxIdx = 0;
                WaitHandle first = e[0];
                int maxPr = first?.Priority ?? int.MinValue;

                for (int i = 1; i < 8; i++)
                {
                    WaitHandle cur = e[i];
                    int p = cur?.Priority ?? int.MinValue;
                    if (p > maxPr)
                    {
                        maxPr = p;
                        maxIdx = i;
                    }
                }

                if (maxIdx != 0) {
                    (e[0], e[maxIdx]) = (e[maxIdx], e[0]);
                }
            }

        }

    }
}

