//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallel.TaskFactory.cs"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the ¡°Software¡±), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//     
// The above copyright notice and 
// this permission notice shall be included in all copies 
// or substantial portions of the Software.
//     
// THE SOFTWARE IS PROVIDED ¡°AS IS¡±, 
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
// ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.HighPerformance;
using StgSharp.HighPerformance.Memory;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Mathematics
{
    /// <summary>
    ///   Factory for creating and managing MatrixParallelTask instances with pooled allocation Uses
    ///   separate SLAB allocators for tasks and scalar data packets
    /// </summary>
    public static unsafe class MatrixParallelTaskFactory
    {

        private static readonly SlabAllocator<ScalarDataPacket> ScalarPacketAllocator = 
            SlabAllocator<ScalarDataPacket>.Create(1024, SlabBufferLayout.Chunked, true);

        // SLAB allocators for pooled allocation
        private static readonly SlabAllocator<MatrixParallelTask> TaskAllocator = 
            SlabAllocator<MatrixParallelTask>.Create(2048, SlabBufferLayout.Chunked, true);

        /// <summary>
        ///   Create a task with no scalar parameters
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixTaskHandle CreateTask(
                                       MatrixKernel* left,
                                       MatrixKernel* right,
                                       MatrixKernel* result,
                                       IntPtr executionHandle)
        {
            nuint taskPtr = TaskAllocator.Allocate();
            MatrixParallelTask* task = (MatrixParallelTask*)taskPtr;

            task->left = left;
            task->right = right;
            task->result = result;
            task->executionHandle = executionHandle;
            task->scalerCount = 0;
            task->scalerDataPacket = 0; // null pointer

            return new MatrixTaskHandle(task, null, false);
        }

        /// <summary>
        ///   Create a task with 1 scalar parameter
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixTaskHandle CreateTask<T0>(
                                       MatrixKernel* left,
                                       MatrixKernel* right,
                                       MatrixKernel* result,
                                       IntPtr executionHandle,
                                       T0 scaler0)
            where T0: unmanaged, INumber<T0>
        {
            return CreateTaskInternal(left, right, result, executionHandle, 1, &InitializePacket<T0>, scaler0);
        }

        /// <summary>
        ///   Create a task with 2 scalar parameters
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixTaskHandle CreateTask<T0, T1>(
                                       MatrixKernel* left,
                                       MatrixKernel* right,
                                       MatrixKernel* result,
                                       IntPtr executionHandle,
                                       T0 scaler0,
                                       T1 scaler1)
            where T0: unmanaged, INumber<T0>
            where T1: unmanaged, INumber<T1>
        {
            return CreateTaskInternal(
                left, right, result, executionHandle, 2, &InitializePacket<T0, T1>, scaler0, scaler1);
        }

        /// <summary>
        ///   Create a task with 3 scalar parameters
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixTaskHandle CreateTask<T0, T1, T2>(
                                       MatrixKernel* left,
                                       MatrixKernel* right,
                                       MatrixKernel* result,
                                       IntPtr executionHandle,
                                       T0 scaler0,
                                       T1 scaler1,
                                       T2 scaler2)
            where T0: unmanaged, INumber<T0>
            where T1: unmanaged, INumber<T1>
            where T2: unmanaged, INumber<T2>
        {
            return CreateTaskInternal(
                left, right, result, executionHandle, 3, &InitializePacket<T0, T1, T2>, scaler0, scaler1, scaler2);
        }

        /// <summary>
        ///   Create a task with 4 scalar parameters
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixTaskHandle CreateTask<T0, T1, T2, T3>(
                                       MatrixKernel* left,
                                       MatrixKernel* right,
                                       MatrixKernel* result,
                                       IntPtr executionHandle,
                                       T0 scaler0,
                                       T1 scaler1,
                                       T2 scaler2,
                                       T3 scaler3)
            where T0: unmanaged, INumber<T0>
            where T1: unmanaged, INumber<T1>
            where T2: unmanaged, INumber<T2>
            where T3: unmanaged, INumber<T3>
        {
            return CreateTaskInternal(
                left, right, result, executionHandle, 4, &InitializePacket<T0, T1, T2, T3>, scaler0, scaler1, scaler2,
                scaler3);
        }

        /// <summary>
        ///   Create a task with 5 scalar parameters
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixTaskHandle CreateTask<T0, T1, T2, T3, T4>(
                                       MatrixKernel* left,
                                       MatrixKernel* right,
                                       MatrixKernel* result,
                                       IntPtr executionHandle,
                                       T0 scaler0,
                                       T1 scaler1,
                                       T2 scaler2,
                                       T3 scaler3,
                                       T4 scaler4)
            where T0: unmanaged, INumber<T0>
            where T1: unmanaged, INumber<T1>
            where T2: unmanaged, INumber<T2>
            where T3: unmanaged, INumber<T3>
            where T4: unmanaged, INumber<T4>
        {
            return CreateTaskInternal(
                left, right, result, executionHandle, 5, &InitializePacket<T0, T1, T2, T3, T4>, scaler0, scaler1,
                scaler2, scaler3, scaler4);
        }

        /// <summary>
        ///   Create a task with 6 scalar parameters
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixTaskHandle CreateTask<T0, T1, T2, T3, T4, T5>(
                                       MatrixKernel* left,
                                       MatrixKernel* right,
                                       MatrixKernel* result,
                                       IntPtr executionHandle,
                                       T0 scaler0,
                                       T1 scaler1,
                                       T2 scaler2,
                                       T3 scaler3,
                                       T4 scaler4,
                                       T5 scaler5)
            where T0: unmanaged, INumber<T0>
            where T1: unmanaged, INumber<T1>
            where T2: unmanaged, INumber<T2>
            where T3: unmanaged, INumber<T3>
            where T4: unmanaged, INumber<T4>
            where T5: unmanaged, INumber<T5>
        {
            return CreateTaskInternal(
                left, right, result, executionHandle, 6, &InitializePacket<T0, T1, T2, T3, T4, T5>, scaler0, scaler1,
                scaler2, scaler3, scaler4, scaler5);
        }

        /// <summary>
        ///   Create a task with 7 scalar parameters
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixTaskHandle CreateTask<T0, T1, T2, T3, T4, T5, T6>(
                                       MatrixKernel* left,
                                       MatrixKernel* right,
                                       MatrixKernel* result,
                                       IntPtr executionHandle,
                                       T0 scaler0,
                                       T1 scaler1,
                                       T2 scaler2,
                                       T3 scaler3,
                                       T4 scaler4,
                                       T5 scaler5,
                                       T6 scaler6)
            where T0: unmanaged, INumber<T0>
            where T1: unmanaged, INumber<T1>
            where T2: unmanaged, INumber<T2>
            where T3: unmanaged, INumber<T3>
            where T4: unmanaged, INumber<T4>
            where T5: unmanaged, INumber<T5>
            where T6: unmanaged, INumber<T6>
        {
            return CreateTaskInternal(
                left, right, result, executionHandle, 7, &InitializePacket<T0, T1, T2, T3, T4, T5, T6>, scaler0,
                scaler1, scaler2, scaler3, scaler4, scaler5, scaler6);
        }

        /// <summary>
        ///   Create a task with 8 scalar parameters (maximum)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixTaskHandle CreateTask<T0, T1, T2, T3, T4, T5, T6, T7>(
                                       MatrixKernel* left,
                                       MatrixKernel* right,
                                       MatrixKernel* result,
                                       IntPtr executionHandle,
                                       T0 scaler0,
                                       T1 scaler1,
                                       T2 scaler2,
                                       T3 scaler3,
                                       T4 scaler4,
                                       T5 scaler5,
                                       T6 scaler6,
                                       T7 scaler7)
            where T0: unmanaged, INumber<T0>
            where T1: unmanaged, INumber<T1>
            where T2: unmanaged, INumber<T2>
            where T3: unmanaged, INumber<T3>
            where T4: unmanaged, INumber<T4>
            where T5: unmanaged, INumber<T5>
            where T6: unmanaged, INumber<T6>
            where T7: unmanaged, INumber<T7>
        {
            return CreateTaskInternal(
                left, right, result, executionHandle, 8, &InitializePacket<T0, T1, T2, T3, T4, T5, T6, T7>, scaler0,
                scaler1, scaler2, scaler3, scaler4, scaler5, scaler6, scaler7);
        }

        /// <summary>
        ///   Get allocation statistics for monitoring pool usage
        /// </summary>
        public static (long TaskCount, long PacketCount, long TotalMemory) GetStatistics()
        {
            // Note: This would need to be implemented in SlabAllocator to get actual statistics
            // For now, return placeholder values
            return (0, 0, 0);
        }

        /// <summary>
        ///   Clean up all allocators (call on application shutdown)
        /// </summary>
        public static void Shutdown()
        {
            TaskAllocator?.Dispose();
            ScalarPacketAllocator?.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AllocateTaskAndPacket(
                            MatrixKernel* left,
                            MatrixKernel* right,
                            MatrixKernel* result,
                            IntPtr executionHandle,
                            int scalerCount,
                            out MatrixParallelTask* task,
                            out ScalarDataPacket* packet)
        {
            if (scalerCount is < 1 or > ScalarDataPacket.MaxScalarCount) {
                throw new ArgumentOutOfRangeException(nameof(scalerCount));
            }

            // Allocate task
            nuint taskPtr = TaskAllocator.Allocate();
            task = (MatrixParallelTask*)taskPtr;

            // Allocate scalar packet
            nuint packetPtr = ScalarPacketAllocator.Allocate();
            packet = (ScalarDataPacket*)packetPtr;

            // Initialize task
            task->left = left;
            task->right = right;
            task->result = result;
            task->executionHandle = executionHandle;
            task->scalerCount = scalerCount;

            // Clear packet memory
            Unsafe.InitBlock(packet, 0, ScalarDataPacket.PacketSize);
        }

        /// <summary>
        ///   Internal helper for creating tasks with scalar packets
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static MatrixTaskHandle CreateTaskInternal<T0>(
                                        MatrixKernel* left,
                                        MatrixKernel* right,
                                        MatrixKernel* result,
                                        IntPtr executionHandle,
                                        int scalerCount,
                                        delegate*<ScalarDataPacket*, T0, void> packetInitializer,
                                        T0 param0)
            where T0: unmanaged
        {
            AllocateTaskAndPacket(
                left, right, result, executionHandle, scalerCount, out MatrixParallelTask* task,
                out ScalarDataPacket* packet);
            packetInitializer(packet, param0);
            task->scalerDataPacket = (nint)packet;
            return new MatrixTaskHandle(task, packet, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static MatrixTaskHandle CreateTaskInternal<T0, T1>(
                                        MatrixKernel* left,
                                        MatrixKernel* right,
                                        MatrixKernel* result,
                                        IntPtr executionHandle,
                                        int scalerCount,
                                        delegate*<ScalarDataPacket*, T0, T1, void> packetInitializer,
                                        T0 param0,
                                        T1 param1)
            where T0: unmanaged
            where T1: unmanaged
        {
            AllocateTaskAndPacket(
                left, right, result, executionHandle, scalerCount, out MatrixParallelTask* task,
                out ScalarDataPacket* packet);
            packetInitializer(packet, param0, param1);
            task->scalerDataPacket = (nint)packet;
            return new MatrixTaskHandle(task, packet, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static MatrixTaskHandle CreateTaskInternal<T0, T1, T2>(
                                        MatrixKernel* left,
                                        MatrixKernel* right,
                                        MatrixKernel* result,
                                        IntPtr executionHandle,
                                        int scalerCount,
                                        delegate*<ScalarDataPacket*, T0, T1, T2, void> packetInitializer,
                                        T0 param0,
                                        T1 param1,
                                        T2 param2)
            where T0: unmanaged
            where T1: unmanaged
            where T2: unmanaged
        {
            AllocateTaskAndPacket(
                left, right, result, executionHandle, scalerCount, out MatrixParallelTask* task,
                out ScalarDataPacket* packet);
            packetInitializer(packet, param0, param1, param2);
            task->scalerDataPacket = (nint)packet;
            return new MatrixTaskHandle(task, packet, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static MatrixTaskHandle CreateTaskInternal<T0, T1, T2, T3>(
                                        MatrixKernel* left,
                                        MatrixKernel* right,
                                        MatrixKernel* result,
                                        IntPtr executionHandle,
                                        int scalerCount,
                                        delegate*<ScalarDataPacket*, T0, T1, T2, T3, void> packetInitializer,
                                        T0 param0,
                                        T1 param1,
                                        T2 param2,
                                        T3 param3)
            where T0: unmanaged
            where T1: unmanaged
            where T2: unmanaged
            where T3: unmanaged
        {
            AllocateTaskAndPacket(
                left, right, result, executionHandle, scalerCount, out MatrixParallelTask* task,
                out ScalarDataPacket* packet);
            packetInitializer(packet, param0, param1, param2, param3);
            task->scalerDataPacket = (nint)packet;
            return new MatrixTaskHandle(task, packet, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static MatrixTaskHandle CreateTaskInternal<T0, T1, T2, T3, T4>(
                                        MatrixKernel* left,
                                        MatrixKernel* right,
                                        MatrixKernel* result,
                                        IntPtr executionHandle,
                                        int scalerCount,
                                        delegate*<ScalarDataPacket*, T0, T1, T2, T3, T4, void> packetInitializer,
                                        T0 param0,
                                        T1 param1,
                                        T2 param2,
                                        T3 param3,
                                        T4 param4)
            where T0: unmanaged
            where T1: unmanaged
            where T2: unmanaged
            where T3: unmanaged
            where T4: unmanaged
        {
            AllocateTaskAndPacket(
                left, right, result, executionHandle, scalerCount, out MatrixParallelTask* task,
                out ScalarDataPacket* packet);
            packetInitializer(packet, param0, param1, param2, param3, param4);
            task->scalerDataPacket = (nint)packet;
            return new MatrixTaskHandle(task, packet, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static MatrixTaskHandle CreateTaskInternal<T0, T1, T2, T3, T4, T5>(
                                        MatrixKernel* left,
                                        MatrixKernel* right,
                                        MatrixKernel* result,
                                        IntPtr executionHandle,
                                        int scalerCount,
                                        delegate*<ScalarDataPacket*, T0, T1, T2, T3, T4, T5, void> packetInitializer,
                                        T0 param0,
                                        T1 param1,
                                        T2 param2,
                                        T3 param3,
                                        T4 param4,
                                        T5 param5)
            where T0: unmanaged
            where T1: unmanaged
            where T2: unmanaged
            where T3: unmanaged
            where T4: unmanaged
            where T5: unmanaged
        {
            AllocateTaskAndPacket(
                left, right, result, executionHandle, scalerCount, out MatrixParallelTask* task,
                out ScalarDataPacket* packet);
            packetInitializer(packet, param0, param1, param2, param3, param4, param5);
            task->scalerDataPacket = (nint)packet;
            return new MatrixTaskHandle(task, packet, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static MatrixTaskHandle CreateTaskInternal<T0, T1, T2, T3, T4, T5, T6>(
                                        MatrixKernel* left,
                                        MatrixKernel* right,
                                        MatrixKernel* result,
                                        IntPtr executionHandle,
                                        int scalerCount,
                                        delegate*<ScalarDataPacket*, T0, T1, T2, T3, T4, T5, T6, void> packetInitializer,
                                        T0 param0,
                                        T1 param1,
                                        T2 param2,
                                        T3 param3,
                                        T4 param4,
                                        T5 param5,
                                        T6 param6)
            where T0: unmanaged
            where T1: unmanaged
            where T2: unmanaged
            where T3: unmanaged
            where T4: unmanaged
            where T5: unmanaged
            where T6: unmanaged
        {
            AllocateTaskAndPacket(
                left, right, result, executionHandle, scalerCount, out MatrixParallelTask* task,
                out ScalarDataPacket* packet);
            packetInitializer(packet, param0, param1, param2, param3, param4, param5, param6);
            task->scalerDataPacket = (nint)packet;
            return new MatrixTaskHandle(task, packet, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static MatrixTaskHandle CreateTaskInternal<T0, T1, T2, T3, T4, T5, T6, T7>(
                                        MatrixKernel* left,
                                        MatrixKernel* right,
                                        MatrixKernel* result,
                                        IntPtr executionHandle,
                                        int scalerCount,
                                        delegate*<ScalarDataPacket*, T0, T1, T2, T3, T4, T5, T6, T7, void> packetInitializer,
                                        T0 param0,
                                        T1 param1,
                                        T2 param2,
                                        T3 param3,
                                        T4 param4,
                                        T5 param5,
                                        T6 param6,
                                        T7 param7)
            where T0: unmanaged
            where T1: unmanaged
            where T2: unmanaged
            where T3: unmanaged
            where T4: unmanaged
            where T5: unmanaged
            where T6: unmanaged
            where T7: unmanaged
        {
            AllocateTaskAndPacket(
                left, right, result, executionHandle, scalerCount, out MatrixParallelTask* task,
                out ScalarDataPacket* packet);
            packetInitializer(packet, param0, param1, param2, param3, param4, param5, param6, param7);
            task->scalerDataPacket = (nint)packet;
            return new MatrixTaskHandle(task, packet, true);
        }

        // Static packet initializer functions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void InitializePacket<T0>(ScalarDataPacket* packet, T0 s0) where T0: unmanaged, INumber<T0>
        {
            packet->GetScalerRef<T0>(0) = s0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void InitializePacket<T0, T1>(ScalarDataPacket* packet, T0 s0, T1 s1)
            where T0: unmanaged, INumber<T0>
            where T1: unmanaged, INumber<T1>
        {
            packet->GetScalerRef<T0>(0) = s0;
            packet->GetScalerRef<T1>(1) = s1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void InitializePacket<T0, T1, T2>(ScalarDataPacket* packet, T0 s0, T1 s1, T2 s2)
            where T0: unmanaged, INumber<T0>
            where T1: unmanaged, INumber<T1>
            where T2: unmanaged, INumber<T2>
        {
            packet->GetScalerRef<T0>(0) = s0;
            packet->GetScalerRef<T1>(1) = s1;
            packet->GetScalerRef<T2>(2) = s2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void InitializePacket<T0, T1, T2, T3>(ScalarDataPacket* packet, T0 s0, T1 s1, T2 s2, T3 s3)
            where T0: unmanaged, INumber<T0>
            where T1: unmanaged, INumber<T1>
            where T2: unmanaged, INumber<T2>
            where T3: unmanaged, INumber<T3>
        {
            packet->GetScalerRef<T0>(0) = s0;
            packet->GetScalerRef<T1>(1) = s1;
            packet->GetScalerRef<T2>(2) = s2;
            packet->GetScalerRef<T3>(3) = s3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void InitializePacket<T0, T1, T2, T3, T4>(
                            ScalarDataPacket* packet,
                            T0 s0,
                            T1 s1,
                            T2 s2,
                            T3 s3,
                            T4 s4)
            where T0: unmanaged, INumber<T0>
            where T1: unmanaged, INumber<T1>
            where T2: unmanaged, INumber<T2>
            where T3: unmanaged, INumber<T3>
            where T4: unmanaged, INumber<T4>
        {
            packet->GetScalerRef<T0>(0) = s0;
            packet->GetScalerRef<T1>(1) = s1;
            packet->GetScalerRef<T2>(2) = s2;
            packet->GetScalerRef<T3>(3) = s3;
            packet->GetScalerRef<T4>(4) = s4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void InitializePacket<T0, T1, T2, T3, T4, T5>(
                            ScalarDataPacket* packet,
                            T0 s0,
                            T1 s1,
                            T2 s2,
                            T3 s3,
                            T4 s4,
                            T5 s5)
            where T0: unmanaged, INumber<T0>
            where T1: unmanaged, INumber<T1>
            where T2: unmanaged, INumber<T2>
            where T3: unmanaged, INumber<T3>
            where T4: unmanaged, INumber<T4>
            where T5: unmanaged, INumber<T5>
        {
            packet->GetScalerRef<T0>(0) = s0;
            packet->GetScalerRef<T1>(1) = s1;
            packet->GetScalerRef<T2>(2) = s2;
            packet->GetScalerRef<T3>(3) = s3;
            packet->GetScalerRef<T4>(4) = s4;
            packet->GetScalerRef<T5>(5) = s5;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void InitializePacket<T0, T1, T2, T3, T4, T5, T6>(
                            ScalarDataPacket* packet,
                            T0 s0,
                            T1 s1,
                            T2 s2,
                            T3 s3,
                            T4 s4,
                            T5 s5,
                            T6 s6)
            where T0: unmanaged, INumber<T0>
            where T1: unmanaged, INumber<T1>
            where T2: unmanaged, INumber<T2>
            where T3: unmanaged, INumber<T3>
            where T4: unmanaged, INumber<T4>
            where T5: unmanaged, INumber<T5>
            where T6: unmanaged, INumber<T6>
        {
            packet->GetScalerRef<T0>(0) = s0;
            packet->GetScalerRef<T1>(1) = s1;
            packet->GetScalerRef<T2>(2) = s2;
            packet->GetScalerRef<T3>(3) = s3;
            packet->GetScalerRef<T4>(4) = s4;
            packet->GetScalerRef<T5>(5) = s5;
            packet->GetScalerRef<T6>(6) = s6;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void InitializePacket<T0, T1, T2, T3, T4, T5, T6, T7>(
                            ScalarDataPacket* packet,
                            T0 s0,
                            T1 s1,
                            T2 s2,
                            T3 s3,
                            T4 s4,
                            T5 s5,
                            T6 s6,
                            T7 s7)
            where T0: unmanaged, INumber<T0>
            where T1: unmanaged, INumber<T1>
            where T2: unmanaged, INumber<T2>
            where T3: unmanaged, INumber<T3>
            where T4: unmanaged, INumber<T4>
            where T5: unmanaged, INumber<T5>
            where T6: unmanaged, INumber<T6>
            where T7: unmanaged, INumber<T7>
        {
            packet->GetScalerRef<T0>(0) = s0;
            packet->GetScalerRef<T1>(1) = s1;
            packet->GetScalerRef<T2>(2) = s2;
            packet->GetScalerRef<T3>(3) = s3;
            packet->GetScalerRef<T4>(4) = s4;
            packet->GetScalerRef<T5>(5) = s5;
            packet->GetScalerRef<T6>(6) = s6;
            packet->GetScalerRef<T7>(7) = s7;
        }

        /// <summary>
        ///   Task handle containing both task and scalar packet pointers
        /// </summary>
        public readonly struct MatrixTaskHandle : IDisposable
        {

            internal readonly ScalarDataPacket* ScalarPacketPtr;
            internal readonly MatrixParallelTask* TaskPtr;
            public readonly bool HasScalarPacket;

            internal MatrixTaskHandle(
                     MatrixParallelTask* taskPtr,
                     ScalarDataPacket* scalarPacketPtr,
                     bool hasScalarPacket)
            {
                TaskPtr = taskPtr;
                ScalarPacketPtr = scalarPacketPtr;
                HasScalarPacket = hasScalarPacket;
            }

            public bool IsValid => TaskPtr != null;

            /// <summary>
            ///   Release the allocated resources back to pools
            /// </summary>
            public void Dispose()
            {
                if (IsValid)
                {
                    TaskAllocator.Free((nuint)TaskPtr);
                    if (HasScalarPacket && ScalarPacketPtr != null) {
                        ScalarPacketAllocator.Free((nuint)ScalarPacketPtr);
                    }
                }
            }

            /// <summary>
            ///   Execute the task
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly void Execute()
            {
                if (!IsValid) {
                    throw new InvalidOperationException("Invalid task handle");
                }

                TaskPtr->Execute();
            }

            /// <summary>
            ///   Execute the task and automatically dispose the handle
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void ExecuteAndDispose()
            {
                try
                {
                    Execute();
                }
                finally
                {
                    Dispose();
                }
            }

        }

    }
}