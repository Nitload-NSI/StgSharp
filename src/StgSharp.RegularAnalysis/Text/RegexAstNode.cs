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
    // TODO 现在存在的问题是，AstNode过于依赖NodeFlag表达其内部语义，这导致进行精细优化时识别节点语义复杂，
    // TODO 当前Ast的类型差异过小，不利于保存特征数据，应当争取进一步细化，或者说这是AstNode一开始为了简化架构设计引入的妥协
    internal class RegexAstNode : ISyntaxNode<RegexAstNode, RegexElementLabel>
    {

        // private RegexAstNode _left, _right;

        public RegexAstNode(
               Token<RegexElementLabel> source
        )
            : this(RegexCommonPayload.FromToken(source))
        { }

        public RegexAstNode(
               RegexAstPayload payload
        )
        {
            Payload = payload;
        }

        public int EnumState { get; set; }

        public long NodeFlag => (long)Label;

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

        public RegexElementLabel EqualityTypeConvert => Label;

        public RegexElementLabel Label => Payload.Flag;

        public string Value => Payload switch
        {
            RegexLiteralPayload literal => literal.Text,
            RegexCharSetPayload set => set.Source,
            _ => Payload.Source
        };

        public RegexAstPayload Payload { get; private protected set; }

        public ISyntaxPayload<RegexElementLabel> Source => Payload;

        internal TPayload PayloadAs<TPayload>() where TPayload : RegexAstPayload =>
            Payload as TPayload ?? throw new InvalidOperationException(
                $"Node {Label} does not carry {typeof(TPayload).Name}.");

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
                Payload = Left.Payload;
                Left.Parent = Empty;
                Left = Empty;
            } else
            {
                Payload = Right.Payload;
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
            Payload = node.Payload;
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

            public EmptyRegexAstNode() : base(new RegexEmptyPayload()) { }

        }

    }
}
