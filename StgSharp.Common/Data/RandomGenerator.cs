//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="RandomGenerator.cs"
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
using StgSharp.HighPerformance;

using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace StgSharp.Data
{
    public unsafe class RandomGenerator
    {

        private byte[] currentKey;
        private readonly byte[] originKey;
        private int cycleCount;

        public RandomGenerator() : this( DateTime.UtcNow.ToBinary() ) { }

        public RandomGenerator( object source )
        {
            ArgumentNullException.ThrowIfNull( source );
            XmlSerializer s = new XmlSerializer( source.GetType() );
            using( StringWriter sw = new StringWriter() )
            {
                s.Serialize( sw, source );
                string data = sw.ToString();
                originKey = Encoding.Unicode.GetBytes( data );
            }
            currentKey = [0,0,0,0];
            fixed( byte* o = originKey, c = currentKey ) {
                *( int* )c = UnsafeCompute.CityHashSimplify( ( char* )o, 0, originKey.Length / 2 );
            }
            cycleCount = 0;
        }

        public int TotalGeneratedRandom
        {
            get => cycleCount;
        }

        public unsafe int GenRandomInt()
        {
            uint source = PrivateRand();
            cycleCount++;
            M64 m = new M64();
            m.Write<uint>( 0, source );
            m.Write<uint>( 1, source );
            return ( m << ( cycleCount % 32 ) ).Read<int>( 1 );
        }

        public byte GenRandomInt8()
        {
            cycleCount++;
            if( cycleCount % 4 == 0 ) {
                PrivateRand();
            }
            return currentKey[ cycleCount % 4 ];
        }

        public float GenRandomSingle()
        {
            cycleCount++;

            while( true )
            {
                uint
                    temp0 = PrivateRand(),
                    temp1 = PrivateRand();

                if( temp0 * temp1 != 0 )
                {
                    float ret = ( ( temp0 % temp1 ) * 1.0f ) / temp1;
                    if( ret < 0.335f )
                    {
                        ret = ret * ( ( ret * ( -0.7811934f ) ) + 1.4489113f );
                    } else
                    {
                        ret = ( ret * ( ( ret * ( -0.1158368f ) ) + 1.0580600f ) ) + 0.0590584f;
                    }
                    if( ret > 1 )
                    {
                        continue;
                    }
                    return ret;
                }
            }
        }

        public void Reset()
        {
            currentKey = originKey;
            cycleCount = 0;
        }

        private unsafe uint PrivateRand()
        {
            uint ret;
            fixed( byte* bptr = currentKey )
            {
                *( int* )bptr = UnsafeCompute.CityHashSimplify(
                    ( char* )bptr, currentKey.Length / 2 );
                ret = *( uint* )bptr;
            }

            //Console.WriteLine(ret.ToString(16));
            return ret;
        }

    }
}
