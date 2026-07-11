// -----------------------------------------------------------------------------
// file="NumaNode"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.ProcessorAbstraction
{
    public class NumaNode
    {

        private LogicalCore[] _logicalCores;

        internal NumaNode(
                 uint numaId,
                 int count,
                 Span<uint> logicalCoreIds

        )
        {
            _logicalCores = new LogicalCore[count];
            for (int i = 0; i < count; i++) {
                _logicalCores[i] = new LogicalCore(1, numaId, logicalCoreIds[i]);
            }
        }

        public int LogicalCoreCount => _logicalCores.Length;

        public ReadOnlySpan<LogicalCore> LogicalCores => _logicalCores;

    }
}
