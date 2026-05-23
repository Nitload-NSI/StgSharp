//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="L4.CacheLine"
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
using StgSharp.HighPerformance.ProcessorAbstraction;
using System.Runtime.InteropServices;

using Tlsf = StgSharp.HighPerformance.Memory.TwoLayerSegregatedFitAllocator;

namespace StgSharp.HighPerformance.Memory
{
    public partial class L4
    {

        [InlineArray(CacheLineCount)]
        private struct PredictionArray
        {

            private int PredictionCount;

        }

        [InlineArray(CacheLineCount)]
        private struct MapArray
        {

            private int MapCount;

        }

        internal enum AllocationState : byte
        {

            SpareOrCollect = 0,
            Free = 1,
            Alloc = 2,

        }

        [StructLayout(LayoutKind.Explicit, Size = 64, Pack = 64)]
        private unsafe struct CacheLineMetadata
        {

            #region tlsf area

            internal const ulong Mask48Bits = 0x0000FFFFFFFFFFFF;

            [FieldOffset(0)] private readonly M512 FillingMask;

            [FieldOffset(16)] private ulong PositionMask;
            [FieldOffset(24)] private ulong SizeMask;

            [FieldOffset(30)] internal AllocationState State;
            [FieldOffset(31)] internal sbyte Level;

            [FieldOffset(0)] public int NextLevel;
            [FieldOffset(4)] public int NextNear;
            [FieldOffset(8)] public int PreviousLevel;
            [FieldOffset(12)] public int PreviousNear;

            public Ptr64 Position
            {
                readonly get => PositionMask;
                set => PositionMask = value;
            }

            public Ptr64 SizeForAllocator
            {
                readonly get => (Ptr64)(SizeMask & Mask48Bits);
                set => SizeMask = (SizeMask & ~Mask48Bits) | ((ulong)value & Mask48Bits);
            }

            public Ptr64 SizeForSpare
            {
                readonly get => SizeMask;
                set => SizeMask = value;
            }

            public static uint SizeOf { get; } = (uint)Unsafe.SizeOf<CacheLineMetadata>();

            #endregion

            #region reference counting and state information

            [FieldOffset(24)] internal int RefCount;
            [FieldOffset(28)] internal readonly int Id;

            #endregion

            #region cache line information

            [FieldOffset(32)] internal short Predictor;
            [FieldOffset(34)] internal ushort Profile;
            [FieldOffset(36)] internal nuint Origin;

            #endregion
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe ref struct Handle : IDisposable
        {

            private CacheLineMetadata* _head;
            private nuint _handle;

            private Handle(
                    nuint handle,
                    CacheLineMetadata* cacheRefCount
            )
            {
                _handle = handle;
                _head = cacheRefCount;
            }

            [Obsolete("You should not create a CacheLineHandle instance outside a L4", true)]
            public Handle() { }

            public readonly nuint Address => _handle;

            public void Dispose()
            {
                if (_handle == 0) {
                    return;
                }
                _handle = 0;
                _ = Interlocked.Decrement(ref _head->RefCount);
            }

        }

    }
}
