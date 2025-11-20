//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="TimeSequence"
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
using StgSharp.Timing;
using System;
using System.Runtime.CompilerServices;

namespace StgSharp.Common.Timing
{
    public abstract class TimeSequence
    {

        public long BeginningTick { get; protected set; }

        public long EndingTick { get; protected set; }

        public long PreviousFrameTick { get; protected set; }

        public static TimeSequence CreateLinear(double totalSeconds, double frameSeconds) => new LinearTimeSequence(totalSeconds, frameSeconds);

        public static TimeSequence CreateLogarithmic(
                                   double initialSliceSeconds,
                                   double totalSeconds,
                                   double growthMultiplier,
                                   double minSliceSeconds) => new LogTimeSequence(initialSliceSeconds, totalSeconds,
                                                                                  growthMultiplier, minSliceSeconds);

        public abstract bool EndsAt(long currentTick);

        public abstract bool IsNextFrameReady(long currentTick);

        public abstract void StartFrom(TimeSourceProviderBase source);

    }

    internal sealed class LinearTimeSequence : TimeSequence
    {

        private readonly double _frameSeconds;
        private readonly double _totalSeconds;
        private long _frameLengthTicks;

        public LinearTimeSequence(double totalSeconds, double frameSeconds)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(totalSeconds);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(frameSeconds);
            if (frameSeconds > totalSeconds) {
                throw new ArgumentException("Frame length greater than total length.");
            }

            _totalSeconds = totalSeconds;
            _frameSeconds = frameSeconds;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool EndsAt(long currentTick) => currentTick >= EndingTick;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool IsNextFrameReady(long currentTick)
        {
            long span = currentTick - PreviousFrameTick;
            if (span >= _frameLengthTicks && currentTick < EndingTick)
            {
                PreviousFrameTick += _frameLengthTicks; // advance start of next frame
                return true;
            }
            return false;
        }

        public override void StartFrom(TimeSourceProviderBase source)
        {
            long f = source?.Frequency ?? throw new ArgumentNullException(nameof(source));
            _frameLengthTicks = (long)Math.Max(1, _frameSeconds * f);
            long totalTicks = (long)Math.Max(_frameLengthTicks, Math.Round(_totalSeconds * f, MidpointRounding.AwayFromZero));
            BeginningTick = source.GetCurrentTimeSpanTick();
            EndingTick = BeginningTick + totalTicks;
            EndingTick = EndingTick < 0 ? long.MaxValue : EndingTick;
            PreviousFrameTick = BeginningTick;
        }

    }

    /// <summary>
    ///   Logarithmic time sequence with growth factor. Boundaries (cumulative times since start)
    ///   follow initial * multiplier^k, but each slice duration (difference between adjusted
    ///   boundaries) is at least minSliceSeconds. First slice uses the exact initial length even if
    ///   below minSliceSeconds, subsequent slices enforce minimum. Example: initial=2,
    ///   multiplier=2, total>=8, min=3 => boundaries: 2,5,8 (durations:2,3,3).
    /// </summary>
    internal sealed class LogTimeSequence : TimeSequence
    {

        private readonly double _initialSeconds;
        private double _lastBoundarySecondsAdjusted; // adjusted cumulative seconds of last emitted boundary (0 at start)
        private readonly double _minSliceSeconds;
        private readonly double _multiplier;
        private double _nextBoundarySecondsAdjusted; // adjusted cumulative seconds for next boundary
        private readonly double _totalSeconds;

        private int _sliceIndex; // index of next boundary (0 => first boundary at initialSeconds)
        private long _freq;
        private long _nextBoundaryTick; // absolute tick for next boundary
        private long _totalTicks;

        public LogTimeSequence(
               double initialSliceSeconds,
               double totalSeconds,
               double growthMultiplier,
               double minSliceSeconds)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(initialSliceSeconds);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(totalSeconds);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(growthMultiplier);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(minSliceSeconds);
            if (growthMultiplier < 1.0) {
                throw new ArgumentException("growthMultiplier must be >= 1");
            }

            if (initialSliceSeconds > totalSeconds) {
                throw new ArgumentException("Initial slice length greater than total length.");
            }

            _initialSeconds = initialSliceSeconds;
            _totalSeconds = totalSeconds;
            _multiplier = growthMultiplier;
            _minSliceSeconds = minSliceSeconds;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool EndsAt(long currentTick) => currentTick >= EndingTick ||
                                                         _nextBoundarySecondsAdjusted >= _totalSeconds;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool IsNextFrameReady(long currentTick)
        {
            if (currentTick < _nextBoundaryTick || currentTick >= EndingTick) {
                return false;
            }

            // Emit frame: update previous frame tick and prepare next boundary.
            PreviousFrameTick = _nextBoundaryTick; // boundary just reached
            _lastBoundarySecondsAdjusted = _nextBoundarySecondsAdjusted;
            _sliceIndex++;

            // Compute original (unadjusted) next cumulative boundary in seconds.
            double originalNext = _initialSeconds * Math.Pow(_multiplier, _sliceIndex);

            // Enforce total cap.
            if (originalNext > _totalSeconds)
            {
                originalNext = _totalSeconds;
            }

            // Compute slice duration based on adjusted previous boundary.
            double delta = originalNext - _lastBoundarySecondsAdjusted;
            if (_sliceIndex > 0 && delta < _minSliceSeconds)
            {
                // Enforce minimum (except for first slice which uses initial value)
                originalNext = _lastBoundarySecondsAdjusted + _minSliceSeconds;
                if (originalNext > _totalSeconds) {
                    originalNext = _totalSeconds;
                }
            }
            _nextBoundarySecondsAdjusted = originalNext;

            if (_nextBoundarySecondsAdjusted >= _totalSeconds)
            {
                _nextBoundaryTick = EndingTick; // sequence will end after this boundary
            } else
            {
                _nextBoundaryTick = BeginningTick + SecondsToTicks(_nextBoundarySecondsAdjusted);
            }
            return true;
        }

        public override void StartFrom(TimeSourceProviderBase source)
        {
            _freq = source?.Frequency ?? throw new ArgumentNullException(nameof(source));
            BeginningTick = source.GetCurrentTimeSpanTick();
            _totalTicks = SecondsToTicks(_totalSeconds);
            EndingTick = BeginningTick + _totalTicks;
            _sliceIndex = 0;
            _lastBoundarySecondsAdjusted = 0.0;
            _nextBoundarySecondsAdjusted = _initialSeconds; // first boundary uses exact initial length
            _nextBoundaryTick = BeginningTick + SecondsToTicks(_nextBoundarySecondsAdjusted);
            PreviousFrameTick = BeginningTick; // start point
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long SecondsToTicks(double seconds) => (long)Math.Max(1L, Math.Round(seconds * _freq, MidpointRounding.AwayFromZero));

    }
}
