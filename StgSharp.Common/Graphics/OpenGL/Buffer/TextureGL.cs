//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="TextureGL"
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
using StgSharp.Graphics;
using StgSharp.HighPerformance;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Graphics.OpenGL
{
    public sealed unsafe class TextureGL
    {

        private int currentEditingTextureIndex;

        private OpenGLFunction _gl;
        internal GlHandle[] _textureHandle;

        internal TextureGL(
                 int count,
                 glRender binding
        )
        {
            // sourceList = new Image[count];
            this._gl = binding.GL;
            _textureHandle = _gl.GenTextures(count);
        }

        public GlHandle this[
                        int index
        ]
        {
            get => _textureHandle[index];
        }

        public int Count
        {
            get => _textureHandle.Length;
        }

        // internal Image[] sourceList;

        public int IndexOfCurrentTexture
        {
            get => currentEditingTextureIndex;
        }

        internal OpenGLFunction GL => _gl;

        public void ActivateAs(
                    TextureUnit unit
        )
        {
            _gl.ActiveTextureUnit((uint)unit);
        }

        public void Bind2D(
                    int index
        )
        {
            currentEditingTextureIndex = index;
            _gl.BindTexture(glConst.TEXTURE_2D, _textureHandle[index]);
        }

        /// <summary>
        ///   Load an <see cref="Image" /> to certain texture.
        /// </summary>
        /// <param _label="index">
        ///
        /// </param>
        /// <param _label="i">
        ///
        /// </param>
        public unsafe void LoadTexture(
                           int index,
                           Image i
        )
        {
            byte[] handle = i.PixelBuffer;
            _gl.TextureImage2d(Texture2DTarget.Texture2D, 0, i.Channel, i.Width, i.Height,
                               i.Channel, i.PixelLayout, pixels:handle);
            _gl.GenerateMipmap(Texture2DTarget.Texture2D);
        }

        #region texture setting

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set2dFilterProperty(
                    int index,
                    TextureFilter onMinify,
                    TextureFilter onMagnify
        )
        {
            #if DEBUG
            if (index != currentEditingTextureIndex)
            {
                throw new InvalidOperationException(
                    "Texture to be editted is not the one being activated.");
            }
            #endif
            _gl.TextureParameter(glConst.TEXTURE_2D, glConst.TEXTURE_MIN_FILTER, (int)onMinify);
            _gl.TextureParameter(glConst.TEXTURE_2D, glConst.TEXTURE_MAG_FILTER,
                                  (int)onMagnify);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set2dWrapProperty(
                    int index,
                    TextureWrap onHorizontial,
                    TextureWrap onVertical
        )
        {
            #if DEBUG
            if (index != currentEditingTextureIndex)
            {
                throw new InvalidOperationException(
                    "Texture to be editted is not the one being activated.");
            }
            #endif
            _gl.TextureParameter(glConst.TEXTURE_2D, glConst.TEXTURE_WRAP_S,
                                  (int)onHorizontial);
            _gl.TextureParameter(glConst.TEXTURE_2D, glConst.TEXTURE_WRAP_T, (int)onVertical);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set2dProperty(
                    int index,
                    TextureProperty property
        )
        {
            Set2dFilterProperty(index, property.FilterOnMinify, property.FilterOnMagnify);
            Set2dWrapProperty(index, property.WrapOnHorizontial, property.WrapOnVertical);
        }

        #endregion texture setting
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct TextureProperty
    {

        [FieldOffset(0)] private M128 mask;
        [FieldOffset(0)] private TextureFilter mag;
        [FieldOffset(4)] private TextureFilter min;

        [FieldOffset(8)] private  TextureWrap hori;
        [FieldOffset(12)] private TextureWrap vert;

        public TextureProperty(
               TextureFilter onMagnify,
               TextureFilter onMinify,
               TextureWrap onHorizontial,
               TextureWrap onVertical
        )
        {
            FilterOnMagnify = onMagnify;
            FilterOnMinify = onMinify;
            WrapOnHorizontial = onHorizontial;
            WrapOnVertical = onVertical;
        }

        public TextureFilter FilterOnMagnify
        {
            get => mag;
            set => mag = value;
        }

        public TextureFilter FilterOnMinify
        {
            get => min;
            set => min = value;
        }

        public TextureWrap WrapOnHorizontial
        {
            get => hori;
            set => hori = value;
        }

        public TextureWrap WrapOnVertical
        {
            get => vert;
            set => vert = value;
        }

        public override bool Equals(
                             [NotNullWhen(true)] object obj
        )
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FilterOnMagnify, FilterOnMagnify, WrapOnHorizontial,
                                    WrapOnVertical);
        }

        public override string ToString()
        {
            return $"FilterOnMagnify:{FilterOnMagnify}, " + $"FilterOnMagnify:{FilterOnMagnify}, " + $"WrapOnHorizontial:{WrapOnHorizontial}, " + $"WrapOnVertical:{WrapOnVertical}";
        }

        public static bool operator !=(
                                    TextureProperty left,
                                    TextureProperty right
        )
        {
            return left.mask != right.mask;
        }

        public static bool operator ==(
                                    TextureProperty left,
                                    TextureProperty right
        )
        {
            return left.mask == right.mask;
        }

        public static implicit operator TextureProperty(
                                        (
            TextureFilter onMagnify, TextureFilter onMinify,
            TextureWrap onHorizontial, TextureWrap onVertical
            ) propertyValueTuple
        )
        {
            return new TextureProperty(propertyValueTuple.onMagnify, propertyValueTuple.onMinify,
                                       propertyValueTuple.onHorizontial,
                                       propertyValueTuple.onVertical);
        }

    }

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