﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Matrix23.cs"
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
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{
    [StructLayout(
            LayoutKind.Explicit,
            Size = ( ( 3 + 2 ) * 4 * sizeof( float ) ) + sizeof( bool ),
            Pack = 16 )]
    public struct Matrix23 : IMatrix<Matrix23>
    {

        [FieldOffset( 5 * 4 * sizeof( float ) )] internal bool isTransposed = false;
        [FieldOffset( 3 * 4 * sizeof( float ) )] internal ColumnSet2 _transpose;
        [FieldOffset( 0 )] internal ColumnSet3 _mat;

        internal Matrix23( Vector4 c0, Vector4 c1, Vector4 c2 )
        {
            _mat.colum0 = c0;
            _mat.colum1 = c1;
            _mat.colum2 = c2;
        }

        public Matrix23()
        {
            Unsafe.SkipInit( out _mat );
            Unsafe.SkipInit( out _transpose );
        }

        public Matrix23( float a00, float a01, float a02, float a10, float a11, float a12 )
        {
            _mat.colum0 = new Vector4( a00, a10, 0, 0 );
            _mat.colum1 = new Vector4( a01, a11, 0, 0 );
            _mat.colum2 = new Vector4( a02, a12, 0, 0 );
        }

        public unsafe float this[ int rowNum, int columNum ]
        {
            get
            {
                if( ( rowNum > 1 ) || ( rowNum < 0 ) ) {
                    throw new ArgumentOutOfRangeException( nameof( columNum ) );
                }
                if( ( columNum > 2 ) || ( columNum < 0 ) ) {
                    throw new ArgumentOutOfRangeException( nameof( columNum ) );
                }
                InternalTranspose();
                fixed( float* p = &this._transpose.m00 )
                {
                    ulong pbit = ( ( ulong )p ) + ( ( ( ulong )sizeof( Vector4 ) ) * ( ( ulong )rowNum ) ) + ( ( ( ulong )sizeof( float ) ) * ( ( ulong )columNum ) );
                    return *( float* )pbit;
                }
            }
            set
            {
                if( ( rowNum > 1 ) || ( rowNum < 0 ) ) {
                    throw new ArgumentOutOfRangeException( nameof( columNum ) );
                }
                if( ( columNum > 2 ) || ( columNum < 0 ) ) {
                    throw new ArgumentOutOfRangeException( nameof( columNum ) );
                }
                InternalTranspose();
                fixed( float* p = &this._transpose.m00 )
                {
                    ulong pbit = ( ( ulong )p ) + ( ( ( ulong )sizeof( Vector4 ) ) * ( ( ulong )rowNum ) ) + ( ( ( ulong )sizeof( float ) ) * ( ( ulong )columNum ) );
                    *( float* )pbit = value;
                }
                isTransposed = false;
            }
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        internal unsafe void InternalTranspose()
        {
            if( !isTransposed )
            {
                fixed( ColumnSet3* source = &this._mat )
                {
                    fixed( ColumnSet2* target = &this._transpose ) {
                        InternalIO.Intrinsic.transpose32( source, target );
                    }
                }

                isTransposed = true;
            }
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static unsafe Matrix23 operator -( Matrix23 left, Matrix23 right )
        {
            Matrix23 ret = new Matrix23();
            InternalIO.Intrinsic.sub_mat_3( &left._mat, &right._mat, &ret._mat );
            return ret;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Matrix23 operator *( Matrix23 mat, float value )
        {
            return new Matrix23(
                mat._mat.colum0 * value, mat._mat.colum1 * value, mat._mat.colum2 * value );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Matrix22 operator *( Matrix23 left, Matrix32 right )
        {
            left.InternalTranspose();
            return new Matrix22(
                Vector4.Dot( left._transpose.colum0, right.mat.colum0 ),
                Vector4.Dot( left._transpose.colum0, right.mat.colum1 ),

                Vector4.Dot( left._transpose.colum0, right.mat.colum0 ),
                Vector4.Dot( left._transpose.colum0, right.mat.colum1 ) );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Matrix23 operator *( Matrix23 left, Matrix33 right )
        {
            left.InternalTranspose();
            return new Matrix23(
                Vector4.Dot( left._transpose.colum0, right.mat.colum0 ),
                Vector4.Dot( left._transpose.colum0, right.mat.colum1 ),
                Vector4.Dot( left._transpose.colum0, right.mat.colum2 ),

                Vector4.Dot( left._transpose.colum0, right.mat.colum0 ),
                Vector4.Dot( left._transpose.colum0, right.mat.colum1 ),
                Vector4.Dot( left._transpose.colum0, right.mat.colum2 ) );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Matrix24 operator *( Matrix23 left, Matrix34 right )
        {
            left.InternalTranspose();
            return new Matrix24(
                Vector4.Dot( left._transpose.colum0, right.mat.colum0 ),
                Vector4.Dot( left._transpose.colum0, right.mat.colum1 ),
                Vector4.Dot( left._transpose.colum0, right.mat.colum2 ),
                Vector4.Dot( left._transpose.colum0, right.mat.colum3 ),

                Vector4.Dot( left._transpose.colum0, right.mat.colum0 ),
                Vector4.Dot( left._transpose.colum0, right.mat.colum1 ),
                Vector4.Dot( left._transpose.colum0, right.mat.colum2 ),
                Vector4.Dot( left._transpose.colum0, right.mat.colum3 ) );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Matrix23 operator /( Matrix23 mat, float value )
        {
            return new Matrix23(
                mat._mat.colum0 / value, mat._mat.colum1 / value, mat._mat.colum2 / value );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static unsafe Matrix23 operator +( Matrix23 left, Matrix23 right )
        {
            Matrix23 ret = new Matrix23();
            InternalIO.Intrinsic.add_mat_3( &left._mat, &right._mat, &ret._mat );
            return ret;
        }

    }
}
