using StgSharp.Math;
using System;
using System.Runtime.InteropServices;

namespace StgSharp.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct formInfo
    {
        public int width;
        public int height;
        public vec4d color;
        public byte[] name;
    }

    public unsafe class Form
    {
        internal IntPtr windowPtr;
        internal formInfo info;
        internal IntPtr shader;
        internal LinkedList<Pool> allControl = new LinkedList<Pool>();

        public vec2d Size
        {
            get { return new vec2d(info.width, info.height); }
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

        public vec4d Color
        {
            get => info.color;
            set => info.color = value;
        }

        /// <summary>
        /// Constructor of a Form, creats an instance of a form and 
        /// initilize all the customizing operations.
        /// </summary>
        public Form()
        {

        }

    }
}
