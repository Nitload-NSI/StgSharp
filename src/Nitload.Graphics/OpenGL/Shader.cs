// -----------------------------------------------------------------------------
// file="Shader"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;

namespace Nitload.Graphics.OpenGL
{
    /// <summary>
    ///   A set of OpenGL shader handles with their common stage type.
    /// </summary>
    public sealed class Shader
    {

        private readonly GlHandle[] _handles;

        public Shader(
               ReadOnlySpan<GlHandle> handles,
               ShaderType type
        )
        {
            _handles = handles.ToArray();
            Type = type;
        }

        public GlHandle this[
                        int index
        ] => _handles[index];

        public int Count => _handles.Length;

        public ReadOnlySpan<GlHandle> Handles => _handles;

        public ShaderType Type { get; }

    }

    public enum ShaderStatus : int
    {

        CompileStatus = glConst.COMPILE_STATUS,

    }

    /// <summary>
    ///   An OpenGL shader-program handle.
    /// </summary>
    public readonly record struct ShaderProgram(
                                  GlHandle Handle
    );
}
