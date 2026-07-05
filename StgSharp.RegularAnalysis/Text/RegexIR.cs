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

        Match_Seq = 1 << 0,
        Match_Set = 1 << 1,
        Condition = 1 << 2,
        Try = 1 << 3,
        Count_Complex = 1 << 4,
        Count_Seq = 1 << 5,
        Count_Set = 1 << 6,
        Pop = 1 << 7,
        Pack = 1 << 8,
        Find_Seq = 1 << 9,
        Find_Set = 1 << 10,
        Find_Complex = 1 << 11,

        Match = Match_Seq | Match_Set,
        Count = Count_Seq | Count_Set | Count_Complex,

    }

    internal enum RegexIRPrefix
    {

        Prev_Success = 1 << 0,
        Prev_Fail = 1 << 1,
        Always = 1 << 2,
        ExeCondition = Prev_Fail | Prev_Success | Always,
        Pop_Fail = 1 << 3,
        Pop_Success = 1 << 4,
        Pop_Condition = Pop_Success | Pop_Fail,

    }

    internal interface IRegexComplex
    {

        bool IsFullyOptimized { get; set; }

        int CommandCount { get; set; }

    }

    internal interface IRegexCount
    {

        bool IsGreedy { get; }

        bool IsStar { get; }

        bool IsPlus { get; }

        int Min { get; }

        int Max { get; }

    }

    internal sealed class RegexFindComplexIR(bool nearest, int count) : RegexIR, IRegexComplex
    {

        public bool IsFullyOptimized { get; set; }

        public bool IsNearest { get; } = nearest;

        public int CommandCount { get; set; } = count;

        internal override RegexIRCommand Command => RegexIRCommand.Find_Complex;

        internal override string AsCommand(int lineNum)
        {
            return $"[{lineNum}]\t FIND_COMPLEX {(IsNearest ? "NEAREST" : "FURTHEST")} COUNT: {CommandCount}";
        }

    }

    internal abstract class RegexIR
    {

        internal abstract RegexIRCommand Command { get; }

        internal abstract string AsCommand(int lineNum);

    }

    internal sealed class RegexFindSeqIR(string pattern, bool isFirst) : RegexIR
    {

        public bool IsFirst { get; } = isFirst;

        public string Pattern { get; } = pattern;

        internal override RegexIRCommand Command => RegexIRCommand.Find_Seq;

        internal override string AsCommand(int lineNum)
        {
            return $"[{lineNum}]\t FIND_SEQ \"{(string.IsNullOrEmpty(Pattern) ? "<EPSILON>" : Pattern)}\"";
        }

    }

    /// <summary>
    ///   Searches for a certain string pattern or charset from current position.
    /// </summary>
    internal sealed class RegexMatchSeqIR(string pattern) : RegexIR
    {

        public string Pattern { get; } = pattern;

        internal override RegexIRCommand Command => RegexIRCommand.Match_Seq;

        internal override string AsCommand(int lineNum)
        {
            return $"[{lineNum}]\t MATCH_SEQ \"{(string.IsNullOrEmpty(Pattern) ? "<EPSILON>" : Pattern)}\"";
        }

    }

    internal sealed class RegexMatchSetIR(string charset) : RegexIR
    {

        public string Charset { get; } = charset;

        internal override RegexIRCommand Command => RegexIRCommand.Match_Set;

        internal override string AsCommand(int lineNum)
        {
            return $"[{lineNum}]\t MATCH_SET \"{Charset}\"";
        }

    }

    internal sealed class RegexConditionIR(int success, int fail) : RegexIR
    {

        public int SuccessCase { get; } = success;

        public int FailCase { get; } = fail;

        internal override RegexIRCommand Command => RegexIRCommand.Condition;

        internal override string AsCommand(int lineNum)
        {
            return $"[{lineNum}]\t CONDITION SUCCESS: {SuccessCase} FAIL: {FailCase}";
        }

    }

    internal sealed class RegexPackIR(bool condition, string name = "") : RegexIR
    {

        public bool Condition { get; } = condition;

        public string GroupName { get; } = name;

        internal override RegexIRCommand Command => RegexIRCommand.Pack;

        internal override string AsCommand(int lineNum)
        {
            return $"[{lineNum}]\t PACK {(Condition ? "SUCCESS" : "FAIL")} GROUP: \"{GroupName}\"";
        }

    }

    internal sealed class RegexCountComplexIR(int min, int max, bool isGreedy, int operation) : RegexIR, IRegexCount, IRegexComplex
    {

        public bool IsGreedy { get; } = isGreedy;

        public bool IsStar => Min == 0 && Max < 0;

        public bool IsPlus => Min == 1 && Max < 0;

        public bool IsFullyOptimized { get; set; }

        public int Min { get; } = min;

        public int Max { get; } = max;

        public int CommandCount { get; set; } = operation;

        internal override RegexIRCommand Command { get; } = RegexIRCommand.Count_Complex;

        internal override string AsCommand(int lineNum)
        {
            return $"[{lineNum}]\t COUNT {(IsGreedy ? "GREEDY" : "NON_GREEDY")} MIN: {Min} MAX: {Max} OPERATION: {CommandCount}";
        }

    }

    internal sealed class RegexCountSeqIR(int min, int max, bool isGreedy, string seq) : RegexIR, IRegexCount
    {

        public bool IsGreedy { get; } = isGreedy;

        public bool IsStar => Min == 0 && Max < 0;

        public bool IsPlus => Min == 1 && Max < 0;

        public int Min { get; } = min;

        public int Max { get; } = max;

        public string Sequence { get; } = seq.Length == 2 && seq[0] == '\\' ? seq[1..2] : seq;

        internal override RegexIRCommand Command { get; } = RegexIRCommand.Count_Seq;

        internal override string AsCommand(int lineNum)
        {
            return $"[{lineNum}]\t COUNT_SEQ {(IsGreedy ? "GREEDY" : "NON_GREEDY")} MIN: {Min} MAX: {Max} SEQ: \"{Sequence}\"";
        }

    }

    internal sealed class RegexCountSetIR(int min, int max, bool isGreedy, string set) : RegexIR, IRegexCount
    {

        public bool IsGreedy { get; } = isGreedy;

        public bool IsDotStar => CharSet == "." && Min == 0 && Max < 0;

        public bool IsStar => Min == 0 && Max < 0;

        public bool IsPlus => Min == 1 && Max < 0;

        public int Min { get; } = min;

        public int Max { get; } = max;

        public string CharSet { get; } = set;

        internal override RegexIRCommand Command => RegexIRCommand.Count_Set;

        internal override string AsCommand(int lineNum)
        {
            return $"[{lineNum}]\t COUNT_SET {(IsGreedy ? "GREEDY" : "NON_GREEDY")} MIN: {Min} MAX: {Max} SET: \"{CharSet}\"";
        }

    }

    internal sealed class RegexTryIR(int count) : RegexIR, IRegexComplex
    {

        public bool IsFullyOptimized { get; set; }

        public int CommandCount { get; set; } = count;

        internal override RegexIRCommand Command => RegexIRCommand.Try;

        internal override string AsCommand(int lineNum)
        {
            return $"[{lineNum}]\t TRY {CommandCount}";
        }

    }

    internal sealed class RegexPopIR(bool isSuccess) : RegexIR
    {

        public bool IsSuccess { get; } = isSuccess;

        internal override RegexIRCommand Command => RegexIRCommand.Pop;

        internal override string AsCommand(int lineNum)
        {
            return $"[{lineNum}]\t POP {(IsSuccess ? "SUCCESS" : "FAIL")}";
        }

    }
}
