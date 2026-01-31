//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="RegularExpression.Node.Char"
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
                foreach (char item in _charset) {
                    _charset.Add(item);
                }
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
