//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Matrix33.cs"
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
            Size = ( 6 * 4 * sizeof( float ) ) + sizeof( bool ),
            Pack = 16 )]
    public struct Matrix33 : IMat
    {

        [FieldOffset( 6 * 4 * sizeof( float ) )] internal bool isTransposed;

        [FieldOffset( 0 )] internal ColumnSet3 mat;
        [FieldOffset( 3 * 4 * sizeof( float ) )] internal ColumnSet3 transpose;
        [FieldOffset( 0 * 4 * sizeof( float ) )] public Vec3 Colum0;
        [FieldOffset( 1 * 4 * sizeof( float ) )] public Vec3 Colum1;
        [FieldOffset( 2 * 4 * sizeof( float ) )] public Vec3 Colum2;

        internal Matrix33( Vector4 c0, Vector4 c1, Vector4 c2 )
        {
            Unsafe.SkipInit( out this );
            isTransposed = false;
            mat.colum0 = c0;
            mat.colum1 = c1;
            mat.colum2 = c2;
        }

        public Matrix33()
        {
            Unsafe.SkipInit( out this );
            isTransposed = false;
        }

        public Matrix33(
                       float a00,
                       float a01,
                       float a02,
                       float a10,
                       float a11,
                       float a12,
                       float a20,
                       float a21,
                       float a22 )
        {
            Unsafe.SkipInit( out this );
            isTransposed = false;
            mat.colum0 = new Vector4( a00, a10, a20, 0 );
            mat.colum1 = new Vector4( a01, a11, a21, 0 );
            mat.colum2 = new Vector4( a02, a12, a22, 0 );
        }

        public unsafe float this[ int rowNum, int columNum ]
        {
            get
            {
                if( ( rowNum > 2 ) || ( rowNum < 0 ) ) {
                    throw new ArgumentOutOfRangeException( nameof( columNum ) );
                }
                if( ( columNum > 2 ) || ( columNum < 0 ) ) {
                    throw new ArgumentOutOfRangeException( nameof( columNum ) );
                }
                InternalTranspose();
                fixed( float* p = &this.transpose.m00 ) {
                    ulong pbit = ( ( ulong )p ) + ( ( ( ulong )sizeof( Vector4 ) ) * ( ( ulong )rowNum ) ) + ( ( ( ulong )sizeof( float ) ) * ( ( ulong )columNum ) );
                    return *( float* )pbit;
                }
            }
            set
            {
                if( ( rowNum > 2 ) || ( rowNum < 0 ) ) {
                    throw new ArgumentOutOfRangeException( nameof( columNum ) );
                }
                if( ( columNum > 2 ) || ( columNum < 0 ) ) {
                    InternalIO.InternalWriteLog(
                        $"Attempt to write unused space in {GetType().Name} {nameof(mat)}",
                        LogType.Error );
                }
                InternalTranspose();
                fixed( float* p = &this.transpose.m00 ) {
                    ulong pbit = ( ( ulong )p ) + ( ( ( ulong )sizeof( Vector4 ) ) * ( ( ulong )rowNum ) ) + ( ( ( ulong )sizeof( float ) ) * ( ( ulong )columNum ) );
                    *( float* )pbit = value;
                }
                isTransposed = false;
            }
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public unsafe float Det()
        {
            fixed( ColumnSet3* mat = &this.mat ) {
                return InternalIO.Intrinsic.det_mat_3( mat, ( ColumnSet3* )0 );
            }
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        internal unsafe void InternalTranspose()
        {
            if( !isTransposed ) {
                fixed( ColumnSet3* source = &this.mat, target = &this.transpose ) {
                    InternalIO.Intrinsic.transpose33( source, target );
                }
                isTransposed = true;
            }
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static unsafe Matrix33 operator -(
                                                       Matrix33 left,
                                                       Matrix33 right )
        {
            Matrix33 ret = new Matrix33();
            InternalIO.Intrinsic.sub_mat_3( &left.mat, &right.mat, &ret.mat );
            return ret;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Matrix33 operator *( Matrix33 mat, float value )
        {
            return new Matrix33(
                mat.mat.colum0 * value, mat.mat.colum1 * value,
                mat.mat.colum2 * value );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Vec3 operator *( Matrix33 left, Vec3 right )
        {
            left.InternalTranspose();
            return new Vec3(
                Vector4.Dot( left.transpose.colum0, right.vec ),
                Vector4.Dot( left.transpose.colum1, right.vec ),
                Vector4.Dot( left.transpose.colum2, right.vec ) );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Matrix32 operator *( Matrix33 left, Matrix32 right )
        {
            left.InternalTranspose();
            return new Matrix32(
                Vector4.Dot( left.transpose.colum0, right.mat.colum0 ),
                Vector4.Dot( left.transpose.colum0, right.mat.colum1 ),

                Vector4.Dot( left.transpose.colum1, right.mat.colum0 ),
                Vector4.Dot( left.transpose.colum1, right.mat.colum1 ),

                Vector4.Dot( left.transpose.colum2, right.mat.colum0 ),
                Vector4.Dot( left.transpose.colum2, right.mat.colum1 ) );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Matrix33 operator *( Matrix33 left, Matrix33 right )
        {
            left.InternalTranspose();
            return new Matrix33(
                Vector4.Dot( left.transpose.colum0, right.mat.colum0 ),
                Vector4.Dot( left.transpose.colum0, right.mat.colum1 ),
                Vector4.Dot( left.transpose.colum0, right.mat.colum2 ),

                Vector4.Dot( left.transpose.colum1, right.mat.colum0 ),
                Vector4.Dot( left.transpose.colum1, right.mat.colum1 ),
                Vector4.Dot( left.transpose.colum1, right.mat.colum2 ),

                Vector4.Dot( left.transpose.colum2, right.mat.colum0 ),
                Vector4.Dot( left.transpose.colum2, right.mat.colum1 ),
                Vector4.Dot( left.transpose.colum2, right.mat.colum2 ) );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Matrix34 operator *( Matrix33 left, Matrix34 right )
        {
            left.InternalTranspose();
            return new Matrix34(
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
                Vector4.Dot( left.transpose.colum2, right.mat.colum3 ) );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Matrix33 operator /( Matrix33 mat, float value )
        {
            return new Matrix33(
                mat.mat.colum0 / value, mat.mat.colum1 / value,
                mat.mat.colum2 / value );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static unsafe Matrix33 operator +(
                                                       Matrix33 left,
                                                       Matrix33 right )
        {
            Matrix33 ret = new Matrix33();
            InternalIO.Intrinsic.add_mat_3( &left.mat, &right.mat, &ret.mat );
            return ret;
        }

    }
}
