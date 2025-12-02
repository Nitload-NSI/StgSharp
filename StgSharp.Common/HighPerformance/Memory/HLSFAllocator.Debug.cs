//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="HLSFAllocator.Debug"
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.Memory
{
    public sealed class HLSFPositionChainBreakException : Exception
    {

        public HLSFPositionChainBreakException()
            : base("Position chain of an HLSF allocator is broken") { }

    }

    public unsafe partial class HybridLayerSegregatedFitAllocator
    {

        private object _lock = new object();

        public void DisplayEntryLayout(bool singleStep)
        {
            lock (_lock)
            {
                Entry* cur = _spareMemory;
                while (cur != null)
                {
                    Console.WriteLine(EntryToString(cur));
                    cur = cur->PreviousNear;
                    if (singleStep) {
                        Console.ReadKey();
                    }
                }
            }
        }

        public void DumpPositionChain(bool singleStep = false, int maxCount = 4096)
        {
            lock (_lock)
            {
                try
                {
                    Entry* cur = _spareMemory;
                    int cnt = 0;
                    if (cur != null) {
                        cur = cur->NextNear;
                    }
                    Console.WriteLine(EntryToString(_spareMemory));
                    while (cur != null && cur != _spareMemory && cnt < maxCount)
                    {
                        Console.WriteLine(EntryToString(cur));
                        cnt++;
                        if (singleStep) {
                            Console.ReadKey();
                        }

                        cur = cur->NextNear;
                    }
                    if (cur == _spareMemory)
                    {
                        Console.WriteLine("(reached sentinel)");
                    } else if (cnt >= maxCount) {
                        Console.WriteLine("(stopped at maxCount)");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DumpPositionChain-forward] Exception: {ex.Message}");
                }
            }
        }

        public IEnumerable<string> ShowEntryLayout()
        {
            lock (_lock)
            {
                Stack<string> layout = [];
                Entry* cur = _spareMemory;
                while (cur != null)
                {
                    layout.Push(EntryToString(cur));
                    cur = cur->PreviousNear;
                }
                return layout;
            }
        }

        private static string EntryToString(Entry* e)
        {
            BucketNode* b = (BucketNode*)e->Position;
            return $"{(ulong)e->PreviousNear}<-prev- Position:{(ulong)e} Bucket:{{{(ulong)e->Position},{e->Level},{e->Size}}} -next->{(ulong)e->NextNear}";
        }

    }
}