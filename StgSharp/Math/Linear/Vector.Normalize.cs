using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp
{

    internal static partial class InternalIO
    {
        [DllImport(SSC_libname, EntryPoint = "normalize",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void Normalize(M128* source, M128* target);

    }

}

namespace StgSharp.Math
{
    public static partial class Linear
    {
        public static unsafe void Normalize(ref vec4d source, ref vec4d target)
        {
            Vector4 s = source.vec;
            Vector4 t = target.vec;

            s = Vector4.Normalize(s);

            float dotProduct = Vector4.Dot(s, t);
            Vector4 projection = Vector4.Multiply(s, t);

            t -= projection;

            source.vec = s;
            target.vec = t;
        }

        public static unsafe void Normalize(ref vec3d source, ref vec3d target)
        {
            Vector3 s = source.v;
            Vector3 t = target.v;

            s = Vector3.Normalize(s);
            Vector3 projection = Vector3.Dot(t, s) / Vector3.Dot(s, s) * s;
            t -= projection;
            t = Vector3.Normalize(t);

            source.v = s;
            target.v = t;
        }

        public static vec4d Normalize(vec4d vec)
        {
            return new vec4d() { vec = Vector4.Normalize(vec.vec) };
        }
        public static vec3d Normalize(vec3d vec)
        {
            return new vec3d() { vec = Vector4.Normalize(vec.vec) };
        }
    }
}
