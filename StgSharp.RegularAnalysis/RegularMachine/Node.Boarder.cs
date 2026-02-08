//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Node.Boarder"
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis
{
    internal class RegularExpressionLeftBoarderNode : RegularStateNode
    {

        public RegularExpressionLeftBoarderNode(
               int index,
               string name
        )
        {
            Boarder = new GroupIndex
            {
                Index = index,
                Name = name
            };
        }

        public event EventHandler<GroupIndex> OnBoarderEntered;

        public GroupIndex Boarder { get; private set; }

        public override NodeType Type => NodeType.LEFT_BOARDER;

        public override bool IsValidateFrom(
                             RegularStateNode former,
                             RegularContext context
        )
        {
            return true;
        }

    }

    internal class RegularExpressionRightBoarderNode : RegularStateNode
    {

        public RegularExpressionRightBoarderNode(
               int index,
               string name
        )
        {
            Boarder = new GroupIndex
            {
                Index = index,
                Name = name
            };
        }

        public event EventHandler<GroupIndex> OnBoarderEntered;

        public GroupIndex Boarder { get; private set; }

        public override NodeType Type => NodeType.RIGHT_BOARDER;

        public override bool IsValidateFrom(
                             RegularStateNode former,
                             RegularContext context
        )
        {
            return true;
        }

    }
}
