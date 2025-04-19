//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpSyntaxAnalyzer.cs"
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

using StgSharp.Threading;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExpKeyword = StgSharp.Script.Express.ExpressCompile.Keyword;

namespace StgSharp.Script.Express
{
    public partial class ExpSyntaxAnalyzer : ISyntaxAnalyzer
    {

        private const int EndLine = 1;
        private const int TokenGenerated = 0;
        private const int TokenTypeNotMatch = -1;

        private CompileStack<ExpSyntaxNode, IExpElementSource> _cache;
        private ExpSchema _context;
        private ExpSyntaxNode _lastStatement;
        private IExpElementSource _local;

        public ExpSyntaxAnalyzer()
        {
            _cache = new CompileStack<ExpSyntaxNode, IExpElementSource>();
        }

        public ExpSyntaxAnalyzer( ExpSchema context, IExpElementSource local )
        {
            _cache = new CompileStack<ExpSyntaxNode, IExpElementSource>();
            ArgumentNullException.ThrowIfNull( context );
            if( context is not ExpSchema_Builtin )
            {
                _context = context;
            } else
            {
                throw new InvalidCastException( "Not a valid Schema type" );
            }
            _local = local;
        }

        /**/
        public void AppendToken( Token expToken )
        {
            switch( ( ExpCompileStateCode )TryEnterBranch( expToken ).Usage )
            {
                case ExpCompileStateCode.IfBranch:
                    AppendToken_IfBranch( expToken );
                    break;
                case ExpCompileStateCode.CaseBranch:
                    AppendToken_CaseOf( expToken );
                    break;
                case ExpCompileStateCode.RepeatLoop:
                    AppendToken_RepeatLoop( expToken );
                    break;
                default:
                    AppendToken_common( expToken );
                    break;
            }
        }

        public ExpSyntaxNode GetNextOperandCache()
        {
            if( _cache.PopOperand( out Token t, out ExpSyntaxNode? n ) )
            {
                return n;
            } else
            {
                // process some rare case here
            }

            throw new NotImplementedException();
        }

        /// <summary>
        ///   Append a token to top of cache. This method will automatically convert token to node
        ///   if meets separators or operators.
        /// </summary>
        private void AppendToken_common( Token expToken )
        {
            switch( expToken.Flag )
            {
                case TokenFlag.Symbol_Unary or TokenFlag.Symbol_Binary:
                    TryAppendSymbol( expToken );
                    break;
                case TokenFlag.Number or TokenFlag.String:
                    ExpElementInstance node = ExpElementInstance.CreateLiteral( expToken );
                    _cache.PushOperand( node );
                    break;
                case TokenFlag.Separator_Left:
                    TryAppendPrefixSeparator( expToken );
                    break;
                case TokenFlag.Separator_Right:
                    TryAppendPrefixSeparatorEnd( expToken );
                    break;
                case TokenFlag.Separator_Middle:
                    TryAppendMiddleSeparator( expToken );
                    break;
                case TokenFlag.Index_Left:
                    TryAppendIndexLeft( expToken );
                    break;
                case TokenFlag.Index_Right:
                    TryAppendIndexRight( expToken );
                    break;
                case TokenFlag.Member:
                    TryAppendMember( expToken );
                    break;
                default:
                    break;
            }
        }

        private CompileDepthMark TryEnterBranch( Token t )
        {
            if( t.Flag == TokenFlag.String ) {
                return _cache.CurrentDepthMark;
            }
            switch( t.Value )
            {
                case ExpKeyword.Case:
                    return _cache.IncreaseDepth( ( int )ExpCompileStateCode.CaseBranch );
                case ExpKeyword.If:
                    return _cache.IncreaseDepth( ( int )ExpCompileStateCode.IfBranch );
                case ExpKeyword.Repeat:
                    return _cache.IncreaseDepth( ( int )ExpCompileStateCode.RepeatLoop );
                default:
                    return _cache.CurrentDepthMark;
            }
        }

        /**/

        public enum ExpCompileStateCode
        {

            Common,
            ForLoop,
            RepeatLoop,
            UntilLoop,
            IfBranch,
            CaseBranch,
            CollectionIndex,

        }

    }
}
