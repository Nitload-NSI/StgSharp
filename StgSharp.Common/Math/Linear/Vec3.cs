//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Vec3.cs"
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
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{
    [StructLayout( LayoutKind.Explicit, Size = 16, Pack = 16 )]
    public struct Vec3 : IEquatable<Vec3>, IVector
    {

        [FieldOffset( 0 )] internal unsafe fixed float num[ 3 ];
        [FieldOffset( 0 )]
        internal M128 reg;

        [FieldOffset( 0 )]
        internal Vector3 v;

        [FieldOffset( 0 )]
        internal Vector4 vec;

        [FieldOffset( 0 )] public float X;
        [FieldOffset( 4 )] public float Y;
        [FieldOffset( 8 )] public float Z;

        internal Vec3( Vector4 vector )
        {
            vec = vector;
        }

        internal Vec3( Vector3 vector )
        {
            v = vector;
        }

        public Vec3( Vec2 xy, float z )
        {
            vec = xy.vec;
            Z = z;
        }

        public Vec3( float x, float y, float z )
        {
            #if NET5_0_OR_GREATER
            Unsafe.SkipInit( out vec );
            #endif
            v = new Vector3( x, y, z );
        }

        public unsafe Vec2 XY
        {
            get => new Vec2( this.vec );
            set
            {
                fixed( Vector3* vptr = &v ) {
                    *( Vector2* )vptr = value.v;
                }
            }
        }

        public Vec3 XYZ
        {
            get { return new Vec3( v ); }
        }

        public static Vec3 Zero
        {
            get => new Vec3( 0, 0, 0 );
        }

        public static Vec3 One => new Vec3( 1, 1, 1 );

        public static Vec3 UnitX => new Vec3( 1, 0, 0 );

        public static Vec3 UnitY => new Vec3( 0, 1, 0 );

        public static Vec3 UnitZ => new Vec3( 0, 0, 1 );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Vec3 Cross( Vec3 left, Vec3 right )
        {
            return Linear.Cross( left, right );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public float Dot( Vec3 right )
        {
            return Vector3.Dot( v, right.v );
        }

        public bool Equals( Vec3 other )
        {
            return this.vec == other.vec;
        }

        public override bool Equals( object obj )
        {
            if( obj is Vec3 ) {
                return this == ( Vec3 )obj;
            } else {
                return false;
            }
        }

        public override unsafe int GetHashCode()
        {
            fixed( float* add = &X ) {
                return HashCode.Combine( X, Y, Z, ( ulong )add );
            }
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public float GetMaxValue()
        {
            float ret = X;
            ret = ( ret > Y ) ? ret : Y;
            ret = ( ret > Z ) ? ret : Z;
            return ret;
        }

        public static bool IsParallel( Vec3 left, Vec3 right )
        {
            return Cross( left, right ) == Zero;
        }

        public override string ToString()
        {
            return $"[{X},{Y},{Z}]";
        }

        public string ToString( string sample )
        {
            return $"[{X.ToString(sample)},{Y.ToString(sample)},{Z.ToString(sample)}]";
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Vec3 operator -( Vec3 vec )
        {
            return new Vec3( -vec.vec );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Vec3 operator -( Vec3 left, Vec3 right )
        {
            return new Vec3( left.v - right.v );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool operator !=( Vec3 left, Vec3 right )
        {
            return left.v != right.v;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Vec3 operator *( Vec3 vec, float value )
        {
            return new Vec3( vec.v * value );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float operator *( Vec3 left, Vec3 right )
        {
            return Vector3.Dot( left.v, right.v );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Vec3 operator /( Vec3 vec, float value )
        {
            return new Vec3( vec.v / value );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Vec3 operator +( Vec3 left, Vec3 right )
        {
            return new Vec3( left.vec + right.vec );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool operator ==( Vec3 left, Vec3 right )
        {
            return left.v == right.v;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static unsafe implicit operator Vec3(
                                                       (float, float, float) tuple )
        {
            Vec3 v = *( Vec3* )&tuple;
            v.vec.W = 0.0f;
            return v;
        }

    }
}
