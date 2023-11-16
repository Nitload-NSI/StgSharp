using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics
{

    public class Uniform<T> where T : struct
    {
        internal int id;
        public unsafe Uniform(
            Shader program, string name)
        {
            byte[] namearray = Encoding.ASCII.GetBytes(name);
            fixed (byte* ptr = namearray)
            {
                id = GL.gl.GetUniformLocation(program.programID, ptr);
            }
        }


    }


    public class Uniform<T, U>
        where T : struct
        where U : struct
    {
        internal int id;

        public unsafe Uniform(Shader program, string name)
        {
            byte[] namearray = Encoding.ASCII.GetBytes(name);
            fixed (byte* ptr = namearray)
            {
                id = GL.gl.GetUniformLocation(program.programID, ptr);
            }
        }
    }

    public class Uniform<T, U, V>
        where T : struct
        where U : struct
        where V : struct
    {
        internal int id;

        public unsafe Uniform(Shader program, string name)
        {
            byte[] namearray = Encoding.ASCII.GetBytes(name);
            fixed (byte* ptr = namearray)
            {
                id = GL.gl.GetUniformLocation(program.programID, ptr);
            }
        }
    }

    public class Uniform<T, U, V, W>
        where T : struct
        where U : struct
        where V : struct
        where W : struct
    {
        internal int id;

        public unsafe Uniform(Shader program, string name)
        {
            byte[] namearray = Encoding.ASCII.GetBytes(name);
            fixed (byte* ptr = namearray)
            {
                id = GL.gl.GetUniformLocation(program.programID, ptr);
            }
        }

    }

    public static partial class GL
    {
        public static void SetValue(this Uniform<float> uniform, float v0)
        {
            gl.Uniform1f(uniform.id, v0);
        }

        public static void SetValue(this Uniform<float, float> uniform, float v0, float v1)
        {
            gl.Uniform2f(uniform.id, v0, v1);
        }

        public static void SetValue(this Uniform<float, float, float> uniform, float v0, float v1, float v2)
        {
            gl.Uniform3f(uniform.id, v0, v1, v2);
        }

        public static void SetValue(this Uniform<float, float, float, float> uniform, float v0, float v1, float v2, float v3)
        {
            gl.Uniform4f(uniform.id, v0, v1, v2, v3);
        }

        public static unsafe void SetValue(this Uniform<float, float, float, float> uniform, Vec4d vec)
        {
            gl.Uniform1fv(uniform.id, 1, &vec);
        }

        public static unsafe void SetValue(this Uniform<Vec4d> uniform, Vec4d vec)
        {
            gl.Uniform1fv(uniform.id, 1, &vec);
        }
    }
}
