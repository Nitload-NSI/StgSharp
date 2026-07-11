// -----------------------------------------------------------------------------
// file="GroupIndex"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace StgSharp.RegularAnalysis
{
    public record struct GroupIndex
    {

        public GroupIndex(
               int index,
               string name
        )
        {
            Index = index;
            Name = name;
        }

        public int Index { get; internal set; }

        public string Name { get; internal set; }

    }
}

