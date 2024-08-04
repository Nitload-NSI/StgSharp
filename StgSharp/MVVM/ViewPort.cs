//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ViewPort.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
using StgSharp.Graphics;
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace StgSharp.MVVM
{
    public class ViewPort
    {

        private FrameBufferSizeHandler _sizeHandler;
        private FramePositionHandler _positionHandler;
        private int _width, _height;

        internal ViewPort()
        {
            _height = 600;
            _width = 800;
            Name = new StackFrame(1, false).
                GetMethod()!.DeclaringType!.Name;
            Monitor = IntPtr.Zero;
            ViewPortID = InternalIO.glfwCreateWindow(_width, _height,
           Encoding.UTF8.GetBytes(Name), Monitor, IntPtr.Zero
                );
            if (ViewPortID == IntPtr.Zero)
            {
                throw new InvalidOperationException("Unable to create viewport handle.");
            }
        }

        public ViewPort(vec2d size, string name, IntPtr monitor)
        {
            _width = (int)size.X;
            _height = (int)size.Y;
            Name = name;
            Monitor = monitor;
            ViewPortID = InternalIO.glfwCreateWindow(_width, _height,
           Encoding.UTF8.GetBytes(Name), Monitor, IntPtr.Zero
                );
            if (ViewPortID == IntPtr.Zero)
            {
                throw new InvalidOperationException("Unable to create viewport handle.");
            }
        }

        public FrameBufferSizeHandler FrameSizeCallback
        {
            set
            {
                _sizeHandler = value;
                IntPtr handlerPtr = Marshal.GetFunctionPointerForDelegate(_sizeHandler);
                InternalIO.glfwSetFramebufferSizeCallback(ViewPortID, handlerPtr);
            }
        }

        public FramePositionHandler FramePostionCallback
        {
            set
            {
                _positionHandler = value;
                IntPtr handlerPtr = Marshal.GetFunctionPointerForDelegate(_positionHandler);
                InternalIO.glfwSetWindowPosCallback(ViewPortID, handlerPtr);
            }
        }

        public unsafe int Height
        {
            get { return _height; }
            set { InternalIO.glfwSetWindowSize(ViewPortID, _width, value); }
        }

        public unsafe int Width
        {
            get { return _width; }
            set { InternalIO.glfwSetWindowSize(ViewPortID, value, _height); }
        }

        public IntPtr GraphicHandle { get; internal set; }

        public IntPtr Monitor { get; internal set; }

        public IntPtr ViewPortID { get; private set; }

        public string Name { get; set; }

        public void Close()
        {
            InternalIO.glfwSetWindowShouldClose(ViewPortID, 1);
        }

        internal unsafe void FlushSize()
        {
            fixed (int* wptr = &_width, hptr = &_height)
            {
                InternalIO.glfwGetWindowSize(ViewPortID, wptr, hptr);
            }
        }

    }
}
