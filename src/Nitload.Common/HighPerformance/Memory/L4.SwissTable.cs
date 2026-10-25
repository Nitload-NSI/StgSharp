// -----------------------------------------------------------------------------
// file="L4.SwissTable"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Buffers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Nitload.Common.HighPerformance.Memory
{
    public sealed unsafe partial class L4
    {

        private const int BucketCapacity = 16;
        private const int GroupCount = 1 << GroupCountLog;
        private const int GroupCountLog = 10;
        private const int IdealCount = GroupCount * BucketCapacity;//16384
        private const int OffsetOfBucket = IdealCount;

        private static readonly int TableBufferSize =
            IdealCount + (GroupCount * Unsafe.SizeOf<SwissBucket>());
        internal static readonly int BufferSize = TableBufferSize * 2;

        private enum SwissControl : byte
        {

            Empty = 0b10000000,
            Deleted = 0b10000001

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct AddressSource : IEquatable<AddressSource>
        {

            public uint Description;
            public uint Size;
            public ulong Address;

            public readonly bool Equals(
                                 AddressSource other
            )
            {
                return this == other;
            }

            public override readonly bool Equals(
                                          object? obj
            )
            {
                return obj is AddressSource source && Equals(source);
            }

            public override readonly int GetHashCode()
            {
                return HashCode.Combine(Address, Description);
            }

            public static bool operator !=(
                                        AddressSource left,
                                        AddressSource right
            )
            {
                return !(left == right);
            }

            public static bool operator ==(
                                        AddressSource left,
                                        AddressSource right
            )
            {
                return left.Address == right.Address && left.Description == right.Description;
            }

        }

        [InlineArray(BucketCapacity)]
        internal struct KeyArray
        {

            private AddressSource _element0;

        }

        internal struct SwissBucket
        {

            public KeyArray Keys;
            public fixed ulong Values[BucketCapacity];

        }

        internal unsafe struct SwissEnumerator(
                               byte* buffer,
                               int begins_at = 0
        )
        {

            private readonly SwissBucket* _buckets = (SwissBucket*)(buffer + IdealCount);
            private readonly Vector128<byte>* _control_bytes = (Vector128<byte>*)buffer;
            private bool _group_loaded = false;
            private int _current_group = (begins_at / BucketCapacity) & (GroupCount - 1);
            private int _current_mask = 0;
            private int _current_offset = 0;
            private int _visited_group = 0;

            public (AddressSource key, ulong value) Current
            {
                get
                {
                    SwissBucket* bucket = &_buckets[_current_group];
                    AddressSource key = bucket->Keys[_current_offset];
                    ulong value = bucket->Values[_current_offset];
                    return (key, value);
                }
            }

            public readonly int CurrentOffset => (_current_group * BucketCapacity) + _current_offset;

            public bool MoveNext()
            {
                while (_visited_group < GroupCount)
                {
                    if (!_group_loaded)
                    {
                        if (Sse42.IsSupported)
                        {
                            _current_mask = SwissReaderX86.CompareOccupiedMask(*(_control_bytes + _current_group));
                        } else
                        {
                            throw new PlatformNotSupportedException();
                        }
                        _group_loaded = true;
                    }

                    if (_current_mask != 0)
                    {
                        _current_offset = BitOperations.TrailingZeroCount(_current_mask);
                        _current_mask &= _current_mask - 1;
                        return true;
                    }

                    _current_group = (_current_group + 1) & (GroupCount - 1);
                    _visited_group++;
                    _group_loaded = false;
                }
                return false;
            }

        }

        internal unsafe struct SwissTable : IDisposable
        {

            private byte* _buffer_write;
            private int _delete_count;
            private ulong _published_buffer;

            private SwissTable(
                    byte* buffers
            )
            {
                _published_buffer = (ulong)buffers;
                _buffer_write = buffers + TableBufferSize;
                InitializeBuffer(buffers);
                InitializeBuffer(_buffer_write);
            }

            internal byte* ReadBufferForLockFreeBenchmark => (byte*)Volatile.Read(ref _published_buffer);

            internal readonly byte* WriteBufferForLockFreeBenchmark => _buffer_write;

            public void Dispose()
            {
                _ = Interlocked.Exchange(ref _published_buffer, 0);
                _buffer_write = null;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void RemoveAtOffset(
                        int offset
            )
            {
                _buffer_write[offset] = (byte)SwissControl.Deleted;
                _delete_count++;
            }

            /// <summary>
            ///   Publishes the write buffer. The caller must provide the phase barrier around this
            ///   operation.
            /// </summary>
            public void SwapBuffers()
            {
                ulong previousBuffer = Interlocked.Exchange(ref _published_buffer, (ulong)_buffer_write);
                _buffer_write = (byte*)previousBuffer;
            }

            /// <summary>
            ///   Writes to the inactive buffer. The caller must serialize all writes.
            /// </summary>
            public bool TryAddOrSet(
                        AddressSource key,
                        ulong value
            )
            {
                if (Sse42.IsSupported) {
                    return SwissWriterX86.TryAddOrSet(_buffer_write, key, value, ref _delete_count);
                }

                throw new NotImplementedException("The Swiss backend is not implemented for this ISA.");
            }

            /// <summary>
            ///   Reads the published buffer. The caller must prevent swaps until all reads finish.
            /// </summary>
            public bool TryGet(
                        AddressSource key,
                        out ulong value
            )
            {
                byte* publishedBuffer = (byte*)Volatile.Read(ref _published_buffer);
                if (Sse42.IsSupported) {
                    return SwissReaderX86.TryGet(publishedBuffer, key, out value);
                }

                throw new NotImplementedException("The Swiss backend is not implemented for this ISA.");
            }

            /// <summary>
            ///   Reads the inactive write buffer. The caller must serialize this query with all
            ///   writes.
            /// </summary>
            public readonly bool TryGetWrite(
                                 AddressSource key,
                                 out ulong value
            )
            {
                if (Sse42.IsSupported) {
                    return SwissReaderX86.TryGet(_buffer_write, key, out value);
                }

                throw new NotImplementedException("The Swiss backend is not implemented for this ISA.");
            }

            public void TryRehash()
            {
                if (_delete_count < 2048)
                {
                    NativeMemory.Copy((byte*)_published_buffer, _buffer_write, (nuint)TableBufferSize);
                    return;
                }
                try
                {
                    SwissEnumerator e = new((byte*)_published_buffer);
                    InitializeBuffer(_buffer_write);
                    if (Sse42.IsSupported)
                    {
                        while (e.MoveNext())
                        {
                            (AddressSource key, ulong value) = e.Current;
                            if (!SwissWriterX86.TryAddOrSet(_buffer_write, key, value,
                                                            ref _delete_count)) {
                                throw new InvalidOperationException("The Swiss table could not fit all active entries during rehash.");
                            }
                        }
                        _delete_count = 0;
                    } else
                    {
                        throw new PlatformNotSupportedException();
                    }
                }
                catch (Exception)
                {
                    NativeMemory.Copy(_buffer_write, (byte*)_published_buffer, (nuint)TableBufferSize);
                    throw;
                }
            }

            // Removes from the inactive buffer. The caller must serialize all writes.
            public bool TryRemove(
                        AddressSource key,
                        out ulong value
            )
            {
                if (Sse42.IsSupported)
                {
                    return SwissWriterX86.TryRemove(_buffer_write, key, out value,
                                                    ref _delete_count);
                } else
                {
                    throw new NotImplementedException("The Swiss backend is not implemented for this ISA.");
                }
            }

            internal static SwissTable Create(
                                       byte* buffers
            )
            {
                ArgumentNullException.ThrowIfNull(buffers);
                return new SwissTable(buffers);
            }

            // Enumerates the write buffer; the caller must serialize writers and prevent swaps.
            internal readonly SwissEnumerator GetWriteBufferEnumerator(
                                              int begins_at = 0
            )
            {
                return new SwissEnumerator(_buffer_write, begins_at);
            }

            private static void InitializeBuffer(
                                byte* buffer
            )
            {
                Unsafe.InitBlock(buffer, (byte)SwissControl.Empty, IdealCount);
            }

        }

    }
}