//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallel.TaskWrapFactory.cs"
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
using StgSharp.HighPerformance.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics
{
    internal static unsafe class MatrixParallelFactory
    {

        private static readonly SlabAllocator<ScalarPacket> _scalarAllocator = SlabAllocator<ScalarPacket>.Create(64, SlabBufferLayout.Chunked, true);

        private static readonly SlabAllocator<MatrixParallelTaskPackage> _wrapAllocator = SlabAllocator<MatrixParallelTaskPackage>.Create(64, SlabBufferLayout.Chunked, true);

        public static ScalarPacket* CreateScalarPacket()
        {
            ScalarPacket* p = (ScalarPacket*)_scalarAllocator.Allocate();
            return p;
        }

        public static MatrixParallelTaskPackage* FromTaskWrap(MatrixParallelTaskPackage* source, int offset)
        {
            MatrixParallelTaskPackage* p = (MatrixParallelTaskPackage*)_wrapAllocator.Allocate();
            *p = *source;
            p->left += offset * p->leftStride;
            p->right += offset * p->rightStride;
            p->result += offset * p->resultStride;
            return p;
        }

        public static void Release(ScalarPacket* source)
        {
            _scalarAllocator.Free((nuint)source);
        }

        public static void Release(MatrixParallelTaskPackage* source)
        {
            if (source->scalerPacket != null) {
                _scalarAllocator.Free((nuint)source->scalerPacket);
            }
            _wrapAllocator.Free((nuint)source);
        }

        public static void SliceTask(MatrixParallelTaskPackage* source, Span<IntPtr> group)
        {
            int count = group.Length;
            group[0] = (nint)source;
            int l = source->leftStride, r = source->rightStride, res = source->resultStride, c = source->count / count;
            source->leftStride *= count;
            source->rightStride *= count;
            source->resultStride *= count;
            int remain = source->count - (c * count);
            for (int i = 1; i < count; i++)
            {
                nint handle = (nint)_wrapAllocator.Allocate();
                MatrixParallelTaskPackage* p = (MatrixParallelTaskPackage*)handle;
                *p = *source;
                p->left += l;
                p->right += r;
                p->result += res;
                group[i] = handle;
            }
            for (int i = 0; i < remain; i++) {
                ((MatrixParallelTaskPackage*)group[^i])->count++;
            }
        }

    }
}