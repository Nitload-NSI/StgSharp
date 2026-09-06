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

namespace Nitload.Common.Internal
{
    internal static unsafe partial class Native
    {

        internal const string LibName =
            "Nitload.Native";

        internal const string LogPath =
            "SS_error.log";

        internal static IntPtr _libPtr = IntPtr.Zero;

    }
}
