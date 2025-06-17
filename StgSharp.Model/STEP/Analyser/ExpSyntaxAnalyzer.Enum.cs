//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpSyntaxAnalyzer.Enum.cs"
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Model.Step
{
    internal partial class StepExpSyntaxAnalyzer
    {

        private static string ParseStepEnumLiteral( string source )
        {
            int length = source.Length;
            ReadOnlySpan<char> span = source.AsSpan();
            if( length > 2 && span[ 0 ] == '.' && span[ length - 1 ] == '.' ) {
                return span.Slice( 1, length - 2 ).ToString();
            }
            throw new ExpInvalidSyntaxException( $"\"{source}\" is not a valid enum syntax" );
        }

        private void TryAppendEnum( Token e )
        {
            ExpSyntaxNode node = e.Value switch
            {
                ".T." => new ExpBoolNode( e, true, true ),
                ".F." => new ExpBoolNode( e, false, true ),
                ".U." => new ExpLogicValueNode( e, ExpLogic.Unknown, true ),
                _ => ExpEnumLiteralNode.Create( e, ParseStepEnumLiteral ),
            };
            _cache.PushOperand( node );
        }

    }
}
