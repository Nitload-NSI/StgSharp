//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="RegexTokenParser"
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
using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.TextRegex
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
            return t == Token<RegexElementLabel>.Empty;
        }

    }
}
