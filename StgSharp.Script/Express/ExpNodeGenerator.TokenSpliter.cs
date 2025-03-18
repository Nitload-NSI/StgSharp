//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpNodeGenerator.TokenSpliter.cs"
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
                            if( _cache.OperatorAheadOfDepth == 0 ) {
                                _cache.TryPeekOperand(
                                    out _, out Token tOperand,
                                    out ExpNode nOperand );

                                return;
                            }
                        } else {
                            return;
                        }
                    }

                    break;


                case TokenFlag.Index_Left:               // a [ found
                    _cache.PushOperand( t );
                    return;
                case TokenFlag.Index_Right:              // a ] found
                    while( _cache.TryPopOperator( out Token op ) ) {
                        if( op.Value == ExpKeyWord.IndexOf ) { } else { }
                    }
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

        private bool TryAppendMember( Token t )
        {
            if( t.Flag != TokenFlag.Member ) {
                return false;
            }
            if( TryParseConstant( t ) ) {
                return true;
            }
            if( TryParseFunction( t ) ) {
                return true;
            }
            if( TryParseInstance( t ) ) {
                return true;
            }

            //Many thing miss here

            _cache.PushOperand( ExpElementMemberNameNode.Create( t ) );

            return true;
        }

        private bool TryAppendNumAndStrLiteral( Token t )
        {
            if( ( t.Flag & ( TokenFlag.String | TokenFlag.Number ) ) == 0 ) {
                return false;
            }
            _cache.PushOperand( ExpElementInstanceBase.CreateLiteral( t ) );
            return true;
        }

        private bool TryAppendPrefixSeparator( Token t )
        {
            if( t.Flag != TokenFlag.Separator_Left ) {
                return false;
            }
            _cache.IncreaseDepth( ( int )ExpCompileStateCode.PrefixSeparator );
            return true;
        }

        private bool TryAppendSymbol( Token t )
        {
            if( ( t.Flag & ( TokenFlag.Symbol_Unary | TokenFlag.Symbol_Binary ) ) ==
                0 ) {
                return false;
            }

            int cmp = -1;
            processPrecedence:
            Token tmp = _cache.PeekOperator();
            cmp = CompareOperatorPrecedence( t, tmp );
            if( cmp <= 0 ) {
                ConvertOneOperator();
                goto processPrecedence;
            }
            _cache.PushOperand( t );
            return true;
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
                return true;
            } else if( _context.TryGetFunction(
                       t.Value, out ExpFunctionSource? f ) ) {
                _cache.PushOperand( t );
                return true;
            }
            return false;
        }

        private bool TryParseInstance( Token t )
        {
            if( _local.TryGetMember( t.Value,
                                     out ExpNode? node ) && node is ExpElementInstanceNode instance ) {
                _cache.PushOperand(
                    ExpInstanceReferenceNode.Create( t, instance ) );
                return true;
            }
            return false;
        }

    }
}
