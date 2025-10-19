//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="intrinsicContextProvider"
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
using StgSharp.Internal.Intrinsic;

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Internal
{
    internal static unsafe partial class InternalIO
    {

        private static readonly IntrinsicContext _intrinsicContext = new();

        public static IntrinsicLevel IntrinsicLevel { get; set; }

        internal static IntrinsicContext Intrinsic
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _intrinsicContext;
        }

        internal static void InitIntrinsicContext()
        {
            int statCode;

            fixed (IntrinsicContext* cPtr = &_intrinsicContext) {
                statCode = LoadIntrinsicFunction(cPtr);
            }

            switch (statCode)
            {
                case 0:

                    // Dialogue.PostError(new PlatformNotSupportedException("No SIMD instructions supported on current hardware platform"));
                    Environment.Exit(-1);
                    break;
                default:
                    IntrinsicLevel = (IntrinsicLevel)statCode;
                    break;
            }
        }

        [LibraryImport(NativeLibName, EntryPoint = "load_intrinsic_function")]
        [UnmanagedCallConv(CallConvs =[ typeof(CallConvCdecl) ])]
        private static partial int LoadIntrinsicFunction(IntrinsicContext* context);

    }

    internal enum IntrinsicLevel : sbyte
    {

        Non = 0,
        SSE = 1,
        AVX2 = 2,
        AVX512 = 3,
        NEON = -1

    }
}