//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallel.TaskFactory.cs"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the “Software”), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//     
// The above copyright notice and 
// this permission notice shall be included in all copies 
// or substantial portions of the Software.
//     
// THE SOFTWARE IS PROVIDED “AS IS”, 
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
// ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.HighPerformance;
using StgSharp.HighPerformance.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using hlsfAllocator = StgSharp.HighPerformance.Memory.HybridLayerSegregatedFitAllocator;

namespace StgSharp.Mathematics
{
    internal static unsafe class MatrixParallelFactory
    {

        private static hlsfAllocator _intermediateResult;

        private static
#if NET9_0_OR_GREATER
            Lock
#else
            object
#endif
            _hlsfLock = new();

        private static SlabAllocator<ScalarPacket> _scalarAllocator ;

        private static SlabAllocator<MatrixParallelTaskPackageNonGeneric> _wrapAllocator ;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixParallelTaskPackage<T>* CreateBaseTask<T>() where T: unmanaged, INumber<T>
        {
            MatrixParallelTaskPackage<T>* p = (MatrixParallelTaskPackage<T>*)_wrapAllocator.Allocate();
            Span<byte> s = new Span<byte>((byte*)p, sizeof(MatrixParallelTaskPackage<T>));
            s.Fill(0);
            return p;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ScalarPacket* CreateScalarPacket()
        {
            ScalarPacket* p = (ScalarPacket*)_scalarAllocator.Allocate();
            return p;
        }

        public static MatrixParallelTaskPackage<T>* FromTaskWrap<T>(MatrixParallelTaskPackage<T>* source, int offset)
            where T: unmanaged, INumber<T>
        {
            MatrixParallelTaskPackage<T>* p = (MatrixParallelTaskPackage<T>*)_wrapAllocator.Allocate();
            /*
            *p = *source;
            p->Left += offset * p->LeftStride;
            p->Right += offset * p->RightStride;
            p->Result += offset * p->ResultStride;
            /**/
            return p;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static void Release(ScalarPacket* source)
        {
            _scalarAllocator.Free((nuint)source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Release(MatrixParallelTaskPackageNonGeneric* source)
        {
            if (source->Scalar != null) {
                _scalarAllocator.Free((nuint)source->Scalar);
            }
            _wrapAllocator.Free((nuint)source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Release<T>(MatrixParallelTaskPackage<T>* source) where T: unmanaged, INumber<T>
        {
            if (source->Scalar != null) {
                _scalarAllocator.Free((nuint)source->Scalar);
            }
            _wrapAllocator.Free((nuint)source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Init()
        {
            if (_intermediateResult is null)
            {
                _intermediateResult = new hlsfAllocator(64 * 16 * 16);
                _scalarAllocator = SlabAllocator<ScalarPacket>.Create(64 * 1024, SlabBufferLayout.Chunked);
                _wrapAllocator = SlabAllocator<MatrixParallelTaskPackageNonGeneric>.
                    Create(128 * 1024, SlabBufferLayout.Chunked);
            }
        }

    }
}