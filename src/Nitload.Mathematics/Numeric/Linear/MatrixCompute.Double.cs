// -----------------------------------------------------------------------------
// file="MatrixCompute.Double"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Common.HighPerformance.Memory;
using Nitload.Mathematics.Numeric.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using unsafe MkProc = delegate* unmanaged[Cdecl]<Nitload.Mathematics.Numeric.Runtime.MatrixParallelTask*, void>;

namespace Nitload.Mathematics.Numeric
{
    public static unsafe partial class MatrixCompute { }
}
