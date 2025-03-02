//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpLiteralGenericNode.cs"
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
using StgSharp.Data;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Script.Express
{
    public class ExpLiteralNode
    {

        private readonly IValueBox box;

    }

    public class ExpLiteralGenericNode<T> : ExpElementInstanceBase
    {

        private readonly ExpBuiltinType _type;
        private readonly T _value;

        public ExpLiteralGenericNode( string name, ExpSchema context, T value )
            : base( name )
        {
            _value = value;
            switch( value ) {
                case float f:
                    ExpSchema.BuiltinSchema
                            .TryGetType(
                                ExpCompile.KeyWord.Real,
                                out ExpTypeSource? type );
                    _type = ( type as ExpBuiltinType )!;
                    _nodeFlag = ExpNodeFlag.BuiltinType_Float | ExpNodeFlag.Element_Const;
                    break;
                case int i:
                    ExpSchema.BuiltinSchema
                            .TryGetType( ExpCompile.KeyWord.Int, out type );
                    _type = ( type as ExpBuiltinType )!;
                    _nodeFlag = ExpNodeFlag.BuiltinType_Int | ExpNodeFlag.Element_Const;
                    break;
                case BitArray b:
                    ExpSchema.BuiltinSchema
                            .TryGetType( ExpCompile.KeyWord.Binary, out type );
                    _type = ( type as ExpBuiltinType )!;
                    _nodeFlag = ExpNodeFlag.BuiltinType_Binary | ExpNodeFlag.Element_Const;
                    break;
                case string s:
                    ExpSchema.BuiltinSchema
                            .TryGetType( ExpCompile.KeyWord.String, out type );
                    _type = ( type as ExpBuiltinType )!;
                    _nodeFlag = ExpNodeFlag.BuiltinType_Int | ExpNodeFlag.Element_Const;
                    break;
                case ExpLogic l:
                    ExpSchema.BuiltinSchema
                            .TryGetType( ExpCompile.KeyWord.Logic, out type );
                    _type = ( type as ExpBuiltinType )!;
                    _nodeFlag = ExpNodeFlag.BuiltinType_Int | ExpNodeFlag.Element_Const;
                    break;
                default:
                    throw new ExpInvalidTypeException(
                        "Builtin Type", value!.GetType().Name );
            }
        }

        public override ExpNode Left => Empty;

        public override ExpNode Right => throw new NotImplementedException();

        public override IExpElementSource EqualityTypeConvert => _type;

        public override string TypeName => _type.Name;

        public T value => _value;

    }
}
