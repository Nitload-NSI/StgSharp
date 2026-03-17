//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="L4.Debug"
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.Memory
{
    public partial class L4
    {

        public void DebugAgeAll()
        {
            AgeAll();
        }

        public bool DebugContains(
                    nuint origin
        )
        {
            return _map.TryGet(origin, out _);
        }

        public int DebugEvict(
                   int count = 1
        )
        {
            return Evict(count);
        }

        public unsafe DebugSnapshot GetDebugSnapshot()
        {
            int maxId = _maxId;
            byte[] rrpv = new byte[maxId];
            byte* p = (byte*)_eviction;
            for (int i = 0; i < maxId; i++) {
                rrpv[i] = p[i];
            }
            return new DebugSnapshot(_activeCount, _aheadCount, maxId, rrpv);
        }

        public readonly struct DebugSnapshot
        {

            /// <summary>
            ///   RRPV value for each line（0~3）, length == MaxId
            /// </summary>
            public readonly byte[] RrpvSnapshot;
            /// <summary>
            ///   Count of activated cache line handle
            /// </summary>
            public readonly int ActiveCount;

            /// <summary>
            ///   Count of predictor ahead of current map position（aheadCount）
            /// </summary>
            public readonly int AheadCount;

            /// <summary>
            ///   Max ID of the line being using or once used（maxId）
            /// </summary>
            public readonly int MaxId;

            internal DebugSnapshot(
                     int activeCount,
                     int aheadCount,
                     int maxId,
                     byte[] rrpv
            )
            {
                ActiveCount = activeCount;
                AheadCount = aheadCount;
                MaxId = maxId;
                RrpvSnapshot = rrpv;
            }

        }

    }
}
