//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Node.Char"
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
using StgSharp.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis
{
    internal enum EpsilonKind
    {

        Always,
        Begin,
        End,
        LineBegin,
        LineEnd,
        WordBoundary,
        NonWordBoundary

    }

    internal class RegularExpressionEpsilonNode : RegularStateNode
    {

        public RegularExpressionEpsilonNode(
               EpsilonKind kind = EpsilonKind.Always
        )
        {
            Kind = kind;
        }

        public EpsilonKind Kind { get; }

        public override NodeType Type => NodeType.EPSILON;

        public override bool IsValidateFrom(
                             RegularStateNode former,
                             RegularContext context
        )
        {
            int pos = context.Position;
            int len = context.Source?.Length ?? 0;

            bool PrevIsWord()
            {
                if (pos - 1 < 0 || pos - 1 >= len) {
                    return false;
                }

                char c = context.Source[pos - 1];
                return char.IsLetterOrDigit(c) || c == '_';
            }

            bool NextIsWord()
            {
                if (pos < 0 || pos >= len) {
                    return false;
                }

                char c = context.Source[pos];
                return char.IsLetterOrDigit(c) || c == '_';
            }

            switch (Kind)
            {
                case EpsilonKind.Always:
                    return true;
                case EpsilonKind.Begin:
                    return pos == context.Begin;
                case EpsilonKind.End:
                    return pos == len;
                case EpsilonKind.LineBegin:
                    if (pos == 0) {
                        return true;
                    }

                    if (pos - 1 < 0 || pos - 1 >= len) {
                        return false;
                    }

                    return context.Source[pos - 1] == '\n' || context.Source[pos - 1] == '\r';
                case EpsilonKind.LineEnd:
                    if (pos == len) {
                        return true;
                    }

                    if (pos < 0 || pos >= len) {
                        return false;
                    }

                    return context.Source[pos] == '\n' || context.Source[pos] == '\r';
                case EpsilonKind.WordBoundary:
                    return PrevIsWord() != NextIsWord();
                case EpsilonKind.NonWordBoundary:
                    return PrevIsWord() == NextIsWord();
                default:
                    return false;
            }
        }

    }

    internal class RegularExpressionCharNode : RegularStateNode
    {

        private char _char;

        public RegularExpressionCharNode(
               char c
        )
        {
            _char = c;
        }

        public override NodeType Type => NodeType.CHAR;

        public override bool IsValidateFrom(
                             RegularStateNode former,
                             RegularContext context
        )
        {
            int pos = context.Position;
            char c = context.CurrentCharSpan[pos + 1];
            return  c == _char;
        }

    }

    internal class RegularExpressionCharsetNode : RegularStateNode
    {

        private bool _isNegated;
        private HashSet<char> _charset;

        public RegularExpressionCharsetNode(
               IEnumerable<char> charset,
               bool isNegated = false
        )
        {
            _charset = new(charset);
            _isNegated = isNegated;
        }

        public RegularExpressionCharsetNode(
               ReadOnlySpan<char> charset,
               bool isNegated = false
        )
        {
            _charset = new(charset.Length);
            _isNegated = isNegated;
        }

        public override NodeType Type => NodeType.CHARSET;

        public override bool IsValidateFrom(
                             RegularStateNode former,
                             RegularContext context
        )
        {
            int pos = context.Position;
            char c = context.CurrentCharSpan[pos + 1];
            bool contains = _charset.Contains(c);
            return _isNegated ? !contains : contains;
        }

    }
}
