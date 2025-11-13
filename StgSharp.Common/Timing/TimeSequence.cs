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
using StgSharp.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Common.Timing
{
    public abstract class TimeSequence
    {

        public ulong BeginningTick { get; protected set; }

        public ulong EndingTick { get; protected set; }

        public ulong FrameLength { get; protected set; }

        public static TimeSequence CreateLinear(ulong timeout, ulong length)
        {
            return new LinearTimeSequence(timeout, timeout + length, length);
        }

        public abstract bool IsNextFrameReady(ulong currentTick);

    }

    internal sealed class LogTimeSequence : TimeSequence
    {

        private float _rate ;

        public LogTimeSequence(ulong beginningTick, ulong endingTick, ulong tickPerDecade)
        {
            BeginningTick = beginningTick;
            EndingTick = endingTick;
            _rate = Scalar.Pow(10, 1 / tickPerDecade);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool IsNextFrameReady(ulong currentTick)
        {
            return (ulong)Math.Log(currentTick - BeginningTick + 1) >= FrameLength;
        }

    }

    internal sealed class LinearTimeSequence : TimeSequence
    {

        public LinearTimeSequence(ulong beginningTick, ulong endingTick, ulong tickPerFrame)
        {
            BeginningTick = beginningTick;
            EndingTick = endingTick;
            FrameLength = tickPerFrame;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool IsNextFrameReady(ulong currentTick)
        {
            return currentTick - BeginningTick >= FrameLength;
        }

    }
}
