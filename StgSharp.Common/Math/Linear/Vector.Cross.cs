﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Vector.Cross.cs"
//     Project: World
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
namespace StgSharp.Math
{
    public static unsafe partial class Linear
    {

        public static float Cross( Vec2 vec1, Vec2 vec2 )
        {
            return ( vec1.X * vec2.Y ) - ( vec1.Y * vec2.X );
        }

        public static Vec3 Cross( Vec3 vec1, Vec3 vec2 )
        {
            return new Vec3(
                ( vec1.Y * vec2.Z ) - ( vec1.Z * vec2.Y ),
                ( vec1.X * vec2.Z ) - ( vec1.Z * vec2.X ),
                ( vec1.X * vec2.Y ) - ( vec1.Y * vec2.X ) );
        }

    }
}
