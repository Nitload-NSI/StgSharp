﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Matrix43.cs"
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
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace StgSharp.Math
{
    [StructLayout(
            layoutKind: LayoutKind.Explicit,
            Size = ( 7 * 4 * sizeof( float ) ) + sizeof( bool ),
            Pack = 16 )]
    public struct Matrix43 : IEquatable<Matrix43>, IMatrix<Matrix43>
    {

        [FieldOffset( 7 * 4 * sizeof( float ) )]
        internal bool isTransposed;

        [FieldOffset( 0 )]
        internal ColumnSet3 mat;
        [FieldOffset( 3 * 4 * sizeof( float ) )]
        internal ColumnSet4 transpose;

        internal Matrix43( Vector4 c0, Vector4 c1, Vector4 c2 )
        {
            Unsafe.SkipInit( out this );
            isTransposed = false;
            mat.colum0 = c0;
            mat.colum1 = c1;
            mat.colum2 = c2;
        }

        public Matrix43()
        {
            Unsafe.SkipInit( out this );
            isTransposed = false;
        }

        public Matrix43(
               float a00,
               float a01,
               float a02,
               float a10,
               float a11,
               float a12,
               float a20,
               float a21,
               float a22,
               float a30,
               float a31,
               float a32 )
        {
            Unsafe.SkipInit( out this );
            isTransposed = false;
            mat.colum0 = new Vector4( a00, a10, a20, a30 );
            mat.colum1 = new Vector4( a01, a11, a21, a31 );
            mat.colum2 = new Vector4( a02, a12, a22, a32 );
        }

        public unsafe float this[ int rowNum, int columNum ]
        {
            get
            {
                if( ( rowNum > 3 ) || ( rowNum < 0 ) ) {
                    throw new ArgumentOutOfRangeException( nameof( columNum ) );
                }
                if( ( columNum > 2 ) || ( columNum < 0 ) ) {
                    throw new ArgumentOutOfRangeException( nameof( columNum ) );
                }
                InternalTranspose();
                fixed( float* p = &this.transpose.m00 )
                {
                    ulong pbit = ( ( ulong )p ) + ( ( ( ulong )sizeof( Vector4 ) ) * ( ( ulong )rowNum ) ) + ( ( ( ulong )sizeof( float ) ) * ( ( ulong )columNum ) );
                    return *( float* )pbit;
                }
            }
            set
            {
                if( ( rowNum > 3 ) || ( rowNum < 0 ) ) {
                    throw new ArgumentOutOfRangeException( nameof( columNum ) );
                }
                if( ( columNum > 2 ) || ( columNum < 0 ) ) {
                    throw new ArgumentOutOfRangeException( nameof( columNum ) );
                }
                InternalTranspose();
                fixed( float* p = &this.transpose.m00 )
                {
                    ulong pbit = ( ( ulong )p ) + ( ( ( ulong )sizeof( Vector4 ) ) * ( ( ulong )rowNum ) ) + ( ( ( ulong )sizeof( float ) ) * ( ( ulong )columNum ) );
                    *( float* )pbit = value;
                }
                isTransposed = false;
            }
        }

        public override bool Equals( object? obj )
        {
            return ( obj is Matrix43 x ) && Equals( x );
        }

        public bool Equals( Matrix43 other )
        {
            return EqualityComparer<ColumnSet3>.Default.Equals( mat, other.mat );
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                mat.colum0.GetHashCode(), mat.colum1.GetHashCode(), mat.colum2.GetHashCode() );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        internal unsafe void InternalTranspose()
        {
            if( !isTransposed )
            {
                fixed( ColumnSet3* source = &this.mat )
                {
                    fixed( ColumnSet4* target = &this.transpose ) {
                        InternalIO.Intrinsic.transpose34( source, target );
                    }
                }

                isTransposed = true;
            }
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Matrix43 operator -( Matrix43 left, Matrix43 right )
        {
            return new Matrix43(
                left.mat.colum0 - right.mat.colum0, left.mat.colum1 - right.mat.colum1,
                left.mat.colum2 - right.mat.colum2 );
        }

        public static bool operator !=( Matrix43 left, Matrix43 right )
        {
            return !( left == right );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static unsafe Matrix43 operator *( Matrix43 mat, float value )
        {
            return new Matrix43(
                mat.mat.colum0 * value, mat.mat.colum1 * value, mat.mat.colum2 * value );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Matrix42 operator *( Matrix43 left, Matrix32 right )
        {
            left.InternalTranspose();
            return new Matrix42(
                Vector4.Dot( left.transpose.colum0, right.mat.colum0 ),
                Vector4.Dot( left.transpose.colum0, right.mat.colum1 ),

                Vector4.Dot( left.transpose.colum1, right.mat.colum0 ),
                Vector4.Dot( left.transpose.colum1, right.mat.colum1 ),

                Vector4.Dot( left.transpose.colum2, right.mat.colum0 ),
                Vector4.Dot( left.transpose.colum2, right.mat.colum1 ),

                Vector4.Dot( left.transpose.colum3, right.mat.colum0 ),
                Vector4.Dot( left.transpose.colum3, right.mat.colum1 ) );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Matrix43 operator *( Matrix43 left, Matrix33 right )
        {
            left.InternalTranspose();
            return new Matrix43(
                Vector4.Dot( left.transpose.colum0, right.mat.colum0 ),
                Vector4.Dot( left.transpose.colum0, right.mat.colum1 ),
                Vector4.Dot( left.transpose.colum0, right.mat.colum2 ),

                Vector4.Dot( left.transpose.colum1, right.mat.colum0 ),
                Vector4.Dot( left.transpose.colum1, right.mat.colum1 ),
                Vector4.Dot( left.transpose.colum1, right.mat.colum2 ),

                Vector4.Dot( left.transpose.colum2, right.mat.colum0 ),
                Vector4.Dot( left.transpose.colum2, right.mat.colum1 ),
                Vector4.Dot( left.transpose.colum2, right.mat.colum2 ),

                Vector4.Dot( left.transpose.colum3, right.mat.colum0 ),
                Vector4.Dot( left.transpose.colum3, right.mat.colum1 ),
                Vector4.Dot( left.transpose.colum3, right.mat.colum2 ) );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Matrix44 operator *( Matrix43 left, Matrix34 right )
        {
            left.InternalTranspose();
            return new Matrix44(
                Vector4.Dot( left.transpose.colum0, right.mat.colum0 ),
                Vector4.Dot( left.transpose.colum0, right.mat.colum1 ),
                Vector4.Dot( left.transpose.colum0, right.mat.colum2 ),
                Vector4.Dot( left.transpose.colum0, right.mat.colum3 ),

                Vector4.Dot( left.transpose.colum1, right.mat.colum0 ),
                Vector4.Dot( left.transpose.colum1, right.mat.colum1 ),
                Vector4.Dot( left.transpose.colum1, right.mat.colum2 ),
                Vector4.Dot( left.transpose.colum1, right.mat.colum3 ),

                Vector4.Dot( left.transpose.colum2, right.mat.colum0 ),
                Vector4.Dot( left.transpose.colum2, right.mat.colum1 ),
                Vector4.Dot( left.transpose.colum2, right.mat.colum2 ),
                Vector4.Dot( left.transpose.colum2, right.mat.colum3 ),

                Vector4.Dot( left.transpose.colum3, right.mat.colum0 ),
                Vector4.Dot( left.transpose.colum3, right.mat.colum1 ),
                Vector4.Dot( left.transpose.colum3, right.mat.colum2 ),
                Vector4.Dot( left.transpose.colum3, right.mat.colum3 ) );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Matrix43 operator /( Matrix43 mat, float value )
        {
            return new Matrix43(
                mat.mat.colum0 / value, mat.mat.colum1 / value, mat.mat.colum2 / value );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static unsafe Matrix43 operator +( Matrix43 left, Matrix43 right )
        {
            Matrix43 ret = new Matrix43();
            InternalIO.Intrinsic.add_mat_3( &left.mat, &right.mat, &ret.mat );
            return ret;
        }

        public static bool operator ==( Matrix43 left, Matrix43 right )
        {
            return left.Equals( right );
        }

    }
}