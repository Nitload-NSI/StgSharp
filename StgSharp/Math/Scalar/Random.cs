//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Random.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
using StgSharp.Controlling;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.Math
{
    public static unsafe partial class Scaler
    {

        internal static float currentSeed
#if DEBUG    
            = 0.5f;
#else
        = Abs(System.DateTime.UtcNow.GetHashCode());
#endif


        /// <summary>
        /// returns a random float number between 0 and 1.
        /// </summary>
        /// <returns></returns>
        public static unsafe float LowProceisionRandom()
        {
            float seed = currentSeed;

            float sample = Scaler.Pow(Scaler.SeedSqrt(seed), 2);
            float sample2 = (sample - seed) / seed;
            sample2 = Scaler.Abs(sample2);

            if (sample == 1)
            {
                sample = sample2;
            }

            currentSeed = sample;
            sample2 *= 1000000;

            float result = sample2 - ((int)sample2);
            if (result == 0)
            {
                currentSeed = sample2 + 0.1f;
                result = Scaler.LowProceisionRandom();
            }
            return result;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Random()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates a random seed in int value.
        /// </summary>
        /// <returns></returns>
        public static unsafe int RandomSeed()
        {
            float seedValue1 = LowProceisionRandom();
            float seedValue2 = LowProceisionRandom();
            uint seedValueBits = *(uint*)&seedValue1;

            float seedValue = seedValueBits * seedValue2;

            return *(int*)&seedValue;
        }

    }
}
