//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpSyntaxAnalyzer.If.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using ExpKeyword = StgSharp.Script.Express.ExpressCompile.Keyword;

namespace StgSharp.Script.Express
{
    public partial class ExpSyntaxAnalyzer
    {

        private bool AppendToken_IfBranch( Token t )
        {
            switch( t.Value )
            {
                case ExpKeyword.If:
                    IfBranchStateCache state = new IfBranchStateCache();
                    _cache.CurrentDepthMark.StateCache = state;
                    state.CurrentState = t.Value;
                    return true;
                case ExpKeyword.Then:
                    state = _cache.StateOfCurrentDepth<IfBranchStateCache>();
                    if( state.CurrentState == ExpKeyword.If )
                    {
                        while( _cache.TryPopOperator( out Token op ) ) {
                            ConvertAndPushOneOperator( op );
                        }
                        _cache.PopOperand( out _, out ExpSyntaxNode? node );
                        state.CurrentState = t.Value;
                        state.BoolExpression = node;
                    } else
                    {
                        throw new ExpCompileException(
                            t, $"Keyword {t.Value} should be after keyword {ExpKeyword.Then}" );
                    }
                    return true;
                case ExpKeyword.Else:
                    state = _cache.StateOfCurrentDepth<IfBranchStateCache>();
                    if( state.CurrentState == ExpKeyword.Then )
                    {
                        ExpSyntaxNode node = _cache.PackAllStatements();
                        state.CurrentState = t.Value;
                        state.ExpressionIfTrue = node;
                    } else
                    {
                        throw new ExpCompileException(
                            t, $"Keyword {t.Value} should be after keyword {ExpKeyword.Then}" );
                    }
                    return true;

                case ExpKeyword.EndIf:
                    state = _cache.StateOfCurrentDepth<IfBranchStateCache>();
                    if( state.CurrentState == ExpKeyword.Then )
                    {
                        ExpSyntaxNode trueNode = _cache.PackAllStatements();
                        state.CurrentState = t.Value;
                        state.ExpressionIfTrue = trueNode;
                        state.ExpressionIfFalse = ExpSyntaxNode.Empty;
                    } else if( state.CurrentState == ExpKeyword.Else )
                    {
                        ExpSyntaxNode falseNode = _cache.PackAllStatements();
                        state.CurrentState = t.Value;
                        state.ExpressionIfFalse = falseNode;
                    } else
                    {
                        throw new ExpCompileException(
                            t,
                            $"Keyword {t.Value} should be after keyword {ExpKeyword.Then} or {ExpKeyword.Else}" );
                    }
                    ExpIfNode ifNode = new ExpIfNode(
                        state.Begin, state.BoolExpression, state.ExpressionIfTrue,
                        state.ExpressionIfFalse );
                    _cache.DecreaseDepth();
                    _cache.StatementsInDepth.Push( ifNode );
                    return true;
                default:
                    AppendToken_Common( t );
                    return true;
            }
        }

        private class IfBranchStateCache : ICompileDepthState
        {

            public ExpSyntaxNode BoolExpression { get; set; }

            public ExpSyntaxNode ExpressionIfTrue { get; set; }

            public ExpSyntaxNode ExpressionIfFalse { get; set; }

            public string CurrentState { get; set; }

            public Token Begin { get; set; }

        }

    }
}
