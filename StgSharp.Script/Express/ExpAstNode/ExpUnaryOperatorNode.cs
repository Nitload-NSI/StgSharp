﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpUnaryOperatorNode.cs"
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
    public class ExpUnaryOperatorNode : ExpNode
    {

        private ExpNode _operand;

        private ExpUnaryOperatorNode( Token source ) : base( source ) { }

        public ExpUnaryOperatorNode(
                       Token source,
                       ExpNode operand,
                       ExpSchema context )
            : base( source )
        {
            _operand = operand;
        }

        public override ExpNode Left => null!;

        public override ExpNode Right => _operand;

        public override IExpElementSource EqualityTypeConvert => _operand.EqualityTypeConvert;

        public static ExpUnaryOperatorNode UnaryMinus(
                                                   Token source,
                                                   ExpNode operand )
        {
            if( !operand.IsNumber ) {
                throw new ExpInvalidTypeException(
                    "Number", operand.EqualityTypeConvert.Name );
            }
            bool isFloat =
                    ( operand._nodeFlag | ExpNodeFlag.BuiltinType_Real ) == 0;
            return new ExpUnaryOperatorNode( source )
            {
                _nodeFlag = ExpNodeFlag.Operator_Unary | ExpNodeFlag.BuiltinType_Number,
                CodeConvertTemplate = "- {0}",
            };
        }

        public static ExpUnaryOperatorNode UnaryNot(
                                                   Token source,
                                                   ExpNode operand )
        {
            if( ( operand._nodeFlag | ( ExpNodeFlag.BuiltinType_Logic | ExpNodeFlag.BuiltinType_Logic ) ) ==
                0 ) {
                throw new ExpInvalidTypeException(
                    "Bool or Logic", operand.EqualityTypeConvert.Name );
            }
            return new ExpUnaryOperatorNode( source )
            {
                _nodeFlag = ExpNodeFlag.Operator_Unary | ExpNodeFlag.BuiltinType_Logic,
                CodeConvertTemplate = "! {0}",
            };
        }

        public static ExpUnaryOperatorNode UnaryPlus(
                                                   Token source,
                                                   ExpNode operand )
        {
            if( !operand.IsNumber ) {
                throw new ExpInvalidTypeException(
                    "Number", operand.EqualityTypeConvert.Name );
            }
            bool isFloat =
                    ( operand._nodeFlag | ExpNodeFlag.BuiltinType_Real ) == 0;
            return new ExpUnaryOperatorNode( source )
            {
                _nodeFlag = ExpNodeFlag.Operator_Unary | ExpNodeFlag.BuiltinType_Number,
                CodeConvertTemplate = "+ {0}",
            };
        }

    }
}
