//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="KeyWord.cs"
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Script.Express
{
    public static partial class ExpCompile
    {

        private static void PoolKeywords()
        {
            FieldInfo[] fields = typeof( KeyWord ).GetFields(
                BindingFlags.Public );
            ParallelLoopResult result = Parallel.ForEach(
                fields, static( field ) => {
                    if( field.FieldType == typeof( string ) && field.GetValue(
                        null ) is string str ) {
                        _compileStringCache.GetOrAdd( str );
                    }
                } );
            while( !result.IsCompleted ) { }
        }

        public static class KeyWord
        {

            public const string Int = "INTAGER",
                Real = "REAL",
                Bool = "BOOLEAN",
                String = "STRING",
                Logic = "LOGIC",
                Binary = "BINARY",
                Constant = "CONSTANT",
                AND = "AND",
                OR = "OR",
                XOR = "XOR",
                ADD = "+",
                SUB = "-",
                MUL = "*",
                DIV = "/",
                NOT = "NOT",
                Assignment = ":=",
                Equal = "=",
                Mod = "MOD",
                Self = "SELF",
                Pi = "PI",
                True = "TRUE",
                False = "FALSE",
                Insert = "INSERT",
                Remove = "REMOVE",
                ANDOR = "ANDOR",
                GetIndex = "[",
                UnaryAdd = "+",
                UnarySub = "-",
                Exp = "**";

        }

    }
}
