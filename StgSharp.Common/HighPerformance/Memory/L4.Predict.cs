//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="L4.Predict"
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.Memory
{
    public interface IPrefetchPredict
    {

        /// <summary>
        ///   Writes predicted cache line descriptors into <paramref name="node" />.
        /// </summary>
        /// <param name="node">
        ///   Prediction output buffer.
        /// </param>
        /// <param name="count">
        ///   When this method returns, contains the number of valid entries written to <paramref
        ///   name="node" />. The value must not exceed <paramref name="node" />.Length.
        /// </param>
        /// <returns>
        ///   <see langword="true" /> if any prediction data was produced; otherwise, <see langword="false"
        ///   />.
        /// </returns>
        bool Predict(Span<CacheLinePrediction> node, out int count);

        /// <summary>
        ///   Materializes the cache line identified by <paramref name="origin" /> into <paramref
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
        void Prefetch(nuint origin, ulong policy, Span<byte> cache);

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
        void WriteBack(nuint origin, ulong policy, ReadOnlySpan<byte> cache);

    }

    public struct CacheLinePrediction : IEquatable<CacheLinePrediction>
    {

        public override readonly bool Equals(object? obj)
        {
            return obj is CacheLinePrediction prediction &&
                   Address.Equals(prediction.Address) &&
                   MapPolicy == prediction.MapPolicy;
        }

        public readonly bool Equals(CacheLinePrediction other)
        {
            return
            Address == other.Address && MapPolicy == other.MapPolicy;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(Address, MapPolicy);
        }

        public static bool operator !=(CacheLinePrediction left, CacheLinePrediction right)
        {
            return !(left == right);
        }
        public static bool operator ==(CacheLinePrediction left, CacheLinePrediction right)
        {
            return left.Equals(right);
        }

#pragma warning disable CA1051
        public nuint Address;
        public ulong MapPolicy;

#pragma warning restore CA1051
    }
}
