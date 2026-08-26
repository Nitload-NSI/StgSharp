// -----------------------------------------------------------------------------
// file="Pixels"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics
{
    public interface IPixel
    {

        public float Red { get; set; }

        public float Green { get; set; }

        public float Blue { get; set; }

        public float Alpha { get; set; }

        internal int Size { get; }

    }

    [StructLayout(LayoutKind.Explicit, Size = 32)]
    public unsafe struct Pixel32RGBA : IPixel
    {

        [FieldOffset(0)] private unsafe fixed byte data[4];
        [FieldOffset(0)] private readonly int mask;

        public readonly int Size => 4;

        public override bool Equals(
                             object obj
        )
        {
            return obj is not null && obj is Pixel32RGBA pixel && this == pixel;
        }

        public override readonly int GetHashCode()
        {
            return mask.GetHashCode();
        }

        public static bool operator !=(
                                    Pixel32RGBA left,
                                    Pixel32RGBA right
        )
        {
            return !(left == right);
        }

        public static bool operator ==(
                                    Pixel32RGBA left,
                                    Pixel32RGBA right
        )
        {
            return left.mask == right.mask;
        }

        float IPixel.Alpha
        {
            get => data[3] / 256;
            set => data[3] = (byte)(value * 256);
        }

        float IPixel.Blue
        {
            get => data[2] / 256;
            set => data[2] = (byte)(value * 256);
        }

        float IPixel.Green
        {
            get => data[1] / 256;
            set => data[1] = (byte)(value * 256);
        }

        float IPixel.Red
        {
            get => data[0] / 256;
            set => data[0] = (byte)(value * 256);
        }

    }
}
