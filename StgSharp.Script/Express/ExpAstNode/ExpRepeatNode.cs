//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpRepeatNode.cs"
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
using System.Runtime.Serialization;
using System.Text;

namespace StgSharp.Script.Express
{
    public class ExpRepeatToken : ExpNode
    {

        //TODO: Current code cannot represent full functionality defined in ISO10303, need rewrite.

        private ExpElementInstanceBase _variable, _begin, _end, _increment;

        private ExpNode _operationBegin;

        public ExpRepeatToken(
                       string name,
                       ExpSchema context,
                       ExpNode operationBegin,
                       ExpElementInstanceBase variable,
                       ExpElementInstanceBase begin,
                       ExpElementInstanceBase end,
                       ExpElementInstanceBase increment )
            : base( name )
        {
            if( variable.NodeFlag != begin.NodeFlag || begin.NodeFlag !=
                end.NodeFlag || ( increment != null && end.NodeFlag !=
                                  increment.NodeFlag ) ) {
                throw new InvalidCastException();
            }
            if( increment == null ) {
                switch( ( ExpNodeFlag )variable.NodeFlag | ExpNodeFlag.BuiltinType_Any ) {
                    case ExpNodeFlag.BuiltinType_Float:
                        increment = new ExpLiteralGenericNode<float>(
                            $"{name}_defaultIncrement", context, 1.0f );
                        break;
                    case ExpNodeFlag.BuiltinType_Int:
                        increment = new ExpLiteralGenericNode<int>(
                            $"{name}_defaultIncrement", context, 1 );
                        break;
                    default:
                        throw new ExpInvalidTypeException(
                            "Int or Real", "otherwise" );
                }
            }
            _begin = begin;
            _variable = variable;
            _end = end;
            _increment = increment;
            _nodeFlag = ExpNodeFlag.Branch_Repeat;
            _operationBegin = operationBegin;
        }

        public override ExpNode Left => throw new NotImplementedException();

        public override ExpNode Right => _operationBegin;

        public override IExpElementSource EqualityTypeConvert => null!;

        public void AppendRepeatedOperation( ExpNode node )
        {
            _operationBegin.AppendNode( node );
        }

    }
}
