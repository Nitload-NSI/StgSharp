//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="TimeFunction"
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
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.Timing
{
    public class TimeFunction
    {

        private float _beginning, _ending;
        private TimeSpanProvider _timeProvider;

        internal TimeFunction() { }

        public TimeFunction(TimeSpanProvider timeSpanProvider, Func<float, float> timeRelatedCalculation, float range)
        {
            _timeProvider = timeSpanProvider;
            TimeRelatedCalculation = timeRelatedCalculation;
            if (timeSpanProvider == null) {
                return;
            }
            _beginning = _timeProvider.CurrentSecond;
            _ending = _timeProvider.CurrentSecond + range;
        }

        public virtual bool IsComplete
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _timeProvider.CurrentSecond > _ending;
        }

        public Func<float, float> TimeRelatedCalculation { get; set; }

        public static TimeFunction One
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new TimeFunctionOne();
        }

        public TimeSpanProvider TimeSpanProvider
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _timeProvider;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _timeProvider = value;
        }

        public virtual float Calculate()
        {
            if (_timeProvider.CurrentSecond < _ending) {
                return 0;
            }
            return TimeRelatedCalculation(_timeProvider.CurrentSecond - _beginning);
        }

        public static TimeFunction Proportional(TimeSpanProvider time, float begin, float range)
        {
            return new TimeFunction(time, (float t) => t + begin, range);
        }

        public virtual TimeFunction Renew((Func<float, float> func, float range) funcPair)
        {
            return new TimeFunction(_timeProvider, funcPair.func, funcPair.range);
        }

        public virtual TimeFunction Renew(Func<float, float> func, float range)
        {
            return new TimeFunction(_timeProvider, func, range);
        }

        public virtual void Reset()
        {
            float range = _ending - _beginning;
            _beginning = _timeProvider.CurrentSecond;
            _ending = _beginning + range;
        }

        public virtual TimeFunction Reverse()
        {
            float range = _ending - _beginning;
            return new TimeFunction(
                _timeProvider, p => TimeRelatedCalculation(range - p), range);
        }

    }

    public sealed class TimeFunctionOne : TimeFunction
    {

        internal TimeFunctionOne() : base() { }

        public override bool IsComplete => true;

        public override float Calculate()
        {
            return 1;
        }

        public override TimeFunction Renew((Func<float, float> func, float range) funcPair)
        {
            throw new NotSupportedException();
        }

        public override TimeFunction Renew(Func<float, float> func, float range)
        {
            throw new NotSupportedException();
        }

        #pragma warning disable CA1822 
        public override void Reset()
        {
        }
 #pragma warning restore CA1822 
    }
}
