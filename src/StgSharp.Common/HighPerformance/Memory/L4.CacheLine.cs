// -----------------------------------------------------------------------------
// file="L4.CacheLine"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.HighPerformance.ProcessorAbstraction;
using System.Runtime.InteropServices;

namespace StgSharp.HighPerformance.Memory
{
    public partial class L4
    {

        internal enum AllocationState : byte
        {

            SpareOrCollect = 0,
            Free = 1,
            Alloc = 2,

        }

        [StructLayout(LayoutKind.Explicit, Size = 32, Pack = 32)]
        private unsafe struct CacheLineMetadata
        {

            internal const uint MaxBufferSize = 0xFFFFFFFF - 64U;

            [FieldOffset(0)] private readonly M256 FillingMask;

            [FieldOffset(24)] internal nuint Origin;

            public static uint SizeOf { get; } = (uint)Unsafe.SizeOf<CacheLineMetadata>();

            #region tlsf linked list

            [FieldOffset(0)] public short NextLevel;
            [FieldOffset(2)] public short NextNear;
            [FieldOffset(4)] public short PreviousLevel;
            [FieldOffset(6)] public short PreviousNear;

            #endregion

            #region buffer

            [FieldOffset(8)] public uint PositionMask;
            [FieldOffset(12)] public uint SizeMask;

            #endregion

            #region mask

            [FieldOffset(16)] internal AllocationState State;
            [FieldOffset(17)] internal byte ReversedProfile;
            [FieldOffset(18)] internal ushort CustomizeProfile;
            [FieldOffset(20)] internal int RefCount;

        #endregion
        }

#pragma warning disable IDE0032
        [StructLayout(LayoutKind.Explicit)]
        public unsafe struct Handle : IDisposable
        {

            [FieldOffset(8)] private readonly CacheLineMetadata* _head;
            [FieldOffset(0)] private nuint _handle;

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
#pragma warning restore IDE0032
    }
}
