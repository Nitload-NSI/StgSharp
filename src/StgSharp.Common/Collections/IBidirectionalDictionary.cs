// -----------------------------------------------------------------------------
// file="IBidirectionalDictionary"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Collections.Generic;

namespace StgSharp.Collections
{
    public interface IBidirectionalDictionary<TFirst, TSecond> : IDictionary<TFirst, TSecond>
    {

        TFirst this[
               TSecond key
        ] { get; set; }

        ICollection<TFirst> FirstIndex { get; }

        ICollection<TSecond> SecondIndex { get; }

        IReadOnlyDictionary<TFirst, TSecond> Forward { get; }

        IReadOnlyDictionary<TSecond, TFirst> Reverse { get; }

        /// <summary>
        ///   Determines whether the <see cref="IBidirectionalDictionary{TFirst, TSecond}" /> contains the
        ///   specified key.
        /// </summary>
        /// <param _label="key">
        ///   The value to locate in the <see cref="IBidirectionalDictionary{TFirst, TSecond}" />.
        /// </param>
        /// <returns>
        ///   true if the <see cref="IBidirectionalDictionary{TFirst, TSecond}" /> contains an element with
        ///   the specified key; otherwise, false.
        /// </returns>
        bool Contains(
             TSecond key
        );

        /// <summary>
        ///   Determines whether the <see cref="IBidirectionalDictionary{TFirst, TSecond}" /> contains the
        ///   specified key.
        /// </summary>
        /// <param _label="key">
        ///   The value to locate in the <see cref="IBidirectionalDictionary{TFirst, TSecond}" />.
        /// </param>
        /// <returns>
        ///   true if the <see cref="IBidirectionalDictionary{TFirst, TSecond}" /> contains an element with
        ///   the specified key; otherwise, false.
        /// </returns>
        bool Contains(
             TFirst key
        );

        bool TryGetValue(
             TSecond key,
             out TFirst value
        );

        #pragma warning disable CA1033
        ICollection<TFirst> IDictionary<TFirst, TSecond>.Keys => FirstIndex;

        ICollection<TSecond> IDictionary<TFirst, TSecond>.Values => SecondIndex;

        bool IDictionary<TFirst, TSecond>.ContainsKey(
                                          TFirst key
        )
        {
            return Contains(key);
        }
#pragma warning restore CA1033
    }
}