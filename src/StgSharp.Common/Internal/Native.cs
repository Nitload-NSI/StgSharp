// -----------------------------------------------------------------------------
// file="Native"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Internal
{
    internal static unsafe partial class Native
    {

        internal const string LibName =
            "StgSharp.Native";

        internal const string LogPath =
            "SS_error.log";

        internal static IntPtr _libPtr = IntPtr.Zero;

    }
}
