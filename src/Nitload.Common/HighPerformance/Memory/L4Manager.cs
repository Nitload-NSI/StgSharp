// -----------------------------------------------------------------------------
// file="L4Manager"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Common.HighPerformance.ProcessorAbstraction;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitload.Common.HighPerformance.Memory
{
    public class L4Manager
    {

        private FrozenDictionary<int, L4AwareThread[]> _threads = FrozenDictionary<int, L4AwareThread[]>.Empty;

        private L4AwareThread[] _allThreads = [];

        private FrozenDictionary<int, L4> _memory = FrozenDictionary<int, L4>.Empty;

        public L4Manager() { }

        private void MakeMemory(
                     nuint memorySize,
                     L4ManagerCapacityLayout layout,
                     int align = 16,
                     uint maxCacheLineSize = 8192

        )
        {
            nuint size = layout switch
            {
                L4ManagerCapacityLayout.Default => memorySize,
                L4ManagerCapacityLayout.AveragePerNuma => memorySize / (nuint)Numa.AllNumaNode.Length,
                L4ManagerCapacityLayout.AveragePerThread => memorySize / (nuint)Environment.ProcessorCount,
                L4ManagerCapacityLayout.PerNuma => memorySize,
                L4ManagerCapacityLayout.PerThread => memorySize,
                _ => throw new NotImplementedException($"Unknown layout type: {layout}"),
            };
            size = (size + 4095) / 4096;
            Dictionary<int, L4> memory = new(Numa.NumaNodeCount);
            foreach (NumaNode node in Numa.AllNumaNode)
            {
                int id = node.NumaNodeId;
                memory.Add(id, new(id));
            }
            _memory = memory.ToFrozenDictionary();
        }

        private void MakeThreads<TThread>(
                     Func<L4AwareThread> threadFactory
        ) where TThread : L4AwareThread
        {
            Dictionary<int, L4AwareThread[]> threads = new(Numa.NumaNodeCount);
            List<L4AwareThread> allThreads = new();
            foreach (NumaNode numaNode in Numa.AllNumaNode)
            {
                ReadOnlySpan<LogicalCore> cores = numaNode.LogicalCores;
                L4AwareThread[] nodeThreads = new L4AwareThread[cores.Length];
                for (int coreIndex = 0; coreIndex < cores.Length; coreIndex++)
                {
                    L4AwareThread t = threadFactory();
                    t.Initialize(numaNode, cores[coreIndex], _memory[numaNode.NumaNodeId]);
                    nodeThreads[coreIndex] = t;
                    allThreads.Add(t);
                }
                threads.Add(numaNode.NumaNodeId, nodeThreads);
            }
            _threads = threads.ToFrozenDictionary();
            _allThreads = allThreads.ToArray();
        }

    }

    /// <summary>
    ///   How manager spread provided memory capacity to numa nodes.
    /// </summary>
    public enum L4ManagerCapacityLayout
    {

        Default = 0,
        /// <summary>
        ///   Amount from all numa nodes is provided, and each numa node will get the same capacity
        ///   of memory, if possible.
        /// </summary>
        AveragePerNuma = 1,
        /// <summary>
        ///   Amount from all numa nodes is provided, and each thread will get the same capacity of
        ///   memory, if possible.
        /// </summary>
        AveragePerThread = 2,
        /// <summary>
        ///   Provided amount of memory will be allocated for each numa node, if possible.
        /// </summary>
        PerNuma = 3,
        /// <summary>
        ///   Provided amount of memory will be allocated for each thread, if possible.
        /// </summary>
        PerThread = 4,

    }
}
