//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="AbstractSyntaxTree"
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
using StgSharp.Collections;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;

namespace StgSharp.RegularAnalysis.Abstraction
{
    public sealed class AbstractSyntaxTree<TNode, TType> where TNode : ISyntaxNode<TNode, TType>
        where TType : unmanaged
    {

        private HashSet<TNode> _allToken = new();

        public HashSet<TNode> AllNodes
        {
            get { return _allToken; }
        }

        public int Count
        {
            get { return _allToken.Count; }
        }

        public TNode Root
        {
            get;
            internal set
            {
                if (_allToken.Contains(value)) {
                    field = value;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AddNode(
                    TNode node
        )
        {
            return _allToken.Add(node);
        }

        public bool Contains(
                    TNode node
        )
        {
            return _allToken.Contains(node);
        }

        public IEnumerator<TNode> GetEnumerator()
        {
            return new TreeEnumerator<TNode, TType>(this);
        }

    }

    internal class TreeEnumerator<TNode, TType>(
                   TNode root
    ) : IEnumerator<TNode> where TNode : ISyntaxNode<TNode, TType> where TType : unmanaged
    {

        private bool _completed = false;

        private int _state, _target; // 0: not started, 1: on right, 2: ended
        private Stack<Record> _recycle = [];
        private Stack<Record> _stack = [];

        // private TNode _lRoot;
        private TNode _root = root;

        internal TreeEnumerator(
                 AbstractSyntaxTree<TNode, TType> tree
        )
            : this(tree.Root) { }

        public int Depth => _stack.Count;

        public TNode Current
        {
            get
            {
                if (_stack.TryPeek(out Record? r)) {
                    return r.Node;
                }
                return TNode.Empty;
            }
        }

        public void Dispose()
        {
            return;
        }

        public bool MoveNext()
        {
            if (_completed) {
                return false;
            }
            while (_stack.TryPeek(out Record? cur))
            {
                switch (cur.State)
                {
                    case 0:
                        cur.State++;
                        if (TNode.IsNullOrEmpty(cur.Node.Left))
                        {
                            goto case 1;
                        } else
                        {
                            _stack.Push(GetNewRecord(cur.Node.Left));
                            return true;
                        }
                    case 1:
                        cur.State++;
                        if (TNode.IsNullOrEmpty(cur.Node.Right))
                        {
                            goto case 2;
                        } else
                        {
                            _stack.Push(GetNewRecord(cur.Node.Right));
                            return true;
                        }
                    case 2:
                        _recycle.Push(_stack.Pop());
                        if (TNode.IsNullOrEmpty(cur.Node.Next))
                        {
                            if (_stack.Count == 0)
                            {
                                _completed = true;
                                return false;
                            }
                        } else
                        {
                            _stack.Push(GetNewRecord(cur.Node.Next));
                            return true;
                        }
                        break;
                    default:
                        break;
                }
            }
            _stack.Push(GetNewRecord(_root));
            return true;
        }

        public void Reset()
        {
            _completed = false;
            _stack.Clear();
            _state = 0;
        }

        /// <summary>
        ///   Skips the node and its subtree if the current node is the given node. Otherwise, do
        ///   nothing.
        /// </summary>
        /// <param name="node">
        ///   The node to skip.
        /// </param>
        public void SkipNode(
                    TNode node
        )
        {
            if (_stack.TryPeek(out Record? r) && ReferenceEquals(r.Node, node))
            {
                _ = _stack.Pop();
                _recycle.Push(r);
            }
        }

        public void SkipOnce()
        {
            if (_stack.TryPeek(out Record? cur))
            {
                switch (cur.State)
                {
                    case 0:
                        cur.State++;
                        break;
                    case 1:
                        cur.State++;
                        break;
                    case 2:
                        if (TNode.IsNullOrEmpty(cur.Node.Next))
                        {
                            _recycle.Push(_stack.Pop());
                            if (_stack.Count == 0)
                            {
                                _completed = true;
                                return;
                            }
                        } else
                        {
                            _stack.Push(GetNewRecord(cur.Node.Next));
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private Record GetNewRecord(
                       TNode node
        )
        {
            if (_recycle.TryPop(out Record? r))
            {
                r.State = 0;
                r.Node = node;
                return r;
            }
            return new Record(node, 0);
        }

        object IEnumerator.Current => Current;

        private class Record(
                      TNode node,
                      int state
        )
        {

            public int State { get; set; } = state;

            public TNode Node { get; set; } = node;

        }

    }
}
