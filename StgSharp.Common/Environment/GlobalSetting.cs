﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="GlobalSetting.cs"
//     Project: World
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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp
{
    public static unsafe partial class World
    {

        internal const int ssdSegmentLength = 16;

        private static byte[] _currentHash;

        private static bool _inited;
        private static int _mainThreadID = -1;

        public static byte[] CurrentAssemblyHash
        {
            get
            {
                if( _currentHash == null ) {
                    string route = Assembly.GetExecutingAssembly().Location;
                    string time = DateTime.UtcNow.ToString();
                    using( SHA256 sh = SHA256.Create() ) {
                        byte[] bytes = Encoding.UTF8.GetBytes( route );
                        _currentHash = sh.ComputeHash( bytes );
                    }
                }
                return _currentHash;
            }
        }

        public static int MainThreadID
        {
            get => ( _mainThreadID == -1 ) ?
            throw new InvalidOperationException(
                "StgSharp environment is not inited. " + "Main thread id is not available" ) : _mainThreadID;
        }

        /// <summary>
        /// CustomizeInit an instance of OpenGL program, This method should be called before any
        /// other World api.
        /// </summary>
        public static void InitGL( int majorVersion, int minorVersion )
        {
            if( API == default ) {
                API = GraphicAPI.GL;
            }
            if( API != GraphicAPI.GL ) {
                return;
            }
            InternalIO.InternalInitGL( majorVersion, minorVersion );
        }

        public static class GlobalSetting
        {

            private static bool vSyncActivated;

            public static bool VSyncActivated
            {
                get => vsyncActivated;
                set
                {
                    InternalIO.glfwSwapInterval( value ? 1 : 0 );
                    vsyncActivated = value;
                }
            }

        }

    }//-------------------------------------- End of Class ---------------------------------------//
}