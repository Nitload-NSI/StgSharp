//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ShowViewPort"
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
using StgSharp.Controls;
using StgSharp.Graphics;
using StgSharp.MVVM;
using StgSharp.Threading;
using StgSharp.Timing;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace StgSharp.MVVM.ViewModel
{
    public abstract partial class ViewModelBase
    {

        private volatile bool _closed = true;
        private ViewBase _nextView, _currentView;

        protected ViewBase CurrentView
        {
            get => _currentView;
            set => _nextView = value;
        }

        /// <summary>
        ///   CustomizeInit this form and render it on screen, and respond with operation
        /// </summary>
        public unsafe void UseViewModel(string viewEntryName)
        {
            CustomizeInitialize();
            UseViewInNextFrame(viewEntryName);
            fpsCountProvider.TimeSpanRefreshed += CountFps;
            fpsCountProvider.MissTimeSpanRefreshed += CountFps;
            if (_nextView != _currentView) {
                _ = Interlocked.Exchange(ref _currentView, _nextView);
            }
            Thread renderThread = new Thread((token) => {
                try
                {
                    ProcessFrameWithoutEvent();
                }
                catch (Exception ex)
                {
                    DefaultLog.InternalWriteLog(
                            $"Critical error when rendering:\n" + $"{ex}", LogType.Error);
                    ShouldClose = true;
                    throw;
                }
            });
            renderThread.Start();
            /**/
            while (_closed)
            {
                if (_nextView != _currentView) {
                    _ = Interlocked.Exchange(ref _currentView, _nextView);
                }
                GraphicFramework.glfwPollEvents();
                frameTimeProvider.WaitNextSpan();
                _closed = !ShouldClose;
            }
            /**/
            renderThread.Join();
            fpsCountProvider.Dispose();
            frameTimeProvider.Dispose();
        }

        protected void UseViewInNextFrame(string name)
        {
            if (!allView.TryGetValue(name, out ViewBase v)) {
                throw new KeyNotFoundException(
                    $"{$"Cannot find View named {name} "}{$"in current View Model of {GetType().FullName}"}");
            }
            CurrentView = v;
        }

        private void CountFps(object sender, EventArgs e)
        {
            Interlocked.Exchange(ref _frameSpeed, _frameCount);
            Interlocked.Exchange(ref _frameCount, 0);
        }

        private unsafe void PrivateShow()
        {
            if (0 == GraphicFramework.glfwWindowShouldClose(this.viewPortDisplay.ViewPortHandle))
            {
                CurrentView.Use();
                RenderStream.PollEvents();
            }
        }

        private void ProcessFrameWithoutEvent()
        {
            frameTimeProvider.TimeSpanRefreshed += (_, _) => { };
            while (_closed)
            {
                foreach (ViewPort context in CurrentView.Render) {
                    context.FlushSize();
                }
                CurrentView.Use();
                frameTimeProvider.WaitNextSpan();
                Interlocked.Add(ref _frameCount, 1);
            }
        }

    }
}
