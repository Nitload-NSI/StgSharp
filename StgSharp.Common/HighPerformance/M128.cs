//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="M128.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the ��Software��), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED ��AS IS��, 
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
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#if NET5_0_OR_GREATER
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
#endif

using System.Text;

namespace StgSharp.HighPerformance
{
    [StructLayout( LayoutKind.Explicit, Pack = 16 )]
    public unsafe struct M128 : IRegisterType
    {

        [FieldOffset( 0 )] internal fixed byte Buffer[ 16 ];
        [FieldOffset( 0 )] internal Vector128<byte> Vec;

        internal M128( Vector4 v )
        {
            Vec = v.AsVector128().AsByte();
        }

        public ref T AsRef<T>() where T: struct, INumber<T>
        {
            return ref Unsafe.As<byte, T>( ref Buffer[ 0 ] );
        }

        public override bool Equals( [NotNullWhen( true )] object obj )
        {
            if( obj is not M128 other ) {
                return false;
            }
            return this == other;
        }

        public override int GetHashCode()
        {
            return Vec.GetHashCode();
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public T Read<T>( int index ) where T: struct, INumber<T>

        {
            fixed( byte* p = Buffer ) {
                #pragma warning disable CS8500
                return *( ( T* )p + index );
                #pragma warning restore CS8500
            }
        }

        public override string ToString()
        {
            return string.Empty;
        }

        public string ToString( int sample )
        {
            string str = string.Empty;
            for( int i = 15; i >= 0; i-- ) {
                str += $"{Convert.ToString(Buffer[i], sample)} ";
            }
            return str;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public void Write<T>( int index, T value ) where T: struct, INumber<T>
        {
            fixed( byte* p = Buffer ) {
                #pragma warning disable CS8500
                *( ( T* )p + index ) = value;
                #pragma warning restore CS8500
            }
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool operator !=( M128 left, M128 right )
        {
            return left.Vec != right.Vec;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool operator ==( M128 left, M128 right )
        {
            return left.Vec == right.Vec;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static explicit operator Vector128<byte>( M128 m )
        {
            return m.Vec.As<byte, byte>();
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static explicit operator Vector128<double>( M128 m )
        {
            return m.Vec.As<byte, double>();
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static explicit operator Vector128<float>( M128 m )
        {
            return m.Vec.As<byte, float>();
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static explicit operator Vector128<int>( M128 m )
        {
            return m.Vec.As<byte, int>();
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static explicit operator Vector128<long>( M128 m )
        {
            return m.Vec.As<byte, long>();
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static explicit operator Vector128<sbyte>( M128 m )
        {
            return m.Vec.As<byte, sbyte>();
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static explicit operator Vector128<short>( M128 m )
        {
            return m.Vec.As<byte, short>();
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static explicit operator Vector128<uint>( M128 m )
        {
            return m.Vec.As<byte, uint>();
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static explicit operator Vector128<ulong>( M128 m )
        {
            return m.Vec.As<byte, ulong>();
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static explicit operator Vector128<ushort>( M128 m )
        {
            return m.Vec.As<byte, ushort>();
        }

    }
}
