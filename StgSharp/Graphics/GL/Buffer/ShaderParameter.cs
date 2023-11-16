using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StgSharp.Graphics.Buffer
{
    public struct ShaderParameter<T>
    {
        private static readonly string[] supportedType = new string[] {
            "Single",
            "Int32",
            "Uint32",
            "Single*"
            };

        internal int signiture;
        public readonly string name;

        internal ShaderParameter(string name, int sign)
        {
            if (ShaderParameter<T>.supportedType.Contains(typeof(T).Name))
            {
                InternalIO.InternalWriteLog($"Invalid type of ShaderProgram named {name}.", LogType.Error);
                return;
            }

            signiture = sign;
            this.name = name;
        }

    }
}
