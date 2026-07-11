// -----------------------------------------------------------------------------
// file="MatrixCompute.Double"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.HighPerformance.Memory;
using StgSharp.Mathematics.Numeric.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using unsafe MkProc = delegate* unmanaged[Cdecl]<StgSharp.Mathematics.Numeric.Runtime.MatrixParallelTask*, void>;

namespace StgSharp.Mathematics.Numeric
{
    public static unsafe partial class MatrixCompute { }
}
