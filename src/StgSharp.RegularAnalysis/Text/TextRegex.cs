// -----------------------------------------------------------------------------
// file="TextRegex"
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
    public abstract class TextRegex
    {

        public abstract MatchResult Match(
                                    ReadOnlySpan<char> text
        );

    }

    public ref struct MatchResult { }
}
