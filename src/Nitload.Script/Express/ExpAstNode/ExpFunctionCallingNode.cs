//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ExpFunctionCallingNode"
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Script.Express
{
    public class ExpFunctionCallingNode : ExpSyntaxNode
    {

        private ExpFunctionSource _function;

        private ExpSyntaxNode _parameter;

        private ExpFunctionCallingNode(Token source, ExpFunctionSource function, ExpSyntaxNode parameterEnumeration)
            : base(source)
        {
            if (parameterEnumeration is ExpTupleNode tuple) {
                parameterEnumeration = tuple.Right;
            }
            _function = function;
            _parameter = parameterEnumeration;
        }

        public ExpFunctionSource FunctionCaller => _function;

        public override ExpInstantiableElement EqualityTypeConvert => _function.ReturningType;

        public override ExpSyntaxNode Left => Empty;

        public override ExpSyntaxNode Right => _parameter;

        public void AppendParameter(ExpSyntaxNode parameter)
        {
            _parameter.AppendNode(parameter);
        }

        public static ExpFunctionCallingNode CallFunction(Token source, ExpFunctionSource function, ExpSyntaxNode param)
        {
            if (param is ExpTupleNode) {
                param = param.Right;
            }
            return new ExpFunctionCallingNode(source, function, param);
        }

    }
}
