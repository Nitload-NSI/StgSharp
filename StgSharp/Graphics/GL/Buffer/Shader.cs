using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics
{
    public class Shader
    {
        public readonly uint programID;
        internal uint vertexShaderID;
        internal uint fragmentShaderID;

        public unsafe Shader()
        {
            programID = GL.gl.CreateProgram();
        }

        public unsafe void CompileVertexShader(string shaderCode)
        {
            vertexShaderID = GL.CompileShader(shaderCode, ShaderType.VERTEX);
            GL.gl.AttachShader(programID, vertexShaderID);
        }

        public unsafe void CompileFragmentShader(string shaderCode)
        {
            fragmentShaderID = GL.CompileShader(shaderCode, ShaderType.FRAGMENT);
            GL.gl.AttachShader(programID, fragmentShaderID);
        }

        public void Link()
        {
            if (InternalIO.InternalLinkShaderProgram(programID) == 0)
            {
                IntPtr logPtr = InternalIO.InternalReadLog();
                try
                {
                    byte[] logByte = new byte[512];
                    Marshal.Copy(logPtr, logByte, 0, 512);
                    string log = Encoding.UTF8.GetString(logByte);
                    log = log.Replace("\0", "");
                    InternalIO.InternalWriteLog(log, LogType.Error);
                }
                catch (Exception ex)
                {
                    InternalIO.InternalWriteLog(ex.Message, LogType.Error);
                }
            }
        }

        public unsafe void Use()
        {
            GL.gl.UseProgram(programID);
        }

    }
}
