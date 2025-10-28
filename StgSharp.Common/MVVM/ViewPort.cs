//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ViewPort"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace StgSharp.MVVM
{
    public class ViewPort
    {

        private static readonly ConcurrentDictionary<IntPtr, ViewPort> _handleToViewPortIndex = [];
        private int _width, _height, _newWidth, _newHeight;

        internal ViewPort()
        {
            _newHeight = 600;
            _newWidth = 800;
            FlushSize();
            Name = new StackFrame(1, false).
                GetMethod()!.DeclaringType!.Name;
            Monitor = IntPtr.Zero;
            ViewPortHandle = GraphicFramework.glfwCreateWindow(
                _newWidth, _newHeight, Encoding.UTF8.GetBytes(Name), Monitor, IntPtr.Zero);
            if (ViewPortHandle == IntPtr.Zero) {
                throw new InvalidOperationException("Unable to create viewport handle.");
            }
            _ = _handleToViewPortIndex.TryAdd(ViewPortHandle, this);
        }

        public ViewPort(int width, int height, string name, IntPtr monitor)
        {
            _newWidth = width;
            _newHeight = height;
            FlushSize();
            Name = name;
            Monitor = monitor;
            ViewPortHandle = GraphicFramework.glfwCreateWindow(
                _newWidth, _newHeight, Encoding.UTF8.GetBytes(Name), Monitor, IntPtr.Zero);
            if (ViewPortHandle == IntPtr.Zero) {
                throw new InvalidOperationException("Unable to create viewport handle.");
            }
            _ = _handleToViewPortIndex.TryAdd(ViewPortHandle, this);
        }

        public unsafe int Height
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _height;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal set => Interlocked.Exchange(ref _height, value);
        }

        public unsafe int Width
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _width;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal set => Interlocked.Exchange(ref _width, value);
        }

        public IntPtr GraphicHandle { get; internal set; }

        public IntPtr Monitor { get; internal set; }

        public IntPtr ViewPortHandle { get; private set; }

        public string Name { get; set; }

        // [BlueprintNodeExecution]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetExistedViewPort(IntPtr handle, out ViewPort port)
        {
            return _handleToViewPortIndex.TryGetValue(handle, out port);
        }

        internal unsafe void FlushSize()
        {
            _ = Interlocked.Exchange(ref _height, _newHeight);
            _ = Interlocked.Exchange(ref _width, _newWidth);
        }

        internal void RequestFlushSizeInNextFrame(int width, int height)
        {
            _ = Interlocked.Exchange(ref _newHeight, height);
            _ = Interlocked .Exchange(ref _newWidth, width);
        }

    }
}
