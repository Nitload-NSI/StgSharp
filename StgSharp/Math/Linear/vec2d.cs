//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="vec2d.cs"
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
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{
    /// <summary>
    /// A two dimension vector defined by two elements.
    /// Vec2Ds in StgSharp are default used as colum vector. 
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 16, Pack = 16)]
    public struct vec2d:IVector
    {

        [FieldOffset(0)] internal unsafe fixed float num[2];

        [FieldOffset(0)]
        internal Vector2 v;

        [FieldOffset(0)]
        internal Vector4 vec;

        [FieldOffset(0)]
        public float X;
        [FieldOffset(4)]
        public float Y;

        internal vec2d(Vector2 vec)
        {
            v = vec;
        }
        internal vec2d(Vector4 vec)
        {
            this.vec = vec;
        }

        public vec2d(float x, float y)
        {
            v = new Vector2(x, y);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Cross(vec2d right)
        {
            return (X * right.Y) - (Y * right.X);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Dot(vec2d vec)
        {
            return Vector2.Dot(v, vec.v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static vec2d operator -(vec2d left, vec2d right)
        {
            return new vec2d(left.v - right.v);
        }

        public static bool operator !=(vec2d left, vec2d right)
        {
            return !(left == right);
        }

        public static vec2d operator *(vec2d vec, float value)
        {
            return new vec2d(vec.v * value);
        }

        public static vec2d operator /(vec2d vec, float value)
        {
            return new vec2d(vec.v / value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static vec2d operator +(vec2d left, vec2d right)
        {
            return new vec2d(left.v + right.v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(vec2d left, vec2d right)
        {
            return left.v == right.v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator vec2d((float, float) tuple)
        {
            return new vec2d(tuple.Item1, tuple.Item2);
        }

    }

    public static class Vec2d
    {

        public static vec2d Unit => new vec2d(1, 1);

        public static vec2d Zero => new vec2d(0, 0);

    }
}
