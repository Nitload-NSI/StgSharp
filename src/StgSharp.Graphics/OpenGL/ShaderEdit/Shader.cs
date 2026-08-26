// -----------------------------------------------------------------------------
// file="Shader"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;

namespace StgSharp.Graphics.OpenGL
{
    /// <summary>
    ///   A set of OpenGL shader handles with their common stage type.
    /// </summary>
    public sealed record Shader
    {

        public Shader(
               GlHandle[] handles,
               ShaderType type
        )
        {
            ArgumentNullException.ThrowIfNull(handles);
            Handles = handles;
            Type = type;
        }

        public int Count => Handles.Length;

        public GlHandle[] Handles { get; }

        public ShaderType Type { get; }

        public GlHandle this[int index] => Handles[index];

    }

    public enum ShaderStatus : int
    {

        CompileStatus = glConst.COMPILE_STATUS,

    }

    /// <summary>
    ///   An OpenGL shader-program handle.
    /// </summary>
    public readonly record struct ShaderProgram(GlHandle Handle);
}
