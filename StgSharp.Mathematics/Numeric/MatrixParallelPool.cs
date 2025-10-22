//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallelPool.cs"
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
using StgSharp.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public class MatrixParallelPool
    {

        private MatrixParallelPool()
        {
            throw new NotSupportedException();
        }
        /*
        internal MatrixParallelPool(int threadCount, ReadOnlySpan<MatrixParallelWrap> wrapSpan)
        {
            SpareWrap = CapacityFixedStackBuilder.Create(wrapSpan);
            SingleThreadAllocation = new CapacityFixedBag<MatrixParallelWrap>(wrapSpan.Length);
            SpareThreadCount = threadCount;
        }
        /**/

        internal MatrixParallelPool(int threadCount, CapacityFixedStack<MatrixParallelWrap> wrapSpan)
        {
            SpareWrap = wrapSpan;
            SingleThreadAllocation = new CapacityFixedBag<MatrixParallelWrap>(wrapSpan.Count);
            SpareThreadCount = threadCount;
        }

        public int SpareThreadCount { get; set; }

        internal CapacityFixedBag<MatrixParallelWrap> SingleThreadAllocation { get; set; }

        internal CapacityFixedStack<MatrixParallelWrap> SpareWrap { get; set; }

        public void ReturnIndividualHandle(ref MatrixParallelWrap.Handle handle)
        {
            MatrixParallelWrap wrap = handle.Wrap;
            handle.Role = 0;
            if (wrap.IsAllAvailable)
            {
                SingleThreadAllocation.RemoveAt(wrap.Magic);
                SpareWrap.Push(wrap);
                SpareThreadCount += wrap.WrapTaskCapacity;
            }
        }

        public bool TryRentIndividualHandle(out MatrixParallelWrap.Handle handle)
        {
            foreach (MatrixParallelWrap item in SingleThreadAllocation)
            {
                if (item.TryGetLeaderHandle(out handle!)) {
                    return true;
                }
            }
            if (TryRequestWrap(out MatrixParallelWrap wrap))
            {
                wrap!.Magic = SingleThreadAllocation.Count;
                SingleThreadAllocation.Add(wrap);
                if (wrap.TryGetLeaderHandle(out handle!)) {
                    return true;
                }
            }
            handle = null!;
            return false;
        }

        public bool TryRequestWrap([NotNull]out MatrixParallelWrap? wrap)
        {
            if (SpareWrap.TryPop(out wrap))
            {
                SpareThreadCount -= wrap.WrapTaskCapacity;
                return true;
            }
            wrap = null!;
            return false;
        }

    }
}
