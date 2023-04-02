using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Math
{

    [StructLayout(LayoutKind.Explicit)]
    public struct matrix4x3:IEquatable<matrix4x3>
    {
        [FieldOffset(0)]
        public vec3d row1;
        [FieldOffset(12)]
        public vec3d row2;
        [FieldOffset(24)]
        public vec3d row3;
        [FieldOffset(36)]
        public vec3d row4;

        public matrix4x3(
            float a11, float a12, float a13,
            float a21, float a22, float a23,
            float a31, float a32, float a33,
            float a41, float a42, float a43
            )
        {
            row1 = new vec3d(a11, a12, a13);
            row2 = new vec3d(a21, a22, a23);
            row3 = new vec3d(a31, a32, a33);
            row4 = new vec3d(a41, a42, a43);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (obj.GetType() != typeof(matrix4x3))
            {
                return false;
            }
            else 
            {
                return this.Equals((matrix4x3)obj);
            }
        }

        bool IEquatable<matrix4x3>.Equals(matrix4x3 other)
        {
            if (this.GetHashCode() != other.GetHashCode())
            {
                return false;
            }
            else
            {
                return this.row1 == other.row1
                    && this.row2 == other.row2
                    && this.row3 == other.row3
                    && this.row4 == other.row4;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        
    }
}
