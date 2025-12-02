//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="M64"
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
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace StgSharp.HighPerformance
{
    [StructLayout(LayoutKind.Explicit, Pack = 8)]
    public unsafe struct M64 : IRegisterPresentation
    {

        [FieldOffset(0)] private fixed byte buffer[8];
        [FieldOffset(0)] private ulong value;
        [FieldOffset(0)] private Vector64<float> vec;

        public M64()
        {
            Unsafe.SkipInit(out this);
            Unsafe.SkipInit(out vec);
            Unsafe.SkipInit(out value);
        }

        public ref T AsRef<T>() where T : unmanaged,INumber<T>
        {
            return ref Unsafe.As<byte, T>(ref buffer[0]);
        }

        public void BroadCastFrom<T>(T value) where T : unmanaged, INumber<T>
        {
            vec = Vector64.Create(value).AsSingle();
        }

        public override bool Equals(object obj)
        {
            return obj is M64 m64 && this == m64;
        }

        public override readonly int GetHashCode()
        {
            return value.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Member<T>(int index) where T : unmanaged, INumber<T>
        {
            return ref Unsafe.As<byte, T>(ref buffer[index * sizeof(T)]);
        }

        public static bool operator !=(M64 left, M64 right)
        {
            return !(left == right);
        }

        public static M64 operator <<(M64 value, int shift)
        {
            M64 result = new M64();
            result.value = value.value << shift;
            return result;
        }

        public static bool operator ==(M64 left, M64 right)
        {
            return left.value == right.value;
        }
        public static M64 operator >>(M64 value, int shift)
        {
            M64 result = new M64();
            result.value = value.value << shift;
            return result;
        }

    }
}
