//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="L4.Prefetch"
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
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class L4
    {

        private partial void Prefetch()
        {
            if (PrefetchPredict is null) {
                return;
            }
            Span<CacheLinePrediction> span = new(_prediction, PredictionCount);
            int actualCount = PrefetchPredict.Predict(span);
            int missIndex = 0;
            AgeAll();
            for (int i = 0; i < actualCount; i++)
            {
                ref CacheLinePrediction handle = ref span[i];
                nuint address = handle.Address;
                if (!TestHit(address))
                {
                    // miss and record
                    span[missIndex] = handle;
                    missIndex++;
                }
            }

            // now prefetch the misses
            for (int i = 0; i < missIndex; i++)
            {
                ref CacheLinePrediction handle = ref span[i];
                nuint address = handle.Address;
                nuint mapPtr = _cacheLineAllocator.Allocate();
                CacheLineHead* head = (CacheLineHead*)_entryAllocator.Allocate();


                // pre fetch them here
            }
        }

        private partial bool TestHit(nuint origin)
        {
            if (_map.TryGet(origin, out nuint handle))
            {
                CacheLineHead* head = (CacheLineHead*)handle;
                int index = head->_id;
                (*_eviction)[index] = 0; // reset eviction counter
            }
            return false;
        }

    }
}
