//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="RegexIR"
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
    internal enum RegexIRCommand
    {

        Search_Seq = 0,
        Search_Set = 1,
        SearchChar = 2,
        ConfigFail = 3,
        Condition = 4,
        Try = 5,
        Count = 6,
        Count_Seq = 7,
        Count_Set = 8,
        Pop = 10

    }

    internal abstract class RegexIR
    {

        internal abstract RegexIRCommand Command { get; }

        internal abstract string AsCommand(
                                 int lineNum
        );

    }

    /// <summary>
    ///   Searches for a certain string pattern or charset from current position.
    /// </summary>
    internal sealed class RegexMatchSeqIR(
                          string pattern
    ) : RegexIR
    {

        public string Pattern { get; } = pattern;

        internal override RegexIRCommand Command => RegexIRCommand.Search_Seq;

        internal override string AsCommand(
                                 int lineNum
        )
        {
            return $"[{lineNum}]\t SEARCH_SEQ \"{(string.IsNullOrEmpty(Pattern) ? "<EPSILON>" : Pattern)}\"";
        }

    }

    internal sealed class RegexMatchSetIR(
                          string charset
    ) : RegexIR
    {

        public string Charset { get; } = charset;

        internal override RegexIRCommand Command => RegexIRCommand.Search_Set;

        internal override string AsCommand(
                                 int lineNum
        )
        {
            return $"[{lineNum}]\t SEARCH_SET \"{Charset}\"";
        }

    }

    /// <summary>
    ///   Configures the failure jump of current position. The failMod ranges 0-2
    /// </summary>
    /// <param name="target">
    ///
    /// </param>
    internal sealed class RegexConfigFailIR(
                          int failMod
    ) : RegexIR
    {

        public int FailMod { get; } = failMod & 0x3;

        internal override RegexIRCommand Command => RegexIRCommand.ConfigFail;

        internal override string AsCommand(
                                 int lineNum
        )
        {
            return $"[{lineNum}]\t CONFIG_FAIL {FailMod}";
        }

    }

    internal sealed class RegexConditionIR(
                          int success,
                          int fail
    ) : RegexIR
    {

        public int SuccessCase { get; } = success;

        public int FailCase { get; } = fail;

        internal override RegexIRCommand Command => RegexIRCommand.Condition;

        internal override string AsCommand(
                                 int lineNum
        )
        {
            return $"[{lineNum}]\t CONDITION SUCCESS: {SuccessCase} FAIL: {FailCase}";
        }

    }

    internal sealed class RegexCountIR(
                          int min,
                          int max,
                          bool isGreedy,
                          int operation
    ) : RegexIR
    {

        public bool IsGreedy { get; } = isGreedy;

        public int Min { get; } = min;

        public int Max { get; } = max;

        public int OperationIndex { get; } = operation;

        internal override RegexIRCommand Command { get; } = RegexIRCommand.Count;

        internal override string AsCommand(
                                 int lineNum
        )
        {
            return $"[{lineNum}]\t COUNT {(IsGreedy ? "GREEDY" : "NON_GREEDY")} MIN: {Min} MAX: {Max} OPERATION: {OperationIndex}";
        }

    }

    internal sealed class RegexCountSeqIR(
                          int min,
                          int max,
                          bool isGreedy,
                          string seq
    ) : RegexIR
    {

        public bool IsGreedy { get; } = isGreedy;

        public int Min { get; } = min;

        public int Max { get; } = max;

        public string Sequence { get; } = seq;

        internal override RegexIRCommand Command { get; } = RegexIRCommand.Count_Seq;

        internal override string AsCommand(
                                 int lineNum
        )
        {
            return $"[{lineNum}]\t COUNT_SEQ {(IsGreedy ? "GREEDY" : "NON_GREEDY")} MIN: {Min} MAX: {Max} SEQ: \"{Sequence}\"";
        }

    }

    internal sealed class RegexCountSetIR(
                          int min,
                          int max,
                          bool isGreedy,
                          string set
    ) : RegexIR
    {

        public bool IsGreedy { get; } = isGreedy;

        public int Min { get; } = min;

        public int Max { get; } = max;

        public string CharSet { get; } = set;

        internal override RegexIRCommand Command => RegexIRCommand.Count_Set;

        internal override string AsCommand(
                                 int lineNum
        )
        {
            return $"[{lineNum}]\t COUNT_SET {(IsGreedy ? "GREEDY" : "NON_GREEDY")} MIN: {Min} MAX: {Max} SET: \"{CharSet}\"";
        }

    }

    internal sealed class RegexTryIR(
                          int count
    ) : RegexIR
    {

        public int CountOfCommand { get; } = count;

        internal override RegexIRCommand Command => RegexIRCommand.Try;

        internal override string AsCommand(
                                 int lineNum
        )
        {
            return $"[{lineNum}]\t TRY {CountOfCommand}";
        }

    }

    internal sealed class RegexPopIR(
                          bool isSuccess
    ) : RegexIR
    {

        public bool IsSuccess { get; } = isSuccess;

        internal override RegexIRCommand Command => RegexIRCommand.Try;

        internal override string AsCommand(
                                 int lineNum
        )
        {
            return $"[{lineNum}]\t POP {(IsSuccess ? "SUCCESS" : "FAIL")}";
        }

    }
}
