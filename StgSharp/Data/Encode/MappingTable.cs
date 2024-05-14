//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="MappingTable.cs"
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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Data
{
    public partial class HuffmanTree<T> where T : struct
    {

        public Dictionary<T, HuffmanKey<T>> BuildHuffmanDictionary()
        {
            Dictionary<T, HuffmanKey<T>> ret = new Dictionary<T, HuffmanKey<T>>();
            TreeNode<T> root = Root;
            HuffmanKey<T> key = new HuffmanKey<T>();
            key = key.SetSide(KeySide.Right);
            InternalBuildDictionary(ret, root, key);
            return ret;
        }

        public void InternalBuildDictionary(
            Dictionary<T, HuffmanKey<T>> dict,
            TreeNode<T> node,
            HuffmanKey<T> valueTemp
            )
        {
            HuffmanKey<T> nativeNode = new HuffmanKey<T>(valueTemp.bitList);
            nativeNode.AddIndexLevel();
            //检查左节点
            if (node._left.Value == null)
            {
                //左节点不是叶子节点，继续搜索
                InternalBuildDictionary(
                    dict,
                    node._left,
                    nativeNode.SetSide(KeySide.Left));
            }
            else
            {
                //左节点是叶子节点，添加值
                dict.Add(node._left.Value.Value, nativeNode.SetSide(KeySide.Left));
            }
            //检查右节点
            if (node._right.Value == null)
            {
                //右节点不是叶子节点，继续搜索
                InternalBuildDictionary(
                    dict,
                    node._right,
                    nativeNode.SetSide(KeySide.Right));
            }
            else
            {
                //右节点是叶子节点，添加值
                dict.Add(node._right.Value.Value, nativeNode.SetSide(KeySide.Right));
            }
            //这里检查自己是不是叶子节点
            //但是上面已经检查过了，不需要被检查
        }

    }



    public class HuffmanKey<T> where T : struct
    {

        internal List<bool> bitList;
        internal BitArray code;

        public HuffmanKey()
        {
            bitList = new List<bool>();
            bitList.Add(false);
        }

        public HuffmanKey(List<bool> former)
        {
            bitList = new List<bool>(former);
        }

        public bool IsSupported
        {
            get
            {
                return
                    (typeof(T) == typeof(byte)) ||
                    (typeof(T) == typeof(uint)) ||
                    (typeof(T) == typeof(ushort)) ||
                    (typeof(T) == typeof(ulong)) ||
#if NET7_0_OR_GREATER
                    typeof(T) == typeof(UInt128) ||
#endif
                    (typeof(T) == typeof(M128));
            }
        }

        public unsafe void AddIndexLevel()
        {
            bitList.Add(true);
        }

        public void Freeze()
        {
            if (code != null)
            {
                return;
            }
            bitList.RemoveAt(0);
            code = new BitArray(bitList.ToArray());
        }

        public BitArray GetBitArray()
        {
            if (code == null)
            {
                List<bool> list = new List<bool>(bitList);
                list.RemoveAt(0);
                return new BitArray(list.ToArray());
            }
            return code;
        }

        public unsafe byte[] GetBytes()
        {
            byte[] bytes = new byte[(bitList.Count / 8) + 1];
            BitArray array = new BitArray(bitList.ToArray());
            array.CopyTo(bytes, 0);
            return bytes;
        }

        public unsafe HuffmanKey<T> SetSide(KeySide side)
        {
            HuffmanKey<T> newKey = new HuffmanKey<T>(bitList);
            newKey.bitList[newKey.bitList.Count - 1] = side == KeySide.Left;
            return newKey;
        }

        public override string ToString()
        {
            string str = string.Empty;
            foreach (var bit in bitList)
            {
                str += $"{bit}   ";
            }
            return str;
        }

    }

    public enum KeySide
    {
        Left,
        Right
    }

}
