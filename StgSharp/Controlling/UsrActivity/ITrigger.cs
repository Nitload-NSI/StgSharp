using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Controlling.UsrActivity
{
    public interface ITrigger
    {
        public int TargetKeyOrButtonID { get; }
        public KeyStatus TriggeredStatus { get; }
    }
}
