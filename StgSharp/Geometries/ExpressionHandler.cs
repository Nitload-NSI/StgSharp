using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Geometries
{
    public delegate float ExpressionHandler
        (
        Counter<uint> tickCounter,
        float beginRange,
        float endRange
        );
}
