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
            if( IsNullOrEmpty( increment ) )
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
            _increment = increment!;
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
        ///   Position of the loop.
        /// </param>
        /// <param name="repeatVariable">
        ///   The variable recording increment by step.
        /// </param>
        /// <param name="begin">
        ///   Beginning value of the loop.
        /// </param>
        /// <param name="end">
        ///   When <see href="repeatVariable" /> reach the value, the loop ends.
        /// </param>
        /// <param name="repeatBody">
        ///   Expressions to operate multiple times.
        /// </param>
        /// <returns>
        ///   A <see cref="ExpRepeatNode" /> representing the repeat expression.
        /// </returns>
        public static ExpRepeatNode Create(
                                    Token t,
                                    ExpInstanceReferenceNode repeatVariable,
                                    ExpNode begin,
                                    ExpNode end,
                                    ExpNode increment,
                                    ExpNode whileRule,
                                    ExpNode untilRule,
                                    ExpNode body )
        {
            untilRule = ExpBoolNode.False( null );
            whileRule = ExpBoolNode.True( null );
            ExpRepeatNode ret = new ExpRepeatNode(
                t, repeatVariable.OriginObject, begin, end, increment, body, whileRule, untilRule )
            {
                /*
                 * 0: variable name
                 * 1: beginning value
                 * 2: ending value
                 * 3: increment
                 * 4: body
                 * 5: while
                 * 6: until
                 */

                CodeConvertTemplate = $$"""
                for( {0} = {1}; {0} <= {2}; {0} += {3})
                {
                    if(!( {5} )) break;
                    {4}
                    if( {6} ) break;
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
                 * 0: variable name
                 * 1: beginning value
                 * 2: ending value
                 * 3: increment
                 * 4: body
                 * 5: while
                 * 6: until
                 */

                CodeConvertTemplate = $$"""
                for({{repeatVariable.TypeName}} {0} = {1}; {0} <= {2}; {0} += {3})
                {
                    if(!( {5} )) break;
                    {4}
                    if( {6} ) break;
                }
                """
            };
            return ret;
        }

    }
}
