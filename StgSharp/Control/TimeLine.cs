//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="TimeLine.cs"
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
using StgSharp.Entities;
using StgSharp.Graphics;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace StgSharp.Controlling
{
    internal static partial class TimeLine
    {

        private static bool isPaused = false;
        private static bool renderChainStarted;

        internal static TimelineBlock currentOperation;
        internal static Queue<Form> formList = new Queue<Form>();

        public static TimelineBlock CurrentOperation
        {
            get => currentOperation;
            set
            {
                if ((currentOperation == default) ||
                    currentOperation.isComplete)
                {
                    currentOperation = value;
                }
            }
        }


        internal static bool RenderStarted => renderChainStarted;


        internal static void RenderAll()
        {
            renderChainStarted = true;
        }



    }
}
