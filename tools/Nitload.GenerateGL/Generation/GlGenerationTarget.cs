// -----------------------------------------------------------------------------
// file="GlGenerationTarget"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;

namespace StgSharp.GenerateGL.Generation
{
    [Flags]
    internal enum GlGenerationTarget
    {
        None = 0,
        Api = 1,
        Enum = 2,
        All = Api | Enum,
    }
}
