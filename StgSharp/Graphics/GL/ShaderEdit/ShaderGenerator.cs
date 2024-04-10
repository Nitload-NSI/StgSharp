//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ShaderGenerator.cs"
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
using StgSharp.Graphics;
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics.ShaderEdit
{
    public class ShaderGenerator
    {
        private Dictionary<int,ShaderParameter> uniformDefine;
        private List<string> codeList;
        private List<ShaderFunction> usedShaderFunctions;

        public Uniform<T> DefineUniform<T>(string name, int index) where T : struct
        {
            ShaderParameter p = new ShaderParameter(name,ShaderParameter.TypeMarshal<T>());
            uniformDefine.Add(index,p);
            //return new Uniform<T>();
            throw new NotImplementedException();
        }

    }

    public class ShaderFunction
    {
        private List<string> codeList;
        private List<ShaderParameter> inputList;
        private string name;
        private ShaderParameter outPut;

        public int ParameterCount => inputList.Count;

        public void EmitCall(ShaderFunction function, ShaderParameter outPut, params ShaderParameter[] inputParams)
        {
            if (inputParams.Length != function.ParameterCount)
            {
                // Param list length not equal
                throw new ArgumentException("Provided parameters does not match function's parameter list.");
            }
            if (!ShaderParameter.IsSameType(outPut, function.outPut))
            {
                // Type of returning value not qual
                throw new Exception("Provided parameters does not match function's parameter list.");
            }
            for (int i = 0; i < inputParams.Length; i++)
            {
                if (!ShaderParameter.IsSameType(function.inputList[i], inputParams[i]))
                {
                    // Input type not match
                    throw new Exception("Provided parameters does not match function's parameter list.");
                }
            }
            string codeLine = string.Empty;    // adding a line like " outvalue = Func(input1, input2,...); "
            if (outPut != ShaderParameter.Void)
            {
                codeLine += $"{outPut.name} = ";
            }
            codeLine += $"{function.name}(";
            foreach (ShaderParameter inputParam in inputParams)
            {
                codeLine += inputParam.name;
            }
            codeLine += ");\n";
            codeList.Add(codeLine);
        }

    }

    public class ShaderParameter
    {
        public static readonly ShaderParameter Void = new ShaderParameter(string.Empty, InternalShaderType.Void);
        internal readonly bool isReadOnly;
        internal readonly string name;
        internal readonly InternalShaderType type;

        public ShaderParameter(string name, InternalShaderType type)
        {
            this.name = name;
            this.type = type;
            isReadOnly = false;
        }

        public static bool IsSameType(ShaderParameter param1, ShaderParameter param2)
        {
            if (param1.type != InternalShaderType.Struct)
            {
                return param1.type == param2.type;
            }
            else
            {
                throw new Exception("Struct is currently not supported.");
            }
        }

        public static InternalShaderType TypeMarshal<T>() where T : struct
        {
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Single:
                    return InternalShaderType.Float;
                case TypeCode.Double:
                    return InternalShaderType.Double;
                case TypeCode.Int32:
                    return InternalShaderType.Int;
                case TypeCode.UInt32:
                    return InternalShaderType.UInt;
                default:
                    break;
            }
            switch (typeof(T).Name)
            {
                case "vec4d":
                    return InternalShaderType.Vector4;
                case "Matrix2x2":
                    return InternalShaderType.Matrix2x2;
                case "Matrix2x3":
                    return InternalShaderType.Matrix2x3;
                case "Matrix2x4":
                    return InternalShaderType.Matrix2x4;
                case "Matrix3x2":
                    return InternalShaderType.Matrix3x2;
                case "Matrix3x3":
                    return InternalShaderType.Matrix3x3;
                case "Matrix3x4":
                    return InternalShaderType.Matrix3x4;
                case "Matrix4x2":
                    return InternalShaderType.Matrix4x2;
                case "Matrix4x3":
                    return InternalShaderType.Matrix4x3;
                case "Matrix4x4":
                    return InternalShaderType.Matrix4x4;
                default:
                    throw new ArgumentException("Not supported shader data type.");
            }
        }

    }

    public enum InternalShaderType
    {
#pragma warning disable CS1591
        Struct, Void,
        Int, UInt,
        Float, Double,
        Vector2, Vector3, Vector4,
        VectorD2, VectorD3, VectorD4,
        Matrix2x2, Matrix2x3, Matrix2x4,
        Matrix3x2, Matrix3x3, Matrix3x4,
        Matrix4x2, Matrix4x3, Matrix4x4,
#pragma warning restore CS1591
    }
}