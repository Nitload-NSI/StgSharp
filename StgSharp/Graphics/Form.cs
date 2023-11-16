using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace StgSharp.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct formInfo
    {
        public int width;
        public int height;
        public Vector4 color;
        public string name;
    }

    public unsafe partial class Form
    {

        internal LinkedList<Pool> allControl;
        internal Vector4 color;
        internal int height;
        internal IntPtr monitor;
        internal string name;
        internal Form sharedForm = null;
        internal int width;
        internal IntPtr windowID = default;

        /// <summary>
        /// Constructor of a Form, creats an instance of a form and 
        /// initilize all the customizing operations.
        /// </summary>
        public Form(int width, int height, string name, IntPtr monitor, Form sharedForm)
        {
            this.width = width;
            this.height = height;
            this.name = name;
            this.sharedForm = sharedForm;
            this.monitor = monitor;
        }

        internal Form()
        { }

        ~Form()
        {
            GL.gl = default;
            Marshal.FreeHGlobal((IntPtr)contextID);
        }

        public int Height
        {
            get => height;
            set => height = value;
        }

        public Vec2d Size
        {
            get { return new Vec2d(width, height); }
            set
            {
                width = (int)value.X;
                height = (int)value.Y;
            }
        }

        public int Width
        {
            get => width;
            set => width = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void MakeAsCurrentContext()
        {
            if (windowID == default)
            {
                IntPtr windowPtr;
                byte[] tittleBytes = Encoding.UTF8.GetBytes(name);
                if (sharedForm == null)
                {
                    windowPtr = InternalIO.glfw.CreateWindow(
                        width, height, tittleBytes, monitor, IntPtr.Zero);
                }
                else
                {
                    windowPtr = InternalIO.glfw.CreateWindow(
                        width, height, tittleBytes, monitor, sharedForm.windowID);
                }

                if ((long)windowPtr == -1 || (long)windowPtr == -2)
                {
                    throw new Exception("Cannot create window instance!");
                }
                else
                {
                    windowID = windowPtr;
                }
                glContext = new GLcontext();
                IntPtr ptr = Marshal.AllocHGlobal(sizeof(GLcontextIO));
                contextID = (GLcontextIO*)ptr;
                *contextID = new GLcontextIO();
                InternalIO.InternalWriteLog("Successfully create a context binded to current form. " +
                    $"Its ID is {(ulong)ptr}", LogType.Info);
            }
            GL.gl = glContext;
            InternalIO.InternalSetCurrentGLContext(windowID, (IntPtr)contextID);
        }

        /// <summary>
        /// Resize the current form
        /// </summary>
        /// <param name="width">New width of the window</param>
        /// <param name="height">New height of the window</param>
        public void SetSize(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            InternalIO.glfw.SetWindowSize(this.windowID, width, height);
        }

        /// <summary>
        /// Resize the current form
        /// </summary>
        /// <param name="size">
        /// A <see cref="Vec2d"/> (width,height) representing the new size of current form 
        /// </param>
        public void SetSize(Vec2d size)
        {
            this.Size = size;
            InternalIO.glfw.SetWindowSize(this.windowID, (int)size.X, (int)size.Y);

        }

        /// <summary>
        /// Set the size limit of current form
        /// </summary>
        /// <param name="minWidth">Minimum width of current form </param>
        /// <param name="minHeight">Minimum height of current form</param>
        /// <param name="maxWidth">Maximum width of curent form</param>
        /// <param name="maxHeight">Maximum height of current form</param>
        public void SetSizeLimit(int minWidth, int minHeight, int maxWidth, int maxHeight)
        {
            InternalIO.glfw.SetWindowSizeLimits(
                this.windowID,
                minWidth, minHeight, maxWidth, maxHeight);
        }

        /// <summary>
        /// Set the size limit of current form
        /// </summary>
        /// <param name="minSize">
        /// Minimun size of current form. In form of a <see cref="Vec2d"/> of (width,height)
        /// </param>
        /// <param name="maxSize">
        /// Maximum size of current form. In form of a <see cref="Vec2d"/> of (width,height)
        /// </param>
        public void SetSizeLimit(Vec2d minSize, Vec2d maxSize)
        {
            InternalIO.glfw.SetWindowSizeLimits(
                this.windowID,
                (int)minSize.X, (int)minSize.Y, (int)maxSize.X, (int)maxSize.Y);
        }
    }
}
