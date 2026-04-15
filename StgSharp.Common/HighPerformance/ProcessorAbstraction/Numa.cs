//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Numa"
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
using StgSharp.HighPerformance.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Common.HighPerformance.ProcessorAbstraction
{
    public static partial class Numa
    {

        private static NumaNode[] _numaNodes;

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
            if ((numaId < 0) || (numaId >= _numaNodes.Length)) {
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
            if ((numaId < 0) || (numaId >= _numaNodes.Length)) {
                throw new ArgumentOutOfRangeException(nameof(numaId), "Invalid NUMA node ID.");
            }
            NumaFreeNative(address, size, numaId);
        }

        [LibraryImport(Native.LibName, EntryPoint = "sn_get_numa_each")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static partial int GetNumaEach(
                                  uint numaId,
                                  Span<ulong> CpuIndexInNuma,
                                  Span<uint> globalCpuIdInNuma
        );

        [LibraryImport(Native.LibName, EntryPoint = "sn_get_numa_overall")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static partial int GetNumaOverall(
                                  ref int count,
                                  Span<int> coreCountInNuma
        );

        public static L4[] RequestBufferForEachNuma()
        {
            if (_numaNodes.Length == 0) {
                throw new InvalidOperationException("Numa info has not been loaded.");
            }
            L4[] buffers = new L4[NumaNodeCount];
            for (int i = 0; i < NumaNodeCount; i++)
            {
                L4 buffer = new L4(i);
                buffers[i] = buffer;
            }
            return buffers;
        }

        internal static void Initialize()
        {
            Span<int> coreCountInNuma = stackalloc int[(Environment.ProcessorCount / 32) + 1];
            int numaNodeCount = 0;
            if (0 == GetNumaOverall(ref numaNodeCount, coreCountInNuma)) {
                throw new InvalidOperationException("Failed to get NUMA overall information.");
            }
            _numaNodes = new NumaNode[numaNodeCount];
            for (uint i = 0; i < numaNodeCount; i++) {
                _numaNodes[i] = InitNumaNode(i, coreCountInNuma[(int)i]);
            }
        }

        [LibraryImport(Native.LibName, EntryPoint = "sn_set_thread_affinity")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        internal static partial void SetThreadAffinity(
                                     int coreId
        );

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static NumaNode InitNumaNode(
                                uint numaId,
                                int count
        )
        {
            Span<uint> globalIndex = stackalloc uint[count];
            _ = GetNumaEach(numaId, null, globalIndex);
            return new(numaId, count, globalIndex);
        }

        [LibraryImport(Native.LibName, EntryPoint = "sn_numa_alloc")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        private static unsafe partial void* NumaAllocNative(
                                            nuint size,
                                            int numaId
        );

        [LibraryImport(Native.LibName, EntryPoint = "sn_numa_free")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        private static unsafe partial void NumaFreeNative(
                                           void* ptr,
                                           nuint size,
                                           int numa_id
        );

    }
}
