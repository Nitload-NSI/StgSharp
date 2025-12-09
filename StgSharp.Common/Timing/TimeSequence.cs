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
                                   double minSliceSeconds) => new LogTimeSequence(initialSliceSeconds,
                                                                                  totalSeconds,
                                                                                  growthMultiplier,
                                                                                  minSliceSeconds);

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
                PreviousFrameTick += _frameLengthTicks;
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
    ///   Logarithmic time sequence with growth factor and minimum slice length enforcement.
    /// </summary>
    internal sealed class LogTimeSequence : TimeSequence
    {

        private bool _finalEmitted;
        private readonly double _initialSeconds;
        private double _lastBoundarySecondsAdjusted;
        private readonly double _minSliceSeconds;
        private readonly double _multiplier;
        private double _nextBoundarySecondsAdjusted;
        private readonly double _totalSeconds;
        private int _sliceIndex;
        private long _freq;
        private long _nextBoundaryTick;
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
        public override bool EndsAt(long currentTick) => currentTick >= EndingTick;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool IsNextFrameReady(long currentTick)
        {
            // Emit final once when reaching or passing end.
            if (currentTick >= EndingTick)
            {
                if (_finalEmitted) {
                    return false;
                }

                PreviousFrameTick = EndingTick;
                _finalEmitted = true;
                return true;
            }

            // Not reached end: only when we cross the next boundary tick do we emit.
            if (currentTick < _nextBoundaryTick)
            {
                return false;
            }

            // Advance to this boundary.
            PreviousFrameTick = _nextBoundaryTick;
            _lastBoundarySecondsAdjusted = _nextBoundarySecondsAdjusted;
            _sliceIndex++;

            // Compute next cumulative boundary seconds with multiplier, then enforce min-slice and total cap.
            double next = _initialSeconds * Math.Pow(_multiplier, _sliceIndex);
            if (next > _totalSeconds) {
                next = _totalSeconds;
            }

            double delta = next - _lastBoundarySecondsAdjusted;
            if (delta < _minSliceSeconds)
            {
                next = _lastBoundarySecondsAdjusted + _minSliceSeconds;
                if (next > _totalSeconds) {
                    next = _totalSeconds;
                }
            }

            _nextBoundarySecondsAdjusted = next;
            _nextBoundaryTick = (_nextBoundarySecondsAdjusted >= _totalSeconds) ?
                                EndingTick :
                                BeginningTick + SecondsToTicks(_nextBoundarySecondsAdjusted);

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
            _nextBoundarySecondsAdjusted = _initialSeconds;
            _nextBoundaryTick = BeginningTick + SecondsToTicks(_nextBoundarySecondsAdjusted);
            PreviousFrameTick = BeginningTick;
            _finalEmitted = false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long SecondsToTicks(double seconds) => (long)Math.Max(1L, Math.Round(seconds * _freq, MidpointRounding.AwayFromZero));

    }
}
