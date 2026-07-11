// -----------------------------------------------------------------------------
// file="RegexAnalyzer.ASTGenerate"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Text
{
    public partial class RegexAnalyzer
    {

        private static AbstractSyntaxTree<RegexAstNode, RegexElementLabel> GenerateAST(
                                                                           string _source
        )
        {
            CompileStack<RegexAstNode, RegexElementLabel> _stack = new();
            AbstractSyntaxTree<RegexAstNode, RegexElementLabel> _tree = new();
            RegexTokenReader reader = new(_source);
            TokenParser<RegexElementLabel, RegexElementLabel> lexer = reader.Pipe(() => new RegexTokenParser());

            while (lexer.TryReadToken(out Token<RegexElementLabel> token))
            {
                // Console.WriteLine(token.Value);
                if ((token.Flag & RegexElementLabel.SEQUENCE) != 0)
                {
                    // char or charset
                    _stack.PushOperand(new RegexAstNode(token));
                } else if ((token.Flag & RegexElementLabel.GROUP_BEGIN) != 0)
                {
                    _stack.PushOperator(new RegexAstNode(token));
                    _stack.IncreaseDepth(0);
                } else if ((token.Flag & RegexElementLabel.GROUP_END) != 0)
                {
                    // close all nodes here
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
                    if (op.Source.Value.Length > 1)
                    {
                        op.Right = _stack.PopOperand();
                        _stack.PushOperand(op);
                    }
                } else if ((token.Flag & RegexElementLabel.OPERATOR) != 0)
                {
                    if (_stack.OperatorInDepthCount == 0)
                    {
                        // the first operator in stack
                        _stack.PushOperator(new RegexAstNode(token));
                    } else
                    {
                        // process operator by precedence
                        while (_stack.TryPeekOperator(out RegexAstNode topOp))
                        {
                            (bool Valid, int Value) cmp = RegexAstNode.ComparePrecedence(topOp.Source.Flag, token.Flag);

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
                    return _tree;
                }
            }
            throw new InvalidOperationException("Invalid regular expression syntax.");


            void ProcessOperator(
                 RegexAstNode op
            )
            {
                if (op.EqualityTypeConvert == RegexElementLabel.CONCAT)
                {
                    if (_stack.TryPopOperand(out RegexAstNode? _1) &&
                        _stack.TryPopOperand(out RegexAstNode? _2))
                    {
                        op.Left = _2;
                        op.Right = _1;
                        _stack.PushOperand(op);
                        _ = _tree.AddNode(op);
                        _ = _tree.AddNode(_1);
                        _ = _tree.AddNode(_2);
                    } else
                    {
                        throw new InvalidOperationException("Insufficient operands for CONCAT operator.");
                    }
                } else if (op.EqualityTypeConvert == RegexElementLabel.COUNT)
                {
                    if (_stack.TryPopOperand(out RegexAstNode? _1))
                    {
                        op.Right = _1;
                        _stack.PushOperand(op);
                        _ = _tree.AddNode(_1);
                        _ = _tree.AddNode(op);
                    } else
                    {
                        throw new InvalidOperationException("Insufficient operands for CONCAT operator.");
                    }
                } else if (op.EqualityTypeConvert == RegexElementLabel.ALT)
                {
                    if (_stack.TryPopOperand(out RegexAstNode? _1) &&
                        _stack.TryPopOperand(out RegexAstNode? _2))
                    {
                        op.Left = _2;
                        op.Right = _1;
                        _stack.PushOperand(op);
                        _ = _tree.AddNode(op);
                        _ = _tree.AddNode(_1);
                        _ = _tree.AddNode(_2);
                    } else
                    {
                        throw new InvalidOperationException("Insufficient operands for CONCAT operator.");
                    }
                }
            }
        }

    }
}
