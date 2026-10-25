// -----------------------------------------------------------------------------
// file="L4.CacheLine"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Nitload.Common.HighPerformance.Memory
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
        private struct CacheLineMetadata
        {

            internal const uint MaxBufferSize = 0xFFFFFFFF - 64U;

            [FieldOffset(31)] internal AllocationState State;

            public static uint SizeOf { get; } = (uint)Unsafe.SizeOf<CacheLineMetadata>();

            /// <summary>
            ///   only available in L4.Predict()
            /// </summary>
            public void CheckCount()
            {
                uint count = Volatile.Read(ref ConsumeCount);
                if (ushort.MaxValue < count)
                {
                    uint delta = unchecked(0u - count);
                    _ = Interlocked.Add(ref ConsumeCount, delta);
                    _ = Interlocked.Add(ref MapCount, delta);
                    _ = Interlocked.Add(ref PredictCount, delta);
                }
            }

            public void IncreaseConsumeCount()
            {
                unchecked {
                    _ = Interlocked.Increment(ref ConsumeCount);
                }
            }

            public void IncreaseMapCount()
            {
                unchecked {
                    _ = Interlocked.Increment(ref MapCount);
                }
            }

            public void IncreasePredictCount()
            {
                unchecked {
                    _ = Interlocked.Increment(ref PredictCount);
                }
            }

            #region tlsf linked list

            [FieldOffset(0)] public short NextLevel;
            [FieldOffset(2)] public short NextNear;
            [FieldOffset(4)] public short PreviousLevel;
            [FieldOffset(6)] public short PreviousNear;

            #endregion

            #region buffer

            // Payload offset from the L4 arena base.
            [FieldOffset(8)] public uint Position;
            [FieldOffset(12)] public uint SizeMask;

            #endregion

            #region count

            [FieldOffset(16)] public uint PredictCount;
            [FieldOffset(20)] public uint MapCount;
            [FieldOffset(24)] public uint ConsumeCount;

        #endregion
        }

#pragma warning disable IDE0032
        [StructLayout(LayoutKind.Explicit, Size = 16)]
        public unsafe struct Handle : IDisposable
        {

            [FieldOffset(0)] private nuint _handle;

            [FieldOffset(8)] private nuint _reserved;

            internal Handle(
                     nuint handle,
                     nuint reserved
            )
            {
                _handle = handle;
                _reserved = reserved;
            }

            [Obsolete("You should not create a CacheLineHandle instance outside a L4", true)]
            public Handle() { }

            public readonly nuint Address => _handle;

            public void Dispose()
            {
                if (_handle == 0) {
                    return;
                }
                _ = Interlocked.Increment(ref ((CacheLineMetadata*)_reserved)->ConsumeCount);
                _handle = 0;
                _reserved = 0;
            }

        }
#pragma warning restore IDE0032
    }
}
