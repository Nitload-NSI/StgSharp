// -----------------------------------------------------------------------------
// file="TextRegexSourceGen.ExitLabel"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitload.RegularAnalysis.Text
{
    internal partial class TextRegexSourceGen
    {

        internal enum ExitKind
        {

            MatchFailed,
            MatchEnd,
            CountRetry,
            CountResume,
            AltNext,
            AltMatched,

        }

        internal readonly record struct ExitLabel
        {

            public ExitLabel(
                   ExitKind kind,
                   int order
            )
            {
                Kind = kind;
                LabelOrder = order;
            }

            public ExitKind Kind { get; init; }

            public int LabelOrder { get; init; }

            public string GetLabelName()
            {
                return Kind switch
                {
                    ExitKind.MatchFailed => $@"__match_failed_{LabelOrder}",
                    ExitKind.MatchEnd => $@"__match_end_{LabelOrder}",
                    ExitKind.CountRetry => $@"__count_retry_{LabelOrder}",
                    ExitKind.CountResume => $@"__count_resume_{LabelOrder}",
                    ExitKind.AltNext => $@"__alt_next_{LabelOrder}",
                    ExitKind.AltMatched => $@"__alt_matched_{LabelOrder}",
                    _ => throw new NotImplementedException(),
                };
            }

        }

    }
}
