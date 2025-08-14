//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="HLSFAllocator.Level.cs"
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class HybridLayerSegregatedFitAllocator
    {

        private const int MaxLevel = 9;

        private static readonly int[] levelSize = [
            64, 256, 1024, 4096, 16384, 65536, 262144, 1048576, 4194304, 16777216
            ];

        public int AlignOfLevel(int bucketLevel)
        {
            int blockSize = levelSize[bucketLevel];
            return int.Min(blockSize, _align);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetLevelFromSize(uint size)
        {
            int level = -1;
            uint temp = size / 8;
            while (temp > 3)
            {
                level++;
                temp >>= 2;
            }
            return level;
        }

    }

    public enum CommonMemoryAlign : int
    {

        XMMLevel = 16,
        YMMLevel = 32,
        ZMMLevel = 64,

        ArmNEONLevel = 16,

    }
}
