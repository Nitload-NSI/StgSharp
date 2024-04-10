//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="FormThreading.cs"
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
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Graphics
{
    public abstract partial class Form
    {

        private List<FrameAsyncThread> multiFrameAsyncThreadList = new List<FrameAsyncThread>();
        private List<FrameAsyncThread> singleFrameAsyncThreadList = new List<FrameAsyncThread>();

        /// <summary>
        /// Init this form and render it on screen, and respond with operation
        /// </summary>
        public unsafe void Show()
        {
            Init();
            frameTimeProvider = new TimeSpanProvider(1 / 60.0, TimeSpanProvider.DefaultProvider);
            MethodInfo[] methods = this.GetType().GetMethods(
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.DeclaredOnly);
            GahterSignleFrameAsyncThread(methods);
            GatherMultiFrameAsyncThread(methods);
            //if no other operation
            if (singleFrameAsyncThreadList.Count == 0)
            {
                while (!ShouldClose)
                {
                    ProcessInput();
                    Main( );
                    InternalIO.glfwSwapBuffers(windowID);
                    InternalIO.glfwPollEvents();
                    frameTimeProvider.WaitNextSpan();
                    Interlocked.Add(ref _frameCount, 1);
                }
                return;
            }

            List<IGrouping<int, FrameAsyncThread>> threadGroup = singleFrameAsyncThreadList.GroupBy<FrameAsyncThread, int>((t) => t.Priority).ToList();

            if (threadGroup.Count == 1)
            {
                int p = threadGroup.First().Key;
                (Barrier, Barrier) bPair = FrameAsyncThread.CombineAsync(true, singleFrameAsyncThreadList);
                FrameAsyncThread.AsyncStartAll(singleFrameAsyncThreadList);
                switch (Scaler.SignOf(p))
                {
                    case 0:
                        while (!ShouldClose)
                        {
                            bPair.Item1.SignalAndWait();
                            ProcessInput();
                            Main( );
                            InternalIO.glfwSwapBuffers(windowID);
                            InternalIO.glfwPollEvents();
                            bPair.Item2.SignalAndWait();
                            Interlocked.Add(ref _frameCount, 1);
                            frameTimeProvider.WaitNextSpan();
                        }
                        break;
                    case 1:
                        while (!ShouldClose)
                        {
                            bPair.Item1.SignalAndWait();
                            bPair.Item2.SignalAndWait();
                            ProcessInput();
                            Main( );
                            InternalIO.glfwSwapBuffers(windowID);
                            InternalIO.glfwPollEvents();
                            Interlocked.Add(ref _frameCount, 1);
                            frameTimeProvider.WaitNextSpan();
                        }
                        break;
                    case -1:
                        while (!ShouldClose)
                        {
                            ProcessInput();
                            Main( );
                            InternalIO.glfwSwapBuffers(windowID);
                            InternalIO.glfwPollEvents();
                            bPair.Item1.SignalAndWait();
                            bPair.Item2.SignalAndWait();
                            Interlocked.Add(ref _frameCount, 1);
                            frameTimeProvider.WaitNextSpan();
                        }
                        break;
                    default:
                        return;
                }

                FrameAsyncThread.TerminateAll(singleFrameAsyncThreadList);
                bPair.Item1.SignalAndWait();
                return;
            }

            threadGroup.Sort();
            int priority = threadGroup.Count;               //rearrange priority label
            (Barrier, Barrier)[] barrierList = new (Barrier, Barrier)[priority];
            for (int i = 0; i < priority; i++)
            {
                var group = threadGroup[i].ToList();
                foreach (var t in group)
                {
                    t.Priority = priority;
                }
                barrierList[i] = FrameAsyncThread.CombineAsync(true, group);
                //setting thread sync
            }
            FrameAsyncThread.AsyncStartAll(singleFrameAsyncThreadList);
            while (!ShouldClose)
            {
                barrierList[0].Item1.SignalAndWait();
                int i = 0;
                while (i < priority - 1)
                {
                    barrierList[i].Item2.SignalAndWait();
                    barrierList[i + 1].Item1.SignalAndWait();
                }
                barrierList[priority - 1].Item1.SignalAndWait();
                InternalIO.glfwSwapBuffers(windowID);
                InternalIO.glfwPollEvents();
                frameTimeProvider.WaitNextSpan();
                Interlocked.Add(ref _frameCount, 1);
            }
            FrameAsyncThread.TerminateAll(singleFrameAsyncThreadList);
            foreach ((Barrier, Barrier) bPair in barrierList)
            {
                bPair.Item1.SignalAndWait();
                bPair.Item2.SignalAndWait();
            }
            FrameAsyncThread.WaitAll(singleFrameAsyncThreadList);
            //InternalDeinit();
        }

        private void GahterSignleFrameAsyncThread(MethodInfo[] methods)
        {
            if (methods.Length == 0)
            {
                return;
            }
            foreach (MethodInfo m in methods)       //find all methods decorated by RenderActivityAttribute
            {
                RenderActivityAttribute attr = m.GetCustomAttribute<RenderActivityAttribute>(false);
                if (attr == null)
                {
                    continue;
                }
                if (!m.IsStatic && (m.GetParameters().Length != 0) && (m.ReturnType != typeof(void)))
                {
                    throw new NotSupportedException("A render activity cannot have parameters, returning value or be static.");
                }
                ThreadStart s = (ThreadStart)Delegate.CreateDelegate(typeof(ThreadStart), this, m);
                FrameAsyncThread thread = new(s, attr._priority);
                singleFrameAsyncThreadList.Add(thread);
            }
        }

        private void GatherMultiFrameAsyncThread(MethodInfo[] methods)
        {
            if (methods.Length == 0)
            {
                return;
            }
            foreach (MethodInfo m in methods)       //find all methods decorated by RenderActivityAttribute
            {
                MultiFrameActivityAttribute attr = m.GetCustomAttribute<MultiFrameActivityAttribute>(false);
                if (attr == null)
                {
                    continue;
                }
                if (!m.IsStatic && (m.GetParameters().Length != 0) && (m.ReturnType != typeof(void)))
                {
                    throw new NotSupportedException("A render activity cannot have parameters, returning value or be static.");
                }
                ThreadStart s = (ThreadStart)Delegate.CreateDelegate(typeof(ThreadStart), this, m);
                FrameAsyncThread thread = new(s, 0);
                multiFrameAsyncThreadList.Add(thread);
            }
        }

    }
}
