//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="RegexInterpreter"
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
using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.TextRegex
{
    public class RegexInterpreter
    {

        private AbstractSyntaxTree<RegexAstNode, RegexElementLabel> _tree = new();
        private CompileStack<RegexAstNode, RegexElementLabel> _stack = new();
        private Exception _interpreteException;
        private string _source;

        private RegexInterpreter() { }

        public static RegexInterpreter Analyze(string regex)
        {
            RegexInterpreter interpreter = new RegexInterpreter();
            interpreter._source = regex;
            bool isSuccess;
            try
            {
                interpreter.Analyze();
                interpreter.OptimizeTree();
                isSuccess = true;
            }
            catch (Exception ex)
            {
                interpreter._interpreteException = ex;
                isSuccess = false;
            }
            return interpreter;
        }

        private void Analyze()
        {
            RegexTokenReader reader = new RegexTokenReader(_source);
            TokenParser<RegexElementLabel, RegexElementLabel> lexer = reader.Pipe(() => new RegexTokenParser());

            while (lexer.TryReadToken(out Token<RegexElementLabel> token))
            {
                if ((token.Flag & RegexElementLabel.SEQUENCE) != 0)
                {
                    // char or charset
                    Console.WriteLine($"Unit {token.Value}");
                    _stack.PushOperand(new RegexAstNode(token));
                } else if ((token.Flag & RegexElementLabel.GROUP_BEGIN) != 0)
                {
                    Console.WriteLine($"Group begin {token.Value}");
                    _stack.PushOperator(new RegexAstNode(token));
                    _stack.IncreaseDepth(0);
                    Console.WriteLine(/**/_stack.OperatorInDepthCount/**/);
                } else if ((token.Flag & RegexElementLabel.GROUP_END) != 0)
                {
                    // close all nodes here
                    Console.WriteLine($"Group end {token.Value}");
                    RegexAstNode op;
                    while (_stack.TryPopOperator(out op)) {
                        ProcessOperator(op);
                    }
                    if (_stack.OperandInDepthCount != 1) {
                        throw new InvalidOperationException();
                    }
                    _stack.DecreaseDepth();
                    op = _stack.PopOperator();
                    if (op.Source.Flag != RegexElementLabel.GROUP_BEGIN) {
                        throw new InvalidOperationException("Unmatched group begin and end symbol");
                    }
                    op.Right = _stack.PopOperand();
                    _stack.PushOperand(op);
                } else if ((token.Flag & RegexElementLabel.OPERATOR) != 0)
                {
                    Console.Write($"Operator {token.Value} with flag {token.Flag},  ");
                    Console.WriteLine(/**/_stack.OperatorInDepthCount/**/);
                    if (_stack.OperatorInDepthCount == 0)
                    {
                        // the first operator in stack
                        _stack.PushOperator(new RegexAstNode(token));
                    } else
                    {
                        // process operator by precedence
                        while (_stack.TryPeekOperator(out RegexAstNode topOp))
                        {
                            (bool Valid, int Value) cmp = RegexAstNode.ComparePrecedence(token.Flag, topOp.Source.Flag);

                            if (!cmp.Valid) {
                                throw new InvalidOperationException("Operators with same precedence are not supported.");
                            }
                            if (cmp.Value < 0)
                            {
                                _stack.PopOperator();
                                ProcessOperator(topOp);
                                continue;
                            } else
                            {
                                break;
                            }
                        }
                        _stack.PushOperator(new RegexAstNode(token));
                    }
                }
            }
            Console.WriteLine(1);
            if (_stack.Depth == 1)
            {
                if (_stack.OperandInDepthCount != 1 || _stack.OperatorInDepthCount != 0)
                {
                    while (_stack.TryPopOperator(out RegexAstNode? op)) {
                        ProcessOperator(op);
                    }
                }
                if (_stack.OperandInDepthCount == 1 && _stack.OperatorInDepthCount == 0)
                {
                    _tree.Root = _stack.PopOperand();
                    return;
                }
                throw new InvalidOperationException("Invalid regular expression syntax.");
            }
            throw new InvalidOperationException("Invalid regular expression syntax.");
        }

        private void OptimizeTree(
        )
        {
            // phase0: scan container and remove no parent nodes
            _tree.AllNodes.RemoveWhere(node => node.Parent == null && node != _tree.Root);

            // phase1: ALT flatten
            int opCount = 0, cycle = 0;
            do
            {
                cycle++;
            } while (opCount != 0 && cycle < 16);

            // phase2: ALT merge
            opCount = 0;
            cycle = 0;
            do
            {
                opCount = 1;
            } while (opCount != 0);

            // phase3: CONCAT merge
        }

        private void ProcessOperator(RegexAstNode op)
        {
            Console.WriteLine($"Processing a {op.Source.Flag} operator");
            if (op.EqualityTypeConvert == RegexElementLabel.CONCAT)
            {
                if (_stack.TryPopOperand(out RegexAstNode? _1) && _stack.TryPopOperand(out RegexAstNode? _2))
                {
                    op.Left = _2;
                    op.Right = _1;
                    _stack.PushOperand(op);
                    _tree.AddNode(op);
                    _tree.AddNode(_1);
                    _tree.AddNode(_2);
                } else
                {
                    throw new InvalidOperationException("Insufficient operands for CONCAT operator.");
                }
            } else if (op.EqualityTypeConvert == RegexElementLabel.COUNT)
            {
                if (_stack.TryPopOperand(out RegexAstNode? _1))
                {
                    op.Left = _1;
                    _stack.PushOperand(op);
                    _tree.AddNode(_1);
                    _tree.AddNode(op);
                } else
                {
                    throw new InvalidOperationException("Insufficient operands for CONCAT operator.");
                }
            } else if (op.EqualityTypeConvert == RegexElementLabel.ALT)
            {
                if (_stack.TryPopOperand(out RegexAstNode? _1) && _stack.TryPopOperand(out RegexAstNode? _2))
                {
                    op.Left = _1;
                    op.Right = _2;
                    _stack.PushOperand(op);
                    _tree.AddNode(op);
                    _tree.AddNode(_1);
                    _tree.AddNode(_2);
                } else
                {
                    throw new InvalidOperationException("Insufficient operands for CONCAT operator.");
                }
            }
        }

    }
}
