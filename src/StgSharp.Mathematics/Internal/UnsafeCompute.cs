// -----------------------------------------------------------------------------
// file="UnsafeCompute"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics.Numeric;
using System.Runtime.CompilerServices;

namespace StgSharp.Mathematics.Internal
{
    public static class UnsafeCompute
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int CityHashSimplify(
                                 char* span,
                                 int length
        )
        {
            return NumericalModule.GlobalContext.city_hash_simplify(span, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int CityHashSimplify(
                                 char* span,
                                 int begin,
                                 int length
        )
        {
            return NumericalModule.GlobalContext.city_hash_simplify(span + begin, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int IndexOfCharPair(
                                 char* source,
                                 int pairValue,
                                 int length
        )
        {
            return NumericalModule.GlobalContext.index_pair(source, pairValue, length);
        }

    }
}
