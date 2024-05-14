using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics.ShaderEdit
{
    public class ShaderStructMember
    {
        internal InternalShaderType type;
        internal object value;
    }

    public class ShaderStruct
    {
        internal Dictionary<glSHandle, ShaderStructMember> uniformPair;

        public ShaderStruct(params (glSHandle, ShaderStructMember)[] UniformPais)
        {
            uniformPair = new Dictionary<glSHandle, ShaderStructMember>();
            if (UniformPais.Length == 0)
            {
                return;
            }

            foreach (var item in UniformPais)
            {
                uniformPair.Add(item.Item1,item.Item2);
            }

        }

        internal ShaderStruct() { }


        public unsafe void SetAllUniforms()
        {
            GLcontext* gl = (GLcontext*) GL.CurrentContextHandle;
            foreach (var uniform in uniformPair)
            {
                int id = uniform.Key.Value;
                ShaderStructMember s = uniform.Value;
                switch (s.type)
                {
                    case InternalShaderType.Struct: throw new ArgumentException(
                        "Incorrect type \"Struct\" defined in this self define type");
                    case InternalShaderType.Void:throw new ArgumentException(
                        "Incorrect type \"Void\" defined in this self define type");
                    case InternalShaderType.Float:
                        gl->glUniform1f(id, (float)s.value);
                        break;
                    case InternalShaderType.Int:
                        gl->glUniform1f(id, (int)s.value);
                        break;
                    default:
                        throw new NotImplementedException();
                        throw new ArgumentException(
                            "Incorrect type \"Unknown\" defined in this self define type");
                }
            }
        }
    }
}
