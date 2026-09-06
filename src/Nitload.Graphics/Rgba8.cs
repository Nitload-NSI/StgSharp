// -----------------------------------------------------------------------------
// file="Rgba8"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace Nitload.Graphics
{
    /// <summary>
    ///   Four-channel, eight-bit-per-channel RGBA pixel storage.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
    public struct Rgba8
    {

        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public Rgba8(
               byte red,
               byte green,
               byte blue,
               byte alpha
        )
        {
            R = red;
            G = green;
            B = blue;
            A = alpha;
        }

    }
}
