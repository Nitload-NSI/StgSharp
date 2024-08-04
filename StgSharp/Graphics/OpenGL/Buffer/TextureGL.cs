//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="texture.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics.OpenGL
{
    public unsafe class TextureGL
    {
        private GlFunction GL;
        internal static bool[] unitActivated = new bool[16];
        internal GlHandle[] _textureHandle;
        internal Image[] sourceList;

        public GlHandle this[int index]
        {
            get => _textureHandle[index];
        }

        internal TextureGL(int n, glRenderStream binding)
        {
            sourceList = new Image[n];
            this.GL = binding.GL;
            _textureHandle = GL.GenTextures(n);
        }

        public void ActivateAs(TextureUnit unit)
        {
            GL.ActiveTexture((uint)unit);
        }

        public void Bind2D(int index)
        {
            GL.BindTexture(GLconst.TEXTURE_2D, _textureHandle[index]);
        }

        /// <summary>
        /// Load an <see cref="Image"/> to certain texture.
        /// </summary>
        /// <param name="index"> </param>
        /// <param name="i"> </param>
        public unsafe void LoadTexture(int index, Image i)
        {
            sourceList[index] = i;
            var handle = sourceList[index].PixelBuffer;
            GL.TextureImage2d(
                Texture2DTarget.Texture2D, 0, (uint)i.Channel,
                (uint)sourceList[index].Width, (uint)sourceList[index].Height,
                (uint)i.Channel, i.PixelLayout,
                pixels: handle
                );
            GL.GenerateMipmap(Texture2DTarget.Texture2D);
        }



        #region texture setting

        public void Set2dFilterProperty(
            Filter onMinify, Filter onMagnify)
        {
            GL.TextureParameter(GLconst.TEXTURE_2D, GLconst.TEXTURE_MIN_FILTER, (int)onMinify);
            GL.TextureParameter(GLconst.TEXTURE_2D, GLconst.TEXTURE_MAG_FILTER, (int)onMagnify);
        }

        public void Set2dWrapProperty(
            Wrap onHorizontial, Wrap onVertical)
        {
            GL.TextureParameter(GLconst.TEXTURE_2D, GLconst.TEXTURE_WRAP_S, (int)onHorizontial);
            GL.TextureParameter(GLconst.TEXTURE_2D, GLconst.TEXTURE_WRAP_T, (int)onVertical);
        }

        #endregion texture setting


    }

#pragma warning disable CA1008
    public enum Filter
#pragma warning restore CA1008
    {
        Nearest = GLconst.NEAREST,
        Linear = GLconst.LINEAR,
        NearestLinearMipmap = GLconst.NEAREST_MIPMAP_LINEAR,
        LinearLinearMipmap = GLconst.LINEAR_MIPMAP_LINEAR,
        NearestNearestMipmap = GLconst.NEAREST_MIPMAP_NEAREST,
        LinearNearestMipmap = GLconst.LINEAR_MIPMAP_NEAREST,
    }

#pragma warning disable CA1008
    public enum TextureUnit
#pragma warning restore CA1008
    {
        Unit0 = GLconst.TEXTURE0,
        Unit1 = GLconst.TEXTURE1,
        Unit2 = GLconst.TEXTURE2,
        Unit3 = GLconst.TEXTURE3,
        Unit4 = GLconst.TEXTURE4,
        Unit5 = GLconst.TEXTURE5,
        Unit6 = GLconst.TEXTURE6,
        Unit7 = GLconst.TEXTURE7,
        Unit8 = GLconst.TEXTURE8,
        Unit9 = GLconst.TEXTURE9,
        Unit10 = GLconst.TEXTURE10,
        Unit11 = GLconst.TEXTURE11,
        Unit12 = GLconst.TEXTURE12,
        Unit13 = GLconst.TEXTURE13,
        Unit14 = GLconst.TEXTURE14,
        Unit15 = GLconst.TEXTURE15,
    }

#pragma warning disable CA1008 
    public enum Wrap
#pragma warning restore CA1008 
    {
        Repeat = GLconst.REPEAT,
        MirroredRepeat = GLconst.MIRRORED_REPEAT,
        ClampToEdge = GLconst.CLAMP_TO_EDGE,
        ClampToBorder = GLconst.CLAMP_TO_BORDER,
    }
}