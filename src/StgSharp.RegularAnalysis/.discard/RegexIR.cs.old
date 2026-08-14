// -----------------------------------------------------------------------------
// file="RegexIR"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp.Syntax;
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
        Find = Find_Seq | Find_Set | Find_Complex,

    }

    internal sealed class RegexFindComplexIR(
                          bool nearest,
                          int count
    ) : RegexIR(RegexIRCommand.Find_Complex), IRegexComplex
    {

        public bool IsFullyOptimized { get; set; }

        public bool IsNearest { get; } = nearest;

        public int CommandCount { get; set; } = count;

        protected override string AsCommand()
        {
            return $"FIND_COMPLEX {(IsNearest ? "NEAREST" : "FURTHEST")} COUNT: {CommandCount}";
        }

    }

    internal sealed class RegexFindSeqIR(
                          string pattern,
                          bool isFirst
    ) : RegexIR(RegexIRCommand.Find_Seq), IRegexSequence
    {

        public bool IsFirst { get; } = isFirst;

        public string Pattern { get; } = pattern;

        protected override string AsCommand()
        {
            return $"FIND_SEQ \"{(string.IsNullOrEmpty(Pattern) ? "<EPSILON>" : Pattern)}\"";
        }

    }

    internal interface IRegexMatch { }

    /// <summary>
    ///   Searches for a certain string pattern or charset from current position.
    /// </summary>
    internal sealed class RegexMatchSeqIR(
                          string pattern
    ) : RegexIR(RegexIRCommand.Match_Seq), IRegexSequence, IRegexMatch
    {

        public string Pattern { get; } = pattern;

        protected override string AsCommand()
        {
            return $"MATCH_SEQ \"{(string.IsNullOrEmpty(Pattern) ? "<EPSILON>" : Pattern)}\"";
        }

    }

    internal sealed class RegexMatchSetIR(
                          string charset
    ) : RegexIR(RegexIRCommand.Match_Set), IRegexMatch
    {

        public string Charset { get; } = charset;

        protected override string AsCommand()
        {
            return $"MATCH_SET \"{Charset}\"";
        }

    }

    internal sealed class RegexConditionIR(
                          int success,
                          int fail
    ) : RegexIR(RegexIRCommand.Condition)
    {

        public int SuccessCase { get; } = success;

        public int FailCase { get; } = fail;

        protected override string AsCommand()
        {
            return $"CONDITION SUCCESS: {SuccessCase} FAIL: {FailCase}";
        }

    }

    internal sealed class RegexPackIR(
                          bool condition,
                          string name = ""
    ) : RegexIR(RegexIRCommand.Pack)
    {

        public bool Condition { get; } = condition;

        public string GroupName { get; } = name;

        protected override string AsCommand()
        {
            return $"PACK {(Condition ? "SUCCESS" : "FAIL")} GROUP: \"{GroupName}\"";
        }

    }

    internal sealed class RegexCountComplexIR(
                          int min,
                          int max,
                          bool isGreedy,
                          int operation
    ) : RegexIR(RegexIRCommand.Count_Complex), IRegexCount, IRegexComplex
    {

        public bool IsGreedy { get; } = isGreedy;

        public bool IsStar => Min == 0 && Max < 0;

        public bool IsPlus => Min == 1 && Max < 0;

        public bool IsFullyOptimized { get; set; }

        public int Min { get; } = min;

        public int Max { get; } = max;

        public int CommandCount { get; set; } = operation;

        protected override string AsCommand()
        {
            return $"COUNT {(IsGreedy ? "GREEDY" : "NON_GREEDY")} MIN: {Min} MAX: {Max} OPERATION: {CommandCount}";
        }

    }

    internal sealed class RegexCountSeqIR(
                          int min,
                          int max,
                          bool isGreedy,
                          string seq
    ) : RegexIR(RegexIRCommand.Count_Seq), IRegexCount, IRegexSequence
    {

        public bool IsGreedy { get; } = isGreedy;

        public bool IsStar => Min == 0 && Max < 0;

        public bool IsPlus => Min == 1 && Max < 0;

        public int Min { get; } = min;

        public int Max { get; } = max;

        public string Pattern { get; } = seq.Length == 2 && seq[0] == '\\' ? seq[1..2] : seq;

        protected override string AsCommand()
        {
            return $"COUNT_SEQ {(IsGreedy ? "GREEDY" : "NON_GREEDY")} MIN: {Min} MAX: {Max} SEQ: \"{Pattern}\"";
        }

    }

    internal sealed class RegexCountSetIR(
                          int min,
                          int max,
                          bool isGreedy,
                          string set
    ) : RegexIR(RegexIRCommand.Count_Set), IRegexCount
    {

        public bool IsGreedy { get; } = isGreedy;

        public bool IsDotStar => CharSet == "." && Min == 0 && Max < 0;

        public bool IsStar => Min == 0 && Max < 0;

        public bool IsPlus => Min == 1 && Max < 0;

        public int Min { get; } = min;

        public int Max { get; } = max;

        public string CharSet { get; } = set;

        protected override string AsCommand()
        {
            return $"COUNT_SET {(IsGreedy ? "GREEDY" : "NON_GREEDY")} MIN: {Min} MAX: {Max} SET: \"{CharSet}\"";
        }

    }

    internal sealed class RegexTryIR(
                          int count
    ) : RegexIR(RegexIRCommand.Try), IRegexComplex, IRegexMatch
    {

        public bool IsFullyOptimized { get; set; }

        public int CommandCount { get; set; } = count;

        protected override string AsCommand()
        {
            return $"TRY {CommandCount}";
        }

    }

    internal sealed class RegexFindSetIR(
                          string charset,
                          bool isFirst
    ) : RegexIR(RegexIRCommand.Find_Set)
    {

        public bool IsFirst { get; } = isFirst;

        public string Charset { get; } = charset;

        protected override string AsCommand()
        {
            return $"FIND_SET \"{Charset}\"";
        }

    }

    internal sealed class RegexPopIR(
                          bool isSuccess
    ) : RegexIR(RegexIRCommand.Pop)
    {

        public bool IsSuccess { get; } = isSuccess;

        protected override string AsCommand()
        {
            return $"POP {(IsSuccess ? "SUCCESS" : "FAIL")}";
        }

    }
}
