// -----------------------------------------------------------------------------
// file="RegexAnalyzer.ASTGenerate"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.RegularAnalysis.Abstraction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitload.RegularAnalysis.Text
{
    public static partial class RegexAnalyzer
    {

        private static AbstractSyntaxTree<RegexAstNode, RegexElementLabel> GenerateAST(
                                                                           string _source
        )
        {
            CompileStack<RegexAstNode, RegexElementLabel> _stack = new();
            AbstractSyntaxTree<RegexAstNode, RegexElementLabel> _tree = new();
            RegexTokenReader reader = new(_source);
            TokenParser<RegexElementLabel, RegexElementLabel> lexer = reader.Pipe(() => new RegexTokenParser());
            Stack<RegexElementLabel> outerLastTokens = [];
            RegexElementLabel lastToken = RegexElementLabel.NONE;

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
                    outerLastTokens.Push(lastToken);
                    lastToken = RegexElementLabel.GROUP_BEGIN;
                } else if ((token.Flag & RegexElementLabel.GROUP_END) != 0)
                {
                    if (lastToken == RegexElementLabel.ALT) {
                        PushEpsilon(token.Line, token.Column);
                    }
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
                    if (op.Label != RegexElementLabel.GROUP_BEGIN) {
                        throw new InvalidOperationException("Unmatched group begin and end symbol");
                    }
                    if (op.Payload.Source.Length > 1)
                    {
                        op.Right = _stack.PopOperand();
                        _stack.PushOperand(op);
                    }
                    _ = outerLastTokens.Pop();
                    lastToken = RegexElementLabel.GROUP_END;
                } else if ((token.Flag & RegexElementLabel.OPERATOR) != 0)
                {
                    if (token.Flag == RegexElementLabel.ALT &&
                        lastToken is RegexElementLabel.NONE or
                                     RegexElementLabel.GROUP_BEGIN or
                                     RegexElementLabel.ALT)
                    {
                        PushEpsilon(token.Line, token.Column);
                    }
                    if (_stack.OperatorInDepthCount == 0)
                    {
                        // the first operator in stack
                        _stack.PushOperator(new RegexAstNode(token));
                    } else
                    {
                        // process operator by precedence
                        while (_stack.TryPeekOperator(out RegexAstNode topOp))
                        {
                            (bool Valid, int Value) cmp = RegexAstNode.ComparePrecedence(topOp.Label, token.Flag);

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
                if (token.Flag != RegexElementLabel.GROUP_BEGIN &&
                    token.Flag != RegexElementLabel.GROUP_END) {
                    lastToken = token.Flag;
                }
            }
            if (lastToken == RegexElementLabel.ALT) {
                PushEpsilon(0, _source.Length);
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

            void PushEpsilon(int row, int column)
            {
                RegexAstNode epsilon = new(new RegexEpsilonPayload(row, column));
                _stack.PushOperand(epsilon);
                _ = _tree.AddNode(epsilon);
            }


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
