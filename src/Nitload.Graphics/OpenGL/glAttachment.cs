// -----------------------------------------------------------------------------
// file="glConst"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Drawing;

namespace Nitload.Graphics.OpenGL
{
    // rename types in style of glConst

    public static class glAttachment
    {

        public const uint Depth = glConst.DEPTH_ATTACHMENT;
        public const uint DepthAndStencil = glConst.DEPTH_STENCIL_ATTACHMENT;
        public const uint Stencil = glConst.STENCIL_ATTACHMENT;

        public static uint Color(
                           int i
        )
        {
            ArgumentOutOfRangeException.ThrowIfNegative(i);
            return (uint)(glConst.COLOR_ATTACHMENT0 + i);
        }

    }
}
