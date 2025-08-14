//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="HLSFAllocator.SegmentEntry.cs"
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe partial class HybridLayerSegregatedFitAllocator
    {

        private void EnqueueLevel(int level, int size, Entry* handle)
        {
            if (handle is null) {
                return;
            }
            int entryIndex = (level * 3) + size;
            if (Interlocked.CompareExchange(
                ref _bucketEntry[entryIndex], (ulong)handle, EmptyHandle) ==
                EmptyHandle) {
                return;
            }
            while (true)
            {
                ulong o = Volatile.Read(ref _bucketEntry[entryIndex]);
                handle->NextLevel = o;
                if (Interlocked.CompareExchange(ref _bucketEntry[entryIndex], (ulong)handle,
                                                o) == o)
                {
                    _ = Interlocked.Exchange(ref handle->State, EntryState.Empty);
                    return;
                }
            }
        }

        private bool TryDequeueLevel(int level, int size, out Entry* handle)
        {
            int entryIndex = (level * 3) + size;
            while (true)
            {
                Entry* e = (Entry*)Volatile.Read(ref _bucketEntry[entryIndex]);
                if (Entry.IsNullOrEmptyIndex(e))
                {
                    handle = null;
                    return false;
                }

                ulong n = Volatile.Read(ref e->NextLevel);

                if (Interlocked.CompareExchange(ref _bucketEntry[entryIndex], n,
                                                (ulong)e) ==
                    (ulong)e)
                {
                    handle = e;
                    handle->State = EntryState.ThreadOccupied;
                    handle->NextLevel = EmptyHandle;
                    return true;
                }
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct Entry
        {

            [FieldOffset(32)] internal int State;
            [FieldOffset(8)]  internal ulong NextLevel;
            [FieldOffset(16)] internal ulong NextNear;
            [FieldOffset(24)] internal ulong PreviousNear;

            [FieldOffset(0)] public byte* Position;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsNullOrEmptyIndex(ulong index)
            {
                return index == ulong.MaxValue;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsNullOrEmptyIndex(Entry* entry)
            {
                return entry == null;
            }

        }

        private static class EntryState
        {

            public const int
                Empty = 0,
                Allocated = 1,
                ThreadOccupied = 2;

        }

    }
}
