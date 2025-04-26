//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpSyntaxAnalyzer.PrefixReverseAnalysing.cs"
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
using StgSharp.Script;
using StgSharp.Script.Express;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExpKeyword = StgSharp.Script.Express.ExpressCompile.Keyword;

namespace StgSharp.Modeling.Step
{
    internal partial class StepExpSyntaxAnalyzer
    {

        private void ClosePrefixReverse( Token rightSeparator )
        {
            Token op;
            ExpSyntaxNode root;
            if( _cache.OperatorAheadOfDepth == 0 )
            {
                if( _cache.OperandAheadOfDepth == 1 )
                {
                    _cache.PopOperand( out _, out ExpSyntaxNode? statement );
                    _cache.StatementsInDepth.Push( statement );
                } else
                {
                    ExpInvalidSyntaxException.ThrowNoOperator( rightSeparator );
                }
            } else
            {
                while( _cache.TryPopOperator( out op ) ) {
                    ConvertAndPushOneOperator( op );
                }
                _cache.PopOperand( out _, out root );
                _cache.StatementsInDepth.Push( root );
            }

            root = _cache.PackAllStatements();
            _cache.DecreaseDepth();
            if( IsSeparatorMatch( _cache.PeekOperator(), rightSeparator ) )
            {
                _cache.PopOperator();
                _cache.PushOperand( root );
            } else
            {
                throw new ExpCompileException(
                    rightSeparator, "Ending of block does not match its begging" );
            }
        }

    }
}
