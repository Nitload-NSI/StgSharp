using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics.ShaderEdit
{
    public interface IglConvertable
    {
        /// <summary>
        /// 
        /// </summary>
        public void SetAllUniforms();

        /// <summary>
        /// 
        /// </summary>
        public void GainAllUniforms(ShaderProgram source, params string[] uniformName);

        /// <summary>
        /// 
        /// </summary>
        public void DisplayGLtypeDefinition();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ShaderStruct GetConvertedGLtype();

    }
}
