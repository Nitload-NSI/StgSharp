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
using System.Net.Security;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class HybridLayerSegregatedFitAllocator
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private nuint GetAllocSize(
                      nuint size
        )

        {
            return (size + 63u) & (~63u);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetBucketIndexOnce(
                    nuint size
        )
        {
            int log2Size = 63 - BitOperations.LeadingZeroCount((ulong)size);

            // Calculate level: level = max(0, (log2Size - 6) / 2)
            int level = Math.Max(0, (log2Size - 6) / 2);

            // caller is responsible for ensuring level is within bounds
            level = Math.Min(level, _maxLevel);
            uint levelSize = 1u << (log2Size & (~1));
            int segment = ((int)(size / levelSize)) - 1;
            return (level * 3) + segment;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private (int level, int segment) GetIndexPairOnce(
                                         nuint size
        )
        {
            int log2Size = 63 - BitOperations.LeadingZeroCount((ulong)size);

            // Calculate level: level = max(0, (log2Size - 6) / 2)
            int level = Math.Max(0, (log2Size - 6) / 2);

            // caller is responsible for ensuring level is within bounds
            level = Math.Min(level, _maxLevel);
            uint levelSize = 1u << (log2Size & (~1));
            int segment = ((int)(size / levelSize)) - 1;
            return (level, segment);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetLevelFromSize(
                    nuint size
        )
        {
            /*
            * Level sizes: [64, 256, 1024, 4096, 16384, 65536, 262144, 1048576, 4194304, 16777216, ......]
            * Pattern: 64 * 4^level = 2^6 * 2^(2*level) = 2^(6+2*level)
            * 
            * For size S, we need: 2^(6+2*level) >= S
            * So: 6+2*level >= log2(S)
            * Therefore: level >= (log2(S) - 6) / 2
            * 
            * We use hardware LZCNT instruction via BitOperations.LeadingZeroCount
            * log2(S) = 63 - LeadingZeroCount(S) for 64-bit values
            */
            int log2Size = 63 - BitOperations.LeadingZeroCount((ulong)size);

            // Calculate level: level = max(0, (log2Size - 6) / 2)
            int level = Math.Max(0, (log2Size - 6) / 2);

            // caller is responsible for ensuring level is within bounds
            return Math.Min(level, _maxLevel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint GetLevelSizeFromLevel(
                            int level
        )
        {
            return 1u << (6 + (2 * level));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint GetLevelSizeFromSize(
                            nuint size
        )
        {
            int log2Size = 63 - BitOperations.LeadingZeroCount((ulong)size);
            return 1u << (log2Size & (~1));
        }

        /// <summary>
        ///   Determine segment index based on size and originLevel. Input level value is regarded
        ///   safe.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetSegmentFromLevel(
                    ulong size,
                    int level
        )
        {
            return ((int)(size / GetLevelSizeFromLevel(level))) - 1;
        }

    }
}
