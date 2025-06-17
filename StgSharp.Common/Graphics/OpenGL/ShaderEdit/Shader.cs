//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Shader.cs"
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
using StgSharp;
using StgSharp.Internal;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace StgSharp.Graphics.OpenGL
{
    public abstract partial class glRender : RenderStream
    {

        /// <summary>
        ///   Create an instance of <see cref="Shader" /> program
        /// </summary>
        /// <returns>
        ///   New instance of <see cref="Shader" /> instance.
        /// </returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        protected override sealed ShaderProgram CreateShaderProgram()
        {
            return new ShaderProgram( this );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        protected override sealed Shader CreateShaderSegment( ShaderType type, int count )
        {
            return new Shader( GL.CreateShaderSet( count, type ), type, this );
        }

    }

    public class Shader : ISSDSerializable
    {

        private readonly OpenGLFunction GL;
        internal readonly GlHandle[] handle;
        public readonly ShaderType type;

        internal unsafe Shader( GlHandle[] handle, ShaderType usage, glRender stream )
        {
            this.handle = handle;
            GL = stream.GL;
            type = usage;
        }

        public SerializableTypeCode SSDTypeCode => SerializableTypeCode.Shader;

        /// <summary>
        ///   Attach one shader code to a shader program.
        /// </summary>
        /// <param _label="target">
        ///   Shader program to attach
        /// </param>
        /// <param _label="index">
        ///   Index of shader code in current shader code set.
        /// </param>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public void AttachTo( int index, [NotNull]ShaderProgram target )
        {
            GL.AttachShader( target.handle, handle[ index ] );
        }

        public void CheckStatus( int index, ShaderStatus status )
        {
            string log = GL.GetShaderStatus( this, index, ( int )status );
            if( !string.IsNullOrEmpty( log ) ) {
                throw new InvalidOperationException( $"{$"Shader Error \t{status}\n"}{log}" );
            }
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public unsafe void Compile( int index, ReadOnlySpan<byte> codeU8Stream )
        {
            GL.LoadShaderSource( handle[ index ], codeU8Stream );
            GL.CompileShader( handle[ index ] );
            CheckStatus( index, ShaderStatus.CompileStatus );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public unsafe void Compile( int index, byte[] codeStream )
        {
            ArgumentNullException.ThrowIfNull( codeStream );
            GL.LoadShaderSource( handle[ index ], codeStream );
            GL.CompileShader( handle[ index ] );
            CheckStatus( index, ShaderStatus.CompileStatus );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public unsafe void Compile( int index, string codeStream )
        {
            if( codeStream == null ) {
                throw new ArgumentNullException( nameof( codeStream ) );
            }
            GL.LoadShaderSource( handle[ index ], codeStream.AsSpan() );
            GL.CompileShader( handle[ index ] );
            CheckStatus( index, ShaderStatus.CompileStatus );
        }

        public void FromBytes( byte[] stream )
        {
            throw new NotImplementedException();
        }

        public static byte[] FromCodeFile( string fileRoute )
        {
            string str = File.ReadAllText( fileRoute );
            if( string.IsNullOrEmpty( str ) ) {
                throw new InvalidOperationException( $"Cannot load and GLSL code from {fileRoute}" );
            }
            return Encoding.UTF8.GetBytes( str );
        }

        public byte[] GetBytes()
        {
            return Array.Empty<byte>();
        }

        public byte[] GetBytes( out int length )
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"This is a {type} shader, its shader handles are {handle.ToString()}.";
        }

    }

    public enum ShaderStatus : int
    {

        CompileStatus = glConst.COMPILE_STATUS,

    }

    public class ShaderProgram
    {

        private readonly OpenGLFunction GL;
        internal readonly GlHandle handle;

        /// <summary>
        ///   CustomizeInit a program with no shader attached.
        /// </summary>
        internal unsafe ShaderProgram( glRender binding )
        {
            GL = binding.GL;
            handle = GL.CreateProgram();
        }

        /// <summary>
        ///   Get a uniform form current shader, the value type of the uniform should be provided.
        /// </summary>
        /// <param _label="name">
        ///   ContextName of the Uniform in shader code
        /// </param>
        /// <returns>
        ///
        /// </returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public unsafe Uniform<T> GetUniform<T>( string name ) where T: struct
        {
            return new Uniform<T>( GL.GetUniformLocation( this.handle, name ) );
        }

        /// <summary>
        ///   Get a uniform form current shader, the value type of the uniform should be provided.
        /// </summary>
        /// <param _label="name">
        ///   ContextName of the Uniform in shader code
        /// </param>
        /// <returns>
        ///
        /// </returns>
        public unsafe Uniform<T, U> GetUniform<T, U>( string name ) where T: struct where U: struct
        {
            return new Uniform<T, U>( GL.GetUniformLocation( this.handle, name ) );
        }

        /// <summary>
        ///   Get a uniform form current shader, the value type of the uniform should be provided.
        /// </summary>
        /// <param _label="name">
        ///   ContextName of the Uniform in shader code
        /// </param>
        /// <returns>
        ///
        /// </returns>
        public unsafe Uniform<T, U, V> GetUniform<T, U, V>( string name ) where T: struct
            where U: struct
            where V: struct
        {
            return new Uniform<T, U, V>( GL.GetUniformLocation( this.handle, name ) );
        }

        /// <summary>
        ///   Get a uniform form current shader, the value type of the uniform should be provided.
        /// </summary>
        /// <param _label="name">
        ///   ContextName of the Uniform in shader code
        /// </param>
        /// <returns>
        ///
        /// </returns>
        public unsafe Uniform<T, U, V, W> GetUniform<T, U, V, W>( string name ) where T: struct
            where U: struct
            where V: struct
            where W: struct
        {
            return new Uniform<T, U, V, W>( GL.GetUniformLocation( this.handle, name ) );
        }

        /// <summary>
        ///   Link the shader to current viewPortDisplay.
        /// </summary>
        public unsafe void Link()
        {
            if( InternalIO.InternalLinkShaderProgram(
                ( OpenglContext* )this.GL.ContextHandle, handle.Value ) ==
                0 )
            {
                IntPtr logPtr = InternalIO.InternalReadSSCLog();
                try
                {
                    byte[] logByte = new byte[512];
                    Marshal.Copy( logPtr, logByte, 0, 512 );
                    string log = Encoding.UTF8.GetString( logByte );
                    log = log.Replace( "\0", string.Empty );
                    InternalIO.InternalWriteLog( log, LogType.Error );
                }
                catch( Exception ex )
                {
                    InternalIO.InternalWriteLog( ex.Message, LogType.Error );
                }
            }
        }

        /// <summary>
        ///   Let the current viewPortDisplay use this shader.
        /// </summary>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public unsafe void Use()
        {
            GL.UseProgram( handle );
        }

        ~ShaderProgram()
        {
            //handle.Dispose();
        }

    }
}