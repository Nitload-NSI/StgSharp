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
    public class ExpIntNode : ExpElementInstanceBase
    {

        private int _value;

        public ExpIntNode( Token source, int value )
            : base( source )
        {
            _value = value;
            _nodeFlag = ExpNodeFlag.BuiltinType_Int | ExpNodeFlag.Element_Type;
        }

        public override IExpElementSource EqualityTypeConvert => ExpSchema.BuiltinSchema
                        .TryGetType(
                            ExpCompile.KeyWord.Int, out ExpTypeSource? type ) ?
                type : throw new ExpCompileNotInitializedException();

        public int Value => _value;

        public override string TypeName => ExpCompile.KeyWord.Int;

    }

    public class ExpStringNode : ExpElementInstanceBase
    {

        private readonly string _value;

        public ExpStringNode( Token source, string value )
            : base( source )
        {
            _value = value;
            _nodeFlag = ExpNodeFlag.BuiltinType_String | ExpNodeFlag.Element_Type;
        }

        public override IExpElementSource EqualityTypeConvert => ExpSchema.BuiltinSchema
                        .TryGetType(
                            ExpCompile.KeyWord.String,
                            out ExpTypeSource? type ) ?
                type : throw new ExpCompileNotInitializedException();

        public override string TypeName => EqualityTypeConvert.Name;

        public string Value => _value;

    }

    public class ExpBoolNode : ExpElementInstanceBase
    {

        private readonly bool _value;

        public ExpBoolNode( Token source, bool value )
            : base( source )
        {
            _value = value;
            _nodeFlag = ExpNodeFlag.BuiltinType_Boolean | ExpNodeFlag.Element_Type;
        }

        public bool Value => _value;

        public override IExpElementSource EqualityTypeConvert => ExpSchema.BuiltinSchema
                        .TryGetType(
                            ExpCompile.KeyWord.Bool, out ExpTypeSource? type ) ?
                type : throw new ExpCompileNotInitializedException();

        public override string TypeName => EqualityTypeConvert.Name;

    }

    public class ExpRealNumberNode : ExpElementInstanceBase
    {

        private readonly float _value;

        public ExpRealNumberNode( Token source, float value )
            : base( source )
        {
            _value = value;
            _nodeFlag = ExpNodeFlag.BuiltinType_Real | ExpNodeFlag.Element_Type;
        }

        public float Value => _value;

        public override IExpElementSource EqualityTypeConvert => ExpSchema.BuiltinSchema
                        .TryGetType(
                            ExpCompile.KeyWord.String,
                            out ExpTypeSource? type ) ?
                type : throw new ExpCompileNotInitializedException();

        public override string TypeName => EqualityTypeConvert.Name;

    }

    public class ExpLogicValueNode : ExpElementInstanceBase
    {

        private readonly ExpLogic _value;

        public ExpLogicValueNode( Token source, ExpLogic value )
            : base( source )
        {
            _value = value;
            _nodeFlag = ExpNodeFlag.BuiltinType_Logic | ExpNodeFlag.Element_Type;
        }

        public ExpLogic Value => _value;

        public override IExpElementSource EqualityTypeConvert => ExpSchema.BuiltinSchema
                        .TryGetType(
                            ExpCompile.KeyWord.Logic,
                            out ExpTypeSource? type ) ?
                type : throw new ExpCompileNotInitializedException();

        public override string TypeName => EqualityTypeConvert.Name;

    }
}
