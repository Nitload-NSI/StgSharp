// -----------------------------------------------------------------------------
// file="MatrixParallelTask"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.HighPerformance.Memory;
using StgSharp.HighPerformance.ProcessorAbstraction;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Mathematics.Numeric.Runtime
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MatrixParallelSource
    {

        public MatrixParallelTask Task;
        public ulong Count;
        public ulong Cursor;

    }

    [StructLayout(LayoutKind.Explicit, Size = 128)]
    internal unsafe struct MatrixParallelTask
    {

        [FieldOffset(104)] private ulong _reserved0;

        [FieldOffset(0)] public L4.Handle Buffer0;
        [FieldOffset(16)] public L4.Handle Buffer1;
        [FieldOffset(32)] public L4.Handle Buffer2;
        [FieldOffset(48)] public L4.Handle Buffer3;
        [FieldOffset(112)] public M128 ScalarPack;
        [FieldOffset(64)] public M512 Profile;
        [FieldOffset(64)] public MatrixElementType ElementType;         //记录了类型掩码的类型位宽的union，大小4byte
        [FieldOffset(68)] public MatrixIntrinsicHandle ComputeHandle;    //记录了函数指针在类型组中的索引，实际就是int，但是为每一个索引起了名字，于是变成了enum，大小4byte

        public MatrixParallelTask()
        {
            Unsafe.SkipInit(out this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy(
                           MatrixParallelTask* source,
                           MatrixParallelTask* target
        )
        {
            Buffer.MemoryCopy(source, target, sizeof(MatrixParallelTask),
                              sizeof(MatrixParallelTask));
        }

        #region offset and length settings

        [FieldOffset(72)] public int Offset0;
        [FieldOffset(76)] public int Count0;
        [FieldOffset(80)] public int Offset1;
        [FieldOffset(84)] public int Count1;
        [FieldOffset(88)] public int Offset2;
        [FieldOffset(92)] public int Count2;
        [FieldOffset(96)] public int Offset3;
        [FieldOffset(100)] public int Count3;

    #endregion
    }
}
