//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Sin.cs"
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
using System.Net.NetworkInformation;

namespace StgSharp.Math
{
    public static partial class Scaler
    {

        public const float Pi = 3.1415926f;

        private static readonly float[] cosData =
            {
            1f, 0.987688341014302f, 0.951056517951169f, 0.891006527837756f, 0.809017000674805f,
            0.707106790659974f, 0.587785265298989f, 0.453990516451646f, 0.309017014761716f, 0.156434488858737f,
            2.67948965850537E-08f,  -0.156434435928723f,    -0.309016963794794f,    -0.453990468702791f,    -0.587785221943935f,
            -0.707106752766267f,    -0.809016969175515f,    -0.891006503508499f,    -0.951056501391011f,    -0.98768833263101f,
            -0.999999999999999f,    -0.987688349397591f,    -0.951056534511324f,    -0.891006552167012f,    -0.809017032174094f,
            -0.707106828553679f,    -0.587785308654041f,    -0.4539905642005f,  -0.309017065728638f,    -0.156434541788751f,
            -8.03846893110719E-08f, 0.156434382998709f, 0.309016912827872f, 0.453990420953934f, 0.58778517858888f,
            0.707106714872559f, 0.809016937676222f, 0.891006479179238f, 0.951056484830851f, 0.987688324247716f,


        };

        private static readonly float[] sinData =
            {
            0f, 0.15643446239373f,  0.309016989278255f, 0.453990492577218f, 0.587785243621462f,
            0.707106771713121f, 0.80901698492516f,  0.891006515673128f, 0.951056509671091f, 0.987688336822657f,
            1f, 0.987688345205947f, 0.951056526231247f, 0.891006540002384f, 0.80901701642445f,
            0.707106809606826f, 0.587785286976515f, 0.453990540326073f, 0.309017040245177f, 0.156434515323744f,
            5.35897931701074E-08f,  -0.156434409463716f,    -0.309016938311333f,    -0.453990444828362f,    -0.587785200266408f,
            -0.707106733819413f,    -0.809016953425868f,    -0.891006491343869f,    -0.951056493110931f,    -0.987688328439363f,
            -0.999999999999997f,    -0.987688353589235f,    -0.951056542791401f,    -0.891006564331638f,    -0.809017047923737f,
            -0.70710684750053f, -0.587785330331566f,    -0.453990588074927f,    -0.309017091212098f,    -0.156434568253757f,


        };

        public static unsafe float Sin(float x)
        {
            float refer = x / 360.0f;
            refer -= (int)refer;

            refer *= 40;
            int a = (int)refer;   //大分度，查表
            refer -= a;    //小分度，拟合

            float refer2 = refer * refer;


            return (sinData[a] * ((1 - (0.0123370055f * refer2)) + (2.536695709e-5f * refer2 * refer2)))
                + (cosData[a] * ((0.1570796327f * refer) - (6.459640975e-4f * refer * refer2)));
        }

    }
}
