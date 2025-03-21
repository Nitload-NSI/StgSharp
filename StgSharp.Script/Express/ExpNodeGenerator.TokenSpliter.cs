﻿//-----------------------------------------------------------------------
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

using ExpKeyword = StgSharp.Script.Express.ExpCompile.KeyWord;

namespace StgSharp.Script.Express
{
    public partial class ExpNodeGenerator
    {

        private bool IsSeparatorMatch( Token source, Token target )
        {
            return source.Value switch
            {
                ExpKeyword.LeftBrace => target.Value == ExpKeyword.RightBrace,
                ExpKeyword.LeftParen => target.Value == ExpKeyword.RightParen,
                ExpKeyword.Comma => target.Value == ExpKeyword.Comma,
                ExpKeyword.Semicolon => target.Value == ExpKeyword.Semicolon,
                _ => false
            };
        }

        private bool TryAppendIndexLeft( Token t )
        {
            if( t.Flag != TokenFlag.Index_Left ) {
                return false;
            }
            if( _cache.TryPeekOperand(
                out bool isNode, out _, out ExpNode? node ) ) {
                if( node.EqualityTypeConvert is not ExpCollectionBase ) {
                    ExpInvalidSyntaxException.ThrowNonCollectionIndex( t );
                }
            }
            _cache.PushOperator( t );
            _cache.IncreaseDepth( ( int )ExpCompileStateCode.CollectionIndex );
            return true;
        }

        private bool TryAppendIndexRight( Token t )
        {
            if( t.Flag != TokenFlag.Index_Right ) {
                return false;
            }

            //reverse building

            //make index node
            _cache.PushOperand( ExpCollectionIndexNode.Create( t ) );
            return true;
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

        private bool TryAppendMiddleSeparator( Token t )
        {
            if( t.Flag != TokenFlag.Separator_Middle ) {
                return false;
            }
            if( _cache.OperatorAheadOfDepth == 0 ) {
                if( _cache.OperandAheadOfDepth == 1 ) {
                    _cache.PushOperand( t );
                    _cache.PushOperator( t );
                    return true;
                } else {
                    ExpInvalidSyntaxException.ThrowNoOperator( t );
                }
            }
            Token op;
            processOperator:
            if( _cache.TryPopOperator( out op ) ) {
                if( op.Value == t.Value ) {
                    _cache.PopOperand( out _, out ExpNode? node );

                    //append node to tail
                    _cache.PushOperand( t );
                    _cache.PushOperator( t );
                } else {
                    ConvertOneOperator();
                    goto processOperator;
                }
            }
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

        private bool TryAppendPrefixSeparatorEnd( Token t )
        {
            if( t.Flag != TokenFlag.Separator_Right ) {
                return false;
            }

            //convert all operator to node until meet a prefix separator

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
            if( t.Value == ExpKeyword.True ) {
                _cache.PushOperand( new ExpBoolNode( t, true ) );
                return true;
            } else if( t.Value == ExpKeyword.False ) {
                _cache.PushOperand( new ExpBoolNode( t, false ) );
                return true;
            }
            return false;
        }

        private bool TryParseFunction( Token t )
        {
            if( ExpKeyword.BuiltinFunctions.Contains( t.Value ) ) {
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
                                     out ExpNode? node ) && node is ExpElementInstanceBase instance ) {
                _cache.PushOperand(
                    ExpInstanceReferenceNode.Create( t, instance ) );
                return true;
            }
            return false;
        }

    }
}
