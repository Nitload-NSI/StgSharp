﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpNodeGenerator.cs"
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
using StgSharp.Threading;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExpKeyWord = StgSharp.Script.Express.ExpCompile.KeyWord;

namespace StgSharp.Script.Express
{
    public partial class ExpNodeGenerator
    {

        private const int EndLine = 1;
        private const int TokenGenerated = 0;
        private const int TokenTypeNotMatch = -1;

        private CompileStack<ExpNode, IExpElementSource> _cache;

        private ExpNode _lastStatement;

        private GeneratedExpSchema _context;
        private IExpElementSource _local;

        public ExpNodeGenerator()
        {
            _cache = new CompileStack<ExpNode, IExpElementSource>();
        }

        /**/

        /// <summary>
        /// Append a token to top of cache. This method will automatically convert token to node if
        /// meets separators or operators.
        /// </summary>
        public void AppendToken( Token expToken )
        {
            switch( _cache.CurrentDepthMark.State ) {
                default:
                    break;
            }
        }

        public ExpNode GetNextOperandCache()
        {
            if( _cache.PopOperand( out Token t, out ExpNode? n ) ) {
                return n;
            } else { }

            throw new NotImplementedException();
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
                default:
                    break;
            }
        }

        /**/

        public enum StateCode
        {

            Common,
            FunctionCalling,
            ForLoop,
            RepeatLoop,
            UntilLoop,
            IfBranch,
            CaseBranch,

        }

    }
}
