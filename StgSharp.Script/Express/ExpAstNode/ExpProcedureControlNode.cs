//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ExpProcedureControlNode"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExpKeyword = StgSharp.Script.Express.ExpressCompile.Keyword;

namespace StgSharp.Script.Express
{
    public class ExpProcedureControlNode : ExpSyntaxNode
    {

        private ExpSyntaxNode _additional;

        internal ExpProcedureControlNode(Token source) : base(source) { }

        public override ExpSyntaxNode Left => Empty;

        public override ExpSyntaxNode Right => _additional;

        public override IExpElementSource EqualityTypeConvert => ExpTypeSource.Void;

        public static ExpProcedureControlNode Return(Token t, ExpSyntaxNode valueToReturn)
        {
            if (t.Value != ExpKeyword.Return) {
                throw new InvalidCastException();
            }
            ExpProcedureControlNode ret = new ExpProcedureControlNode(t)
            {
                _additional = valueToReturn ?? Empty,
                CodeConvertTemplate = IsNullOrEmpty(valueToReturn!) ? ExpKeyword.Return : "return {0}",
            };
            return ret;
        }

        public static ExpProcedureControlNode Skip(Token t)
        {
            ExpProcedureControlNode ret = new ExpProcedureControlNode(t)
            {
                _additional = Empty,
                CodeConvertTemplate = "break"
            };
            return ret;
        }

    }
}
