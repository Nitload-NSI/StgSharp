using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp
{

    public static partial class StgSharp
    {
        private static ConcurrentDictionary<int, KeyStatus> keyListener 
            = new ConcurrentDictionary<int, KeyStatus>();

        public static KeyStatus CheckKey(KeyboardKey key)
        {
            return keyListener.GetOrAdd((int)key, KeyStatus.RELEASE);
        }


    }
}
