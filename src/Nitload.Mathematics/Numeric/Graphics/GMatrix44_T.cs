// -----------------------------------------------------------------------------
// file="GMatrix44_T"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace Nitload.Mathematics.Numeric.Graphics
{
    [StructLayout(LayoutKind.Sequential, Pack = 16)]
    public struct GMatrix44<T> where T : unmanaged, INumber<T>
    {

        private Buffer _buffer;

        public Vec4<T> this[
                       int x
        ]
        {
            readonly get => _buffer[x];
            set => _buffer[x] = value;
        }

        public T this[
                 int x,
                 int y
        ]
        {
            readonly get => _buffer[x][y];
            set => _buffer[x][y] = value;
        }

        public static GMatrix44<T> Unit
        {
            get
            {
                GMatrix44<T> result = new();
                result[0] = new Vec4<T>(T.One, T.Zero, T.Zero, T.Zero);
                result[1] = new Vec4<T>(T.Zero, T.One, T.Zero, T.Zero);
                result[2] = new Vec4<T>(T.Zero, T.Zero, T.One, T.Zero);
                result[3] = new Vec4<T>(T.Zero, T.Zero, T.Zero, T.One);
                return result;
            }
        }

        public GMatrix44<T> Transpose
        {
            get
            {
                GMatrix44<T> transpose = new();

                // Native SK transpose is intentionally deferred until the module
                // startup path can guarantee that GlobalContext has been initialized.

                if (Unsafe.SizeOf<T>() == 4)
                {
                    Vector128<T> row0Source = _buffer[0].AsVector128Unsafe();
                    Vector128<T> row1Source = _buffer[1].AsVector128Unsafe();
                    Vector128<T> row2Source = _buffer[2].AsVector128Unsafe();
                    Vector128<T> row3Source = _buffer[3].AsVector128Unsafe();
                    Vector128<int> row0 = Unsafe.As<Vector128<T>, Vector128<int>>(ref row0Source);
                    Vector128<int> row1 = Unsafe.As<Vector128<T>, Vector128<int>>(ref row1Source);
                    Vector128<int> row2 = Unsafe.As<Vector128<T>, Vector128<int>>(ref row2Source);
                    Vector128<int> row3 = Unsafe.As<Vector128<T>, Vector128<int>>(ref row3Source);

                    Vector128<int> pairMask = Vector128.Create(-1, 0, -1, 0);
                    Vector128<int> firstPairMask = Vector128.Create(-1, -1, 0, 0);
                    Vector128<int> lowShuffle = Vector128.Create(0, 0, 1, 1);
                    Vector128<int> highShuffle = Vector128.Create(2, 2, 3, 3);
                    Vector128<int> secondLaneShuffle = Vector128.Create(2, 3, 2, 3);

                    Vector128<int> pair01Low = Vector128.ConditionalSelect(
                        pairMask,
                        Vector128.Shuffle(row0, lowShuffle),
                        Vector128.Shuffle(row1, lowShuffle));
                    Vector128<int> pair23Low = Vector128.ConditionalSelect(
                        pairMask,
                        Vector128.Shuffle(row2, lowShuffle),
                        Vector128.Shuffle(row3, lowShuffle));
                    Vector128<int> pair01High = Vector128.ConditionalSelect(
                        pairMask,
                        Vector128.Shuffle(row0, highShuffle),
                        Vector128.Shuffle(row1, highShuffle));
                    Vector128<int> pair23High = Vector128.ConditionalSelect(
                        pairMask,
                        Vector128.Shuffle(row2, highShuffle),
                        Vector128.Shuffle(row3, highShuffle));

                    Vector128<int> result0 = Vector128.ConditionalSelect(
                        firstPairMask, pair01Low, pair23Low);
                    Vector128<int> result1 = Vector128.ConditionalSelect(
                        firstPairMask,
                        Vector128.Shuffle(pair01Low, secondLaneShuffle),
                        Vector128.Shuffle(pair23Low, secondLaneShuffle));
                    Vector128<int> result2 = Vector128.ConditionalSelect(
                        firstPairMask, pair01High, pair23High);
                    Vector128<int> result3 = Vector128.ConditionalSelect(
                        firstPairMask,
                        Vector128.Shuffle(pair01High, secondLaneShuffle),
                        Vector128.Shuffle(pair23High, secondLaneShuffle));
                    transpose._buffer[0] = Vec4.FromVector128Unsafe(
                        Unsafe.As<Vector128<int>, Vector128<T>>(ref result0));
                    transpose._buffer[1] = Vec4.FromVector128Unsafe(
                        Unsafe.As<Vector128<int>, Vector128<T>>(ref result1));
                    transpose._buffer[2] = Vec4.FromVector128Unsafe(
                        Unsafe.As<Vector128<int>, Vector128<T>>(ref result2));
                    transpose._buffer[3] = Vec4.FromVector128Unsafe(
                        Unsafe.As<Vector128<int>, Vector128<T>>(ref result3));
                    return transpose;
                }

                if (Unsafe.SizeOf<T>() == 8)
                {
                    Vector256<T> row0Source = _buffer[0].AsVector256Unsafe();
                    Vector256<T> row1Source = _buffer[1].AsVector256Unsafe();
                    Vector256<T> row2Source = _buffer[2].AsVector256Unsafe();
                    Vector256<T> row3Source = _buffer[3].AsVector256Unsafe();
                    Vector256<long> row0 = Unsafe.As<Vector256<T>, Vector256<long>>(ref row0Source);
                    Vector256<long> row1 = Unsafe.As<Vector256<T>, Vector256<long>>(ref row1Source);
                    Vector256<long> row2 = Unsafe.As<Vector256<T>, Vector256<long>>(ref row2Source);
                    Vector256<long> row3 = Unsafe.As<Vector256<T>, Vector256<long>>(ref row3Source);

                    Vector256<long> pairMask = Vector256.Create(-1L, 0L, -1L, 0L);
                    Vector256<long> firstPairMask = Vector256.Create(-1L, -1L, 0L, 0L);
                    Vector256<long> firstShuffle = Vector256.Create(0L, 0L, 1L, 1L);
                    Vector256<long> secondShuffle = Vector256.Create(2L, 2L, 3L, 3L);
                    Vector256<long> pair01First = Vector256.ConditionalSelect(
                        pairMask,
                        Vector256.Shuffle(row0, firstShuffle),
                        Vector256.Shuffle(row1, firstShuffle));
                    Vector256<long> pair23First = Vector256.ConditionalSelect(
                        pairMask,
                        Vector256.Shuffle(row2, firstShuffle),
                        Vector256.Shuffle(row3, firstShuffle));
                    Vector256<long> pair01Second = Vector256.ConditionalSelect(
                        pairMask,
                        Vector256.Shuffle(row0, secondShuffle),
                        Vector256.Shuffle(row1, secondShuffle));
                    Vector256<long> pair23Second = Vector256.ConditionalSelect(
                        pairMask,
                        Vector256.Shuffle(row2, secondShuffle),
                        Vector256.Shuffle(row3, secondShuffle));
                    Vector256<long> result0 = Vector256.ConditionalSelect(
                        firstPairMask, pair01First, pair23First);
                    Vector256<long> result1 = Vector256.ConditionalSelect(
                        firstPairMask,
                        Vector256.Shuffle(pair01First, secondShuffle),
                        Vector256.Shuffle(pair23First, secondShuffle));
                    Vector256<long> result2 = Vector256.ConditionalSelect(
                        firstPairMask, pair01Second, pair23Second);
                    Vector256<long> result3 = Vector256.ConditionalSelect(
                        firstPairMask,
                        Vector256.Shuffle(pair01Second, secondShuffle),
                        Vector256.Shuffle(pair23Second, secondShuffle));
                    transpose._buffer[0] = Vec4.FromVector256Unsafe(
                        Unsafe.As<Vector256<long>, Vector256<T>>(ref result0));
                    transpose._buffer[1] = Vec4.FromVector256Unsafe(
                        Unsafe.As<Vector256<long>, Vector256<T>>(ref result1));
                    transpose._buffer[2] = Vec4.FromVector256Unsafe(
                        Unsafe.As<Vector256<long>, Vector256<T>>(ref result2));
                    transpose._buffer[3] = Vec4.FromVector256Unsafe(
                        Unsafe.As<Vector256<long>, Vector256<T>>(ref result3));
                    return transpose;
                }

                throw GraphicVector.ThrowUnsupportedTypeException<T>();
            }
        }

        [InlineArray(4)]
        private struct Buffer
        {

            public Vec4<T> _column;

        }

    }
}
