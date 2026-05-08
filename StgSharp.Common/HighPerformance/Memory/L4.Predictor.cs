//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="L4.Predictor"
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.Memory
{
    [Flags]
    public enum PredictResult
    {

        Success = 1,
        Exhausted = 2,

    }

    public interface IL4Predict
    {

        PredictResult Predict(
                      Span<CacheLineDescription> node,
                      out int count
        );

        /// <summary>
        ///   Materializes the cache line identified by <paramref name="origin" /> into <paramref ///  
        ///   name="cache" />.
        /// </summary>
        /// <param name="origin">
        ///   Source address associated with the cache line.
        /// </param>
        /// <param name="policy">
        ///   Mapping policy associated with the cache line.
        /// </param>
        /// <param name="cache">
        ///   Destination cache line buffer.
        /// </param>
        void Prefetch(
             nuint origin,
             uint policy,
             Span<byte> cache
        );

        /// <summary>
        ///   Writes the cache line in <paramref name="cache" /> back to <paramref name="origin" />.
        /// </summary>
        /// <param name="origin">
        ///   Source address associated with the cache line.
        /// </param>
        /// <param name="policy">
        ///   Mapping policy associated with the cache line.
        /// </param>
        /// <param name="cache">
        ///   Source cache line buffer.
        /// </param>
        void WriteBack(
             nuint origin,
             uint policy,
             ReadOnlySpan<byte> cache
        );

    }

    [StructLayout(LayoutKind.Explicit)]
    public struct CacheLineDescription : IEquatable<CacheLineDescription>
    {

        public override readonly bool Equals(
                                      object? obj
        )
        {
            return (obj is CacheLineDescription prediction) &&
                   Address.Equals(prediction.Address) &&
                   (MapPolicy == prediction.MapPolicy);
        }

        public readonly bool Equals(
                             CacheLineDescription other
        )
        {
            return
            (Address == other.Address) && (MapPolicy == other.MapPolicy);
        }

        public override readonly int GetHashCode()
        {
            return (int)ComposePredictionKey64(Address, MapPolicy);
        }

        public readonly nuint GetHashCodeLong()
        {
            return ComposePredictionKey64(Address, MapPolicy);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint ComposePredictionKey64(
                             nuint address,
                             ushort mapPolicy
        )
        {
            nuint x = address;
            nuint y = mapPolicy;
            unchecked
            {
                nuint mixed = x ^ (y * ((nuint)0x9e3779b97f4a7c15UL));
                mixed ^= mixed >> 30;
                mixed *= (nuint)0xbf58476d1ce4e5b9UL;
                mixed ^= mixed >> 27;
                mixed *= (nuint)0x94d049bb133111ebUL;
                mixed ^= mixed >> 31;
                return mixed;
            }
        }

        public static bool operator !=(
                                    CacheLineDescription left,
                                    CacheLineDescription right
        )
        {
            return !(left == right);
        }
        public static bool operator ==(
                                    CacheLineDescription left,
                                    CacheLineDescription right
        )
        {
            return left.Equals(right);
        }

#pragma warning disable CA1051
        [FieldOffset(0)]public nuint Address;
        [FieldOffset(8)]public uint Size;
        [FieldOffset(12)]public ushort MapPolicy;
        [FieldOffset(14)]internal short Predictor;
#pragma warning restore CA1051
    }
}
