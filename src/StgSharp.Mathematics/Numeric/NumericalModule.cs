// -----------------------------------------------------------------------------
// file="NumericalModule"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public class NumericalModule : IStaticModule
    {

        public static unsafe int DefaultSIMDAlignment
        {
            get
            {
                SIMDID id = GlobalIntrinsicMask;
                return (id.MaskByte[0] & 0b_00001111) switch
                {
                    1 => id.MaskByte[1] switch
                    {
                        1 => 16,
                        2 => 32,
                        3 => 64,
                        4 => 64,
                        5 => 64,
                        _ => 8,
                    },
                    _ => 8,
                };
            }
        }

        public string ModuleName => "Math.Numerical";

        internal static IntrinsicContext GlobalContext { get; set; } = new();

        internal static SIMDID GlobalIntrinsicMask { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InitializeModule()
        {
            MatrixParallel.Init();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InitializeModule(
                    IModuleInitializeProfile profile
        )
        {
            try
            {
                MatrixParallel.Init();
            }
            finally
            {
                GC.Collect();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UninitializeModule()
        {
            MatrixParallel.Deinit();
        }

    }
}
