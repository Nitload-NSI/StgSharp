using System.Runtime.InteropServices;

namespace StgSharp.Graphics.GL
{

    [StructLayout(LayoutKind.Sequential)]
    internal struct shaderObject
    {
        uint VBO;
        uint VAO;
        uint shaderProgram;
    }
}
