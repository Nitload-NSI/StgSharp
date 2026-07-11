// -----------------------------------------------------------------------------
// file="IUnmanagedVector"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public interface IUnmanagedVector<TSelf> where TSelf : unmanaged, IUnmanagedVector<TSelf>
    {

        public static abstract TSelf Zero { get; }

        public static abstract TSelf One { get; }

    }
}
