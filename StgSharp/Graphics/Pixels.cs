using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics
{
    public interface IPixel
    {
        internal int Size { get; }

        public float Red { get; set; }
        public float Green { get; set; }
        public float Blue { get; set; }
        public float Alpha { get; set; }

        
    }

    [StructLayout(LayoutKind.Explicit,Size = 32)]
    public unsafe struct Pixel32
    {
        [FieldOffset(0)]private unsafe fixed byte data[4];

        internal int Size => 4;
    }



}
