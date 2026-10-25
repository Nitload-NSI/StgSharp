// -----------------------------------------------------------------------------
// file="NumaNode"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nitload.Common.HighPerformance.ProcessorAbstraction
{
    public class NumaNode
    {

        private LogicalCore[] _logicalCores;

        internal NumaNode(
                 int numaId,
                 int count,
                 Span<uint> processorGroupIds,
                 Span<uint> socketIds,
                 Span<uint> cpuIndices,
                 Span<uint> numaNodeIds

        )
        {
            NumaNodeId = numaId;
            _logicalCores = new LogicalCore[count];
            for (int i = 0; i < count; i++) {
                _logicalCores[i] = new LogicalCore(processorGroupIds[i], socketIds[i],
                                                   cpuIndices[i], numaNodeIds[i]);
            }
        }

        public int LogicalCoreCount => _logicalCores.Length;

        public int NumaNodeId { get; }

        public ReadOnlySpan<LogicalCore> LogicalCores => _logicalCores;

    }
}
