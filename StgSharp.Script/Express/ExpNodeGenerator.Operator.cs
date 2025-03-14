﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpNodeGenerator.Operator.cs"
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

namespace StgSharp.Script.Express
{
    public partial class ExpNodeGenerator
    {

        public int CompareOperatorPrecedence( Token left, Token right )
        {
            ExpCompile.TryGetOperatorPrecedence(
                left.Value, out int leftPrecedence );
            ExpCompile.TryGetOperatorPrecedence(
                right.Value, out int rightPrecedence );
            return leftPrecedence - rightPrecedence;
        }

        public bool TryGenerateBinaryNode( Token t, out ExpNode node )
        {
            ExpNode left = GetNextOperandCache();
            ExpNode right = GetNextOperandCache();
            switch( t.Value ) {
                case ExpCompile.KeyWord.Add:
                    node = ExpBinaryOperatorNode.Add( t, left, right );
                    return true;
                case ExpCompile.KeyWord.Sub:
                    node = ExpBinaryOperatorNode.Sub( t, left, right );
                    return true;
                case ExpCompile.KeyWord.Mul:
                    node = ExpBinaryOperatorNode.Mul( t, left, right );
                    return true;
                case ExpCompile.KeyWord.Div:
                    node = ExpBinaryOperatorNode.Div( t, left, right );
                    return true;
                case ExpCompile.KeyWord.Equal:
                    node = ExpBinaryOperatorNode.EqualTo( t, left, right );
                    return true;
                case ExpCompile.KeyWord.NotEqual:
                    node = ExpBinaryOperatorNode.NotEqualTo( t, left, right );
                    return true;

                default:
                    node = ExpNode.Empty;
                    return false;
            }
        }

        public bool TryGenerateUnaryNode( Token t, out ExpNode node )
        {
            ExpNode operand = GetNextOperandCache();
            switch( t.Value ) {
                case ExpCompile.KeyWord.UnaryPlus:
                    node = ExpUnaryOperatorNode.UnaryPlus( t, operand );
                    return true;
                case ExpCompile.KeyWord.UnaryMinus:
                    node = ExpUnaryOperatorNode.UnaryMinus( t, operand );
                    return true;
                case ExpCompile.KeyWord.Not:
                    node = ExpUnaryOperatorNode.UnaryNot( t, operand );
                    return true;
                default:
                    node = ExpNode.Empty;
                    return false;
            }
        }

    }
}
