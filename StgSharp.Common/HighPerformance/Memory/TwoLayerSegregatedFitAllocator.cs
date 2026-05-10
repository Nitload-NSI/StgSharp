//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="TwoLayerSegregatedFitAllocator"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
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
using StgSharp.HighPerformance.Memory;
using StgSharp.HighPerformance.ProcessorAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static StgSharp.HighPerformance.Memory.TwoLayerSegregatedFitAllocator;

namespace StgSharp.HighPerformance.Memory
{
    public unsafe class TwoLayerSegregatedFitAllocator : IDisposable
    {

        private const ulong Mask48Bits = 0x0000FFFFFFFFFFFF;

        private readonly Metadata*[] _bucket;
        private readonly Metadata* _spareMemory;
        private readonly Action<nuint> _freeHandle;
        private bool disposedValue;
        private readonly int _align;

        private int _allocEnd;
        private readonly Ptr64 _basePtr;
        private readonly ulong _maxAllocSize;
        private readonly ulong _totalSize;

        internal TwoLayerSegregatedFitAllocator(
                 nuint byteSize,
                 int align,
                 ulong maxBlockSize,
                 Func<nuint, int, nuint> allocator,
                 Action<nuint> freeHandle)
            : this(byteSize, align, maxBlockSize, (void*)allocator(byteSize, align), freeHandle) { }

        internal TwoLayerSegregatedFitAllocator(
                 nuint byteSize,
                 int align,
                 ulong maxBlockSize,
                 void* externalPointer,
                 Action<nuint> freeHandle)
        {
            _totalSize = byteSize;
            _basePtr = (Ptr64)(ulong)externalPointer;
            _align = align;
            _maxAllocSize = ulong.Max(64, (~64UL) & (ulong.Min(maxBlockSize + 63, Mask48Bits)));
            _bucket = new Metadata*[58 - BitOperations.LeadingZeroCount(_maxAllocSize)];
            _spareMemory = &((Metadata*)(ulong)(_basePtr + _totalSize))[-1];
            _freeHandle = freeHandle ?? (static_ => { });
            _allocEnd = 0;

            // init spare memory node

            _spareMemory->Position = 0UL;
            _spareMemory->Size = _totalSize - Metadata.SizeOf;
            _spareMemory->NextLevel = 0;
            _spareMemory->NextNear = 0;
            _spareMemory->PreviousLevel = 0;
            _spareMemory->PreviousNear = 0;
        }

        public TwoLayerSegregatedFitAllocator(nuint byteSize)
            : this(byteSize, 16, 32U * 1024U * 1024U, NativeMemory.AlignedAlloc(byteSize, 16),
                   ptr => NativeMemory.AlignedFree((void*)ptr)) { }

        public TwoLayerSegregatedFitAllocator(nuint byteSize, uint maxBlockSize)
            : this(byteSize, 16, maxBlockSize, NativeMemory.AlignedAlloc(byteSize, 16),
                   ptr => NativeMemory.AlignedFree((void*)ptr)) { }

        public TwoLayerSegregatedFitAllocator(nuint byteSize, int align)
            : this(byteSize, align, 32U * 1024U * 1024U, NativeMemory.AlignedAlloc(byteSize, 16),
                   ptr => NativeMemory.AlignedFree((void*)ptr)) { }

        public TwoLayerSegregatedFitAllocator(nuint byteSize, int align, nuint maxBlockSize)
            : this(byteSize, align, maxBlockSize, NativeMemory.AlignedAlloc(byteSize, 16),
                   ptr => NativeMemory.AlignedFree((void*)ptr)) { }

        public Handle Alloc(nuint size)
        {
            ulong allocSize = nuint.Max(64, (~64U) & (size + 63));
            if (allocSize > _maxAllocSize) {
                throw new OverflowException("Requested allocation size exceeds the maximum block size.");
            }
            int level = 57 - BitOperations.LeadingZeroCount(allocSize);
            Metadata* entry = null;
            for (int i = level; i < _bucket.Length; i++)
            {
                entry = _bucket[i];
                if (entry is not null)
                {
                    _bucket[i] = entry->NextLevel != 0 ? &_spareMemory[-entry->NextLevel] : null;
                    break;
                }
            }
            if (entry is null && !TryGetNewMetadata(allocSize, out entry)) {
                throw new OverflowException("No available ");
            }
            ulong remain = entry->Size - allocSize;

            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(disposing:true);
            GC.SuppressFinalize(this);
        }

        public void Free(Handle handle)
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing) {
                    _freeHandle((nuint)_basePtr);
                }

                disposedValue = true;
            }
        }

        private bool TryGetNewMetadata(ulong size, out Metadata* meta)
        {
            int idx = _spareMemory->NextLevel;
            if (idx != 0)
            {
                meta = &_spareMemory[-idx];
                _spareMemory->NextLevel = meta->NextLevel;
                return true;
            }
            ulong spareSize = _spareMemory->Size;
            if (spareSize < 32UL + size)
            {
                meta = null;
                return false;
            }
            _allocEnd++;
            meta = &_spareMemory[-_allocEnd];
            spareSize -= 32UL - size;
            _spareMemory->Size = spareSize;
            _spareMemory->Position += size;
            meta->Size = size;
            return true;
        }

        [StructLayout(LayoutKind.Sequential)]
        public readonly struct Handle
        {

            internal readonly Metadata* _meta;
            public readonly void* Pointer;
            public readonly Ptr64 Size;

            internal Handle(Metadata* meta, Ptr64 baseAddress)
            {
                _meta = meta;
                Pointer = (void*)(ulong)(meta->Position + baseAddress);
                Size = meta->Size;
            }

            [Obsolete("You should not create instance from default. Alloc from allocator please")]
            public Handle() { }

            public ref byte this[int offset] => ref ((byte*)Pointer)[offset];

        }

        [StructLayout(LayoutKind.Explicit, Size = 32)]
        internal struct Metadata
        {

            [FieldOffset(24)] private ulong PositionMask;
            [FieldOffset(16)] private ulong SizeMask;

            [FieldOffset(0)] public int NextLevel;
            [FieldOffset(4)] public int NextNear;
            [FieldOffset(8)] public int PreviousLevel;
            [FieldOffset(12)] public int PreviousNear;

            [FieldOffset(22)] public ushort Mask1;
            [FieldOffset(30)] public ushort Mask2;

            public Ptr64 Position
            {
                readonly get => (Ptr64)(PositionMask & Mask48Bits);
                set => PositionMask = (PositionMask & ~Mask48Bits) | ((ulong)value & Mask48Bits);
            }

            public Ptr64 Size
            {
                readonly get => (Ptr64)(SizeMask & Mask48Bits);
                set => SizeMask = (SizeMask & ~Mask48Bits) | ((ulong)value & Mask48Bits);
            }

            public Ptr64 SizeForSpare
            {
                get => SizeMask;
                set => SizeMask = value;
            }

            public static uint SizeOf { get; } = (uint)Unsafe.SizeOf<Metadata>();

        }

    }
}
