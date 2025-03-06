//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpNodeFlag.cs"
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
    [Flags]
    public enum ExpNodeFlag : long
    {

        //string as name of non-builtin variables
        None = 0,
        Name_Element = 1,
        Name_Param = 2,
        Name_Block = 4,
        Name_Keyword = 8,
        Name_Operator = 16,
        Name_Any = Name_Block | Name_Element | Name_Keyword | Name_Param | Name_Operator,

        //any type of collections, also has NAME flag
        Collection_Set = 32,
        Collection_Bag = 64,
        Collection_Array = 128,
        Collection_List = 256,
        Collection_Any = Collection_Set | Collection_Bag | Collection_Array | Collection_List,

        //the variable is type of element, both builtin and non-builtin has this flag
        Element_Const = 512,
        Element_Entity = 1024,
        Element_Type = 2048,
        Element_Rule = 4096,
        Element_Function = 8192,
        Element_Any = Element_Entity | Element_Const | Element_Function | Element_Rule | Element_Type,

        //variable is one of these builtin type
        BuiltinType_Int = 16384,
        BuiltinType_Float = 32768,
        BuiltinType_String = 65536,
        BuiltinType_Boolean = 0x2000,
        BuiltinType_Binary = 0x4000,
        BuiltinType_Logic = 0x8000,
        BuiltinType_Number = BuiltinType_Float | BuiltinType_Int,
        BuiltinType_Any = BuiltinType_Int | BuiltinType_Float | BuiltinType_String | BuiltinType_Boolean | BuiltinType_Binary | BuiltinType_Logic,

        //the token is root of a branch
        Branch_If = 0x1_000,
        Branch_Switch = 0x2_0000,
        Branch_Repeat = 0x4_0000,
        Branch_Any = Branch_If | Branch_Repeat | Branch_Switch,

        //the token is one of separators
        Separator_Inline = 0x8_0000,        //, or sth else
        Separator_Line = 0x10_0000,         //; or eth else
        Separator_Block = 0x20_0000,        //{}or sth else
        Separator_Range = 0x40_0000,        //()or sth else
        Separator_Any = Separator_Inline | Separator_Line | Separator_Block | Separator_Range,

        //the token is a kind of operator
        Operator_Unary = 0x40_0000,
        Operator_Binary = 0x80_0000,
        Operator_SuperType = 0x100_0000,
        Operator_Any = Operator_Unary | Operator_Binary | Operator_SuperType,

    }
}
