//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ISyntaxNode"
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
using StgSharp.Collections;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace StgSharp.RegularAnalysis.Abstraction
{
    public interface ISyntaxNode<TNode, TLabel> where TNode: ISyntaxNode<TNode, TLabel> where TLabel: unmanaged
    {

        long NodeFlag { get; }

        string CodeConvertTemplate { get; }

        TLabel EqualityTypeConvert { get; }

        TNode Previous { get; set; }

        TNode Next { get; set; }

        TNode Left { get; set; }

        TNode Right { get; set; }

        TNode Parent { get; set; }

        static abstract TNode Empty { get; }

        Token<TLabel> Source { get; }

        protected internal int EnumState { get; set; }

        void AppendNode(TNode nextToken);

        void PrependNode(TNode previousNode);

    }
}
