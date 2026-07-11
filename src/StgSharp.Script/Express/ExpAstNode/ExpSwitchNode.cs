//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ExpSwitchNode"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
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
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Script.Express
{
    public class ExpSwitchNode : ExpSyntaxNode
    {

        private ExpSyntaxNode _default;
        private ExpSyntaxNode _target;

        private FrozenDictionary<ExpSyntaxNode, ExpSyntaxNode> _cases;

        public ExpSwitchNode(
               Token source,
               ExpSyntaxNode target,
               ExpSyntaxNode defaultCase,
               params (ExpSyntaxNode label, ExpSyntaxNode op)[] cases)
            : base(source)
        {
            _target = target;
            _default = defaultCase;
            Dictionary<ExpSyntaxNode, ExpSyntaxNode> casesDic = new Dictionary<ExpSyntaxNode, ExpSyntaxNode>(
                );
            foreach ((ExpSyntaxNode label, ExpSyntaxNode op ) item in cases)
            {
                if (item.op == null)
                {
                    casesDic.Add(item.label, null!);
                } else
                {
                    casesDic.Add(item.label, item.op);
                }
            }
            _cases = casesDic.ToFrozenDictionary();


            string str = @"switch( {0} ){";
            StringBuilder codeFormatBuilder = new StringBuilder();
            codeFormatBuilder.AppendLine(str);
            for (int i = 0; i < casesDic.Count; i++)
            {
                int index = i * 2 + 1;
                codeFormatBuilder.AppendLine(
                    $$"""
                        case {{{index}}}:
                            {{{index + 1}}}
                            break;
                    """);
            }
            CodeConvertTemplate = codeFormatBuilder.ToString();
        }

        public ExpSwitchNode(
               Token source,
               ExpSyntaxNode target,
               ExpSyntaxNode defaultCase,
               IDictionary<ExpSyntaxNode, ExpSyntaxNode> cases)
            : base(source)
        {
            _target = target;
            _default = defaultCase;
            _cases = cases.ToFrozenDictionary();

            string str = @"switch( {0} ){";
            StringBuilder codeFormatBuilder = new StringBuilder();
            codeFormatBuilder.AppendLine(str);
            for (int i = 0; i < cases.Count; i++)
            {
                int index = i * 2 + 1;
                codeFormatBuilder.AppendLine(
                    $$"""
                        case {{{index}}}:
                            {{{index + 1}}}
                            break;
                    """);
            }
            CodeConvertTemplate = codeFormatBuilder.ToString();
        }

        public ExpSyntaxNode this[ExpSyntaxNode index]
        {
            get
            {
                if (_cases.TryGetValue(index, out ExpSyntaxNode? ret)) {
                    return ret;
                }
                return _default;
            }
        }

        public override ExpSyntaxNode Left => Empty;

        public override ExpSyntaxNode Right => Empty;

        public override IExpElementSource EqualityTypeConvert => null!;

        public bool ContainsCase(ExpSyntaxNode token)
        {
            return _cases.ContainsKey(token);
        }

    }
}
