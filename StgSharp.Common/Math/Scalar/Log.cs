﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Log.cs"
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
using System;

namespace StgSharp.Math
{
    public static partial class Scaler
    {

        private static readonly float[] logArray = {
            0f, 0.00995033085316809f, 0.0198026272961797f, 0.0295588022415444f, 0.0392207131532813f, 0.048790164169432f, 0.0582689081239758f, 0.0676586484738149f, 0.0769610411361284f, 0.0861776962410524f, 0.0953101798043249f, 0.104360015324243f, 0.113328685307003f, 0.122217632724249f, 0.131028262406404f, 0.139761942375159f, 0.148420005118273f, 0.157003748809665f, 0.165514438477573f, 0.173953307123438f, 0.182321556793955f, 0.19062035960865f, 0.198850858745165f, 0.207014169384326f, 0.215111379616945f, 0.22314355131421f, 0.231111720963387f, 0.2390169004705f, 0.246860077931526f, 0.254642218373581f, 0.262364264467491f, 0.27002713721306f, 0.27763173659828f, 0.285178942233662f, 0.29266961396282f, 0.300104592450338f, 0.307484699747961f, 0.314810739840034f, 0.322083499169113f, 0.3293037471426f, 0.336472236621213f, 0.343589704390077f, 0.350656871613169f, 0.357674444271816f, 0.364643113587909f, 0.371563556432483f, 0.378436435720245f, 0.385262400790645f, 0.392042087776024f, 0.398776119957368f, 0.405465108108164f, 0.412109650826833f, 0.418710334858185f, 0.425267735404344f, 0.431782416425538f, 0.438254930931155f, 0.444685821261446f, 0.451075619360217f, 0.457424847038875f, 0.46373401623214f, 0.470003629245736f, 0.476234178996372f, 0.482426149244293f, 0.488580014818671f, 0.494696241836107f, 0.500775287912489f, 0.506817602368452f, 0.512823626428664f, 0.518793793415168f, 0.524728528934982f, 0.53062825106217f, 0.536493370514568f, 0.542324290825362f, 0.548121408509688f, 0.553885113226438f, 0.559615787935423f, 0.56531380905006f, 0.570979546585738f, 0.576613364303994f, 0.582215619852664f, 0.587786664902119f, 0.593326845277734f, 0.598836501088704f, 0.60431596685333f, 0.609765571620894f, 0.615185639090233f, 0.62057648772511f, 0.625938430866495f, 0.631271776841858f, 0.636576829071551f, 0.641853886172395f, 0.647103242058538f, 0.65232518603969f, 0.657520002916794f, 0.662687973075237f, 0.667829372575655f, 0.672944473242426f, 0.678033542749897f, 0.683096844706444f, 0.688134638736401f,
        };

        /// <summary>
        ///
        /// </summary>
        /// <param _label="x">
        ///
        /// </param>
        /// <returns>
        ///   Returns the value of ln(x).
        /// </returns>
        public static unsafe float Log( float x )
        {
            if( x < 0 ) {
                throw new ArgumentOutOfRangeException();
            }
            uint input = *( uint* )&x;

            int f = ( int )input;
            f >>= 23;
            f -= 127;
            input <<= 9;
            input >>= 9;
            input += 0b_0011_1111_1000_0000_0000_0000_0000_0000;
            float result = *( float* )&input;
            uint order = ( ( uint )( result * 100 ) ) - 100;

            float cache = ( result / ( 1f + ( 0.01f * order ) ) ) - 1;
            result = ( cache * ( -0.25f ) ) + 0.3333333333333f;
            result = ( result * cache ) - 0.5f;
            result = ( result * cache ) + 1.0f;
            result *= cache;

            return
                ( f * 0.6931471806f ) + result + logArray[ order ];
        }

        public static float Log( float x, float y )
        {
            return Log( x ) / Log( y );
        }

    }
}
