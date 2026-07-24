// -----------------------------------------------------------------------------
// file="TextRegexAttribute"
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
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TextRegexAttribute(
                        string pattern
    ) : Attribute
    {

        public string Pattern { get; } = pattern;

    }
}
