// -----------------------------------------------------------------------------
// file="RegexAstNode"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.RegularAnalysis.Abstraction;
using System.Runtime.CompilerServices;

namespace StgSharp.RegularAnalysis.Text
{
    internal class RegexAstNode : ISyntaxNode<RegexAstNode, RegexElementLabel>
    {

        // private RegexAstNode _left, _right;

        public RegexAstNode(
               Token<RegexElementLabel> source
        )
        {
            Source = source;
        }

        public int EnumState { get; set; }

        public long NodeFlag => (long)Source.Flag;

        public static RegexAstNode Empty { get; }
        = new EmptyRegexAstNode();

        public RegexAstNode Previous { get; set; }

        public RegexAstNode Next { get; set; }

        public RegexAstNode Parent { get; set; }

        public RegexAstNode Left
        {
            get;
            set
            {
                _ = (field?.Parent = Empty);
                field = value;
                if (!IsNullOrEmpty(value)) {
                    value.Parent = this;
                }
            }
        }

        public RegexAstNode Right
        {
            get;
            set
            {
                _ = (field?.Parent = Empty);
                field = value;
                if (!IsNullOrEmpty(value)) {
                    value.Parent = this;
                }
            }
        }

        public RegexElementLabel EqualityTypeConvert => Source.Flag;

        public string Value => Source.Value;

        public Token<RegexElementLabel> Source { get; private protected set; }

        public void AppendNode(
                    RegexAstNode nextToken
        )
        {
            Next = nextToken;
            nextToken.Previous = this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (bool Valid, int Value) ComparePrecedence(
                                              RegexElementLabel left,
                                              RegexElementLabel right
        )
        {
            left &= RegexElementLabel.OPERATOR;
            right &= RegexElementLabel.OPERATOR;
            int v_left = (int)left, v_right = (int)right;
            if (v_left == 0 || v_right == 0) {
                return (false,0);
            }
            return (true,v_left - v_right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(
                           RegexAstNode node
        )
        {
            return node is null || ReferenceEquals(node, Empty);
        }
        /**/

        public void PrependNode(
                    RegexAstNode previousNode
        )
        {
            Previous = previousNode;
            previousNode.Next = this;
        }

        public void RemoveChild(
                    RegexAstNode node
        )
        {
            if (Left == node)
            {
                Left = Empty;
            } else if (Right == node) {
                Right = Empty;
            }
            node.Parent = Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static RegexAstNode NewEmptyNode(
        )
        {
            return new EmptyRegexAstNode();
        }

        internal void RemoveAndAsChild(
                      bool useLeft
        )
        {
            if (useLeft)
            {
                Source = Left.Source;
                Left.Parent = Empty;
                Left = Empty;
            } else
            {
                Source = Right.Source;
                Right.Parent = Empty;
                Right = Empty;
            }
        }
        /**/

        /// <summary>
        ///   Replace the current node with another node, and update the parent and sibling
        ///   references accordingly.  This is implemented by copying metadata and nearby
        ///   environment of the given node to the current node, and then update the nearby nodes to
        ///   point to the current node.
        /// </summary>
        /// <param name="node">
        ///   The node to replace the current node with.
        /// </param>
        internal void ReplaceBy(
                      RegexAstNode node
        )
        {
            Source = node.Source;
            Left = node.Left;
            Right = node.Right;
            if (!IsNullOrEmpty(Left)) {
                Left.Parent = this;
            }
            if (!IsNullOrEmpty(Right)) {
                Right.Parent = this;
            }
            Previous = node.Previous;
            Next = node.Next;

            RegexAstNode prev = Previous;
            RegexAstNode next = Next;
            if (!IsNullOrEmpty(prev)) {
                prev.Next = this;
            }
            if (!IsNullOrEmpty(next)) {
                next.Previous = this;
            }
        }

        private sealed class EmptyRegexAstNode : RegexAstNode

        {

            public EmptyRegexAstNode() : base(Token<RegexElementLabel>.Empty) { }

        }

    }
}
