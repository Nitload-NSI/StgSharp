// -----------------------------------------------------------------------------
// file="ShaderStruct"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Graphics;
using StgSharp.Graphics.OpenGL;

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

        internal Dictionary<GlHandle, ShaderStructMember> uniformPair;

        internal ShaderStruct() { }

        public ShaderStruct(
               params (GlHandle, ShaderStructMember)[] uniformPairs
        )
        {
            uniformPair = new Dictionary<GlHandle, ShaderStructMember>();
            if (uniformPairs.Length == 0) {
                return;
            }

            foreach ((GlHandle, ShaderStructMember) item in uniformPairs) {
                uniformPair.Add(item.Item1, item.Item2);
            }
        }

        public unsafe void SetAllUniforms()
        {
            OpenglContext* gl = (OpenglContext*)OpenGL.OpenGLFunction.CurrentGL.ContextHandle;
            foreach (KeyValuePair<GlHandle, ShaderStructMember> uniform in uniformPair)
            {
                int id = uniform.Key.SignedValue;
                ShaderStructMember s = uniform.Value;
                switch (s.type)
                {
                    case InternalShaderType.Struct:
                        throw new ArgumentException(
                            "Incorrect type \"Struct\" defined in this self define type");
                    case InternalShaderType.Void:
                        throw new ArgumentException(
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
        }//------------------------------------ End of Class ---------------------------------------

    }
}
