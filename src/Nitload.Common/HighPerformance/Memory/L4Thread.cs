// -----------------------------------------------------------------------------
// file="L4Thread"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Common.HighPerformance.ProcessorAbstraction;
using Nitload.Threading;

namespace Nitload.Common.HighPerformance.Memory
{
#pragma warning disable CA1063
    public abstract class L4AwareThread : IEquatable<L4AwareThread>, IDisposable
    {

        private bool _disposed;
        private bool _started;
        private volatile bool _stopRequested;
        private volatile L4AwareThreadState _state;
        private readonly ManualResetEventSlim _workSignal = new(false);
        private readonly object _lifeCycleLock = new();

        private readonly Thread _mainThread;

        protected L4AwareThread()
        {
            _mainThread = new Thread(Work);
        }

        public NumaNode? BindedNumaNode { get; private set; }

        public L4AwareThreadState State
        {
            get => _state;
            protected set => _state = value;
        }

        public LogicalCore BindedLogicalCore { get; private set; }

        public ThreadPriority AfterWork { get; set; }

        protected internal L4? BindedL4 { get; private set; }

        protected bool IsStopRequested => _stopRequested;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool Equals(
                    L4AwareThread? other
        )
        {
            return (other is not null) && ReferenceEquals(this, other);
        }

        public override bool Equals(
                             object? obj
        )
        {
            return obj is L4AwareThread other && Equals(other);
        }

        public override int GetHashCode()
        {
            return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this);
        }

        public void Initialize(
                    NumaNode node,
                    LogicalCore bindedLogicalCore,
                    L4? bindedL4 = null
        )
        {
            ArgumentNullException.ThrowIfNull(node);
            if (node.NumaNodeId != bindedLogicalCore.NumaNodeId) {
                throw new ArgumentException("The logical core must belong to the specified NUMA node.", nameof(bindedLogicalCore));
            }

            lock (_lifeCycleLock)
            {
                ObjectDisposedException.ThrowIf(_disposed, this);
                if (_started) {
                    throw new InvalidOperationException("A started worker thread cannot be reconfigured.");
                }
                BindedLogicalCore = bindedLogicalCore;
                BindedNumaNode = node;
                BindedL4 = bindedL4;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Start()
        {
            lock (_lifeCycleLock)
            {
                ObjectDisposedException.ThrowIf(_disposed, this);
                if (_started) {
                    return;
                }
                if (BindedNumaNode is null) {
                    throw new InvalidOperationException("The worker thread must be initialized before it is started.");
                }
                _started = true;
                _mainThread.Start();
            }
        }

        protected void SignalWork()
        {
            lock (_lifeCycleLock)
            {
                if (!_disposed) {
                    _workSignal.Set();
                }
            }
        }

        protected bool WaitForSignal()
        {
            _workSignal.Wait();
            _workSignal.Reset();
            return !_stopRequested;
        }

        private void Dispose(
                     bool disposing
        )
        {
            if (!disposing) {
                return;
            }
            if (ReferenceEquals(Thread.CurrentThread, _mainThread)) {
                throw new InvalidOperationException("A worker thread cannot dispose itself.");
            }

            bool joinThread;
            lock (_lifeCycleLock)
            {
                if (_disposed) {
                    return;
                }
                _disposed = true;
                _stopRequested = true;
                _workSignal.Set();
                joinThread = _started;
            }

            if (joinThread)
            {
                _mainThread.Join();
            } else
            {
                State = L4AwareThreadState.Retired;
            }

            try
            {
                CustomizeDispose();
            }
            finally
            {
                lock (_lifeCycleLock) {
                    _workSignal.Dispose();
                }
            }
        }

        private void Work()
        {
            State = L4AwareThreadState.Born;
            try
            {
                ThreadHelper.BindToLogicalCore(BindedLogicalCore);
                CustomizeInitialization();
                State = L4AwareThreadState.Ready;
                ThreadMain();
            }
            finally
            {
                State = L4AwareThreadState.Retired;
            }
        }

        #region customize phase

        protected abstract void ThreadMain();

        protected abstract void CustomizeInitialization();

        protected abstract void CustomizeDispose();

    #endregion
    }
#pragma warning restore CA1063

    [Flags]
    public enum L4AwareThreadState : int
    {
#pragma warning disable CA1008
        Born = 0,
        Ready = 1,
        Running = 2,
        Idle = 4,
        Retired = 8,
#pragma warning restore CA1008
    }
}
