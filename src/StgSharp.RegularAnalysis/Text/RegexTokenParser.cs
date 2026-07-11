// -----------------------------------------------------------------------------
// file="RegexTokenParser"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Text
{
    internal class RegexTokenParser : TokenParser<RegexElementLabel, RegexElementLabel>
    {

        private readonly Queue<Token<RegexElementLabel>> _buffer = [];

        private RegexElementLabel _prev;

        public override bool IsEmpty => Source.IsEmpty && _buffer.Count == 0;

        public override Token<RegexElementLabel> ReadToken()
        {
            if (_buffer.TryDequeue(out Token<RegexElementLabel> token)) {
                return token;
            }
            if (Source.TryReadToken(out Token<RegexElementLabel> orig))
            {
                if (((int)(orig.Flag & RegexElementLabel.ATOM_BEGIN) != 0) && ((int)_prev != 0))
                {
                    Token<RegexElementLabel> ret = new Token<RegexElementLabel>(string.Empty,
                                                                                orig.Line,
                                                                                orig.Column,
                                                                                RegexElementLabel.CONCAT);
                    _buffer.Enqueue(orig);
                    _prev = orig.Flag & RegexElementLabel.ATOM_END;
                    return ret;
                } else
                {
                    _prev = orig.Flag & RegexElementLabel.ATOM_END;
                    return orig;
                }
            } else
            {
                return Token<RegexElementLabel>.Empty;
            }
        }

        public override bool TryReadToken(
                             out Token<RegexElementLabel> t
        )
        {
            if (IsEmpty)
            {
                t = Token<RegexElementLabel>.Empty;
                return false;
            }
            t = ReadToken();
            return t != Token<RegexElementLabel>.Empty;
        }

    }
}
