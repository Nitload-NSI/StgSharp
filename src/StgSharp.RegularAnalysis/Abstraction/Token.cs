// -----------------------------------------------------------------------------
// file="Token"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

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

        public static Token<TLabel> Empty { get; } = new Token<TLabel>();

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
            return !left.Value.SequenceEqual(right.Value) ||
                   left.Line != right.Line ||
                   left.Column != right.Column ||
                   !left.Flag.Equals(right.Flag);
        }

        public static bool operator ==(
                                    Token<TLabel> left,
                                    Token<TLabel> right
        )
        {
            return left.Value.SequenceEqual(right.Value) &&
                   left.Line == right.Line &&
                   left.Column == right.Column &&
                   left.Flag.Equals(right.Flag);
        }

    }
}
