//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpLiteralNode.cs"
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
using System.Text.RegularExpressions;

namespace StgSharp.Script.Express
{
    public class ExpIntNode : ExpElementInstance
    {

        private int _value;

        public ExpIntNode( Token source, int value, bool isLiteral )
            : base( source )
        {
            this.IsLiteral = isLiteral;
            _value = value;
            _nodeFlag = ExpNodeFlag.BuiltinType_Int | ExpNodeFlag.Element_Type;
            CodeConvertTemplate = IsLiteral ? value.ToString() : source.Value;
        }

        public override IExpElementSource EqualityTypeConvert
        {
            get => ExpressCompile.ExpInt;
        }

        public int Value => _value;

        public override string TypeName => ExpressCompile.Keyword.Integer;

    }

    public class ExpStringNode : ExpElementInstance
    {

        private readonly string _value;

        public ExpStringNode( Token source, string value, bool isLiteral )
            : base( source )
        {
            this.IsLiteral = IsLiteral;
            _value = value;
            _nodeFlag = ExpNodeFlag.BuiltinType_String | ExpNodeFlag.Element_Type;
            CodeConvertTemplate = IsLiteral ? $"\"{value}\"" : source.Value;
        }

        public override IExpElementSource EqualityTypeConvert
        {
            get => ExpressCompile.ExpString;
        }

        public override string TypeName => EqualityTypeConvert.Name;

        public string Value => _value;

    }

    public class ExpBoolNode : ExpElementInstance
    {

        private readonly bool _value;

        public ExpBoolNode( Token source, bool value, bool isLiteral )
            : base( source )
        {
            this.IsLiteral = isLiteral;
            _value = value;
            _nodeFlag = ExpNodeFlag.BuiltinType_Boolean | ExpNodeFlag.Element_Type;
            CodeConvertTemplate = IsLiteral ? value.ToString() : source.Value;
        }

        public bool Value => _value;

        public override IExpElementSource EqualityTypeConvert
        {
            get => ExpSchema.BuiltinSchema
                            .TryGetType( ExpressCompile.Keyword.Boolean, out ExpTypeSource? type ) ?
                    type : throw new ExpCompileNotInitializedException();
        }

        public override string TypeName => EqualityTypeConvert.Name;

        public static ExpBoolNode False( Token? t )
        {
            return new( t ?? new Token( "FALSE", 0, -1, TokenFlag.Member ), false,
                        isLiteral: true );
        }

        public static ExpBoolNode True( Token? t )
        {
            return new( t ?? new Token( "TRUE", 0, -1, TokenFlag.Member ), true, isLiteral: true );
        }

    }

    public class ExpRealNumberNode : ExpElementInstance
    {

        private readonly float _value;

        public ExpRealNumberNode( Token source, float value, bool isLiteral )
            : base( source )
        {
            _value = value;
            _nodeFlag = ExpNodeFlag.BuiltinType_Real | ExpNodeFlag.Element_Type;
            CodeConvertTemplate = isLiteral ? $"{value:F8}f" : source.Value;
        }

        public float Value => _value;

        public override IExpElementSource EqualityTypeConvert
        {
            get => ExpSchema.BuiltinSchema
                            .TryGetType( ExpressCompile.Keyword.String, out ExpTypeSource? type ) ?
                    type : throw new ExpCompileNotInitializedException();
        }

        public override string TypeName => EqualityTypeConvert.Name;

    }

    public class ExpLogicValueNode : ExpElementInstance
    {

        private readonly ExpLogic _value;

        public ExpLogicValueNode( Token source, ExpLogic value, bool isLiteral )
            : base( source )
        {
            this.IsLiteral = isLiteral;
            _value = value;
            _nodeFlag = ExpNodeFlag.BuiltinType_Logic | ExpNodeFlag.Element_Type;
            CodeConvertTemplate = isLiteral ? value.ToString() : source.Value;
        }

        public ExpLogic Value => _value;

        public override IExpElementSource EqualityTypeConvert
        {
            get => ExpSchema.BuiltinSchema
                            .TryGetType( ExpressCompile.Keyword.Logical, out ExpTypeSource? type ) ?
                    type : throw new ExpCompileNotInitializedException();
        }

        public override string TypeName => EqualityTypeConvert.Name;

    }
}
