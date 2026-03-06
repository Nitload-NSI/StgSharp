//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="RegexInterpreter.Optimize"
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
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StgSharp.RegularAnalysis.TextRegex
{
    public partial class RegexInterpreter
    {

        private static RegexAstNode FlattenAlt(
                                    RegexAstNode root
        )
        {
            TreeEnumerator<RegexAstNode, RegexElementLabel> enumerator = new(root);
            List<RegexAstNode> cases = [];
            RegexAstNode current;
            _ = enumerator.MoveNext();             // skip root
            while (enumerator.MoveNext())
            {
                current = enumerator.Current;
                if (current.EqualityTypeConvert != RegexElementLabel.ALT)
                {
                    cases.Add(current);
                    enumerator.SkipNode(current);
                }
                current.Parent.RemoveChild(current);
            }
            for (int i = 0; i < cases.Count - 1; i++)
            {
                cases[i].AppendNode(cases[i + 1]);
                cases[i].Parent = root;
            }
            cases[^1].Parent = root;
            return cases[0];
        }

        private static bool MergeConcat(
                            RegexAstNode root
        )
        {
            if (root.EqualityTypeConvert != RegexElementLabel.CONCAT) {
                return false;
            }

            /*
             *    concat      →      'a'
             *    ╱
             *  'a'
            */
            if ((root.Left.EqualityTypeConvert & RegexElementLabel.SEQUENCE) != 0 &&
                RegexAstNode.IsNullOrEmpty(root.Right))
            {
                root.RemoveAndAsChild(true);
                return true;
            }
            if ((root.Right.EqualityTypeConvert & RegexElementLabel.SEQUENCE) != 0 &&
                RegexAstNode.IsNullOrEmpty(root.Left))
            {
                root.RemoveAndAsChild(false);
                return true;
            }

            /*
             *    concat      →      concat
             *    ╱    ╲             /
             *  'a'   concat       "abc"
             *        /    \
             *       'b'    'c'
            */
            List<string> buffer = [];
            int length = 0;
            RegexAstNode right = root.Right;
            RegexAstNode left = root.Left;
            if (left.EqualityTypeConvert != RegexElementLabel.UNIT_SPAN &&
                left.EqualityTypeConvert != RegexElementLabel.UNIT) {
                return false;
            }
            string str = left.Value;
            buffer.Add(str);
            length += str.Length;
            while (!RegexAstNode.IsNullOrEmpty(right) &&
                   right.EqualityTypeConvert == RegexElementLabel.CONCAT)
            {
                left = right.Left;
                if (left.EqualityTypeConvert != RegexElementLabel.UNIT_SPAN &&
                    left.EqualityTypeConvert != RegexElementLabel.UNIT)
                {
                    break;
                }
                str = left.Value;
                buffer.Add(str);
                length += str.Length;
                root.Right = right.Right;
                right.Parent = RegexAstNode.Empty;
                right = root.Right;
                right.Parent = root;
            }
            if (right.EqualityTypeConvert == RegexElementLabel.UNIT_SPAN ||
                right.EqualityTypeConvert == RegexElementLabel.UNIT)
            {
                root.Right = RegexAstNode.Empty;
                buffer.Add(right.Value);
                length += right.Value.Length;
            }
            str = string.Create(length, buffer, static(
                                                span,
                                                buffer
            ) => {
                int offset = 0;
                for (int i = 0; i < buffer.Count; i++)
                {
                    buffer[i].AsSpan().CopyTo(span[offset..]);
                    offset += buffer[i].Length;
                }
            });
            root.Left = new RegexAstNode(new Token<RegexElementLabel>(str, left.Source.Line,
                                                                      left.Source.Column,
                                                                      str.Length == 1 ?
                                                                      RegexElementLabel.UNIT :
                                                                      RegexElementLabel.UNIT_SPAN));
            return true;
        }

        private static void RotateConcat(
                            RegexAstNode root
        )
        {
            RegexAstNode left = root.Left;
            while (!RegexAstNode.IsNullOrEmpty(left) &&
                   left.EqualityTypeConvert == RegexElementLabel.CONCAT)
            {
                /*
                 *     root      →        root
                 *    ╱    ╲             ╱    ╲
                 *  left    c           a    left
                 *  ╱  ╲                     ╱  ╲
                 * a    b                   b    c
                */
                RegexAstNode a = left.Left;
                RegexAstNode b = left.Right;
                RegexAstNode c = root.Right;
                root.Left = a;
                if (RegexAstNode.IsNullOrEmpty(a)) {
                    a.Parent = root;
                }

                root.Right = left;
                left.Left = b;
                left.Right = c;
                if (RegexAstNode.IsNullOrEmpty(b)) {
                    b.Parent = left;
                }

                if (RegexAstNode.IsNullOrEmpty(c)) {
                    c.Parent = left;
                }
                left = root.Left;
            }
        }

        #region merge alt

        /// <summary>
        ///   Get union beginning part of cases
        /// </summary>
        /// <param name="cases">
        ///
        /// </param>
        /// <returns>
        ///
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static RegexAstNode MergeAlt(
                                    List<RegexAstNode> cases
        )
        {
            UnionAlt union = new(0, cases);
            RegexAstNode root = union.AsNormal();

            return root;
        }

        private class UnionAlt : RegexAstNode
        {

            private const int MaxLevel = 8;

            private UnionAlt(
                    int level,
                    RegexAstNode node
            )
                : base(node.Source)
            {
                Level = level;
                Cases = new List<RegexAstNode>();

                RegexAstNode right = IsNullOrEmpty(node.Right) ?
                                     new RegexAstNode(
                    new Token<RegexElementLabel>(node.Value, 0, node.Source.Column,
                                                 RegexElementLabel.UNIT)
                    ) :
                                     node.Right;
                Cases.Add(right);

                node.PrependNode(Empty);
                node.AppendNode(Empty);
                UnionBegin = node;
                UnionEnd = node;
            }

            public UnionAlt(
                   int level,
                   List<RegexAstNode> c
            )
                : base(c[0].Source)
            {
                Cases = c;
                Level = level;
            }

            public List<RegexAstNode> Cases { get; set; }

            public RegexAstNode UnionBegin { get; set; }

            public RegexAstNode UnionEnd { get; set; }

            private int Level { get; set; }

            public RegexAstNode AsNormal()
            {
                if (Level >= MaxLevel)
                {
                    goto build;
                }
                merge:
                List<RegexAstNode> buffer = [];
                for (int i = 0; i < Cases.Count; i++)
                {
                    if ((Cases[i].EqualityTypeConvert & RegexElementLabel.SEQUENCE) != 0)
                    {
                        RegexAstNode concat = new RegexAstNode(new Token<RegexElementLabel>(string.Empty,
                                                                                            0,
                                                                                            Cases[i].Source.Column,
                                                                                            RegexElementLabel.CONCAT));
                        concat.Left = Cases[i];
                        Cases[i].Parent = concat;
                        concat.Right = Empty;
                        Cases[i] = concat;
                    }
                    RegexAstNode current = Cases[i];
                    int j = 0;
                    for (; j < buffer.Count; j++)
                    {
                        if (CanMerge(current, buffer[j]))
                        {
                            // find a case in buffer that can merge with current case,
                            // then merge them and break
                            RegexAstNode b = buffer[j];
                            if (b is not UnionAlt alt)
                            {
                                alt = new UnionAlt(Level + 1, b);
                                alt.AddCase(current);
                                buffer[j] = alt;
                            } else
                            {
                                alt.AddCase(current);
                            }
                            break;
                        }
                    }
                    if (j >= buffer.Count) {
                        buffer.Add(current);
                    }
                }
                if (Cases.Count != buffer.Count) {
                    Cases = buffer;
                }

                if (buffer.Count == 1)
                {
                    // if there is only one case,
                    // then we suppose that its contained cases has a same beginning,
                    // so we attach it to UnionEnd and stay level unchanged
                    UnionAlt alt = (Cases[0] as UnionAlt)!;
                    if (IsNullOrEmpty(UnionBegin))
                    {
                        UnionBegin = alt.UnionBegin;
                        UnionEnd = alt.UnionEnd;
                    } else
                    {
                        alt.UnionBegin.Parent = UnionEnd;
                        UnionEnd.Right = alt.UnionBegin;
                        UnionEnd = alt.UnionBegin;
                    }

                    Cases = alt.Cases;
                    goto merge;
                }

                build:

                // suppose that from UnionBegin to UnionEnd is a normal form without union
                for (int i = 0; i < Cases.Count; i++)
                {
                    if (Cases[i] is UnionAlt union) {
                        Cases[i] = union.AsNormal();
                    }
                }

                RegexAstNode branch;
                if (IsNullOrEmpty(UnionEnd))
                {
                    branch = new RegexAstNode(new Token<RegexElementLabel>(string.Empty, 0,
                                                                           Source.Column,
                                                                           RegexElementLabel.ALT));
                    UnionBegin = branch;
                    UnionEnd = branch;
                } else
                {
                    branch = new RegexAstNode(new Token<RegexElementLabel>(string.Empty, 0,
                                                                           UnionEnd.Source.Column,
                                                                           RegexElementLabel.ALT));
                    UnionEnd.Right = branch;
                    branch.Parent = UnionEnd;
                    UnionEnd = branch;
                }

                for (int i = 0; i < Cases.Count - 1; i++)
                {
                    Cases[i] ??= NewEmptyNode();
                    Cases[i].AppendNode(Cases[i + 1]);
                    Cases[i].Parent = UnionEnd;
                }
                UnionEnd.Right = Cases[0];
                TreeEnumerator<RegexAstNode, RegexElementLabel> enumerator = new TreeEnumerator<RegexAstNode, RegexElementLabel>(UnionBegin);

                return UnionBegin;
            }

            private void AddCase(
                         RegexAstNode _case
            )
            {
                RegexAstNode right = IsNullOrEmpty(_case.Right) ?
                                     new RegexAstNode(
                    new Token<RegexElementLabel>(_case.Value, 0, _case.Source.Column,
                                                 RegexElementLabel.UNIT)
                    ) :
                                     _case.Right;
                Cases.Add(right);
            }

            private static bool CanMerge(
                                RegexAstNode origin,
                                RegexAstNode target
            )
            {
                // suppose that origin is not a union,
                if (target is UnionAlt alt)
                {
                    // then we suppose that alt has only one are same
                    return origin .EqualityTypeConvert == RegexElementLabel.CONCAT &&
                           (origin.Left.EqualityTypeConvert & RegexElementLabel.SINGLE) != 0 &&
                           origin.Left.EqualityTypeConvert == alt.UnionEnd.Left.EqualityTypeConvert &&
                           origin.Left.Value == alt.UnionEnd.Left.Value;
                } else
                {
                    return origin.EqualityTypeConvert == RegexElementLabel.CONCAT &&
                           target.EqualityTypeConvert == RegexElementLabel.CONCAT &&
                           (origin.Left.EqualityTypeConvert & RegexElementLabel.SINGLE) != 0 &&
                           origin.Left.EqualityTypeConvert == target.Left.EqualityTypeConvert &&
                           origin.Left.Value == target.Left.Value;
                }
            }

        }

        #endregion

        /**/
    }
}
