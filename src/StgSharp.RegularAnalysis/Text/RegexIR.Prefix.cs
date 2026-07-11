// -----------------------------------------------------------------------------
// file="RegexIR.Prefix"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Text
{
    internal enum RegexIRPrefixMask
    {

        Prev_Success = 1 << 0,
        Prev_Fail = 1 << 1,
        Always = 1 << 2,
        ExeCondition = Prev_Fail | Prev_Success | Always,
        Pop_Fail = 1 << 3,
        Pop_Success = 1 << 4,
        PopCondition = Pop_Success | Pop_Fail,
        Observe_Line = 1 << 5,
        Observe_Region = 1 << 6,
        ObserveMethod = Observe_Line | Observe_Region

    }

    internal record struct RegexIRPrefix(int WorkingRegion, RegexIRPrefixMask Mask, int ObserveSource)
    {

        public override string ToString()
        {
            return $"{WorkingRegion:D4}, {DisplayMask(Mask , RegexIRPrefixMask.ExeCondition)}, {DisplayMask(Mask, RegexIRPrefixMask.PopCondition)}, {DisplayMask(Mask,RegexIRPrefixMask.ObserveMethod)}, {ObserveSource:D4}";
        }

        private static string DisplayMask(RegexIRPrefixMask mask, RegexIRPrefixMask range)
        {
            return range switch
            {
                RegexIRPrefixMask.ExeCondition => ((int)(mask & RegexIRPrefixMask.ExeCondition) >> 0) switch
                {
                    0 => "DEF ",
                    (1 << 0) => "Sccs",
                    (1 << 1) => "Fail",
                    (1 << 2) => "Alws",
                    _ => "????"
                },
                RegexIRPrefixMask.PopCondition => ((int)(mask & RegexIRPrefixMask.PopCondition) >> 3) switch
                {
                    0 => "DEF ",
                    (1 << 0) => "Fail",
                    (1 << 1) => "Sccs",
                    _ => "????"
                },
                RegexIRPrefixMask.ObserveMethod => ((int)(mask & RegexIRPrefixMask.ObserveMethod) >> 5) switch
                {
                    0 => "NONE",
                    (1 << 0) => "LINE",
                    (1 << 1) => "REGI",
                    _ => "????"
                },
                _ => throw new ArgumentOutOfRangeException(nameof(range), range, null)
            };
        }

    }
}
