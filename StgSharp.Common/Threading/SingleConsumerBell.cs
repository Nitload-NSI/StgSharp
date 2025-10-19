//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="SingleConsumerBell"
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
using System;
using System.Threading;

namespace StgSharp.Threading
{
    /// <summary>
    ///   A single-consumer bell/signal primitive.
    /// </summary>
    /// <remarks>
    ///   <para> Producers can call <see cref="Set" /> multiple times to publish signals. The bell
    ///   keeps a pending count of not-yet-consumed signals. The consumer uses <see
    ///   cref="Wait(object, CancellationToken)" /> or <see cref="TryWait(object)" /> to consume
    ///   signals.</para> <para> When a producer increments the pending count from 0 to 1, the
    ///   internal event is set to wake the consumer. Consuming via <see cref="TryWait(object)" />
    ///   or <see cref="Wait(object, CancellationToken)" /> clears the entire pending count
    ///   (aggregate consumption). If new signals arrive while resetting, the event is set again to
    ///   avoid missed signals.</para> <para> Ownership: only the specified <c> owner </c> can call
    ///   the consumer APIs.</para>
    /// </remarks>
    public sealed class SingleConsumerBell : IDisposable
    {

        private volatile bool _disposed;
        private int _pending; // not volatile: used with Interlocked/Volatile APIs to avoid CS0420
        private readonly ManualResetEventSlim _evt;
        private readonly object _owner;

        /// <summary>
        ///   Initializes a new instance of <see cref="SingleConsumerBell" />.
        /// </summary>
        /// <param name="owner">
        ///   The unique consumer identity; subsequent calls to Wait/TryWait must pass the same
        ///   reference.
        /// </param>
        /// <param name="spinCount">
        ///   Spin count passed to the underlying <see cref="ManualResetEventSlim" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown if <paramref name="owner" /> is null.
        /// </exception>
        public SingleConsumerBell(object owner, int spinCount = 100)
        {
            _owner = owner ?? throw new ArgumentNullException(nameof(owner));
            _evt = new ManualResetEventSlim(false, spinCount);
        }

        /// <summary>
        ///   Disposes the bell, sets the event to wake any waiting consumer, and releases
        ///   resources.
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
            _evt.Set();
            _evt.Dispose();
        }

        /// <summary>
        ///   Publishes a signal by incrementing the not-yet-consumed count and returns the current
        ///   pending count.
        /// </summary>
        /// <remarks>
        ///   If the pending count transitions from 0 to 1, the event is set to wake the consumer.
        /// </remarks>
        /// <returns>
        ///   The current pending count including this call.
        /// </returns>
        public int Set()
        {
            if (_disposed) {
                return Volatile.Read(ref _pending);
            }

            // Increment pending; if we transitioned 0 -> 1, set the event to wake the consumer
            int newCount = Interlocked.Increment(ref _pending);
            if (newCount == 1) {
                _evt.Set();
            }
            return newCount;
        }

        /// <summary>
        ///   Attempts to consume any pending signals without blocking.
        /// </summary>
        /// <param name="consumer">
        ///   The consumer object; must be the same reference provided at construction.
        /// </param>
        /// <returns>
        ///   <c> true </c> if a signal was available (pending or event set); otherwise <c> false
        ///   </c>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///   Thrown if <paramref name="consumer" /> is not the owner.
        /// </exception>
        public bool TryWait(object consumer)
        {
            if (!ReferenceEquals(_owner, consumer)) {
                throw new InvalidOperationException("This bell is owned by another consumer.");
            }

            if (!_evt.IsSet && Volatile.Read(ref _pending) == 0) {
                return false;
            }

            // Aggregate consumption: clear pending and reset event
            _ = Interlocked.Exchange(ref _pending, 0);
            _evt.Reset();

            // If concurrent Set happened during reset, re-set the event to avoid lost signals
            if (Volatile.Read(ref _pending) != 0)
            {
                _evt.Set();
            }
            return true;
        }

        /// <summary>
        ///   Blocks until a signal is available, then consumes all pending signals.
        /// </summary>
        /// <param name="consumer">
        ///   The consumer object; must be the same reference provided at construction.
        /// </param>
        /// <param name="cancellationToken">
        ///   A token to cancel the wait.
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///   Thrown if <paramref name="consumer" /> is not the owner.
        /// </exception>
        public void Wait(object consumer, CancellationToken cancellationToken = default)
        {
            if (!ReferenceEquals(_owner, consumer)) {
                throw new InvalidOperationException("This bell is owned by another consumer.");
            }

            _evt.Wait(cancellationToken);

            // Consume this round of signals: clear pending and handle reset/concurrency
            _ = Interlocked.Exchange(ref _pending, 0);

            _evt.Reset();

            if (Volatile.Read(ref _pending) != 0) {
                _evt.Set();
            }
        }

    }
}