//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="SamplingAdaptiveBackoff"
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
///   Optimized sampling-based adaptive backoff strategy that re-samples and evaluates the current
///   contention situation and adjusts the strategy after a certain number of fallbacks
/// </summary>
public struct SamplingAdaptiveBackoff
{
    /*
    private const int CONFIDENCE_THRESHOLD = 3;          // Requires N consecutive sampling confirmations to switch strategies

    // Configuration constants
    private const int FALLBACK_SAMPLING_THRESHOLD = 64;  // Triggers re-sampling every 64 fallbacks
    private const int INITIAL_SPINWAIT_THRESHOLD = 8;    // Cheap spinning for the first few SpinWait attempts
    private const int MAX_SAMPLING_WINDOW_MS = 1000;     // Maximum sampling window time
    private const int MIN_SAMPLING_WINDOW_MS = 100;      // Minimum sampling window time
    private const int PERFORMANCE_HISTORY_MASK = 7;      // Use 3-bit mask for 8 entries (power of 2)
    private const double SIGNIFICANT_IMPROVEMENT = 0.15; // 15% improvement is considered significant

    private BackoffMethod _candidateMethod;
    private BackoffMethod _currentMethod;
    private byte _confidenceCounter;
    private byte _consecutiveFailures;

    // Packed performance data to reduce memory footprint
    private PackedMethodPerformance _packedPerformances; // Bit-packed performance data for all 3 methods
    private SamplingWindow _currentSample;

    // Instance state
    private SpinWait _spinWait;
    private uint _rng;

    /// <summary>
    ///   Gets the current method
    /// </summary>
    public readonly BackoffMethod CurrentMethod => _currentMethod;

    /// <summary>
    ///   Creates a new optimized sampling adaptive backoff strategy instance
    /// </summary>
    public static SamplingAdaptiveBackoff Create()
    {
        // Choose initial strategy based on system characteristics
        BackoffMethod initialMethod = DetermineInitialMethod();

        SamplingAdaptiveBackoff instance = new()
        {
            _spinWait = default,
            _currentMethod = initialMethod,
            _consecutiveFailures = 0,
            _confidenceCounter = 0,
            _candidateMethod = initialMethod,
            _rng = GenerateInitialSeed(),
            _packedPerformances = PackedMethodPerformance.Initialize()
        };

        // Start the first sampling window
        instance._currentSample.Reset(initialMethod);

        return instance;
    }

    /// <summary>
    ///   Gets the current sampling status (for debugging)
    /// </summary>
    public readonly (int attempts, int successes, int fallbacks, double successRate, double fallbackRate) GetCurrentSampleStats()
    {
        return (_currentSample.TotalAttempts,
                _currentSample.SuccessfulAttempts,
                _currentSample.FallbackCount,
                _currentSample.SuccessRate,
                _currentSample.FallbackRate);
    }

    /// <summary>
    ///   Gets the historical performance of the specified method (for monitoring)
    /// </summary>
    public readonly (double successRate, double fallbackRate, double overallScore) GetMethodPerformance(
                                                                                   BackoffMethod method)
    {
        var (successRate, fallbackRate, score) = _packedPerformances.GetPerformance(method);
        return (successRate, fallbackRate, score);
    }

    /// <summary>
    ///   Called when attempting CAS (must be called regardless of success or failure)
    /// </summary>
    /// <param name="success">
    ///   Whether this CAS attempt was successful
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void OnCASAttempt(bool success)
    {
        _currentSample.TotalAttempts++;

        if (success)
        {
            _currentSample.SuccessfulAttempts++;
            _consecutiveFailures = 0;
            _spinWait = default; // Reset SpinWait after success
        } else
        {
            _consecutiveFailures++;
        }
    }

    /// <summary>
    ///   Called when CAS fails and fallback needs to be executed, performs backoff and may trigger
    ///   re-sampling
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void OnFallbackAndBackoff()
    {
        _currentSample.FallbackCount++;

        // Check if re-sampling threshold is reached using bitwise operations for faster comparison
        if ((_currentSample.FallbackCount & ~(FALLBACK_SAMPLING_THRESHOLD - 1)) != 0 ||
            _currentSample.ElapsedTicks > MAX_SAMPLING_WINDOW_MS)
        {
            if (_currentSample.ElapsedTicks >= MIN_SAMPLING_WINDOW_MS) {
                EvaluateCurrentSample();
            }
        }

        // Perform backoff using jump table approach for better branch prediction
        PerformBackoffOptimized();
    }

    /// <summary>
    ///   Determines initial method based on system characteristics
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static BackoffMethod DetermineInitialMethod()
    {
        int logicalCores = Environment.ProcessorCount;
        return logicalCores <= 16 ? BackoffMethod.SpinWait : BackoffMethod.Sleep0;
    }

    /// <summary>
    ///   Determines the method to test in the next sampling window using optimized random selection
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private BackoffMethod DetermineNextTestMethod()
    {
        // Use bitwise operations for 80%/20% probability split
        return (NextRandom() & 0xF) < 13 ? _candidateMethod : (BackoffMethod)(NextRandom() & 3);
    }

    /// <summary>
    ///   Evaluates the current sampling window and may adjust the strategy
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void EvaluateCurrentSample()
    {
        // Update performance record for the current method using packed data
        _packedPerformances.UpdatePerformance(
            _currentSample.TestedMethod, _currentSample.SuccessRate, _currentSample.FallbackRate,
            _currentSample.ElapsedTicks / Math.Max(1, _currentSample.TotalAttempts));

        // Find the currently best performing method
        BackoffMethod bestMethod = _packedPerformances.FindBestMethod();

        // Use branchless logic for confidence tracking
        bool methodChanged = bestMethod != _currentMethod;
        bool candidateMatches = bestMethod == _candidateMethod;

        _confidenceCounter = methodChanged ? (candidateMatches ? (byte)(_confidenceCounter + 1) : (byte)1) : (byte)0;

        if (methodChanged && candidateMatches && _confidenceCounter >= CONFIDENCE_THRESHOLD)
        {
            SwitchToMethod(bestMethod);
        } else if (methodChanged && !candidateMatches)
        {
            _candidateMethod = bestMethod;
        } else if (!methodChanged) {
            _candidateMethod = _currentMethod;
        }

        // Start new sampling window
        BackoffMethod nextTestMethod = DetermineNextTestMethod();
        _currentSample.Reset(nextTestMethod);

        if (nextTestMethod != _currentMethod)
        {
            _currentMethod = nextTestMethod;
            _spinWait = default;
        }
    }

    /// <summary>
    ///   Generates initial seed for RNG
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint GenerateInitialSeed()
    {
        return (uint)(Environment.TickCount64 ^ (Thread.CurrentThread.ManagedThreadId * 0x9E3779B9U));
    }

    /// <summary>
    ///   Fast pseudo-random number generator using XorShift32
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private uint NextRandom()
    {
        // XorShift32 - faster than LCG with better distribution
        _rng ^= _rng << 13;
        _rng ^= _rng >> 17;
        _rng ^= _rng << 5;
        return _rng;
    }

    /// <summary>
    ///   Optimized backoff operation using jump table approach
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void PerformBackoffOptimized()
    {
        // Use computed goto pattern for better branch prediction
        switch (_currentMethod)
        {
            case BackoffMethod.SpinWait:
                PerformSpinWaitBackoff();
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
    ///   Specialized SpinWait backoff with optimized logic
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void PerformSpinWaitBackoff()
    {
        if (_consecutiveFailures > INITIAL_SPINWAIT_THRESHOLD && _spinWait.NextSpinWillYield)
        {
            // Optimized random yield count using bitwise operations
            int yields = (int)(NextRandom() & 7) + 1;
            for (int i = 0; i < yields; i++) {
                _ = Thread.Yield();
            }
        } else
        {
            _spinWait.SpinOnce(sleep1Threshold:-1);
        }
    }

    /// <summary>
    ///   Switches to a new method with optimized state reset
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SwitchToMethod(BackoffMethod method)
    {
        _currentMethod = method;
        _candidateMethod = method;
        _confidenceCounter = 0;
        _spinWait = default;
    }

    public enum BackoffMethod : byte
    {

        SpinWait = 0,
        Yield = 1,
        Sleep0 = 2

    }

    // Optimized sampling window state with better packing
    private struct SamplingWindow
    {

        public BackoffMethod TestedMethod; // Current method being tested
        public int FallbackCount;         // Number of fallbacks
        public int SuccessfulAttempts;    // Number of successful attempts
        public int TotalAttempts;         // Total number of attempts
        public long StartTicks;           // Sampling window start time

        public readonly double SuccessRate
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => TotalAttempts == 0 ? 0.0 : (double)SuccessfulAttempts / TotalAttempts;
        }

        public readonly double FallbackRate
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => TotalAttempts == 0 ? 0.0 : (double)FallbackCount / TotalAttempts;
        }

        public readonly long ElapsedTicks
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Environment.TickCount64 - StartTicks;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset(BackoffMethod method)
        {
            StartTicks = Environment.TickCount64;
            TotalAttempts = 0;
            SuccessfulAttempts = 0;
            FallbackCount = 0;
            TestedMethod = method;
        }

    }

    // Bit-packed performance data structure for memory efficiency
    private struct PackedMethodPerformance
    {

        // Pack all 3 methods' performance into compact format
        private ulong _packed0; // Method 0 (SpinWait) data
        private ulong _packed1; // Method 1 (Yield) data  
        private ulong _packed2; // Method 2 (Sleep0) data

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly BackoffMethod FindBestMethod()
        {
            (double _, double _, double score0) = UnpackPerformanceData(_packed0);
            (double _, double _, double score1) = UnpackPerformanceData(_packed1);
            (double _, double _, double score2) = UnpackPerformanceData(_packed2);

            // Use branchless comparison for best performance
            BackoffMethod best = score1 > score0 + SIGNIFICANT_IMPROVEMENT ?
                                 BackoffMethod.Yield :
                                 BackoffMethod.SpinWait;
            best = score2 > GetScore(best) + SIGNIFICANT_IMPROVEMENT ? BackoffMethod.Sleep0 : best;

            return best;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly (double successRate, double fallbackRate, double overallScore) GetPerformance(
                                                                                       BackoffMethod method)
        {
            ulong packed = method switch
            {
                BackoffMethod.SpinWait => _packed0,
                BackoffMethod.Yield => _packed1,
                BackoffMethod.Sleep0 => _packed2,
                _ => _packed0
            };

            return UnpackPerformanceData(packed);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PackedMethodPerformance Initialize()
        {
            return new PackedMethodPerformance
            {
                _packed0 = PackPerformanceData(0.5, 0.5, 0),
                _packed1 = PackPerformanceData(0.5, 0.5, 0),
                _packed2 = PackPerformanceData(0.5, 0.5, 0)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdatePerformance(BackoffMethod method, double successRate, double fallbackRate, long avgLatency)
        {
            ulong packed = PackPerformanceData(successRate, fallbackRate, avgLatency);

            switch (method)
            {
                case BackoffMethod.SpinWait:
                    _packed0 = packed;
                    break;
                case BackoffMethod.Yield:
                    _packed1 = packed;
                    break;
                case BackoffMethod.Sleep0:
                    _packed2 = packed;
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly double GetScore(BackoffMethod method)
        {
            return method switch
            {
                BackoffMethod.SpinWait => UnpackPerformanceData(_packed0).overallScore,
                BackoffMethod.Yield => UnpackPerformanceData(_packed1).overallScore,
                BackoffMethod.Sleep0 => UnpackPerformanceData(_packed2).overallScore,
                _ => 0.0
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong PackPerformanceData(double successRate, double fallbackRate, long avgLatency)
        {
            // Pack data into 64-bit value: 20 bits success rate, 20 bits fallback rate, 24 bits latency
            uint success = (uint)(successRate * 1048575); // 20 bits (2^20 - 1)
            uint fallback = (uint)(fallbackRate * 1048575); // 20 bits
            uint latency = (uint)Math.Min(avgLatency, 16777215); // 24 bits (2^24 - 1)

            return ((ulong)success << 44) | ((ulong)fallback << 24) | latency;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (double successRate, double fallbackRate, double overallScore) UnpackPerformanceData(
                                                                                      ulong packed)
        {
            double successRate = ((packed >> 44) & 0xFFFFF) / 1048575.0;
            double fallbackRate = ((packed >> 24) & 0xFFFFF) / 1048575.0;
            long avgLatency = (long)(packed & 0xFFFFFF);

            // Calculate overall score with optimized formula
            double baseScore = successRate * 0.6 + (1.0 - fallbackRate) * 0.4;
            double latencyPenalty = avgLatency > 0 ? Math.Min(0.1, avgLatency / 10000.0) : 0;
            double overallScore = baseScore - latencyPenalty;

            return (successRate, fallbackRate, overallScore);
        }

    }
    /**/

}