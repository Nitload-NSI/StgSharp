//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpToken.cs"
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
using System.ComponentModel.Design;
using System.Text;

namespace StgSharp.Script.Expression
{
    [Flags]
    public enum ExpTokenFlag : long
    {

        None = 0,
        Name_Element = 1,
        Name_Param = 2,
        Name_Block=4,
        Name_Keyword=8,
        Name_Any = Name_Block | Name_Element | Name_Keyword | Name_Param,
        Collection_Set=16,
        Collection_Bag=32,
        Collection_Array=64,
        Collection_List=128,
        Collection_Any = Collection_Set | Collection_Bag | Collection_Array | Collection_List,
        Element_Const=256,
        Element_Entity=512,
        Element_type=1024,
        Element_Rule=2048,
        Element_Function=4096,
        Element_Any = Element_Entity | Element_Const | Element_Function | Element_Rule | Element_type,
        BuiltinType_Int=8192,
        BuiltinType_Float=16384,
        BuiltinType_String=32768,
        BuiltinType_Boolean=65536,
        BuiltinType_Binary=0x2000,
        BuiltinType_Logic=0x4000,
        BuiltinType_Any = BuiltinType_Int | BuiltinType_Float | BuiltinType_String | BuiltinType_Boolean | BuiltinType_Binary | BuiltinType_Logic,

    }

    public abstract class ExpToken : IExpressionToken
    {

        protected internal ExpToken _previousToken;
        protected internal ExpTokenFlag _tokenType;
        protected internal List<ExpToken> _nextLevelTokens = new List<ExpToken>(
            );
        protected internal string _name;

        protected ExpToken( string name, ExpSchema context )
        {
            _name = name;
            Context = context;
        }

        public abstract object? Value
        {
            get;
        }

        public ICollection<IExpressionToken> NextLevel => ( _nextLevelTokens as ICollection<IExpressionToken> )!;

        public int TokenTypeCode => ( int )_tokenType;

        public string Name => _name;

        protected ExpSchema Context
        {
            get;
            private set;
        }

    }
}
