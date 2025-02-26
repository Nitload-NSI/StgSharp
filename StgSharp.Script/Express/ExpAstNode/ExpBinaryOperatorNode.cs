//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpBinaryOperatorNode.cs"
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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Script.Express
{
    public class ExpBinaryOperatorNode : ExpBaseNode
    {

        private ExpBaseNode _left;
        private ExpBaseNode _right;
        private ExpTypeSource _returnType;

        public ExpBinaryOperatorNode( string name ) : base( name, null! ) { }

        public ExpBinaryOperatorNode(
                       string source,
                       ExpSchema context,
                       ExpBaseNode left,
                       ExpBaseNode right )
            : base( source, context )
        {
            if( !left.EqualityTypeConvert
                    .IsConvertable( right.EqualityTypeConvert ) ) {
                throw new InvalidCastException(
                    $"Cannot convert element {left.EqualityTypeConvert} to {right.EqualityTypeConvert}" );
            }
            _left = left;
            _right = right;
        }

        public override ExpBaseNode Left => _left;

        public override ExpBaseNode Right => _right;

        public override IExpElementSource EqualityTypeConvert => _returnType;

        public static ExpBinaryOperatorNode Add(
                                                    ExpBaseNode left,
                                                    ExpBaseNode right )
        {
            if( !left.IsNumber && !right.IsNumber ) {
                throw new ExpInvalidTypeException(
                    "Number", left.EqualityTypeConvert.Name );
            }
            bool isFloat =
                    ( left._nodeFlag | ExpNodeFlag.BuiltinType_Float ) == 0 || ( right._nodeFlag | ExpNodeFlag.BuiltinType_Float ) ==
                    0;
            return new ExpBinaryOperatorNode( ExpCompile.KeyWord.ADD )
            {
                _returnType = isFloat ? null : null,
                _nodeFlag = ExpNodeFlag.Operator_Binary | ExpNodeFlag.BuiltinType_Number,
                CodeConvertTemplate = "{0} + {1}",
            };
        }

        public static ExpBinaryOperatorNode Div(
                                                    ExpBaseNode left,
                                                    ExpBaseNode right )
        {
            if( !left.IsNumber && !right.IsNumber ) {
                throw new ExpInvalidTypeException(
                    "Number", left.EqualityTypeConvert.Name );
            }
            bool isFloat =
                    ( left._nodeFlag | ExpNodeFlag.BuiltinType_Float ) == 0 || ( right._nodeFlag | ExpNodeFlag.BuiltinType_Float ) ==
                    0;
            return new ExpBinaryOperatorNode( ExpCompile.KeyWord.DIV )
            {
                _returnType = isFloat ? null : null,
                _nodeFlag = ExpNodeFlag.Operator_Binary | ExpNodeFlag.BuiltinType_Number,
                CodeConvertTemplate = "{0} / {1}",
            };
        }

        public static ExpBinaryOperatorNode Mul(
                                                    ExpBaseNode left,
                                                    ExpBaseNode right )
        {
            if( !left.IsNumber && !right.IsNumber ) {
                throw new ExpInvalidTypeException(
                    "Number", left.EqualityTypeConvert.Name );
            }
            bool isFloat =
                    ( left._nodeFlag | ExpNodeFlag.BuiltinType_Float ) == 0 || ( right._nodeFlag | ExpNodeFlag.BuiltinType_Float ) ==
                    0;
            return new ExpBinaryOperatorNode( ExpCompile.KeyWord.MUL )
            {
                _returnType = isFloat ? null : null,
                _nodeFlag = ExpNodeFlag.Operator_Binary | ExpNodeFlag.BuiltinType_Number,
                CodeConvertTemplate = "{0} * {1}",
            };
        }

        public static ExpBinaryOperatorNode Sub(
                                                    ExpBaseNode left,
                                                    ExpBaseNode right )
        {
            if( !left.IsNumber && !right.IsNumber ) {
                throw new ExpInvalidTypeException(
                    "Number", left.EqualityTypeConvert.Name );
            }
            bool isFloat =
                    ( left._nodeFlag | ExpNodeFlag.BuiltinType_Float ) == 0 || ( right._nodeFlag | ExpNodeFlag.BuiltinType_Float ) ==
                    0;
            return new ExpBinaryOperatorNode( ExpCompile.KeyWord.SUB )
            {
                _returnType = isFloat ? null : null,
                _nodeFlag = ExpNodeFlag.Operator_Binary | ExpNodeFlag.BuiltinType_Number,
                CodeConvertTemplate = "{0} - {1}",
            };
        }

    }
}
