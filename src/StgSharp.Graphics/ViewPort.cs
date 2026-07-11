// -----------------------------------------------------------------------------
// file="ViewPort"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace StgSharp.Graphics
{
    public class ViewPort
    {

        private static readonly ConcurrentDictionary<IntPtr, ViewPort> _handleToViewPortIndex = [];
        private int _width, _height, _newWidth, _newHeight;

        internal unsafe ViewPort()
        {
            _newHeight = 600;
            _newWidth = 800;
            FlushSize();
            Name = new StackFrame(1, false).
                GetMethod()!.DeclaringType!.Name;
            Monitor = IntPtr.Zero;
            ViewPortHandle = GraphicFramework.glfwCreateWindow(_newWidth, _newHeight,
                                                               Encoding.UTF8.GetBytes(Name),
                                                               Monitor, IntPtr.Zero);
            if (ViewPortHandle == IntPtr.Zero) {
                throw new InvalidOperationException("Unable to create viewport handle.");
            }
            _ = _handleToViewPortIndex.TryAdd(ViewPortHandle, this);
        }

        public unsafe ViewPort(
                      int width,
                      int height,
                      string name,
                      IntPtr monitor
        )
        {
            _newWidth = width;
            _newHeight = height;
            FlushSize();
            Name = name;
            Monitor = monitor;
            ViewPortHandle = GraphicFramework.glfwCreateWindow(_newWidth, _newHeight,
                                                               Encoding.UTF8.GetBytes(Name),
                                                               Monitor, IntPtr.Zero);
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
        public static bool GetExistedViewPort(
                           IntPtr handle,
                           out ViewPort port
        )
        {
            return _handleToViewPortIndex.TryGetValue(handle, out port);
        }

        internal unsafe void FlushSize()
        {
            _ = Interlocked.Exchange(ref _height, _newHeight);
            _ = Interlocked.Exchange(ref _width, _newWidth);
        }

        internal void RequestFlushSizeInNextFrame(
                      int width,
                      int height
        )
        {
            _ = Interlocked.Exchange(ref _newHeight, height);
            _ = Interlocked .Exchange(ref _newWidth, width);
        }

    }
}
