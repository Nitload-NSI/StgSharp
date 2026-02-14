//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="CompileStack"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
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

        public int OperandInDepthCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _operands.Count - _depthMarks.Peek().OperandPos;
        }

        public int OperatorInDepthCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _operators.Count - _depthMarks.Peek().OperatorPos;
        }

        public int Depth
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _depthMarks.Count;
        }

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
