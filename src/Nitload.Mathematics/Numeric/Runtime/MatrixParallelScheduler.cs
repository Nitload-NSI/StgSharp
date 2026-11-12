// -----------------------------------------------------------------------------
// file="MatrixParallelScheduler"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Common.HighPerformance.Memory;
using System.Runtime.CompilerServices;

using TLSF = Nitload.Common.HighPerformance.Memory.TwoLayerSegregatedFitAllocator;

namespace Nitload.Mathematics.Numeric.Runtime
{
    [InlineArray(256)]
    internal struct MatrixSchedulerBufferFragment
    {

        public MatrixParallelTask Task;

    }

    internal abstract unsafe class MatrixParallelScheduler(
                                   TLSF.Handle handle,
                                   int capacity
    ) : IL4Predictor<MatrixParallelTask>, IDisposable
    {

        private readonly MatrixParallelTask* _currentPack = (MatrixParallelTask*)(nuint)handle.Address;

        private bool disposedValue;
        private int _consumeCursor;
        private int _predictCursor;
        private int _pubCursor;

        public int Id { get; set; }

        protected bool IsFullyPredicted { get; set; }

        public abstract int PredictMission(
                            in Span<L4.AddressSource> RequiredBuffer
        );

        public abstract void Prefetch(
                             L4.AddressSource RequiredBuffer,
                             nuint allocBuffer,
                             nuint bufferSize
        );

        public bool TryGetNextMission(
                    out MatrixParallelTask mission
        )
        {
            if (Volatile.Read(ref _pubCursor) <= Volatile.Read(ref _consumeCursor))
            {
                mission = default;
                return false;
            }
            int consume_cursor = Interlocked.Increment(ref _consumeCursor) - 1;
            mission = _currentPack[consume_cursor % capacity];
            return true;
        }

        /// <summary>
        ///   This method runs in a sync region
        /// </summary>
        /// <returns>
        ///
        /// </returns>
        protected MatrixParallelTask* GetCurrentPack()
        {
            while (Volatile.Read(ref _predictCursor) - Volatile.Read(ref _consumeCursor) >= capacity) {
                Thread.Sleep(1);
            }

            int slot = Interlocked.Increment(ref _predictCursor) - 1;
            MatrixParallelTask* ptr = &_currentPack[slot % capacity];
            InitTaskDescription(ptr);

            // maybe i can move task profile to L4 span here
            return ptr;
        }

        protected abstract void InitTaskDescription(
                                MatrixParallelTask* task
        );

        #region dispose

        // ~MatrixParallelQueue()
        // {
        // Dispose(disposing: false);
        // }

        public void Dispose()
        {
            Dispose(disposing:true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(
                               bool disposing
        )
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // _exchangeLock.Dispose();
                }

                disposedValue = true;
            }
        }

        #endregion
    }
}

