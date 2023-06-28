using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.StgSharpTest
{

    public static class PInvokeTest
    {
		[DllImport("StgSharpGraphic.dll",EntryPoint ="getPtr")]
		internal static extern IntPtr getPtr();

        public static void GLloadtest()
        {

			try
			{
				IntPtr ptr = Marshal.GetFunctionPointerForDelegate(Graphic.glIO.internalGLcreateWindow);

				Console.WriteLine(ptr);

				IntPtr ptr1 = getPtr();

				Console.WriteLine(ptr1);

			}
			catch (Exception)
			{

				throw;
			}

        }
    }
}
