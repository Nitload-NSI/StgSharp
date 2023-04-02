using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Math
{
    [StructLayout(LayoutKind.Explicit)]
    public struct colum2:IEquatable<colum2>
    {
        [FieldOffset(0)]
        public float X;
        [FieldOffset(4)]
        public float Y;

        public colum2(float x,float y)
        {
            X= x;
            Y= y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (obj.GetType() != typeof(colum2))
            {
                return false;
            }
            else
            {
                return this.Equals((colum2)obj);
            }
        }

        bool IEquatable<colum2>.Equals(colum2 other)
        {
            if (this.GetHashCode() != other.GetHashCode())
            {
                return false;
            }
            else
            {
                return X == other.X && Y == other.Y;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X,Y);
        }

        public override string ToString()
        {
            return $"{X}\n{Y}";
        }

        public static colum2 operator +(colum2 c1,colum2 c2)
        {
            return new colum2(
                c1.X + c2.X,
                c1.Y + c2.Y
                );
        }

        public static colum2 operator -(colum2 c1, colum2 c2)
        {
            return new colum2(
                c1.X - c2.X,
                c1.Y - c2.Y
                );
        }

        public static colum2 operator *(colum2 colum, float x)
        {
            return new colum2(colum.X * x,colum.Y * x);
        }

        public static colum2 operator /(colum2 colum, float x)
        {
            return new colum2(colum.X / x, colum.Y / x);
        }
    }
}
