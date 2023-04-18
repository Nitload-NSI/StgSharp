using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Control
{
    public abstract class TimelineBlock : INode<TimelineBlock>
    {

        public abstract void OnUpdating();

    }
}
