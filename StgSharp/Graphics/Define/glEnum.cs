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


namespace StgSharp.Graphics
{
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

    public enum ShaderType : int
    {
        Fragment = 0x8B30,
        Vertex = 0x8B31,
    }

    public enum BufferType : uint
    {
        ARRAY_BUFFER = 0x8892,
        ELEMENT_ARRAY_BUFFER = 0x8893,
        ATOMIC_COUNTER_BUFFER = GLconst.ATOMIC_COUNTER_BUFFER,
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

    public enum BufferUsage : int
    {
        STREAM_DRAW = 0x88E0,
        STREAM_READ = 0x88E1,
        STREAM_COPY = 0x88E2,
        STATIC_DRAW = 0x88E4,
        STATIC_READ = 0x88E5,
        STATIC_COPY = 0x88E6,
        DYNAMIC_DRAW = 0x88E8,
        DYNAMIC_READ = 0x88E9,
        DYNAMIC_COPY = 0x88EA
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
