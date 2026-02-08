//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Node.Branch"
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
using StgSharp.MVVM;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis
{
    internal sealed class RegularBranchNode : RegularStateNode
    {

        private int _afterBranch;

        private int _lastBranch;

        private List<BranchMeasureResult> _result;

        public override NodeType Type => NodeType.BRANCH;

        public override bool IsValidateFrom(
                             RegularStateNode former,
                             RegularContext context
        )
        {
            return true;
        }

        public override bool TryTransaction(
                             ReadOnlySpan<RegularStateNode> nodeSpan,
                             RegularContext context,
                             out int label
        )
        {
            if (context.StateCode == 0)
            {
                // branch match
                label = _afterBranch;
                _ = context.LastBranch.Pop();
                _lastBranch = 0;
                return true;
            }

            // restore sequence position
            int pos = context.LastBranch.Peek();
            context.Position = pos;
            if (AvailableNodes.Length > _lastBranch)
            {
                label = FailNodeIndex;
                return false;
            }
            _lastBranch++;
            label = AvailableNodes[_lastBranch];
            return true;
        }

    }

    internal struct BranchMeasureResult
    {

        internal BranchMeasureResult(
                 bool match,
                 int size
        )
        {
            IsMatch = match;

            Length = size;
        }

        public bool IsMatch { get; init; }

        public static BranchMeasureResult Fail { get; } = new BranchMeasureResult();

        public int Length { get; init; }

    }
}
