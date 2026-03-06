//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="VariableCapacitySemaphore"
// Project: StgSharp.Common
// AuthorGroup: Nitload
// License: MIT
// -----------------------------------------------------------------------
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace StgSharp.Threading
{
    /// <summary>
    ///   A counting semaphore with adjustable maximum capacity at runtime. Implemented using one or
    ///   more ManualResetEventSlim (MRES) gates and atomic counters to avoid kernel semaphore
    ///   overhead while supporting high-frequency Release/Wait patterns.  Notes: - This is a
    ///   counting primitive (not a barrier). Permits accumulate up to <see cref="MaxPermits" />. -
    ///   Reducing <see cref="MaxPermits" /> does not revoke already issued permits; it only clamps
    ///   future releases. - Striped MRES can reduce contention in highly parallel waits.
    /// </summary>
    public sealed class VariableCapacitySemaphore : IDisposable
    {

        // MRES stripes (one gate per stripe). All stripes are set when transitioning 0 -> >0 and reset at >0 -> 0.
        private readonly ManualResetEventSlim[] _events;
        private volatile int _current;
        private volatile int _max;
        private readonly int _stripeMask; // for power-of-two stripe counts; otherwise -1

        /// <summary>
        ///   Create a new variable-capacity semaphore.
        /// </summary>
        /// <param name="initialCount">
        ///   Initial available permits (clamped to [0, maxPermits]).
        /// </param>
        /// <param name="maxPermits">
        ///   Logical maximum permits (>= 1).
        /// </param>
        /// <param name="stripes">
        ///   Optional stripe count for MRES (<= 0 to auto choose by CPU).
        /// </param>
        public VariableCapacitySemaphore(int initialCount = 0, int maxPermits = 1, int stripes = 0)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(maxPermits, 1);
            ArgumentOutOfRangeException.ThrowIfNegative(initialCount);

            _max = maxPermits;
            _current = Math.Min(initialCount, maxPermits);

            int stripeCount = stripes > 0 ? stripes : ClampToPow2(Math.Min(Environment.ProcessorCount, 8));
            _events = new ManualResetEventSlim[stripeCount];
            bool set = _current > 0;
            for (int i = 0; i < stripeCount; i++) {
                _events[i] = new ManualResetEventSlim(set, 0);
            }

            _stripeMask = IsPow2(stripeCount) ? (stripeCount - 1) : -1;
        }

        /// <summary>
        ///   Current number of available permits.
        /// </summary>
        public int CurrentCount => Volatile.Read(ref _current);

        /// <summary>
        ///   Logical maximum number of permits. Can be changed at runtime.
        /// </summary>
        public int MaxPermits
        {
            get => Volatile.Read(ref _max);
            set
            {
                ArgumentOutOfRangeException.ThrowIfLessThan(value, 1);

                Volatile.Write(ref _max, value);

                // Clamp current to not exceed the new max (do not block; let Wait() consume down).
                while (true)
                {
                    int cur = Volatile.Read(ref _current);
                    if (cur <= value)
                    {
                        break;
                    }

                    if (Interlocked.CompareExchange(ref _current, value, cur) == cur)
                    {
                        break;
                    }
                }

                // Reflect availability to gates
                if (Volatile.Read(ref _current) > 0)
                {
                    SetAll();
                } else
                {
                    ResetAll();
                }
            }
        }

        public void Dispose()
        {
            foreach (ManualResetEventSlim e in _events) {
                e.Dispose();
            }
        }

        /// <summary>
        ///   Release a single permit if under <see cref="MaxPermits" />. Returns the previous
        ///   count.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Release()
        {
            while (true)
            {
                int cur = Volatile.Read(ref _current);
                int max = Volatile.Read(ref _max);
                if (cur >= max) {
                    return cur;
                }

                if (Interlocked.CompareExchange(ref _current, cur + 1, cur) == cur)
                {
                    if (cur == 0) {
                        SetAll();
                    }

                    return cur;
                }
            }
        }

        /// <summary>
        ///   Release multiple permits (capped by <see cref="MaxPermits" />). Returns the previous
        ///   count.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Release(int permits)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(permits);
            while (true)
            {
                int cur = Volatile.Read(ref _current);
                int max = Volatile.Read(ref _max);
                if (cur >= max) {
                    return cur;
                }

                long target = (long)cur + permits;
                if (target > max) {
                    target = max;
                }

                if (Interlocked.CompareExchange(ref _current, (int)target, cur) == cur)
                {
                    if (cur == 0) {
                        SetAll();
                    }

                    return cur;
                }
            }
        }

        /// <summary>
        ///   Try to take a permit without blocking. Returns true if acquired.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryWait()
        {
            while (true)
            {
                int cur = Volatile.Read(ref _current);
                if (cur == 0) {
                    return false;
                }

                if (Interlocked.CompareExchange(ref _current, cur - 1, cur) == cur)
                {
                    if (cur - 1 == 0) {
                        ResetAll();
                    }

                    return true;
                }
            }
        }

        /// <summary>
        ///   Block until a permit is available, then consume one.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Wait()
        {
            ManualResetEventSlim gate = GetStripeEvent();
            SpinWait spinner = new SpinWait();
            while (true)
            {
                int cur = Volatile.Read(ref _current);
                if (cur > 0 && Interlocked.CompareExchange(ref _current, cur - 1, cur) == cur)
                {
                    if (cur - 1 == 0) {
                        ResetAll();
                    }

                    return;
                }
                if (!spinner.NextSpinWillYield)
                {
                    spinner.SpinOnce();
                } else
                {
                    gate.Wait();
                }
            }
        }

        /// <summary>
        ///   Block until a permit is available or canceled, then consume one.
        /// </summary>
        public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken = default)
        {
            ManualResetEventSlim gate = GetStripeEvent();
            SpinWait spinner = new SpinWait();
            int start = Environment.TickCount;
            int remaining = millisecondsTimeout;
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                int cur = Volatile.Read(ref _current);
                if (cur > 0 && Interlocked.CompareExchange(ref _current, cur - 1, cur) == cur)
                {
                    if (cur - 1 == 0) {
                        ResetAll();
                    }

                    return true;
                }
                if (!spinner.NextSpinWillYield)
                {
                    spinner.SpinOnce();
                    continue;
                }
                if (millisecondsTimeout < 0)
                {
                    gate.Wait(cancellationToken);
                    continue;
                }
                if (remaining <= 0) {
                    return false;
                }

                if (!gate.Wait(remaining, cancellationToken)) {
                    return false;
                }

                remaining = millisecondsTimeout - (Environment.TickCount - start);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int ClampToPow2(int x)
        {
            if (x < 1) {
                x = 1;
            }

            if (x > 8) {
                x = 8;
            }

            int p = 1;
            while ((p << 1) <= x) {
                p <<= 1;
            }

            return p;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ManualResetEventSlim GetStripeEvent()
        {
            int id = Environment.CurrentManagedThreadId & 0x7fffffff;
            if (_stripeMask != -1) {
                return _events[id & _stripeMask];
            }

            int idx = id % _events.Length;
            return _events[idx];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsPow2(int x) => (x & (x - 1)) == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ResetAll()
        {
            foreach (ManualResetEventSlim e in _events) {
                e.Reset();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetAll()
        {
            foreach (ManualResetEventSlim e in _events) {
                e.Set();
            }
        }

    }
}
