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
    public static class L4Manager
    {

        public static L4Manager<TThread> Create<TThread>(
                                         Func<TThread> threadFactory,
                                         uint memorySize,
                                         L4ManagerCapacityLayout layout,
                                         int align = 16,
                                         uint maxCacheLineSize = 8192
        ) where TThread : L4AwareThread
        {
            ArgumentNullException.ThrowIfNull(threadFactory, nameof(threadFactory));
            L4Manager<TThread> manager = new();
            manager.MakeMemory(memorySize, layout, align, maxCacheLineSize);
            manager.MakeThreads(threadFactory);
            return manager;
        }

    }

    public class L4Manager<TThread> : IDisposable where TThread : L4AwareThread
    {

        private TThread[] _allThreads = [];

        private bool disposedValue;

        private FrozenDictionary<int, L4> _memory = FrozenDictionary<int, L4>.Empty;

        private FrozenDictionary<int, TThread[]> _threadsInNuma = FrozenDictionary<int, TThread[]>.Empty;

        internal L4Manager() { }

        public int ActiveThreadCount { get; private set; }

        public void Dispose()
        {
            Dispose(disposing:true);
            GC.SuppressFinalize(this);
        }

        public void ForeachThread(
                    Action<TThread> action
        )
        {
            if (action is not null)
            {
                foreach (TThread t in _allThreads)
                {
                    if (t.State != L4AwareThreadState.Retired) {
                        action(t);
                    }
                }
            }
        }

        public void RetireByOrder(
                    IComparer<TThread> comparer,
                    L4ManagerRetirePolicy policy
        )
        {
            ArgumentNullException.ThrowIfNull(comparer);
            auto:
            policy = policy switch
            {
                L4ManagerRetirePolicy.Auto when Numa.NumaNodeCount > 1 => L4ManagerRetirePolicy.PerNumaWeakest,
                L4ManagerRetirePolicy.Auto when Environment.ProcessorCount > 8 => L4ManagerRetirePolicy.GlobalWeakest,
                L4ManagerRetirePolicy.Auto => L4ManagerRetirePolicy.MinimumRemain,
                L4ManagerRetirePolicy.ClusterComputeNode => policy,
                L4ManagerRetirePolicy.GlobalWeakest => policy,
                L4ManagerRetirePolicy.PerNumaWeakest => policy,
                L4ManagerRetirePolicy.MinimumRemain => policy,
                _ => throw new ArgumentOutOfRangeException(nameof(policy))
            };

            switch (policy)
            {
                case L4ManagerRetirePolicy.ClusterComputeNode:
                    ActiveThreadCount -= RetireWeakest(_allThreads, 1, comparer);
                    break;
                case L4ManagerRetirePolicy.GlobalWeakest:
                    ActiveThreadCount -= RetireWeakest(_allThreads, 4, comparer);
                    break;
                case L4ManagerRetirePolicy.PerNumaWeakest:
                    foreach (TThread[] threads in _threadsInNuma.Values) {
                        ActiveThreadCount -= RetireWeakest(threads, 4, comparer);
                    }

                    break;
                case L4ManagerRetirePolicy.MinimumRemain:
                    IEnumerable<TThread> activeThreads = _allThreads
                        .Where(thread => thread.State != L4AwareThreadState.Retired);
                    int activeCount = activeThreads.Count();
                    int remainCount = activeCount == 0 ?
                                      0 :
                                      Math.Min(Math.Max(activeCount / 4, 1), 4);
                    ActiveThreadCount -= RetireWeakest(activeThreads, activeCount - remainCount, comparer);
                    break;
                case L4ManagerRetirePolicy.Auto:
                    goto auto;
                default:
                    goto auto;
            }
        }

        internal void MakeMemory(
                      uint memorySize,
                      L4ManagerCapacityLayout layout,
                      int align = 16,
                      uint maxCacheLineSize = 8192

        )
        {
            uint size = layout switch
            {
                L4ManagerCapacityLayout.Default => memorySize,
                L4ManagerCapacityLayout.AveragePerNuma => memorySize / (uint)Numa.AllNumaNode.Length,
                L4ManagerCapacityLayout.AveragePerThread => memorySize / (uint)Environment.ProcessorCount,
                L4ManagerCapacityLayout.PerNuma => memorySize,
                L4ManagerCapacityLayout.PerThread => memorySize,
                _ => throw new NotImplementedException($"Unknown layout type: {layout}"),
            };
            size = (size + 4095) / 4096;
            Dictionary<int, L4> memory = new(Numa.NumaNodeCount);
            foreach (NumaNode node in Numa.AllNumaNode)
            {
                int id = node.NumaNodeId;
                uint l4_size = layout switch
                {
                    L4ManagerCapacityLayout.Default => size,
                    L4ManagerCapacityLayout.AveragePerNuma => size,
                    L4ManagerCapacityLayout.AveragePerThread => size * (uint)node.LogicalCoreCount,
                    L4ManagerCapacityLayout.PerNuma => size,
                    L4ManagerCapacityLayout.PerThread => size * (uint)node.LogicalCoreCount,
                    _ => throw new NotImplementedException($"Unknown layout type: {layout}"),
                };
                memory.Add(id, new(id, l4_size, align, maxCacheLineSize));
            }
            _memory = memory.ToFrozenDictionary();
            ActiveThreadCount = _allThreads.Length;
        }

        internal void MakeThreads(
                      Func<TThread> threadFactory
        )
        {
            Dictionary<int, TThread[]> threads = new(Numa.NumaNodeCount);
            List<TThread> allThreads = [];
            foreach (NumaNode numaNode in Numa.AllNumaNode)
            {
                ReadOnlySpan<LogicalCore> cores = numaNode.LogicalCores;
                TThread[] nodeThreads = new TThread[cores.Length];
                for (int coreIndex = 0; coreIndex < cores.Length; coreIndex++)
                {
                    TThread t = threadFactory();
                    t.Initialize(numaNode, cores[coreIndex], _memory[numaNode.NumaNodeId]);
                    nodeThreads[coreIndex] = t;
                    allThreads.Add(t);
                }
                threads.Add(numaNode.NumaNodeId, nodeThreads);
            }
            _threadsInNuma = threads.ToFrozenDictionary();
            _allThreads = [.. allThreads];
        }

        protected virtual void Dispose(
                               bool disposing
        )
        {
            if (!disposedValue)
            {
                foreach (TThread t in _allThreads) {
                    t.Dispose();
                }

                foreach (L4 l in _memory.Values) {
                    l.Dispose();
                }
                disposedValue = true;
            }
        }

        private static int RetireWeakest(
                           IEnumerable<TThread> threads,
                           int maxRetireCount,
                           IComparer<TThread> comparer
        )
        {
            if (maxRetireCount <= 0) {
                return 0;
            }

            TThread[] candidates = threads
                .Where(thread => thread.State != L4AwareThreadState.Retired)
                .ToArray();
            Array.Sort(candidates, comparer);

            int retireCount = Math.Min(maxRetireCount, candidates.Length);
            for (int index = 0; index < retireCount; index++) {
                candidates[index].RequestStop();
            }

            return retireCount;
        }

        ~L4Manager()
        {
            Dispose(disposing:false);
        }

    }

    public enum L4ManagerRetirePolicy
    {

        /// <summary>
        ///   L4 manager will automatically select a retire policy based on current machine's
        ///   topology and numa node count. Selected policy will be either <see
        ///   cref="GlobalWeakest"> FourGlobalWeakest </see> or <see cref="PerNumaWeakest">
        ///   FourPerNumaWeakest </see>.
        /// </summary>
        Auto,
        /// <summary>
        ///   If current L4 runs in single numa node device, like a common home computer, then four
        ///   threads will be retired to leave resource for other threads. The four threads will be
        ///   selected from all threads in the system, and the weakest four threads will be retired.
        ///
        /// </summary>
        GlobalWeakest,
        /// <summary>
        ///   Similar to <see cref="FourGlobalWeakest" />, but current machine has multiple numa
        ///   nodes, commonly a HEDT workstation, so the four threads will be selected from all
        ///   threads in the same numa node, and the weakest four threads will be retired.
        /// </summary>
        PerNumaWeakest,
        /// <summary>
        ///   Only a quarter of amount and no more than four thread will stay alive. The rest will
        ///   be retired. This policy is suitable for a machine with no many threads, like a NUC.
        /// </summary>
        MinimumRemain,
        /// <summary>
        ///   Current L4 runs in a cluster's compute node, so only one or no thread need to be
        ///   retired.
        /// </summary>
        ClusterComputeNode,

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
