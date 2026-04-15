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

        /*
         * Planned optimization contract types:
         * 1. internal interface IL4PredictOptimizationContract
         *    - Trusted extension point queried once during predictor registration.
         *    - Produces normalized compatibility tokens for a given map policy.
         * 2. readonly struct L4PredictCompatibilityToken
         *    - Hot-path comparable descriptor cached by L4.
         *    - Should at least carry ResidentFamily, FlushFamily and AccessMode.
         * 3. enum L4PredictAccessMode
         *    - Distinguishes read-only sources from write-back or write-through targets.
         *
         * L4 should only use these cached tokens in the prefetch path. Predictors without the
         * internal contract stay on the conservative path: write back the old owner, then
         * rematerialize the cache line for the new predictor.
         */

        /// <summary>
        ///   Determines whether a cache line materialized by <paramref name="predict" /> under
        ///   <paramref name="policy" /> can be adopted by the current predictor without an
        ///   intermediate rematerialization.
        /// </summary>
        /// <remarks>
        ///   This is intentionally stronger than comparing raw policy bits for equality. A true
        ///   result means the resident bytes can continue to be interpreted safely after ownership
        ///   is transferred to the current predictor.
        /// </remarks>
        bool IsSamePolicy(
             uint policy,
             IL4Predict predict
        );

        PredictResult Predict(
                      Span<CacheLinePrediction> node,
                      out int count
        );

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

    public struct CacheLinePrediction : IEquatable<CacheLinePrediction>
    {

        public override readonly bool Equals(
                                      object? obj
        )
        {
            return (obj is CacheLinePrediction prediction) &&
                   Address.Equals(prediction.Address) &&
                   (MapPolicy == prediction.MapPolicy);
        }

        public readonly bool Equals(
                             CacheLinePrediction other
        )
        {
            return
            (Address == other.Address) && (MapPolicy == other.MapPolicy);
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(Address, MapPolicy);
        }

        public static bool operator !=(
                                    CacheLinePrediction left,
                                    CacheLinePrediction right
        )
        {
            return !(left == right);
        }
        public static bool operator ==(
                                    CacheLinePrediction left,
                                    CacheLinePrediction right
        )
        {
            return left.Equals(right);
        }

#pragma warning disable CA1051
        public nuint Address;
        public uint MapPolicy;
        internal uint Predictor;
#pragma warning restore CA1051
    }
}
