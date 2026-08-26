// -----------------------------------------------------------------------------
// file="OpenGLFunction.Uniform"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics.Graphics;

using System;
using System.Runtime.CompilerServices;

namespace StgSharp.Graphics.OpenGL
{
    public unsafe partial class OpenGLFunction
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetUniformValue(
                    Uniform<float, float, float, float> uniform,
                    Vec4 vec
        )
        {
            ArgumentNullException.ThrowIfNull(uniform);
            Context.glUniform4fv(uniform.id.SignedValue, 1, (float*)&vec);
        }

    }
}
