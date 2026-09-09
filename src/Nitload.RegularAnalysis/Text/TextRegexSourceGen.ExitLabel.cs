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

        internal enum ExitMode
        {

            Break,
            Continue,
            Goto,

        }

        internal readonly record struct ExitLabel
        {

            public ExitLabel(
                   ExitMode mode,
                   int order
            )
            {
                Mode = mode;
                LabelOrder = order;
            }

            public ExitMode Mode { get; init; }

            public int LabelOrder { get; init; }

            public string GetLabelName()
            {
                return Mode switch
                {
                    ExitMode.Break => $@"__break_{LabelOrder}",
                    ExitMode.Continue => $@"__continue_{LabelOrder}",
                    ExitMode.Goto => $@"__goto_{LabelOrder}",
                    _ => throw new NotImplementedException(),
                };
            }

        }

    }
}
