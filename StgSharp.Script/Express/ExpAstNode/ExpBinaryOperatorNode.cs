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
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Script.Express
{
    public class ExpBinaryOperatorNode : ExpNode
    {

        private ExpInstantiableElement _returnType;

        private ExpNode _left;
        private ExpNode _right;

        private ExpBinaryOperatorNode( Token source, ExpNode left, ExpNode right )
            : base( source )
        {
            _left = left;
            _right = right;
        }

        public override ExpInstantiableElement EqualityTypeConvert => _returnType;

        public override ExpNode Left => _left;

        public override ExpNode Right => _right;

        public static ExpBinaryOperatorNode Add( Token source, ExpNode left, ExpNode right )
        {
            if( !left.IsNumber && !right.IsNumber ) {
                throw new ExpInvalidTypeException( "Number", left.EqualityTypeConvert.Name );
            }
            bool isFloat =
                    ( left._nodeFlag | ExpNodeFlag.BuiltinType_Real ) == 0 || ( right._nodeFlag | ExpNodeFlag.BuiltinType_Real ) ==
                    0;
            return new ExpBinaryOperatorNode( source, left, right )
            {
                _returnType = isFloat ? ExpressCompile.ExpReal : ExpressCompile.ExpInt,
                _nodeFlag = ExpNodeFlag.Operator_Binary | ExpNodeFlag.BuiltinType_Number,
                CodeConvertTemplate = "{0} + {1}",
            };
        }

        public static ExpBinaryOperatorNode Assign( Token source, ExpNode left, ExpNode right )
        {
            if( left.EqualityTypeConvert != right.EqualityTypeConvert ) {
                throw new ExpInvalidTypeException(
                    left.EqualityTypeConvert.Name, right.EqualityTypeConvert.Name );
            }
            return new ExpBinaryOperatorNode( source, left, right )
            {
                _returnType = ExpInstantiableElement.Void,
                _nodeFlag = ExpNodeFlag.Operator_Binary | left._nodeFlag,
                CodeConvertTemplate = "{0} = {1}",
            };
        }

        public static ExpBinaryOperatorNode ConvertElement( Token source, ExpNode left, ExpNode right )
        {
            if( right is not ExpMetaRefNode metaref ) {
                throw new ExpInvalidTypeException( "<source of element>", right.EqualityTypeConvert.Name );
            }
            return new ExpBinaryOperatorNode( source, left, right )
            {
                _returnType = metaref.SourceRef,
                _nodeFlag = ExpNodeFlag.Operator_Binary,
                /*
                 * 0. instance to convert
                 * 1. target type
                 */
                CodeConvertTemplate = @"({0} as {1})"
            };
        }

        public static ExpBinaryOperatorNode Div( Token source, ExpNode left, ExpNode right )
        {
            if( !left.IsNumber && !right.IsNumber ) {
                throw new ExpInvalidTypeException( "Number", left.EqualityTypeConvert.Name );
            }
            bool isFloat =
                    ( left._nodeFlag | ExpNodeFlag.BuiltinType_Real ) == 0 || ( right._nodeFlag | ExpNodeFlag.BuiltinType_Real ) ==
                    0;
            return new ExpBinaryOperatorNode( source, left, right )
            {
                _returnType = isFloat ? ExpressCompile.ExpReal : ExpressCompile.ExpInt,
                _nodeFlag = ExpNodeFlag.Operator_Binary | ExpNodeFlag.BuiltinType_Number,
                CodeConvertTemplate = "{0} / {1}",
            };
        }

        public static ExpBinaryOperatorNode EqualTo( Token source, ExpNode left, ExpNode right )
        {
            if( left.EqualityTypeConvert != right.EqualityTypeConvert ) {
                throw new ExpInvalidTypeException(
                    left.EqualityTypeConvert.Name, right.EqualityTypeConvert.Name );
            }
            return new ExpBinaryOperatorNode( source, left, right )
            {
                _returnType = ExpressCompile.ExpBool,
                _nodeFlag = ExpNodeFlag.BuiltinType_Boolean | ExpNodeFlag.Operator_Binary,
                CodeConvertTemplate = "{0} == {1}",
            };
        }

        public static ExpBinaryOperatorNode GreaterThan( Token source, ExpNode left, ExpNode right )
        {
            if( !left.IsNumber && !right.IsNumber ) {
                throw new ExpInvalidTypeException( "Number", left.EqualityTypeConvert.Name );
            }
            return new ExpBinaryOperatorNode( source, left, right )
            {
                _returnType = ExpressCompile.ExpBool,
                _nodeFlag = ExpNodeFlag.BuiltinType_Boolean | ExpNodeFlag.Operator_Binary,
                CodeConvertTemplate = "{0} > {1}",
            };
        }

        public static ExpBinaryOperatorNode LessThan( Token source, ExpNode left, ExpNode right )
        {
            if( !left.IsNumber && !right.IsNumber ) {
                throw new ExpInvalidTypeException( "Number", left.EqualityTypeConvert.Name );
            }
            return new ExpBinaryOperatorNode( source, left, right )
            {
                _returnType = ExpressCompile.ExpBool,
                _nodeFlag = ExpNodeFlag.BuiltinType_Boolean | ExpNodeFlag.Operator_Binary,
                CodeConvertTemplate = "{0} < {1}",
            };
        }

        public static ExpBinaryOperatorNode Mul( Token source, ExpNode left, ExpNode right )
        {
            if( !left.IsNumber && !right.IsNumber ) {
                throw new ExpInvalidTypeException( "Number", left.EqualityTypeConvert.Name );
            }
            bool isFloat =
                    ( left._nodeFlag | ExpNodeFlag.BuiltinType_Real ) == 0 || ( right._nodeFlag | ExpNodeFlag.BuiltinType_Real ) ==
                    0;
            return new ExpBinaryOperatorNode( source, left, right )
            {
                _returnType = isFloat ? ExpressCompile.ExpReal : ExpressCompile.ExpInt,
                _nodeFlag = ExpNodeFlag.Operator_Binary | ExpNodeFlag.BuiltinType_Number,
                CodeConvertTemplate = "{0} * {1}",
            };
        }

        public static ExpBinaryOperatorNode NotEqualTo( Token source, ExpNode left, ExpNode right )
        {
            if( left.EqualityTypeConvert != right.EqualityTypeConvert ) {
                throw new ExpInvalidTypeException(
                    left.EqualityTypeConvert.Name, right.EqualityTypeConvert.Name );
            }
            return new ExpBinaryOperatorNode( source, left, right )
            {
                _returnType = ExpressCompile.ExpBool,
                _nodeFlag = ExpNodeFlag.BuiltinType_Boolean | ExpNodeFlag.Operator_Binary,
                CodeConvertTemplate = "{0} != {1}",
            };
        }

        public static ExpBinaryOperatorNode SmallerThan( Token source, ExpNode left, ExpNode right )
        {
            if( !left.IsNumber && !right.IsNumber ) {
                throw new ExpInvalidTypeException( "Number", left.EqualityTypeConvert.Name );
            }
            return new ExpBinaryOperatorNode( source, left, right )
            {
                _returnType = ExpressCompile.ExpBool,
                _nodeFlag = ExpNodeFlag.BuiltinType_Boolean | ExpNodeFlag.Operator_Binary,
                CodeConvertTemplate = "{0} < {1}",
            };
        }

        public static ExpBinaryOperatorNode Sub( Token source, ExpNode left, ExpNode right )
        {
            if( !left.IsNumber && !right.IsNumber ) {
                throw new ExpInvalidTypeException( "Number", left.EqualityTypeConvert.Name );
            }
            bool isFloat =
                    ( left._nodeFlag | ExpNodeFlag.BuiltinType_Real ) == 0 || ( right._nodeFlag | ExpNodeFlag.BuiltinType_Real ) ==
                    0;
            return new ExpBinaryOperatorNode( source, left, right )
            {
                _returnType = isFloat ? ExpressCompile.ExpReal : ExpressCompile.ExpInt,
                _nodeFlag = ExpNodeFlag.Operator_Binary | ExpNodeFlag.BuiltinType_Number,
                CodeConvertTemplate = "{0} - {1}",
            };
        }

    }
}
