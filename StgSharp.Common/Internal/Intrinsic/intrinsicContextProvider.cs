//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="intrinsicContextProvider.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the ¡°Software¡±), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED ¡°AS IS¡±, 
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
using StgSharp.Internal.Intrinsic;

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Internal
{
    internal static unsafe partial class InternalIO
    {

        private static readonly IntrinsicContext _intrinsicContext = new IntrinsicContext(
            );

        internal static IntrinsicContext Intrinsic
        {
            get => _intrinsicContext;
        }

        internal static void InitIntrinsicContext()
        {
            int statCode;

            fixed( IntrinsicContext* cptr = &_intrinsicContext ) {
                statCode = LoadIntrinsicFunction( cptr );
            }

            switch( statCode ) {
                case 0:

                    //Dialogue.PostError(new PlatformNotSupportedException("No SIMD instructions supported on current hardware platform"));
                    Environment.Exit( -1 );
                    break;
                default:
                    break;
            }
        }

        [LibraryImport( SSC_libName, EntryPoint = "load_intrinsic_function" )]
        [UnmanagedCallConv( CallConvs = [typeof( CallConvCdecl )] )]
        private static partial int LoadIntrinsicFunction(
                                           IntrinsicContext* context );

    }
}