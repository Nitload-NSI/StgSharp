//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpNodeGenerator.OperatorParser.cs"
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

using ExpKeyword = StgSharp.Script.Express.ExpCompile.KeyWord;

namespace StgSharp.Script.Express
{
    public partial class ExpNodeGenerator
    {

        public int CompareOperatorPrecedence( Token left, Token right )
        {
            ExpCompile.TryGetOperatorPrecedence(
                left.Value, out int leftPrecedence );
            ExpCompile.TryGetOperatorPrecedence(
                right.Value, out int rightPrecedence );
            return leftPrecedence - rightPrecedence;
        }

        public bool TryGenerateBinaryNode( Token t, out ExpNode node )
        {
            ExpNode left = GetNextOperandCache();
            ExpNode right = GetNextOperandCache();
            switch( t.Value ) {
                case ExpKeyword.Add:
                    node = ExpBinaryOperatorNode.Add( t, left, right );
                    return true;
                case ExpKeyword.Sub:
                    node = ExpBinaryOperatorNode.Sub( t, left, right );
                    return true;
                case ExpKeyword.Mul:
                    node = ExpBinaryOperatorNode.Mul( t, left, right );
                    return true;
                case ExpKeyword.Div:
                    node = ExpBinaryOperatorNode.Div( t, left, right );
                    return true;
                case ExpKeyword.Equal:
                    node = ExpBinaryOperatorNode.EqualTo( t, left, right );
                    return true;
                case ExpKeyword.NotEqual:
                    node = ExpBinaryOperatorNode.NotEqualTo( t, left, right );
                    return true;
                case ExpKeyword.GreaterThan:
                    node = ExpBinaryOperatorNode.GreaterThan( t, left, right );
                    return true;
                case ExpKeyword.LessThan:
                    node = ExpBinaryOperatorNode.LessThan( t, left, right );
                    return true;
                default:
                    node = ExpNode.Empty;
                    return false;
            }
        }

        public bool TryGenerateUnaryNode( Token t, out ExpNode node )
        {
            ExpNode operand = GetNextOperandCache();
            switch( t.Value ) {
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
                    node = ExpNode.Empty;
                    return false;
            }
        }

        /// <summary>
        /// WARNING: This method cannot process INDEXOF, FUNCTION CALLING, LOOP and any ENDING
        /// SYMBOLS.
        /// </summary>
        private void ConvertOneOperator()
        {
            Token @operator = _cache.PopOperator();

            switch( @operator.Flag ) {
                case TokenFlag.Symbol_Unary:
                    TryGenerateUnaryNode( @operator, out ExpNode? node );
                    _cache.PushOperand( node );
                    break;
                case TokenFlag.Symbol_Binary:
                    TryGenerateBinaryNode( @operator, out node );
                    _cache.PushOperand( node );
                    break;
                case TokenFlag.Member:
                    if( _context.TryGetFunction(
                        @operator.Value, out ExpFunctionSource? f ) ) {
                        if( !_cache.PopOperand( out Token t,
                                                out ExpNode? p ) ) {
                            ExpFunctionCallingNode.CallFunction(
                                @operator, f, p );
                        } else {
                            throw new ExpCompileException(
                                t,
                                "An ExpNode for function calling is needed, but a token was popped" );
                        }
                    } else if( ExpSchema.BuiltinSchema
                                   .TryGetFunction( @operator.Value, out f ) ) {
                        if( !_cache.PopOperand( out Token t,
                                                out ExpNode? p ) ) {
                            ExpFunctionCallingNode.CallFunction(
                                @operator, f, p );
                        } else {
                            throw new ExpCompileException(
                                t,
                                "An ExpNode for function calling is needed, but a token was popped" );
                        }
                    }
                    break;
                default:
                    break;
            }
        }

    }
}
