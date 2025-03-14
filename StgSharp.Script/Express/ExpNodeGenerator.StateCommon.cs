﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpNodeGenerator.StateCommon.cs"
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

using ExpKeyWord = StgSharp.Script.Express.ExpCompile.KeyWord;

namespace StgSharp.Script.Express
{
    public partial class ExpNodeGenerator
    {

        private void AppendToken_Common( Token t )
        {
            switch( t.Flag ) {
                // operand, check precedence and push
                case TokenFlag.Symbol_Unary or TokenFlag.Symbol_Binary:
                    int cmp = -1;
                    processPrecedence:
                    Token tmp = _cache.PeekOperator();
                    cmp = CompareOperatorPrecedence( t, tmp );
                    if( cmp <= 0 ) {
                        ConvertOneOperator();
                        goto processPrecedence;
                    }
                    _cache.PushOperand( t );
                    break;

                // literal, construct and push
                case TokenFlag.Number or TokenFlag.String:
                    _cache.PushOperand( t );
                    break;

                // instance ref, make ref and push
                // function call, push as operator
                // keyword, call TryParseConstant method to process
                // name of member in instance, parse as MemberNameNode
                case TokenFlag.Member:
                    if( _context.TryGetFunction( t.Value, out _ ) ) {
                        _cache.PushOperator( t );
                    } else if( _local.TryGetMember( t.Value,
                                                    out ExpNode? node ) ) {
                        if( node is ExpElementInstanceBase instance ) {
                            _cache.PushOperand( instance.MakeReference( t ) );
                            return;// ref of an instance
                        }
                    } else if( TryParseConstant( t ) ) {
                        return;
                    } else if( TryParseFunction( t ) ) {
                        return;
                    } else if( TryParseBranch( t ) ) {
                        return;
                    } else {
                        _cache.PushOperand( t );
                    }
                    break;
                case TokenFlag.Separator_Single:         //a , ; found
                    // if operator stack is not empty, convert all operators to node until meet a same separator
                    // or OperandAheadOfSeparator property is less than certain value
                    _cache.PushOperator( t );
                    break;
                case TokenFlag.Separator_Left:           //a ( [ { found
                    // check if the last token is a function name, loop block or branch
                    _cache.PushOperator( t );
                    break;
                case TokenFlag.Separator_Right:          //a } ] ) found
                    //Convert operator until meet a matched separator
                    while( _cache.TryPopOperator( out Token op ) ) {
                        if( IsSeparatorMatch( t, op ) ) {
                            return;
                        } else { }
                    }

                    break;
                case TokenFlag.Index_Left:               // a [ found
                    break;
                case TokenFlag.Index_Right:              // a ] found
                    break;
                default:
                    break;
            }
        }

        private bool IsSeparatorMatch( Token source, Token target )
        {
            return source.Value switch
            {
                ExpKeyWord.LeftBrace => target.Value == ExpKeyWord.RightBrace,
                ExpKeyWord.LeftParen => target.Value == ExpKeyWord.RightParen,
                ExpKeyWord.Comma => target.Value == ExpKeyWord.Comma,
                ExpKeyWord.Semicolon => target.Value == ExpKeyWord.Semicolon,
                _ => false
            };
        }

        private bool TryParseBranch( Token t )
        {
            if( t.Value == ExpKeyWord.Case ) {
                _cache.PushOperator( t );
                _cache.IncreaseDepth( ( int )StateCode.CaseBranch );
                return true;
            } else if( t.Value == ExpKeyWord.If ) {
                _cache.PushOperator( t );
                _cache.IncreaseDepth( ( int )StateCode.IfBranch );
                return true;
            } else if( t.Value == ExpKeyWord.Repeat ) {
                _cache.PushOperator( t );
                _cache.IncreaseDepth( ( int )StateCode.RepeatLoop );
                return true;
            }
            return false;
        }

        private bool TryParseConstant( Token t )
        {
            if( t.Value == ExpKeyWord.True ) {
                _cache.PushOperand( new ExpBoolNode( t, true ) );
                return true;
            } else if( t.Value == ExpKeyWord.False ) {
                _cache.PushOperand( new ExpBoolNode( t, false ) );
                return true;
            }
            return false;
        }

        private bool TryParseFunction( Token t )
        {
            if( ExpKeyWord.BuiltinFunctions.Contains( t.Value ) ) {
                _cache.PushOperator( t );
                _cache.IncreaseDepth( ( int )StateCode.FunctionCalling );
                return true;
            } else if( _context.TryGetFunction(
                       t.Value, out ExpFunctionSource? f ) ) {
                _cache.PushOperand( t );
                _cache.IncreaseDepth( ( int )StateCode.FunctionCalling );
                return true;
            }
            return false;
        }

    }
}
