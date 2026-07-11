// -----------------------------------------------------------------------------
// file="L4.Debug"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.Memory
{
    /**/
    public partial class L4
    {

        public void DebugAgeAll()
        {
            // AgeAll();
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
            throw new NotImplementedException();
            /*
            int maxId = _maxId;
            byte[] rrpv = new byte[maxId];
            byte* p = (byte*)_eviction;
            for (int i = 0; i < maxId; i++) {
                rrpv[i] = p[i];
            }
            return new DebugSnapshot(_activeCount, _aheadCount, maxId, rrpv);
            /**/
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
    /**/
}
