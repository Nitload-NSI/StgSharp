//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="AdaptiveBackoffStrategy"
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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

/// <summary>
///   Efficient adaptive backoff strategy that dynamically selects optimal backoff methods based on
///   system load and success rate
/// </summary>
public struct AdaptiveBackoffStrategy
{

    private const int EVALUATION_INTERVAL_MS = 2000;   // Global re-evaluation interval
    private const int EXPLORATION_PROBABILITY = 10;     // 10% probability to explore non-optimal strategies
    private const int FAST_CONVERGENCE_THRESHOLD = 1000; // Fast convergence threshold

    // Configuration constants
    private const int INITIAL_SPINWAIT_THRESHOLD = 8;  // Cheap spin count for first few SpinWait iterations
    private const int MIN_SAMPLES_FOR_EVALUATION = 100; // Minimum sample count before starting evaluation

    // Global shared state (all threads share learning results)
    private static readonly PerformanceStats[] s_globalStats = new PerformanceStats[3];
    private static volatile BackoffMethod s_globalBestMethod;
    private static volatile int s_evaluationInProgress;
    private static long s_lastGlobalEvaluationTicks;
    private BackoffMethod _currentMethod;
    private int _consecutiveFailures;
    private int _localAttempts;

    // Instance state
    private SpinWait _spinWait;
    private uint _fastRng; // Lightweight pseudo-random number generator

    // Static constructor: initialize optimal strategy based on system characteristics
    static AdaptiveBackoffStrategy()
    {
        // Based on your benchmark results:
        // - Low core count (<= 8): SpinWait has clear advantage
        // - Medium core count (<= 16): Close performance, slightly favor SpinWait
        // - High core count (>= 20): Sleep0 has significant advantage under high concurrency
        int logicalCores = Environment.ProcessorCount;

        if (logicalCores <= 8)
        {
            s_globalBestMethod = BackoffMethod.SpinWait;
        } else if (logicalCores <= 16)
        {
            s_globalBestMethod = BackoffMethod.SpinWait; // Default conservative choice
        } else
        {
            // High core count systems, based on your 64-thread benchmark results   
            s_globalBestMethod = BackoffMethod.Sleep0;
        }

        s_lastGlobalEvaluationTicks = Environment.TickCount64;

        // Initialize statistics array
        for (int i = 0; i < s_globalStats.Length; i++)
        {
            s_globalStats[i].Reset();
        }
    }

    /// <summary>
    ///   Gets the currently used backoff method (for debugging/monitoring)
    /// </summary>
    public readonly BackoffMethod CurrentMethod => _currentMethod;

    /// <summary>
    ///   Gets the globally considered optimal backoff method
    /// </summary>
    public static BackoffMethod GlobalBestMethod => s_globalBestMethod;

    /// <summary>
    ///   Creates a new adaptive backoff strategy instance
    /// </summary>
    public static AdaptiveBackoffStrategy Create()
    {
        return new AdaptiveBackoffStrategy
        {
            _spinWait = default,
            _currentMethod = s_globalBestMethod,
            _consecutiveFailures = 0,
            _localAttempts = 0,
            _fastRng = (uint)(Environment.TickCount64 ^ Thread.CurrentThread.ManagedThreadId * 0x9E3779B9U)
        };
    }

    /// <summary>
    ///   Gets global statistics for the specified strategy (for monitoring)
    /// </summary>
    public static (long attempts, long successes, double successRate) GetGlobalStats(BackoffMethod method)
    {
        PerformanceStats stats = s_globalStats[(int)method];
        return (stats.Attempts, stats.Successes, stats.SuccessRate);
    }

    /// <summary>
    ///   Called after CAS failure, performs backoff and may switch strategies
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void OnFailureAndBackoff()
    {
        _consecutiveFailures++;

        // Record failure
        s_globalStats[(int)_currentMethod].RecordAttempt(success:false);

        // Periodically re-evaluate global optimal strategy
        if (ShouldEvaluateGlobalStrategy())
        {
            EvaluateAndUpdateGlobalStrategy();
        }

        // Decide whether to switch strategies based on failure count and current strategy
        if (_consecutiveFailures >= INITIAL_SPINWAIT_THRESHOLD && ShouldSwitchStrategy())
        {
            SwitchToOptimalStrategy();
        }

        // Perform the actual backoff operation
        PerformBackoff();
    }

    /// <summary>
    ///   Called after CAS success, resets state
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void OnSuccess()
    {
        // Record success
        s_globalStats[(int)_currentMethod].RecordAttempt(success:true);

        // Reset state
        _spinWait = default;
        _consecutiveFailures = 0;

        // Fast convergence: tend to keep current strategy when continuously successful
        if (_localAttempts < FAST_CONVERGENCE_THRESHOLD)
        {
            _localAttempts++;
        }
    }

    /// <summary>
    ///   Evaluates and updates the global optimal strategy
    /// </summary>
    private static void EvaluateAndUpdateGlobalStrategy()
    {
        // Use CAS to ensure only one thread performs evaluation
        if (Interlocked.CompareExchange(ref s_evaluationInProgress, 1, 0) != 0)
        {
            return;
        }

        try
        {
            long now = Environment.TickCount64;

            // Double-check to avoid duplicate evaluation
            if (now - Volatile.Read(ref s_lastGlobalEvaluationTicks) <= EVALUATION_INTERVAL_MS)
            {
                return;
            }

            BackoffMethod bestMethod = s_globalBestMethod;
            double bestSuccessRate = -1.0;

            // Find the strategy with the highest success rate
            for (int i = 0; i < s_globalStats.Length; i++)
            {
                PerformanceStats stats = s_globalStats[i];
                if (stats.Attempts >= MIN_SAMPLES_FOR_EVALUATION)
                {
                    double successRate = stats.SuccessRate;
                    if (successRate > bestSuccessRate)
                    {
                        bestSuccessRate = successRate;
                        bestMethod = (BackoffMethod)i;
                    }
                }
            }

            // Update global optimal strategy
            s_globalBestMethod = bestMethod;
            Volatile.Write(ref s_lastGlobalEvaluationTicks, now);

            // Reset statistics (maintain exploratory behavior)
            for (int i = 0; i < s_globalStats.Length; i++)
            {
                s_globalStats[i].Reset();
            }
        }
        finally
        {
            Volatile.Write(ref s_evaluationInProgress, 0);
        }
    }

    /// <summary>
    ///   Fast pseudo-random number generator (LCG)
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private uint NextRandom()
    {
        _fastRng = _fastRng * 1103515245U + 12345U;
        return _fastRng;
    }

    /// <summary>
    ///   Performs the actual backoff operation
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)] // Avoid inlining to reduce hot path code size
    private void PerformBackoff()
    {
        switch (_currentMethod)
        {
            case BackoffMethod.SpinWait:
                if (_consecutiveFailures <= INITIAL_SPINWAIT_THRESHOLD)
                {
                    _spinWait.SpinOnce(sleep1Threshold:-1);
                } else if (_spinWait.NextSpinWillYield)
                {
                    int yields = (int)(NextRandom() & 7) + 1; // 1-8 times
                    for (int i = 0; i < yields; i++) {
                        _ = Thread.Yield();
                    }
                } else
                {
                    _spinWait.SpinOnce(sleep1Threshold:-1);
                }
                break;

            case BackoffMethod.Yield:
                _ = Thread.Yield();
                break;

            case BackoffMethod.Sleep0:
                Thread.Sleep(0);
                break;
            default:
                break;
        }
    }

    /// <summary>
    ///   Checks whether global strategy evaluation should be performed
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool ShouldEvaluateGlobalStrategy()
    {
        long now = Environment.TickCount64;
        return now - Volatile.Read(ref s_lastGlobalEvaluationTicks) > EVALUATION_INTERVAL_MS;
    }

    /// <summary>
    ///   Determines whether strategy should be switched
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool ShouldSwitchStrategy()
    {
        // Fast convergence phase: reduce strategy switching
        if (_localAttempts < FAST_CONVERGENCE_THRESHOLD && _consecutiveFailures < 32)
        {
            return false;
        }

        // Small probability exploration: occasionally try other strategies even when current is optimal
        return NextRandom() % 100 < EXPLORATION_PROBABILITY || _currentMethod != s_globalBestMethod;
    }

    /// <summary>
    ///   Switches to the currently considered optimal strategy
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SwitchToOptimalStrategy()
    {
        BackoffMethod targetMethod;

        // Most of the time choose the globally optimal
        if (NextRandom() % 100 >= EXPLORATION_PROBABILITY)
        {
            targetMethod = s_globalBestMethod;
        } else
        {
            // Small probability random exploration
            targetMethod = (BackoffMethod)(NextRandom() % 3);
        }

        if (targetMethod != _currentMethod)
        {
            _currentMethod = targetMethod;
            _spinWait = default; // Reset SpinWait state when switching strategies
            _localAttempts = 0;
        }
    }

    public enum BackoffMethod : byte
    {

        SpinWait = 0,
        Yield = 1,
        Sleep0 = 2

    }

    private struct PerformanceStats
    {

        public long Attempts;
        public long LastResetTicks;
        public long Successes;

        public readonly double SuccessRate
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Attempts == 0 ? 0.0 : (double)Successes / Attempts;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RecordAttempt(bool success)
        {
            _ = Interlocked.Increment(ref Attempts);
            if (success) {
                _ = Interlocked.Increment(ref Successes);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            Attempts = 0;
            Successes = 0;
            LastResetTicks = Environment.TickCount64;
        }

    }

}