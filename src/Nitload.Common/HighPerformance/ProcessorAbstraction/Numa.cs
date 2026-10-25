// -----------------------------------------------------------------------------
// file="Numa"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Common.HighPerformance.Memory;
using System.Runtime.InteropServices;

namespace Nitload.Common.HighPerformance.ProcessorAbstraction
{
    public static partial class Numa
    {

        private static NumaNode[] _numaNodes = [];

        public static int NumaNodeCount { get; private set; }

        public static ReadOnlySpan<NumaNode> AllNumaNode => _numaNodes;

        public static unsafe void* Alloc(
                                   int numaId,
                                   nuint size
        )
        {
            if (_numaNodes.Length == 0) {
                throw new InvalidOperationException("Numa info has not been loaded.");
            }
            if ((numaId < 0) || !ContainsNode((uint)numaId)) {
                throw new ArgumentOutOfRangeException(nameof(numaId), "Invalid NUMA node ID.");
            }
            return NumaAllocNative(size, numaId);
        }

        public static T[][] CreateForeachThread<T>(
                            Func<LogicalCore, T> factory
        )
        {
            if (factory is null) {
                return [];
            }
            T[][] arr = new T[_numaNodes.Length][];
            for (int i = 0; i < _numaNodes.Length; i++)
            {
                NumaNode node = _numaNodes[i];
                T[] values = new T[node.LogicalCoreCount];
                ReadOnlySpan<LogicalCore> cores = node.LogicalCores;
                for (int j = 0; j < cores.Length; j++) {
                    values[j] = factory(cores[j]);
                }
                arr[i] = values;
            }
            return arr;
        }

        public static unsafe void Free(
                                  void* address,
                                  int numaId,
                                  nuint size
        )
        {
            if (_numaNodes.Length == 0) {
                throw new InvalidOperationException("Numa info has not been loaded.");
            }
            if ((numaId < 0) || !ContainsNode((uint)numaId)) {
                throw new ArgumentOutOfRangeException(nameof(numaId), "Invalid NUMA node ID.");
            }
            NumaFreeNative(address, size, numaId);
        }

        public static L4[] RequestBufferForEachNuma()
        {
            if (_numaNodes.Length == 0) {
                throw new InvalidOperationException("Numa info has not been loaded.");
            }
            L4[] buffers = new L4[_numaNodes.Length];
            for (int i = 0; i < _numaNodes.Length; i++)
            {
                L4 buffer = new(_numaNodes[i].NumaNodeId);
                buffers[i] = buffer;
            }
            return buffers;
        }

        internal static void Initialize()
        {
            int numaNodeCount = 0;
            if (0 == GetNumaOverall(ref numaNodeCount, default)) {
                throw new InvalidOperationException("Failed to get NUMA overall information.");
            }
            int[] coreCountInNuma = new int[numaNodeCount];
            if (0 == GetNumaOverall(ref numaNodeCount, coreCountInNuma)) {
                throw new InvalidOperationException("Failed to get NUMA node processor counts.");
            }
            int activeNodeCount = 0;
            for (int i = 0; i < coreCountInNuma.Length; i++)
            {
                if (coreCountInNuma[i] > 0) {
                    activeNodeCount++;
                }
            }
            NumaNodeCount = activeNodeCount;
            _numaNodes = new NumaNode[activeNodeCount];
            int nodeIndex = 0;
            for (int numaId = 0; numaId < numaNodeCount; numaId++)
            {
                int coreCount = coreCountInNuma[numaId];
                if (coreCount == 0)
                {
                    continue;
                }
                _numaNodes[nodeIndex++] = InitNumaNode(numaId, coreCount);
            }
        }

        private static bool ContainsNode(
                            uint numaId
        )
        {
            foreach (NumaNode node in _numaNodes)
            {
                if (node.NumaNodeId == numaId) {
                    return true;
                }
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static NumaNode InitNumaNode(
                                int numaId,
                                int count
        )
        {
            Span<uint> processorGroupIds = stackalloc uint[count];
            Span<uint> socketIds = stackalloc uint[count];
            Span<uint> cpuIndices = stackalloc uint[count];
            Span<uint> numaNodeIds = stackalloc uint[count];
            if (0 == GetNumaEach(numaId, processorGroupIds, socketIds, cpuIndices, numaNodeIds)) {
                throw new InvalidOperationException($"Failed to enumerate processors for NUMA node {numaId}.");
            }
            return new(numaId, count, processorGroupIds, socketIds, cpuIndices, numaNodeIds);
        }

        #region native invoke

#pragma warning disable CA5393

        [LibraryImport(Native.LibName, EntryPoint = "nfn_get_numa_each")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static partial int GetNumaEach(
                                  int numaId,
                                  Span<uint> processorGroupIds,
                                  Span<uint> socketIds,
                                  Span<uint> cpuIndices,
                                  Span<uint> numaNodeIds
        );

        [LibraryImport(Native.LibName, EntryPoint = "nfn_get_numa_overall")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static partial int GetNumaOverall(
                                  ref int count,
                                  Span<int> coreCountInNuma
        );

        [LibraryImport(Native.LibName, EntryPoint = "nfn_set_thread_affinity")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        internal static partial void SetThreadAffinity(
                                     int cpuIndex,
                                     int processorGroupId
        );

        [LibraryImport(Native.LibName, EntryPoint = "nfn_numa_alloc")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        private static unsafe partial void* NumaAllocNative(
                                            nuint size,
                                            int numaId
        );

        [LibraryImport(Native.LibName, EntryPoint = "nfn_numa_free")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        private static unsafe partial void NumaFreeNative(
                                           void* ptr,
                                           nuint size,
                                           int numa_id
        );

#pragma warning restore CA5393

    #endregion
    }
}
