//-----------------------------------------------------------------------
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

namespace StgSharp.Script.Express
{
    public partial class ExpNodeGenerator
    {

        private const int EndLine = 1;
        private const int TokenGenerated = 0;
        private const int TokenTypeNotMatch = -1;

        private ExpNode _lastStatement;

        private GeneratedExpSchema _context;
        private IExpElementSource _local;
        private List<ExpNode> _cache;
        private Stack<ExpNode> _operandsNode;
        private Stack<Token> _operandsToken;
        private Stack<Token> _operators;

        public ExpNodeGenerator()
        {
            _operators = new Stack<Token>();
            _operandsToken = new Stack<Token>();
        }

        /// <summary>
        /// Append a token to top of cache. This method will automatically convert token to node if
        /// meets separators or operators.
        /// </summary>
        /// <param name="expToken"></param>
        public ExpNode AppendToken( Token expToken )
        {
            switch( expToken.Flag ) {
                case TokenFlag.Symbol_Unary or TokenFlag.Symbol_Binary:
                    _operandsToken.Push( expToken );
                    break;
                case TokenFlag.Number or  TokenFlag.String or TokenFlag.Member:
                    break;
                case TokenFlag.Separator_Single:         // a , ; found
                    break;
                case TokenFlag.Separator_Left:           //a ( [ { found
                    break;
                case TokenFlag.Separator_Right:          //a } ] ) found
                    break;
                case TokenFlag.Index_Left:               // a [ found
                    break;
                case TokenFlag.Index_Right:              // a ] found
                    break;
                default:
                    break;
            }
        }

        public ExpNode GetNextOperandCache()
        {
            if( _operandsNode.Count != 0 ) {
                return _operandsNode.Pop();
            }
            if( _operandsToken.Count != 0 ) {
                Token token = _operandsToken.Pop();
            }
            return ExpNode.Empty;
        }

        //ConvertOneOperator not finish
        private void ConvertOneOperator()
        {
            Token @operator = _operators.Pop();
            switch( @operator.Flag ) {
                case TokenFlag.Symbol_Unary:
                    break;
                case TokenFlag.Symbol_Binary:
                    break;
                case TokenFlag.Index_Left:
                    break;
                default:
                    break;
            }
        }

    }
}
