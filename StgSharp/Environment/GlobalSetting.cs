//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="GlobalSetting.cs"
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp
{
    public static unsafe partial class StgSharp
    {

        private static bool _inited;

        /// <summary>
        /// Init an instance of OpenGL program,
        /// This method should be called before any other StgSharp api.
        /// </summary>
        public static void InitGL(int majorVersion, int minorVersion)
        {
            if (API == default)
            {
                API = GraphicAPI.GL;
            }
            if (API != GraphicAPI.GL)
            {
                return;
            }
            InternalIO.InternalInitGL(majorVersion, minorVersion);
        }

        public static class GlobalSetting
        {

            private static bool vSyncActivated;

            public static bool VSyncActivated
            {
                get => vsyncActivated;
                set
                {
                    InternalIO.glfwSwapInterval(value ? 1 : 0);
                    vsyncActivated = value;
                }
            }

        }

    }
}