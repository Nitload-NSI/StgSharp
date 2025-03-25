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
using System.Xml.Schema;

using ExpKeyword = StgSharp.Script.Express.ExpressCompile.Keyword;

namespace StgSharp.Script.Express
{
    public partial class ExpNodeGenerator
    {

        private bool IsSeparatorMatch( Token leftSperator, Token rightSeparator )
        {
            return leftSperator.Value switch
            {
                ExpKeyword.LeftBrace => rightSeparator.Value == ExpKeyword.RightBrace,
                ExpKeyword.LeftParen => rightSeparator.Value == ExpKeyword.RightParen,
                ExpKeyword.Comma => rightSeparator.Value == ExpKeyword.Comma,
                ExpKeyword.Semicolon => rightSeparator.Value == ExpKeyword.Semicolon,
                _ => false
            };
        }

        private bool TryAppendIndexLeft( Token t )
        {
            if( t.Flag != TokenFlag.Index_Left ) {
                return false;
            }
            if( _cache.TryPeekOperand( out bool isNode, out _, out ExpNode? node ) )
            {
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
            ClosePrefixReverse( t );

            //make index node
            _cache.PopOperand( out _, out ExpNode? expNode );
            if( !_cache.PopOperand( out Token colleToken, out ExpNode? colleNode ) ) { }
            _cache.PushOperand(
                ExpCollectionIndexNode.Create(
                    t, ( colleNode as ExpCollectionInstanceBase )!, expNode ) );
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
            processOperator:
            if( _cache.OperatorAheadOfDepth == 0 )
            {
                if( _cache.OperandAheadOfDepth == 1 )
                {
                    if( _cache.Depth != 1 )
                    {
                        _cache.DecreaseDepth();
                        if( _cache.PeekOperator().Value == t.Value )
                        {
                            _cache.PopOperand( out _, out ExpNode? currentExpression );
                            _cache.PopOperand( out Token valToken, out _ );
                            _cache.PopOperand( out _, out ExpNode? previousExpression );
                            currentExpression.PrependNode( previousExpression );
                            _cache.PushOperand( previousExpression );
                        }
                    }
                    _cache.PushOperand( t );
                    _cache.PushOperator( t );
                    _cache.IncreaseDepth( ( int )ExpCompileStateCode.Common );
                    return true;
                } else
                {
                    ExpInvalidSyntaxException.ThrowNoOperator( t );
                }
            }
            Token op;
            if( _cache.TryPopOperator( out op ) )
            {
                if( op.Value == t.Value )
                {
                    _cache.PopOperand( out _, out ExpNode? node );

                    //append node to tail
                    _cache.PushOperand( t );
                    _cache.PushOperator( t );
                } else
                {
                    ExpNode node = ConvertAndPushOneOperator( op );
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
            _cache.PushOperand( ExpElementInstance.CreateLiteral( t ) );
            return true;
        }

        private bool TryAppendPrefixSeparator( Token t )
        {
            if( t.Flag != TokenFlag.Separator_Left ) {
                return false;
            }
            _cache.PushOperator( t );
            _cache.IncreaseDepth( ( int )ExpCompileStateCode.PrefixSeparator );
            return true;
        }

        private bool TryAppendPrefixSeparatorEnd( Token t )
        {
            if( t.Flag != TokenFlag.Separator_Right ) {
                return false;
            }
            ClosePrefixReverse( t );
            return true;
        }

        private bool TryAppendSymbol( Token t )
        {
            if( ( t.Flag & ( TokenFlag.Symbol_Unary | TokenFlag.Symbol_Binary ) ) == 0 ) {
                return false;
            }

            int cmp = -1;
            processPrecedence:
            if( _cache.TryPeekOperator( out Token tmp ) )
            {
                cmp = CompareOperatorPrecedence( t, tmp );
                if( cmp >= 0 )
                {
                    _cache.PopOperator();
                    ExpNode node = ConvertAndPushOneOperator( tmp );
                    goto processPrecedence;
                }
            }
            _cache.PushOperator( t );
            return true;
        }

        private bool TryParseConstant( Token t )
        {
            switch( t.Value )
            {
                case ExpKeyword.E:
                    _cache.PushOperand( new ExpRealNumberNode( t, MathF.E, true ) );
                    return true;
                case ExpKeyword.Pi:
                    _cache.PushOperand( new ExpRealNumberNode( t, MathF.PI, true ) );
                    return true;
                default:
                    if( ExpElementInstance.TryCreateLiteral( t, out ExpElementInstance? node ) )
                    {
                        _cache.PushOperand( node );
                        return true;
                    }
                    return false;
            }
        }

        private bool TryParseFunction( Token t )
        {
            if( ExpKeyword.BuiltinFunctions.Contains( t.Value ) )
            {
                _cache.PushOperator( t );
                return true;
            } else if( _context.TryGetFunction( t.Value, out ExpFunctionSource? f ) )
            {
                _cache.PushOperand( t );
                return true;
            }
            return false;
        }

        private bool TryParseInstance( Token t )
        {
            if( _local.TryGetMember( t.Value,
                                     out ExpNode? node ) && node is ExpElementInstance instance )
            {
                _cache.PushOperand( ExpInstanceReferenceNode.MakeReferenceFrom( t, instance ) );
                return true;
            }
            return false;
        }

    }
}
