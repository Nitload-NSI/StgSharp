// -----------------------------------------------------------------------------
// file="IglConvertable"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Graphics.OpenGL;

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
        public void DisplayGLtypeDefinition();

        /// <summary>
        ///
        /// </summary>
        public void GainAllUniforms(
                    ShaderProgram source,
                    params string[] uniformName
        );

        /// <summary>
        ///
        /// </summary>
        /// <returns>
        ///
        /// </returns>
        public ShaderStruct GetConvertedGLtype();

        /// <summary>
        ///
        /// </summary>
        public void SetAllUniforms();

    }
}
