// -----------------------------------------------------------------------------
// file="ITokenReader"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;

namespace StgSharp.RegularAnalysis.Abstraction
{
    public static class TokenReader
    {

        public static TokenParser<TIn, TOut> Pipe<TIn, TOut>(
                                             this ITokenReader<TIn> source,
                                             Func<TokenParser<TIn, TOut>> builder
        ) where TIn : unmanaged where TOut : unmanaged
        {
            TokenParser<TIn, TOut> parser = builder();
            parser.Source = source;
            return parser;
        }

    }

    public interface ITokenReader<TLabel> where TLabel : unmanaged
    {

        bool IsEmpty { get; }

        Token<TLabel> ReadToken();

        bool TryReadToken(
             out Token<TLabel> t
        );

    }
}