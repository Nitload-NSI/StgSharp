//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="RegexIRGenerator"
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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Text
{
    internal class RegexIRGenerator
    {

        private Node _head = new();
        private Node _tail;

        public RegexIRGenerator()
        {
            _tail = _head;
        }

        public int Count { get; private set; }

        public void EmitCondition(
                    int success,
                    int fail
        )
        {
            _tail.Add(new RegexConditionIR(success, fail));
            Count++;
        }

        public void EmitConfigFail(
                    int fail
        )
        {
            _tail.Add(new RegexConfigFailIR(fail));
            Count++;
        }

        public void EmitCount(
                    int min,
                    int max,
                    bool isGreedy,
                    int operation
        )
        {
            _tail.Add(new RegexCountIR(min, max, isGreedy, operation));
            Count++;
        }

        public void EmitCountSeq(
                    int min,
                    int max,
                    bool isGreedy,
                    string seq
        )
        {
            _tail.Add(new RegexCountSeqIR(min, max, isGreedy, seq));
            Count++;
        }

        public void EmitCountSet(
                    int min,
                    int max,
                    bool isGreedy,
                    string set
        )
        {
            _tail.Add(new RegexCountSetIR(min, max, isGreedy, set));
            Count++;
        }

        public void EmitIRStream(
                    RegexIRGenerator stream
        )
        {
            if (stream is null || stream.Count == 0) {
                return;
            }
            _tail.Next = stream._head;
            _tail = stream._tail;
            Count += stream.Count;
        }

        public void EmitMatchSeq(
                    string unit
        )
        {
            _tail.Add(new RegexMatchSeqIR(unit));
            Count++;
        }

        public void EmitMatchSet(
                    string unit
        )
        {
            _tail.Add(new RegexMatchSetIR(unit));
            Count++;
        }

        public void EmitPop(
                    bool isSuccess
        )
        {
            _tail.Add(new RegexPopIR(isSuccess));
            Count++;
        }

        public void EmitTry(
                    int count
        )
        {
            _tail.Add(new RegexTryIR(count));
            Count++;
        }

        public List<RegexIR> ToList()
        {
            List<RegexIR> result = new(Count);
            Node current = _head;
            while (current != null)
            {
                result.AddRange(current.IRList);
                current = current.Next!;
            }
            return result;
        }

        private class Node
        {

            public Node? Next { get; set; }

            public int Count => IRList.Count;

            public List<RegexIR> IRList { get; } = [];

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Add(
                        RegexIR ir
            )
            {
                IRList.Add(ir);
            }

        }

    }
}
