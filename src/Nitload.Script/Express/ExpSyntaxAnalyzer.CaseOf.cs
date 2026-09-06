//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ExpSyntaxAnalyzer.CaseOf"
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExpKeyword = StgSharp.Script.Express.ExpressCompile.Keyword;

namespace StgSharp.Script.Express
{
    public partial class ExpSyntaxAnalyzer
    {

        private bool AppendToken_CaseOf(Token t)
        {
            Token op;
            ExpSyntaxNode root;
            switch (t.Value)
            {
                case ExpKeyword.Case:
                    CaseOfStateCache state = new CaseOfStateCache(t);
                    _cache.CurrentDepthMark.StateCache = state;
                    state.CurrentState = t.Value;
                    return true;
                case ExpKeyword.Of:
                    state = _cache.StateOfCurrentDepth<CaseOfStateCache>();
                    while (_cache.TryPopOperator(out op)) {
                        ConvertAndPushOneOperator(op);
                    }
                    _cache.PopOperand(out _, out root);
                    state.CaseExpression = root;
                    state.CurrentState = t.Value;
                    return true;
                case ":":
                    state = _cache.StateOfCurrentDepth<CaseOfStateCache>();
                    if (state.CurrentState != ExpKeyword.Of && state.CurrentState != ExpKeyword.Otherwise)
                    {
                        root = _cache.PackAllStatements();
                        state.CloseCurrentCase(root);
                    }
                    while (_cache.TryPopOperator(out op)) {
                        ConvertAndPushOneOperator(op);
                    }
                    if (!_cache.PopOperand(out _, out root))
                    {
                        // sth wrong, case expression cannot generate;
                    }
                    state.CurrentCase = root;
                    return true;
                case ExpKeyword.Otherwise:
                    state = _cache.StateOfCurrentDepth<CaseOfStateCache>();
                    root = _cache.PackAllStatements();
                    state.CloseCurrentCase(root);
                    state.CurrentState = t.Value;
                    return true;
                case ExpKeyword.EndCase:
                    state = _cache.StateOfCurrentDepth<CaseOfStateCache>();
                    root = _cache.PackAllStatements();
                    state.CloseCurrentCase(root);
                    root = new ExpSwitchNode(
                        state.Begin, state.CaseExpression, state.DefaultCase, state.Cases);
                    _cache.DecreaseDepth();
                    _cache.StatementsInDepth.Push(root);
                    return true;
                default:
                    AppendToken_Common(t);
                    return true;
            }
        }

        private class CaseOfStateCache : ICompileDepthState
        {

            public CaseOfStateCache(Token t)
            {
                Begin = t;
            }

            public Dictionary<ExpSyntaxNode, ExpSyntaxNode> Cases { get; } = new Dictionary<ExpSyntaxNode, ExpSyntaxNode>(
                );

            public ExpSyntaxNode CaseExpression { get; set; }

            public ExpSyntaxNode DefaultCase { get; set; }

            public ExpSyntaxNode CurrentCase { get; set; }

            public string CurrentState { get; set; }

            public Token Begin { get; init; }

            public void CloseCurrentCase(ExpSyntaxNode root)
            {
                Cases.Add(CurrentCase, root);
            }

        }

    }
}
