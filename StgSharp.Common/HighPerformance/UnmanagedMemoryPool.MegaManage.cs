//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="UnmanagedMemoryPool.MegaManage.cs"
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
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StgSharp.HighPerformance
{
    public unsafe partial class UnmanagedMemoryPool
    {

        private readonly LinkedList<MegaBlockHead> _megaAlloc = new();

        private readonly LinkedList<MegaBlockHead> _megaEmpty = new();
        private MegaBlockHead _megaFirst;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void InsertMegaNear(MegaBlockHead previous, MegaBlockHead current)
        {
            MegaBlockHead pp = current.PreviousNear;
            pp.NextNear = previous;
            previous.PreviousNear = pp;
            previous.NextNear = current;
            current.PreviousNear = previous;
        }

        private static void InsertMegaSorted(LinkedList<MegaBlockHead> list, MegaBlockHead newItem)
        {
            LinkedListNode<MegaBlockHead>? node = list.First;

            while (node != null && node.Value.Position < newItem.Position) {
                node = node.Next;
            }

            _ = node != null ? list.AddBefore(node, newItem) : list.AddLast(newItem);
        }

        private bool TryGetMegaBlock(nuint size, out nuint position, int align = 1)
        {
            LinkedListNode<MegaBlockHead> node = _megaEmpty.First!;
            while (node is not null)
            {
                MegaBlockHead block = node.Value;
                if (block.Size >= size + (nuint)align)
                {
                    position = block.Position;
                    nuint s = position + (nuint)(align - 1);
                    MegaBlockHead b;
                    if (block.Rebase(s))
                    {
                        b = new()
                        {
                            Position = position,
                            Size = position + (nuint)(align - 1),
                            Usage = MegaBlockUsage.MegaAlloc,
                        };
                        InsertMegaSorted(_megaAlloc, b);
                        InsertMegaNear(b, block);
                    } else
                    {
                        b = new()
                        {
                            Position = position,
                            Size = block.Size,
                            Usage = MegaBlockUsage.MegaAlloc,
                        };
                        InsertMegaSorted(_megaAlloc, b);
                        _megaEmpty.Remove(node);
                        InsertMegaNear(b, block.NextNear);
                    }
                    position = s & (nuint)~(align - 1);
                    return true;
                }
                node = node.Next!;
            }
            position = 0;
            return false;
        }

        private unsafe class MegaBlockHead
        {

            private readonly ulong[] Heads = new ulong[4];

            public MegaBlockHead NextNear { get; set; }

            public MegaBlockHead PreviousNear { get; set; }

            public MegaBlockUsage Usage { get; set; }

            public nuint Position { get; set; }

            public nuint Size { get; set; }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ref nuint DagHead(int level)
            {
                return ref Unsafe.As<ulong, nuint>(ref Heads[level]);
            }

            public bool Rebase(nuint size)
            {
                nuint s = this.Size - size;
                if (s > 1024 * 1024 * 128)//space larger than 128MB is considered a new mega block
                {
                    Position += size;
                    this.Size -= size;
                    Usage = MegaBlockUsage.EmptyDag;
                    return true;
                }
                MegaBlockHead pre = PreviousNear, nxt = NextNear;
                pre.NextNear = nxt;
                nxt.PreviousNear = pre;
                return false;
            }

        }

        private enum MegaBlockUsage
        {

            EmptyDag = 0,
            UsedDag = 1,
            MegaAlloc = 2

        }

        private MegaBlockHead

    }
}
