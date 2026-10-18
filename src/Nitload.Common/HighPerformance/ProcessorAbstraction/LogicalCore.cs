// -----------------------------------------------------------------------------
// file="LogicalCore"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitload.Common.HighPerformance.ProcessorAbstraction
{
    public readonly struct LogicalCore
    {

        public readonly uint ProcessorGroupId;
        public readonly uint SocketId;
        public readonly uint CpuIndex;
        public readonly uint NumaNodeId;

        internal LogicalCore(
                 uint processorGroupId,
                 uint socketId,
                 uint cpuIndex,
                 uint numaNodeId
        )
        {
            ProcessorGroupId = processorGroupId;
            SocketId = socketId;
            CpuIndex = cpuIndex;
            NumaNodeId = numaNodeId;
        }

        [Obsolete(
                "Do not manually create instances of LogicalCore. Use NumaNode to obtain LogicalCore instances.",
                true)]
        public LogicalCore() { }

    }
}
