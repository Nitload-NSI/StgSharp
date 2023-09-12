using StgSharp.Math;
using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace StgSharp.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct formInfo
    {
        public int width;
        public int height;
        public Vector4 color;
        public byte[] name;
    }

    public unsafe class Form
    {
        
        internal IntPtr windowID;    //pointer to the form
        internal formInfo info = new formInfo() ;
        internal IntPtr shader;
        internal LinkedList<Pool> allControl;

        public Vec2d Size
        {
            get { return new Vec2d(info.width, info.height); }
            set
            {
                info.width = (int)value.X;
                info.height = (int)value.Y;
            }
        }

        public int Width
        {
            get => info.width;
            set => info.width = value;
        }

        public int Height
        {
            get => info.height;
            set => info.height = value;
        }

        /*
        public Vector4 Color
        {
            get => info.color;
            set => info.color = value;
        }
        /**/

        /// <summary>
        /// Constructor of a Form, creats an instance of a form and 
        /// initilize all the customizing operations.
        /// </summary>
        public Form()
        {

        }

    }
}
