using StgSharp.Controlling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace StgSharp.Graphics
{
    public static unsafe partial class Graphic
    {



        internal static TDele ConvertAPI<TDele>(IntPtr funcPtr)
        {
            if (funcPtr == IntPtr.Zero)
            {
                throw new Exception("Failed to convert api, the function pointer is zero.");
            }
            try
            {
                return Marshal.GetDelegateForFunctionPointer<TDele>(funcPtr);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal static Delegate ConvertAPI(IntPtr funcPtr, Type T)
        {

            if (funcPtr == IntPtr.Zero)
            {
                throw new Exception("Failed to convert api, the function pointer is zero.");
            }
            try
            {
                return Marshal.GetDelegateForFunctionPointer(funcPtr, T);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Init an instance of OpenGL program,
        /// This method should be called before other 
        /// graphic methods, after LoadGL method.
        /// </summary>
        public static void InitGL(int majorVersion, int minorVersion)
        {
            if (InternalIO.glfw == null)
            {
                try
                {
                    InternalIO.glfw = new glFramework_PInvoke();
                    InternalIO.glfw.Init();
                    InternalIO.InternalWriteLog("Successfully load GLFW APIs. StgSharp is now working on p/invoke mode", LogType.Info);
                }
                catch (Exception sig)
                {
                    string log = $"Failed to load GLFW APIs by P/Invoke," +
                        $"StgSharp will switch to delegate mode. The detailed info is:\n" +
                        $"{sig.Message}\n";

                    if (InternalIO.SSClibPtr == default)
                    {
                        InternalIO.SSClibPtr =
                            InternalIO.InternalLoadWINlib(InternalIO.SSC_libname + ".dll");
                    }

                    InternalIO.glfw = new glFramework_Delegate();
                    InternalIO.SSClibPtr = InternalIO.InternalLoadWINlib(InternalIO.SSC_libname);
                    FieldInfo[] glfwApis = typeof(glFramework_Delegate).GetFields();
                    foreach (FieldInfo item in glfwApis)
                    {
                        if (item.FieldType.BaseType == typeof(Delegate))
                        {
                            try
                            {
                                IntPtr funcptr = InternalIO.InternalGetWINproc(
                                    InternalIO.SSClibPtr, item.Name);
                                item.SetValue(null, ConvertAPI(funcptr, item.FieldType));
                                log += $"Successfully load GLFW API called {item.Name}";
                            }
                            catch (Exception ex)
                            {
                                log += $"Failed to load GLFW API called {item.Name}, the detailed info is\n" +
                                    $"{ex.Message}";
                                continue;
                            }
                        }
                    }
                    InternalIO.InternalWriteLog(log, LogType.Warning);
                }
            }
            else
            {
                InternalIO.InternalWriteLog("Attempt to load GLFW APIs twice", LogType.Warning);
            }
            InternalIO.InternalInitGL(4, 6);

        }





    }
}
