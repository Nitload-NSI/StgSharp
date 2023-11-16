using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics
{
    public unsafe partial class Form
    {
        internal GLcontext glContext;
        internal GLcontextIO* contextID;

        private int missingAPIcount = 0;
        private int problemAPIcount = 0;

    }
}
