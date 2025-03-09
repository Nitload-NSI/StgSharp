//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpNodeGenerator.FunctionCalling.cs"
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

using static StgSharp.Script.Express.ExpCompile;

namespace StgSharp.Script.Express
{
    public partial class ExpNodeGenerator
    {

        private bool _isCallingFunction = false;
        private List<CompileDepthMark> _funcCallDepthCount = new List<CompileDepthMark>(
            );

        private void EnterFunctionCalling()
        {
            _isCallingFunction = true;
            CompileDepthMark mark = _cache.IncreaseDepth();
            _funcCallDepthCount.Add( mark );
        }

        private void ExitFunctionCalling()
        {
            CompileDepthMark mark = _funcCallDepthCount.Last();
            if( _funcCallDepthCount.Count == 0 ) {
                _isCallingFunction = false;
            }
        }

        //checking _operandAheadOfDepth can be used to check if the function calling has no param.
        //put one of param to tail of function calling pram expression node
        /// <summary>
        /// Compile a parameter expression and add it to head of function parameter node list.
        /// </summary>
        private void MergeFunctionCallingParameter( Token currentSeparator )
        {
            if( _cache.OperandAheadOfDepth == 1 ) {
                if( _cache.PopOperand( out Token t, out ExpNode? n ) ) {
                    n!.PrependNode( ExpNode.NonOperation( currentSeparator ) );
                } else {
                    switch( t.Flag ) {
                        case TokenFlag.Number:
                            if( int.TryParse( t.Value, out int intVal ) ) {
                                n = new ExpIntNode( t, intVal );
                            } else if( float.TryParse(
                                       t.Value, out float floatVal ) ) {
                                n = new ExpRealNumberNode( t, floatVal );
                            }
                            break;
                        case TokenFlag.String:
                            n = new ExpStringNode( t, t.Value );
                            break;
                        case TokenFlag.Member:

                            // case1: name of a instance, make ref copy
                            // case2: name of a function, make a function call
                            // case3: unknown, make a instance member node, check while link
                            break;
                        default:
                            break;
                    }
                }
            } else {
                //TODO compile the param expression
            }
        }

    }
}
