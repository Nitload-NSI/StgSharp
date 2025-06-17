//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Matrix22.cs"
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

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{
    [StructLayout(
            LayoutKind.Explicit,
            Size = ( 2 * 2 * 4 * sizeof( float ) ) + sizeof( bool ),
            Pack = 16 )]
    public struct Matrix22 : IMatrix<Matrix22>
    {

        [FieldOffset( 2 * 2 * 4 * sizeof( float ) )]
        internal bool isTransposed = false;

        [FieldOffset( 0 )]
        internal ColumnSet2 _mat;
        [FieldOffset( 2 * 4 * sizeof( float ) )]
        internal ColumnSet2 _transpose;

        internal Matrix22( ColumnSet2 mat )
        {
            this._mat = mat;
        }

        internal Matrix22( Vector4 c0, Vector4 c1 )
        {
            Unsafe.SkipInit( out this );
            isTransposed = false;
            _mat.colum0 = c0;
            _mat.colum1 = c1;
        }

        public Matrix22()
        {
            Unsafe.SkipInit( out this );
            isTransposed = false;
        }

        public Matrix22( float a00, float a01, float a10, float a11 )
        {
            Unsafe.SkipInit( out this );
            isTransposed = false;
            _mat.colum0 = new Vector4( a00, a10, 0, 0 );
            _mat.colum1 = new Vector4( a01, a11, 0, 0 );
        }

        public Matrix22 Transpose
        {
            get
            {
                InternalTranspose();
                return new Matrix22( this._transpose );
            }
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public float Det()
        {
            return ( _mat.m00 * _mat.m11 ) - ( _mat.m10 * _mat.m01 );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        internal void InternalTranspose()
        {
            if( !isTransposed )
            {
                _transpose = _mat;
                float cache = _transpose.m10;
                _transpose.m10 = _transpose.m01;
                _transpose.m01 = cache;
                isTransposed = true;
            }
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static unsafe Matrix22 operator -( Matrix22 left, Matrix22 right )
        {
            Matrix22 ret = new Matrix22();
            InternalIO.Intrinsic.sub_mat_2( &left._mat, &right._mat, &ret._mat );
            return ret;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Matrix22 operator *( Matrix22 mat, float value )
        {
            return new Matrix22( mat._mat.colum0 * value, mat._mat.colum1 * value );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Matrix22 operator *( Matrix22 left, Matrix22 right )
        {
            left.InternalTranspose();
            return new Matrix22(
                Vector4.Dot( left._transpose.colum0, right._mat.colum0 ),
                Vector4.Dot( left._transpose.colum0, right._mat.colum1 ),

                Vector4.Dot( left._transpose.colum1, right._mat.colum0 ),
                Vector4.Dot( left._transpose.colum1, right._mat.colum1 ) );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Matrix23 operator *( Matrix22 left, Matrix23 right )
        {
            left.InternalTranspose();
            return new Matrix23(
                Vector4.Dot( left._transpose.colum0, right._mat.colum0 ),
                Vector4.Dot( left._transpose.colum0, right._mat.colum1 ),
                Vector4.Dot( left._transpose.colum0, right._mat.colum2 ),

                Vector4.Dot( left._transpose.colum1, right._mat.colum0 ),
                Vector4.Dot( left._transpose.colum1, right._mat.colum1 ),
                Vector4.Dot( left._transpose.colum1, right._mat.colum2 ) );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Matrix24 operator *( Matrix22 left, Matrix24 right )
        {
            left.InternalTranspose();
            return new Matrix24(
                Vector4.Dot( left._transpose.colum0, right.mat.colum0 ),
                Vector4.Dot( left._transpose.colum0, right.mat.colum1 ),
                Vector4.Dot( left._transpose.colum0, right.mat.colum2 ),
                Vector4.Dot( left._transpose.colum0, right.mat.colum3 ),

                Vector4.Dot( left._transpose.colum1, right.mat.colum0 ),
                Vector4.Dot( left._transpose.colum1, right.mat.colum1 ),
                Vector4.Dot( left._transpose.colum1, right.mat.colum2 ),
                Vector4.Dot( left._transpose.colum1, right.mat.colum3 ) );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Matrix22 operator /( Matrix22 mat, float value )
        {
            return new Matrix22( mat._mat.colum0 / value, mat._mat.colum1 / value );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static unsafe Matrix22 operator +( Matrix22 left, Matrix22 right )
        {
            Matrix22 ret = new Matrix22();
            InternalIO.Intrinsic.add_mat_2( &left._mat, &right._mat, &ret._mat );
            return ret;
        }

    }
}
