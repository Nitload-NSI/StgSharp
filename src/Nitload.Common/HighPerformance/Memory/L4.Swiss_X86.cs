// -----------------------------------------------------------------------------
// file="L4.Swiss_X86"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Nitload.Common.HighPerformance.Memory
{
    public partial class L4
    {

        private const uint CrcMagic = 0x7f1c9961;

        internal static class SwissHashX64
        {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static uint Compute(
                               AddressSource key
            )
            {
                ulong hash = CrcMagic;
                hash = Sse42.X64.Crc32(hash, key.Address);
                hash = Sse42.X64.Crc32(hash, key.Description);
                return (uint)hash;
            }

        }

        internal readonly unsafe struct SwissWriterX86
        {

            public static bool TryAddOrSet(
                               byte* buffer,
                               AddressSource key,
                               ulong value,
                               ref int delete_count
            )
            {
                Vector128<byte>* control_bytes = (Vector128<byte>*)buffer;
                SwissBucket* buckets = (SwissBucket*)(buffer + IdealCount);

                uint hash = SwissHashX64.Compute(key);
                uint begin = hash % GroupCount;

                uint first_tomb_group = 0;
                int first_tomb_idx = -1;
                byte target_hash = (byte)((hash >> GroupCountLog) & ~(1 << 7));

                for (int i = 0; i < GroupCount; i++)
                {
                    Vector128<byte>* control_bytes_ptr = control_bytes + begin;
                    Vector128<byte> control_bytes_v128 = Sse2.LoadAlignedVector128((byte*)control_bytes_ptr);

                    Vector128<byte> target_hash_v128 = Vector128.Create(target_hash);
                    Vector128<byte> target_empty_v128 = Vector128.Create((byte)SwissControl.Empty);
                    Vector128<byte> target_tomb_v128 = Vector128.Create((byte)SwissControl.Deleted);

                    Vector128<byte> cmp_hash = Sse2.CompareEqual(control_bytes_v128, target_hash_v128);
                    Vector128<byte> cmp_empty = Sse2.CompareEqual(control_bytes_v128, target_empty_v128);
                    Vector128<byte> cmp_tomb = Sse2.CompareEqual(control_bytes_v128, target_tomb_v128);

                    int hash_mask = Sse2.MoveMask(cmp_hash);
                    int empty_mask = Sse2.MoveMask(cmp_empty);
                    int tomb_mask = Sse2.MoveMask(cmp_tomb);

                    if (tomb_mask != 0 && first_tomb_idx < 0)
                    {
                        int index = BitOperations.TrailingZeroCount(tomb_mask);
                        first_tomb_group = begin;
                        first_tomb_idx = index;
                    }
                    while (hash_mask != 0)
                    {
                        int index = BitOperations.TrailingZeroCount(hash_mask);
                        SwissBucket* bucket = &buckets[begin];
                        if (bucket->Keys[index] == key)
                        {
                            bucket->Values[index] = value;
                            return true;
                        }
                        hash_mask &= ~(1 << index);
                    }
                    while (empty_mask != 0)
                    {
                        int index = BitOperations.TrailingZeroCount(empty_mask);
                        if (first_tomb_idx < 0)
                        {
                            SwissBucket* bucket = &buckets[begin];
                            bucket->Keys[index] = key;
                            bucket->Values[index] = value;
                            ((byte*)control_bytes_ptr)[index] = target_hash;
                        } else
                        {
                            SwissBucket* bucket = &buckets[first_tomb_group];
                            bucket->Keys[first_tomb_idx] = key;
                            bucket->Values[first_tomb_idx] = value;
                            ((byte*)(control_bytes + first_tomb_group))[first_tomb_idx] = target_hash;
                            delete_count--;
                        }
                        return true;
                    }

                    begin = (begin + 1) & (GroupCount - 1);
                }
                if (first_tomb_idx >= 0)
                {
                    SwissBucket* bucket = &buckets[first_tomb_group];
                    bucket->Keys[first_tomb_idx] = key;
                    bucket->Values[first_tomb_idx] = value;
                    ((byte*)(control_bytes + first_tomb_group))[first_tomb_idx] = target_hash;
                    delete_count--;
                    return true;
                }
                value = 0;
                return false;
            }

            public static bool TryRemove(
                               byte* buffer,
                               AddressSource key,
                               out ulong value,
                               ref int delete_count
            )
            {
                Vector128<byte>* control_bytes = (Vector128<byte>*)buffer;
                SwissBucket* buckets = (SwissBucket*)(buffer + IdealCount);
                uint hash = SwissHashX64.Compute(key);
                uint begin = hash % GroupCount;
                byte target_hash = (byte)((hash >> GroupCountLog) & ~(1 << 7));

                for (int i = 0; i < GroupCount; i++)
                {
                    Vector128<byte> control_bytes_v128 = Sse2.LoadAlignedVector128((byte*)(control_bytes + begin));

                    Vector128<byte> target_hash_v128 = Vector128.Create(target_hash);
                    Vector128<byte> cmp_hash = Sse2.CompareEqual(control_bytes_v128, target_hash_v128);

                    int mask = Sse2.MoveMask(cmp_hash);

                    while (mask != 0)
                    {
                        int index = BitOperations.TrailingZeroCount(mask);
                        SwissBucket* bucket = &buckets[begin];
                        if (bucket->Keys[index] == key)
                        {
                            ((byte*)(control_bytes + begin))[index] = (byte)SwissControl.Deleted;
                            value = bucket->Values[index];
                            delete_count++;
                            return true;
                        }
                        mask &= ~(1 << index);
                    }

                    Vector128<byte> target_empty_v128 = Vector128.Create((byte)SwissControl.Empty);
                    Vector128<byte> cmp_empty = Sse2.CompareEqual(control_bytes_v128, target_empty_v128);
                    int empty_mask = Sse2.MoveMask(cmp_empty);
                    if (empty_mask != 0)
                    {
                        break;
                    }
                    begin = (begin + 1) & (GroupCount - 1);
                }
                value = 0;
                return false;
            }

        }

        internal readonly unsafe struct SwissReaderX86
        {

            private static readonly Vector128<byte> _empty_mask = Vector128.Create((byte)SwissControl.Empty);
            private static readonly Vector128<byte> _tomb_mask = Vector128.Create((byte)SwissControl.Deleted);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int CompareOccupiedMask(
                              Vector128<byte> control_bytes_v128
            )
            {
                Vector128<byte> cmp_eql = Sse2.CompareEqual(control_bytes_v128, _empty_mask);
                Vector128<byte> cmp_del = Sse2.CompareEqual(control_bytes_v128, _tomb_mask);
                int specialMask = Sse2.MoveMask(Sse2.Or(cmp_eql, cmp_del));
                return (ushort)~(ushort)specialMask;
            }

            public static bool TryGet(
                               byte* buffer,
                               AddressSource key,
                               out ulong value
            )
            {
                Vector128<byte>* control_bytes = (Vector128<byte>*)buffer;
                SwissBucket* buckets = (SwissBucket*)(buffer + IdealCount);

                uint hash = SwissHashX64.Compute(key);
                uint begin = hash % GroupCount;
                byte target_hash = (byte)((hash >> GroupCountLog) & ~(1 << 7));

                for (int i = 0; i < GroupCount; i++)
                {
                    Vector128<byte> control_bytes_v128 = Sse2.LoadAlignedVector128((byte*)(control_bytes + begin));
                    Vector128<byte> target_hash_v128 = Vector128.Create(target_hash);
                    Vector128<byte> cmp_hash = Sse2.CompareEqual(control_bytes_v128, target_hash_v128);
                    int mask = Sse2.MoveMask(cmp_hash);
                    while (mask != 0)
                    {
                        int index = BitOperations.TrailingZeroCount(mask);
                        SwissBucket* bucket = &buckets[begin];
                        if (bucket->Keys[index] == key)
                        {
                            value = bucket->Values[index];
                            return true;
                        }
                        mask &= ~(1 << index);
                    }

                    Vector128<byte> target_empty_v128 = Vector128.Create((byte)SwissControl.Empty);
                    Vector128<byte> cmp_empty = Sse2.CompareEqual(control_bytes_v128, target_empty_v128);
                    if (Sse2.MoveMask(cmp_empty) != 0)
                    {
                        break;
                    }

                    begin = (begin + 1) & (GroupCount - 1);
                }
                value = 0;
                return false;
            }

        }

    }
}
