//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpSyntaxAnalyzer.RangeAndIndex.cs"
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
using Microsoft.CodeAnalysis.Operations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExpKeyword = StgSharp.Script.Express.ExpressCompile.Keyword;

namespace StgSharp.Script.Express
{
    public partial class ExpSyntaxAnalyzer
    {

        private void AppendToken_RangeAndIndex( Token t )
        {
            ExpSyntaxNode root;
            switch( t.Value )
            {
                case "[":
                    RangeAndIndexState state = new RangeAndIndexState();
                    state.CurrentState = t.Value;
                    _cache.CurrentDepthMark.StateCache = state;
                    break;
                case "]":
                    state = _cache.StateOfCurrentDepth<RangeAndIndexState>();

                    //Generate range or index object
                    if( _cache.StatementsInDepth.Count != 0 )
                    {
                        throw new ExpInvalidSyntaxException(
                            "Range and index syntax error: expected one statement to define range beginning." );
                    }
                    ReverseAnalyzeStatement();
                    root = _cache.StatementsInDepth.Pop();
                    if( ExpNodeFlag.BuiltinType_Int.IsNotInFlag( root.NodeFlag ) ) {
                        throw new ExpInvalidTypeException(
                            root, "integer", root.EqualityTypeConvert?.Name ?? string.Empty );
                    }
                    if( state.CurrentState == "[" ) { } else { }
                    break;
                case ":":
                    state = _cache.StateOfCurrentDepth<RangeAndIndexState>();
                    if( state.CurrentState != "[" ) {
                        throw new ExpInvalidSyntaxException(
                            "Range and index syntax error: wrong position of :" );
                    }
                    if( _cache.StatementsInDepth.Count != 0 ) {
                        throw new ExpInvalidSyntaxException(
                            "Range and index syntax error: expected one statement to define range beginning." );
                    }
                    ReverseAnalyzeStatement();
                    root = _cache.StatementsInDepth.Pop();
                    if( ExpNodeFlag.BuiltinType_Int.IsNotInFlag( root.NodeFlag ) ) {
                        throw new ExpInvalidTypeException(
                            root, "integer", root.EqualityTypeConvert?.Name ?? string.Empty );
                    }
                    state.RangeStart = root;
                    break;
                default:
                    state = _cache.StateOfCurrentDepth<RangeAndIndexState>();
                    if( t.Value == "?" )
                    {
                        switch( state.CurrentState )
                        {
                            case ":":
                                _cache.PushOperand( new ExpIntNode( t, int.MaxValue, true ) );
                                break;
                            case "[":
                                _cache.PushOperand( new ExpIntNode( t, int.MaxValue, true ) );
                                break;
                            default:
                                throw new ExpInvalidSyntaxException(
                                    "Range and index syntax error: \"?\" can only be used in range or index" );
                        }
                    }
                    AppendToken_Common( t );
                    break;
            }
        }

        private class RangeAndIndexState : ICompileDepthState
        {

            public ExpSyntaxNode RangeStart { get; set; }

            public string CurrentState { get; set; } = string.Empty;

        }

    }
}
