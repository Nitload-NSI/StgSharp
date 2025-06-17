//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="glType.cs"
//     Project: StepVisualizer
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
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Internal.OpenGL
{
    internal delegate void GLVULKANPROCNV();

    public unsafe struct glHandleARB
    {

        private dynamic value;

        public unsafe glHandleARB( dynamic value )
        {
            if( !RuntimeInformation.IsOSPlatform( OSPlatform.OSX ) )
            {
                if( value.GetType().Name != "Uint32" )
                {
                    goto errorlog;
                }
                this.value = value;
            } else
            {
                if( value.GetType().Name != "IntPtr" )
                {
                    goto errorlog;
                }
                this.value = value;
            }

            errorlog:
            {
                InternalIO.InternalWriteLog(
                    $"{value.GetType().Name} does not match definination of GLhandlARB on OS {Environment.OSVersion.Platform} ",
                    LogType.Error );
            }
        }

        public static implicit operator uint( glHandleARB value )
        {
            return ( uint )value;
        }

    }
}