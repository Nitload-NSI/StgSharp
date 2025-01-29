//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="GeometryOperation.cs"
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
using StgSharp.Math;
using StgSharp.Timing;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Geometries
{
[UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    public delegate Vec3 GeometryMotionDelegate(
        TimeSpanProvider timeSource,
        out float rotation );

    public class GeometryMotion
    {

        private bool isIncrementMode;
        private bool sourceIsStatic;
        private GeometryMotionDelegate movement;
        private IntPtr handle;
        private TimeSpanProvider time;

        public unsafe GeometryMotion(
            bool isIncreament,
            TimeSpanProvider timeSource,
            GeometryMotionDelegate motionDelegate )
        {
            isIncrementMode = isIncreament;
            time = timeSource;
            if( motionDelegate == null ) {
                handle = ( IntPtr )( delegate*<TimeSpanProvider, out float, Vec3> )&defaultMotion;
                movement = defaultMotion;
            } else if( motionDelegate.Method.IsStatic ) {
                sourceIsStatic = true;
                handle = Marshal.GetFunctionPointerForDelegate( motionDelegate );
                movement = motionDelegate;
            } else {
                movement = motionDelegate;
            }

            //sourceIsStatic = false;
        }

        public bool IsIncrement
        {
            get => isIncrementMode;
        }

        public static GeometryMotion DefaultMotion
        {
            get => new GeometryMotion( true, null, null );
        }

        public unsafe void RunMotion( ref Vec3 coord, ref float rotation )
        {
            float rot;
            Vec3 vec;
            if( sourceIsStatic ) {
                vec = ( ( delegate* unmanaged[Cdecl]<TimeSpanProvider, out float, Vec3> )handle )(
                    time, out rot );
            } else {
                vec = movement( time, out rot );
            }

            if( isIncrementMode ) {
                coord += vec;
                rotation += rot;
            } else {
                coord = vec;
                rotation = rot;
            }
        }

        private static Vec3 defaultMotion(
            TimeSpanProvider provider,
            out float rotation )
        {
            rotation = 0;
            return Vec3.Zero;
        }

    }//------------------------------- End of Class -------------------------------
}
