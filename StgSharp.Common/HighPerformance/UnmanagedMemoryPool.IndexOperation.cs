//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="UnmanagedMemoryPool.IndexOperation.cs"
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
using System.Runtime.CompilerServices;

namespace StgSharp.HighPerformance
{
    public unsafe partial class UnmanagedMemoryPool
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ConnectToNodeAtLevel(
                     IndexNode previous,
                     IndexNode current,
                     int level,
                     bool isHead)
        {
            if (isHead)
            {
                _entrance[level] = current.Position;
                current.Previous[level] = 0;
            } else
            {
                previous.Next[level] = current.Previous[level] = (uint)(current.Position - previous.Position);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IndexNode GetNode(nuint pos)
        {
            ref byte span = ref *(_buffer + pos);
            IndexNode node = new(ref span)
            {
                Position = pos
            };
            return node;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsNodeBeginning(nuint pos, int level)
        {
            return _entrance[level] == pos;
        }

        private bool Rebase(IndexNode n, uint offset)
        {
            nuint pos = n.Position;
            uint size = n.Size;
            if (size <= offset + 64)
            {
                for (int i = 0; i <= n.Level; i++) {
                    RemoveLevelHead(n, i);
                }
                return false;
            }
            uint
                newOffset = ((offset / 64) + 1) * 64,
                remain = n.Size - newOffset,
                c64 = remain % 4,
                c256 = remain / 4,
                c1024 = c256 / 4,
                c4096 = c1024 / 4;
            c256 %= 4;
            c1024 %= 4;
            IndexNode
                /*
                 * level is not important here, higher level will get 0 Size,
                 * and ignored in coming logic
                 */
                p64 = GetNode(n.Position - n.Previous[1]),
                p256 = GetNode(n.Position - n.Previous[2]),
                p1024 = GetNode(n.Position - n.Previous[3]),
                p4096 = GetNode(n.Position - n.Previous[4]),
                n64 = GetNode(n.Position + n.Previous[1]),
                n256 = GetNode(n.Position + n.Previous[2]),
                n1024 = GetNode(n.Position + n.Previous[3]),
                n4096 = GetNode(n.Position + n.Previous[4]);
            IndexNode nTemp;
            if (c64 != 0)
            {
                // set param to separated 64 bytes block and set next and previous offset to it.
                size = c64 * 64;
                pos += size;
                nTemp = GetNode(pos);
                nTemp.Position = pos;
                nTemp.Size = size;
                ConnectToNodeAtLevel(p64, nTemp, 1, n.Position == _entrance[1]);
                p64 = nTemp;
            }
            if (c256 != 0)
            {
                size = c256 * 256;
                pos += size;
                nTemp = GetNode(pos);
                nTemp.Position = pos;
                nTemp.Size = size;
                nTemp.Previous[2] = n.Previous[2] + size;
                ConnectToNodeAtLevel(p64, nTemp, 1, p64.Position == _entrance[1]);
                ConnectToNodeAtLevel(p256, nTemp, 2, n.Position == _entrance[2]);
                p64 = p256 = nTemp;
            }
            if (c1024 != 0)
            {
                size = c1024 * 1024;
                pos += size;
                nTemp = GetNode(pos);
                nTemp.Position = pos;
                nTemp.Size = size;
                nTemp.Previous[2] = n.Previous[2] + size;

                // connect to previous node
                ConnectToNodeAtLevel(p64, nTemp, 1, p64.Position == _entrance[1]);
                ConnectToNodeAtLevel(p256, nTemp, 2, p256.Position == _entrance[2]);
                ConnectToNodeAtLevel(p1024, nTemp, 3, n.Position == _entrance[3]);
                p64 = p256 = p1024 = nTemp;
            }
            if (c4096 != 0)
            {
                size = c1024 * 1024;
                pos += size;
                nTemp = GetNode(pos);
                nTemp.Position = pos;
                nTemp.Size = size;
                nTemp.Previous[2] = n.Previous[2] + size;

                // connect to previous node
                ConnectToNodeAtLevel(p64, nTemp, 1, p64.Position == _entrance[1]);
                ConnectToNodeAtLevel(p256, nTemp, 2, p256.Position == _entrance[2]);
                ConnectToNodeAtLevel(p1024, nTemp, 3, p1024.Position == _entrance[3]);
                ConnectToNodeAtLevel(p4096, nTemp, 4, n.Position == _entrance[4]);
                p64 = p256 = p1024 = p4096 = nTemp;
            }

            ConnectToNodeAtLevel(p64, n64, 1, false);
            ConnectToNodeAtLevel(
                p256, n256, 2, _entrance[2] == n.Position && n256.Position == n.Position);
            ConnectToNodeAtLevel(
                p1024, n1024, 3, _entrance[3] == n.Position && n1024.Position == n.Position);
            ConnectToNodeAtLevel(
                p4096, n4096, 4, _entrance[4] == n.Position && n4096.Position == n.Position);
            return true;
        }

        private void RemoveLevelHead(IndexNode n, int level)
        {
            IndexNode nxt, pre;
            if (n.Previous[level] == 0)
            {
                // head of usable LEVEL;
                nxt = GetNode(n.Position + n.Next[level]);
                _entrance[level] = nxt.Position;
            }
            pre = GetNode(n.Previous[level]);
            nxt = GetNode(n.Next[level]);
            nxt.Previous[level] = nxt.Previous[level] + n.Previous[level];
            pre.Next[level] = pre.Next[level] + n.Next[level];
        }

    }
}
