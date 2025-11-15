//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixParallel.Publish"
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
using StgSharp.Collections;
using StgSharp.HighPerformance;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public static partial class MatrixParallel
    {

        private static unsafe MatrixParallelHandle PublicTaskPrivate<T>(MatrixParallelTaskPackage<T>* package)
            where T : unmanaged, INumber<T>
        {
            int primCount = package->PrimCount;
            int secCount = package->SecCount;
            int tileWidth = (1024 / secCount) + 1;
            int tileCount = (primCount + tileWidth - 1) / tileWidth;

            MatrixParallelThread[] pool = LeadParallel(tileCount);

            if (pool.Length == 0)
            {
                /*
                 * wait until has enough threads
                 */
            }
            MatrixParallelHandle handle = new MatrixParallelHandle(pool);
            if (handle.Wraps.Count() == 1)
            {
                handle.Wraps.First().WrapTask = (MatrixParallelTaskPackageNonGeneric*)package;
                return handle;
            }
            int threadCount = pool.Length;
            int tilePerThreadLess = tileCount / threadCount;                           // tiles per thread without extra slice
            int columnAfterBaseTile = tileCount % threadCount * tileWidth;             // tile sliced into columns
            int extraColumnPerThread = columnAfterBaseTile / threadCount;
            int extraColumnCount = columnAfterBaseTile % threadCount;                  // columns to be sliced

            int total = primCount;
            Stack<MatrixParallelWrap> taskBunch = new Stack<MatrixParallelWrap>(threadCount);
            MatrixParallelTaskPackage<T>* current = package;
            foreach (MatrixParallelWrap wrap in handle.Wraps)
            {
                bool hasSlice = columnAfterBaseTile > 0;
                bool isLargeSlice = extraColumnCount > 0;

                // _count tiles for wrap
                int wrapWidth = wrap!.WrapTaskCapacity/**/;
                int columnFromSlicingCount = (hasSlice ? extraColumnPerThread : 0) * wrapWidth;
                int baseColumn = wrapWidth * tilePerThreadLess * tileWidth;
                int columnInWrap = baseColumn + columnFromSlicingCount + (isLargeSlice ? 1 : 0);

                // set tasks
                if (columnInWrap == 0)
                {
#if DEBUG
                    throw new InvalidOperationException("Zero tasks published.");
#else
                    break;
#endif
                }

                // update current task
                current->PrimColumnCountInTile = columnInWrap;
                current->SecColumnCountInTile = current->SecCount;
                current->SecTileOffset = 0;

                // remove tiles _count
                extraColumnCount--;
                columnAfterBaseTile -= columnFromSlicingCount;
                total -= columnInWrap;

                // set to wrap and push
                wrap.WrapTask = (MatrixParallelTaskPackageNonGeneric*)current;

                if (total <= 0)
                {
                    break;
                }
                current = MatrixParallelFactory.FromExistPackage(current);
                current->PrimTileOffset += columnInWrap;
            }

            return handle;
        }

    }
}
