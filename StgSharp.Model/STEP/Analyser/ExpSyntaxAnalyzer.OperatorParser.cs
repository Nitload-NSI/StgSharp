//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpSyntaxAnalyzer.OperatorParser.cs"
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
using StgSharp.Script;
using StgSharp.Script.Express;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExpKeyword = StgSharp.Script.Express.ExpressCompile.Keyword;

namespace StgSharp.Model.Step
{
    internal partial class StepExpSyntaxAnalyzer
    {

        public int CompareOperatorPrecedence( Token left, Token right )
        {
            ExpressCompile.TryGetOperatorPrecedence( left.Value, out int leftPrecedence );
            ExpressCompile.TryGetOperatorPrecedence( right.Value, out int rightPrecedence );
            return leftPrecedence - rightPrecedence;
        }

        public bool TryGenerateUnaryNode( Token t, out ExpSyntaxNode node )
        {
            ExpSyntaxNode operand = GetNextOperandCache();
            switch( t.Value )
            {
                case ExpKeyword.UnaryPlus:
                    node = ExpUnaryOperatorNode.UnaryPlus( t, operand );
                    return true;
                case ExpKeyword.UnaryMinus:
                    node = ExpUnaryOperatorNode.UnaryMinus( t, operand );
                    return true;
                case ExpKeyword.Not:
                    node = ExpUnaryOperatorNode.UnaryNot( t, operand );
                    return true;
                default:
                    node = ExpSyntaxNode.Empty;
                    return false;
            }
        }

        /// <summary>
        ///   WARNING: This method cannot process INDEXOF, FUNCTION CALLING, LOOP and any ENDING
        ///   SYMBOLS.
        /// </summary>
        private ExpSyntaxNode ConvertAndPushOneOperator( Token op )
        {
            switch( op.Flag )
            {
                case TokenFlag.Symbol_Unary:
                    TryGenerateUnaryNode( op, out ExpSyntaxNode? node );
                    _cache.PushOperand( node );
                    return node;
                case TokenFlag.Symbol_Binary:
                    TryGenerateBinaryNode( op, out node );
                    _cache.PushOperand( node );
                    return node;
                case TokenFlag.Member:
                    _cache.PopOperand( out _, out ExpSyntaxNode? p );
                    StepEntityInitCaller caller = StepEntityInitCaller.CreateCaller( op.Value );
                    node = ExpFunctionCallingNode.CallFunction( op, caller, p );
                    _cache.StatementsInDepth.Push( node );
                    return node;
                default:
                    throw new NotImplementedException();
            }
        }

        private bool TryGenerateBinaryNode( Token t, out ExpSyntaxNode node )
        {
            /*
            ExpSyntaxNode right = GetNextOperandCache();
            ExpSyntaxNode left = GetNextOperandCache();
            /**/
            switch( t.Value )
            {
                case "=":
                    ExpSyntaxNode right = GetNextOperandCache();
                    ExpSyntaxNode left;
                    if( right is ExpTupleNode tuple )
                    {
                        right = new StepComplexEntityNode( tuple );
                        left = GetNextOperandCache();
                    } else
                    {
                        left = right;
                        right = _cache.StatementsInDepth.Pop();
                    }

                    node = ExpBinaryOperatorNode.Assign( t, left, right );
                    return true;
                default:
                    node = ExpSyntaxNode.Empty;
                    return false;
            }
        }

    }
}
