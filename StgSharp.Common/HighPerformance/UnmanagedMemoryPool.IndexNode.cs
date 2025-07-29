//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="UnmanagedMemoryPool.IndexNode.cs"
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance
{
    public unsafe partial class UnmanagedMemoryPool
    {

        private const int
            LevelOf4096 = 4,
            LevelOf1024 = 3,
            LevelOf256 = 2,
            LevelOf64 = 1;

        internal readonly ref struct NextIndexNode
        {

            private readonly Span<M64> _layout;

            public readonly nuint this[int level]
            {
[MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => Position - _layout[level].Member<uint>(0);
[MethodImpl(MethodImplOptions.AggressiveInlining)]
                set => _layout[level].Member<uint>(0) = (uint)(Position - value);
            }

            private readonly ref nuint Position
            {
[MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref _layout[0].Member<nuint>(0);
            }

        }

        internal readonly ref struct PreviousIndexNode
        {

            private readonly Span<M64> _layout;

            public readonly nuint this[int level]
            {
[MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => Position - _layout[level].Member<uint>(1);
[MethodImpl(MethodImplOptions.AggressiveInlining)]
                set => _layout[level].Member<uint>(1) = (uint)(Position - value);
            }

            private readonly ref nuint Position
            {
[MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref _layout[0].Member<nuint>(0);
            }

        }

        [StructLayout(LayoutKind.Explicit)]
        internal readonly ref struct IndexNode
        {

            private static readonly uint[] _sizeArr = [16, 64, 256, 1024, 4096];
            [FieldOffset(0)] private readonly NextIndexNode nxt;
            [FieldOffset(0)] private readonly PreviousIndexNode pre;
            [FieldOffset(0)] private readonly Span<M64> _layout;

            public IndexNode(ref byte span)
            {
                _layout = MemoryMarshal.CreateSpan(ref Unsafe.As<byte, M64>(ref span), 8);
            }

            public readonly ref nuint Position => ref _layout[0].Member<nuint>(0);

            public readonly ref uint Size => ref _layout[5].Member<uint>(0);

            public readonly ref int Level => ref _layout[5].Member<int>(1);

            public readonly ref nuint Segment => ref _layout[6].Member<nuint>(0);

            public readonly uint BlockSize => LevelToSize(Level);

            public static uint LevelToSize(int level)
            {
                return level switch
                {
                    < 0 or > 4 => throw new ArgumentOutOfRangeException(
                        nameof(level), "Level must be from 1 to 4."),
                    _ => _sizeArr[level]
                };
            }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            public readonly ref readonly NextIndexNode Next
            {
[MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref nxt;
            }

            public readonly ref readonly PreviousIndexNode Previous
            {
[MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref pre;
            }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        }

    }
}
