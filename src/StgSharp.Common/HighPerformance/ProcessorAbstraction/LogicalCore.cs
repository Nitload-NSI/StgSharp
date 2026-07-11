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

namespace StgSharp.HighPerformance.ProcessorAbstraction
{
    public readonly struct LogicalCore
    {

        public readonly uint GlobalCpuId;
        public readonly uint NumaGroupId;

        /// <summary>
        ///   Stays 0 on linux.
        /// </summary>
        public readonly uint NumaNodeId;

        internal LogicalCore(
                 uint globalCpuId,
                 uint numaGroupId,
                 uint numaNodeId
        )
        {
            GlobalCpuId = globalCpuId;
            NumaGroupId = numaGroupId;
            NumaNodeId = numaNodeId;
        }

        [Obsolete(
                "Do not manually create instances of LogicalCore. Use NumaNode to obtain LogicalCore instances.",
                true)]
        public LogicalCore() { }

    }
}
