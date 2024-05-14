//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Vector.Orthogonalize.cs"
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
using StgSharp.Data;
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

        [DllImport(SSC_libName, EntryPoint = "normalize",
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void Normalize(M128* source, M128* target);

    }

}

namespace StgSharp.Math
{
    public static partial class Linear
    {

        public static vec4d Orthogonalize(vec4d vec)
        {
            return new vec4d { vec = Vector4.Normalize(vec.vec) };
        }
        public static vec3d Orthogonalize(vec3d vec)
        {
            return new vec3d { vec = Vector4.Normalize(vec.vec) };
        }
        public static unsafe void Orthogonalize(ref vec4d source, ref vec4d target)
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

        /// <summary>
        /// Orthogonalize two <see cref="vec3d"/>s, let their length be 1
        /// and perpendicular to each other.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static unsafe void Orthogonalize(ref vec3d source, ref vec3d target)
        {
            Vector3 s = source.v;
            Vector3 t = target.v;

            s = Vector3.Normalize(s);
            Vector3 projection = (Vector3.Dot(t, s) / Vector3.Dot(s, s)) * s;
            t -= projection;
            t = Vector3.Normalize(t);

            source.v = s;
            target.v = t;
        }

    }
}
