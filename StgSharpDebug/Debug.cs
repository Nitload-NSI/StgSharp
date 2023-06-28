using StgSharp;
using StgSharp.Control;
using StgSharp.Geometries;
using StgSharp.Graphics;
using StgSharp.Math;
using System;

namespace StgSharp
{

    internal class Program
    {
        private unsafe static void Main(string[] args)
        {

            Console.WriteLine(sizeof(int*));
            Graphic.LoadGL();
            Graphic.InitGL();
            Graphic.InitGLAD();

            Form form = Graphic.GLcreateWindow
                (800, 600, "你好 Stg#!", IntPtr.Zero, IntPtr.Zero);

            /*
            Form f2 = new Form();
            f2.Color = new vec4d(0.1,0.5,0.5,0.1);
            f2.Init();
            /**/
        }


    }
}
    

