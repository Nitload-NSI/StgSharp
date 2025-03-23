//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpNodeGenerator.PrefixReverseAnalysing.cs"
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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExpKeyword = StgSharp.Script.Express.ExpressCompile.KeyWord;

namespace StgSharp.Script.Express
{
    public partial class ExpNodeGenerator
    {

        private void ClosePrefixReverse( Token rightSeparator )
        {
            Token op;
            while( _cache.TryPopOperator( out op ) ) {
                if( op.Value == ExpKeyword.Comma ) {
                    if( !_cache.TryPopOperand(
                        out bool isNode, out Token t1, out ExpNode? n1 ) ) {
                        throw new ExpCompileException(
                            op, "No expression before a comma" );
                    }
                    if( !isNode ) {
                        //On some occasions, a token may found,
                        //but the processor is not implemented
                        throw new NotImplementedException();
                    }
                    if( !_cache.TryPopOperand(
                        out isNode, out Token t2, out ExpNode? n2 ) ) {
                        throw new ExpCompileException(
                            op, "No expression before a comma" );
                    }
                    if( !isNode ) {
                        //On some occasions, a token may found,
                        //but the processor is not implemented
                        throw new NotImplementedException();
                    }
                    n1.PrependNode( n2 );
                    _cache.PushOperand( n1 );
                    continue;
                }
                ConvertOneOperator( op );

                ExpNode node = GetNextOperandCache();
                _cache.PushOperand( node );
            }
            if( !_cache.TryPopOperand( out _, out _, out ExpNode? root ) ) {
                _cache.PushOperand( ExpNode.Empty );
            }
            _cache.DecreaseDepth();
            if( IsSeparatorMatch( _cache.PeekOperator(), rightSeparator ) ) {
                _cache.PopOperator();
            } else {
                // some unexpected occasions
                throw new NotImplementedException();
            }
        }

    }
}
