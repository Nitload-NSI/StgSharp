// -----------------------------------------------------------------------------
// file="TextureGL"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

namespace Nitload.Graphics.OpenGL
{
    /// <summary>A set of OpenGL texture handles.</summary>
    public sealed record class TextureGL(
        GlHandle[] Handles
    )
    {

        public int Count => Handles.Length;

        public GlHandle this[int index] => Handles[index];

    }

    public readonly record struct TextureProperty(
        TextureFilter FilterOnMagnify,
        TextureFilter FilterOnMinify,
        TextureWrap WrapOnHorizontal,
        TextureWrap WrapOnVertical
    );

    #pragma warning disable CA1008
    public enum TextureFilter
    #pragma warning restore CA1008
    {

        Nearest = glConst.NEAREST,
        Linear = glConst.LINEAR,
        NearestLinearMipmap = glConst.NEAREST_MIPMAP_LINEAR,
        LinearLinearMipmap = glConst.LINEAR_MIPMAP_LINEAR,
        NearestNearestMipmap = glConst.NEAREST_MIPMAP_NEAREST,
        LinearNearestMipmap = glConst.LINEAR_MIPMAP_NEAREST,

    }

    #pragma warning disable CA1008
    public enum TextureWrap
    #pragma warning restore CA1008
    {

        Repeat = glConst.REPEAT,
        MirroredRepeat = glConst.MIRRORED_REPEAT,
        ClampToEdge = glConst.CLAMP_TO_EDGE,
        ClampToBorder = glConst.CLAMP_TO_BORDER,

    }
}
