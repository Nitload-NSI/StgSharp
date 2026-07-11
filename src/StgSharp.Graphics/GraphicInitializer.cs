// -----------------------------------------------------------------------------
// file="GraphicInitializer"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace StgSharp.Graphics
{
    public class GraphicModuel : IStaticModule
    {

        public string ModuleName => "Graphics";

        public void InitializeModule(
                    IModuleInitializeProfile profile
        )
        {
            GraphicFramework.LoadGlfw();
            if (GraphicFramework.glfwInit() == 0) {
                throw new Exception("Failed to init system graphic environment.");
            }
        }

        public void UninitializeModule()
        {
            GraphicFramework.glfwTerminate();
        }

    }
}
