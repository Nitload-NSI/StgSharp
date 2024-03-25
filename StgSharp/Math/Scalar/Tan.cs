//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Tan.cs"
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
namespace StgSharp.Math
{
    public static partial class Scaler
    {

        public static float Tan(float x)
        {
            float refer = x / 360.0f;
            refer -= (int)refer;

            refer *= 40;
            int a = (int)refer;   //大分度，查表
            refer -= a;    //小分度，拟合

            float refer2 = refer * refer;

            float sin = (0.1570796327f * refer) - (6.459640975e-4f * refer * refer2);
            float cos = (1 - (0.0123370055f * refer2)) + (2.536695709e-5f * refer2 * refer2);

            return ((sinData[a] * cos) + (cosData[a] * sin))
                / ((cosData[a] * cos) - (sinData[a] * sin));
        }

    }
}
