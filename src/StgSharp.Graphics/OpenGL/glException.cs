// -----------------------------------------------------------------------------
// file="glException"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;

namespace StgSharp.Graphics.OpenGL
{
    public sealed class GlErrorOverflowException : Exception
    {

        public GlErrorOverflowException(
               int errorCount,
               uint errorCode
        )
            : base(
            "Too many opengl error detected at certain position. " + $"The amount of errors exceeds {errorCount}. " + $"Code of the last error is {errorCode}") { }

    }

    public sealed class GlExecutionException : Exception
    {

        public GlExecutionException(
               uint errorCode
        )
            : base($"Critical OpenGL error: {errorCode}") { }

    }

}
