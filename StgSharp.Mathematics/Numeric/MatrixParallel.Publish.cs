//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallel.Publish"
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
using StgSharp.Collections;
using StgSharp.HighPerformance;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public static partial class MatrixParallel
    {

        private const int MIN_TASK_PER_THREAD = 256;
        private const int REC_TASK_PER_THREAD = 1024;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        internal static unsafe MatrixParallelHandle ScheduleTask(MatrixParallelTaskPackage* package)
        {
            MatrixParallelTaskPackage* nonGeneric = package;
            switch (nonGeneric->ComputeMode.OperationStyle)
            {
                case MatrixIndexStyle.BUFFER_INDEX:
                    return ScheduleBufferTask(nonGeneric);
                default:
                    throw new InvalidOperationException($"Unknown mode {nonGeneric->ComputeMode.OperationStyle}.");
            }
        }

        private static unsafe MatrixParallelHandle ScheduleBufferTask(
                                                   MatrixParallelTaskPackage* package)
        {
            int primCount = package->PrimCount;
            int secCount = package->SecCount;
            long count = (long)primCount * secCount;

            // Console.WriteLine($"amount of kernels is {count}");

            long recommendCount = count / 1024;
            MatrixParallelThread[] pool = LeadParallel((int)recommendCount);
            MatrixParallelHandle handle = new MatrixParallelHandle(pool);
            if (handle.Wraps.Length == 1)
            {
                // Console.WriteLine($"Scheduling into {pool.Length} threads.");
                ref long offsetRef = ref Unsafe.As<int, long>(ref package->PrimTileOffset);
                offsetRef = 0;
                ref long lengthRef = ref Unsafe.As<int, long>(ref package->PrimCount);
                lengthRef = count;
                handle.Wraps[0].WrapTask.Enqueue((nint)package);

                // Console.WriteLine($"Scheduling {count} kernels into thread");
                return handle;
            }
            long actual = count / pool.Length;
            long extra = count % pool.Length;

            long offset = 0;
            MatrixParallelTaskPackage* current = package;

            // Console.WriteLine($"Scheduling into {pool.Length} threads.");
            foreach (MatrixParallelWrap wrap in handle.Wraps)
            {
                long wrapTaskLength = actual * wrap.WrapTaskCapacity;
                long ex = Math.Min(extra, wrap.WrapTaskCapacity);
                long length = wrapTaskLength + ex;
                extra -= ex;
                ref long offsetRef = ref Unsafe.As<int, long>(ref current->PrimTileOffset);
                offsetRef = offset;
                ref long lengthRef = ref Unsafe.As<int, long>(ref current->PrimCount);
                lengthRef = length;
                wrap.WrapTask.Enqueue((nint) current);

                // Console.WriteLine($"Offset is {offset}");
                // Console.WriteLine($"Scheduling {length} kernels into {wrap.WrapTaskCapacity} threads");
                // Console.WriteLine();
                current = MatrixParallelFactory.FromExistPackage(current);
                offset += length;
            }
            return handle;
        }

    }
}
