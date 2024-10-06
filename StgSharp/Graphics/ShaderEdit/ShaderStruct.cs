//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ShaderStruct.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
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
            params (GlHandle, ShaderStructMember)[] uniformPairs )
        {
            uniformPair = new Dictionary<GlHandle, ShaderStructMember>();
            if( uniformPairs.Length == 0 ) {
                return;
            }

            foreach( (GlHandle, ShaderStructMember) item in uniformPairs ) {
                uniformPair.Add( item.Item1, item.Item2 );
            }
        }

        public unsafe void SetAllUniforms()
        {
            OpenglContext* gl = ( OpenglContext* )OpenGL.OpenGLFunction.CurrentGL.ContextHandle;
            foreach( KeyValuePair<GlHandle, ShaderStructMember> uniform in uniformPair ) {
                int id = uniform.Key.SignedValue;
                ShaderStructMember s = uniform.Value;
                switch( s.type ) {
                    case InternalShaderType.Struct:
                        throw new ArgumentException(
                            "Incorrect type \"Struct\" defined in this self define type" );
                    case InternalShaderType.Void:
                        throw new ArgumentException(
                            "Incorrect type \"Void\" defined in this self define type" );
                    case InternalShaderType.Float:
                        gl->glUniform1f( id, ( float )s.value );
                        break;
                    case InternalShaderType.Int:
                        gl->glUniform1f( id, ( int )s.value );
                        break;
                    default:
                        throw new NotImplementedException();
                        throw new ArgumentException(
                            "Incorrect type \"Unknown\" defined in this self define type" );
                }
            }
        }//------------------------------------ End of Class ---------------------------------------

    }
}
