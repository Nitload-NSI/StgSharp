//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="HuffmanTree.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Linq;

namespace StgSharp.Data
{
    public partial class HuffmanTree<T> where T : struct
    {

        HuffmanNodeList<T> leaves;

        internal HuffmanTree(HuffmanNodeList<T> nodeList)
        {
            leaves = nodeList;
        }

        public HuffmanNodeList<T> LeafList
=> leaves;

        public TreeNode<T> Root
=> leaves.Last;

        public static HuffmanTree<T> BuildHuffmanTree(List<(T, ulong)> metaData)
        {
            if (metaData.Count <= 3)
            {
                throw new Exception("Data is too little, no need to compress");
            }
            HuffmanNodeList<T> leaves = new HuffmanNodeList<T>();
            HuffmanNodeList<T> all = new HuffmanNodeList<T>();

            foreach (var item in metaData)
            {
                leaves.Add(new TreeNode<T>(item));
            }


            while (leaves.Count > 2)
            {
                TreeNode<T> node, toMove;
                if (leaves.Last._frequency < leaves.BeforeLast._frequency)
                {
                    if (leaves.BeforeLast._frequency < leaves.OverLast._frequency)
                    {
                        //第三节点最大
                        node = new TreeNode<T>(leaves.BeforeLast, leaves.Last);
                        toMove = leaves.RemoveAndGetLast; all.Add(toMove);
                        toMove = leaves.RemoveAndGetLast; all.Add(toMove);
                        leaves.Add(node);
                    }
                    else
                    {
                        //第二节点最大
                        node = (leaves.Last._frequency < leaves.OverLast._frequency) ?
                            (new TreeNode<T>(leaves.OverLast, leaves.Last)) :
                            (new TreeNode<T>(leaves.Last, leaves.OverLast));
                        toMove = leaves.RemoveAndGetLast; all.Add(toMove);
                        toMove = leaves.RemoveAndGetBeforeLast; all.Add(toMove);
                        leaves.Add(node);
                    }
                }
                else
                {

                    if (leaves.Last._frequency < leaves.OverLast._frequency)
                    {
                        //第三节点最大
                        node = new TreeNode<T>(leaves.Last, leaves.BeforeLast);
                        toMove = leaves.RemoveAndGetLast; all.Add(toMove);
                        toMove = leaves.RemoveAndGetLast; all.Add(toMove);
                        leaves.Add(node);
                    }
                    else
                    {
                        //第一节点最大
                        node = (leaves.BeforeLast._frequency < leaves.OverLast._frequency) ?
                            (new TreeNode<T>(leaves.OverLast, leaves.BeforeLast)) :
                            (new TreeNode<T>(leaves.BeforeLast, leaves.OverLast));
                        toMove = leaves.RemoveAndGetBeforeLast; all.Add(toMove);
                        toMove = leaves.RemoveAndGetBeforeLast; all.Add(toMove);
                        leaves.Add(node);
                    }
                }

                leaves = new HuffmanNodeList<T>(leaves.OrderByDescending(pair => pair.frequency)
                    .Select(pair => pair).ToList());
            }

            TreeNode<T> lastOne = leaves[0];
            TreeNode<T> lastTwo = leaves[1];
            TreeNode<T> root = (lastOne.frequency > lastTwo.frequency) ?
                (new TreeNode<T>(lastOne, lastTwo)) :
                (new TreeNode<T>(lastTwo, lastOne));

            all.Add(lastOne);
            all.Add(lastTwo);
            all.Add(root);

            leaves = all;
            return new HuffmanTree<T>(all);
        }


    }

    public class HuffmanNodeList<T> : List<TreeNode<T>> where T : struct
    {

        public HuffmanNodeList() : base()
        {

        }

        public HuffmanNodeList(IEnumerable<TreeNode<T>> values) : base(values)
        { }

        public TreeNode<T> BeforeLast
=> this[Count - 2];

        public TreeNode<T> Last
=> this[Count - 1];

        public TreeNode<T> OverLast
=> this[Count - 3];

        public TreeNode<T> RemoveAndGetBeforeLast
        {
            get
            {
                TreeNode<T> ret = this[Count - 2];
                RemoveAt(Count - 2);
                return ret;
            }
        }

        public TreeNode<T> RemoveAndGetLast
        {
            get
            {
                TreeNode<T> ret = this[Count - 1];
                RemoveAt(Count - 1);
                return ret;
            }
        }

        public TreeNode<T> RemoveAndGetOverLast
        {
            get
            {
                TreeNode<T> ret = this[Count - 3];
                RemoveAt(Count - 3);
                return ret;
            }
        }

        public void Add(T element, ulong f)
        {
            base.Add(new TreeNode<T>(element, f));
        }

    }

    public class TreeNode<T> where T : struct
    {

        internal ulong _frequency;
        internal TreeNode<T> _left;
        internal TreeNode<T> _right;
        internal T? _value;

        internal TreeNode(TreeNode<T> left, TreeNode<T> right)
        {
            _left = left;
            _right = right;
            _value = null;
            _frequency = left._frequency + right._frequency;
        }

        public TreeNode((T, ulong) data)
        {
            _value = data.Item1;
            _frequency = data.Item2;

        }

        public TreeNode(T? value, ulong frequncy)
        {
            _value = value;
            _frequency = frequncy;

        }

        public ulong frequency => _frequency;

        public TreeNode<T> Left => _left;
        public TreeNode<T> Right => _right;

        public T? Value => _value;

    }
}
