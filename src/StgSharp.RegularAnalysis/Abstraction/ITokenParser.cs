// -----------------------------------------------------------------------------
// file="ITokenParser"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Abstraction
{
    public abstract class TokenParser<TIn, TOut> : ITokenReader<TOut> where TIn : unmanaged
        where TOut : unmanaged
    {

        public abstract bool IsEmpty { get; }

        protected internal ITokenReader<TIn> Source { get; internal set; }

        public abstract Token<TOut> ReadToken();

        public abstract bool TryReadToken(
                             out Token<TOut> t
        );

    }
}
