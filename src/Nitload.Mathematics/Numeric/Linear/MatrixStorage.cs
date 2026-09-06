// -----------------------------------------------------------------------------
// file="MatrixStorage"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Common.HighPerformance.Memory;
using Nitload.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Tlsf = global::Nitload.Common.HighPerformance.Memory.TwoLayerSegregatedFitAllocator;

namespace Nitload.Mathematics.Numeric
{
    public abstract class MatrixStorage<T> : IDisposable where T : unmanaged, INumber<T>
    {

        internal unsafe ref MatrixKernel<T> this[
                                            long index
        ] => ref Unsafe.AsRef<MatrixKernel<T>>(BufferPointer + (MatrixKernel<T>.Size * index));

        protected internal abstract unsafe T* BufferPointer { get; }

        public abstract void Dispose();

    }

    internal class MatrixStorageHLSF<T>(
                   Tlsf allocator,
                   Tlsf.Handle handle
    ) : MatrixStorage<T> where T : unmanaged, INumber<T>
    {

        internal readonly Tlsf.Handle _handle = handle;

        internal readonly Tlsf _allocator = allocator;

        protected internal override unsafe T* BufferPointer => (T*)(ulong)_handle.Address;

        public override void Dispose()
        {
            allocator.Free(_handle);
        }

    }
}
