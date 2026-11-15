// -----------------------------------------------------------------------------
// file="MatrixParallelTask"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Common.HighPerformance.Memory;
using Nitload.Common.HighPerformance.ProcessorAbstraction;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Nitload.Mathematics.Numeric.Runtime
{
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public struct MatrixParallelSource
    {

        #region source

        [FieldOffset(0)] internal L4.AddressSource Source;
        [FieldOffset(4)] internal uint Size;
        [FieldOffset(8)] internal nuint SourceAddress;

        #endregion

        #region handle

        [FieldOffset(8)] internal L4.Handle Handle;
        [FieldOffset(8)] public nuint MappedAddress;
        [FieldOffset(16)] internal int Count;
        [FieldOffset(20)] internal int Offset;

        #endregion
    }

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct MatrixParallelTask
    {

        public MatrixParallelTask()
        {
            Unsafe.SkipInit(out this);
        }

        private static bool VerifySize { get; } = Unsafe.SizeOf<MatrixParallelTask>() == 128 ?
                                                  true :
                                                  throw new InvalidOleVariantTypeException($"Size of {nameof(MatrixParallelTask)} is not 128 bytes. Please verify runtime environment or concat lib maintainer.");

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy(
                           MatrixParallelTask* source,
                           MatrixParallelTask* target
        )
        {
            Buffer.MemoryCopy(source, target, sizeof(MatrixParallelTask),
                              sizeof(MatrixParallelTask));
        }

        #region mask

        [FieldOffset(64)] private M512 Hi;
        [FieldOffset(0)] private M512 Lo;

        #endregion

        #region layout

        [FieldOffset(0)] public MatrixParallelSource Buffer0;
        [FieldOffset(24)] public MatrixParallelSource Buffer1;
        [FieldOffset(48)] public MatrixParallelSource Buffer2;
        [FieldOffset(72)] public MatrixParallelSource Buffer3;
        [FieldOffset(96)] public MatrixElementType ElementType;
        [FieldOffset(98)] internal MatrixIntrinsicHandle ComputeHandle;
        [FieldOffset(100)] private uint _reverse4;
        [FieldOffset(104)] private ulong _reserved8;
        [FieldOffset(112)] public M128 ScalarPack;

        public override readonly bool Equals(
                                      object obj
        )
        {
            return (obj is not null) &&
                   (obj is MatrixParallelTask task) &&
                   Hi.Equals(task.Hi) &&
                   Lo.Equals(task.Lo);
        }

        public override readonly int GetHashCode()
        {
            return Hi.GetHashCode();
        }

        public static bool operator ==(
                                    MatrixParallelTask left,
                                    MatrixParallelTask right
        )
        {
            return left.Equals(right);
        }

        public static bool operator !=(
                                    MatrixParallelTask left,
                                    MatrixParallelTask right
        )
        {
            return !(left == right);
        }

        #endregion
    }
}
