// -----------------------------------------------------------------------------
// file="CompileStack"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Collections;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Abstraction
{
    public partial class CompileStack<TNode, TLabel> where TNode : ISyntaxNode<TNode, TLabel>
        where TLabel : unmanaged
    {

        private readonly RandomAccessibleStack<DepthMark> _depthMarks = [];
        private readonly RandomAccessibleStack<TNode> _operands = [];
        private readonly RandomAccessibleStack<TNode> _operators = [];

        public CompileStack()
        {
            _depthMarks.Push(new DepthMark(0, 0, 0));
        }

        public int OperandInDepthCount => _operands.Count - _depthMarks.Peek().OperandPos;

        public int OperatorInDepthCount => _operators.Count - _depthMarks.Peek().OperatorPos;

        public int Depth => _depthMarks.Count;

        private record struct DepthMark(
                              int OperandPos,
                              int OperatorPos,
                              int Mark
        );

        #region operator

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TNode PopOperator(
        )
        {
            if (OperatorInDepthCount > 0) {
                return _operators.Pop();
            }
            return TNode.Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PushOperator(
                    TNode operatorNode
        )
        {
            _operators.Push(operatorNode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryPopOperator(
                    out TNode op
        )
        {
            if (OperatorInDepthCount > 0)
            {
                op = _operators.Pop();
                return true;
            }
            op = TNode.Empty;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryPeekOperator(
                    out TNode op
        )
        {
            if (OperatorInDepthCount > 0)
            {
                op = _operators.Peek();
                return true;
            }
            op = TNode.Empty;
            return false;
        }

            #endregion

        #region operand

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TNode PopOperand(
        )
        {
            if (OperandInDepthCount > 0) {
                return _operands.Pop();
            }
            return TNode.Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PushOperand(
                    TNode operatorNode
        )
        {
            _operands.Push(operatorNode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryPopOperand(
                    out TNode op
        )
        {
            if (OperandInDepthCount > 0)
            {
                op = _operands.Pop();
                return true;
            }
            op = TNode.Empty;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryPeekOperand(
                    out TNode op
        )
        {
            if (OperandInDepthCount > 0)
            {
                op = _operands.Peek();
                return true;
            }
            op = TNode.Empty;
            return false;
        }

            #endregion

        #region depth

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DecreaseDepth(
        )
        {
            if (Depth > 1)
            {
                DepthMark mark = _depthMarks.Pop();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void IncreaseDepth(
                    int mark
        )
        {
            _depthMarks.Push(new DepthMark(_operands.Count, _operators.Count, mark));
        }

        #endregion
    }
}
