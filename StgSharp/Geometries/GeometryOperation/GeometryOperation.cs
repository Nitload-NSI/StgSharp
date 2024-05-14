using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Geometries
{
    public delegate vec3d GeometryMotionDelegate(TimeSpanProvider timeSource, out float rotation);

    public class GeometryMotion
    {
        private bool isIncrementMode;
        private bool sourceIsStatic;
        private TimeSpanProvider time;
        private GeometryMotionDelegate movement;
        private IntPtr handle;

        public static GeometryMotion DefaultMotion
        {
            get
            { 
            return new GeometryMotion
                (
                true,
                 null,
                null
                );
            }
        }

        private static vec3d defaultMotion(TimeSpanProvider provider, out float rotation)
        {
            rotation = 0;
            return Vec3d.Zero;
        }

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
                handle = (IntPtr)(delegate*<TimeSpanProvider, out float, vec3d>)&defaultMotion;
                movement = defaultMotion;
            }
            else if (motionDelegate.Method.IsStatic)
            {
                sourceIsStatic = true;
                handle =  Marshal.GetFunctionPointerForDelegate(motionDelegate);
                movement = motionDelegate;
            }
            else
            {
                movement = motionDelegate;
            }
            sourceIsStatic = false;
        }

        public bool IsIncrement
        {
            get => isIncrementMode; 

        }

        public unsafe void RunMotion(ref vec3d coord, ref float rotation)
        {
            float rot = 0;
            vec3d vec;
            if (sourceIsStatic) 
            {
                vec = ((delegate*<TimeSpanProvider, out float, vec3d>)handle)(time, out rot);
            }
            else
            {
                vec = movement(time, out rot);
            }
            
            if (isIncrementMode)
            {
                coord += vec;
                rotation += rot;
            }
            else
            {
                coord = vec.XYZ;
                rotation = rot;
            }
        }
    }//------------------------------- End of Class -------------------------------
}
