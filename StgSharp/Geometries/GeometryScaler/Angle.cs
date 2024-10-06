//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Angle.cs"
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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Math
{
    public static partial class GeometryScaler
    {

        internal const float DegToRad = 180 / Scaler.Pi;

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float Cos( Radius r )
        {
            return MathF.Cos( r._radius );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float Cos( Degree r )
        {
            return MathF.Cos( r._degree / DegToRad );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float Sin( Radius r )
        {
            return MathF.Sin( r._radius );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float Sin( Degree r )
        {
            return MathF.Sin( r._degree / DegToRad );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float Tan( Radius r )
        {
            return MathF.Tan( r._radius );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float Tan( Degree r )
        {
            return MathF.Tan( r._degree / DegToRad );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Degree ToDegree( float value )
        {
            return new Degree( value );
        }

    }

    public struct Degree
    {

        internal float _degree;

        internal Degree( float dgree )
        {
            _degree = dgree;
        }

        public static implicit operator Radius( Degree d )
        {
            return new Radius( d._degree / GeometryScaler.DegToRad );
        }

    }

    public struct Radius
    {

        internal float _radius;

        [Obsolete( "Unsafe, you may forget this is radius.", true )]
        public Radius() { }

        public Radius( float radius )
        {
            _radius = radius;
        }

        public static Radius Zero
        {
            get => new Radius( 0 );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Radius operator -( Radius r )
        {
            return new Radius( -r._radius );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Radius operator -( Radius left, Radius Right )
        {
            return new Radius( left._radius - Right._radius );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool operator !=( Radius left, Radius right )
        {
            return left._radius != right._radius;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Radius operator *( Radius left, float right )
        {
            return new Radius( left._radius * right );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Radius operator /( Radius left, float right )
        {
            return new Radius( left._radius / right );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float operator /( Radius left, Radius right )
        {
            return left._radius / right._radius;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Radius operator +( Radius left, Radius Right )
        {
            return new Radius( left._radius + Right._radius );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool operator ==( Radius left, Radius right )
        {
            return left._radius == right._radius;
        }

        public static implicit operator Degree( Radius r )
        {
            return new Degree( ( r._radius * 180 ) / Scaler.Pi );
        }

    }
}
