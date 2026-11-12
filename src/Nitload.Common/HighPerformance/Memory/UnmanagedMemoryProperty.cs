// -----------------------------------------------------------------------------
// file="UnmanagedMemoryProperty"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitload.Common.HighPerformance.Memory
{
    public record struct UnmanagedMemoryProperty(
                         nuint Address,
                         ulong Size,
                         int Alignment,
                         int BindedNuma
    );
}
