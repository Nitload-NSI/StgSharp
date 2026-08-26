// -----------------------------------------------------------------------------
// file="glType"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics.OpenGL
{
    internal delegate void GLVULKANPROCNV();

    public unsafe struct glHandleARB(
                         IntPtr value
    ) { }

    /// <summary>
    ///   OpenGL numeric type. An IEEE-754 floating-point value, clamped to the range [0,1].
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct GlClampF
    {

        public float value;

        public GlClampF(
               float a
        )
        {
            value = a;
        }

    }

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct glHalfNv(
                  Half bin
    )
    {

        [FieldOffset(0)] public Half binary = bin;

        public float ToSingle()
        {
            return (float)binary;
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct glSync
    {

        public IntPtr Handle;

    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct GlFixed
    {

        [FieldOffset(0)] public int All;
        [FieldOffset(0)] public short Decimals;
        [FieldOffset(4)] public short Integer;

    }

    /// <summary>
    ///   OpenGL numeric type. An IEEE-754 double precision floating-point value, clamped to the
    ///   range [0,1].
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct GlClampD
    {

        public double value;

        public GlClampD(
               double a
        )
        {
            value = a;
        }

    }
}