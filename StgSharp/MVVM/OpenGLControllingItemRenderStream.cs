using StgSharp.Graphics.OpenGL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.MVVM
{
    public class OpenGLControllingItemRenderStream : glRenderStream
    {
        public override bool IsContextSharable => true;

        protected override void CustomizeDeinit()
        {
            throw new NotImplementedException();
        }

        protected override void CustomizeInit()
        {
            throw new NotImplementedException();
        }

        protected override void Main()
        {
            throw new NotImplementedException();
        }
    }
}
