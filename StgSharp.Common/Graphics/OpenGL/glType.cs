//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="glType"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
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