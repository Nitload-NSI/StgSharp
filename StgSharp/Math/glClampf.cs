//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="glClampf.cs"
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Math
{
    /// <summary>
    /// OpenGL numeric type.
    /// An IEEE-754 floating-point value, clamped to the range [0,1].
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct GLclampf
    {

        public float value;

        public GLclampf(float a)
        {
            value = a;
        }

        public static explicit operator float(GLclampf v)
        {
            return v.value;
        }

        public static explicit operator GLclampf(float v)
        {
            return new GLclampf(v);
        }

    }

    /// <summary>
    /// OpenGL numeric type.
    /// An IEEE-754 double precision floating-point value, clamped to the range [0,1].
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct GLclampd
    {

        public double value;

        public GLclampd(double a)
        {
            value = a;
        }

        public static explicit operator double(GLclampd v)
        {
            return v.value;
        }

        public static explicit operator GLclampd(double v)
        {
            return new GLclampd(v);
        }

    }
}
