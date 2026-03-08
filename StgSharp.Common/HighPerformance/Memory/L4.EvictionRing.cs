//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="L4.EvictionRing"
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.Memory
{
    public partial class L4
    {

        private EvictionRing _ring ;

        private unsafe class EvictionRing
        {

            private nint[] _bucket;

            public void AddNode(EvictionRingNode* node)
            {
                nint address = node->_address;
                int hash = (int)(address % (nint)_bucket.Length);
                nint head = _bucket[hash];
                if (head != 0)
                {
                    EvictionRingNode* headNode = (EvictionRingNode*)head;
                    headNode->_prev = node;
                    node->_next = headNode;
                }
                _bucket[hash] = address;
            }

        }

        [StructLayout(LayoutKind.Sequential)]
        private unsafe struct EvictionRingNode
        {

            public EvictionRingNode* _next;
            public EvictionRingNode* _prev;
            public int _level;
            public int _refCount;
            public nint _address;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void IncrementRefCount()
            {
                _ = Interlocked.Increment(ref _refCount);
            }

        }

    }
}
