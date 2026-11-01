// -----------------------------------------------------------------------------
// file="NumericalModule"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace Nitload.Mathematics.Numeric
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
        public void InitializeModule(IModuleInitializeConfiguration profile)
        {
            try
            {
                NumericModuleInitProfile? p = profile as NumericModuleInitProfile;
                p ??= NumericModuleInitProfile.Default;
                MatrixParallel.Init(p);
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
