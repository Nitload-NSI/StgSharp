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
        private Stack<int> _callDepth = new Stack<int>();

        private void EnterFunctionCalling( Token funcNameToken )
        {
            _isCallingFunction = true;
            _callDepth.Push( _operandsToken.Count );
        }

        private void ExitFunctionCalling()
        {
            _callDepth.Pop();
            if( _callDepth.Count == 0 ) {
                _isCallingFunction = false;
            }
        }

        //TODO cannot process occasion of void input or null at first
        //put one of param to tail of function calling pram expression node
        private void MergeFunctionCallingParameter()
        {
            if( _cache.OperandAheadOfDepth == 1 ) {
                if( _cache.PopOperand( out Token t, out ExpNode? n ) ) {
                    n!.PrependNode(
                        ExpNode.NonOperation( PoolString( "FuncParam" ) ) );
                } else { }
            } else {
                //TODO compile the param expression
            }
        }

    }
}
}
