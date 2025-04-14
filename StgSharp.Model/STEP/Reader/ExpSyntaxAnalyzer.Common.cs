//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepExpSyntaxAnalyzer.Common.cs"
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
using Microsoft.CodeAnalysis.CSharp.Syntax;

using StgSharp.Script;
using StgSharp.Script.Express;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

using ExpKeyword = StgSharp.Script.Express.ExpressCompile.Keyword;

namespace StgSharp.Modeling.Step
{
    public partial class StepExpSyntaxAnalyzer
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
            if( TryParseKeyword( t ) ) {
                return true;
            }
            if( TryParseElementName( t ) ) {
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
            while( _cache.OperatorAheadOfDepth > 0 )
            {
                _cache.TryPopOperator( out Token op );
                ExpNode node = ConvertAndPushOneOperator( op );
            }
            switch( _cache.OperandAheadOfDepth )
            {
                case 0:
                    if( t.Value == ";" ) {
                        return true;
                    }
                    _cache.StatementsInDepth.Push( ExpNode.Empty );
                    goto case 1;
                case 1:
                    _cache.PopOperand( out _, out ExpNode? statement );
                    _cache.StatementsInDepth.Push( statement );
                    return true;
                default:
                    ExpInvalidSyntaxException.ThrowNoOperator( t );
                    break;
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
            _cache.IncreaseDepth( ( int )ExpCompileStateCode.Common );
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

        private bool TryParseElementName( Token t )
        {
            if( _context.TryGetType( t.Value, out ExpTypeSource? type ) )
            {
                _cache.PushOperand( ExpMetaRefNode.RefType( t, type ) );
                return true;
            }
            if( _context.TryGetEntity( t.Value, out ExpEntitySource? entity ) )
            {
                _cache.PushOperand( ExpMetaRefNode.RefEntity( t, entity ) );
                return true;
            }
            return false;
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
            } else if( _cache.IsLastAddedOperator )
            {
                if( _cache.PeekOperand( out _,
                                        out ExpNode? typenode ) && typenode is ExpMetaRefNode typeRef )
                {
                    _cache.PopOperand( out _, out _ );
                    _local.AddMember( t.Value, typeRef.SourceRef.CreateInstanceNode( t ) );
                } else
                {
                    return false;
                }
            }
            return false;
        }

        private bool TryParseKeyword( Token t )
        {
            switch( t.Value )
            {
                case ExpKeyword.Return:
                    _cache.PushOperator( t );
                    return true;
                case ExpKeyword.Skip:
                    _cache.PushOperand( ExpProcedureControlNode.Skip( t ) );
                    return true;
                default:
                    return false;
            }
        }

    }
}
