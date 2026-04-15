//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="NumericalModule"
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
