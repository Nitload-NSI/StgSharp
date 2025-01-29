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
using StgSharp.Internal.Intrinsic;

using System;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;

namespace StgSharp.Internal
{
    public class RandomGenerator
    {

        private byte[] currentKey;

        private readonly byte[] originKey;
        private int cycleCount;
        private SHA512 hash;

        public RandomGenerator() : this( DateTime.UtcNow.ToBinary() ) { }

        public RandomGenerator( object source )
        {
            try {
                if( source == null ) {
                    throw new ArgumentNullException( nameof( source ) );
                }
                XmlSerializer s = new XmlSerializer( source.GetType() );
                using( StringWriter sw = new StringWriter() ) {
                    s.Serialize( sw, source );
                    string data = sw.ToString();
                    originKey = Encoding.UTF8.GetBytes( data );
                }
            }
            catch( Exception ) {
                throw;
            }
            hash = SHA512.Create();
            originKey = hash.ComputeHash( originKey );
            currentKey = originKey;
            cycleCount = 0;
        }

        public int TotalGeneratedRandom
        {
            get => cycleCount;
        }

        public unsafe int GenRandomInt()
        {
            M128 source = PrivateRand();
            cycleCount++;

            /*
            float d0 =  MathF.Sin(1 / source.Read<float>(0));
            float d1 =  MathF.Sin(1 / source.Read<float>(1));
            float d2 =  MathF.Sin(1 / source.Read<float>(2));
            float d3 =  MathF.Sin(1 / source.Read<float>(3));

            source.Write<short>(0, *( short*)&d3);
            source.Write<short>(2, *((short*)&d2+1));
            source.Write<short>(4, *( short*)&d1);
            source.Write<short>(6, *((short*)&d0+1));

            /**/

            return source.Read<int>( cycleCount % 4 );
        }

        public byte GenRandomInt8()
        {
            cycleCount++;
            if( cycleCount % 64 == 0 ) {
                PrivateRand();
            }
            return currentKey[ cycleCount % 64 ];
        }

        public float GenRandomSingle()
        {
            cycleCount++;

            while( true ) {
                M128 source = PrivateRand();

                uint
                    temp0 = source.Read<uint>( 0 ),
                    temp1 = source.Read<uint>( 2 );

                if( temp0 * temp1 != 0 ) {
                    float ret = ( ( temp0 % temp1 ) * 1.0f ) / temp1;
                    if( ret < 0.335f ) {
                        ret = ret * ( ( ret * ( -0.7811934f ) ) + 1.4489113f );
                    } else {
                        ret = ( ret * ( ( ret * ( -0.1158368f ) ) + 1.0580600f ) ) + 0.0590584f;
                    }
                    if( ret > 1 ) {
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

        private unsafe M128 PrivateRand()
        {
            currentKey = hash.ComputeHash( currentKey );
            M128 ret;
            fixed( byte* bptr = currentKey ) {
                switch( ( cycleCount % 16 ) / 4 ) {
                    case 0:
                        ret = *( ( ( M128* )bptr ) + 0 );
                        break;
                    case 1:
                        ret = *( ( ( M128* )bptr ) + 1 );
                        break;
                    case 2:
                        ret = *( ( ( M128* )bptr ) + 2 );
                        break;
                    case 3:
                        ret = *( ( ( M128* )bptr ) + 3 );
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }

            //Console.WriteLine(ret.ToString(16));
            return ret;
        }

    }
}
