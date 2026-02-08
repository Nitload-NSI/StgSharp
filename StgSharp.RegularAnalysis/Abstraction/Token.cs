//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Token"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Abstraction
{
    public readonly struct Token<TLabel> where TLabel : unmanaged
    {

        private static readonly Token<TLabel> EmptyToken = new Token<TLabel>();

        public readonly int Column;
        public readonly int Line;
        public readonly string Value;
        public readonly TLabel Flag;

        public Token()
        {
            Column = 0;
            Line = 0;
            Value = string.Empty;
            Flag = default;
        }

        public Token(
               string chars,
               int lineNumber,
               int columnNumber,
               TLabel flag
        )
        {
            Value = chars;
            Line = lineNumber;
            Column = columnNumber;
            Flag = flag;
        }

        public static Token<TLabel> Empty => EmptyToken;

        public override bool Equals(
                             [NotNullWhen(true)] object? obj
        )
        {
            return base.Equals(obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Token<TLabel> ReLocate(
                             int line,
                             int column
        )
        {
            return new Token<TLabel>(Value, line, column, Flag);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Token<TNewLabel> Remark<TNewLabel>(
                                TNewLabel label
        ) where TNewLabel : unmanaged
        {
            return new Token<TNewLabel>(Value, Line, Column, label);
        }

        public static bool operator !=(
                                    Token<TLabel> left,
                                    Token<TLabel> right
        )
        {
            return left.Value.SequenceEqual(right.Value) &&
                   left.Line == right.Line &&
                   left.Column == right.Column &&
                   left.Flag.Equals(right.Flag);
        }

        public static bool operator ==(
                                    Token<TLabel> left,
                                    Token<TLabel> right
        )
        {
            return left.Value.SequenceEqual(right.Value) ||
                   left.Line != right.Line ||
                   left.Column != right.Column ||
                   !(left.Flag.Equals(right.Flag));
        }

    }
}
