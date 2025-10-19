//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ExpSyntaxAnalyzer.ReverseAnalysing"
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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExpKeyword = StgSharp.Script.Express.ExpressCompile.Keyword;

namespace StgSharp.Script.Express
{
    public partial class ExpSyntaxAnalyzer
    {

        private void ClosePrefixReverse(Token rightSeparator)
        {
            ExpSyntaxNode root;
            ReverseAnalyzeStatement();
            root = _cache.PackAllStatements();
            if (rightSeparator.Value == ")") {
                root = ExpTupleNode.Pack(root);
            }
            _cache.DecreaseDepth();
            if (IsSeparatorMatch(_cache.PeekOperator(), rightSeparator))
            {
                _cache.PopOperator();
                _cache.PushOperand(root);
            } else
            {
                throw new ExpCompileException(
                    rightSeparator, "Ending of block does not match its begging");
            }
        }

        private void ReverseAnalyzeStatement()
        {
            ExpSyntaxNode root;
            if (_cache.OperatorAheadOfDepth == 0)
            {
                switch (_cache.OperandAheadOfDepth)
                {
                    case 1:
                        _cache.PopOperand(out _, out ExpSyntaxNode? statement);
                        _cache.StatementsInDepth.Push(statement);
                        break;
                    case 0:
                        _cache.StatementsInDepth.Push(ExpSyntaxNode.Empty);
                        break;
                    default:
                        throw new ExpInvalidSyntaxException(
                            "More than one operand but no operator provided.");
                }
            } else
            {
                while (_cache.TryPopOperator(out Token op)) {
                    ConvertAndPushOneOperator(op);
                }
                _cache.PopOperand(out _, out root);
                _cache.StatementsInDepth.Push(root);
            }
        }

    }
}
