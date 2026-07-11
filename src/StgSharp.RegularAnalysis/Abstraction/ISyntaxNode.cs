// -----------------------------------------------------------------------------
// file="ISyntaxNode"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Collections;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.RegularAnalysis.Abstraction
{
    public interface ITrinitySyntaxNode<TNode, TLabel> : ISyntaxNode<TNode, TLabel>
        where TNode : ISyntaxNode<TNode, TLabel>
        where TLabel : unmanaged
    {

        TNode Third { get; set; }

    }

    public interface ISyntaxNode<TNode, TLabel> where TNode : ISyntaxNode<TNode, TLabel>
        where TLabel : unmanaged
    {

        long NodeFlag { get; }

        TLabel EqualityTypeConvert { get; }

        TNode Previous { get; set; }

        TNode Next { get; set; }

        TNode Left { get; set; }

        TNode Right { get; set; }

        TNode Parent { get; set; }

        static abstract TNode Empty { get; }

        Token<TLabel> Source { get; }

        protected internal int EnumState { get; set; }

        void AppendNode(
             TNode nextToken
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual bool IsNullOrEmpty(
                                   TNode node
        )
        {
            return node is null || node.Equals(TNode.Empty);
        }

        void PrependNode(
             TNode previousNode
        );

    }
}
