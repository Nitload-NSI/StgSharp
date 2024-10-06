//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ColumnSet2.cs"
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
using StgSharp.Data.Intrinsic;
using StgSharp.Math;

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Data.Intrinsic
{
    [StructLayout( LayoutKind.Explicit, Size = 12 * sizeof( float ), Pack = 16 )]
    internal struct ColumnSet2 : IEquatable<ColumnSet2>
    {

        [FieldOffset( 0 * sizeof( float ) )] internal float m00;
        [FieldOffset( 4 * sizeof( float ) )] internal float m01;
        [FieldOffset( 1 * sizeof( float ) )] internal float m10;
        [FieldOffset( 5 * sizeof( float ) )] internal float m11;
        [FieldOffset( 2 * sizeof( float ) )] internal float m20;
        [FieldOffset( 6 * sizeof( float ) )] internal float m21;
        [FieldOffset( 3 * sizeof( float ) )] internal float m30;
        [FieldOffset( 7 * sizeof( float ) )] internal float m31;

        [FieldOffset( 0 )] internal Vector4 colum0;
        [FieldOffset( 4 * sizeof( float ) )] internal Vector4 colum1;

        internal ColumnSet2( Vector4 c0, Vector4 c1 )
        {
            colum0 = c0;
            colum1 = c1;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public override bool Equals( object? obj )
        {
            return ( obj is ColumnSet2 mat ) && Equals( mat );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public bool Equals( ColumnSet2 other )
        {
            return colum0.Equals( other.colum0 ) && colum1.Equals(
                other.colum1 );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool operator !=( ColumnSet2 left, ColumnSet2 right )
        {
            return !( left == right );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool operator ==( ColumnSet2 left, ColumnSet2 right )
        {
            return left.Equals( right );
        }

    }
}
