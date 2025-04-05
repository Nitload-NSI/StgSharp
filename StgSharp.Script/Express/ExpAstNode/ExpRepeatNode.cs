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
    public class ExpRepeatNode : ExpNode
    {

        private ExpElementInstance _variable;
        private ExpNode _begin, _end, _increment, _operationBegin,_untilRule,_whileRule;

        internal ExpRepeatNode(
                 Token source,
                 ExpNode operationBegin,
                 ExpElementInstance variable,
                 ExpNode begin,
                 ExpNode end )
            : base( source )
        {
            ExpNode increment;
            switch( ( ExpNodeFlag )variable.NodeFlag | ExpNodeFlag.BuiltinType_Any )
            {
                case ExpNodeFlag.BuiltinType_Real:
                    increment = new ExpRealNumberNode(
                        new Token(
                            ExpressCompile.PoolString( "1.0f" ), source.Line, -1,
                            TokenFlag.Number ),
                        1.0f, isLiteral: true );
                    break;
                case ExpNodeFlag.BuiltinType_Int:
                    increment = new ExpIntNode(
                        new Token(
                            ExpressCompile.PoolString( "1" ), source.Line, -1, TokenFlag.Number ),
                        1, isLiteral: true );
                    break;
                default:
                    throw new ExpInvalidTypeException( "Int or Real", "otherwise" );
            }
            _begin = begin;
            _variable = variable;
            _end = end;
            _increment = increment;
            _nodeFlag = ExpNodeFlag.Branch_Repeat;
            _operationBegin = operationBegin;
        }

        internal ExpRepeatNode(
                 Token source,
                 ExpNode operationBegin,
                 ExpElementInstance variable,
                 ExpNode begin,
                 ExpNode end,
                 ExpNode increment )
            : base( source )
        {
            if( variable.NodeFlag != begin.NodeFlag || begin.NodeFlag != end.NodeFlag || ( increment !=
                                                                                           null && end.NodeFlag !=
                                                                                           increment.NodeFlag ) ) {
                throw new InvalidCastException();
            }
            ArgumentNullException.ThrowIfNull( increment );
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

        public static ExpRepeatNode Create(
                                    Token t,
                                    ExpElementInstance repeatVariable,
                                    ExpNode begin,
                                    ExpNode end,
                                    ExpNode repeatBody )
        {
            string typename = repeatVariable.NodeFlag.ToString();
            ExpRepeatNode ret = new ExpRepeatNode( t, repeatBody, repeatVariable, begin, end, null )
            {
                /*
                 * 0 increment variable type
                 * 1: variable name
                 * 2: beginning value
                 * 3: ending value
                 * 4: increment
                 * 5: body
                 */
                CodeConvertTemplate = $$"""
                for({{0}} {{1}} = {{2}}; {{1}} <= {{3}}; {{1}} += {{4}})
                {
                    {{5}}
                }
                """
            };
            return ret;
        }

        public static ExpRepeatNode Create(
                                    Token t,
                                    ExpElementInstance repeatVariable,
                                    ExpNode begin,
                                    ExpNode end,
                                    ExpNode increment,
                                    ExpNode whileRule,
                                    ExpNode untilRule,
                                    ExpNode body )
        {
            ExpRepeatNode ret = new ExpRepeatNode( t, body, repeatVariable, begin, end, increment )
            {
                /*
                 * 0 increment variable type
                 * 1: variable name
                 * 2: beginning value
                 * 3: ending value
                 * 4: increment
                 * 5: body
                 * 6: while
                 * 7: until
                 */
                CodeConvertTemplate = $$"""
                for({{0}} {{1}} = {{2}}; {{1}} <= {{3}}; {{1}} += {{4}})
                {
                    if(!( {{6}} )) break;
                    {{5}}
                    if(!( {{7}} )) break;
                }
                """
            };
            return ret;
        }

    }
}
