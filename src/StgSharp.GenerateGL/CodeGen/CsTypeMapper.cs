//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="CsTypeMapper"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using System;

namespace GenerateGL.CodeGen
{
    internal static class CsTypeMapper
    {

        public static string MapDelegateParameter(
                             GlParam param
        )
        {
            int pointerLevel = param.PointerLevel;
            if (param.CType.Equals("GLchar", StringComparison.Ordinal) && pointerLevel > 0) {
                return $"byte{new string('*', pointerLevel)}";
            }

            string mapped = MapType(param.CType, allowVoid:false);
            if (pointerLevel > 0)
            {
                if (mapped == "void") {
                    return $"void{new string('*', pointerLevel)}";
                }
                return $"{mapped}{new string('*', pointerLevel)}";
            }
            return mapped;
        }

        public static string MapDelegateReturn(
                             string cType,
                             int pointerLevel
        )
        {
            string mapped = MapType(cType, allowVoid:true);
            if (pointerLevel > 0) {
                return $"{mapped}{new string('*', pointerLevel)}";
            }
            return mapped;
        }

        public static string MapParameter(
                             GlParam param
        )
        {
            if (param.PointerLevel == 0)
            {
                if (param.CType.Equals("GLenum", StringComparison.Ordinal) &&
                    LooksLikeShaderType(param.Name) &&
                    !param.IsPointer) {
                    return "ShaderType";
                }

                if (param.CType.Equals("GLuint", StringComparison.Ordinal) &&
                    LooksLikeHandle(param.Name) &&
                    !param.IsPointer) {
                    return "GlHandle";
                }
            }

            int pointerLevel = param.PointerLevel;
            if (param.CType.Equals("GLchar", StringComparison.Ordinal) && pointerLevel > 0) {
                return $"byte{new string('*', pointerLevel)}";
            }

            string mapped = MapType(param.CType, allowVoid:false);
            if (pointerLevel > 0)
            {
                if (mapped == "void") {
                    return $"void{new string('*', pointerLevel)}";
                }
                return $"{mapped}{new string('*', pointerLevel)}";
            }
            return mapped;
        }

        public static string MapReturn(
                             string cType,
                             int pointerLevel
        )
        {
            string mapped = MapType(cType, allowVoid:true);
            if (pointerLevel > 0) {
                return $"{mapped}{new string('*', pointerLevel)}";
            }
            return mapped;
        }

        public static bool TryMapConstPointerToSpan(
                           GlParam param,
                           out string spanElementType
        )
        {
            spanElementType = string.Empty;
            if (!param.IsConst || param.PointerLevel != 1) {
                return false;
            }

            string mapped = MapType(param.CType, allowVoid:false);
            if (mapped is "void") {
                return false;
            }

            spanElementType = mapped;
            return true;
        }

        private static bool LooksLikeHandle(
                            string name
        )
        {
            if (string.IsNullOrWhiteSpace(name)) {
                return false;
            }

            return name.Contains("program", StringComparison.OrdinalIgnoreCase) ||
                   name.Contains("shader", StringComparison.OrdinalIgnoreCase) ||
                   name.Contains("buffer", StringComparison.OrdinalIgnoreCase) ||
                   name.Contains("texture", StringComparison.OrdinalIgnoreCase) ||
                   name.Contains("framebuffer", StringComparison.OrdinalIgnoreCase) ||
                   name.Contains("renderbuffer", StringComparison.OrdinalIgnoreCase) ||
                   name.Contains("vertexarray", StringComparison.OrdinalIgnoreCase) ||
                   name.Contains("query", StringComparison.OrdinalIgnoreCase) ||
                   name.Contains("sampler", StringComparison.OrdinalIgnoreCase) ||
                   name.Contains("transformfeedback", StringComparison.OrdinalIgnoreCase);
        }

        private static bool LooksLikeShaderType(
                            string name
        )
        {
            if (string.IsNullOrWhiteSpace(name)) {
                return false;
            }
            return name.Contains("shadertype", StringComparison.OrdinalIgnoreCase) ||
                   name.Equals("type", StringComparison.OrdinalIgnoreCase);
        }

        private static string MapType(
                              string cType,
                              bool allowVoid
        )
        {
            return cType switch
            {
                "void" when allowVoid => "void",
                "GLenum" or "GLbitfield" => "uint",
                "GLboolean" => "byte",
                "GLuint" or "GLhandleARB" => "uint",
                "GLint" or "GLsizei" or "GLfixed" => "int",
                "GLshort" => "short",
                "GLbyte" => "sbyte",
                "GLubyte" => "byte",
                "GLushort" => "ushort",
                "GLfloat" => "float",
                "GLdouble" => "double",
                "GLclampf" or "GLclampd" => "float",
                "GLintptr" => "nint",
                "GLsizeiptr" => "nuint",
                "GLchar" => "byte",
                "GLsync" => "GLsync",
                _ => allowVoid && cType == "void" ? "void" : "IntPtr"
            };
        }

    }
}
