//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="L4.Evict"
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
namespace StgSharp.HighPerformance.Memory
{
    public partial class L4
    {

        /// <summary>
        ///
        /// </summary>
        /// <param name="count">
        ///
        /// </param>
        /// <returns>
        ///   Count of lines not evicted
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe int Evict(
                           int count = 1
        )
        {
            count = (count < 0) ? 0 : count;
            int* pPtr = _predictCount, mPtr = _mapCount;


            int remain = count;
            for (int i = 0; i < CacheLineCount; i++)
            {
                if (pPtr[i] > mPtr[i])
                {
                    continue;
                }
                remain -= TryEvictAt(i) ? 1 : 0;
            }
            return remain;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe bool TryEvictAt(
                            int index
        )
        {
            CacheLineMetadata* head = &_head[index];
            if (Interlocked.CompareExchange(ref head->RefCount, int.MinValue, 0) == 0)
            {
                // EvictLine(i + index);
                _ = _map.TryRemove(head->Origin, out _);
                IL4Predict predict = CurrentPredict;
                predict.WriteBack(head->Origin, head->CustomizeProfile, new ReadOnlySpan<byte>((byte*)(ulong)(head->PositionMask + _baseAddress), (int)(ulong)head->SizeMask));
                _bufferAllocator.Free(index);
                _mapCount[index] = 0;
                _predictCount[index] = int.MaxValue;
                return true;
            }
            return false;
        }

    }
}
