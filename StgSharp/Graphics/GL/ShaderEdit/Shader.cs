//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Shader.cs"
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
using StgSharp;
using StgSharp.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace StgSharp.Graphics
{
    public partial class glRenderStream
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected sealed override Shader CreateShaderSegment(ShaderType type, int count)
        {
            return new Shader( GL.CreateShaderSet(count, type), type);
        }
        /// <summary>
        /// Create an instance of <see cref="Shader"/> program
        /// </summary>
        /// <returns>New instance of <see cref="Shader"/> instance. </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected sealed override ShaderProgram CreateShaderProgram()
        {
            return new ShaderProgram(this);
        }
    }

    public class Shader : SSDSerializable
    {

        private readonly IntPtr context;
        internal readonly glHandleSet handle;
        public readonly ShaderType type;

        public override SerializableTypeCode SSDTypeCode => SerializableTypeCode.Shader;

        internal unsafe Shader(glHandleSet handle, ShaderType usage)
        {
            this.handle = handle;
            context = GL.CurrentContextHandle;
            type = usage;
        }

        /// <summary>
        /// Attach one shader code to a shader program.
        /// </summary>
        /// <param name="target">Shader program to attach</param>
        /// <param name="index">Index of shader code in current shader code set.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AttachTo(int index, ShaderProgram target)
        {
            GL.AttachShader(target.handle, handle[index]);
        }

        public static string FromCodeFile(string fileRoute)
        {
            string str = File.ReadAllText(fileRoute);
            if (str == "")
            {
                throw new Exception($"Cannot load and GLSL code from {fileRoute}");
            }
            return str;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Compile(int index, string code)
        {
            if (code == null)
            {
                throw new ArgumentNullException("Must provide enough code");
            }
            GL.LoadShaderSource(handle[index], $"{code}\0");
            GL.CompileShader(handle[index]);
            CheckStatus(index,ShaderStatus.CompileStatus);
        }

        public override string ToString()
        {
            return $"This is a {type} shader, its shader handles are {handle.ToString()}.";
        }

        //TODO shader 的序列化究极麻烦，原因是shader的架构问题
        public override byte[] GetBytes()
        {
            throw new NotImplementedException();
        }

        public void CheckStatus(int index, ShaderStatus status)
        {
            string log = GL.GetShaderStatus(this,index, (int)status);
            if (log != "")
            {
                throw new Exception($"Shader Error\n{status}\n" + log);
            }
        }

        public override byte[] GetBytes(out int length)
        {
            throw new NotImplementedException();
        }

        internal override void BuildFromByteStream(byte[] stream)
        {
            throw new NotImplementedException();
        }
    }

    public enum ShaderStatus : int
    {
        CompileStatus = GLconst.COMPILE_STATUS,
    }

    public class ShaderProgram
    {

        private readonly IntPtr binding;
        internal readonly glHandle handle;

        /// <summary>
        /// Init a program with no shader attached.
        /// </summary>
        internal unsafe ShaderProgram(glRenderStream binding)
        {
            this.binding = binding.ContextHandle;
            handle = GL.CreateProgram();
        }

        /// <summary>
        /// Get a uniform form current shader,
        /// the value type of the uniform should be provided.
        /// </summary>
        /// <param name="name">Name of the Uniform in shader code</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe Uniform<T> GetUniform<T>(string name) where T : struct
        {
            return new Uniform<T>(this, name, binding);
        }

        /// <summary>
        /// Get a uniform form current shader,
        /// the value type of the uniform should be provided.
        /// </summary>
        /// <param name="name">Name of the Uniform in shader code</param>
        /// <returns></returns>
        public unsafe Uniform<T, U> GetUniform<T, U>(string name)
            where T : struct
            where U : struct
        {
            return new Uniform<T, U>(this, name, binding);
        }

        /// <summary>
        /// Get a uniform form current shader,
        /// the value type of the uniform should be provided.
        /// </summary>
        /// <param name="name">Name of the Uniform in shader code</param>
        /// <returns></returns>
        public unsafe Uniform<T, U, V> GetUniform<T, U, V>(string name)
            where T : struct
            where U : struct
            where V : struct
        {
            return new Uniform<T, U, V>(this, name);
        }
        /// <summary>
        /// Get a uniform form current shader,
        /// the value type of the uniform should be provided.
        /// </summary>
        /// <param name="name">Name of the Uniform in shader code</param>
        /// <returns></returns>
        public unsafe Uniform<T, U, V, W> GetUniform<T, U, V, W>(string name)
            where T : struct
            where U : struct
            where V : struct
            where W : struct
        {
            return new Uniform<T, U, V, W>(this, name);
        }

        /// <summary>
        /// Link the shader to current context.
        /// </summary>
        public unsafe void Link()
        {
            if (InternalIO.InternalLinkShaderProgram((GLcontext*)this.binding, handle.Value) == 0)
            {
                IntPtr logPtr = InternalIO.InternalReadSSCLog();
                try
                {
                    byte[] logByte = new byte[512];
                    Marshal.Copy(logPtr, logByte, 0, 512);
                    string log = Encoding.UTF8.GetString(logByte);
                    log = log.Replace("\0", string.Empty);
                    InternalIO.InternalWriteLog(log, LogType.Error);
                }
                catch (Exception ex)
                {
                    InternalIO.InternalWriteLog(ex.Message, LogType.Error);
                }
            }
        }

        /// <summary>
        /// Let the current context use this shader.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Use()
        {
            GL.UseProgram(handle);
        }

        ~ShaderProgram()
        {
            //handle.Dispose();
        }

    }
}