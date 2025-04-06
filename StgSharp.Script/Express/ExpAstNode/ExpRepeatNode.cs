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
using System.Threading.Channels;

namespace StgSharp.Script.Express
{
    public class ExpRepeatNode : ExpNode
    {

        private ExpElementInstance _variable;
        private ExpNode _begin, _end, _increment, _operationBegin,_untilRule,_whileRule;

        internal ExpRepeatNode(
                 Token source,
                 ExpElementInstance variable,
                 ExpNode begin,
                 ExpNode end,
                 ExpNode increment,
                 ExpNode operationBegin,
                 ExpNode untilRule,
                 ExpNode whileRule )
            : base( source )
        {
            if( variable.NodeFlag != begin.NodeFlag || begin.NodeFlag != end.NodeFlag || ( increment !=
                                                                                           null && end.NodeFlag !=
                                                                                           increment.NodeFlag ) ) {
                throw new InvalidCastException();
            }
            if( ExpNode.IsNullOrEmpty( increment ) )
            {
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
                                ExpressCompile.PoolString( "1" ), source.Line, -1,
                                TokenFlag.Number ),
                            1, isLiteral: true );
                        break;
                    default:
                        throw new ExpInvalidTypeException( "Int or Real", "otherwise" );
                }
            }
            _variable = variable;
            _begin = begin;
            _end = end;
            _increment = increment;
            _operationBegin = operationBegin;
            _untilRule = untilRule;
            _whileRule = whileRule;
        }

        public override ExpNode Left => throw new NotImplementedException();

        public override ExpNode Right => _operationBegin;

        public override IExpElementSource EqualityTypeConvert => null!;

        public void AppendRepeatedOperation( ExpNode node )
        {
            _operationBegin.AppendNode( node );
        }

        /// <summary>
        ///   Create an AST node for repeat expression in EXPRESS language. The loop does not has
        ///   WHILE and UNTIL regulation.
        /// </summary>
        /// <param name="t">
        ///   Position of the loop
        /// </param>
        /// <param name="repeatVariable">
        ///
        /// </param>
        /// <param name="begin">
        ///
        /// </param>
        /// <param name="end">
        ///
        /// </param>
        /// <param name="repeatBody">
        ///
        /// </param>
        /// <returns>
        ///
        /// </returns>
        public static ExpRepeatNode Create(
                                    Token t,
                                    ExpElementInstance repeatVariable,
                                    ExpNode begin,
                                    ExpNode end,
                                    ExpNode increment,
                                    ExpNode body )
        {
            ExpNode untilRule = ExpBoolNode.False( null );
            ExpNode whileRule = ExpBoolNode.True( null );
            ExpRepeatNode ret = new ExpRepeatNode(
                t, repeatVariable, begin, end, increment, body, whileRule, untilRule )
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
                    if( {{7}} ) break;
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
            whileRule = IsNullOrEmpty( whileRule ) ? ExpBoolNode.True( t ) : whileRule;
            untilRule = IsNullOrEmpty( untilRule ) ? ExpBoolNode.False( t ) : untilRule;
            ExpRepeatNode ret = new ExpRepeatNode(
                t, repeatVariable, begin, end, increment, body, whileRule, untilRule )
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
                    if( {{7}} ) break;
                }
                """
            };
            return ret;
        }

    }
}
