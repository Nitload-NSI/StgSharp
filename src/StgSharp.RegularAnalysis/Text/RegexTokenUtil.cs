// -----------------------------------------------------------------------------
// file="RegexTokenUtil"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Text
{
    internal static class RegexTokenUtil
    {

        public static bool IsAccurateChar(
                           this Token<RegexElementLabel> token
        )
        {
            return ((token.Flag & RegexElementLabel.UNIT) != 0 && token.Value.Length == 1) ||
                   (token.Flag & RegexElementLabel.UNIT_SPAN) != 0;
        }

    }
}
