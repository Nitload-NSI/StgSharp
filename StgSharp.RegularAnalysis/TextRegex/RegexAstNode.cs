//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="RegexAstNode"
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
using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StgSharp.RegularAnalysis.TextRegex
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

        public  RegexAstNode Previous { get; set; }

        public  RegexAstNode Next { get; set; }

        public  RegexAstNode Parent { get; set; }

        public RegexAstNode Left
        {
            get;
            set
            {
                field?.Parent = Empty;
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
                field?.Parent = Empty;
                field = value;
                if (!IsNullOrEmpty(value)) {
                    value.Parent = this;
                }
            }
        }

        public RegexElementLabel EqualityTypeConvert
        {
            get => Source.Flag;
        }

        public string Value => Source.Value;

        public string CodeConvertTemplate { get; }

        public Token<RegexElementLabel> Source { get; private protected set; }

        public void AppendNode(
                    RegexAstNode nextToken
        )
        {
            Next = nextToken;
            nextToken.Previous = this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (bool Valid,int Value) ComparePrecedence(
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
