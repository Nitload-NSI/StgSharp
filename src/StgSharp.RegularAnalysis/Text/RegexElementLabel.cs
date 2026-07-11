// -----------------------------------------------------------------------------
// file="RegexElementLabel"
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
    [Flags]
    public enum RegexElementLabel
    {

        NONE = 0,
        UNIT = 1,
        UNIT_SET = 2,
        UNIT_SPAN = 4,
        GROUP_BEGIN = 8,
        GROUP_END = 16,
        COUNT = 32,
        CONCAT = 64,
        ALT = 128,
        SEQUENCE = UNIT | UNIT_SPAN | UNIT_SET,
        SINGLE = UNIT | UNIT_SET,
        OPERATOR = COUNT | ALT | CONCAT,
        VAST_OPERATOR = OPERATOR | GROUP_BEGIN,
        ATOM_BEGIN = SEQUENCE | GROUP_BEGIN ,
        ATOM_END = SEQUENCE | GROUP_END | COUNT,

    }
}
