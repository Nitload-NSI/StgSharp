//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="RegularExpression.NodeState"
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
    internal enum NodeType
    {

        // single unicode character: a、B、1、@、汉字 etc.
        CHAR,

        // collections of finate characters:  [abc]、[^abc]、\d、\w、\s、\D、\W、\S etc.
        CHARSET,

        // increase count of a counter, half function of *+? quantifiers
        COUNT,

        // left boarder of a sub sequnce, usually begin of match group or lookaround
        LEFT_BOARDER,

        // right boarder of a sub sequnce, usually end of match group or lookaround
        RIGHT_BOARDER,

        // zero length char or structure
        EPSILON,

        // more than one cases to match, like alter|nation
        BRANCH,

        // a sequence of nodes to match in order, each char should be fully defined but not charset
        SEQUENCE

    }
}
 