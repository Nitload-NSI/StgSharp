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

namespace StgSharp.Graphics
{
    public unsafe class Texture
    {

        internal static bool[] unitActivated = new bool[16];
        private IntPtr binding;
        internal glHandleSet _textureHandle;
        internal Image[] sourceList;

        internal Texture(int n, Form binding)
        {
            sourceList = new Image[n];
            this.binding = binding.graphicContextID;
            _textureHandle = GL.GenTextures(n);
        }

        public void Activate(TextureUnit unit)
        {
            GL.ActiveTexture((uint)unit);
        }

        public void Bind2D(int index)
        {
            GL.BindTexture(GLconst.TEXTURE_2D, _textureHandle[index]);
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="index"> </param>
        /// <param name="loader"> </param>
        public unsafe void LoadTexture(int index, string name, ImageLoader loader)
        {
            ImageInfo info = default;
            InternalIO.InternalLoadImage(name, &info, loader);
            sourceList[index] = new Image(info);
            if (sourceList[index].info.pixelPtr != IntPtr.Zero)
            {
                uint channel;
                switch (sourceList[index].info.channel)
                {
                    case 1:
                        channel = GLconst.RED; break;
                    case 2:
                        channel = GLconst.RG; break;
                    case 3:
                        channel = GLconst.RGB; break;
                    case 4:
                        channel = GLconst.RGBA; break;
                    default:
                        channel = 0;
                        break;
                }
                GL.TextureImage2d<byte>(
                    GLconst.TEXTURE_2D, 0, channel,
                    (uint)sourceList[index].Width, (uint)sourceList[index].Height,
                    channel, GLconst.UNSIGNED_BYTE,
                    sourceList[index].pixelArrayHandle
                    );
            }
            else
            {
                InternalIO.InternalWriteLog($"Failed to load image file of \"{name}\".", LogType.Error);
            }
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

    public enum Filter
    {
        Nearest = GLconst.NEAREST,
        Linear = GLconst.LINEAR,
        Nearest_LinearMipmap = GLconst.NEAREST_MIPMAP_LINEAR,
        Linear_LinearMipmap = GLconst.LINEAR_MIPMAP_LINEAR,
        Nearest_NearestMipmap = GLconst.NEAREST_MIPMAP_NEAREST,
        Linear_NearestMipmap = GLconst.LINEAR_MIPMAP_NEAREST,
    }

    public enum TextureUnit
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

    public enum Wrap
    {
        Repeat = GLconst.REPEAT,
        MirroredRepeat = GLconst.MIRRORED_REPEAT,
        ClampToEdge = GLconst.CLAMP_TO_EDGE,
        ClampToBorder = GLconst.CLAMP_TO_BORDER,
    }
}