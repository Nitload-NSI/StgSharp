//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Token.cs"
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

namespace StgSharp.Script
{
    public readonly struct Token
    {

        private static Token EmptyToken = new Token();

        public readonly int Column;
        public readonly int Line;
        public readonly string Value;
        public readonly TokenFlag Flag;

        public Token(
                       string chars,
                       int lineNumber,
                       int columnNumber,
                       TokenFlag flag )
        {
            Value = chars;
            Line = lineNumber;
            Column = columnNumber;
            Flag = flag;
        }

        public static Token Empty => EmptyToken;

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public Token ReLocate( int line, int column )
        {
            return new Token( Value, line, column, Flag );
        }

    }

    public enum TokenFlag : int
    {

        None = 0,
        Symbol_Unary = 1,
        Symbol_Binary = 2,
        Number = 3,
        String = 4,
        Member = 5,
        Separator_Middle = 6,
        Separator_Left = 7,
        Separator_Right = 8,
        Index_Left = 9,
        Index_Right = 10,

    }
}
