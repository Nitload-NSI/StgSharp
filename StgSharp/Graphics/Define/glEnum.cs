//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="glEnum.cs"
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

using System.Runtime.CompilerServices;

namespace StgSharp.Graphics.OpenGL
{
    public enum PixelChannelLayout
    {

        UByte = GLconst.UNSIGNED_BYTE,
        Byte = GLconst.BYTE,
        UShort = GLconst.UNSIGNED_SHORT,
        Short = GLconst.SHORT,
        UInt = GLconst.UNSIGNED_INT,
        Int = GLconst.INT,
        Float = GLconst.FLOAT,
        UByte332 = GLconst.UNSIGNED_BYTE_3_3_2,
        UByte233Rev = GLconst.UNSIGNED_BYTE_2_3_3_REV,
        UShort565 = GLconst.UNSIGNED_SHORT_5_6_5,
        UShort565Rev = GLconst.UNSIGNED_SHORT_5_6_5_REV,
        UShort4444 = GLconst.UNSIGNED_SHORT_4_4_4_4,
        UShort4444Rev = GLconst.UNSIGNED_SHORT_4_4_4_4_REV,
        UShort5551 = GLconst.UNSIGNED_SHORT_5_5_5_1,
        UShort1555Rev = GLconst.UNSIGNED_SHORT_1_5_5_5_REV,
        UInt8888 = GLconst.UNSIGNED_INT_8_8_8_8,
        UInt8888Rev = GLconst.UNSIGNED_INT_8_8_8_8_REV,
        UInt1010102 = GLconst.UNSIGNED_INT_10_10_10_2,
        UInt2101010Rev = GLconst.UNSIGNED_INT_2_10_10_10_REV

    }

    public enum FrameBufferStatus
    {

        Complete = GLconst.FRAMEBUFFER_COMPLETE, // GL_FRAMEBUFFER_COMPLETE
        IncompleteAttachment = GLconst.FRAMEBUFFER_INCOMPLETE_ATTACHMENT, // GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT
        IncompleteMissingAttachment = GLconst.FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT, // GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT
        IncompleteDrawBuffer = GLconst.FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER, // GL_FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER
        IncompleteReadBuffer = GLconst.FRAMEBUFFER_INCOMPLETE_READ_BUFFER, // GL_FRAMEBUFFER_INCOMPLETE_READ_BUFFER
        Unsupported = GLconst.FRAMEBUFFER_UNSUPPORTED, // GL_FRAMEBUFFER_UNSUPPORTED
        IncompleteMultisample = GLconst.FRAMEBUFFER_INCOMPLETE_MULTISAMPLE, // GL_FRAMEBUFFER_INCOMPLETE_MULTISAMPLE
        IncompleteLayerTargets = GLconst.FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS // GL_FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS

    }

    internal enum VkStructureType : uint
    {

        VK_STRUCTURE_TYPE_XLIB_SURFACE_CREATE_INFO_KHR = 1000004000,
        VK_STRUCTURE_TYPE_XCB_SURFACE_CREATE_INFO_KHR = 1000005000,
        VK_STRUCTURE_TYPE_WAYLAND_SURFACE_CREATE_INFO_KHR = 1000006000,
        VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR = 1000009000,
        VK_STRUCTURE_TYPE_MACOS_SURFACE_CREATE_INFO_MVK = 1000123000,
        VK_STRUCTURE_TYPE_METAL_SURFACE_CREATE_INFO_EXT = 1000217000,
        VK_STRUCTURE_TYPE_MAX_ENUM = 0x7FFFFFFF

    }

    public enum FrameBufferChannel
    {

        STENCIL_INDEX = GLconst.STENCIL_INDEX,
        DEPTH_COMPONENT = GLconst.DEPTH_COMPONENT,
        DEPTH_STENCIL = GLconst.DEPTH_STENCIL,
        RED = GLconst.RED,
        GREEN = GLconst.GREEN,
        BLUE = GLconst.BLUE,
        RGB = GLconst.RGB,
        BGR = GLconst.BGR,
        RGBA = GLconst.RGBA,
        BGRA = GLconst.BGRA

    }

    public enum FrameBufferAttachment
    {

        Color = GLconst.COLOR_ATTACHMENT0,
        Depth = GLconst.DEPTH_ATTACHMENT,
        Stencil = GLconst.STENCIL_ATTACHMENT,
        DepthAndStencil = GLconst.DEPTH_STENCIL_ATTACHMENT,

    }

    public enum Texture2DTarget
    {

        Texture2D = GLconst.TEXTURE_2D,
        ProxyTexture2D = GLconst.PROXY_TEXTURE_2D,
        Texture1DArray = GLconst.TEXTURE_1D_ARRAY,
        ProxyTexture1DArray = GLconst.PROXY_TEXTURE_1D_ARRAY,
        TextureRectangle = GLconst.TEXTURE_RECTANGLE,
        ProxyTextureRectangle = GLconst.PROXY_TEXTURE_RECTANGLE,
        TextureCubeMapPositiveX = GLconst.TEXTURE_CUBE_MAP_POSITIVE_X,
        TextureCubeMapNegativeX = GLconst.TEXTURE_CUBE_MAP_NEGATIVE_X,
        TextureCubeMapPositiveY = GLconst.TEXTURE_CUBE_MAP_POSITIVE_Y,
        TextureCubeMapNegativeY = GLconst.TEXTURE_CUBE_MAP_NEGATIVE_Y,
        TextureCubeMapPositiveZ = GLconst.TEXTURE_CUBE_MAP_POSITIVE_Z,
        TextureCubeMapNegativeZ = GLconst.TEXTURE_CUBE_MAP_NEGATIVE_Z,
        ProxyTextureCubeMap = GLconst.PROXY_TEXTURE_CUBE_MAP

    }

    public enum RenderBufferInternalFormat : uint
    {

        RGBA8 = GLconst.RGBA8,
        Depth24_Stencil8 = GLconst.DEPTH24_STENCIL8,

    }

    public enum GLOperation : uint
    {

        StencilTest = GLconst.STENCIL_TEST,

    }

    internal enum VkResult
    {

        VK_SUCCESS = 0,
        VK_NOT_READY = 1,
        VK_TIMEOUT = 2,
        VK_EVENT_SET = 3,
        VK_EVENT_RESET = 4,
        VK_INCOMPLETE = 5,
        VK_ERROR_OUT_OF_HOST_MEMORY = -1,
        VK_ERROR_OUT_OF_DEVICE_MEMORY = -2,
        VK_ERROR_INITIALIZATION_FAILED = -3,
        VK_ERROR_DEVICE_LOST = -4,
        VK_ERROR_MEMORY_MAP_FAILED = -5,
        VK_ERROR_LAYER_NOT_PRESENT = -6,
        VK_ERROR_EXTENSION_NOT_PRESENT = -7,
        VK_ERROR_FEATURE_NOT_PRESENT = -8,
        VK_ERROR_INCOMPATIBLE_DRIVER = -9,
        VK_ERROR_TOO_MANY_OBJECTS = -10,
        VK_ERROR_FORMAT_NOT_SUPPORTED = -11,
        VK_ERROR_SURFACE_LOST_KHR = -1000000000,
        VK_SUBOPTIMAL_KHR = 1000001003,
        VK_ERROR_OUT_OF_DATE_KHR = -1000001004,
        VK_ERROR_INCOMPATIBLE_DISPLAY_KHR = -1000003001,
        VK_ERROR_NATIVE_WINDOW_IN_USE_KHR = -1000000001,
        VK_ERROR_VALIDATION_FAILED_EXT = -1000011001,
        VK_RESULT_MAX_ENUM = 0x7FFFFFFF

    }

    #pragma warning disable CA1008 
    public enum ShaderType : int
    #pragma warning restore CA1008 
    {

        Fragment = GLconst.FRAGMENT_SHADER,
        Vertex = GLconst.VERTEX_SHADER,
        Compute = GLconst.COMPUTE_SHADER,
        Geometry = GLconst.GEOMETRY_SHADER,

    }

    #pragma warning disable CA1008 // 枚举应具有零值
    public enum BufferType : int
    #pragma warning restore CA1008 // 枚举应具有零值
    {

        ArrayBuffer = GLconst.ARRAY_BUFFER,
        ElementArrayBuffer = GLconst.ELEMENT_ARRAY_BUFFER,
        AtomicCounterBuffer = GLconst.ATOMIC_COUNTER_BUFFER,
        COPY_READ_BUFFER = GLconst.COPY_READ_BUFFER,
        COPY_WRITE_BUFFER = GLconst.COPY_WRITE_BUFFER,
        DISPATCH_INDIRECT_BUFFER = GLconst.DISPATCH_INDIRECT_BUFFER,
        DRAW_INDIRECT_BUFFER = GLconst.DRAW_INDIRECT_BUFFER,
        PIXEL_PACK_BUFFER = GLconst.PIXEL_PACK_BUFFER,
        PIXEL_UNPACK_BUFFER = GLconst.PIXEL_UNPACK_BUFFER,
        QUERY_BUFFER = GLconst.QUERY_BUFFER,
        SHADER_STORAGE_BUFFER = GLconst.SHADER_STORAGE_BUFFER,
        TEXTURE_BUFFER = GLconst.TEXTURE_BUFFER,
        TRANSFORM_FEEDBACK_BUFFER = GLconst.TRANSFORM_FEEDBACK_BUFFER,
        UNIFORM_BUFFER = GLconst.UNIFORM_BUFFER,

    }

    #pragma warning disable CA1008 // 枚举应具有零值
    public enum BufferUsage : int
    #pragma warning restore CA1008 // 枚举应具有零值
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

        COLOR = GLconst.COLOR_BUFFER_BIT,
        DEPTH = GLconst.DEPTH_BUFFER_BIT,
        STENCIL = GLconst.STENCIL_BUFFER_BIT,

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
