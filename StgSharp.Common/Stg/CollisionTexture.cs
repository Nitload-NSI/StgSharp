//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="CollisionTexture"
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
using StgSharp.Geometries;
using StgSharp.Graphics;
using StgSharp.Graphics.OpenGL;
using StgSharp.Internal;

using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphic;
using StgSharp.MVVM;

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Stg
{
    public interface IOneDimensionalCollisionTexture2D
    {

        public void Init(Radius angleStride, string name);

    }

    public sealed class OneDimensionalCollisionTexture2D_GL : glRender, IOneDimensionalCollisionTexture2D
    {

        private float[] textureOutput;
        private ElementBuffer ebo;
        private FrameBuffer fbo;
        private RenderBuffer rbo;

        private ShaderProgram generatingShader;

        private TextureGL t;

        private VertexArray vao;
        private VertexBuffer vbo;

        public OneDimensionalCollisionTexture2D_GL() { }

        public override bool IsContextSharable => false;

        public void GenerateCollisionTexture(IEnumerable<IInstancingBuffer> bufferEnumeration)
        {
            fbo.Bind(0);
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(MaskBufferBit.DEPTH | MaskBufferBit.COLOR);
            generatingShader.Use();
            vao.Bind(0);

            foreach (IInstancingBuffer item in bufferEnumeration)
            {
                IGeometry g = item.TypicalShape;

                if (!(g is PlainGeometry)) {
                    throw new InvalidOperationException(
                        "The current particle buffer contains non-planar geometry,");
                }

                ebo.Bind(0);
                ebo.SetValue<int>(0, g.VertexIndices, BufferUsage.StreamDraw);

                vbo.WriteVectorData<Vec4>(0, item.CoordAndRotationSpan, BufferUsage.StreamDraw);
                vao.SetVertexAttribute(1, 3, TypeCode.Single, false, 4 * sizeof(float), 0);
                vao.SetVertexAttribute(2, 1, TypeCode.Single, false, 4 * sizeof(float), 3 * sizeof(float));

                vbo.WriteScalerData<float>(2, item.ScalingSpan, BufferUsage.StreamDraw);
                vao.SetVertexAttribute(3, 1, TypeCode.Single, false, sizeof(float), 0);

                vbo.WriteVectorData(1, g.VertexStream, BufferUsage.StreamDraw);
                vao.SetVertexAttribute(0, 3, TypeCode.Single, false, 4 * sizeof(float), 0);

                GL.DrawElementsInstanced(
                    GeometryType.TRIANGLES, ((uint)g.VertexIndices.Length) / 3, TypeCode.UInt32,
                    IntPtr.Zero, (uint)item.ScalingSpan.Length);
            }

            t.Bind2D(0);
            GL.GetTextureImage(
                Texture2DTarget.Texture2D, 0, ImageChannel.SingleColor, PixelChannelLayout.Float,
                textureOutput);
            VertexArray.BindNull();
        }

        public void Init(Radius angleStride, string name)
        {
            primeArgs = new ViewPort(
                1, (int)((new Radius(Scaler.Pi)) / angleStride), name, IntPtr.Zero);

            PlatformSpecifiedInitialize();
            CustomizeInit();
        }

        #pragma warning disable CS8618

        protected override void CustomizeDeinit()
        {
            throw new NotImplementedException();
        }

        protected override void CustomizeInit()
        {
            generatingShader = CreateShaderProgram();

            Shader vertexShader = CreateShaderSegment(ShaderType.Vertex, 1);
            Shader fragmentShader = CreateShaderSegment(ShaderType.Fragment, 1);

            vertexShader.Compile(0, InternalIO.CollisionTextureVertexShader);
            fragmentShader.Compile(0, InternalIO.CollisionTextureFragmentShader);
            vertexShader.AttachTo(0, generatingShader);
            fragmentShader.AttachTo(0, generatingShader);

            generatingShader.Link();
            generatingShader.Use();

            vbo = CreateVertexBuffer(3);
            vao = CreateVertexArray(1);
            ebo = CreateElementBuffer(1);


            fbo = CreateFrameBuffer(1);
            rbo = CreateRenderBuffer(1);
            GL.SetRenderBufferStorage(RenderBufferInternalFormat.Depth24_Stencil8, this.Size);


            t = CreateTexture(1);
            t.Bind2D(0);
            GL.TextureImage2d<byte>(
                Texture2DTarget.Texture2D, 0, ImageChannel.WithAlphaChannel, (uint)Width, (uint)Height,
                ImageChannel.WithAlphaChannel, PixelChannelLayout.Byte, Array.Empty<byte>());

            t.Set2dWrapProperty(0, TextureWrap.Repeat, TextureWrap.Repeat);
            t.Set2dFilterProperty(0, TextureFilter.Nearest, TextureFilter.Nearest);

            GL.FrameBufferTexture2d(
                FrameBufferTarget.All, glAttachment.Color(0), Texture2DTarget.Texture2D, t[0], 0);

            GL.CombineFrameBufferRenderBuffer(FrameBufferAttachment.Color, rbo[0]);

            textureOutput = new float[this.Width];
        }

    }
}
