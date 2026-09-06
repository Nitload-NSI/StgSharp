// -----------------------------------------------------------------------------
// file="GeometryOperation"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Common.Timing;
using Nitload.Mathematics.Numeric.Graphics;

using System;
using System.Runtime.InteropServices;

namespace Nitload.Geometries
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate Vec3<float> GeometryMotionDelegate(
                                TimeSpanProvider timeSource,
                                out float rotation
    );

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
                      GeometryMotionDelegate motionDelegate
        )
        {
            isIncrementMode = isIncreament;
            time = timeSource;
            if (motionDelegate == null)
            {
                handle = (IntPtr)(delegate*<TimeSpanProvider, out float, Vec3<float>>)&defaultMotion;
                movement = defaultMotion;
            } else if (motionDelegate.Method.IsStatic)
            {
                sourceIsStatic = true;
                handle = Marshal.GetFunctionPointerForDelegate(motionDelegate);
                movement = motionDelegate;
            } else
            {
                movement = motionDelegate;
            }

            // sourceIsStatic = false;
        }

        public bool IsIncrement => isIncrementMode;

        public static GeometryMotion DefaultMotion => new GeometryMotion(true, null, null);

        public unsafe void RunMotion(
                           ref Vec3<float> coord,
                           ref float rotation
        )
        {
            float rot;
            Vec3<float> vec;
            if (sourceIsStatic)
            {
                vec = ((delegate* unmanaged[Cdecl]<TimeSpanProvider, out float, Vec3<float>>)handle)(
                    time, out rot);
            } else
            {
                vec = movement(time, out rot);
            }

            if (isIncrementMode)
            {
                coord += vec;
                rotation += rot;
            } else
            {
                coord = vec;
                rotation = rot;
            }
        }

        private static Vec3<float> defaultMotion(
                                   TimeSpanProvider provider,
                                   out float rotation
        )
        {
            rotation = 0;
            return Vec3<float>.Zero;
        }

    }//------------------------------- End of Class -------------------------------
}
