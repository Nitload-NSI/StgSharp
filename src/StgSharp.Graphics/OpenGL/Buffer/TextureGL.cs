// -----------------------------------------------------------------------------
// file="TextureGL"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

namespace StgSharp.Graphics.OpenGL
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
    public enum TextureUnit
    #pragma warning restore CA1008
    {

        Unit0 = glConst.TEXTURE0,
        Unit1 = glConst.TEXTURE1,
        Unit2 = glConst.TEXTURE2,
        Unit3 = glConst.TEXTURE3,
        Unit4 = glConst.TEXTURE4,
        Unit5 = glConst.TEXTURE5,
        Unit6 = glConst.TEXTURE6,
        Unit7 = glConst.TEXTURE7,
        Unit8 = glConst.TEXTURE8,
        Unit9 = glConst.TEXTURE9,
        Unit10 = glConst.TEXTURE10,
        Unit11 = glConst.TEXTURE11,
        Unit12 = glConst.TEXTURE12,
        Unit13 = glConst.TEXTURE13,
        Unit14 = glConst.TEXTURE14,
        Unit15 = glConst.TEXTURE15,

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
