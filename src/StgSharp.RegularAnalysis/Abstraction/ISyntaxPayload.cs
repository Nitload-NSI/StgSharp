// -----------------------------------------------------------------------------
// file="ISyntaxPayload"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Abstraction
{
    public interface ISyntaxPayload<TLabel> where TLabel : unmanaged
    {

        int Column { get; }

        int Row { get; }

        string Source { get; }

        TLabel Flag { get; }

    }
}
