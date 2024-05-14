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
using StgSharp.Logic;

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
        private Blueprint renderLogic;

        public Blueprint RenderLogic
        {
            get => renderLogic;
            set => renderLogic = value;
        }

        /// <summary>
        /// Init this form and render it on screen, and respond with operation
        /// </summary>
        public unsafe void Show()
        {
            while (!ShouldClose)
            {
                renderLogic.RunRepeatedly(() =>  ShouldClose);
                frameTimeProvider.WaitNextSpan();
                Interlocked.Add(ref _frameCount, 1);
            }
        }

        /*
        private void GatherSingleFrameAsyncThread(MethodInfo[] methods)
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
                //singleFrameAsyncThreadList.Add(thread);
            }
        }
        /**/

    }
}
