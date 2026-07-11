// -----------------------------------------------------------------------------
// file="GlobalSetting"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

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

        private static byte[] _currentHash = [];

        private static bool _inited;

        public static byte[] CurrentAssemblyHash
        {
            get
            {
                if (_currentHash == null)
                {
                    string route = Assembly.GetExecutingAssembly().Location;
                    string time = DateTime.UtcNow.ToString();
                    using (SHA256 sh = SHA256.Create())
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(route);
                        _currentHash = sh.ComputeHash(bytes);
                    }
                }
                return _currentHash;
            }
        }

        public static int MainThreadID
        {
            get => (_mainThreadID == -1) ?
                   throw new InvalidOperationException(
                "StgSharp environment is not inited. Main thread id is not available") :
                   _mainThreadID;
            internal set => _mainThreadID = value;
        }

        private static int _mainThreadID { get; set; } = -1;

        public static class GlobalSetting
        {

            private static bool vSyncActivated;

            public static bool VSyncActivated
            {
                get => vsyncActivated;
                set =>
 // GraphicFramework.glfwSwapInterval(value ? 1 : 0);
 vsyncActivated = value;
            }

        }

    }//-------------------------------------- End of Class ---------------------------------------//
}