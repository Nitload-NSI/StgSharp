//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="TimeSpanProvider.Awaiting"
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
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Timing
{
    public partial class TimeSpanProvider
    {

        private static readonly ConcurrentBufferStack<int> s_unusedID = new ConcurrentBufferStack<int>(8);

        private static int s_maxID;

        private EventHandler? _refreshHandler, _missHandler;
        private int _pendingSpans; // Number of unconsumed span ticks since last Wait.
        private int _size, _id;

        private readonly ManualResetEventSlim _event = new();
        private readonly object _participantObject = new();

        public event EventHandler MissTimeSpanRefreshed
        {
            add => _missHandler += value;
            remove => _missHandler -= value;
        }

        public event EventHandler TimeSpanRefreshed
        {
            add => _refreshHandler += value;
            remove => _refreshHandler -= value;
        }

        /// <summary>
        ///   True if there is at least one unconsumed span tick (PendingSpanCount &gt; 0).
        /// </summary>
        public bool HasPendingSpan => PendingSpanCount > 0;

        public int TokenID
        {
            get => _id;
            private set => _id = value;
        }

        /// <summary>
        ///   Number of span ticks not yet consumed by Wait ( &gt; 0 means last tick not waited).
        /// </summary>
        public int PendingSpanCount => Volatile.Read(ref _pendingSpans);

        internal ManualResetEventSlim AwaitingEvent
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _event;
        }

        public void Dispose()
        {
            // mark provider as ended so external users can observe terminal state
            Volatile.Write(ref _endedFlag, 1);
            s_unusedID.Push(TokenID);
            _event.Set();
            GC.SuppressFinalize(this);
        }

        public override int GetHashCode() => _id;

        public void Participant()
        {
            lock (_participantObject)
            {
                // Increase the maximum concurrent consumers for the next tick.
                _size++;
            }
        }

        public void WaitNextSpan()
        {
            _event.Wait();

            // Consume one span slot (clamp at 0 so multiple subscribers won't make it negative).
            while (true)
            {
                int cur = Volatile.Read(ref _pendingSpans);
                if (cur == 0)
                {
                    _event.Dispose();
                    return;
                }

                if (Interlocked.CompareExchange(ref _pendingSpans, cur - 1, cur) == cur)
                {
                    if (cur - 1 == 0)
                    {
                        // Last consumer resets the gate for the next tick.
                        _event.Reset();
                    }
                    return;
                }
            }
        }

        public async Task WaitNextSpanAsync()
        {
            await Task.Run(() => _event.Wait()).ConfigureAwait(false);
            while (true)
            {
                int cur = Volatile.Read(ref _pendingSpans);
                if (cur == 0)
                {
                    break;
                }

                if (Interlocked.CompareExchange(ref _pendingSpans, cur - 1, cur) == cur)
                {
                    if (cur - 1 == 0) {
                        _event.Reset();
                    }

                    break;
                }
            }
        }

        /// <summary>
        ///   Wait for the next timespan tick, but honor the provided cancellation token. Throws
        ///   OperationCanceledException when token is cancelled. If the provider is disposed during
        ///   wait an OperationCanceledException is thrown to indicate end.
        /// </summary>
        public Task WaitNextSpanAsync(CancellationToken ct)
        {
            try
            {
                // Block the current thread until the event is signaled or cancellation requested.
                _event.Wait(ct);
            }
            catch (ObjectDisposedException)
            {
                // provider disposed while waiting - treat as cancellation/ended
                throw new OperationCanceledException(ct);
            }

            // consume one pending span (same logic as non-cancellable variant)
            while (true)
            {
                int cur = Volatile.Read(ref _pendingSpans);
                if (cur == 0)
                {
                    break;
                }

                if (Interlocked.CompareExchange(ref _pendingSpans, cur - 1, cur) == cur)
                {
                    if (cur - 1 == 0) {
                        _event.Reset();
                    }

                    break;
                }
            }

            return Task.CompletedTask;
        }

        internal void MissRefresh() => _missHandler?.Invoke(this, EventArgs.Empty); // Legacy compatibility.

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentTick">
        ///   Tick passed since binded time source.
        /// </param>
        /// <returns>
        ///   True if the span provider is working. False if time is over (the sequence ended).
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool OnSpanTick(long currentTick)
        {
            if (_sequence.EndsAt(currentTick))
            {
                // mark ended due to sequence timeout, then dispose
                Volatile.Write(ref _endedFlag, 1);

                // Console.WriteLine("Time span ended.");
                Dispose();
                return false;
            }
            if (!_sequence.IsNextFrameReady(currentTick)) {
                return true;
            }

            // Console.WriteLine("Frame hit!");

            // Fast path: if no pending consumers, initialize the counter to current capacity (_size).
            if (Volatile.Read(ref _pendingSpans) == 0)
            {
                int toIssue = Math.Max(1, Volatile.Read(ref _size));
                if (Interlocked.CompareExchange(ref _pendingSpans, toIssue, 0) == 0)
                {
                    // Open the gate for all current waiters (up to `_size`).
                    _event.Set();
                    _refreshHandler?.Invoke(this, EventArgs.Empty);
                }
            } else
            {
                // A tick is already pending (or was just set by another thread): mark as miss.
                _missHandler?.Invoke(this, EventArgs.Empty);
            }
            return true;
        }

        ~TimeSpanProvider() => Dispose();

    }
}
