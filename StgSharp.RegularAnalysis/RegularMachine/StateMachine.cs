//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="StateMachine"
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
using StgSharp.Internal;
using StgSharp.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis
{
    internal class RegularExpressionState : ISimpleStateMachine<RegularStateNode, int, RegularContext>
    {

        private List<RegularStateNode> _nodes;
        private RegularStateNode _currentState;

        public RegularStateNode this[
                                int label
        ] => _nodes[label];

        public ref readonly RegularStateNode GetCurrentStateNode()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public bool TryMoveNext(
                    RegularContext ctx
        )
        {
            throw new NotImplementedException();
        }

        #region node factory

        public RegularExpressionCharNode AddChar(
                                         char c
        )
        {
            RegularExpressionCharNode node = new RegularExpressionCharNode(c);
            int index = _nodes.Count;
            _nodes.Add(node);
            node.FailNodeIndex = index; // Point to self by default
            return node;
        }

        public RegularExpressionCharsetNode AddCharset(
                                            ReadOnlySpan<char> chars,
                                            bool isNegate
        )
        {
            RegularExpressionCharsetNode node = new RegularExpressionCharsetNode(chars, isNegate);
            int index = _nodes.Count;
            _nodes.Add(node);
            node.FailNodeIndex = index; // Point to self by default
            return node;
        }

        public (RegularExpressionLeftBoarderNode left,RegularExpressionRightBoarderNode right) AddBoarder(
                                                                                               string groupName,
                                                                                               int groupIndex
        )
        {
            RegularExpressionLeftBoarderNode leftNode = new RegularExpressionLeftBoarderNode(groupIndex, groupName);
            int leftIndex = _nodes.Count;
            _nodes.Add(leftNode);
            leftNode.FailNodeIndex = leftIndex; // Point to self by default
            RegularExpressionRightBoarderNode rightNode = new RegularExpressionRightBoarderNode(groupIndex, groupName);
            int rightIndex = _nodes.Count;
            _nodes.Add(rightNode);
            rightNode.FailNodeIndex = rightIndex; // Point to self by default
            return (left: leftNode,right:rightNode);
        }

        public RegularExpressionCounterNode AddCounter(
                                            int minCount,
                                            bool isEager
        )
        {
            RegularExpressionCounterNode node = new RegularExpressionCounterNode(minCount, isEager);
            int index = _nodes.Count;
            _nodes.Add(node);
            node.FailNodeIndex = index; // Point to self by default
            return node;
        }

        #endregion
    }

    internal class RegularContext : StateContext
    {

        public int Begin { get; set; }

        public int Position { get; set; } = -1;

        public int StateCode { get; set; } = 0;

        public ReadOnlySpan<char> CurrentCharSpan => Source.AsSpan()[Begin..Position];

        public string Source { get; init; }

        internal Stack<int> LastBranch { get; } = new();

    }
}
