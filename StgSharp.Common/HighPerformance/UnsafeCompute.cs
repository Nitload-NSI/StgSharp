﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="UnsafeCompute.cs"
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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.HighPerformance
{
    public static class UnsafeCompute
    {

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static unsafe int CityHashSimplify( char* span, int length )
        {
            return InternalIO.Intrinsic.city_hash_simplify( span, length );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static unsafe int CityHashSimplify( char* span, int begin, int length )
        {
            return InternalIO.Intrinsic.city_hash_simplify( span + begin, length );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static unsafe int IndexOfCharPair( char* source, int pairValue, int length )
        {
            return InternalIO.Intrinsic.index_pair( source, pairValue, length );
        }

    }
}
