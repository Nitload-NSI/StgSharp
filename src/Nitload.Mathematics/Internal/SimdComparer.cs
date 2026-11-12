// -----------------------------------------------------------------------------
// file="SimdComparer"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Nitload.Mathematics.Numeric.Runtime
{
    internal partial class SimdComparer : IComparer<MatrixParallelThread>
    {

        private SimdComparer() { }

        public static SimdComparer Instance { get; } = new();

        public int Compare(MatrixParallelThread? x, MatrixParallelThread? y)
        {
            if (x is null && y is null) {
                return 0;
            }
            if (x is null) {
                return -1;
            }
            if (y is null) {
                return 1;
            }

            return CompareSIMDID(
                x.Simd,
                y.Simd
            );
        }

        [LibraryImport(Native.LibName, EntryPoint = "nfn_compare_simd")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        private static partial int CompareSIMDID(SIMDID left, SIMDID right);

    }
}
