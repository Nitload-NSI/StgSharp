//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="glEnum.cs"
//     Project: StepVisualizer
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
using System.Runtime.CompilerServices;

namespace StgSharp.Graphics.OpenGL
{
    public enum PixelChannelLayout
    {

        UByte = glConst.UNSIGNED_BYTE,
        Byte = glConst.BYTE,
        UShort = glConst.UNSIGNED_SHORT,
        Short = glConst.SHORT,
        UInt = glConst.UNSIGNED_INT,
        Int = glConst.INT,
        Float = glConst.FLOAT,
        UByte332 = glConst.UNSIGNED_BYTE_3_3_2,
        UByte233Rev = glConst.UNSIGNED_BYTE_2_3_3_REV,
        UShort565 = glConst.UNSIGNED_SHORT_5_6_5,
        UShort565Rev = glConst.UNSIGNED_SHORT_5_6_5_REV,
        UShort4444 = glConst.UNSIGNED_SHORT_4_4_4_4,
        UShort4444Rev = glConst.UNSIGNED_SHORT_4_4_4_4_REV,
        UShort5551 = glConst.UNSIGNED_SHORT_5_5_5_1,
        UShort1555Rev = glConst.UNSIGNED_SHORT_1_5_5_5_REV,
        UInt8888 = glConst.UNSIGNED_INT_8_8_8_8,
        UInt8888Rev = glConst.UNSIGNED_INT_8_8_8_8_REV,
        UInt1010102 = glConst.UNSIGNED_INT_10_10_10_2,
        UInt2101010Rev = glConst.UNSIGNED_INT_2_10_10_10_REV

    }

    public enum FaceMode
    {

        Point = glConst.POINT,
        Line = glConst.LINE,
        Fill = glConst.FILL

    }

    public enum FrameBufferStatus
    {

        Complete = glConst.FRAMEBUFFER_COMPLETE, // GL_FRAMEBUFFER_COMPLETE
        IncompleteAttachment = glConst.FRAMEBUFFER_INCOMPLETE_ATTACHMENT, // GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT
        IncompleteMissingAttachment = glConst.FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT, // GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT
        IncompleteDrawBuffer = glConst.FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER, // GL_FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER
        IncompleteReadBuffer = glConst.FRAMEBUFFER_INCOMPLETE_READ_BUFFER, // GL_FRAMEBUFFER_INCOMPLETE_READ_BUFFER
        Unsupported = glConst.FRAMEBUFFER_UNSUPPORTED, // GL_FRAMEBUFFER_UNSUPPORTED
        IncompleteMultisample = glConst.FRAMEBUFFER_INCOMPLETE_MULTISAMPLE, // GL_FRAMEBUFFER_INCOMPLETE_MULTISAMPLE
        IncompleteLayerTargets = glConst.FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS // GL_FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS

    }

    public enum FrameBufferChannel
    {

        STENCIL_INDEX = glConst.STENCIL_INDEX,
        DEPTH_COMPONENT = glConst.DEPTH_COMPONENT,
        DEPTH_STENCIL = glConst.DEPTH_STENCIL,
        RED = glConst.RED,
        GREEN = glConst.GREEN,
        BLUE = glConst.BLUE,
        RGB = glConst.RGB,
        BGR = glConst.BGR,
        RGBA = glConst.RGBA,
        BGRA = glConst.BGRA

    }

    public enum FrameBufferAttachment
    {

        Color = glConst.COLOR_ATTACHMENT0,
        Depth = glConst.DEPTH_ATTACHMENT,
        Stencil = glConst.STENCIL_ATTACHMENT,
        DepthAndStencil = glConst.DEPTH_STENCIL_ATTACHMENT,

    }

    public enum Texture2DTarget
    {

        Texture2D = glConst.TEXTURE_2D,
        ProxyTexture2D = glConst.PROXY_TEXTURE_2D,
        Texture1DArray = glConst.TEXTURE_1D_ARRAY,
        ProxyTexture1DArray = glConst.PROXY_TEXTURE_1D_ARRAY,
        TextureRectangle = glConst.TEXTURE_RECTANGLE,
        ProxyTextureRectangle = glConst.PROXY_TEXTURE_RECTANGLE,
        TextureCubeMapPositiveX = glConst.TEXTURE_CUBE_MAP_POSITIVE_X,
        TextureCubeMapNegativeX = glConst.TEXTURE_CUBE_MAP_NEGATIVE_X,
        TextureCubeMapPositiveY = glConst.TEXTURE_CUBE_MAP_POSITIVE_Y,
        TextureCubeMapNegativeY = glConst.TEXTURE_CUBE_MAP_NEGATIVE_Y,
        TextureCubeMapPositiveZ = glConst.TEXTURE_CUBE_MAP_POSITIVE_Z,
        TextureCubeMapNegativeZ = glConst.TEXTURE_CUBE_MAP_NEGATIVE_Z,
        ProxyTextureCubeMap = glConst.PROXY_TEXTURE_CUBE_MAP

    }

    public enum RenderBufferInternalFormat : uint
    {

        RGBA8 = glConst.RGBA8,
        Depth24_Stencil8 = glConst.DEPTH24_STENCIL8,

    }

    public enum glOperation : uint
    {

        StencilTest = glConst.STENCIL_TEST,
        DepthTest = glConst.DEPTH_TEST,
        PolygonOffsetFill = glConst.POLYGON_OFFSET_FILL,
        PolygonOffsetLine = glConst.POLYGON_OFFSET_LINE,
        PolygonOffsetPoint = glConst.POLYGON_OFFSET_POINT,

    }

    #pragma warning disable CA1008
    public enum ShaderType : int
    #pragma warning restore CA1008
    {

        Fragment = glConst.FRAGMENT_SHADER,
        Vertex = glConst.VERTEX_SHADER,
        Compute = glConst.COMPUTE_SHADER,
        Geometry = glConst.GEOMETRY_SHADER,

    }

    #pragma warning disable CA1008
    public enum BufferType : int
    #pragma warning restore CA1008
    {

        ArrayBuffer = glConst.ARRAY_BUFFER,
        ElementArrayBuffer = glConst.ELEMENT_ARRAY_BUFFER,
        AtomicCounterBuffer = glConst.ATOMIC_COUNTER_BUFFER,
        CopyHeadBuffer = glConst.COPY_READ_BUFFER,
        COPY_WRITE_BUFFER = glConst.COPY_WRITE_BUFFER,
        DISPATCH_INDIRECT_BUFFER = glConst.DISPATCH_INDIRECT_BUFFER,
        DRAW_INDIRECT_BUFFER = glConst.DRAW_INDIRECT_BUFFER,
        PIXEL_PACK_BUFFER = glConst.PIXEL_PACK_BUFFER,
        PIXEL_UNPACK_BUFFER = glConst.PIXEL_UNPACK_BUFFER,
        QUERY_BUFFER = glConst.QUERY_BUFFER,
        ShaderStorageBuffer = glConst.SHADER_STORAGE_BUFFER,
        TEXTURE_BUFFER = glConst.TEXTURE_BUFFER,
        TRANSFORM_FEEDBACK_BUFFER = glConst.TRANSFORM_FEEDBACK_BUFFER,
        UNIFORM_BUFFER = glConst.UNIFORM_BUFFER,

    }

    #pragma warning disable CA1008
    public enum BufferUsage : int
    #pragma warning restore CA1008
    {

        StreamDraw = 0x88E0,
        StreamRead = 0x88E1,
        StreamCopy = 0x88E2,
        StaticDraw = 0x88E4,
        StaticRead = 0x88E5,
        StaticCopy = 0x88E6,
        DynamicDraw = 0x88E8,
        DynamicRead = 0x88E9,
        DynamicCopy = 0x88EA

    }

    public enum MaskBufferBit
    {

        COLOR = glConst.COLOR_BUFFER_BIT,
        DEPTH = glConst.DEPTH_BUFFER_BIT,
        STENCIL = glConst.STENCIL_BUFFER_BIT,

    }

    public enum GeometryType : uint
    {

        POINTS = 0x0000,
        LINES = 0x0001,
        LINE_LOOP = 0x0002,
        LINE_STRIP = 0x0003,
        TRIANGLES = 0x0004,
        TRIANGLE_STRIP = 0x0005,
        TRIANGLE_FAN = 0x0006

    }

    public enum TextureFiltering
    {

        Nearest = 0x2600,
        Linear = 0x2601

    }
}
