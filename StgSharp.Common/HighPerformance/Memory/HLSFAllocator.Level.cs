//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="HLSFAllocator.Level"
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class HybridLayerSegregatedFitAllocator
    {

        private const int MaxLevel = 9;

        private int[] _levelSizeArray = [
            64,
            256,
            1024,
            4096,
            16384,
            65536,
            262144,
            1048576,
            4194304,
            16777216
        ];

        public int AlignOfLevel(
                   int bucketLevel
        )
        {
            int blockSize = _levelSizeArray[bucketLevel];
            return int.Min(blockSize, _align);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetLevelFromSize(
                           long size
        )
        {
            return GetLevelFromSize((uint)size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetLevelFromSize(
                           uint size
        )
        {
            /*
                * Level sizes: [64, 256, 1024, 4096, 16384, 65536, 262144, 1048576, 4194304, 16777216]
                * Pattern: 64 * 4^level = 2^6 * 2^(2*level) = 2^(6+2*level)
                * 
                * For size S, we need: 2^(6+2*level) >= S
                * So: 6+2*level >= log2(S)
                * Therefore: level >= (log2(S) - 6) / 2
                * 
                * We use hardware LZCNT instruction via BitOperations.LeadingZeroCount
                * log2(S) = 31 - LeadingZeroCount(S) for 32-bit values
                */
            int log2Size = 31 - BitOperations.LeadingZeroCount(size);

            // Calculate level: level = max(0, (log2Size - 6) / 2)
            int level = Math.Max(0, (log2Size - 6) / 2);

            // caller is responsible for ensuring level is within bounds
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
