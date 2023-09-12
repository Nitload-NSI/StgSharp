using StgSharp.Controlling;
using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Math
{
    public static unsafe partial class Calc
    {

        internal static float currentSeed
#if DEBUG    
            = 0.5f;
#else
        = Calc.Abs(System.DateTime.UtcNow.GetHashCode());
#endif

        /// <summary>
        /// returns a random float number between 0 and 1.
        /// </summary>
        /// <returns></returns>

        public static unsafe float Random()
        { 
            float seed = currentSeed;

            float sample = Calc.Pow(Calc.SeedSqrt(seed),2);
            float sample2 = (sample - seed) / seed;
            sample2 = Calc.Abs(sample2);

            if (sample==1)
            {
                sample = sample2;
            }
            
            currentSeed = sample;
            sample2 *= 1000000;

            float result = sample2-(int)sample2;
            if (result == 0)
            {
                currentSeed = sample2+0.1f;
                result = Calc.Random();
            }
            return result;

        }

        /// <summary>
        /// Generates a random seed in int value.
        /// </summary>
        /// <returns></returns>
        public static unsafe int RandomSeed()
        {
            float seedValue1 = Random();
            float seedValue2 = Random();
            uint seedValueBits = *(uint*)&seedValue1;

            float seedValue = seedValueBits * seedValue2;

            return *(int*)&seedValue;
        }
    }
}
