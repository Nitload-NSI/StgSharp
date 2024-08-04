using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Graphics.OpenGL
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class OpenGLAPICallingAttribute:Attribute
    {
        public OpenGLAPICallingAttribute()
        {
            
        }
    }
}
