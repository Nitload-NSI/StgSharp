//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="AbstractSyntaxTree.cs"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the “Software”), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//     
// The above copyright notice and 
// this permission notice shall be included in all copies 
// or substantial portions of the Software.
//     
// THE SOFTWARE IS PROVIDED “AS IS”, 
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
// ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.Collections;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Schema;

namespace StgSharp.RegularAnalysis.Abstraction
{
    public sealed class AbstractSyntaxTree<TNode, TType> where TNode: ISyntaxNode<TNode, TType> where TType: unmanaged
    {

        private HashSet<TNode> _allToken = new();

        private TNode _root;

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
        public bool AddNode(TNode node)
        {
            return _allToken.Add(node);
        }

        public bool Contains(TNode node)
        {
            return _allToken.Contains(node);
        }

        public IEnumerator<TNode> GetEnumerator()
        {
            return new Enumerator(this);
        }

        private class Enumerator : IEnumerator<TNode>
        {

            private int _state,_target; // 0: not started, 1: on right, 2: ended
            private Stack<TNode> _stack, _listStack;
            private TNode _lRoot;
            private TNode _root;

            public Enumerator(AbstractSyntaxTree<TNode, TType> tree)
            {
                _root = tree._root;
                _stack = [];
                _listStack = [];
            }

            public TNode Current
            {
                get
                {
                    if (_stack.TryPeek(out TNode? node)) {
                        return node;
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
                if (_stack.TryPop(out TNode? cur)) { }
                _stack.Push(_root);
                return true;
            }

            public void Reset()
            {
                _stack.Clear();
                _state = 0;
            }

            object IEnumerator.Current => Current;

        }

    }
}
