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
    /// Vec2Ds in StgSharp are defaultly usaed as colum vector. 
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 16, Pack = 16)]
    public struct Vec2d
    {

        [FieldOffset(0)]
        public float X;
        [FieldOffset(4)]
        public float Y;

        [FieldOffset(0)]
        internal Vector4 vec;
        [FieldOffset(0)]
        internal Vector2 v;

        internal Vec2d(Vector2 vec)
        {
            v = vec;
        }

        public Vec2d(float x, float y)
        {
            v = new Vector2(x, y);
        }

        public static Vec2d Unit => new Vec2d(1, 1);

        public static Vec2d Zero => new Vec2d(0, 0);

        public float Cross(Vec2d right)
        {
            return (X * right.Y) - (Y * right.X);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2d operator -(Vec2d left, Vec2d right)
        {
            return new Vec2d(left.v - right.v);
        }

        public static bool operator !=(Vec2d left, Vec2d right)
        {
            return !(left == right);
        }

        public static Vec2d operator *(Vec2d vec, float value)
        {
            return new Vec2d(vec.v * value);
        }

        public static Vec2d operator /(Vec2d vec, float value)
        {
            return new Vec2d(vec.v / value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2d operator +(Vec2d left, Vec2d right)
        {
            return new Vec2d(left.v + right.v);
        }

        public static bool operator ==(Vec2d left, Vec2d right)
        {
            return left.v == right.v;
        }

    }
}
