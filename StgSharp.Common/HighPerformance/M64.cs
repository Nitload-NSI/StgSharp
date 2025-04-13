//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="M64.cs"
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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance
{
    [StructLayout( LayoutKind.Explicit, Pack = 8 )]
    public unsafe struct M64 : IRegisterType
    {

        [FieldOffset( 0 )] private fixed byte buffer[ 8 ];
        [FieldOffset( 0 )] private ulong value;

        public M64()
        {
            Unsafe.SkipInit( out this );
            value = 0;
        }

        public ref T AsRef<T>() where T: struct,INumber<T>
        {
            return ref Unsafe.As<byte, T>( ref buffer[ 0 ] );
        }

        public override bool Equals( object obj )
        {
            return obj is M64 m64 && this == m64;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public T Read<T>( int index ) where T: struct, INumber<T>
        {
            return Unsafe.As<byte, T>( ref buffer[ index * sizeof( T ) ] );
        }

        public void Write<T>( int index, T value ) where T: struct, INumber<T>
        {
            Unsafe.As<byte, T>( ref buffer[ index * sizeof( T ) ] ) = value;
        }

        public static bool operator !=( M64 left, M64 right )
        {
            return !( left == right );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static M64 operator <<( M64 m, int shift )
        {
            return new M64
            {
                value = m.value << shift
            };
        }

        public static bool operator ==( M64 left, M64 right )
        {
            return left.value == right.value;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static M64 operator >>( M64 m, int shift )
        {
            return new M64
            {
                value = m.value >> shift
            };
        }

    }
}
